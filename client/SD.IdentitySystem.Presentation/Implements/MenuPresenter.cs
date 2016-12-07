﻿using SD.IdentitySystem.IAppService.DTOs.Outputs;
using SD.IdentitySystem.IAppService.Interfaces;
using SD.IdentitySystem.IPresentation.Interfaces;
using SD.IdentitySystem.IPresentation.ViewModels.Formats.EasyUI;
using SD.IdentitySystem.IPresentation.ViewModels.Outputs;
using SD.IdentitySystem.Presentation.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.IdentitySystem.Presentation.Implements
{
    /// <summary>
    /// 菜单呈现器实现
    /// </summary>
    public class MenuPresenter : IMenuPresenter
    {
        #region # 字段及构造器

        /// <summary>
        /// 权限服务接口
        /// </summary>
        private readonly IAuthorizationContract _authorizationContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="authorizationContract">权限服务接口</param>
        public MenuPresenter(IAuthorizationContract authorizationContract)
        {
            this._authorizationContract = authorizationContract;
        }

        #endregion

        #region # 获取菜单列表 —— IEnumerable<MenuView> GetMenus(string systemNo)
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <returns>菜单列表</returns>
        public IEnumerable<MenuView> GetMenus(string systemNo)
        {
            IEnumerable<MenuInfo> menuInfos = this._authorizationContract.GetMenus(systemNo);

            IEnumerable<MenuView> menuViews = menuInfos.Select(x => x.ToViewModel());

            return menuViews;
        }
        #endregion

        #region # 获取菜单树 —— IEnumerable<Node> GetMenuTree(string systemNo)
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <returns>菜单树</returns>
        public IEnumerable<Node> GetMenuTree(string systemNo)
        {
            IEnumerable<MenuView> menuViews = this.GetMenus(systemNo);

            ICollection<Node> menuTree = menuViews.ToTree(null);

            return menuTree;
        }
        #endregion

        #region # 获取菜单 —— MenuView GetMenu(Guid menuId)
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <returns>菜单</returns>
        public MenuView GetMenu(Guid menuId)
        {
            MenuInfo menuInfo = this._authorizationContract.GetMenu(menuId);

            return menuInfo.ToViewModel();
        }
        #endregion
    }
}
