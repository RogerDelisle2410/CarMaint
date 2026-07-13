using CarMaint.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarMaint.Controllers
{
    public class CustomerDatasController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // ---------------------------
        // LANGUAGE LOADER
        // ---------------------------
        private JObject LoadLang()
        {
            string lang = Request.Cookies["lang"]?.Value ?? "en";

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

        // ---------------------------
        // INDEX
        // ---------------------------
        public ActionResult Index(string searchString)
        {
            var customers = db.CustomerDatas.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(customers.OrderBy(c => c.Name).ToList());
        }

        // ---------------------------
        // DETAILS (with cars)
        // ---------------------------
        public ActionResult Details(int id)
        {
            var customer = db.CustomerDatas.Find(id);
            if (customer == null) return HttpNotFound();

            var cars = db.CarDatas.Where(c => c.CustomerId == id).ToList();

            var vm = new CustomerDetailsViewModel
            {
                Customer = customer,
                Cars = cars
            };

            return View(vm);
        }

        // ---------------------------
        // CREATE
        // ---------------------------
        public ActionResult Create()
        {
            return View(new CustomerData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerData customer)
        {
            JObject Lang = LoadLang();

            // VALIDATION
            if (string.IsNullOrWhiteSpace(customer.Name))
                ModelState.AddModelError("Name", Lang["name_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.Address))
                ModelState.AddModelError("Address", Lang["address_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.City))
                ModelState.AddModelError("City", Lang["city_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.PostalCode))
                ModelState.AddModelError("PostalCode", Lang["postalcode_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.Phone))
                ModelState.AddModelError("Phone", Lang["phone_required"].ToString());

            if (!ModelState.IsValid)
                return View(customer);

            db.CustomerDatas.Add(customer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ---------------------------
        // EDIT
        // ---------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = db.CustomerDatas.Find(id);
            if (customer == null) return HttpNotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerData customer)
        {
            JObject Lang = LoadLang();

            // VALIDATION
            if (string.IsNullOrWhiteSpace(customer.Name))
                ModelState.AddModelError("Name", Lang["name_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.Address))
                ModelState.AddModelError("Address", Lang["address_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.City))
                ModelState.AddModelError("City", Lang["city_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.PostalCode))
                ModelState.AddModelError("PostalCode", Lang["postalcode_required"].ToString());

            if (string.IsNullOrWhiteSpace(customer.Phone))
                ModelState.AddModelError("Phone", Lang["phone_required"].ToString());

            if (!ModelState.IsValid)
                return View(customer);

            // SAVE
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ---------------------------
        // DISPOSE
        // ---------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
