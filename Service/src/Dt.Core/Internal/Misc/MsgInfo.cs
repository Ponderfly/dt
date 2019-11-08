﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-09-05 创建
******************************************************************************/
#endregion

#region 引用命名
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
#endregion

namespace Dt.Core
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public class MsgInfo : IRpcJson
    {
        /// <summary>
        /// 在线时调用客户端的方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 在线时调用客户端方法的参数
        /// </summary>
        public List<object> Params { get; set; }

        /// <summary>
        /// 消息推送方式
        /// </summary>
        public MsgPushMode PushMode { get; set; }

        /// <summary>
        /// 离线推送时的消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 离线推送时的消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 校验消息内容是否有效
        /// </summary>
        /// <returns></returns>
        public string Validate()
        {
            if (PushMode == MsgPushMode.All)
            {
                if (string.IsNullOrEmpty(MethodName)
                    || string.IsNullOrEmpty(Title)
                    || string.IsNullOrEmpty(Content))
                    return "推送时MethodName, Title, Content不可为空！";
                return null;
            }

            if (PushMode == MsgPushMode.Online)
                return string.IsNullOrEmpty(MethodName) ? "在线推送的MethodName不可为空！" : null;

            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Content))
                return "离线推送时Title, Content不可为空！";
            return null;
        }

        /// <summary>
        /// 获取在线推送的内容
        /// </summary>
        /// <returns></returns>
        public string GetOnlineMsg()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sr = new StringWriter(sb))
            using (JsonWriter writer = new JsonTextWriter(sr))
            {
                writer.WriteStartArray();
                writer.WriteValue(MethodName);
                if (Params != null && Params.Count > 0)
                {
                    foreach (var par in Params)
                    {
                        JsonRpcSerializer.Serialize(par, writer);
                    }
                }
                writer.WriteEndArray();
                writer.Flush();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取Toast内容xml
        /// </summary>
        /// <returns></returns>
        public byte[] GetToastMsg()
        {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                writer.WriteStartElement("toast");

                // 启动参数
                if (!string.IsNullOrEmpty(MethodName))
                    writer.WriteAttributeString("launch", GetOnlineMsg());
                writer.WriteStartElement("visual");
                writer.WriteStartElement("binding");
                writer.WriteAttributeString("template", "ToastText02");

                writer.WriteStartElement("text");
                writer.WriteAttributeString("id", "1");
                writer.WriteValue(Title);
                writer.WriteEndElement();

                writer.WriteStartElement("text");
                writer.WriteAttributeString("id", "2");
                writer.WriteValue(Content);
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        #region IRpcJson
        void IRpcJson.ReadRpcJson(JsonReader p_reader)
        {
            MethodName = p_reader.ReadAsString();

            // 参数外层 [
            p_reader.Read();
            Params = new List<object>();
            while (p_reader.Read())
            {
                // 参数外层 ]
                if (p_reader.TokenType == JsonToken.EndArray)
                    break;
                Params.Add(JsonRpcSerializer.Deserialize(p_reader));
            }

            PushMode = (MsgPushMode)p_reader.ReadAsInt32();
            Title = p_reader.ReadAsString();
            Content = p_reader.ReadAsString();

            // 最外层 ]
            p_reader.Read();
        }

        void IRpcJson.WriteRpcJson(JsonWriter p_writer)
        {
            p_writer.WriteStartArray();
            p_writer.WriteValue("#msg");
            p_writer.WriteValue(MethodName);

            // 参数
            p_writer.WriteStartArray();
            if (Params != null && Params.Count > 0)
            {
                foreach (var par in Params)
                {
                    JsonRpcSerializer.Serialize(par, p_writer);
                }
            }
            p_writer.WriteEndArray();

            p_writer.WriteValue((int)PushMode);
            p_writer.WriteValue(Title);
            p_writer.WriteValue(Content);

            p_writer.WriteEndArray();
        }
        #endregion
    }

    /// <summary>
    /// 消息推送方式
    /// </summary>
    public enum MsgPushMode
    {
        /// <summary>
        /// 优先推送在线，不在线时离线推送
        /// </summary>
        All,

        /// <summary>
        /// 只推送在线
        /// </summary>
        Online,

        /// <summary>
        /// 只推送离线
        /// </summary>
        Offline
    }
}