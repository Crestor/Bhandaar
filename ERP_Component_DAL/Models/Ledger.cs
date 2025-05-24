using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Ledger
    {

        public Guid ledgerId { get; set; }

        public Guid itemID { get; set; }
        public string  ItemName { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
