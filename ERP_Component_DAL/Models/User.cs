using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class User
    {

        public Guid EmployeeID { get; set; }
        public int userId {  get; set; }
        public string userName { get; set; }

        public string NewUsername { get; set; }
        public string contactNumber { get; set; } 
        public string? password { get; set; }

        public int role {  get; set; }

        public string Fullname { get; set; }

        public int departmentid { get; set; }
        public string department { get; set; }

        public string proofID { get; set; }

        public string iddetails { get; set; }
    }
}
