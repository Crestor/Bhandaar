using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class CreateSalesModel
    {


    public string CustomerName { get; set; }


    public string ContactNumber { get; set; }

    public List<CartItems> CartItems { get; set; } = new List<CartItems> { new CartItems() };

    public List<ProuctsTest> Products { get; set; }
}
}