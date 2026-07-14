using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarMaint.Models;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CarMaint.Controllers
{
    public class CarDatasController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // Load JSON language file
        private JObject LoadLang()
        {
            string lang = "en";

            if (Request.Cookies["lang"] != null)
                lang = Request.Cookies["lang"].Value;

            try
            {
                string jsonPath = Server.MapPath("~/Lang/" + lang + ".json");
                string jsonText = System.IO.File.ReadAllText(jsonPath);
                return JObject.Parse(jsonText);
            }
            catch
            {
                string fallbackPath = Server.MapPath("~/Lang/en.json");
                string fallbackText = System.IO.File.ReadAllText(fallbackPath);
                return JObject.Parse(fallbackText);
            }
        }

        // GET: CarDatas
        public ActionResult Index(string searchString)
        {
            var cars = db.CarDatas
                .Include(c => c.CustomerData);   // ⭐ FIX: eager load CustomerData

            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(c => c.Manufacturer.Contains(searchString));
            }

            return View(cars.ToList());  // ⭐ Only ONE SQL query
        } 

        // GET: CarDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CarData carData = db.CarDatas.Find(id);

            if (carData == null)
                return HttpNotFound();

            return View(carData);
        }

        // GET: CarDatas/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name");

            var brands = db.CarBrands
                .OrderBy(b => b.BrandName)
                .Select(b => new SelectListItem
                {
                    Text = b.BrandName,
                    Value = b.BrandName
                })
                .ToList();

            brands.Insert(0, new SelectListItem { Text = "Select", Value = "" });

            ViewBag.ManufacturerList = brands;

            return View(new CarData());
        }

        // POST: CarDatas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarData carData)
        {
            JObject Lang = LoadLang();

            // JSON-based validation
            if (carData.CustomerId == 0)
                ModelState.AddModelError("CustomerId", Lang["customerid_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Manufacturer))
                ModelState.AddModelError("Manufacturer", Lang["manufacturer_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Model))
                ModelState.AddModelError("Model", Lang["model_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Year))
                ModelState.AddModelError("Year", Lang["year_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.EngineType))
                ModelState.AddModelError("EngineType", Lang["enginetype_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Mileage))
                ModelState.AddModelError("Mileage", Lang["mileage_required"].ToString());

            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);

                var brands = db.CarBrands
                    .OrderBy(b => b.BrandName)
                    .Select(b => new SelectListItem
                    {
                        Text = b.BrandName,
                        Value = b.BrandName
                    })
                    .ToList();

                brands.Insert(0, new SelectListItem { Text = "Select", Value = "" });

                ViewBag.ManufacturerList = brands;

                return View(carData);
            }

            db.CarDatas.Add(carData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CarDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CarData carData = db.CarDatas.Find(id);

            if (carData == null)
                return HttpNotFound();

            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);

            var brands = db.CarBrands
                .OrderBy(b => b.BrandName)
                .Select(b => new SelectListItem
                {
                    Text = b.BrandName,
                    Value = b.BrandName
                })
                .ToList();

            brands.Insert(0, new SelectListItem { Text = "Select", Value = "" });

            ViewBag.ManufacturerList = brands;

            return View(carData);
        }

        // POST: CarDatas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarData carData)
        {
            JObject Lang = LoadLang();

            // JSON-based validation
            if (carData.CustomerId == 0)
                ModelState.AddModelError("CustomerId", Lang["customerid_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Manufacturer))
                ModelState.AddModelError("Manufacturer", Lang["manufacturer_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Model))
                ModelState.AddModelError("Model", Lang["model_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Year))
                ModelState.AddModelError("Year", Lang["year_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.EngineType))
                ModelState.AddModelError("EngineType", Lang["enginetype_required"].ToString());

            if (string.IsNullOrWhiteSpace(carData.Mileage))
                ModelState.AddModelError("Mileage", Lang["mileage_required"].ToString());

            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);

                var brands = db.CarBrands
                    .OrderBy(b => b.BrandName)
                    .Select(b => new SelectListItem
                    {
                        Text = b.BrandName,
                        Value = b.BrandName
                    })
                    .ToList();

                brands.Insert(0, new SelectListItem { Text = "Select", Value = "" });

                ViewBag.ManufacturerList = brands;

                return View(carData);
            }

            db.Entry(carData).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CarDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CarData carData = db.CarDatas.Find(id);

            if (carData == null)
                return HttpNotFound();

            return View(carData);
        }

        // POST: CarDatas/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult DeleteConfirmed(int id)
        ////{
        ////    CarData carData = db.CarDatas.Find(id);
        ////    db.CarDatas.Remove(carData);
        ////    db.SaveChanges();
        ////    return RedirectToAction("Index");
        ////}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
