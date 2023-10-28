﻿using System;
using System.Collections.Generic;

namespace Estore2API.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public float? Weight { get; set; }
        public int UnitPrice { get; set; }
        public int UnitInStock { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
