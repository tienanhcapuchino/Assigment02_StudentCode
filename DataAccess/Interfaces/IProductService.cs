using BussinessObject.Entities;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IProductService : IRepository<Product>
    {
        List<ProductVM> GetAllProducts();
        bool DeleteProduct(int id);
        bool AddProduct(ProductAddModel product);
    }
}
