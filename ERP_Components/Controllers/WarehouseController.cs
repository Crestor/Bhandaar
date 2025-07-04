﻿using ERP_Component_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using ERP_Component_DAL.Models;
using ERP_Components.Helper;


namespace ERP_Components.Controllers
{

    [SessionTimeout]
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly UserServices _userServices;

        private readonly WarehouseServices warehouseServices;
        private readonly IConfiguration _configuration;


        public WarehouseController(ILogger<WarehouseController> logger, IConfiguration configuration)
        {

            _logger = logger;
            _configuration = configuration;
            _userServices = new UserServices(configuration);
            warehouseServices = new WarehouseServices(configuration);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        //<---------------------------Warehouse Location------------------------->
        public IActionResult WarehouseLocation()
        {
            List<Warehouse> warehouseNames = warehouseServices.getWarehouseName();
            return View(warehouseNames);
        }

        public IActionResult AddWarehouseLocation(Warehouse warehouse)
        {
            warehouseServices.WarehouseLocation(warehouse);
            return RedirectToAction("WarehouseLocation");
        }

        public IActionResult WarehouseLocationView()
        {
            List<Warehouse> warehouse = warehouseServices.ViewWarehouseLocation();
            return View(warehouse);
        }


        public IActionResult EditWarehouseLocation(int locationId)
        {
            WarehouseNew wh = new WarehouseNew();
            wh.warehouseNames = warehouseServices.getWarehouseName();
            wh.warehouseLocation = warehouseServices.GetWarehouseLocation(locationId);
            //List<Warehouse> warehouseNames = warehouseServices.getWarehouseName();
            //warehouseServices.GetWarehouseLocation(locationId);

            return View(wh);
        }

        public IActionResult UpdateWarehouseLocation(Warehouse warehouse)
        {
            warehouseServices.UpdateWarehouseLocation(warehouse);
            return RedirectToAction("WarehouseLocationView");
        }

        public IActionResult DeleteWarehouseLocation(int locationId)
        {
            warehouseServices.DeleteWarehouseLocation(locationId);
            return RedirectToAction("WarehouseLocationView");
        }


        //<---------------------------------Stock Management---------------------------------->

        [HttpGet]
        public IActionResult AddWarehouseStock()
        
            {
                var product = new List<Product>
    {
        new Product
        {

            items = warehouseServices.GetItemsNames() ?? new List<Items>(),
            warehouse = warehouseServices.getWarehouseName() ?? new List<Warehouse>()

        }
    };
                return View(product);
            }

        public JsonResult GetItemSKU(Guid itemId)
        {
            var sku = warehouseServices.GetItemSKU(itemId);
            return Json(sku);
        }
        public JsonResult GetZone(Guid warehouseId)
        {
            List<Warehouse> zone = warehouseServices.GetWarehouseZoneNames(warehouseId);
            return Json(zone);
        }

        [HttpPost]
        public IActionResult AddWarehouseStock(Items item)
        {
            warehouseServices.AddWarehouseStockDetails(item);
            return RedirectToAction("AddWarehouseStock");
        }

        public IActionResult WarehouseStockView()
        {
         List<Warehouse> warehouse =   warehouseServices.WarehouseStockView();
            return View(warehouse);
        }


        public IActionResult EditWarehouseStock(Guid detailId)
        {

            WarehouseNew wh = new WarehouseNew();
            wh.itemNames =  warehouseServices.GetItemsNames();
          wh.warehouseNames =  warehouseServices.getWarehouseName();
            //wh.warehouseLocation = warehouseServices.GetWarehouseLocation(locationId);
            wh.warehouseStock = warehouseServices.getWarehouseStock(detailId);
            return View(wh);
        }

        public IActionResult UpdateWarehouseStock(Warehouse warehouse)
        {
            warehouseServices.UpdateWarehouseStock(warehouse);
            return RedirectToAction("WarehouseStockView");
        }

        public IActionResult DeleteWarehouseStock(Guid detailId)
        {
            warehouseServices.DeleteWarehouseStock(detailId);
            return RedirectToAction("WarehouseStockView");
        }

    }
    }

