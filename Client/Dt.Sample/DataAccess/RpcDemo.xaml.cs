﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2013-12-16 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Base;
using Dt.Core;
using Dt.Core.Rpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endregion

namespace Dt.Sample
{
    public partial class RpcDemo : PageWin
    {
        public RpcDemo()
        {
            InitializeComponent();
        }

        async void OnGetString(object sender, RoutedEventArgs e)
        {
            _tbInfo.Text = "返回：" + await AtTestRpc.GetString();
        }

        async void OnSetString(object sender, RoutedEventArgs e)
        {
            if (await AtTestRpc.SetString("abc"))
                _tbInfo.Text = "OnSetString成功";
        }

        async void OnServerStream(object sender, RoutedEventArgs e)
        {
            _tbInfo.Text = "ServerStream模式：";
            var reader = await AtTestRpc.OnServerStream("hello");
            while (await reader.MoveNext())
            {
                _tbInfo.Text += $"{Environment.NewLine}收到：{reader.Val<string>()}";
            }
            _tbInfo.Text += Environment.NewLine + "结束";
        }

        void OnClientStream(object sender, RoutedEventArgs e)
        {
            AtMsg.PushMsg(110, "abc");
            //_tbInfo.Text = "ClientStream模式未实现";
            //_tbInfo.Text = "ClientStream模式：";
            //var writer = await AtTestRpc.OnClientStream("hello");
            //int i = 0;
            //while (true)
            //{
            //    var msg = $"hello {i++}";
            //    if (!await writer.Write(msg))
            //        break;

            //    _tbInfo.Text += $"{Environment.NewLine}写入：{msg}";
            //    await Task.Delay(1000);
            //}
            //writer.Complete();
        }

        void OnDuplexStream(object sender, RoutedEventArgs e)
        {
            _tbInfo.Text = "DuplexStream模式未实现";
        }
    }

    internal static class AtTestRpc
    {
        #region TestRpc
        public static Task<string> GetString()
        {
            return new UnaryRpc(
                "cm",
                "TestRpc.GetString"
            ).Call<string>();
        }

        public static Task<bool> SetString(string p_str)
        {
            return new UnaryRpc(
                "cm",
                "TestRpc.SetString",
                p_str
            ).Call<bool>();
        }

        public static Task<ResponseReader> OnServerStream(string p_title)
        {
            return new ServerStreamRpc(
                "cm",
                "TestRpc.OnServerStream",
                p_title
            ).Call();
        }

        public static Task<RequestWriter> OnClientStream(string p_title)
        {
            return new ClientStreamRpc(
                "cm",
                "TestRpc.OnClientStream",
                p_title
            ).Call();
        }

        public static Task<DuplexStream> OnDuplexStream(string p_title)
        {
            return new DuplexStreamRpc(
                "cm",
                "TestRpc.OnDuplexStream",
                p_title
            ).Call();
        }
        #endregion
    }
}