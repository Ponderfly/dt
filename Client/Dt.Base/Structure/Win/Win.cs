﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-03-03 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Base.Docking;
using Dt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endregion

namespace Dt.Base
{
    /// <summary>
    /// 可停靠多区域窗口
    /// </summary>
    public partial class Win : ItemsControl, IWin
    {
        #region 静态内容
        /// <summary>
        /// 页面标题
        /// </summary>
        public readonly static DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(Win),
            null);

        /// <summary>
        /// 图标名称
        /// </summary>
        public readonly static DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(Icons),
            typeof(Win),
            new PropertyMetadata(Icons.文件));

        /// <summary>
        /// PhoneUI模式的首页Title
        /// </summary>
        public static readonly DependencyProperty HomeProperty = DependencyProperty.Register(
            "Home",
            typeof(string),
            typeof(Win),
            new PropertyMetadata(null));

        /// <summary>
        /// 是否自动保存布局状态
        /// </summary>
        public static readonly DependencyProperty AutoSaveLayoutProperty = DependencyProperty.Register(
            "AutoSaveLayout",
            typeof(bool),
            typeof(Win),
            new PropertyMetadata(true));

        /// <summary>
        /// 是否显示恢复默认布局按钮
        /// </summary>
        public static readonly DependencyProperty LayoutButtonVisibleProperty = DependencyProperty.Register(
            "LayoutButtonVisible",
            typeof(Visibility),
            typeof(Win),
            new PropertyMetadata(Visibility.Collapsed));

#if UWP
        static Win()
        {
            EventManager.RegisterClassHandler(typeof(Win), Tab.DragStartedEvent, new DragInfoEventHandler(OnDragStarted));
            EventManager.RegisterClassHandler(typeof(Win), Tab.DragDeltaEvent, new DragInfoEventHandler(OnDragDelta));
            EventManager.RegisterClassHandler(typeof(Win), Tab.DragCompletedEvent, new DragInfoEventHandler(OnDragCompleted));

            EventManager.RegisterClassHandler(typeof(Win), GridResizer.PreviewResizeStartEvent, new EventHandler<ResizeEventArgs>(OnPreviewResize));
            EventManager.RegisterClassHandler(typeof(Win), GridResizer.LayoutChangeEndedEvent, new EventHandler<BaseRoutedEventArgs>(OnLayoutChangeEnded));

            EventManager.RegisterClassHandler(typeof(Win), Tab.PinChangeEvent, new EventHandler<PinChangeEventArgs>(OnPinChange));
        }
