﻿using SD.IdentitySystem.IPresentation.Models;
using SD.Infrastructure.Constants;
using SD.Infrastructure.PresentationBase;
using SD.Toolkits.EasyUI;
using System;
using System.Collections.Generic;

namespace SD.IdentitySystem.IPresentation.Interfaces
{
    /// <summary>
    /// 菜单呈现器接口
    /// </summary>
    public interface IMenuPresenter : IPresenter
    {
        #region # 获取菜单 —— Menu GetMenu(Guid menuId)
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns>菜单</returns>
        Menu GetMenu(Guid menuId);
        #endregion

        #region # 获取菜单树 —— IEnumerable<Node> GetMenuTree(string infoSystemNo...
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="infoSystemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns>菜单树</returns>
        IEnumerable<Node> GetMenuTree(string infoSystemNo, ApplicationType? applicationType);
        #endregion

        #region # 获取用户菜单树 —— IEnumerable<Node> GetUserMenuTree(string loginId, string infoSystemNo...
        /// <summary>
        /// 获取用户菜单树
        /// </summary>
        /// <param name="loginId">用户名</param>
        /// <param name="infoSystemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns>菜单树</returns>
        IEnumerable<Node> GetUserMenuTree(string loginId, string infoSystemNo, ApplicationType? applicationType);
        #endregion

        #region # 获取菜单TreeGrid —— IEnumerable<Menu> GetMenuTreeGrid(string infoSystemNo...
        /// <summary>
        /// 获取菜单TreeGrid
        /// </summary>
        /// <param name="infoSystemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns>菜单TreeGrid</returns>
        IEnumerable<Menu> GetMenuTreeGrid(string infoSystemNo, ApplicationType? applicationType);
        #endregion
    }
}
