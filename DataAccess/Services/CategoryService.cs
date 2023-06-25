using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EStoreDbContext _dbContext;
        public CategoryService(EStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetAll()
        {
            var result = _dbContext.Category.ToList();
            return result;
        }

        public Category GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Category entity, bool saveChange = true)
        {
            throw new NotImplementedException();
        }
    }
}
