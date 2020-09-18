﻿#region 文件描述
/**************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-06-11 创建
**************************************************************************/
#endregion

#region 命名空间
using Dt.Cells.Data;
using Dt.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
#endregion

namespace Dt.Base.Report
{
    internal class RptRender
    {
        readonly RptInfo _info;
        Worksheet _ws;

        public RptRender(RptInfo p_info)
        {
            _info = p_info;
        }

        /// <summary>
        /// 渲染输出
        /// </summary>
        public void Render()
        {
            int start;
            RptRootInst inst = _info.Inst;
            List<double> rows = new List<double>();

            // 填充空行空列，统计所有行高列宽
            foreach (PageDefine page in inst.Rows)
            {
                // 页面之间的缝隙，为避免打印时分页边框不正常！
                rows.Add(RptRootInst.PageGap);

                // 记录页面开始行索引
                start = rows.Count;
                page.Offset = start;

                // 页眉行
                if (inst.HeaderHeight > 0)
                    rows.Add(inst.HeaderHeight);

                // 内容行
                double total = 0;
                foreach (double height in page.Size)
                {
                    rows.Add(height);
                    total += height;
                }

                // 填充空行
                if (total < inst.BodyHeight)
                    rows.Add(inst.BodyHeight - total);

                // 页脚行
                if (inst.FooterHeight > 0)
                    rows.Add(inst.FooterHeight);
                page.Total = rows.Count - start;
            }

            List<double> cols = new List<double>();
            foreach (PageDefine page in inst.Cols)
            {
                // 页面之间的左侧间隔缝隙
                cols.Add(RptRootInst.PageGap);

                // 记录页面开始列索引
                start = cols.Count;
                page.Offset = start;

                double total = 0.0;
                foreach (double width in page.Size)
                {
                    cols.Add(width);
                    total += width;
                }

                // 填充空列
                if (total < inst.BodyWidth)
                    cols.Add(inst.BodyWidth - total);
                page.Total = cols.Count - start;
            }

            // 创建Worksheet
            _ws = new Worksheet(rows.Count, cols.Count);
            // 不显示选择区黑框和触摸时的两圈
            _ws.SelectionBorderColor = Colors.Transparent;
            _ws.TouchSelectionGripperBackgroundColor = Colors.Transparent;
            // 单元格不可编辑，图表可拖动
            _ws.LockCell = true;
            // Wp始终不可编辑
            if (AtSys.IsPhoneUI)
                _ws.Protect = true;
            _info.Sheet = _ws;

            // 初始化行高列宽
            for (int i = 0; i < rows.Count; i++)
            {
                _ws.Rows[i].Height = rows[i];
            }
            for (int i = 0; i < cols.Count; i++)
            {
                _ws.Columns[i].Width = cols[i];
            }

            // 输出所有项
            foreach (RptPage page in _info.Inst.Pages)
            {
                PageDefine define = page.Rows;
                int startRow = define.Start;
                int offsetRow = define.Offset;
                int rowTotal = define.Total;
                int offsetBody = offsetRow + (page.HeaderItems.Count > 0 ? 1 : 0);
                define = page.Cols;
                int startCol = define.Start;
                int offsetCol = define.Offset;
                page.UpdatePageNum();

                // 页眉
                if (page.HeaderItems.Count > 0)
                {
                    foreach (RptTextInst item in page.HeaderItems)
                    {
                        // 不渲染超出的列
                        int tempCol = offsetCol + item.Item.Col;
                        if (tempCol < cols.Count)
                            RenderText(item, offsetRow, tempCol);
                    }
                }

                // 内容
                foreach (RptOutputInst item in page.Items)
                {
                    RptChartInst chart;
                    int row = item.Region.Row - startRow + offsetBody;
                    int col = item.Region.Col - startCol + offsetCol;
                    RptTextInst txt = item as RptTextInst;
                    if (txt != null)
                    {
                        Cells.Data.Cell tmpCell;
                        CellRange range;
                        RptText text = txt.Item as RptText;
                        var dataRow = (txt.Item as RptText).Data;
                        var randerCell = RenderText(txt, row, col);

                        if (row > startRow && dataRow.Bool("hidetopdup"))
                        {
                            tmpCell = _ws[row - 1, col];
                            if (tmpCell.Tag != null
                                && txt.Item.Data.Bool("hidetopdup")
                                && tmpCell.Text == randerCell.Text)
                            {
                                range = _ws.GetSpanCell(row - 1, col);
                                if (range != null)
                                    tmpCell = _ws[range.Row, range.Column];
                                if (tmpCell.ColumnSpan == randerCell.ColumnSpan)
                                    tmpCell.RowSpan += randerCell.RowSpan;
                            }
                        }

                        if (col > startCol && dataRow.Bool("hideleftdup"))
                        {
                            tmpCell = _ws[row, col - 1];
                            if (tmpCell.Tag != null
                               && txt.Item.Data.Bool("hidetopdup")
                               && tmpCell.Text == randerCell.Text)
                            {
                                range = _ws.GetSpanCell(row, col - 1);
                                if (range != null)
                                    tmpCell = _ws[range.Row, range.Column];
                                if (tmpCell.RowSpan == randerCell.RowSpan)
                                    tmpCell.ColumnSpan += randerCell.ColumnSpan;
                            }
                        }
                    }
                    else if ((chart = (item as RptChartInst)) != null)
                    {
                        RenderChart(chart, row, col);
                    }
                }

                // 页脚
                if (page.FooterItems.Count > 0)
                {
                    foreach (RptTextInst item in page.FooterItems)
                    {
                        // 不渲染超出的列
                        int tempCol = offsetCol + item.Item.Col;
                        if (tempCol < cols.Count)
                            RenderText(item, offsetRow + rowTotal - 1, tempCol);
                    }
                }
            }
        }

