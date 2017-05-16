﻿using SD.IdentitySystem.IAppService.Interfaces;
using SD.Infrastructure.Constants;
using System;

namespace SD.IdentitySystem.StubWCFServer.Stubs
{
    public class StubAuthenticationContract : IAuthenticationContract
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginId">登录名</param>
        /// <param name="password">密码
        /// </param><param name="ip">IP地址</param>
        /// <returns>登录信息</returns>
        public LoginInfo Login(string loginId, string password, string ip)
        {
            return null;
        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="publicKey">公钥</param>
        public void Authenticate(Guid publicKey)
        {

        }
    }
}