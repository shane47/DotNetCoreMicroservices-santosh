using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Contracts.Persistence;
using Orders.Domain.Entities;
using Orders.Infrastructure.Persistence;

namespace Orders.Infrastructure.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext){}
        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            return await dbContext.Orders.Where(o => o.UserName == userName)
                                         .ToListAsync();
        }
    }
}
