﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class OrderDetailVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
