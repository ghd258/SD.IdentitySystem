﻿using SD.IdentitySystem.Domain.Entities;
using SD.Infrastructure.DomainServiceBase;
using System;

namespace SD.IdentitySystem.Domain.IDomainServices
{
    /// <summary>
    /// 角色领域服务接口
    /// </summary>
    public interface IRoleService : IDomainService<Role>
    {
        #region # 是否存在角色 —— bool ExistsRole(string systemNo, Guid? roleId...
        /// <summary>
        /// 是否存在角色
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>是否存在</returns>
        bool ExistsRole(string systemNo, Guid? roleId, string roleName);
        #endregion
    }
}
