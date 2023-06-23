using DataAccess.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class ProductService : IProductService
    {
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
            throw new NotImplementedException();
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
