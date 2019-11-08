#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-07-29 创建
******************************************************************************/
#endregion

#region 引用命名
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
#endregion

namespace Dt.Core.Rpc
{
    /// <summary>
    /// 基于Http2的请求/响应模式的远程调用
    /// </summary>
    public class UnaryRpc : BaseRpc
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="p_serviceName">服务名称</param>
        /// <param name="p_methodName">方法名</param>
        /// <param name="p_params">参数列表</param>
        public UnaryRpc(string p_serviceName, string p_methodName, params object[] p_params)
            : base(p_serviceName, p_methodName, p_params)
        { }

        /// <summary>
        /// 发送json格式的Http Rpc远程调用
        /// </summary>
        /// <typeparam name="T">结果对象的类型</typeparam>
        /// <returns>返回远程调用结果</returns>
        public async Task<T> Call<T>()
        {
            // 远程请求
            byte[] data = null;
            using (var request = CreateRequestMessage())
            using (var content = new PushStreamContent((ws) => RpcClientKit.WriteFrame(ws, _data, _isCompressed)))
            {
                request.Content = content;
                HttpResponseMessage response;
                try
                {
                    response = await _client.SendAsync(request).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"调用【{_methodName}】时服务器连接失败！\r\n{ex.Message}");
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
#if !SERVER
                    // 无权限时跳转到登录页面
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        AtSys.Login(true);
                        return default(T);
                    }
#endif
                    throw new Exception($"调用【{_methodName}】时返回状态码：{response.StatusCode}");
                }

                var stream = await response.Content.ReadAsStreamAsync();
                data = await RpcClientKit.ReadFrame(stream);
                response.Dispose();
            }

            // 解析结果
            T val = default(T);
            RpcResult result = new RpcResult();
            using (MemoryStream ms = new MemoryStream(data))
            using (StreamReader sr = new StreamReader(ms))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                try
                {
                    // [
                    reader.Read();
                    // 0成功，1错误，2警告提示
                    result.ResultType = (RpcResultType)reader.ReadAsInt32();
                    // 耗时，非调试状态为0
                    result.Elapsed = reader.ReadAsString();
                    if (result.ResultType == RpcResultType.Value)
                    {
                        reader.Read();
                        val = JsonRpcSerializer.Deserialize<T>(reader);
                    }
                    else
                    {
                        // 错误或提示信息
                        result.Info = reader.ReadAsString();
                    }

#if !SERVER
                    // 输出监视信息
                    string content = null;
                    if (AtSys.TraceRpc)
                    {
                        // 输出详细内容
                        ms.Position = 0;
                        content = sr.ReadToEnd();
                    }
                    AtKit.Trace(TraceOutType.RpcRecv, string.Format("{0}—{1}ms", _methodName, result.Elapsed), content, _svcName);
#endif
                }
                catch
                {
                    result.ResultType = RpcResultType.Error;
                    result.Info = "返回Json内容结构不正确！";
                }
            }

            if (result.ResultType == RpcResultType.Value)
                return val;

#if !SERVER
            if (result.ResultType == RpcResultType.Message)
                throw new FriendlyException(result.Info);
#endif

            throw new Exception($"调用【{_methodName}】异常：\r\n{result.Info}");
        }
    }
}