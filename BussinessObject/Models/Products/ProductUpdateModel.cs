﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class ProductUpdateModel
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
    }
}
