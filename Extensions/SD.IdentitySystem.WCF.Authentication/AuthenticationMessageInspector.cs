﻿using SD.CacheManager;
using SD.Infrastructure;
using SD.Infrastructure.Constants;
using SD.Infrastructure.CustomExceptions;
using SD.Infrastructure.MemberShip;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace SD.IdentitySystem.WCF.Authentication
{
    /// <summary>
    /// WCF客户端/服务端身份认证消息拦截器
    /// </summary>
    internal class AuthenticationMessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region # 字段

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        #endregion

        #region # Implements of IDispatchMessageInspector

        /// <summary>
        /// 接收请求后事件
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="channel">信道</param>
        /// <param name="instanceContext">WCF实例上下文</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //获取消息头
            MessageHeaders headers = request.Headers;
            string action = headers.Action;

            EndpointDispatcher endpointDispatcher = OperationContext.Current.EndpointDispatcher;
            DispatchOperation operationDispatcher = endpointDispatcher.DispatchRuntime.Operations.Single(x => x.Action == action);

            /*
             * 通过OperationBehavior设置Impersonation属性，
             * 默认值为ImpersonationOption.NotAllowed，
             * 当ImpersonationOption.NotAllowed时验证身份，
             * 如无需验证身份，则将Impersonation属性赋值为ImpersonationOption.Allowed。
             */
            if (operationDispatcher.Impersonation == ImpersonationOption.NotAllowed)
            {
                #region # 验证消息头

                if (!headers.Any(x => x.Name == CommonConstants.WcfAuthHeaderName && x.Namespace == CommonConstants.WcfAuthHeaderNamespace))
                {
                    throw new NullReferenceException("身份认证消息头不存在，请检查程序！");
                }

                #endregion

                //读取消息头中的公钥
                Guid publicKey = headers.GetHeader<Guid>(CommonConstants.WcfAuthHeaderName, CommonConstants.WcfAuthHeaderNamespace);

                //认证
                lock (_Sync)
                {
                    //以公钥为键，查询分布式缓存，如果有值则通过，无值则不通过
                    LoginInfo loginInfo = CacheMediator.Get<LoginInfo>(publicKey.ToString());

                    if (loginInfo == null)
                    {
                        throw new NoPermissionException("身份过期，请重新登录！");
                    }

                    //通过后，重新设置缓存过期时间
                    int timeout = FrameworkSection.Setting.AuthenticationTimeout.Value.HasValue
                        ? FrameworkSection.Setting.AuthenticationTimeout.Value.Value
                        : 20;
                    CacheMediator.Set(publicKey.ToString(), loginInfo, DateTime.Now.AddMinutes(timeout));
                }
            }

            return null;
        }

        /// <summary>
        /// 响应请求前事件
        /// </summary>
        /// <param name="reply">响应消息</param>
        /// <param name="correlationState">相关状态</param>
        public void BeforeSendReply(ref Message reply, object correlationState) { }

        #endregion

        #region # Implements of IClientMessageInspector

        /// <summary>
        /// 请求发送前事件
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="channel">信道</param>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (OperationContext.Current != null)
            {
                //WCF客户端获取公钥处理
                MessageHeaders incomingHeaders = OperationContext.Current.IncomingMessageHeaders;

                #region # 验证消息头

                if (!incomingHeaders.Any(x => x.Name == CommonConstants.WcfAuthHeaderName && x.Namespace == CommonConstants.WcfAuthHeaderNamespace))
                {
                    throw new NullReferenceException("身份认证消息头不存在，请检查程序！");
                }

                #endregion

                //读取消息头中的公钥
                Guid publishKey = incomingHeaders.GetHeader<Guid>(CommonConstants.WcfAuthHeaderName, CommonConstants.WcfAuthHeaderNamespace);

                //添加消息头
                MessageHeader outgoingheader = MessageHeader.CreateHeader(CommonConstants.WcfAuthHeaderName, CommonConstants.WcfAuthHeaderNamespace, publishKey);
                request.Headers.Add(outgoingheader);
            }

            return null;
        }

        /// <summary>
        /// 请求响应后事件
        /// </summary>
        /// <param name="reply">响应消息</param>
        /// <param name="correlationState">相关状态</param>
        public void AfterReceiveReply(ref Message reply, object correlationState) { }

        #endregion
    }
}