        /// <summary>
        /// 输出文本内容
        /// </summary>
        /// <param name="p_txt"></param>
        /// <param name="p_row"></param>
        /// <param name="p_col"></param>
        Cells.Data.Cell RenderText(RptTextInst p_txt, int p_row, int p_col)
        {
            var cell = _ws[p_row, p_col];
            var item = p_txt.Item as RptText;
            cell.ColumnSpan = item.ColSpan;
            cell.RowSpan = item.RowSpan;
            cell.Tag = p_txt;
            cell.Value = p_txt.GetText();
            AtKit.RunSync(() => item.ApplyStyle(cell));
            return cell;
        }

        /// <summary>
        /// 输出图表
        /// </summary>
        /// <param name="p_inst"></param>
        /// <param name="p_row"></param>
        /// <param name="p_col"></param>
        void RenderChart(RptChartInst p_inst, int p_row, int p_col)
        {
            RptChart ct = p_inst.Item as RptChart;
            RptData data = null;

            if (!ValidChartFilds(ct) || !_info.DataSet.TryGetValue(ct.Tbl, out data))
                return;

            Task<RenderTargetBitmap> task = null;
            AtKit.RunSync(() =>
            {
                Dt.Base.Chart chart = ct.CreateChart();
                chart.LoadData(data.Data, ct.FieldSeries, ct.FieldX, ct.FieldY, ct.FieldZ);
                task = chart.GetSnapshot();
            });
            task.Wait();
            Rect rc = _ws.GetRangeLocation(new CellRange(p_row, p_col, ct.RowSpan, ct.ColSpan));
            Picture pic = _ws.AddPicture(_ws.Pictures.Count.ToString(), task.Result, rc.Left, rc.Top, rc.Width, rc.Height);
            // 锁定图表，禁止拖动缩放
            pic.Locked = true;
        }

        /// <summary>
        /// 判断数据表字段是否完整
        /// </summary>
        /// <param name="p_chart"></param>
        /// <returns></returns>
        bool ValidChartFilds(RptChart p_chart)
        {
            if (string.IsNullOrEmpty(p_chart.Tbl))
            {
                AtKit.Msg("数据源不可为空。");
                return false;
            }
            string type = p_chart.Data.Str("type");
            if (type == "Gantt")
            {
                if (string.IsNullOrEmpty(p_chart.FieldZ)
                    || string.IsNullOrEmpty(p_chart.FieldX)
                    || string.IsNullOrEmpty(p_chart.FieldY))
                {
                    AtKit.Msg("任务字段、起始时间字段及终止时间字段均不可为空，图表生成失败。");
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(p_chart.FieldX)
                    || string.IsNullOrEmpty(p_chart.FieldY))
                {
                    AtKit.Msg("分类字段和值字段不可为空，图表生成失败。");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 输出图片
        /// </summary>
        /// <param name="p_img"></param>
        /// <param name="p_row"></param>
        /// <param name="p_col"></param>
        void RenderImage(RptTextInst p_img, int p_row, int p_col)
        {
            RptText item = p_img.Item as RptText;
            AtKit.RunSync(() =>
            {
                Rect rc = _ws.GetRangeLocation(new CellRange(p_row, p_col, item.RowSpan, item.ColSpan));
                _ws.AddPicture(_ws.Pictures.Count.ToString(), new Uri(item.Val), rc.Left, rc.Top, rc.Width, rc.Height);
            });
        }
    }
}
