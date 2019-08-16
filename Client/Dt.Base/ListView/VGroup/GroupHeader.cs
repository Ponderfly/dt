﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2013-06-04 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
#endregion

namespace Dt.Base.ListView
{
    /// <summary>
    /// 分组导航头
    /// </summary>
    public partial class GroupHeader : Panel
    {
        Border _border;
        double _left;
        double _maxTrans;
        GroupHeaderCell _cellSelected;

        #region 构造方法
        public GroupHeader(Lv p_owner)
        {
            Lv = p_owner;
            Background = AtRes.WhiteBrush;
            foreach (var grp in p_owner.GroupRows)
            {
                Children.Add(new GroupHeaderCell(grp, this));
            }
            _border = new Border { BorderThickness = new Thickness(0, 0, 0, 1), BorderBrush = AtRes.浅灰边框, IsHitTestVisible = false };
            Children.Add(_border);

            if (!AtSys.IsPhoneUI)
                PointerWheelChanged += OnPointerWheelChanged;
        }
        #endregion

        internal Lv Lv { get; }

        internal void SetCurrentGroup(GroupRow p_group)
        {
            if (_cellSelected != null && _cellSelected.Group == p_group)
                return;

            foreach (var cell in Children.OfType<GroupHeaderCell>())
            {
                if (cell.Group == p_group)
                    _cellSelected = cell;
                else
                    cell.ClearValue(GroupHeaderCell.IsSelectedProperty);
            }
            if (_cellSelected != null)
            {
                _cellSelected.IsSelected = true;
                if (_cellSelected.Left < 0)
                {
                    _left -= _cellSelected.Left;
                    InvalidateArrange();
                }
                else if (ActualWidth > 0 && _cellSelected.Left + _cellSelected.DesiredSize.Width > ActualWidth)
                {
                    _left -= _cellSelected.Left + _cellSelected.DesiredSize.Width - ActualWidth;
                    InvalidateArrange();
                }
            }
        }

        internal void DoHorScroll(double p_delta)
        {
            if (p_delta < 0 && _left > -_maxTrans)
            {
                _left = Math.Max(-_maxTrans, _left + p_delta);
                InvalidateArrange();
            }
            else if (p_delta > 0 && _left < 0)
            {
                _left = Math.Min(0, _left + p_delta);
                InvalidateArrange();
            }
        }

        void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            DoHorScroll(e.GetCurrentPoint(null).Properties.MouseWheelDelta);
        }

        #region 测量布局
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(availableSize.Width, AtRes.RowOuterHeight);
            double width = 0;
            foreach (var elem in Children.OfType<GroupHeaderCell>())
            {
                elem.Measure(size);
                width += elem.DesiredSize.Width;
            }
            _border.Measure(size);
            // 水平最大平移范围
            _maxTrans = Math.Max(width - availableSize.Width, 0);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double left = _left;
            foreach (var cell in Children.OfType<GroupHeaderCell>())
            {
                // 完全不显示、显示部分、显示全部
                if (left > finalSize.Width)
                    cell.Arrange(new Rect());
                else if (left + cell.DesiredSize.Width > finalSize.Width)
                    cell.Arrange(new Rect(left, 0, finalSize.Width - left, AtRes.RowOuterHeight));
                else
                    cell.Arrange(new Rect(left, 0, cell.DesiredSize.Width, AtRes.RowOuterHeight));
                cell.Left = left;
                left += cell.DesiredSize.Width;
            }
            _border.Arrange(new Rect(new Point(), finalSize));
            return finalSize;
        }
        #endregion
    }
}
