﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoOrganizationUnitRepository : MongoDbRepository<IAbpIdentityMongoDbContext, OrganizationUnit, Guid>, IOrganizationUnitRepository
    {
        public MongoOrganizationUnitRepository(
            IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<OrganizationUnit>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ou => ou.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<OrganizationUnit>> GetListAsync(
            IEnumerable<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(t => ids.Contains(t.Id))
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<OrganizationUnit> GetOrganizationUnitAsync(
            string displayName,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(
                    ou => ou.DisplayName == displayName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public async Task<List<IdentityRole>> GetOrganizationUnitRoles(
            Guid organizationUnitId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var organizationUnit = await GetAsync(organizationUnitId, includeDetails, cancellationToken);
            var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id)).ToListAsync(cancellationToken);
        }
    }
}