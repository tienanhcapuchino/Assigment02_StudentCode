using AutoMapper;
using BussinessObject.Entities;
using BussinessObject.Models;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class OrderService : IOrderService
    {
        private readonly EStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public OrderService(EStoreDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> Add(Order entity, bool saveChange = true)
        {
            await _dbContext.AddAsync(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return await Task.FromResult(result);
        }

        public Task<bool> AddOrder(OrderAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Order entity, bool saveChange = true)
        {
            _dbContext.Orders.Remove(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            var orderEntity = await GetById(orderId);
            if (orderEntity.OrderDetails != null && orderEntity.OrderDetails.Any())
            {
                _dbContext.OrderDetails.RemoveRange(orderEntity.OrderDetails);
            }
            if (orderEntity == null) return false;
            bool result = await Delete(orderEntity, false);
            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<List<Order>> GetAll()
        {
            var orders = await _dbContext.Orders.Include(x => x.OrderDetails).ToListAsync();
            return orders;
        }

        public async Task<List<OrderVM>> GetAllOrders()
        {
            var orders = await GetAll();
            var result = _mapper.Map<List<OrderVM>>(orders);
            return result;
        }

        public async Task<Order> GetById(int id)
        {
            var order = await _dbContext.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.OrderId == id);
            return order;
        }

        public async Task<List<Order>> GetOrdersHistoryByUser(string userId)
        {
            var orders  = await _dbContext.Orders.Include(x => x.OrderDetails).Where(x => x.MemberId.Equals(userId)).ToListAsync();
            return orders;
        }

        public async Task<bool> Update(Order entity, bool saveChange = true)
        {
            _dbContext.Orders.Update(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return await Task.FromResult(result);
        }

        public async Task<bool> UpdateOrder(int orderId, OrderVM model)
        {
            var orderEntity = await GetById(orderId);
            if (orderEntity == null)
            {
                return false;
            }
            orderEntity.Freight = model.Freight;
            orderEntity.OrderDate = model.OrderDate;
            orderEntity.ShipDate = model.ShipDate;
            orderEntity.RequiredDate = model.RequiredDate;
            bool result = await Update(orderEntity);
            return result;
        }
    }
}
