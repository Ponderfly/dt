﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2017-12-06 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core;
#endregion

namespace Dt.Mgr
{
    /// <summary>
    /// 收藏菜单项
    /// </summary>
    [Sqlite("lob")]
    public class MenuFavX : EntityX<MenuFavX>
    {
        #region 构造方法
        MenuFavX() { }

        public MenuFavX(
            long UserID,
            long MenuID = default,
            int Clicks = default)
        {
            AddCell("UserID", UserID);
            AddCell("MenuID", MenuID);
            AddCell("Clicks", Clicks);
            IsAdded = true;
        }
        #endregion

        /// <summary>
        /// 用户标识
        /// </summary>
        [PrimaryKey]
        public long UserID
        {
            get { return (long)this["UserID"]; }
            set { this["UserID"] = value; }
        }

        /// <summary>
        /// 菜单标识
        /// </summary>
        [PrimaryKey]
        public long MenuID
        {
            get { return (long)this["MenuID"]; }
            set { this["MenuID"] = value; }
        }

        /// <summary>
        /// 点击次数，在常用列表中排序用，可以被重置
        /// </summary>
        public int Clicks
        {
            get { return (int)this["Clicks"]; }
            set { this["Clicks"] = value; }
        }
    }
}