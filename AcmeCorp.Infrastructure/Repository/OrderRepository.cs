﻿using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Model;
using AcmeCorp.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Infrastructure.Repository
{

    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {

        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
