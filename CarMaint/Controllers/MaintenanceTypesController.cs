using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarMaint.Models;
using Newtonsoft.Json.Linq;

namespace CarMaint.Controllers
{
    public class MaintenanceTypesController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // ---------------------------
        // LANGUAGE LOADER
        // ---------------------------
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

        // ---------------------------
        // CONTROLLER ACTIONS
        // ---------------------------

        // GET: MaintenanceTypes
        public ActionResult Index(string SearchString)
        {
            var maint = db.MaintenanceTypes.ToList();

            string lang = Request.Cookies["lang"]?.Value ?? "en";

            foreach (var m in maint)
            {
                if (lang == "fr") m.TaskName = m.TaskName_FR;
                else if (lang == "es") m.TaskName = m.TaskName_ES;
                else m.TaskName = m.TaskName_EN;
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                maint = maint.Where(m => m.TaskName.ToLower().Contains(SearchString.ToLower())).ToList();
            }

            return View(maint.OrderBy(m => m.TaskName).ToList());
        }

        // GET: MaintenanceTypes/Create
        public ActionResult Create()
        {
            return View(new MaintenanceType());
        }

        // POST: MaintenanceTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaintenanceType maintenanceType)
        {
            JObject Lang = LoadLang();

            // ---------------------------
            // JSON VALIDATION
            //----------------------------

            if (string.IsNullOrWhiteSpace(maintenanceType.TaskName_EN))
                ModelState.AddModelError("TaskName_EN", Lang["taskname_en_required"].ToString());

            if (string.IsNullOrWhiteSpace(maintenanceType.TaskName_FR))
                ModelState.AddModelError("TaskName_FR", Lang["taskname_fr_required"].ToString());

            if (string.IsNullOrWhiteSpace(maintenanceType.TaskName_ES))
                ModelState.AddModelError("TaskName_ES", Lang["taskname_es_required"].ToString());

            if (string.IsNullOrWhiteSpace(maintenanceType.Cost))
                ModelState.AddModelError("Cost", Lang["cost_required"].ToString());

            // At least one fuel type must be selected
            if (!maintenanceType.Gas && !maintenanceType.Diesel && !maintenanceType.Electric)
                ModelState.AddModelError("Gas", Lang["fuel_required"].ToString());

            if (!ModelState.IsValid)
                return View(maintenanceType);

            db.MaintenanceTypes.Add(maintenanceType);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: MaintenanceTypes/Edit/5
        public ActionResult Edit(int id)
        {
            var item = db.MaintenanceTypes.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        // POST: MaintenanceTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MaintenanceType item)
        {
            JObject Lang = LoadLang();
            string lang = Request.Cookies["lang"]?.Value ?? "en";

            // Validate ONLY the active language field
            if (lang == "en" && string.IsNullOrWhiteSpace(item.TaskName_EN))
                ModelState.AddModelError("TaskName_EN", Lang["taskname_en_required"].ToString());

            if (lang == "fr" && string.IsNullOrWhiteSpace(item.TaskName_FR))
                ModelState.AddModelError("TaskName_FR", Lang["taskname_fr_required"].ToString());

            if (lang == "es" && string.IsNullOrWhiteSpace(item.TaskName_ES))
                ModelState.AddModelError("TaskName_ES", Lang["taskname_es_required"].ToString());

            // Cost is string → validate properly
            if (string.IsNullOrWhiteSpace(item.Cost))
                ModelState.AddModelError("Cost", Lang["cost_required"].ToString());

            // At least one fuel type must be selected
            if (!item.Gas && !item.Diesel && !item.Electric)
                ModelState.AddModelError("Gas", Lang["fuel_required"].ToString());

            if (!ModelState.IsValid)
                return View(item);

            // SAVE
            var dbItem = db.MaintenanceTypes.Find(item.MaintId);
            if (dbItem == null)
                return HttpNotFound();

            dbItem.TaskName_EN = item.TaskName_EN;
            dbItem.TaskName_FR = item.TaskName_FR;
            dbItem.TaskName_ES = item.TaskName_ES;
            dbItem.Cost = item.Cost;
            dbItem.Gas = item.Gas;
            dbItem.Diesel = item.Diesel;
            dbItem.Electric = item.Electric;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var item = db.MaintenanceTypes.Find(id);
            if (item == null)
                return HttpNotFound();

            string lang = Request.Cookies["lang"]?.Value ?? "en";

            if (lang == "fr") item.TaskName = item.TaskName_FR;
            else if (lang == "es") item.TaskName = item.TaskName_ES;
            else item.TaskName = item.TaskName_EN;

            return View(item);
        }


        // GET: MaintenanceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceType maintenanceType = db.MaintenanceTypes.Find(id);
            if (maintenanceType == null) return HttpNotFound();

            string lang = Request.Cookies["lang"]?.Value ?? "en";

            if (lang == "fr") maintenanceType.TaskName = maintenanceType.TaskName_FR;
            else if (lang == "es") maintenanceType.TaskName = maintenanceType.TaskName_ES;
            else maintenanceType.TaskName = maintenanceType.TaskName_EN;

            return View(maintenanceType);
        }

        // POST: MaintenanceTypes/Delete/5
        ////[HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult DeleteConfirmed(int id)
        ////{
        ////    MaintenanceType maintenanceType = db.MaintenanceTypes.Find(id);
        ////    if (maintenanceType == null) return HttpNotFound();

        ////    db.MaintenanceTypes.Remove(maintenanceType);
        ////    db.SaveChanges();
        ////    return RedirectToAction("Index");
        ////}

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
