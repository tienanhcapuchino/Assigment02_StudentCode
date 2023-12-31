﻿using BussinessObject.Entities;
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
        Task<List<ProductVM>> GetAllProducts();
        Task<bool> DeleteProduct(int id);
        Task<bool> AddProduct(ProductAddModel product);
        Task<bool> UpdateProduct(int productId, ProductUpdateModel model);
        Task<List<Product>> GetProductsByProductIds(List<int> productIds);
        Task<List<ProductVM>> SearchByName(string name);
        Task<List<ProductVM>> SearchByPrice(double? priceFrom, double? priceTo);
    }
}
