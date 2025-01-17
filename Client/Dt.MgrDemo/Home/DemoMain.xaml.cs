﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2023-01-09 创建
******************************************************************************/
#endregion

#region 引用命名
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dt.Base;
using Dt.MgrDemo.单实体;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#endregion

namespace Dt.MgrDemo
{
    public partial class DemoMain : Mv
    {
        public DemoMain()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            Nl<GroupData<DemoItem>> ds = new Nl<GroupData<DemoItem>>();

            var group = new GroupData<DemoItem>
            {
                new DemoItem("实体基础", typeof(AccessDemo), ""),
                
            };
            group.Title = "基础";
            ds.Add(group);

            _lv.Data= ds;
        }

        void OnItemClick(object sender, ItemClickArgs e)
        {
            Kit.RunAsync(() =>
            {
                var item = e.Data.To<DemoItem>();
                Kit.OpenWin(item.Type, item.Title);
            });
        }
    }
}