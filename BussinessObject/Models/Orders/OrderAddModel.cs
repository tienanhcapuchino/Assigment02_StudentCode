using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class OrderAddModel
    {
        public string MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShipDate { get; set; }
        public double Freight { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
