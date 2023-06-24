using AutoMapper;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Models;
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
            throw new NotImplementedException();
        }

        public bool Delete(Product entity, bool saveChange = true)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Update(Product entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }
    }
}