#endif

        static void OnDragStarted(object sender, DragInfoEventArgs e)
        {
            (sender as Win).OnDragStarted(e);
        }

        static void OnDragDelta(object sender, DragInfoEventArgs e)
        {
            (sender as Win).OnDragDelta(e);
        }

        static void OnDragCompleted(object sender, DragInfoEventArgs e)
        {
            (sender as Win).OnDragCompleted(e);
        }

        static void OnPreviewResize(object sender, ResizeEventArgs e)
        {
            Win dock = sender as Win;
            FrameworkElement lastChild = (from item in dock._dockPanel.Children.OfType<FrameworkElement>()
                                          where item.Visibility == Visibility.Visible
                                          select item).LastOrDefault();
            if (lastChild != null)
            {
                e.AvailableSize = new Size(lastChild.ActualWidth, lastChild.ActualHeight);
                e.MinSize = new Size(lastChild.MinWidth, lastChild.MinHeight);
            }
        }

        static void OnLayoutChangeEnded(object sender, BaseRoutedEventArgs e)
        {
            Win dock = sender as Win;
            GridResizer resizer = e.OriginalSource as GridResizer;
            if (resizer != null)
            {
                dock.UpdateLayout();
                if (resizer.Preview.OffsetX != 0 || resizer.Preview.OffsetY != 0)
                    dock.OnLayoutChanged();
            }
        }

        static void OnPinChange(object sender, PinChangeEventArgs args)
        {
            args.Handled = true;
            Win dock = sender as Win;
            Tab item = args.OriginalSource as Tab;
            if (dock != null && item != null && item.Parent != null)
                dock.OnPinChange(item);
        }

        #endregion

        #region 成员变量
        static Size _defaultFloatSize = new Size(300.0, 300.0);
        Canvas _popupPanel;
        Compass _compass;
        RootCompass _rootCompass;
        Border _dragCue;
        AutoHideTab _leftAutoHide;
        AutoHideTab _rightAutoHide;
        AutoHideTab _topAutoHide;
        AutoHideTab _bottomAutoHide;
        WinItemPanel _dockPanel;
        WinItem _centerItem;
        bool _isReseting = true;
        readonly LayoutManager _layout;
        Tabs _sectWithCompass;
        bool _isDragDelta;
        #endregion

        #region 构造方法
        public Win()
        {
            if (!AtSys.IsPhoneUI)
            {
                DefaultStyleKey = typeof(Win);
                _layout = new LayoutManager(this);
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 关闭前事件，可以取消关闭
        /// </summary>
        public event EventHandler<AsyncCancelEventArgs> Closing;

        /// <summary>
        /// 关闭后事件
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// 布局变化结束事件
        /// </summary>
        public event EventHandler LayoutChanged;
        #endregion

        #region 属性
        /// <summary>
        /// 获取设置标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 获取设置图标名称
        /// </summary>
        public Icons Icon
        {
            get { return (Icons)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// 获取设置初始参数
        /// </summary>
        public virtual string Params
        {
            get { return null; }
        }

        /// <summary>
        /// 获取设置PhoneUI模式的首页Title，首页为多个页面时用逗号隔开(自动以Tab形式显示)
        /// </summary>
        public string Home
        {
            get { return (string)GetValue(HomeProperty); }
            set { SetValue(HomeProperty, value); }
        }

        /// <summary>
        /// 获取设置是否自动保存布局状态
        /// </summary>
        public bool AutoSaveLayout
        {
            get { return (bool)GetValue(AutoSaveLayoutProperty); }
            set { SetValue(AutoSaveLayoutProperty, value); }
        }

        /// <summary>
        /// 获取是否显示恢复默认布局按钮
        /// </summary>
        public Visibility LayoutButtonVisible
        {
            get { return (Visibility)GetValue(LayoutButtonVisibleProperty); }
            internal set { SetValue(LayoutButtonVisibleProperty, value); }
        }

        /// <summary>
        /// 获取所有浮动项
        /// </summary>
        public IEnumerable<WinItem> FloatItems
        {
            get
            {
                if (_popupPanel != null && _popupPanel.Children.Count > 0)
                {
                    foreach (ToolWindow win in _popupPanel.Children.OfType<ToolWindow>())
                    {
                        WinItem item = win.Content as WinItem;
                        if (item != null)
                            yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// 获取内部停靠面板
        /// </summary>
        internal WinItemPanel DockPanel
        {
            get { return _dockPanel; }
        }

        /// <summary>
        /// 获取左侧隐藏面板
        /// </summary>
        internal AutoHideTab LeftAutoHide
        {
            get { return _leftAutoHide; }
        }

        /// <summary>
        /// 获取右侧隐藏面板
        /// </summary>
        internal AutoHideTab RightAutoHide
        {
            get { return _rightAutoHide; }
        }

        /// <summary>
        /// 获取上侧隐藏面板
        /// </summary>
        internal AutoHideTab TopAutoHide
        {
            get { return _topAutoHide; }
        }

        /// <summary>
        /// 获取下侧隐藏面板
        /// </summary>
        internal AutoHideTab BottomAutoHide
        {
            get { return _bottomAutoHide; }
        }

        /// <summary>
        /// 获取是否正在重置布局
        /// </summary>
        internal bool IsReseting
        {
            get { return _isReseting; }
            set { _isReseting = value; }
        }

        /// <summary>
        /// 获取中部停靠容器
        /// </summary>
        internal WinItem CenterItem
        {
            get { return _centerItem; }
        }
        #endregion

        #region PhoneUI
        Dictionary<string, Tab> _tabs;

        /// <summary>
        /// 导航到指定页，支持多页Tab形式
        /// </summary>
        /// <param name="p_tabTitle">多个页面时用逗号隔开(自动以Tab形式显示)</param>
        public void NaviTo(string p_tabTitle)
        {
            if (!AtSys.IsPhoneUI || string.IsNullOrEmpty(p_tabTitle))
                return;

            // 导航到单页
            Tab tab;
            if (!p_tabTitle.Contains(','))
            {
                if (_tabs.TryGetValue(p_tabTitle, out tab))
                {
                    if (AtApp.Frame.Content == null)
                        tab.PinButtonVisibility = Visibility.Collapsed;
                    PhonePage.Show(tab);
                }
                return;
            }

            // 导航到多页Tab
            string[] names = p_tabTitle.Split(',');
            PhoneTabs tabs = new PhoneTabs();
            if (p_tabTitle == Home)
                tabs.OwnerWin = this;
            foreach (var name in names)
            {
                if (_tabs.TryGetValue(name, out tab))
                    tabs.AddItem(tab);
            }
            if (AtApp.Frame.Content == null)
                tabs.HideBackButton();
            tabs.SelectFirstItem();
            PhonePage.Show(tabs);
        }

        /// <summary>
        /// 深度查找所有Tab项，构造以Tab.Title为键名以Tab为值的字典
        /// </summary>
        /// <param name="p_items"></param>
        void ExtractItems(ItemsControl p_items)
        {
            foreach (var item in p_items.Items.OfType<ItemsControl>())
            {
                Tabs tabs = item as Tabs;
                if (tabs != null)
                {
                    foreach (var tab in tabs.Items.OfType<Tab>())
                    {
                        if (tab == null || string.IsNullOrEmpty(tab.Title))
                            continue;

                        tab.OwnerWin = this;
                        _tabs[tab.Title] = tab;
                    }
                    tabs.Items.Clear();
                }
                else
                {
                    ExtractItems(item);
                }
            }
        }
        #endregion

        #region 实现接口
        /// <summary>
        /// 导航到窗口主页
        /// </summary>
        void IWin.NaviToHome()
        {
            if (!AtSys.IsPhoneUI)
                return;

            if (_tabs == null)
            {
                _tabs = new Dictionary<string, Tab>(StringComparer.OrdinalIgnoreCase);
                ExtractItems(this);
            }
            NaviTo(Home);
        }

        /// <summary>
        /// 关闭或后退之前，返回false表示禁止关闭
        /// </summary>
        /// <returns>true 表允许关闭</returns>
        async Task<bool> IPhonePage.OnClosing()
        {
            if (Closing != null)
            {
                var args = new AsyncCancelEventArgs();
                Closing(this, args);
                await args.EnsureAllCompleted();
                return !args.Cancel;
            }
            return true;
        }

        /// <summary>
        /// 关闭或后退之后
        /// </summary>
        void IPhonePage.OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region 外部方法
        /// <summary>
        /// 恢复默认布局
        /// </summary>
        public void LoadDefaultLayout()
        {
            _layout.LoadDefaultLayout();
        }

        /// <summary>
        /// 删除所有ToolWindow
        /// </summary>
        internal void ClearWindows()
        {
            if (_popupPanel == null || _popupPanel.Children.Count == 0)
                return;

            int index = 0;
            while (index < _popupPanel.Children.Count)
            {
                ToolWindow win = _popupPanel.Children[index] as ToolWindow;
                if (win != null)
                {
                    // 先移除当前项，再清除子项，不可颠倒！
                    _popupPanel.Children.RemoveAt(index);
                    WinItem di = win.Content as WinItem;
                    if (di != null)
                        _layout.ClearItems(di);
                }
                else
                {
                    index++;
                }
            }
        }

        /// <summary>
        /// 窗口内容是否可停靠
        /// </summary>
        /// <param name="p_win"></param>
        /// <returns></returns>
        internal static bool CheckIsDockable(ToolWindow p_win)
        {
            return ((p_win != null) && CheckIsDockable(p_win.Content as WinItem));
        }

        /// <summary>
        /// WinItem内容是否可停靠
        /// </summary>
        /// <param name="p_dockItem"></param>
        /// <returns></returns>
        internal static bool CheckIsDockable(WinItem p_dockItem)
        {
            if ((p_dockItem == null) || (p_dockItem.Items.Count <= 0))
                return false;

            Tabs sect;
            return (((sect = p_dockItem.Items[0] as Tabs) != null && CheckIsDockable(sect))
                || CheckIsDockable(p_dockItem.Items[0] as WinItem));
        }

        /// <summary>
        /// 内容是否可停靠
        /// </summary>
        /// <param name="p_sect"></param>
        /// <returns></returns>
        internal static bool CheckIsDockable(Tabs p_sect)
        {
            return (p_sect != null
                && p_sect.Items.Count > 0
                && CheckIsDockable(p_sect.Items[0] as Tab));
        }

        /// <summary>
        /// 内容是否可停靠
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        internal static bool CheckIsDockable(Tab pane)
        {
            return ((pane != null) && pane.CanDock);
        }

        /// <summary>
        /// 内容是否可停靠
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        internal static bool CheckIsDockable(TabHeader header)
        {
            return ((header != null) && CheckIsDockable(header.Owner as Tabs));
        }
        #endregion

        #region 重写方法
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popupPanel = GetTemplateChild("PopupPanel") as Canvas;
            _compass = GetTemplateChild("Compass") as Compass;
            _rootCompass = GetTemplateChild("RootCompass") as RootCompass;
            _dragCue = GetTemplateChild("DragCue") as Border;
            _leftAutoHide = GetTemplateChild("LeftAutoHide") as AutoHideTab;
            _rightAutoHide = GetTemplateChild("RightAutoHide") as AutoHideTab;
            _topAutoHide = GetTemplateChild("TopAutoHide") as AutoHideTab;
            _bottomAutoHide = GetTemplateChild("BottomAutoHide") as AutoHideTab;
            _dockPanel = GetTemplateChild("ContentDockPanel") as WinItemPanel;
            _centerItem = GetTemplateChild("CenterDockItem") as WinItem;

            Button btn = GetTemplateChild("DefaultLayoutButton") as Button;
            if (btn != null)
            {
                btn.Click -= OnDefaultLayoutClick;
                btn.Click += OnDefaultLayoutClick;
            }
            _layout.Init();
            SizeChanged -= OnSizeChanged;
            SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// 增删子项
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            if (_isReseting || _dockPanel == null)
                return;

            IVectorChangedEventArgs args = (IVectorChangedEventArgs)e;
            int index = (int)args.Index;
            if (args.CollectionChange == CollectionChange.ItemInserted)
            {
                WinItem item = Items[index] as WinItem;
                if (item != null
                    && item.DockState != WinItemState.Floating
                    && !_dockPanel.Children.Contains(item))
                {
                    _dockPanel.Children.Insert(index, item);
                }
            }
            else if (args.CollectionChange == CollectionChange.ItemRemoved)
            {
                int i = 0;
                while (i < _dockPanel.Children.Count - 1)
                {
                    if (!Items.Contains(_dockPanel.Children[i]))
                    {
                        _dockPanel.Children.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            else
            {
                throw new Exception("Win不支持子项重置！");
            }
        }

        #endregion

        #region 开始拖动
        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="e"></param>
        void OnDragStarted(DragInfoEventArgs e)
        {
            Tab sectItem;
            TabHeader header = null;
            FrameworkElement element = null;
            Tabs sect = null;

            if ((sectItem = e.OriginalSource as Tab) != null)
            {
                element = sectItem;
                sect = sectItem.Container;
            }
            else if ((header = e.OriginalSource as TabHeader) != null)
            {
                element = header;
                sect = header.Owner as Tabs;
            }
            else
            {
                throw new Exception("拖动时异常，拖动对象只可能是Tab或PaneHeader！");
            }

            // 水平位置所占比例
            Point offset = e.PointerArgs.GetCurrentPoint(sect).Position;
            double offsetX = offset.X / sect.RenderSize.Width;

            ToolWindow win = null;
            if (sectItem != null)
            {
                win = OpenInWindow(sectItem);
            }
            else
            {
                win = OpenInWindow(sect);
            }

            Point winPos = e.PointerArgs.GetCurrentPoint(this).Position;
            offsetX = offsetX > 0 ? Math.Round(offsetX * win.Width) : 10;
            win.HorizontalOffset = winPos.X - offsetX;
            win.VerticalOffset = winPos.Y - 20;
            win.StartDrag(e.PointerArgs);
        }

        /// <summary>
        /// 构造ToolWindow承载Tab，结构 ToolWindow -> WinItem -> Tabs -> Tab
        /// </summary>
        /// <param name="p_sectItem"></param>
        /// <returns></returns>
        ToolWindow OpenInWindow(Tab p_sectItem)
        {
            Point initPos = new Point();
            Size initSize = _defaultFloatSize;
            ToolWindow oldWin = GetParentWindow(p_sectItem);
            if (oldWin != null)
            {
                initPos = new Point(oldWin.HorizontalOffset, oldWin.VerticalOffset);
                initSize = new Size(oldWin.Width, oldWin.Height);
            }
            else
            {
                WinItem oldContainer = null;
                if (p_sectItem.Container != null)
                    oldContainer = p_sectItem.Container.Parent as WinItem;

                if (oldContainer != null)
                {
                    initPos = oldContainer.FloatLocation;
                    initSize = oldContainer.FloatSize;
                }
            }
            p_sectItem.RemoveFromParent();

            ToolWindow win = CreateWindow(initSize, initPos);
            WinItem dockItem = new WinItem();
            dockItem.DockState = WinItemState.Floating;
            Tabs sect = new Tabs();
            sect.Items.Add(p_sectItem);
            dockItem.Items.Add(sect);
            win.Content = dockItem;
            win.Show();
            return win;
        }

        /// <summary>
        /// 构造ToolWindow承载Tabs，直接将Tabs移动到新WinItem
        /// </summary>
        /// <param name="p_sect"></param>
        /// <returns></returns>
        ToolWindow OpenInWindow(Tabs p_sect)
        {
            Point initPos = new Point();
            Size initSize = _defaultFloatSize;
            ToolWindow oldWin = GetParentWindow(p_sect);
            if (oldWin != null)
            {
                initPos = new Point(oldWin.HorizontalOffset, oldWin.VerticalOffset);
                initSize = new Size(oldWin.Width, oldWin.Height);
            }

            ToolWindow win = CreateWindow(initSize, initPos);
            WinItem dockItem = new WinItem();
            dockItem.DockState = WinItemState.Floating;
            p_sect.RemoveFromParent();
            dockItem.Items.Add(p_sect);
            win.Content = dockItem;
            win.Show();
            return win;
        }

        internal ToolWindow CreateWindow(Size p_size, Point p_location)
        {
            ToolWindow win = new ToolWindow();
            win.Owner = _popupPanel;
            win.Width = p_size.Width;
            win.Height = p_size.Height;
            win.HorizontalOffset = p_location.X;
            win.VerticalOffset = p_location.Y;
            return win;
        }
        #endregion

        #region 拖动过程
        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="e"></param>
        void OnDragDelta(DragInfoEventArgs e)
        {
            ToolWindow win = e.OriginalSource as ToolWindow;
            if (win == null)
                return;

            if (CheckIsDockable(win))
            {
                Point pos = e.PointerArgs.GetCurrentPoint(_popupPanel).Position;
                UpdateCompass(pos, win);
                UpdateRootCompass(pos);
                AdjustCueSize(win);
            }
            else
            {
                _rootCompass.Visibility = Visibility.Collapsed;
            }
            _isDragDelta = true;
        }

        /// <summary>
        /// 更新Tabs内部停靠导航
        /// </summary>
        /// <param name="p_pos"></param>
        /// <param name="p_win"></param>
        void UpdateCompass(Point p_pos, ToolWindow p_win)
        {
            // 获取当前位置处其他窗口中的Tabs
            Tabs sect = GetHitSect(p_pos, _popupPanel, p_win);
            if (sect == null || !CheckIsDockable(sect))
            {
                // 当前位置处的Tabs
                sect = GetHitSect(p_pos, this, p_win);
                if (sect != null && sect.IsInCenter && !p_win.CanDockInCenter)
                    sect = null;
            }

            // 有变化
            if (sect != _sectWithCompass && _compass != null)
            {
                _compass.ClearIndicators();
            }

            if (sect == null)
            {
                // 无选中区域
                _sectWithCompass = null;
                _compass.Visibility = Visibility.Collapsed;
            }
            else
            {
                // 有停靠区域
                _sectWithCompass = sect;
                _compass.Visibility = Visibility.Visible;
                double horOffset = (sect.ActualWidth - _compass.Width) / 2.0;
                double verOffset = (sect.ActualHeight - _compass.Height) / 2.0;
                Point pos = GetElementPositionRelatedToPopup(sect);
                double left = Math.Round((double)(horOffset + pos.X));
                double top = Math.Round((double)(verOffset + pos.Y));
                Canvas.SetLeft(_compass, left);
                Canvas.SetTop(_compass, top);
                _compass.ChangeDockPosition(p_pos);
            }
        }

        /// <summary>
        /// 更新最外层停靠导航
        /// </summary>
        /// <param name="p_pos"></param>
        void UpdateRootCompass(Point p_pos)
        {
            _rootCompass.Visibility = Visibility.Visible;
            _rootCompass.ChangeDockPosition(p_pos);
        }

        /// <summary>
        /// 显示可停靠区域背景
        /// </summary>
        /// <param name="p_win"></param>
        void AdjustCueSize(ToolWindow p_win)
        {
            Rect rect = new Rect();
            bool showCue = false;
            if (_compass.DockPosition != DockPosition.None && _sectWithCompass != null)
            {
                // 在WinItem内部停靠
                rect = _sectWithCompass.GetRectDimenstion(_compass.DockPosition, p_win.Content as WinItem);
                Point topLeft = GetElementPositionRelatedToPopup(_sectWithCompass.Container);
                rect.X += topLeft.X;
                rect.Y += topLeft.Y;
                showCue = true;
            }
            else if (_rootCompass.DockPosition != DockPosition.None && _rootCompass.DockPosition != DockPosition.Center)
            {
                // 最外层停靠
                WinItem dockItem = p_win.Content as WinItem;
                Size relativeSize = new Size(dockItem.InitWidth, dockItem.InitHeight);
                switch (_rootCompass.DockPosition)
                {
                    case DockPosition.Top:
                        rect.Width = ActualWidth;
                        rect.Height = relativeSize.Height;
                        break;

                    case DockPosition.Bottom:
                        rect.Y += ActualHeight - relativeSize.Height;
                        rect.Width = ActualWidth;
                        rect.Height = relativeSize.Height;
                        break;

                    case DockPosition.Left:
                        rect.Width = relativeSize.Width;
                        rect.Height = ActualHeight;
                        break;

                    case DockPosition.Right:
                        rect.X += ActualWidth - relativeSize.Width;
                        rect.Width = relativeSize.Width;
                        rect.Height = ActualHeight;
                        break;
                }
                showCue = true;
            }

            if (showCue)
            {
                _dragCue.Visibility = Visibility.Visible;
                _dragCue.Width = rect.Width;
                _dragCue.Height = rect.Height;
                Canvas.SetLeft(_dragCue, rect.Left);
                Canvas.SetTop(_dragCue, rect.Top);
            }
            else
            {
                _dragCue.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 获取指定位置的Tabs
        /// </summary>
        /// <param name="p_pos"></param>
        /// <param name="p_subtree"></param>
        /// <param name="p_parent"></param>
        /// <returns></returns>
        static Tabs GetHitSect(Point p_pos, UIElement p_subtree, UIElement p_parent)
        {
            if (p_subtree != null && p_parent != null)
            {
                Point pt = p_subtree.TransformToVisual(null).TransformPoint(p_pos);
                return (from sect in VisualTreeHelper.FindElementsInHostCoordinates(pt, p_subtree).OfType<Tabs>()
                        where CheckIsDockable(sect) && !p_parent.IsAncestorOf(sect.Container)
                        select sect).FirstOrDefault();
            }
            return null;
        }

        #endregion

        #region 拖动结束
        /// <summary>
        /// 拖拽结束
        /// </summary>
        /// <param name="e"></param>
        void OnDragCompleted(DragInfoEventArgs e)
        {
            ToolWindow win = e.OriginalSource as ToolWindow;
            if (win == null)
                return;

            WinItem dockItem = win.Content as WinItem;
            if (_sectWithCompass != null && _compass.DockPosition != DockPosition.None)
            {
                // 停靠在WinItem内部
                win.ClearValue(ContentControl.ContentProperty);
                _sectWithCompass.AddItem(dockItem, _compass.DockPosition);
            }
            else if (_rootCompass.DockPosition != DockPosition.None && _rootCompass.DockPosition != DockPosition.Center)
            {
                // 停靠在四边
                win.ClearValue(ContentControl.ContentProperty);
                switch (_rootCompass.DockPosition)
                {
                    case DockPosition.Top:
                        dockItem.DockState = WinItemState.DockedTop;
                        break;
                    case DockPosition.Bottom:
                        dockItem.DockState = WinItemState.DockedBottom;
                        break;
                    case DockPosition.Left:
                        dockItem.DockState = WinItemState.DockedLeft;
                        break;
                    case DockPosition.Right:
                        dockItem.DockState = WinItemState.DockedRight;
                        break;
                }
                Items.Insert(0, dockItem);
            }

            _sectWithCompass = null;
            _compass.DockPosition = DockPosition.None;
            _rootCompass.DockPosition = DockPosition.None;
            _compass.Visibility = Visibility.Collapsed;
            _rootCompass.Visibility = Visibility.Collapsed;
            _dragCue.Visibility = Visibility.Collapsed;

            if (_isDragDelta)
            {
                OnLayoutChanged();
                _isDragDelta = false;
            }
        }
        #endregion

        #region Pin状态切换
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void OnPinChange(Tab item)
        {
            if (item.IsPinned)
            {
                AutoHideTab autoHide = item.Parent as AutoHideTab;
                if (autoHide != null)
                {
                    autoHide.Pin(item);
                    WinItemState dockState = GetDockState(autoHide.TabStripPlacement);
                    Tabs sect = FindPinSect(dockState);
                    if (sect != null)
                    {
                        // 直接停靠
                        sect.Items.Add(item);
                    }
                    else
                    {
                        WinItem dockItem = new WinItem();
                        dockItem.DockState = dockState;
                        sect = new Tabs();
                        dockItem.Items.Add(sect);
                        sect.Items.Add(item);
                        Items.Add(dockItem);
                    }
                }
            }
            else
            {
                Tabs sect = item.Parent as Tabs;
                WinItem dockItem;
                if (sect != null && (dockItem = sect.Container) != null)
                {
                    AutoHideTab autoHide = null;
                    switch (dockItem.DockState)
                    {
                        case WinItemState.DockedLeft:
                            autoHide = _leftAutoHide;
                            break;
                        case WinItemState.DockedBottom:
                            autoHide = _bottomAutoHide;
                            break;
                        case WinItemState.DockedRight:
                            autoHide = _rightAutoHide;
                            break;
                        case WinItemState.DockedTop:
                            autoHide = _topAutoHide;
                            break;
                    }
                    if (autoHide != null)
                        autoHide.Unpin(item);
                }
            }
            OnLayoutChanged();
        }

        /// <summary>
        /// 查找停靠位置的Tabs
        /// </summary>
        /// <param name="p_dockState"></param>
        /// <returns></returns>
        Tabs FindPinSect(WinItemState p_dockState)
        {
            Tabs sect = null;
            WinItem item = (from dockItem in Items.OfType<WinItem>()
                            where dockItem.DockState == p_dockState
                            select dockItem).FirstOrDefault();
            if (item != null)
            {
                sect = (from obj in item.GetAllTabs()
                        select obj).FirstOrDefault();
            }
            return sect;
        }

        WinItemState GetDockState(ItemPlacement p_dockState)
        {
            switch (p_dockState)
            {
                case ItemPlacement.Left:
                    return WinItemState.DockedLeft;

                case ItemPlacement.Right:
                    return WinItemState.DockedRight;

                case ItemPlacement.Top:
                    return WinItemState.DockedTop;

                case ItemPlacement.Bottom:
                    return WinItemState.DockedBottom;
            }
            return WinItemState.DockedLeft;
        }
        #endregion

        #region 内部方法
        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 关闭自动隐藏项
            _leftAutoHide.SelectedIndex = -1;
            _rightAutoHide.SelectedIndex = -1;
            _topAutoHide.SelectedIndex = -1;
            _bottomAutoHide.SelectedIndex = -1;

            // 更新RootCompass的大小，因在Canvas中不能自动伸展
            Size size = e.NewSize;
            _rootCompass.Width = size.Width;
            _rootCompass.Height = size.Height;

            if (size.Width != e.PreviousSize.Width)
                _layout.OnWidthChanged(size.Width);
        }

        static ToolWindow GetParentWindow(Tab p_sectItem)
        {
            return GetParentWindow(p_sectItem.Container);
        }

        static ToolWindow GetParentWindow(Tabs p_sect)
        {
            if (p_sect == null)
                return null;
            return GetParentWindow(p_sect.Container);
        }

        static ToolWindow GetParentWindow(WinItem p_container)
        {
            if (p_container != null)
            {
                while (p_container.Container != null)
                {
                    p_container = p_container.Container;
                }
                return (p_container.Parent as ToolWindow);
            }
            return null;
        }

        Point GetElementPositionRelatedToPopup(FrameworkElement element)
        {
            return element.TransformToVisual(this).TransformPoint(new Point());
        }

        /// <summary>
        /// 恢复默认布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDefaultLayoutClick(object sender, RoutedEventArgs e)
        {
            _layout.LoadDefaultLayout();
        }

        /// <summary>
        /// 触发布局变化结束事件
        /// </summary>
        void OnLayoutChanged()
        {
            _layout.SaveCurrentLayout();
            LayoutChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}