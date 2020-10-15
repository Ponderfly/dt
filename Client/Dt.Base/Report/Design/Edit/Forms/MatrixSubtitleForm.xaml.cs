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
    public sealed partial class MatrixSubtitleForm : UserControl
    {
        RptDesignInfo _info;
        RptMtxSubtitle _title;

        public MatrixSubtitleForm(RptDesignInfo p_info)
        {
            InitializeComponent();
            _info = p_info;
        }

        internal void LoadItem(RptText p_item)
        {
            _title = p_item.Parent as RptMtxSubtitle;
        }
    }
}