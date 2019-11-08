#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-07-30 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Dt.Core.Rpc
{
    /// <summary>
    /// 客户端发送一个请求，服务端返回数据流的处理类
    /// </summary>
    class ServerStreamHandler : RpcHandler
    {
        public ServerStreamHandler(ApiInvoker p_invoker)
            : base(p_invoker)
        { }

        /// <summary>
        /// 调用服务方法
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> CallMethod()
        {
            try
            {
                // 补充参数
                List<object> objs = new List<object>();
                if (_invoker.Args != null && _invoker.Args.Length > 0)
                    objs.AddRange(_invoker.Args);
                objs.Add(new ResponseWriter(_invoker));

                await (Task)_invoker.Api.Method.Invoke(_tgt, objs.ToArray());
            }
            catch (Exception ex)
            {
                LogCallError(ex);
                return false;
            }
            return true;
        }
    }
}