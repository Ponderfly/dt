﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2018-08-23 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endregion

namespace Dt.Base.Report
{
    public sealed partial class RptDesignHome : Win
    {
        RptDesignInfo _info;
        RptDesignWin _design;
        TextBox _tbXaml;
        ViewSettingWin _viewSetting;
        PageSettingWin _pageSetting;
        ParamsWin _params;
        DbDataWin _dbData;
        ScriptDataWin _scriptData;

        public RptDesignHome(RptDesignInfo p_info)
        {
            InitializeComponent();
            _info = p_info;
            _design = new RptDesignWin(_info);
            LoadCenter(_design);
            _btnSave.Command = new SaveCmd(_info);
        }

        void OnEditTemplate(object sender, RoutedEventArgs e)
        {
            LoadCenter(_design);
        }

        void OnExport(object sender, RoutedEventArgs e)
        {
            if (_tbXaml == null)
            {
                _tbXaml = new TextBox { AcceptsReturn = true, BorderThickness = new Thickness(0) };
                ScrollViewer.SetHorizontalScrollBarVisibility(_tbXaml, ScrollBarVisibility.Auto);
                ScrollViewer.SetVerticalScrollBarVisibility(_tbXaml, ScrollBarVisibility.Auto);
            }
                
            _tbXaml.Text = AtRpt.SerializeTemplate(_info.Root);
            LoadCenter(_tbXaml);
        }

        async void OnImport(object sender, RoutedEventArgs e)
        {
            if (_info.IsDirty && !await AtKit.Confirm("当前模板已修改，导入新模板会丢失修改内容，继续导入吗？"))
                return;

            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".xml");
            StorageFile sf = await picker.PickSingleFileAsync();
            if (sf != null)
            {
                try
                {
                    using (var stream = await sf.OpenStreamForReadAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        await _info.ImportTemplate(reader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("导入报表模板时异常：{0}", ex.Message));
                }
            }
        }

        void OnPreview(object sender, RoutedEventArgs e)
        {
            // 比较窗口类型和初始参数，关闭旧窗口
            var info = new RptInfo { Name = _info.Name, Root = _info.Root };
            Win win;
            if (!AtSys.IsPhoneUI
                && (win = Desktop.Inst.ActiveWin(typeof(RptViewWin), info)) != null)
            {
                win.Close();
            }

            AtRpt.Show(info);
        }

        void OnPageSetting(object sender, RoutedEventArgs e)
        {
            if (_pageSetting == null)
                _pageSetting = new PageSettingWin(_info);
            LoadCenter(_pageSetting);
        }

        void OnViewSetting(object sender, RoutedEventArgs e)
        {
            if (_viewSetting == null)
                _viewSetting = new ViewSettingWin(_info);
            LoadCenter(_viewSetting);
        }

        void OnParams(object sender, RoutedEventArgs e)
        {
            if (_params == null)
                _params = new ParamsWin(_info);
            LoadCenter(_params);
        }

        void OnDbData(object sender, RoutedEventArgs e)
        {
            if (_dbData == null)
                _dbData = new DbDataWin(_info);
            LoadCenter(_dbData);
        }

        void OnScriptData(object sender, RoutedEventArgs e)
        {
            if (_scriptData == null)
                _scriptData = new ScriptDataWin(_info);
            LoadCenter(_scriptData);
        }

        void OnSave(object sender, RoutedEventArgs e)
        {
            _info.SaveTemplate();
        }
    }
}