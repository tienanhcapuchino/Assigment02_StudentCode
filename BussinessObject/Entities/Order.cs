using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        [Column("MemberId")]
        public string MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShipDate { get; set; }
        public double Freight { get; set; }
        public virtual User User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
