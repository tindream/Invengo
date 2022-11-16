﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;
using System.Xml;
using MQTTnet.Server;
using System.Threading.Tasks;
using MQTTnet.Protocol;
using MQTTnet;
using System.Net;
using Paway.Helper;
using MQTTnet.Internal;

namespace Paway.Comm
{
    /// <summary>
    /// 封装MQTT服务端
    /// </summary>
    public partial class MQTTService
    {
        #region 变量
        private MqttServer mqttServer = null;

        #endregion

        #region 外部方法
        /// <summary>
        /// 启动，完成后引发StartEvent
        /// </summary>
        /// <param name="port">服务端口</param>
        public virtual Task StartAsync(int port)
        {
            return CreateMQTTServer(port);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public Task StopAsync()
        {
            if (mqttServer == null) return CompletedTask.Instance;
            return mqttServer.StopAsync();
        }
        /// <summary>
        /// 获取客户端连接列表
        /// </summary>
        public IList<MqttClientStatus> Clients()
        {
            return mqttServer.GetClientsAsync().Result;
        }
        /// <summary>
        /// 发布消息
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        public virtual Task Publish(string topic, string data)
        {
            if (data == null) return CompletedTask.Instance;
            return Publish(topic, Encoding.UTF8.GetBytes(data));
        }
        /// <summary>
        /// 发布消息
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        public virtual Task Publish(string topic, byte[] buffer)
        {
            if (topic == null || buffer == null) return CompletedTask.Instance;
            var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(buffer).Build();
            return mqttServer.InjectApplicationMessage(new InjectedMqttApplicationMessage(message) { SenderClientId = "SenderClientId" });
        }
        /// <summary>
        /// 响应消息
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        protected void Response(ApplicationMessageNotConsumedEventArgs e, string data)
        {
            if (data == null) return;
            Response(e, Encoding.UTF8.GetBytes(data));
        }
        /// <summary>
        /// 响应消息
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        protected void Response(ApplicationMessageNotConsumedEventArgs e, byte[] buffer)
        {
            if (buffer == null) return;
            e.ApplicationMessage.Payload = buffer;
        }

        #endregion

        #region 接收处理
        /// <summary>
        /// 客户端发起连接前登陆验证
        /// </summary>
        protected virtual void LoginChecked(ValidatingConnectionEventArgs context) { }
        /// <summary>
        /// 客户端发起订阅主题通知
        /// </summary>
        protected virtual Task SubScribedTopicAsync(ClientSubscribedTopicEventArgs args) { return CompletedTask.Instance; }
        /// <summary>
        /// 客户端取消主题订阅通知
        /// </summary>
        protected virtual Task UnScribedTopicAsync(ClientUnsubscribedTopicEventArgs args) { return CompletedTask.Instance; }
        /// <summary>
        /// 客户端连接成功后的的处理通知
        /// </summary>
        protected virtual Task ClientConnectedAsync(ClientConnectedEventArgs args) { return CompletedTask.Instance; }
        /// <summary>
        /// 客户端断开连接通知
        /// </summary>
        protected virtual Task ClientDisConnectedAsync(ClientDisconnectedEventArgs args) { return CompletedTask.Instance; }
        /// <summary>
        /// 接收客户端发送的消息
        /// </summary>
        protected virtual Task MessageReceivedAsync(InterceptingPublishEventArgs args) { return CompletedTask.Instance; }

        #endregion

        #region 开启服务
        /// <summary>
        /// 开启服务
        /// </summary>
        private Task CreateMQTTServer(int port)
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port)//指定端口
                .Build();
            mqttServer = new MqttFactory().CreateMqttServer(options);
            //在 MqttServerOptions 选项中，你可以使用 ConnectionValidator 来对客户端连接进行验证。比如客户端ID标识 ClientId，用户名 Username 和密码 Password 等。
            mqttServer.ValidatingConnectionAsync += e =>
            {
                try
                {
                    e.ResponseUserProperties = new List<MQTTnet.Packets.MqttUserProperty>();
                    LoginChecked(e);
                }
                catch (Exception ex)
                {
                    e.ResponseUserProperties.Add(new MQTTnet.Packets.MqttUserProperty("error", ex.Message()));
                    e.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }
                return CompletedTask.Instance;
            };

            // 设置消息订阅通知
            mqttServer.ClientSubscribedTopicAsync += SubScribedTopicAsync;
            // 设置消息退订通知
            mqttServer.ClientUnsubscribedTopicAsync += UnScribedTopicAsync;
            // 设置客户端连接成功后的处理程序
            mqttServer.ClientConnectedAsync += ClientConnectedAsync;
            // 设置客户端断开后的处理程序
            mqttServer.ClientDisconnectedAsync += ClientDisConnectedAsync;
            // 设置消息处理程序
            mqttServer.InterceptingPublishAsync += MessageReceivedAsync;
            return mqttServer.StartAsync();
        }

        #endregion
    }
}
