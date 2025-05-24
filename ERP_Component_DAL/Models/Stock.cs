using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Stock
    {
        public Guid itemId { get; set; }
        public Guid stockId { get; set; }

        public decimal costPrice { get; set; }
        public Guid purchaseId { get; set; }
        public Guid lotId { get; set; }

        public int gstrate { get; set; }
        public DateTime expiry { get; set; }
        public DateTime arrival { get; set; }

        public DateTime stockentryDate { get; set; }
       
      
 
        public string? itemName { get; set; }
        public string? itemType { get; set; }
        public string? lotSeries { get; set; }
        public decimal quantity { get; set; }

        public string perticular { get; set; }
        public Guid sourceId { get; set; }

        public Guid destinationID { get; set; }
        public decimal unitPrice { get; set; }
      

        public string? type { get; set; }


        public List<Items> items { get; set; }
        public List<Warehouse> warehouse { get; set; }
    }
}
