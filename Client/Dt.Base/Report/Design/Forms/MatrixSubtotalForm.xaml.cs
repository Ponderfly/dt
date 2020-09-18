﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2020-09-18 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dt.Base;
using Dt.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endregion

namespace Dt.Base.Report
{
    public sealed partial class MatrixSubtotalForm : UserControl
    {
        RptDesignWin _owner;
        RptMtxSubtotal _total;

        public MatrixSubtotalForm(RptDesignWin p_owner)
        {
            InitializeComponent();
            _owner = p_owner;
        }

        internal void LoadItem(RptText p_item)
        {
            if (p_item != null)
                _total = p_item.Parent as RptMtxSubtotal;
        }
    }
}
