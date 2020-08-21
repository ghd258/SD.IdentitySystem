﻿using SD.IdentitySystem.IAppService.DTOs.Outputs;
using SD.IdentitySystem.IAppService.Interfaces;
using SD.IdentitySystem.IPresentation.Interfaces;
using SD.IdentitySystem.IPresentation.ViewModels.Outputs;
using SD.IdentitySystem.Presentation.Maps;
using SD.Infrastructure.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using SD.FormatModel.EasyUI;

namespace SD.IdentitySystem.Presentation.Implements
{
    /// <summary>
    /// 角色呈现器实现
    /// </summary>
    public class RolePresenter : IRolePresenter
    {
        #region # 字段及构造器

        /// <summary>
        /// 权限服务接口
        /// </summary>
        private readonly IAuthorizationContract _authorizationContract;

        /// <summary>
        /// 用户服务接口
        /// </summary>
        private readonly IUserContract _userContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="authorizationContract">权限服务接口</param>
        /// <param name="userContract">用户服务接口</param>
        public RolePresenter(IAuthorizationContract authorizationContract, IUserContract userContract)
        {
            this._authorizationContract = authorizationContract;
            this._userContract = userContract;
        }

        #endregion

        #region # 获取角色 —— RoleView GetRole(Guid roleId)
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>角色</returns>
        public RoleView GetRole(Guid roleId)
        {
            RoleInfo roleInfo = this._authorizationContract.GetRole(roleId);

            return roleInfo.ToViewModel();
        }
        #endregion

        #region # 获取角色列表 —— IEnumerable<RoleView> GetRoles(string systemNo)
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <returns>角色列表</returns>
        public IEnumerable<RoleView> GetRoles(string systemNo)
        {
            IEnumerable<RoleInfo> roleInfos = this._authorizationContract.GetRoles(systemNo);

            IEnumerable<RoleView> roleViews = roleInfos.Select(x => x.ToViewModel());

            return roleViews;
        }
        #endregion

        #region # 获取用户角色列表 —— IEnumerable<RoleView> GetUserRoles(string loginId)
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="loginId">用户登录名</param>
        /// <returns>角色列表</returns>
        public IEnumerable<RoleView> GetUserRoles(string loginId)
        {
            IEnumerable<RoleInfo> roleInfos = this._userContract.GetUserRoles(loginId, null);
            IEnumerable<RoleView> roleViews = roleInfos.Select(x => x.ToViewModel());

            return roleViews;
        }
        #endregion

        #region # 获取信息系统/角色树 —— IEnumerable<Node> GetSystemRoleTree()
        /// <summary>
        /// 获取信息系统/角色树
        /// </summary>
        /// <returns>信息系统/角色树</returns>
        public IEnumerable<Node> GetSystemRoleTree()
        {
            IEnumerable<InfoSystemInfo> systems = this._authorizationContract.GetInfoSystems();

            IList<Node> tree = new List<Node>();

            foreach (InfoSystemInfo system in systems)
            {
                IEnumerable<RoleView> roles = this.GetRoles(system.Number);

                Node node = system.ToViewModel().ToNode(roles);
                tree.Add(node);
            }

            return tree;
        }
        #endregion

        #region # 获取用户的信息系统/角色树 —— IEnumerable<Node> GetUserSystemRoleTree(string loginId)
        /// <summary>
        /// 获取用户的信息系统/角色树
        /// </summary>
        /// <param name="loginId">用户登录名</param>
        /// <returns>信息系统/角色树</returns>
        public IEnumerable<Node> GetUserSystemRoleTree(string loginId)
        {
            //获取当前用户及其角色集
            IEnumerable<RoleView> userRoles = this.GetUserRoles(loginId).ToArray();

            //获取信息系统/角色树
            IEnumerable<Node> roleTree = this.GetSystemRoleTree().ToArray();

            //遍历信息系统
            foreach (Node system in roleTree)
            {
                //遍历角色
                foreach (Node role in system.children)
                {
                    //如果用户有该角色，则选中
                    if (userRoles.Any(x => x.Id == role.id))
                    {
                        role.@checked = true;
                    }
                }
            }

            return roleTree;
        }
        #endregion

        #region # 分页获取角色列表 —— PageModel<RoleView> GetRolesByPage(string systemNo...
        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>角色列表</returns>
        public PageModel<RoleView> GetRolesByPage(string systemNo, string keywords, int pageIndex, int pageSize)
        {
            PageModel<RoleInfo> pageModel = this._authorizationContract.GetRolesByPage(systemNo, keywords, pageIndex, pageSize);

            IEnumerable<RoleView> roleViews = pageModel.Datas.Select(x => x.ToViewModel());

            return new PageModel<RoleView>(roleViews, pageModel.PageIndex, pageModel.PageSize, pageModel.PageCount, pageModel.RowCount);
        }
        #endregion
    }
}
