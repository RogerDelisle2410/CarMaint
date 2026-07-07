using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarMaint.Models;

namespace CarMaint.Controllers
{
    public class CarDatasController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // GET: CarDatas
        public ActionResult Index(string searchString)
        {
            var carData = from s in db.CarDatas
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                carData = carData.Where(s => s.Manufacturer.Contains(searchString));
            }
            return View(carData.ToList().OrderBy(t => t.Manufacturer));
        }

        // GET: CarDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarData carData = db.CarDatas.Find(id);
            if (carData == null)
            {
                return HttpNotFound();
            }
            return View(carData);
        }

        // GET: CarDatas/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name");
            return View(new CarData());
        }

        // POST: CarDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,CustomerId,Manufacturer,Model,Year,EngineType,Mileage")] CarData carData)
        {
            if (ModelState.IsValid)
            {
                db.CarDatas.Add(carData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);
            return View(carData);
        }

        // GET: CarDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarData carData = db.CarDatas.Find(id);
            if (carData == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);
            return View(carData);
        }

        // POST: CarDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarId,CustomerId,Manufacturer,Model,Year,EngineType,Mileage")] CarData carData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.CustomerDatas, "CustomerId", "Name", carData.CustomerId);
            return View(carData);
        }

        // GET: CarDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarData carData = db.CarDatas.Find(id);
            if (carData == null)
            {
                return HttpNotFound();
            }
            return View(carData);
        }

        // POST: CarDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarData carData = db.CarDatas.Find(id);
            db.CarDatas.Remove(carData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
