﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2023-01-30 创建
******************************************************************************/
#endregion

#region 引用命名
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Dt.Mgr.Domain
{
    [Tbl("cm_role_menu")]
    public partial class RoleMenuX : EntityX<RoleMenuX>
    {
        #region 构造方法
        RoleMenuX() { }

        public RoleMenuX(CellList p_cells) : base(p_cells) { }

        public RoleMenuX(
            long RoleID,
            long MenuID)
        {
            AddCell("RoleID", RoleID);
            AddCell("MenuID", MenuID);
            IsAdded = true;
        }
        #endregion

        /// <summary>
        /// 角色标识
        /// </summary>
        public long RoleID
        {
            get { return (long)this["RoleID"]; }
            set { this["RoleID"] = value; }
        }

        /// <summary>
        /// 菜单标识
        /// </summary>
        public long MenuID
        {
            get { return (long)this["MenuID"]; }
            set { this["MenuID"] = value; }
        }

        new public long ID { get { return -1; } }
    }
}