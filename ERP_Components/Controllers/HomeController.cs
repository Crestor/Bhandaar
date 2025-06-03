//using ERP_Component_DAL.Models;
using ERP_Component_DAL.Models;
using ERP_Component_DAL.Services;
using ERP_Components.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;
using ERP_Components.Helper;

namespace ERP_Components.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices userServices;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _configuration = configuration;
            userServices = new UserServices(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }


        public  IActionResult ForgotPassword()
        {
            return View();
        }


        [SessionTimeout]
        public IActionResult AddDepartments()
        {
          var  dept=  userServices.GetDepartments();
            var numberedList = dept
    .Select((dept, index) => {
        dept.departmentSerialNumber = index + 1;
        return dept;
    })
    .ToList();

            return View(numberedList);
        }

        [SessionTimeout]
        [HttpPost]
        public IActionResult AddDepartments(Departments departments)
        {
            userServices.addDepartment(departments);
            var dept = userServices.GetDepartments();
            var numberedList = dept
.Select((dept, index) => {
dept.departmentSerialNumber = index + 1;
return dept;
})
.ToList();

            return View(numberedList);
        }

        [HttpGet]
        [SessionTimeout]
        public IActionResult EditDepartment(int departmentID)
        {
            var category = userServices.GetDepartmentbyID(departmentID);
            return View(category);
        }

     
        [SessionTimeout]
        [HttpPost]
        public IActionResult EditDepartment(Departments dept)
        {
            userServices.UpdateDepartment(dept);
            return RedirectToAction("AddDepartments");
        }
       
        
        
        
        
        
        [SessionTimeout]

        public IActionResult DeleteDepartment(int departmentId)
        {
            userServices.DeleteDepartment(departmentId);
            return RedirectToAction("AddDepartments");
        }




        [SessionTimeout]
        public IActionResult UpdateCredentials()
        {
            return View();
        }
        [SessionTimeout]
        public IActionResult UpdatePassword()
        {
            return View();
        }

        [SessionTimeout]
        public IActionResult SetPassword(User user)
        {
            user.userName = HttpContext.Session.GetString("UserName");
            var auth = userServices.UsernameExists(user.userName);


            if (auth)
            {
                HandleChangePassword(user);
            }


            return RedirectToAction("Logout");
        }

        [SessionTimeout]
        private JsonResult HandleChangePassword(User login)
        {

          

            bool isPasswordUpdated = userServices.UpdatePassword(login);
            if (isPasswordUpdated)
            {
                return Json(new { status = true, message = "Password updated successfully." });
            }



            return Json(new { status = false, message = "Failed to update password. Please try again." });


        }
        [SessionTimeout]
        public IActionResult UpdateUserName(User user)
        {
            user.userName = HttpContext.Session.GetString("UserName");
            var auth = userServices.UsernameExists(user.userName);

           
                if (auth)
                {
                    HandleChangeUsername(user);
                }
     

            return RedirectToAction("Logout");

        }

        [SessionTimeout]
        private JsonResult HandleChangeUsername(User login)
        {
            

            bool isUsernameUpdated = userServices.UpdateUsername(login);
            if (isUsernameUpdated)
            {
                HttpContext.Session.SetString("UserName", login.NewUsername);
                return Json(new { status = true, message = "Username updated successfully." });
            }

            return Json(new { status = false, message = "Failed to update username. Please try again." });
        }



        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return View("Index");
        }


        [HttpPost]
        public IActionResult Login(User users)
        {

            HttpContext.Session.Clear();
            var x = userServices.Authentication(users);

          users.EmployeeID = x.EmployeeID;
                
                if (x.role == 1 && x.userName == users.userName && x.password == users.password)
                {
                    
                    return HandleAdminLogin(users);
                }
               

                else if (x.role == 2 && x.userName == users.userName && x.password == users.password)
                {
                   
                    return HandleManagerLogin(users);
                }

            else if (users.role == 6 && x.userName == users.userName && x.password == users.password)
            {
                return HandleInventoryLogin(users);
            }
            else if (users.role == 7 && x.userName == users.userName && x.password == users.password)
                {
                    return HandleWarehouseLogin(users);
                } 
            //else if (users.role == 8 && x.userName == users.userName && x.password == users.password)
            //    {
            //        return HandleAssetLogin(users);
            //    }

            
            if (users.userName != null)
            {
                return Json(new { status = false, message = "Check UserName and Password " });

            }
            else
            {
                return Json(new { status = false, message = "Invalid Role!" });
            }

        }



        private JsonResult HandleAdminLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleAdmin(users);

            if (user != null && user.password == user.password && user.role == 2)
            {
                SetAdminSession(user);
                return Json(new { status = true, url = Url.Action("CheckerDashboard", "Dashboard") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleManagerLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleManager(users);

            if (user != null && user.password == user.password && user.role == 1)
            {
                SetManagerSession(user);
                return Json(new { status = true, url = Url.Action("MakerDashboard", "Dashboard") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }
        private JsonResult HandleInventoryLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            //var user = userServices.HandleUsers(users);

            if (users != null)
            {
                SetInventorySession(users);
                return Json(new { status = true, url = Url.Action("Dashboard", "Inventory") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleWarehouseLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            var user = userServices.HandleUsers(users);

            if (user != null && user.password == user.password && user.role == 7)
            {
                SetWarehouseSession(user);
                return Json(new { status = true, url = Url.Action("Dashboard", "Warehouse") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private JsonResult HandleAssetLogin(User users)
        {
            if (string.IsNullOrEmpty(users.userName))
            {
                return Json(new { status = false, message = "User does not exist, please enter a valid username" });
            }
            //SuperAdmin 
            //var user = userServices.HandleUsers(users);

            if (users != null)
            {
                SetAssetSession(users);
                return Json(new { status = true, url = Url.Action("Dashboard", "Asset") });
            }

            return Json(new { status = false, message = "Invalid credentials or you are not a registered Admin, Sign Up to use this service" });
        }

        private void SetAdminSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.EmployeeID));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Admin");
            
        }
        private void SetManagerSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.EmployeeID));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Manager");
           
        }
        private void SetInventorySession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.EmployeeID));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Inventory");
            
        }
        private void SetWarehouseSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.EmployeeID));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Warehouse");
            
        }
        private void SetAssetSession(User user)
        {
            HttpContext.Session.SetString("UserId", Convert.ToString(user.EmployeeID));
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("Role", "Asset");

        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
