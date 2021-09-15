﻿#region 文件描述
/******************************************************************************
* 创建: $username$
* 摘要: 
* 日志: $time$ 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Base;
using Dt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endregion

namespace $rootnamespace$
{
    public sealed partial class $safeitemname$ : Mv
    {
        public $safeitemname$()
        {
            InitializeComponent();
            Menu["保存"].Bind(IsEnabledProperty, _fv, "IsDirty");
        }

        public async void Update(long p_id)
        {
            if (!await _fv.DiscardChanges())
                return;

            if (p_id > 0)
            {
                //_fv.Data = await AtCm.First<>("xxx", new { id = p_id });
            }
            else
            {
                Create();
            }
        }

        public void Clear()
        {
            _fv.Data = null;
        }

        void Create()
        {
            //_fv.Data = new xxx(
            //    ID: await AtCm.NewID());
        }

        void OnSave(object sender, Mi e)
        {
            Save();
        }

        void OnAdd(object sender, Mi e)
        {
            Create();
        }

        protected override Task<bool> OnClosing()
        {
            return _fv.DiscardChanges();
        }

        void Save()
        {
            //var d = _fv.Data.To<>();
            //if (await AtCm.Save(d))
            //{
            //    _win.List.Update();
            //}
        }

        async void OnDel(object sender, Mi e)
        {
            //var d = _fv.Data.To<>();
            //if (d == null)
            //    return;

            //if (d.IsAdded)
            //{
            //    Clear();
            //    return;
            //}

            if (!await Kit.Confirm("确认要删除吗？"))
            {
                Kit.Msg("已取消删除！");
                return;
            }

        }
    }
}
