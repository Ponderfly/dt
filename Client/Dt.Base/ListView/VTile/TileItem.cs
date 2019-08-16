﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-01-29 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
#endregion

namespace Dt.Base.ListView
{
    /// <summary>
    /// 普通列表的行
    /// </summary>
    public partial class TileItem : LvRow
    {
        Button _btnMenu;

        public TileItem(Lv p_owner, DataTemplate p_temp) : base(p_owner)
        {
            LoadTemplate(p_temp);
            AttachEvent();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double height = AtRes.RowOuterHeight;

            // 内容
            var elem = (UIElement)Children[0];
            if (_owner.ItemHeight > 0)
            {
                height = _owner.ItemHeight;
                elem.Measure(new Size(availableSize.Width, height));
            }
            else
            {
                elem.Measure(availableSize);
                if (elem.DesiredSize.Height > height)
                    height = elem.DesiredSize.Height;
            }

            // 上下文菜单
            if (_btnMenu != null)
                _btnMenu.Measure(new Size(_flagWidth, _flagWidth));

            // 选择背景
            elem = (UIElement)Children[Children.Count - 2];
            Size size = new Size(availableSize.Width, height);
            elem.Measure(size);

            // 交互背景
            _rcPointer.Measure(size);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect rc = new Rect(new Point(), finalSize);
            ((UIElement)Children[0]).Arrange(rc);
            if (_btnMenu != null)
                _btnMenu.Arrange(new Rect(finalSize.Width - _flagWidth - 1, 0, _flagWidth, _flagWidth));
            ((UIElement)Children[Children.Count - 2]).Arrange(rc);
            _rcPointer.Arrange(rc);
            return finalSize;
        }

        void LoadTemplate(DataTemplate p_template)
        {
            // 背景
            SetBinding(BackgroundProperty, new Binding { Path = new PropertyPath("Background") });

            // 内容
            Children.Add(p_template.LoadContent() as UIElement);

            // 上下文菜单
            Menu menu = Ex.GetMenu(_owner);
            if (menu != null)
            {
                _btnMenu = AttachContextMenu(menu);
                if (_btnMenu != null)
                    Children.Add(_btnMenu);
            }

            // 分割线及选择背景
            Border bd;
            if (_owner.ShowItemBorder)
                bd = new Border { BorderThickness = new Thickness(0, 0, 1, 1), BorderBrush = AtRes.浅灰边框, IsHitTestVisible = false };
            else
                bd = new Border { IsHitTestVisible = false };
            if (_owner.SelectionMode != SelectionMode.None)
            {
                bd.SetBinding(Border.BackgroundProperty, new Binding
                {
                    Path = new PropertyPath("IsSelected"),
                    Converter = new SelectedBackgroundConverter(),
                });
            }
            Children.Add(bd);

            // 交互背景
            _rcPointer = new Rectangle { IsHitTestVisible = false };
            Children.Add(_rcPointer);
        }
    }
}