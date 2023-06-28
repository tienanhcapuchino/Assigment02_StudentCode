using DataAccess.DataContext;
using BussinessObject.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EStoreDbContext _dbContext;
        public CategoryService(EStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Add(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAll()
        {
            var result = await _dbContext.Category.ToListAsync();
            return result;
        }

        public Task<Category> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }
    }
}
