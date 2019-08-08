#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-06-21 创建
******************************************************************************/
#endregion

#region 引用命名
#endregion

namespace Dts.Core.Rpc
{
    /// <summary>
    /// 远程回调结果包装类
    /// </summary>
    internal class RpcResult
    {
        /// <summary>
        /// 结果类型
        /// </summary>
        public RpcResultType ResultType { get; set; }

        /// <summary>
        /// 结果值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string Elapsed { get; set; }

        /// <summary>
        /// 监控结果内容
        /// </summary>
        public string Trace { get; set; }
    }
}