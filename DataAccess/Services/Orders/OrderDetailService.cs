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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly EStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public OrderDetailService(EStoreDbContext dbContext,
            IMapper map,
            IProductService productService)
        {
            _dbContext = dbContext;
            _mapper = map;
            _productService = productService;
        }
        public Task<bool> Add(OrderDetail entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddOrderDetails(int orderId, List<OrderDetailVM> orderDetails)
        {
            var listEntity = _mapper.Map<List<OrderDetail>>(orderDetails);
            foreach (var item in listEntity)
            {
                var product = await _productService.GetById(item.ProductId);
                item.OrderId = orderId;
                item.UnitPrice = product.UnitPrice;

                var existingEntity = _dbContext.OrderDetails
                    .FirstOrDefault(od => od.OrderId == item.OrderId && od.ProductId == item.ProductId);

                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(item);
                }
                else
                {
                    _dbContext.OrderDetails.Add(item);
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public Task<bool> Delete(OrderDetail entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOrderDetails(int resourceId)
        {
            var orderDetails = await _dbContext.OrderDetails
                .Where(x => x.ProductId == resourceId || x.OrderId == resourceId)
                .ToListAsync();
            _dbContext.OrderDetails.RemoveRange(orderDetails);
            return true;
        }

        public Task<List<OrderDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(OrderDetail entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }
    }
}
