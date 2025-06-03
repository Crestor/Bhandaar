using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Product
    {
   
     public List<Items> items {  get; set; }
       public List<Warehouse> warehouse { get; set; }

       public List<Category> category {  get; set; }

        public List<Warrenty> warrenty { get; set; }

        public List<GST> gst { get; set; }

        public List<UOM> UOM { get; set; }
       public Stock stock {  get; set; }
       public Order order {  get; set; }

        public List<Departments> departments { get; set; }

        public List<LotBatch> lots { get; set; }


    }
}
