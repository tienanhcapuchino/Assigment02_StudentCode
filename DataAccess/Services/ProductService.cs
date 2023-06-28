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
        public bool Add(Product entity, bool saveChange = true)
        {
            _dbContext.Add(entity);
            bool result = true;
            if (saveChange)
            {
                result = _dbContext.SaveChanges() > 0;
            }
            return result;
        }

        public bool AddProduct(ProductAddModel product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return false;
            }
            Product entity = _mapper.Map<Product>(product);
            bool result = Add(entity);
            return result;
        }

        public bool Delete(Product entity, bool saveChange = true)
        {
            _dbContext.Remove(entity);
            bool result = true;
            if (saveChange)
            {
                result = _dbContext.SaveChanges() > 0;
            }
            return result;
        }

        public bool DeleteProduct(int id)
        {
            var product = GetById(id);
            if (product == null)
            {
                return false;
            }
            bool result = Delete(product);
            return result;
        }

        public List<Product> GetAll()
        {
            var result = _dbContext.Products.Include(x => x.Category).ToList();
            return result;
        }

        public List<ProductVM> GetAllProducts()
        {
            var products = GetAll();
            var result = _mapper.Map<List<ProductVM>>(products);
            return result;
        }

        public Product GetById(int id)
        {
            return _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
        }

        public bool Update(Product entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }
    }
}
