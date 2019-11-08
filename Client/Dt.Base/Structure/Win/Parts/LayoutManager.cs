#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-03-03 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
using Dt.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endregion

namespace Dt.Base.Docking
{
    /// <summary>
    /// 布局管理
    /// 1. 宽度足够时，加载历史布局或默认布局
    /// 2. 宽度小时，自动隐藏两侧，隐藏优先级：先右再左，Center始终显示
    /// </summary>
    internal class LayoutManager
    {
        #region 成员变量
        const double _centerWidth = 300;
        Win _owner;
        string _default;
        readonly Dictionary<string, Tab> _tabs;
        List<double> _colsWidth;
        // 自动隐藏位置，-1表示不自动隐藏
        int _fitCols = -1;
        #endregion

        #region 构造方法
        public LayoutManager(Win p_owner)
        {
            _owner = p_owner;
            _tabs = new Dictionary<string, Tab>();
        }
        #endregion

        #region 外部方法
        /// <summary>
        /// 初始化布局环境
        /// 1. 记录默认布局
        /// 2. 加载状态库中的历史布局
        /// 3. 无历史布局则加载默认布局
        /// </summary>
        public void Init()
        {
            if (DesignMode.DesignModeEnabled)
            {
                // 设计模式下可视处理
                WinItem dockItem;
                WinCenter center;
                for (int i = 0; i < _owner.Items.Count; i++)
                {
                    if ((dockItem = _owner.Items[i] as WinItem) != null)
                        _owner.DockPanel.Children.Insert(i, dockItem);
                    else if ((center = _owner.Items[i] as WinCenter) != null)
                        _owner.DockPanel.CenterItem = center;
                }
                return;
            }

            // 记录默认布局
            SaveDefaultXml();
            if (_owner.AutoSaveLayout)
            {
                DockLayout cookie = AtLocal.GetFirst<DockLayout>($"select * from DockLayout where BaseUri=\"{_owner.BaseUri.AbsolutePath}\"");
                if (cookie != null)
                {
                    // 加载历史布局
                    if (ApplyLayout(cookie.Layout))
                    {
                        _owner.LayoutButtonVisible = Visibility.Visible;
                    }
                    else
                    {
                        // 历史布局加载失败，重载默认布局
                        ApplyLayout(_default);
                        _owner.LayoutButtonVisible = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// 恢复默认布局
        /// 1. 删除状态库的历史布局
        /// 2. 加载最初布局
        /// </summary>
        public void LoadDefaultLayout()
        {
            AtLocal.Execute($"delete from DockLayout where BaseUri=\"{_owner.BaseUri.AbsolutePath}\"");
            ApplyLayout(_default);
            _owner.LayoutButtonVisible = Visibility.Collapsed;
        }

        /// <summary>
        /// 保存当前布局
        /// </summary>
        public void SaveCurrentLayout()
        {
            // 宽度小时不保存
            if (!_owner.AutoSaveLayout || _fitCols != -1)
                return;

            AtKit.RunAsync(() =>
            {
                DockLayout cookie = new DockLayout();
                cookie.BaseUri = _owner.BaseUri.AbsolutePath;
                cookie.Layout = WriteXml();
                AtLocal.Save(cookie);
                _owner.LayoutButtonVisible = Visibility.Visible;
            });
        }

        /// <summary>
        /// Win宽度变化时自动调整
        /// </summary>
        /// <param name="p_width"></param>
        public void OnWidthChanged(double p_width)
        {
            double width = 0;
            int index = -1;
            for (int i = 0; i < _colsWidth.Count; i++)
            {
                width += _colsWidth[i];
                if (width > p_width)
                {
                    index = i;
                    break;
                }
            }
            if (_fitCols == index)
                return;

            _fitCols = index;
            if (_fitCols == -1)
            {
                // 宽度足够，加载历史布局或默认布局
                DockLayout cookie;
                if (_owner.AutoSaveLayout
                    && (cookie = AtLocal.GetFirst<DockLayout>($"select * from DockLayout where BaseUri=\"{_owner.BaseUri.AbsolutePath}\"")) != null
                    && ApplyLayout(cookie.Layout))
                {
                    _owner.LayoutButtonVisible = Visibility.Visible;
                }
                else
                {
                    ApplyLayout(_default);
                    _owner.LayoutButtonVisible = Visibility.Collapsed;
                }
            }
            else
            {
                // 自动隐藏两侧
                ApplyAutoHide();
                _owner.LayoutButtonVisible = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 深度清除所有子项
        /// </summary>
        /// <param name="p_items"></param>
        public void ClearItems(ItemsControl p_items)
        {
            // 不可使用Items.Clear
            while (p_items.Items.Count > 0)
            {
                // 先移除当前项，再清除子项，不可颠倒！
                object item = p_items.Items[0];
                p_items.Items.RemoveAt(0);
                ItemsControl child = item as ItemsControl;
                if (child != null)
                    ClearItems(child);
            }
        }
        #endregion

        #region 应用xml布局
        /// <summary>
        /// 将xml布局描述应用到当前布局，若应用过程中若异常从状态库中删除xml，已调优
        /// </summary>
        /// <param name="p_content"></param>
        /// <returns>应用布局是否成功</returns>
        bool ApplyLayout(string p_content)
        {
            if (string.IsNullOrEmpty(p_content))
                return false;

            bool succ = true;
            _owner.IsReseting = true;
            ClearAllItems();

            try
            {
                using (StringReader reader = new StringReader(p_content))
                {
                    XDocument doc = XDocument.Load(reader);
                    if (p_content != _default)
                    {
                        // 校验标签
                        int count = 0;
                        foreach (XElement item in doc.Root.Descendants("Tab"))
                        {
                            count++;
                            XAttribute attr = item.Attribute("Title");
                            if (attr == null
                                || string.IsNullOrEmpty(attr.Value)
                                || !_tabs.ContainsKey(attr.Value))
                            {
                                succ = false;
                                break;
                            }
                        }
                        if (!succ || count != _tabs.Count)
                            throw new Exception("加载时确保标签个数和标题都能对上，否则删除历史布局！");
                    }

                    // 停靠项
                    int index = 0;
                    XElement elem = doc.Root.Element("Win");
                    foreach (XElement item in elem.Elements())
                    {
                        WinItem dockItem = CreateDockItem(item);
                        _owner.Items.Add(dockItem);
                        _owner.DockPanel.Children.Insert(index++, dockItem);
                    }

                    // 中部项
                    elem = doc.Root.Element("Center");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            if (item.Name == "Tabs")
                                _owner.CenterItem.Items.Add(CreateSect(item));
                            else if (item.Name == "WinItem")
                                _owner.CenterItem.Items.Add(CreateDockItem(item));
                        }
                    }

                    // 左侧隐藏项
                    elem = doc.Root.Element("Left");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.LeftAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 右侧隐藏项
                    elem = doc.Root.Element("Right");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.RightAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 上侧隐藏项
                    elem = doc.Root.Element("Top");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.TopAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 下侧隐藏项
                    elem = doc.Root.Element("Bottom");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.BottomAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 浮动项
                    elem = doc.Root.Element("Float");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            OpenInWindow(CreateDockItem(item));
                        }
                    }
                }
            }
            catch
            {
                succ = false;
                AtLocal.Execute($"delete from DockLayout where BaseUri=\"{_owner.BaseUri.AbsolutePath}\"");
            }
            finally
            {
                _owner.IsReseting = false;
            }
            return succ;
        }

        /// <summary>
        /// 为适应宽度自动隐藏两侧窗口
        /// </summary>
        void ApplyAutoHide()
        {
            _owner.IsReseting = true;
            ClearAllItems();

            try
            {
                using (StringReader reader = new StringReader(_default))
                {
                    XDocument doc = XDocument.Load(reader);

                    // 停靠项，xml中按左右上下顺序
                    int start = _fitCols - 1;
                    int end = _colsWidth.Count - 1;
                    int index = 0;
                    XElement elem = doc.Root.Element("Win");
                    var pnl = _owner.DockPanel.Children;
                    foreach (XElement item in elem.Elements())
                    {
                        WinItem di = CreateDockItem(item);
                        if (index >= start && index < end)
                        {
                            // 范围之内的放入两侧自动隐藏
                            MoveToAutoHide(di, di.DockState);
                        }
                        else
                        {
                            _owner.Items.Add(di);
                            pnl.Insert(pnl.Count - 1, di);
                        }
                        index++;
                    }

                    // 中部项
                    elem = doc.Root.Element("Center");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            if (item.Name == "Tabs")
                                _owner.CenterItem.Items.Add(CreateSect(item));
                            else if (item.Name == "WinItem")
                                _owner.CenterItem.Items.Add(CreateDockItem(item));
                        }
                    }

                    // 左侧隐藏项
                    elem = doc.Root.Element("Left");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.LeftAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 右侧隐藏项
                    elem = doc.Root.Element("Right");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.RightAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 上侧隐藏项
                    elem = doc.Root.Element("Top");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.TopAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 下侧隐藏项
                    elem = doc.Root.Element("Bottom");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            _owner.BottomAutoHide.Unpin(CreateSectItem(item));
                        }
                    }

                    // 浮动项
                    elem = doc.Root.Element("Float");
                    if (elem != null)
                    {
                        foreach (XElement item in elem.Elements())
                        {
                            OpenInWindow(CreateDockItem(item));
                        }
                    }
                }
            }
            finally
            {
                _owner.IsReseting = false;
            }
        }

        WinItem CreateDockItem(XElement p_elem)
        {
            WinItem item = new WinItem();
            XAttribute attr = p_elem.Attribute("DockState");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.DockState = (WinItemState)Enum.Parse(typeof(WinItemState), attr.Value);

            attr = p_elem.Attribute("InitWidth");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.InitWidth = Convert.ToDouble(attr.Value);

            attr = p_elem.Attribute("InitHeight");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.InitHeight = Convert.ToDouble(attr.Value);

            attr = p_elem.Attribute("Orientation");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.Orientation = (Orientation)Enum.Parse(typeof(Orientation), attr.Value);

            attr = p_elem.Attribute("FloatLocation");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                Point pt = new Point();
                string[] loc = attr.Value.Split(',');
                if (loc.Length == 2)
                {
                    pt.X = Convert.ToDouble(loc[0]);
                    pt.Y = Convert.ToDouble(loc[1]);
                    item.FloatLocation = pt;
                }
            }

            attr = p_elem.Attribute("FloatPos");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.FloatPos = (FloatPosition)Enum.Parse(typeof(FloatPosition), attr.Value);

            attr = p_elem.Attribute("FloatSize");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                Size size = new Size();
                string[] loc = attr.Value.Split(',');
                if (loc.Length == 2)
                {
                    size.Width = Convert.ToDouble(loc[0]);
                    size.Height = Convert.ToDouble(loc[1]);
                    item.FloatSize = size;
                }
            }

            foreach (XElement elem in p_elem.Elements())
            {
                if (elem.Name == "Tabs")
                    item.Items.Add(CreateSect(elem));
                else if (elem.Name == "WinItem")
                    item.Items.Add(CreateDockItem(elem));
            }

            attr = p_elem.Attribute("IsInCenter");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.IsInCenter = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("IsInWindow");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                item.IsInWindow = Convert.ToBoolean(attr.Value);
            return item;
        }

        Tabs CreateSect(XElement p_elem)
        {
            Tabs sect = new Tabs();
            XAttribute attr = p_elem.Attribute("IsOutlookStyle");
            if (attr != null)
                sect.IsOutlookStyle = true;

            attr = p_elem.Attribute("InitWidth");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                sect.InitWidth = Convert.ToDouble(attr.Value);

            attr = p_elem.Attribute("InitHeight");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                sect.InitHeight = Convert.ToDouble(attr.Value);

            attr = p_elem.Attribute("Padding");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                string[] padding = attr.Value.Split(',');
                if (padding.Length == 4)
                {
                    double left = Convert.ToDouble(padding[0]);
                    double top = Convert.ToDouble(padding[1]);
                    double right = Convert.ToDouble(padding[2]);
                    double bottom = Convert.ToDouble(padding[3]);
                    sect.Padding = new Thickness(left, top, right, bottom);
                }
            }

            foreach (XElement elem in p_elem.Elements("Tab"))
            {
                sect.Items.Add(CreateSectItem(elem));
            }

            // 索引需后设置
            attr = p_elem.Attribute("SelectedIndex");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                sect.SelectedIndex = Convert.ToInt32(attr.Value);
            return sect;
        }

        Tab CreateSectItem(XElement p_elem)
        {
            Tab tab;
            XAttribute attr = p_elem.Attribute("Title");
            if (attr == null
                || string.IsNullOrEmpty(attr.Value)
                || !_tabs.TryGetValue(attr.Value, out tab))
                return new Tab();

            tab.Title = attr.Value;
            attr = p_elem.Attribute("Name");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.Name = attr.Value;

            attr = p_elem.Attribute("IsPinned");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.IsPinned = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("CanDockInCenter");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.CanDockInCenter = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("CanDock");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.CanDock = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("CanFloat");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.CanFloat = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("CanUserPin");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.CanUserPin = Convert.ToBoolean(attr.Value);

            attr = p_elem.Attribute("PopHeight");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.PopHeight = Convert.ToDouble(attr.Value);

            attr = p_elem.Attribute("PopWidth");
            if (attr != null && !string.IsNullOrEmpty(attr.Value))
                tab.PopWidth = Convert.ToDouble(attr.Value);
            return tab;
        }
        #endregion

        #region 输出xml
        /// <summary>
        /// 将当前布局输出为xml
        /// </summary>
        /// <returns></returns>
        string WriteXml()
        {
            StringBuilder xml = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xml, AtKit.WriterSettings))
            {
                writer.WriteStartElement("Items");

                // 停靠项
                writer.WriteStartElement("Win");
                foreach (WinItem dockItem in _owner.Items.OfType<WinItem>())
                {
                    WriteDockItem(dockItem, writer);
                }
                writer.WriteEndElement();

                // 中部项
                if (_owner.CenterItem.Items.Count > 0)
                {
                    writer.WriteStartElement("Center");
                    WriteCenter(writer, _owner.CenterItem.Items);
                    writer.WriteEndElement();
                }

                // 左侧隐藏项
                if (_owner.LeftAutoHide.Items.Count > 0)
                {
                    writer.WriteStartElement("Left");
                    foreach (Tab sectItem in _owner.LeftAutoHide.Items.OfType<Tab>())
                    {
                        WriteSectItem(sectItem, writer);
                    }
                    writer.WriteEndElement();
                }

                // 右侧隐藏项
                if (_owner.RightAutoHide.Items.Count > 0)
                {
                    writer.WriteStartElement("Right");
                    foreach (Tab sectItem in _owner.RightAutoHide.Items.OfType<Tab>())
                    {
                        WriteSectItem(sectItem, writer);
                    }
                    writer.WriteEndElement();
                }

                // 上侧隐藏项
                if (_owner.TopAutoHide.Items.Count > 0)
                {
                    writer.WriteStartElement("Top");
                    foreach (Tab sectItem in _owner.TopAutoHide.Items.OfType<Tab>())
                    {
                        WriteSectItem(sectItem, writer);
                    }
                    writer.WriteEndElement();
                }

                // 下侧隐藏项
                if (_owner.BottomAutoHide.Items.Count > 0)
                {
                    writer.WriteStartElement("Bottom");
                    foreach (Tab sectItem in _owner.BottomAutoHide.Items.OfType<Tab>())
                    {
                        WriteSectItem(sectItem, writer);
                    }
                    writer.WriteEndElement();
                }

                // 浮动项
                writer.WriteStartElement("Float");
                foreach (WinItem dockItem in _owner.FloatItems)
                {
                    WriteDockItem(dockItem, writer);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();
            }
            return xml.ToString();
        }

        /// <summary>
        /// 保存初始布局，同步处理布局、提取Tab字典，已调优
        /// </summary>
        void SaveDefaultXml()
        {
            _owner.IsReseting = true;

            // 按类型提取各项
            List<WinCenter> centers = new List<WinCenter>();
            List<WinItem> lefts = new List<WinItem>();
            List<WinItem> rights = new List<WinItem>();
            List<WinItem> topBottom = new List<WinItem>();
            List<WinItem> floats = new List<WinItem>();
            List<Tab> leftHide = new List<Tab>();
            List<Tab> rightHide = new List<Tab>();
            List<Tab> topHide = new List<Tab>();
            List<Tab> bottomHide = new List<Tab>();
            while (_owner.Items.Count > 0)
            {
                WinCenter center;
                object obj = _owner.Items[0];
                WinItem di = obj as WinItem;
                if (di != null)
                {
                    ExtractItems(di);
                    if (di.DockState == WinItemState.Floating)
                    {
                        floats.Add(di);
                    }
                    else
                    {
                        // 在停靠中挑出自动隐藏项
                        foreach (Tab sectItem in GetHideItems(di))
                        {
                            if (di.DockState == WinItemState.DockedLeft)
                                leftHide.Add(sectItem);
                            else if (di.DockState == WinItemState.DockedRight)
                                rightHide.Add(sectItem);
                            else if (di.DockState == WinItemState.DockedTop)
                                topHide.Add(sectItem);
                            else if (di.DockState == WinItemState.DockedBottom)
                                bottomHide.Add(sectItem);
                        }

                        if (!IsRemoved(di))
                        {
                            RemoveUnused(di);

                            if (di.DockState == WinItemState.DockedLeft)
                                lefts.Add(di);
                            else if (di.DockState == WinItemState.DockedRight)
                                rights.Add(di);
                            else
                                topBottom.Add(di);
                        }
                    }
                }
                else if ((center = obj as WinCenter) != null)
                {
                    ExtractItems(center);
                    centers.Add(center);
                }
                _owner.Items.RemoveAt(0);
            }

            StringBuilder xml = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xml, AtKit.WriterSettings))
            {
                writer.WriteStartElement("Items");

                // 停靠项，按左右上下顺序输出
                writer.WriteStartElement("Win");
                int index = 0;
                _colsWidth = new List<double>();
                // 首先Center宽度
                _colsWidth.Add(centers.Count > 0 ? _centerWidth : 0);
                if (lefts.Count > 0)
                {
                    foreach (var di in lefts)
                    {
                        WriteDockItem(di, writer);
                        _owner.Items.Add(di);
                        _owner.DockPanel.Children.Insert(index++, di);
                        _colsWidth.Add(di.InitWidth);
                    }
                }
                if (rights.Count > 0)
                {
                    foreach (var di in rights)
                    {
                        WriteDockItem(di, writer);
                        _owner.Items.Add(di);
                        _owner.DockPanel.Children.Insert(index++, di);
                        _colsWidth.Add(di.InitWidth);
                    }
                }
                if (topBottom.Count > 0)
                {
                    foreach (var di in topBottom)
                    {
                        WriteDockItem(di, writer);
                        _owner.Items.Add(di);
                        _owner.DockPanel.Children.Insert(index++, di);
                    }
                }
                writer.WriteEndElement();

                // 中部项
                if (centers.Count > 0)
                {
                    writer.WriteStartElement("Center");
                    foreach (WinCenter center in centers)
                    {
                        WriteCenter(writer, center.Items);
                        // 挪到CenterItem，特殊处理！
                        while (center.Items.Count > 0)
                        {
                            object centerItem = center.Items[0];
                            center.Items.RemoveAt(0);
                            _owner.CenterItem.Items.Add(centerItem);
                        }
                    }
                    writer.WriteEndElement();
                }

                // 左侧隐藏项
                if (leftHide.Count > 0)
                {
                    writer.WriteStartElement("Left");
                    foreach (Tab sectItem in leftHide)
                    {
                        WriteSectItem(sectItem, writer);
                        _owner.LeftAutoHide.Unpin(sectItem);
                    }
                    writer.WriteEndElement();
                }

                // 右侧隐藏项
                if (rightHide.Count > 0)
                {
                    writer.WriteStartElement("Right");
                    foreach (Tab sectItem in rightHide)
                    {
                        WriteSectItem(sectItem, writer);
                        _owner.RightAutoHide.Unpin(sectItem);
                    }
                    writer.WriteEndElement();
                }

                // 上侧隐藏项
                if (topHide.Count > 0)
                {
                    writer.WriteStartElement("Top");
                    foreach (Tab sectItem in topHide)
                    {
                        WriteSectItem(sectItem, writer);
                        _owner.TopAutoHide.Unpin(sectItem);
                    }
                    writer.WriteEndElement();
                }

                // 下侧隐藏项
                if (bottomHide.Count > 0)
                {
                    writer.WriteStartElement("Bottom");
                    foreach (Tab sectItem in bottomHide)
                    {
                        WriteSectItem(sectItem, writer);
                        _owner.BottomAutoHide.Unpin(sectItem);
                    }
                    writer.WriteEndElement();
                }

                // 浮动项
                if (floats.Count > 0)
                {
                    writer.WriteStartElement("Float");
                    foreach (WinItem dockItem in floats)
                    {
                        WriteDockItem(dockItem, writer);
                        OpenInWindow(dockItem);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();
            }
            _default = xml.ToString();
            _owner.IsReseting = false;
        }

        void WriteCenter(XmlWriter p_writer, ItemCollection p_items)
        {
            foreach (object obj in p_items)
            {
                Tabs sect;
                WinItem dockItem;
                if ((sect = obj as Tabs) != null)
                {
                    WriteSect(sect, p_writer);
                }
                else if ((dockItem = obj as WinItem) != null)
                {
                    WriteDockItem(dockItem, p_writer);
                }
            }
        }

        void WriteDockItem(WinItem p_item, XmlWriter p_writer)
        {
            p_writer.WriteStartElement("WinItem");

            if (p_item.ReadLocalValue(WinItem.DockStateProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("DockState", p_item.DockState.ToString());

            if (!double.IsNaN(p_item.ActualWidth) && p_item.ActualWidth > 0)
                p_writer.WriteAttributeString("InitWidth", p_item.ActualWidth.ToString());
            else if (p_item.ReadLocalValue(WinItem.InitWidthProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("InitWidth", p_item.InitWidth.ToString());

            if (!double.IsNaN(p_item.ActualHeight) && p_item.ActualHeight > 0)
                p_writer.WriteAttributeString("InitHeight", p_item.ActualHeight.ToString());
            else if (p_item.ReadLocalValue(WinItem.InitHeightProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("InitHeight", p_item.InitHeight.ToString());

            if (p_item.ReadLocalValue(WinItem.OrientationProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("Orientation", p_item.Orientation.ToString());

            ToolWindow win = p_item.Parent as ToolWindow;
            if (win != null)
            {
                p_writer.WriteAttributeString("FloatLocation", string.Format("{0},{1}", Math.Ceiling(win.HorizontalOffset), Math.Ceiling(win.VerticalOffset)));
                p_writer.WriteAttributeString("FloatSize", string.Format("{0},{1}", Math.Ceiling(win.ActualWidth), Math.Ceiling(win.ActualHeight)));
            }
            else
            {
                if (p_item.ReadLocalValue(WinItem.FloatLocationProperty) != DependencyProperty.UnsetValue)
                    p_writer.WriteAttributeString("FloatLocation", string.Format("{0},{1}", p_item.FloatLocation.X, p_item.FloatLocation.Y));
                else if (p_item.ReadLocalValue(WinItem.FloatPosProperty) != DependencyProperty.UnsetValue)
                    p_writer.WriteAttributeString("FloatPos", p_item.FloatPos.ToString());

                if (p_item.ReadLocalValue(WinItem.FloatSizeProperty) != DependencyProperty.UnsetValue)
                    p_writer.WriteAttributeString("FloatSize", string.Format("{0},{1}", p_item.FloatSize.Width, p_item.FloatSize.Height));
            }

            if (p_item.IsInCenter)
                p_writer.WriteAttributeString("IsInCenter", "True");

            if (p_item.IsInWindow)
                p_writer.WriteAttributeString("IsInWindow", "True");

            foreach (object obj in p_item.Items)
            {
                Tabs sect;
                WinItem dockItem;
                if ((sect = obj as Tabs) != null)
                {
                    WriteSect(sect, p_writer);
                }
                else if ((dockItem = obj as WinItem) != null)
                {
                    WriteDockItem(dockItem, p_writer);
                }
            }
            p_writer.WriteEndElement();
        }

        void WriteSect(Tabs p_sect, XmlWriter p_writer)
        {
            p_writer.WriteStartElement("Tabs");
            if (p_sect.ReadLocalValue(TabControl.SelectedIndexProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("SelectedIndex", p_sect.SelectedIndex.ToString());

            if (p_sect.IsOutlookStyle)
                p_writer.WriteAttributeString("IsOutlookStyle", "True");

            if (!double.IsNaN(p_sect.ActualWidth) && p_sect.ActualWidth > 0)
                p_writer.WriteAttributeString("InitWidth", p_sect.ActualWidth.ToString());
            else if (p_sect.ReadLocalValue(Tabs.InitWidthProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("InitWidth", p_sect.InitWidth.ToString());

            if (!double.IsNaN(p_sect.ActualHeight) && p_sect.ActualHeight > 0)
                p_writer.WriteAttributeString("InitHeight", p_sect.ActualHeight.ToString());
            else if (p_sect.ReadLocalValue(Tabs.InitHeightProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("InitHeight", p_sect.InitHeight.ToString());

            if (p_sect.ReadLocalValue(Tabs.PaddingProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("Padding", string.Format("{0},{1},{2},{3}", p_sect.Padding.Left, p_sect.Padding.Top, p_sect.Padding.Right, p_sect.Padding.Bottom));

            foreach (Tab tab in p_sect.Items.OfType<Tab>())
            {
                // 不输出自动隐藏的项
                if (tab.IsPinned && !string.IsNullOrEmpty(tab.Title))
                    WriteSectItem(tab, p_writer);
            }
            p_writer.WriteEndElement();
        }

        void WriteSectItem(Tab p_item, XmlWriter p_writer)
        {
            p_writer.WriteStartElement("Tab");
            p_writer.WriteAttributeString("Title", p_item.Title);

            if (!string.IsNullOrEmpty(p_item.Name))
                p_writer.WriteAttributeString("Name", p_item.Name);

            if (p_item.ReadLocalValue(Tab.IsPinnedProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("IsPinned", p_item.IsPinned.ToString());

            if (p_item.ReadLocalValue(Tab.CanDockInCenterProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("CanDockInCenter", p_item.CanDockInCenter.ToString());

            if (p_item.ReadLocalValue(Tab.CanDockProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("CanDock", p_item.CanDock.ToString());

            if (p_item.ReadLocalValue(Tab.CanFloatProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("CanFloat", p_item.CanFloat.ToString());

            if (p_item.ReadLocalValue(Tab.CanUserPinProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("CanUserPin", p_item.CanUserPin.ToString());

            if (p_item.ReadLocalValue(TabItem.PopWidthProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("PopWidth", Math.Ceiling(p_item.PopWidth).ToString());

            if (p_item.ReadLocalValue(TabItem.PopHeightProperty) != DependencyProperty.UnsetValue)
                p_writer.WriteAttributeString("PopHeight", Math.Ceiling(p_item.PopHeight).ToString());

            p_writer.WriteEndElement();
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 深度查找所有Tab项，构造以Tab.Title为键以Tab为值的字典，Title不为空
        /// </summary>
        /// <param name="p_items"></param>
        void ExtractItems(ItemsControl p_items)
        {
            foreach (object item in p_items.Items)
            {
                ItemsControl child;
                Tabs sect = item as Tabs;
                if (sect != null)
                {
                    foreach (var obj in sect.Items)
                    {
                        Tab si = obj as Tab;
                        if (si != null && !string.IsNullOrEmpty(si.Title))
                            _tabs[si.Title] = si;
                    }
                }
                else if ((child = item as ItemsControl) != null)
                {
                    ExtractItems(child);
                }
            }
        }

        /// <summary>
        /// 深度移除所有子项
        /// </summary>
        void ClearAllItems()
        {
            ClearItems(_owner.CenterItem);
            ClearItems(_owner);
            ClearItems(_owner.LeftAutoHide);
            ClearItems(_owner.RightAutoHide);
            ClearItems(_owner.TopAutoHide);
            ClearItems(_owner.BottomAutoHide);
            _owner.DockPanel.Clear();
            _owner.ClearWindows();
        }

        /// <summary>
        /// 深度查找所有自动隐藏项，同步移除
        /// </summary>
        /// <param name="p_items"></param>
        /// <returns></returns>
        IEnumerable<Tab> GetHideItems(ItemsControl p_items)
        {
            foreach (object item in p_items.Items)
            {
                ItemsControl child;
                Tabs sect = item as Tabs;
                if (sect != null)
                {
                    int index = 0;
                    while (index < sect.Items.Count)
                    {
                        Tab si = sect.Items[index] as Tab;
                        if (si != null && !si.IsPinned)
                        {
                            sect.Items.RemoveAt(index);
                            yield return si;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                else if ((child = item as ItemsControl) != null)
                {
                    foreach (Tab si in GetHideItems(child))
                    {
                        yield return si;
                    }
                }
            }
        }

        /// <summary>
        /// 将内部所有的Tab转移到两侧隐藏
        /// </summary>
        /// <param name="p_items"></param>
        /// <param name="p_state"></param>
        void MoveToAutoHide(ItemsControl p_items, WinItemState p_state)
        {
            foreach (object item in p_items.Items)
            {
                ItemsControl child;
                Tabs sect = item as Tabs;
                if (sect != null)
                {
                    int index = 0;
                    while (index < sect.Items.Count)
                    {
                        Tab si = sect.Items[index] as Tab;
                        if (si != null)
                        {
                            sect.Items.RemoveAt(index);
                            if (p_state == WinItemState.DockedLeft)
                                _owner.LeftAutoHide.Unpin(si);
                            else if (p_state == WinItemState.DockedRight)
                                _owner.RightAutoHide.Unpin(si);
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                else if ((child = item as ItemsControl) != null)
                {
                    MoveToAutoHide(child, p_state);
                }
            }
        }

        /// <summary>
        /// 构造ToolWindow承载WinItem
        /// </summary>
        /// <param name="p_dockItem"></param>
        /// <returns></returns>
        ToolWindow OpenInWindow(WinItem p_dockItem)
        {
            Point location = p_dockItem.FloatLocation;
            ToolWindow win;
            if (p_dockItem.ReadLocalValue(WinItem.FloatLocationProperty) == DependencyProperty.UnsetValue)
            {
                win = _owner.CreateWindow(p_dockItem.FloatSize, new Point());
                win.Loaded += OnWinLoaded;
            }
            else
            {
                win = _owner.CreateWindow(p_dockItem.FloatSize, location);
            }
            win.Content = p_dockItem;
            win.Show();
            return win;
        }

        /// <summary>
        /// 浮动窗口指定相对位置时加载时计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnWinLoaded(object sender, RoutedEventArgs e)
        {
            ToolWindow win = sender as ToolWindow;
            WinItem item;
            if (win == null || (item = win.Content as WinItem) == null)
                return;

            win.Loaded -= OnWinLoaded;
            switch (item.FloatPos)
            {
                case FloatPosition.Center:
                    win.HorizontalOffset = Math.Ceiling((_owner.ActualWidth - item.FloatSize.Width) / 2);
                    win.VerticalOffset = Math.Ceiling((_owner.ActualHeight - item.FloatSize.Height) / 2);
                    break;
                case FloatPosition.TopLeft:
                    win.HorizontalOffset = 0;
                    win.VerticalOffset = 0;
                    break;
                case FloatPosition.TopRight:
                    win.HorizontalOffset = _owner.ActualWidth - item.FloatSize.Width;
                    win.VerticalOffset = 0;
                    break;
                case FloatPosition.BottomLeft:
                    win.HorizontalOffset = 0;
                    win.VerticalOffset = _owner.ActualHeight - item.FloatSize.Height;
                    break;
                case FloatPosition.BottomRight:
                    win.HorizontalOffset = _owner.ActualWidth - item.FloatSize.Width;
                    win.VerticalOffset = _owner.ActualHeight - item.FloatSize.Height;
                    break;
            }
        }

        /// <summary>
        /// 判断WinItem是否需要从布局中移除
        /// </summary>
        /// <param name="p_di"></param>
        /// <returns></returns>
        bool IsRemoved(WinItem p_di)
        {
            foreach (object item in p_di.Items)
            {
                WinItem di;
                Tabs sect = item as Tabs;

                // 因之前已将IsPinned = false的所有Tab移除
                if (sect != null && sect.Items.Count > 0)
                    return false;
                else if ((di = item as WinItem) != null)
                {
                    if (!IsRemoved(di))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 移除当前WinItem子项中不需要的WinItem
        /// </summary>
        /// <param name="p_di"></param>
        void RemoveUnused(WinItem p_di)
        {
            var dis = from item in p_di.Items
                      where item is WinItem
                      select (item as WinItem);

            foreach (WinItem di in dis)
            {
                if (IsRemoved(di))
                    p_di.Items.Remove(di);
                else
                    RemoveUnused(di);
            }
        }
        #endregion
    }
}