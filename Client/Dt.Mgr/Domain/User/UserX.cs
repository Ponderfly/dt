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
    public partial class UserX
    {
        public static async Task<UserX> New()
        {
            long id = await NewID();
            return new UserX(id, Name: "新用户");
        }

        protected override void InitHook()
        {
            OnSaving(() =>
            {
                if (IsAdded || Cells["Name"].IsChanged)
                    Throw.IfEmpty(Name, "名称不可为空！");

                if (IsAdded || Cells["Name"].IsChanged)
                    Throw.IfEmpty(Phone, "手机号不可为空！");
                return Task.CompletedTask;
            });
        }
    }
}