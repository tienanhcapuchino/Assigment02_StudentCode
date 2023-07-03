using BussinessObject.Entities;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IOrderDetailService : IRepository<OrderDetail>
    {
        Task<bool> AddOrderDetails(int orderId, List<OrderDetailVM> orderDetails);
        Task<bool> DeleteOrderDetails(int resourceId);
    }
}
