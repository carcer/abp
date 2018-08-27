﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Tagging
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
        Task<List<Tag>> GetListAsync();

        Task<List<Tag>> GetListAsync(IEnumerable<Guid> ids);
    }
}
