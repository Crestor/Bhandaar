using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Items
    {
        //Items
        public Guid itemId {  get; set; }

        public Guid lotId { get; set; }
        public string? itemName { get; set; }

        public string itemCode { get; set; }
        public int itemType { get; set; }
     

        public string brand { get; set; }  



        public string? specification { get; set; }
        public int gst { get; set; }
        public string UOM { get; set; }
        public int categoryId {  get; set; }
        public int subCategoryId { get; set; }

        public string categoryName { get; set; }

        public string subCategoryName { get; set; }

        // Price

        public decimal unitPrice { get; set; }
     
     
        public decimal TotalPrice { get; set; }
       
        public string TransactionType { get; set; }

        public string TransactionReason { get; set; }
 
   
        public Guid employeeId{ get; set; }
      
        public string lotSeries{get;set;}
        public string  batchSeries{get;set;}
        public DateTime ExpiryDate { get; set; }

       public DateTime InvoiceDate { get; set; }
        public DateTime StockDate { get; set; }

        public string invoice { get; set; }


        public string type { get; set; }
     

        public string Perticulars { get; set; }
        //public string? warehouse { get; set; }

        public Guid inventoryId { get; set; }
      
        public int InstockQuantity { get; set; }
        public int CurrentQuantity { get; set; }

        public int countedQuantity { get; set;  }

        public int StockVariance { get; set; }
        public int quantity { get; set; }
        public int stockAlert { get; set; }

        public Guid SourceDC { get; set; }

        public Guid DestinationDC { get; set; }

        public List<Category> categories { get; set; }

        public Guid CreatedBY { get; set; }


        public int stockiin { get; set; }

        public int stockout { get; set; }

        public List<LotBatch> AssignedLots { get; set; }

        public decimal lotQunatity { get; set; }


        public string VendorName { get; set; }



    }
}
