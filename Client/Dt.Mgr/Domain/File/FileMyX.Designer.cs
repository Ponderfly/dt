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
    [Tbl("cm_file_my")]
    public partial class FileMyX : EntityX<FileMyX>
    {
        #region 构造方法
        FileMyX() { }

        public FileMyX(CellList p_cells) : base(p_cells) { }

        public FileMyX(
            long ID,
            long? ParentID = default,
            string Name = default,
            bool IsFolder = default,
            string ExtName = default,
            string Info = default,
            DateTime Ctime = default,
            long UserID = default)
        {
            AddCell("ID", ID);
            AddCell("ParentID", ParentID);
            AddCell("Name", Name);
            AddCell("IsFolder", IsFolder);
            AddCell("ExtName", ExtName);
            AddCell("Info", Info);
            AddCell("Ctime", Ctime);
            AddCell("UserID", UserID);
            IsAdded = true;
        }
        #endregion

        /// <summary>
        /// 上级目录，根目录的parendid为空
        /// </summary>
        public long? ParentID
        {
            get { return (long?)this["ParentID"]; }
            set { this["ParentID"] = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsFolder
        {
            get { return (bool)this["IsFolder"]; }
            set { this["IsFolder"] = value; }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtName
        {
            get { return (string)this["ExtName"]; }
            set { this["ExtName"] = value; }
        }

        /// <summary>
        /// 文件描述信息
        /// </summary>
        public string Info
        {
            get { return (string)this["Info"]; }
            set { this["Info"] = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Ctime
        {
            get { return (DateTime)this["Ctime"]; }
            set { this["Ctime"] = value; }
        }

        /// <summary>
        /// 所属用户
        /// </summary>
        public long UserID
        {
            get { return (long)this["UserID"]; }
            set { this["UserID"] = value; }
        }
    }
}