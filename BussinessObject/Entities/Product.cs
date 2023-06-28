using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
