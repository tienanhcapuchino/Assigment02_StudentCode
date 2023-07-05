using AutoMapper;
using DataAccess.DataContext;
using BussinessObject.Entities;
using DataAccess.Interfaces;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class ProductService : IProductService
    {
        private readonly EStoreDbContext _dbContext;
        private IMapper _mapper;
        public ProductService(EStoreDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<bool> Add(Product entity, bool saveChange = true)
        {
            await _dbContext.AddAsync(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        public async Task<bool> AddProduct(ProductAddModel product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return false;
            }
            Product entity = _mapper.Map<Product>(product);
            bool result = await Add(entity);
            return result;
        }

        public async Task<bool> Delete(Product entity, bool saveChange = true)
        {
            _dbContext.Remove(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await GetById(id);
            if (product == null)
            {
                return false;
            }
            bool result = await Delete(product);
            return result;
        }

        public async Task<List<Product>> GetAll()
        {
            var result = await _dbContext.Products.Include(x => x.Category).ToListAsync();
            return result;
        }

        public async Task<List<ProductVM>> GetAllProducts()
        {
            var products = await GetAll();
            var result = _mapper.Map<List<ProductVM>>(products);
            return result;
        }

        public async Task<Product> GetById(int id)
        {
            return await _dbContext.Products.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<List<Product>> GetProductsByProductIds(List<int> productIds)
        {
            var result = await _dbContext.Products.Where(x => productIds.Contains(x.ProductId)).ToListAsync();
            return result;
        }

        public async Task<bool> Update(Product entity, bool saveChange = true)
        {
            _dbContext.Update(entity);
            bool result = true;
            if (saveChange)
            {
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        public async Task<bool> UpdateProduct(int productId, ProductUpdateModel model)
        {
            if (string.IsNullOrEmpty(model.Name)) return false;
            var entity = await GetById(productId);
            if (entity == null) return false;
            entity.UnitPrice = model.UnitPrice;
            entity.UnitInStock = model.UnitInStock;
            entity.Name = model.Name;
            entity.Weight = model.Weight;
            bool result = await Update(entity);
            return result;
        }
    }
}
