using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class CartItems
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; } // in percentage
        public decimal Total => Quantity * Price * (1 - Discount / 100);
        public decimal Price { get; set; }
    }
}
