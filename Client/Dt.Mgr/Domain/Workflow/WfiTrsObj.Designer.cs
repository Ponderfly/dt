﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2023-01-19 创建
******************************************************************************/
#endregion

#region 引用命名
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Dt.Mgr.Domain
{
    [Tbl("cm_wfi_trs")]
    public partial class WfiTrsObj : Entity
    {
        #region 构造方法
        WfiTrsObj() { }

        public WfiTrsObj(CellList p_cells) : base(p_cells) { }

        public WfiTrsObj(
            long ID,
            long TrsdID = default,
            long SrcAtviID = default,
            long TgtAtviID = default,
            bool IsRollback = default,
            DateTime Ctime = default)
        {
            AddCell("ID", ID);
            AddCell("TrsdID", TrsdID);
            AddCell("SrcAtviID", SrcAtviID);
            AddCell("TgtAtviID", TgtAtviID);
            AddCell("IsRollback", IsRollback);
            AddCell("Ctime", Ctime);
            IsAdded = true;
        }
        #endregion

        /// <summary>
        /// 迁移模板标识
        /// </summary>
        public long TrsdID
        {
            get { return (long)this["TrsdID"]; }
            set { this["TrsdID"] = value; }
        }

        /// <summary>
        /// 起始活动实例标识
        /// </summary>
        public long SrcAtviID
        {
            get { return (long)this["SrcAtviID"]; }
            set { this["SrcAtviID"] = value; }
        }

        /// <summary>
        /// 目标活动实例标识
        /// </summary>
        public long TgtAtviID
        {
            get { return (long)this["TgtAtviID"]; }
            set { this["TgtAtviID"] = value; }
        }

        /// <summary>
        /// 是否为回退迁移，1表回退
        /// </summary>
        public bool IsRollback
        {
            get { return (bool)this["IsRollback"]; }
            set { this["IsRollback"] = value; }
        }

        /// <summary>
        /// 迁移时间
        /// </summary>
        public DateTime Ctime
        {
            get { return (DateTime)this["Ctime"]; }
            set { this["Ctime"] = value; }
        }

        #region 静态方法
        /// <summary>
        /// 根据主键获得实体对象(包含所有列值)，仅支持单主键，当启用实体缓存时：
        /// <para>1. 首先从缓存中获取，有则直接返回</para>
        /// <para>2. 无则查询数据库，并将查询结果添加到缓存以备下次使用</para>
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <returns>返回实体对象或null</returns>
        public static Task<WfiTrsObj> GetByID(string p_id)
        {
            return EntityEx.GetByID<WfiTrsObj>(p_id);
        }

        /// <summary>
        /// 根据主键获得实体对象(包含所有列值)，仅支持单主键，当启用实体缓存时：
        /// <para>1. 首先从缓存中获取，有则直接返回</para>
        /// <para>2. 无则查询数据库，并将查询结果添加到缓存以备下次使用</para>
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <returns>返回实体对象或null</returns>
        public static Task<WfiTrsObj> GetByID(long p_id)
        {
            return EntityEx.GetByID<WfiTrsObj>(p_id);
        }

        /// <summary>
        /// 根据主键或唯一索引列获得实体对象(包含所有列值)，仅支持单主键，当启用实体缓存时：
        /// <para>1. 首先从缓存中获取，有则直接返回</para>
        /// <para>2. 无则查询数据库，并将查询结果添加到缓存以备下次使用</para>
        /// </summary>
        /// <param name="p_keyName">主键或唯一索引列名</param>
        /// <param name="p_keyVal">键值</param>
        /// <returns>返回实体对象或null</returns>
        public static Task<WfiTrsObj> GetByKey(string p_keyName, string p_keyVal)
        {
            return EntityEx.GetByKey<WfiTrsObj>(p_keyName, p_keyVal);
        }

        /// <summary>
        /// 根据主键删除实体对象，仅支持单主键，删除前先根据主键获取该实体对象，并非直接删除！！！
        /// <para>删除成功后：</para>
        /// <para>1. 若存在领域事件，则发布事件</para>
        /// <para>2. 若已设置服务端缓存，则删除缓存</para>
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <param name="p_isNotify">是否提示删除结果</param>
        /// <returns>true 删除成功</returns>
        public static Task<bool> DelByID(string p_id, bool p_isNotify = true)
        {
            return EntityEx.DelByID<WfiTrsObj>(p_id, p_isNotify);
        }

        /// <summary>
        /// 根据主键删除实体对象，仅支持单主键，删除前先根据主键获取该实体对象，并非直接删除！！！
        /// <para>删除成功后：</para>
        /// <para>1. 若存在领域事件，则发布事件</para>
        /// <para>2. 若已设置服务端缓存，则删除缓存</para>
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <param name="p_isNotify">是否提示删除结果</param>
        /// <returns>true 删除成功</returns>
        public static Task<bool> DelByID(long p_id, bool p_isNotify = true)
        {
            return EntityEx.DelByID<WfiTrsObj>(p_id, p_isNotify);
        }

        /// <summary>
        /// 查询实体列表，每个实体包含所有列值，过滤条件null或空时返回所有实体
        /// </summary>
        /// <param name="p_filter">过滤串，where后面的部分，null或空返回所有实体</param>
        /// <param name="p_params">参数值，支持Dict或匿名对象，默认null</param>
        /// <returns>返回实体列表</returns>
        public static Task<Table<WfiTrsObj>> Query(string p_filter = null, object p_params = null)
        {
            return EntityEx.Query<WfiTrsObj>(p_filter, p_params);
        }

        /// <summary>
        /// 按页查询实体列表，每个实体包含所有列值
        /// </summary>
        /// <param name="p_starRow">起始行号：mysql中第一行为0行</param>
        /// <param name="p_pageSize">每页显示行数</param>
        /// <param name="p_filter">过滤串，where后面的部分</param>
        /// <param name="p_params">参数值，支持Dict或匿名对象，默认null</param>
        /// <returns>返回实体列表</returns>
        public static Task<Table<WfiTrsObj>> Page(int p_starRow, int p_pageSize, string p_filter = null, object p_params = null)
        {
            return EntityEx.Page<WfiTrsObj>(p_starRow, p_pageSize, p_filter, p_params);
        }

        /// <summary>
        /// 返回符合条件的第一个实体对象，每个实体包含所有列值，不存在时返回null
        /// </summary>
        /// <param name="p_filter">过滤串，where后面的部分，null或空返回所有中的第一行</param>
        /// <param name="p_params">参数值，支持Dict或匿名对象，默认null</param>
        /// <returns>返回实体对象或null</returns>
        public static Task<WfiTrsObj> First(string p_filter, object p_params = null)
        {
            return EntityEx.First<WfiTrsObj>(p_filter, p_params);
        }

        /// <summary>
        /// 获取新ID
        /// </summary>
        /// <returns></returns>
        public static Task<long> NewID()
        {
            return EntityEx.GetNewID<WfiTrsObj>();
        }

        /// <summary>
        /// 获取新序列值
        /// </summary>
        /// <param name="p_colName">字段名称，不可为空</param>
        /// <returns>新序列值</returns>
        public static Task<int> NewSeq(string p_colName)
        {
            return EntityEx.GetNewSeq<WfiTrsObj>(p_colName);
        }
        #endregion
    }
}