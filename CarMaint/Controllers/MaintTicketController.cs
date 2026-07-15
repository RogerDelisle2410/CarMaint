using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CarMaint.Models;
using System.Dynamic;

namespace CarMaint.Controllers
{
    public class MaintTicketController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();
        public decimal theCost;
        public int maId;

        // ---------------------------
        // LANGUAGE + TRANSLATION HELPERS
        // ---------------------------

        private string GetLang()
        {
            string lang = "en";
            if (Request.Cookies["lang"] != null)
                lang = Request.Cookies["lang"].Value;

            return lang;
        }

        private void ApplyTaskNameTranslation(IEnumerable<MaintenanceType> items)
        {
            string lang = GetLang();

            foreach (var m in items)
            {
                if (lang == "fr")
                    m.TaskName = m.TaskName_FR;
                else if (lang == "es")
                    m.TaskName = m.TaskName_ES;
                else
                    m.TaskName = m.TaskName_EN;
            }
        } 

        // ---------------------------
        // CONTROLLER ACTIONS
        // ---------------------------

        // GET: NewMaint
        public ActionResult Index(string searchString)
        {
            var customer = from s in db.CustomerDatas
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                customer = customer.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(customer.ToList().OrderBy(t => t.Name));
        }

        public ActionResult Select(int id)
        {
            var cust = db.CustomerDatas.Where(cu => cu.CustomerId == id);

            foreach (var item in cust)
            {
                Session["CustomerName"] = item.Name;
                Session["CustomerId"] = item.CustomerId;
            }

            var car = db.CarDatas.Where(ca => ca.CustomerId == id);

            return View(car.ToList().OrderBy(t => t.Manufacturer));
        }
        public ActionResult SelectCar(string engineType, int carId)
        {
            var car = db.CarDatas.Where(ca => ca.CarId == carId);

            foreach (var item in car)
            {
                Session["CarId"] = item.CarId;
                Session["Manufacturer"] = item.Manufacturer;
                Session["Model"] = item.Model;
                Session["EngineType"] = item.EngineType;
                Session["Year"] = item.Year;
            }

            var maint = db.MaintenanceTypes.AsQueryable();

            // Normalize engine type
            engineType = (engineType ?? "").Trim();

            // ================================
            // FIXED ENGINE FILTERING LOGIC
            // ================================
            switch (engineType)
            {
                case "Hybrid":
                    // Hybrid cars still have engines → Gas tasks apply
                    // Hybrid cars also have electric systems → Electric tasks apply
                    // Hybrid-specific tasks also apply
                    maint = maint.Where(ma => ma.Gas == true
                                           || ma.Electric == true
                                           || ma.Hybrid == true);
                    break;

                case "Turbo":
                    // Turbo cars are gas engines → Gas tasks apply
                    // Turbo-specific tasks also apply
                    maint = maint.Where(ma => ma.Gas == true
                                           || ma.Turbo == true);
                    break;

                case "Diesel":
                    maint = maint.Where(ma => ma.Diesel == true);
                    break;

                case "Electric":
                    maint = maint.Where(ma => ma.Electric == true);
                    break;

                default: // Gas
                    maint = maint.Where(ma => ma.Gas == true);
                    break;
            }

            // Translate names BEFORE sorting
            var list = maint.ToList();
            ApplyTaskNameTranslation(list);

            // Sort by translated name
            list = list.OrderBy(t => t.TaskName).ToList();

            return View(list);
        }

        //public ActionResult SelectCar(string engineType, int carId)
        //{
        //    var car = db.CarDatas.Where(ca => ca.CarId == carId);

        //    foreach (var item in car)
        //    {
        //        Session["CarId"] = item.CarId;
        //        Session["Manufacturer"] = item.Manufacturer;
        //        Session["Model"] = item.Model;
        //        Session["EngineType"] = item.EngineType;
        //        Session["Year"] = item.Year;
        //    }

        //    var maint = db.MaintenanceTypes.AsQueryable();

        //    switch (engineType)
        //    {
        //        case "Gas":
        //            maint = maint.Where(ma => ma.Gas == true);
        //            break;
        //        case "Diesel":
        //            maint = maint.Where(ma => ma.Diesel == true);
        //            break;
        //        case "Electric":
        //            maint = maint.Where(ma => ma.Electric == true);
        //            break;
        //        case "Hybrid":
        //            maint = maint.Where(ma => ma.Hybrid == true);
        //            break;
        //        case "Turbo":
        //            maint = maint.Where(ma => ma.Turbo == true);
        //            break;
        //    }

        //    // IMPORTANT: SQL cannot sort by TaskName (not a column)
        //    // So we translate first, then sort in memory.
        //    var list = maint.ToList();
        //    ApplyTaskNameTranslation(list);
        //    list = list.OrderBy(t => t.TaskName).ToList(); 

        //    return View(list);
        //}

        [HttpPost]
        public JsonResult AddToHistory(string[] arrayOfValues)
        {
            var cac = Session["CarId"];
            var cu = Session["CustomerId"];

            foreach (var it in arrayOfValues)
            {
                MaintenanceHistory maintenanceHistory = new MaintenanceHistory();
                var maId = Convert.ToInt16(it);

                var maintCost = db.MaintenanceTypes.Where(ma => ma.MaintId == maId);

                foreach (var item in maintCost)
                {
                    maintenanceHistory.Cost = Convert.ToDecimal(item.Cost);
                }

                maintenanceHistory.CarId = (int)cac;
                maintenanceHistory.CustId = (int)cu;
                maintenanceHistory.Date = DateTime.Now;
                maintenanceHistory.MaintId = maId;

                db.MaintenanceHistories.Add(maintenanceHistory);
                db.SaveChanges();
            }

            return Json(new
            {
                redirectUrl = Url.Action("Index", "Home"),
                isRedirect = true
            });
        }
    }
}
