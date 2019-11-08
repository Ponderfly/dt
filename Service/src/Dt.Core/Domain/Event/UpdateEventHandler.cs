﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-11-07 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core.EventBus;
using System.Threading.Tasks;
#endregion

namespace Dt.Core.Domain
{
    /// <summary>
    /// 更新事件处理的默认基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class UpdateEventHandler<TEntity, TKey> : IRemoteHandler<UpdateEvent<TEntity, TKey>>, ILocalHandler<UpdateEvent<TEntity, TKey>>
        where TEntity : Root<TKey>
    {
        public virtual Task Handle(UpdateEvent<TEntity, TKey> p_event)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 更新事件处理的默认基类，实体ID类型为long
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class UpdateEventHandler<TEntity> : IRemoteHandler<UpdateEvent<TEntity>>, ILocalHandler<UpdateEvent<TEntity>>
        where TEntity : Root<long>
    {
        public virtual Task Handle(UpdateEvent<TEntity> p_event)
        {
            return Task.CompletedTask;
        }
    }
}