﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2013-12-16 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Base;
using Dt.Core;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endregion

namespace Dt.Sample
{
    public partial class FvDataSource : Win
    {
        Table _tbl;
        int _rowNum;

        public FvDataSource()
        {
            InitializeComponent();

            _tbl = new Table { { "name" }, { "fontsize", typeof(double) }, { "id" }, };
        }

        void OnDataRow(object sender, RoutedEventArgs e)
        {
            _fv.Data = _tbl.NewRow(
                $"第{++_rowNum}行",
                22,
                _rowNum.ToString()
                );
        }

        void OnTgt1(object sender, RoutedEventArgs e)
        {
            _fv.Data = _tb;
        }

        void OnTgt2(object sender, RoutedEventArgs e)
        {
            _fv.Data = _btn;
        }

        void OnTgt3(object sender, RoutedEventArgs e)
        {
            _fv.Data = _tb2;
        }

        void OnNull(object sender, RoutedEventArgs e)
        {
            _fv.Data = null;
        }
    }
}