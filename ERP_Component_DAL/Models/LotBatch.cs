using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public  class LotBatch
    {
        public Guid lotbatchid { get; set; }

        public Guid itemid { get; set; }

        public DateTime invoicedate     { get; set; }

        public decimal quantity { get; set; }

        public decimal Unitprice { get; set; }

        public string type { get; set; }

        public DateTime expirydate { get; set; }
        public DateTime stockDate { get; set; }


        public string perticulars { get; set; }

        public string lotseries { get; set; }

        public int AssignedQuantity { get; set; }

        public int InstockQuantity { get; set; }
        public int countedQuantity { get; set; }

        public int StockVariance { get; set; }
    }
}
