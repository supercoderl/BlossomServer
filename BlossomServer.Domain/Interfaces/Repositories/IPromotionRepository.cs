﻿using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IPromotionRepository : IRepository<Promotion, Guid>
    {
        Task<Promotion?> CheckByCode(string code);
    }
}
