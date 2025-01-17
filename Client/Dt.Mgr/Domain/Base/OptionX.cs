﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2023-01-06 创建
******************************************************************************/
#endregion

#region 引用命名
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Dt.Mgr.Domain
{
    public partial class OptionX
    {
        public static async Task<OptionX> New(
            string Name,
            string Category)
        {
            return new OptionX(Name, Category, await NewSeq("Dispidx"));
        }

        protected override void InitHook()
        {
            OnSaving(async () =>
            {
                Throw.If(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Category), "选项名称和所属分类不可为空！");
                if (Cells["Name"].IsChanged || Cells["Category"].IsChanged)
                {
                    var op = await AtCm.First<OptionX>("选项-选项", new { Category = Category, Name = Name });
                    Throw.If(op != null, "选项重复！");
                }
            });
        }
    }
}