﻿using SD.IdentitySystem.Domain.Entities;
using SD.IdentitySystem.Domain.IRepositories.Interfaces;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFramework;
using SD.Toolkits.EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SD.IdentitySystem.Repository.Implements
{
    /// <summary>
    /// 菜单仓储实现
    /// </summary>
    public class MenuRepository : EFAggRootRepositoryProvider<Menu>, IMenuRepository
    {
        #region # 分页获取菜单列表 —— ICollection<Menu> FindByPage(string keywords, string systemNo...
        /// <summary>
        /// 分页获取菜单列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>菜单列表</returns>
        public ICollection<Menu> FindByPage(string keywords, string systemNo, ApplicationType? applicationType, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            QueryBuilder<Menu> queryBuilder = QueryBuilder<Menu>.Affirm();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                queryBuilder.And(x => x.Keywords.Contains(keywords));
            }
            if (!string.IsNullOrWhiteSpace(systemNo))
            {
                queryBuilder.And(x => x.SystemNo == systemNo);
            }
            if (applicationType != null)
            {
                ApplicationType applicationType_ = applicationType.Value;
                queryBuilder.And(x => x.ApplicationType == applicationType_ || x.ApplicationType == ApplicationType.Complex);
            }

            Expression<Func<Menu, bool>> condition = queryBuilder.Build();
            IQueryable<Menu> menus = base.FindByPage(condition, pageIndex, pageSize, out rowCount, out pageCount);

            return menus.ToList();
        }
        #endregion

        #region # 获取菜单列表 —— ICollection<Menu> FindBySystem(string systemNo...
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="systemNo">信息系统编号</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns>菜单列表</returns>
        public ICollection<Menu> FindBySystem(string systemNo, ApplicationType? applicationType)
        {
            QueryBuilder<Menu> queryBuilder = QueryBuilder<Menu>.Affirm();
            if (!string.IsNullOrWhiteSpace(systemNo))
            {
                queryBuilder.And(x => x.SystemNo == systemNo);
            }
            if (applicationType != null)
            {
                ApplicationType applicationType_ = applicationType.Value;
                queryBuilder.And(x => x.ApplicationType == applicationType_ || x.ApplicationType == ApplicationType.Complex);
            }

            Expression<Func<Menu, bool>> condition = queryBuilder.Build();
            IQueryable<Menu> menus = base.Find(condition).OrderBy(x => x.Sort);

            return menus.ToList();
        }
        #endregion

        #region # 根据权限获取菜单列表 —— ICollection<Menu> FindByAuthority(IEnumerable<Guid>...
        /// <summary>
        /// 根据权限获取菜单列表
        /// </summary>
        /// <param name="authorityIds">权限Id集</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <returns>菜单列表</returns>
        public ICollection<Menu> FindByAuthority(IEnumerable<Guid> authorityIds, ApplicationType? applicationType)
        {
            Guid[] authorityIds_ = authorityIds?.Distinct().ToArray() ?? Array.Empty<Guid>();

            QueryBuilder<Menu> queryBuilder = QueryBuilder<Menu>.Affirm();
            if (authorityIds_.Any())
            {
                queryBuilder.And(x => x.Authorities.Any(y => authorityIds_.Contains(y.Id)));
            }
            if (applicationType != null)
            {
                ApplicationType applicationType_ = applicationType.Value;
                queryBuilder.And(x => x.ApplicationType == applicationType_);
            }

            Expression<Func<Menu, bool>> condition = queryBuilder.Build();
            IQueryable<Menu> menus = base.Find(condition).OrderBy(x => x.Sort);

            return menus.ToList();
        }
        #endregion

        #region # 是否存在菜单 —— bool Exists(Guid? parentNodeId, ApplicationType applicationType...
        /// <summary>
        /// 是否存在菜单
        /// </summary>
        /// <param name="parentNodeId">上级节点Id</param>
        /// <param name="applicationType">应用程序类型</param>
        /// <param name="menuName">菜单名称</param>
        /// <returns>是否存在</returns>
        public bool Exists(Guid? parentNodeId, ApplicationType applicationType, string menuName)
        {
            Expression<Func<Menu, bool>> condition;
            if (parentNodeId.HasValue)
            {
                Guid parentNodeId_ = parentNodeId.Value;
                condition =
                    x =>
                        x.ParentNode.Id == parentNodeId_ &&
                        x.ApplicationType == applicationType &&
                        x.Name == menuName;
            }
            else
            {
                condition =
                    x =>
                        x.IsRoot &&
                        x.ApplicationType == applicationType &&
                        x.Name == menuName;
            }

            return base.Exists(condition);
        }
        #endregion
    }
}