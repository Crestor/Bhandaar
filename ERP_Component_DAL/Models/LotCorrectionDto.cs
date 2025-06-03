using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
   public  class LotCorrectionDto
    {

        public Guid LotBatchId { get; set; }
        public int CurrentQty { get; set; }
        public int CountedQty { get; set; }
    }
}
