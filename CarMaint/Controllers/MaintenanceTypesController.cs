using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarMaint.Models;

namespace CarMaint.Controllers
{
    public class MaintenanceTypesController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

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

        private void ApplyTaskNameTranslation(MaintenanceType item)
        {
            string lang = GetLang();

            if (lang == "fr")
                item.TaskName = item.TaskName_FR;
            else if (lang == "es")
                item.TaskName = item.TaskName_ES;
            else
                item.TaskName = item.TaskName_EN;
        }

        // ---------------------------
        // CONTROLLER ACTIONS
        // ---------------------------

        // GET: MaintenanceTypes
        public ActionResult Index(string SearchString)
        {
            var maint = db.MaintenanceTypes.ToList();

            // Translate first
            ApplyTaskNameTranslation(maint);

            // Filter AFTER translation
            if (!String.IsNullOrEmpty(SearchString))
            {
                maint = maint.Where(m => m.TaskName.ToLower().Contains(SearchString.ToLower())).ToList();
            }

            // Sort AFTER translation
            maint = maint.OrderBy(m => m.TaskName).ToList();

            return View(maint);
        }

        // GET: MaintenanceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceType maintenanceType = db.MaintenanceTypes.Find(id);

            if (maintenanceType == null)
                return HttpNotFound();

            ApplyTaskNameTranslation(maintenanceType);

            return View(maintenanceType);
        }

        // GET: MaintenanceTypes/Create
        public ActionResult Create()
        {
            return View(new MaintenanceType());
        }

        // POST: MaintenanceTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaintId,TaskName_EN,TaskName_FR,TaskName_ES,Cost,Gas,Diesel,Electric")] MaintenanceType maintenanceType)
        {
            if (ModelState.IsValid)
            {
                db.MaintenanceTypes.Add(maintenanceType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maintenanceType);
        }

        // GET: MaintenanceTypes/Edit/5
        public ActionResult Edit(int id)
        {
            var item = db.MaintenanceTypes.Find(id);

            if (item == null)
                return HttpNotFound();

            string lang = GetLang();

            switch (lang)
            {
                case "fr":
                    item.TaskName = item.TaskName_FR;
                    break;
                case "es":
                    item.TaskName = item.TaskName_ES;
                    break;
                default:
                    item.TaskName = item.TaskName_EN;
                    break;
            }

            return View(item);
        }

        // POST: MaintenanceTypes/Edit/5
        [HttpPost]
        public ActionResult Edit(MaintenanceType item)
        {
            string lang = GetLang();

            var dbItem = db.MaintenanceTypes.Find(item.MaintId);

            if (dbItem == null)
                return HttpNotFound();

            // Save TaskName to correct language field
            switch (lang)
            {
                case "fr":
                    dbItem.TaskName_FR = item.TaskName;
                    break;
                case "es":
                    dbItem.TaskName_ES = item.TaskName;
                    break;
                default:
                    dbItem.TaskName_EN = item.TaskName;
                    break;
            }

            // Save other fields
            dbItem.Cost = item.Cost;
            dbItem.Gas = item.Gas;
            dbItem.Diesel = item.Diesel;
            dbItem.Electric = item.Electric;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: MaintenanceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceType maintenanceType = db.MaintenanceTypes.Find(id);

            if (maintenanceType == null)
                return HttpNotFound();

            ApplyTaskNameTranslation(maintenanceType);

            return View(maintenanceType);
        }

        // POST: MaintenanceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MaintenanceType maintenanceType = db.MaintenanceTypes.Find(id);

            if (maintenanceType == null)
                return HttpNotFound();

            db.MaintenanceTypes.Remove(maintenanceType);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
