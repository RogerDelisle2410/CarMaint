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
    public class CustomerDatasController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // GET: CustomerDatas

        public ActionResult Index(string searchString)
        {
            var customerData = from s in db.CustomerDatas
                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                customerData = customerData.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }
            return View(customerData.ToList().OrderBy(t => t.Name));
        }

        // GET: CustomerDatas/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CustomerData customerData = db.CustomerDatas.Find(id);
        //    if (customerData == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customerData);
        //}

        public ActionResult Details(int id)
        {
            var customer = db.CustomerDatas.Find(id);

            // Get cars for this customer
            var cars = db.CarDatas
                         .Where(c => c.CustomerId == id)
                         .ToList();

            // Build a view model
            var vm = new CustomerDetailsViewModel
            {
                Customer = customer,
                Cars = cars
            };

            return View(vm);
        }


        // GET: CustomerDatas/Create
        public ActionResult Create()
        {
            return View(new CustomerData());
        }

        // POST: CustomerDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,Name,Address,City,PostalCode,Phone")] CustomerData customerData)
        {
            if (ModelState.IsValid)
            {
                db.CustomerDatas.Add(customerData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerData);
        }

        // GET: CustomerDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerData customerData = db.CustomerDatas.Find(id);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // POST: CustomerDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,Name,Address,City,PostalCode,Phone")] CustomerData customerData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerData);
        }

        // GET: CustomerDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerData customerData = db.CustomerDatas.Find(id);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // POST: CustomerDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerData customerData = db.CustomerDatas.Find(id);
            db.CustomerDatas.Remove(customerData);
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
