using BussinessObject.Entities;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IOrderService : IRepository<Order>
    {
        Task<bool> AddOrder(OrderAddModel model, string userId);
        Task<bool> UpdateOrder(int orderId, OrderVM model);
        Task<bool> DeleteOrder(int orderId);
        Task<List<OrderVM>> GetAllOrders();
        Task<List<Order>> GetOrdersHistoryByUser(string userId);
    }
}
