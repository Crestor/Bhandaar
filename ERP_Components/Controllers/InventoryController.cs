using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Services;
//using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERP_Component_DAL.Models;
using System.Configuration;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using static Azure.Core.HttpHeader;
using Microsoft.Extensions.Logging;
using ERP_Components.Helper;
using DotNetEnv;
using Sprache;
using System.Xml.Linq;

namespace ERP_Components.Controllers
{

    [SessionTimeout]
    public class InventoryController : Controller
    {
      
        private readonly string jsonFilePath = "wwwroot/Json/city.json";
        private readonly ILogger<InventoryController> _logger;
        private readonly UserServices _userServices;
        private readonly InventoryServices inventoryServices;
        private readonly IConfiguration _configuration;
      


        public InventoryController(ILogger<InventoryController> logger, IConfiguration configuration)
        {
          
            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            inventoryServices = new InventoryServices(configuration);
        }
       
        public IActionResult Index()
        {
         
            return View("Dashboard");
        }

        public IActionResult Dashboard()
        {
            Dashboard d = new Dashboard();
       
            d.StocKRecords = inventoryServices.GetStockTransactionRecords();

            return View(d);
        }


        [HttpPost]
        public JsonResult AddUOM([FromBody] UOM uom)
        {
            try
            {
                // Save to database
                var newId = inventoryServices.SaveUOMToDatabase(uom); // implement this

                return Json(new { success = true, uomId = newId, uomName = uom.uomname });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult AddWarrenty([FromBody] Warrenty war)
        {
            try
            {
                // Save to database
                var newId = inventoryServices.AddWarrenty(war); // implement this

                return Json(new { success = true, id = war.warrentyID, name = war.PeriodName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //<----------------------Product-------------------->

        [HttpGet]
        public IActionResult Product()
        {
            var product = new List<Product>
    {
        new Product
        {

            category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
         
            UOM = inventoryServices.getProductUOM() ?? new List<UOM>(),


            //warrenty =  inventoryServices.getProductWarrenty() ?? new List<Warrenty>(),

            //gst = inventoryServices.getProductGST() ?? new List<GST>(),


        }
    };
            return View(product);
        }

        [HttpGet]
        public JsonResult SubCategoriesNames(int categoryId)
        {
            List<Category> data = inventoryServices.getSubCategoriesName(categoryId);
            return Json(data);
        }


        public IActionResult AddProduct(Items item)
        {
            inventoryServices.AddItem(item);
            return RedirectToAction("Product");
        }


        public IActionResult GetProductBasedonCategory(int SubCategoryID)
        {
            List<Items> item = inventoryServices.ViewProductByCategory(SubCategoryID);
            return Json(item);
        }



        public IActionResult StockTransfer()
        {
            var product = new List<Product>
    {
        new Product
        {

            items = inventoryServices.GetProductNamesFromItems() ?? new List<Items>(),
            //warehouse = inventoryServices.getWarehouseName() ?? new List<Warehouse>()

        }
    };
            return View(product);

        }

        public IActionResult getLotBatch(Guid itemid)
        {
           var lot= inventoryServices.GetLotBatch(itemid);
            return Json(lot);
        }

        public IActionResult ViewProduct()
        {
            List<Items> item = inventoryServices.ViewProduct();
            return View(item);
        }

        public IActionResult EditProduct(Guid itemId)
        {
            List<Category> category = inventoryServices.getProductCategoriesName();
            //List<Warehouse> warehouses = inventoryServices.getWarehouseName();
            var item = inventoryServices.GetProductData(itemId);


            item.categories = category;
            //item.Warehouse = warehouses;
            return View(item);
            
        }
        public IActionResult UpdateProduct(Items item)
        {
            inventoryServices.UpdateMaterialItem(item);
            inventoryServices.UpdateInventory(item);
            inventoryServices.UpdateProductPrice(item);

            return RedirectToAction("ViewProduct");
        }

        //<----------------------Material-------------------->
        public IActionResult Material()
        {
            //        var product = new List<Product>
            //{
            //    new Product
            //    {

            //        category = inventoryServices.getMaterialCategoriesName() ?? new List<Category>(),
            //        //warehouse = inventoryServices.getWarehouseName() ?? new List<Warehouse>()

            //    }


            //};


        var    Products = new List<ProuctsTest>
        {
            new ProuctsTest { Id = 1, Name = "Shampoo", Price = 150 },
            new ProuctsTest { Id = 2, Name = "Toothpaste", Price = 90 },
            new ProuctsTest { Id = 3, Name = "Soap", Price = 45 }
        };

            var product = new CreateSalesModel();
            product.Products = Products;


            return View(product);
        }

        public IActionResult AddMaterial(Items item)
        {
            inventoryServices.AddItem(item);
            return View();
        }


        public IActionResult ViewMaterial()
        {
          List<Items> item =  inventoryServices.ViewMaterial();
            return View(item);
        }

        public IActionResult EditMaterial(Guid itemId)
        {
            List<Category> category = inventoryServices.getMaterialCategoriesName();
            //List<Warehouse> warehouses = inventoryServices.getWarehouseName();


            var item = inventoryServices.GetMaterialData(itemId);


            item.categories = category;
            //item.Warehouse = warehouses;
            return View(item);

        }

        public IActionResult UpdateMaterial(Items item)
        {
            inventoryServices.UpdateMaterialItem(item);
            inventoryServices.UpdateInventory(item);
            

            return RedirectToAction("ViewMaterial");
        }




        //<---------------------Category------------>
        public IActionResult Category()
        {
            var categories=  inventoryServices.ViewProductCategory();
            var numberedList = categories
    .Select((dept, index) => {
        dept.categorySerialNumber = index + 1;
        return dept;
    })
    .ToList();

            return View(numberedList);

        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            inventoryServices.AddCategory(category);
            return RedirectToAction("Category");
        }

        public IActionResult ViewCategory()
        {
            List<Category> category = inventoryServices.ViewProductCategory();
            return View(category);
        }



        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            var category = inventoryServices.GetEditCategory(categoryId);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            inventoryServices.UpdateCategory(category);
            return RedirectToAction("Category");
        }

        public IActionResult DeleteCategory(int categoryId)
        {
            inventoryServices.DeleteCategory(categoryId);
            return RedirectToAction("Category");
        }



        //<-------------------SubCategory-------------->
        public IActionResult SubCategory()
        {

            ManageCategory Category = new ManageCategory();
            Category.Categories = inventoryServices.CategoryProductNames();
            var subCategory = inventoryServices.ViewSubCategory();
            var numberedList = subCategory
    .Select((dept, index) => {
        dept.categorySerialNumber = index + 1;
        return dept;
    })
    .ToList();

            Category.SubCategories = numberedList;       

            return View(Category);
        }

        public JsonResult CategoriesNames()
        {
            List<Category> data = inventoryServices.CategoryMaterialNames();
            return Json(data);
        }


        public JsonResult ViewMaterialSubCategories()
        {
            List<Category> data = inventoryServices.ViewMaterialSubCategory();
            return Json(data);
        }


        [HttpPost]
        public IActionResult AddSubCategory(Category category)
        {
            inventoryServices.AddSubCategory(category);
            return RedirectToAction("SubCategory");
        }

        public IActionResult ViewSubCategory()
        {
            List<Category> category = inventoryServices.ViewSubCategory();
            return View(category);
        }

        [HttpGet]
        public IActionResult EditSubCategory(int subCategoryId)
        {
            var category = inventoryServices.GetEditSubCategory(subCategoryId);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditSubCategory(Category category)
        {
            inventoryServices.UpdateSubCategory(category);
            return RedirectToAction("ViewSubCategory");
        }

        public IActionResult DeleteSubCategory(int subCategoryId)
        {
            inventoryServices.DeleteSubCategory(subCategoryId);
            return RedirectToAction("ViewSubCategory");
        }


        //<---------------------------Warehouse------------------>
        public IActionResult Warehouses()
        {
           
            return View();
        }


        [HttpPost]

        public IActionResult AddCity([FromBody] CityRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewCity))
                return BadRequest(new { success = false, message = "Invalid city data." });

            try
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var jsonObject = JObject.Parse(jsonData);

                JArray states = (JArray)jsonObject["states"];
                if (states == null || states.Count <= request.StateIndex)
                    return BadRequest(new { success = false, message = "Invalid state index." });

                JArray districts = (JArray)states[request.StateIndex]["districts"];
                if (districts == null || districts.Count <= request.DistrictIndex)
                    return BadRequest(new { success = false, message = "Invalid district index." });

                JArray cities = (JArray)districts[request.DistrictIndex]["cities"];
                if (!cities.Contains(request.NewCity))
                {
                    cities.Add(request.NewCity);
                    System.IO.File.WriteAllText(jsonFilePath, jsonObject.ToString());
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, message = "City already exists." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error: " + ex.Message });
            }
        }


        [HttpPost]

        public IActionResult AddWarehouses(Warehouse w)
        {
            inventoryServices.AddWarehouses(w);

            return RedirectToAction("Warehouses");
        }

        public IActionResult WarehouseView()
        {
            List<Warehouse> wh = inventoryServices.ViewWarehouse();
            return View(wh);
        }
        [HttpGet]
        public IActionResult EditWarehouse(int addressId)
        {
           var wh =  inventoryServices.GetWarehouse(addressId);
            return View(wh);
        }



        [HttpPost]
        public IActionResult EditWarehouse(Warehouse wh)
        {
            inventoryServices.UpdateWarehouse(wh);
            return RedirectToAction("WarehouseView");
        }

        public IActionResult DeleteWarehouse(int addressId)
        {
            inventoryServices.DeleteWarehouse(addressId);
            return RedirectToAction("WarehouseView");
        }

        //<--------------------opening stock ------------------->

        [HttpGet]
        public IActionResult OpeningStockEntryForm()
        {
            List<Items> item = inventoryServices.GetItemsNames();
            return View(item);
        }

        [HttpPost]
        public IActionResult OpeningStockEntryForm(Items item)
        {
            inventoryServices.OpeningStockEntryForm(item);
            return RedirectToAction("OpeningStockEntryForm");
        }


        //<--------------------Stock In ------------------->

        public IActionResult AddStock()
        {


            var product = new Product
            {
                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                warrenty = inventoryServices.getProductWarrenty() ?? new List<Warrenty>(),
                items = new List<Items>()
            };
            return View(product);
          
        }

        [HttpPost]
        public IActionResult AddStock(Items item)
        {
                item.SourceDC = Guid.Parse("8CA42812-F890-47D8-A5D8-794A6931E98B");
                item.DestinationDC = Guid.Parse("EF229FAC-4171-42D4-B0F1-5A4B8A5E22CB");

            item.CreatedBY = Guid.Parse(HttpContext.Session.GetString("UserId"));

            item.Perticulars = item.Perticulars + " From " + item.VendorName;
            inventoryServices.AddStock(item);
            var product = new Product
            {
                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                warrenty = inventoryServices.getProductWarrenty() ?? new List<Warrenty>(),
                items = new List<Items> { item }
            };
            return View(product);
          
        }

        public IActionResult StockOut()
        {

            var product = new Product
            {

                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                departments = _userServices.GetDepartments() ?? new List<Departments>(),
                items = new List<Items>()

            };
  
            return View(product);

        }

        [HttpPost]
        public IActionResult StockOut(Items item)
        {

            item.SourceDC = Guid.Parse("EF229FAC-4171-42D4-B0F1-5A4B8A5E22CB");
            inventoryServices.StockTransfer(item);
            inventoryServices.InventoryUpdate(item);
            item.Perticulars = item.Perticulars + " To " + item.VendorName;
            int totalQuantity = item.AssignedLots.Sum(l => l.AssignedQuantity);
            item.quantity = totalQuantity;
            var product = new Product
            {

                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                departments = _userServices.GetDepartments() ?? new List<Departments>(),
                items = new List<Items> { item }

            };

            return View(product);
       
        }




        public IActionResult ViewTransaction(Guid id, string filter)
        {
            var x = new List<StocKRecords>();


            if (id != Guid.Empty && filter != null)
            {
                 x = inventoryServices.GetStockTransactionRecordsByID(id, filter);
            }
            else if(id == Guid.Empty && filter != null)
            {
                 x = inventoryServices.GetStockTransactionRecords(filter);

            }
            else if (id == Guid.Empty && filter == null )
            {
                x = inventoryServices.GetStockTransactionRecords();
            }
                return View(x);
        }

        public IActionResult ViewInventoryData()
        {
          List<Items> item =  inventoryServices.ViewInventoryData();
            return View(item);
        }

        public IActionResult GetProductName(int SubCategoryID)
        {

            List<Items> data = inventoryServices.GetProductDataBasedonCategory(SubCategoryID);
            return Json(data);

        }

            


        [HttpGet]
        public JsonResult GetMaterialNamesFromItems()
        {
            List<Items> data = inventoryServices.GetMaterialNamesFromItems();
            return Json(data);
        }


        public IActionResult ItemReport()
        {

            var items = inventoryServices.GetItemsReport();
            return View(items);
        }
        public IActionResult InventoryReport()
        {

        var inventory=   inventoryServices.GetInventoryReport();
            return View(inventory);
        }


        //public IActionResult ViewStock()
        //{
        //    List<Items> stockList  = inventoryServices.ViewProductStock();

        //    return View(stockList);
        //}

        //public JsonResult ViewMaterialStock()
        //{
        //    List<Stock> data = inventoryServices.ViewMaterialStock();
        //    return Json(data);
        //}


        //public IActionResult EditStock(Guid stockId)
        //{
        //    // Get dropdown lists
        //    List<Items> items = inventoryServices.GetItemsNames();
        //    //List<Warehouse> warehouses = inventoryServices.getWarehouseName();


        //    var stock = inventoryServices.GetStock(stockId);


        //    stock.items = items;
        //    //stock.warehouse = warehouses;

        //    return View(stock);
        //}



        //[HttpPost]
        //public IActionResult UpdateStock(Stock stock)
        //{
        //    inventoryServices.UpdateStock(stock);
        //    return RedirectToAction("ViewStock");
        //}

        //public IActionResult DeleteStock(Guid stockId)
        //{
        //    inventoryServices.DeleteStock(stockId);
        //    return RedirectToAction("ViewStock");
        //}



        //<--------------------Stock Out----------->
       

        public IActionResult StockReturnUpdate(Asset asset)
        {

            inventoryServices.StockReturnUpdate(asset);
            return RedirectToAction("StockReturn");
    }
        public IActionResult stockReturn()
        {

            var category = inventoryServices.getProductCategoriesName() ?? new List<Category>();
            return View(category);
        }

        public IActionResult StockAssignmenttoUser(Items item)
        {

            item.SourceDC = Guid.Parse("EF229FAC-4171-42D4-B0F1-5A4B8A5E22CB");
            inventoryServices.StockTransfer(item);
            inventoryServices.InventoryUpdate(item);
            return RedirectToAction("StockTransfer");
        }


        public IActionResult StockAssignment(Asset asset)
        {
            //inventoryServices.StockTransfer(asset);
            //inventoryServices.InventoryUpdate(asset);
            return RedirectToAction("StockTransfer");
        }

        public IActionResult AddStockTransfer(Order order)
        {
            inventoryServices.AddStockTransfer(order);
            return RedirectToAction("StockTransfer");
        }

        public IActionResult ViewStockTransfer()
        {
          List<Order> order = inventoryServices.ViewStockTransfer();
            return View(order);
        }


        

        public IActionResult EditStockTransfer(Guid orderId)
        {
           
            List<Items> items = inventoryServices.GetItemsNames();
            //List<Warehouse> warehouses = inventoryServices.getWarehouseName();

           
            var order = inventoryServices.GetStockTransfer(orderId);

           
            order.items = items;
            //order.warehouse = warehouses;

            return View(order);
        }



      

        public IActionResult UpdateStockTransfer(Order order)
        {
            inventoryServices.UpdateStockTransfer(order);
           return RedirectToAction("ViewStockTransfer");
        }

        public IActionResult DeleteStockTransfer(Guid stockId)
        {
            inventoryServices.DeleteStockTransfer(stockId);
            return RedirectToAction("ViewStock");
        }

        //<-------------------Stock Adjustment--------------->


        public IActionResult ExpiryReport()
        {
            List<Items> item = inventoryServices.ExpiryReport();
            return View(item);
        }


        public IActionResult ReorderReport()
        {
            List<Items> item = inventoryServices.ReorderReport();
            return View(item);
        }

        public IActionResult AddStockAdjustment()
        {
            var product = new Product
            {

                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                items = new List<Items>()

            };
            return View(product);
        }

        [HttpPost]
        public IActionResult AddStockAdjustment(Items items )
        {
        
            items.SourceDC = Guid.Parse("EF229FAC-4171-42D4-B0F1-5A4B8A5E22CB");
            items.DestinationDC = Guid.Parse("EF229FAC-4171-42D4-B0F1-5A4B8A5E22CB");

            items.CreatedBY = Guid.Parse( HttpContext.Session.GetString("UserId"));
            inventoryServices.ApplyStockCorrectionsAsync(items);
            int totalQuantity = items.AssignedLots.Sum(l => l.StockVariance);
            items.quantity = totalQuantity;


            var product = new Product
            {

                category = inventoryServices.getProductCategoriesName() ?? new List<Category>(),
                items = new List<Items> { items }
            };
            return View(product);

        }

        public JsonResult GetCurrentStock(Guid itemId)
        {
            var CurrentStock = inventoryServices.GetCurrentStock(itemId);
            return Json(CurrentStock);
        }


      public IActionResult StockLedger()
        {
            LedgerDetails ledge = new LedgerDetails();
            ledge.Items = inventoryServices.GetItemsNames();
            ledge.ledger = new List<Items>();
            return View(ledge);
        }

        [HttpPost]
       public IActionResult StockLedger(Ledger ledger)
        {

            LedgerDetails ledge = new LedgerDetails();
           ledge.Items = inventoryServices.GetItemsNames();
            ledge.ledger = inventoryServices.GetStockTransaction(ledger);
            return View(ledge);
        }


        public IActionResult ViewStockLedger(Ledger ledger)
        {
            List<Items> stock = inventoryServices.GetStockTransaction(ledger);
            return View(stock);
        }

        public IActionResult StockAdjustment()
        {
          
            return View();
        }



       

    }
}