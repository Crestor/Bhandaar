using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class StocKRecords
    {
        public int id { get; set; }

        public string ItemName { get; set; }

        public string ItemCode { get; set; }

        public string Type { get; set; }

        public int Quantity { get; set; }

        public DateTime transcationDate { get; set; }

        public int stockiin { get; set; }

        public int stockout { get; set; }

        public string Perticulars { get; set; }



    }
}
