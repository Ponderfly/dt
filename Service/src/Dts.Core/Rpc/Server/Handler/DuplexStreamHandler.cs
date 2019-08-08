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

namespace Dts.Core.Rpc
{
    /// <summary>
    /// 客户端发送请求数据流，服务端返回数据流的处理类
    /// </summary>
    public class DuplexStreamHandler : RpcHandler
    {
        public DuplexStreamHandler(LobContext p_lc)
            : base(p_lc)
        { }

        /// <summary>
        /// 调用服务方法
        /// </summary>
        /// <returns></returns>
        protected override Task CallMethod()
        {
            try
            {
                // 补充参数
                List<object> objs = new List<object>();
                if (_lc.Args != null && _lc.Args.Length > 0)
                    objs.AddRange(_lc.Args);
                objs.Add(new RequestReader(_lc));
                objs.Add(new ResponseWriter(_lc));

                return (Task)_lc.Api.Method.Invoke(_tgt, objs.ToArray());
            }
            catch (Exception ex)
            {
                LogCallError(ex);
            }
            return Task.CompletedTask;
        }
    }
}