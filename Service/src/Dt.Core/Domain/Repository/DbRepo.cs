﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-10-17 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core.EventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Dt.Core.Domain
{
    /// <summary>
    /// 包含"ID"主键的mysql仓库
    /// </summary>
    /// <typeparam name="TEntity">聚合根类型</typeparam>
    /// <typeparam name="TKey">聚合根主键类型</typeparam>
    public class DbRepo<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Root<TKey>
    {
        #region 静态内容
        protected static readonly ModelBuilder _model;

        static DbRepo()
        {
            _model = new ModelBuilder(typeof(TEntity));
        }
        #endregion

        /// <summary>
        /// 业务线上下文
        /// </summary>
        protected readonly LobContext _ = LobContext.Current;

        /// <summary>
        /// 根据主键获得实体对象，不存在时返回null
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <param name="p_loadDetails">是否加载附加数据，默认false</param>
        /// <returns>返回实体对象或null</returns>
        public async Task<TEntity> Get(TKey p_id, bool p_loadDetails = false)
        {
            TEntity entity = await _.Db.First<TEntity>(_model.SqlSelect, new { id = p_id });
            if (entity != null && p_loadDetails)
                await LoadDetails(entity);
            return entity;
        }

        /// <summary>
        /// 加载附加数据，默认加载所有关联的子表数据
        /// </summary>
        /// <param name="p_entity"></param>
        /// <returns></returns>
        public virtual async Task LoadDetails(TEntity p_entity)
        {
            if (!_model.ExistChild)
                return;

            foreach (var child in _model.Children)
            {
                var ls = await _.Db.List(child.Type, child.SqlSelect, new { parentid = p_entity.ID });
                child.PropInfo.SetValue(p_entity, ls);
            }
        }

        /// <summary>
        /// 以参数值方式执行Sql语句，返回实体列表，未加载实体的附加数据！
        /// </summary>
        /// <param name="p_keyOrSql">Sql字典中的键名(无空格) 或 Sql语句</param>
        /// <param name="p_params">参数值，支持Dict或匿名对象，默认null</param>
        /// <returns>返回实体列表</returns>
        public Task<List<TEntity>> GetList(string p_keyOrSql, object p_params = null)
        {
            return _.Db.List<TEntity>(p_keyOrSql, p_params);
        }

        /// <summary>
        /// 以参数值方式执行Sql语句，返回实体枚举，未加载实体的附加数据！高性能
        /// </summary>
        /// <param name="p_keyOrSql">Sql字典中的键名(无空格) 或 Sql语句</param>
        /// <param name="p_params">参数值，支持Dict或匿名对象，默认null</param>
        /// <returns>返回实体枚举</returns>
        public Task<IEnumerable<TEntity>> ForEach(string p_keyOrSql, object p_params = null)
        {
            return _.Db.ForEach<TEntity>(p_keyOrSql, p_params);
        }

        /// <summary>
        /// 插入实体对象
        /// </summary>
        /// <param name="p_entity">待插入的实体</param>
        /// <param name="p_cudEvent">触发插入实体的事件类型，默认不触发</param>
        /// <returns>true 成功</returns>
        public async Task<bool> Insert(TEntity p_entity, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entity);
            int cnt = await _.Db.Exec(_model.SqlInsert, p_entity);
            if (cnt != 1)
                return false;

            if (_model.ExistChild)
            {
                foreach (var child in _model.Children)
                {
                    IEnumerable ls = child.PropInfo.GetValue(p_entity) as IEnumerable;
                    if (ls != null)
                    {
                        // 子实体列表
                        foreach (object obj in ls)
                        {
                            cnt = await _.Db.Exec(child.SqlInsert, obj);
                            if (cnt != 1)
                                throw new Exception("插入子实体对象失败");
                        }
                    }
                }
            }

            // 插入实体事件
            AddInsertEvent(p_entity, p_cudEvent);
            GatherEvents(p_entity);
            return true;
        }

        /// <summary>
        /// 批量插入实体对象
        /// </summary>
        /// <param name="p_entities">待插入的实体列表</param>
        /// <param name="p_cudEvent">触发插入实体的事件类型，默认不触发</param>
        /// <returns>true 成功</returns>
        public async Task<bool> BatchInsert(IEnumerable<TEntity> p_entities, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entities);
            foreach (var entity in p_entities)
            {
                if (!await Insert(entity, p_cudEvent))
                    throw new Exception($"插入实体{typeof(TEntity).Name }：{entity.ID} 时失败");
            }
            return true;
        }

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="p_entity">待更新的实体</param>
        /// <param name="p_cudEvent">触发更新实体的事件类型，默认不触发</param>
        /// <returns>true 成功</returns>
        public async Task<bool> Update(TEntity p_entity, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entity);

            // 跟踪时只更新改变的值，无跟踪时全更
            if (p_entity.IsTracking())
            {
                Type type = p_entity.GetType();
                StringBuilder updateVal = new StringBuilder();
                var schema = DbSchema.GetTableSchema(_model.TblName);
                foreach (var col in schema.Columns)
                {
                    // 无此列或值未改变的不更新
                    var pi = type.GetProperty(col.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pi == null || p_entity.IsOriginalVal(col.Name, pi.GetValue(p_entity)))
                        continue;

                    if (updateVal.Length > 0)
                        updateVal.Append(", ");
                    updateVal.Append(col.Name);
                    updateVal.Append("=@");
                    updateVal.Append(col.Name);
                }
                int cnt = await _.Db.Exec($"update `{_model.TblName}` set {updateVal} where id=@id", p_entity);
                if (cnt != 1)
                    return false;

                p_entity.StopTrack();
            }
            else if (await _.Db.Exec(_model.SqlUpdate, p_entity) != 1)
            {
                return false;
            }

            if (_model.ExistChild)
            {
                foreach (var child in _model.Children)
                {
                    // 删除原有
                    await _.Db.Exec(child.SqlDelete, new { parentid = p_entity.ID });
                    // 重新插入
                    IEnumerable ls = child.PropInfo.GetValue(p_entity) as IEnumerable;
                    if (ls != null)
                    {
                        foreach (object obj in ls)
                        {
                            if (await _.Db.Exec(child.SqlInsert, obj) != 1)
                                throw new Exception("插入子实体对象失败");
                        }
                    }
                }
            }

            // 更新实体事件
            AddUpdateEvent(p_entity, p_cudEvent);
            GatherEvents(p_entity);
            return true;
        }

        /// <summary>
        /// 批量更新实体对象
        /// </summary>
        /// <param name="p_entities">待更新的实体列表</param>
        /// <param name="p_cudEvent">触发更新实体的事件类型，默认不触发</param>
        /// <returns>true 成功</returns>
        public async Task<bool> BatchUpdate(IEnumerable<TEntity> p_entities, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entities);
            foreach (var entity in p_entities)
            {
                if (!await Update(entity, p_cudEvent))
                    throw new Exception($"更新实体{typeof(TEntity).Name }：{entity.ID} 时失败");
            }
            return true;
        }

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="p_entity">待删除的实体</param>
        /// <param name="p_cudEvent">触发删除实体的事件类型，默认不触发</param>
        /// <returns>true 删除成功</returns>
        public async Task<bool> Delete(TEntity p_entity, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entity);

            // 删除子实体依靠数据库的级联删除
            bool suc = await _.Db.Exec(_model.SqlDelete, p_entity) > 0;
            if (suc)
            {
                // 删除实体事件
                AddDeleteEvent(p_entity.ID, p_cudEvent);
                GatherEvents(p_entity);
            }
            return suc;
        }

        /// <summary>
        /// 根据主键删除实体对象
        /// </summary>
        /// <param name="p_id">主键</param>
        /// <param name="p_cudEvent">触发删除实体的事件类型，默认不触发</param>
        /// <returns>true 删除成功</returns>
        public async Task<bool> Delete(TKey p_id, CudEvent p_cudEvent = CudEvent.None)
        {
            // 删除子实体依靠数据库的级联删除
            bool suc = await _.Db.Exec(_model.SqlDelete, new { id = p_id }) > 0;
            if (suc)
                AddDeleteEvent(p_id, p_cudEvent);
            return suc;
        }

        /// <summary>
        /// 批量删除实体对象
        /// </summary>
        /// <param name="p_entities">待删除的实体列表</param>
        /// <param name="p_cudEvent">触发删除实体的事件类型，默认不触发</param>
        /// <returns>成功删除行数</returns>
        public async Task<int> BatchDelete(IEnumerable<TEntity> p_entities, CudEvent p_cudEvent = CudEvent.None)
        {
            Check.NotNull(p_entities);
            int cnt = 0;
            foreach (var entity in p_entities)
            {
                if (await Delete(entity, p_cudEvent))
                    cnt++;
            }
            return cnt;
        }

        /// <summary>
        /// 收集领域事件：插入实体事件
        /// </summary>
        /// <param name="p_entity"></param>
        /// <param name="p_cudEvent"></param>
        protected virtual void AddInsertEvent(TEntity p_entity, CudEvent p_cudEvent)
        {
            if (p_cudEvent != CudEvent.None)
            {
                var ev = new InsertEvent<TEntity, TKey> { ID = p_entity.ID };
                _.AddDomainEvent(new DomainEvent(p_cudEvent == CudEvent.Remote, ev));
            }
        }

        /// <summary>
        /// 收集领域事件：更新实体事件
        /// </summary>
        /// <param name="p_entity"></param>
        /// <param name="p_cudEvent"></param>
        protected virtual void AddUpdateEvent(TEntity p_entity, CudEvent p_cudEvent)
        {
            if (p_cudEvent != CudEvent.None)
            {
                var ev = new UpdateEvent<TEntity, TKey> { ID = p_entity.ID };
                _.AddDomainEvent(new DomainEvent(p_cudEvent == CudEvent.Remote, ev));
            }
        }

        /// <summary>
        /// 收集领域事件：删除实体事件
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_cudEvent"></param>
        protected virtual void AddDeleteEvent(TKey p_id, CudEvent p_cudEvent)
        {
            if (p_cudEvent != CudEvent.None)
            {
                var ev = new DeleteEvent<TEntity, TKey> { ID = p_id };
                _.AddDomainEvent(new DomainEvent(p_cudEvent == CudEvent.Remote, ev));
            }
        }

        /// <summary>
        /// 收集待发布的领域事件
        /// </summary>
        /// <param name="p_entity"></param>
        protected void GatherEvents(TEntity p_entity)
        {
            var events = p_entity.GetDomainEvents();
            if (events != null)
                _.AddDomainEvents(events);
        }
    }
}