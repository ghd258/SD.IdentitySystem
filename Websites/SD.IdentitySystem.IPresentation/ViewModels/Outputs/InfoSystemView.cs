﻿using SD.Infrastructure.Constants;
using SD.Infrastructure.PresentationBase;

namespace SD.IdentitySystem.IPresentation.ViewModels.Outputs
{
    /// <summary>
    /// 信息系统视图模型
    /// </summary>
    public class InfoSystemView : ViewModel
    {
        #region 管理员登录名 —— string AdminLoginId
        /// <summary>
        /// 管理员登录名
        /// </summary>
        public string AdminLoginId { get; set; }
        #endregion

        #region 应用程序类型 —— ApplicationType ApplicationType
        /// <summary>
        /// 应用程序类型
        /// </summary>
        public ApplicationType ApplicationType { get; set; }
        #endregion

        #region 主机名 —— string Host
        /// <summary>
        /// 主机名
        /// </summary>
        public string Host { get; set; }
        #endregion

        #region 端口 —— int? Port
        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }
        #endregion

        #region 首页 —— string Index
        /// <summary>
        /// 首页
        /// </summary>
        public string Index { get; set; }
        #endregion


        //Others

        #region 应用程序类型名称 —— string ApplicationTypeName
        /// <summary>
        /// 应用程序类型名称
        /// </summary>
        public string ApplicationTypeName { get; set; }
        #endregion
    }
}
