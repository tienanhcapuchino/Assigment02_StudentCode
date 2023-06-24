using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ProductAddModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
    }
}
