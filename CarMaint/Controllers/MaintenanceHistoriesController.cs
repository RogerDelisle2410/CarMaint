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
    public class MaintenanceHistoriesController : Controller
    {
        private readonly BCATPEntities1 db = new BCATPEntities1();

        // ---------------------------
        // LANGUAGE HELPER
        // ---------------------------
        private string GetLang()
        {
            string lang = "en";
            if (Request.Cookies["lang"] != null)
                lang = Request.Cookies["lang"].Value;
            return lang;
        }

        // ---------------------------
        // TRANSLATE MAINTENANCE TYPE NAMES
        // ---------------------------
        private void TranslateMaintenanceTypes(IEnumerable<MaintenanceHistory> list)
        {
            string lang = GetLang();

            foreach (var h in list)
            {
                if (h.MaintenanceType != null)
                {
                    h.MaintenanceType.TaskName =
                        lang == "fr" ? h.MaintenanceType.TaskName_FR :
                        lang == "es" ? h.MaintenanceType.TaskName_ES :
                        h.MaintenanceType.TaskName_EN;
                }
            }
        }

        private void TranslateMaintenanceType(MaintenanceHistory h)
        {
            string lang = GetLang();

            if (h.MaintenanceType != null)
            {
                h.MaintenanceType.TaskName =
                    lang == "fr" ? h.MaintenanceType.TaskName_FR :
                    lang == "es" ? h.MaintenanceType.TaskName_ES :
                    h.MaintenanceType.TaskName_EN;
            }
        }

        // ---------------------------
        // GET: MaintenanceHistories
        // ---------------------------
        public ActionResult Index(string searchString)
        {
            var histories = db.MaintenanceHistories
                .Include(m => m.MaintenanceType)
                .Include(m => m.CustomerData)
                .Include(m => m.CarData)
                .ToList();

            // Translate maintenance type names
            TranslateMaintenanceTypes(histories);

            // Search by customer name (translated or not)
            if (!string.IsNullOrEmpty(searchString))
            {
                histories = histories
                    .Where(s => s.CustomerData.Name.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            return View(histories.OrderBy(t => t.CustId).ToList());
        }

        // ---------------------------
        // GET: MaintenanceHistories/Details/5
        // ---------------------------
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceHistory history = db.MaintenanceHistories
                .Include(m => m.MaintenanceType)
                .Include(m => m.CustomerData)
                .Include(m => m.CarData)
                .FirstOrDefault(m => m.HistoryId == id);

            if (history == null)
                return HttpNotFound();

            TranslateMaintenanceType(history);

            return View(history);
        }

        // ---------------------------
        // GET: MaintenanceHistories/Create
        // ---------------------------
        public ActionResult Create()
        {
            string lang = GetLang();

            var maintList = db.MaintenanceTypes.ToList();
            foreach (var m in maintList)
            {
                m.TaskName =
                    lang == "fr" ? m.TaskName_FR :
                    lang == "es" ? m.TaskName_ES :
                    m.TaskName_EN;
            }

            ViewBag.MaintId = new SelectList(maintList, "MaintId", "TaskName");
            ViewBag.CustId = new SelectList(db.CustomerDatas, "CustomerId", "Name");
            ViewBag.CarId = new SelectList(db.CarDatas, "CarId", "Manufacturer");

            return View(new MaintenanceHistory());
        }

        // ---------------------------
        // POST: MaintenanceHistories/Create
        // ---------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistoryId,CarId,MaintId,Date,CustId,Cost")] MaintenanceHistory history)
        {
            if (ModelState.IsValid)
            {
                db.MaintenanceHistories.Add(history);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Rebuild dropdowns if validation fails
            string lang = GetLang();

            var maintList = db.MaintenanceTypes.ToList();
            foreach (var m in maintList)
            {
                m.TaskName =
                    lang == "fr" ? m.TaskName_FR :
                    lang == "es" ? m.TaskName_ES :
                    m.TaskName_EN;
            }

            ViewBag.MaintId = new SelectList(maintList, "MaintId", "TaskName", history.MaintId);
            ViewBag.CustId = new SelectList(db.CustomerDatas, "CustomerId", "Name", history.CustId);
            ViewBag.CarId = new SelectList(db.CarDatas, "CarId", "Manufacturer", history.CarId);

            return View(history);
        }

        // ---------------------------
        // GET: MaintenanceHistories/Edit/5
        // ---------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceHistory history = db.MaintenanceHistories
                .Include(m => m.MaintenanceType)
                .Include(m => m.CustomerData)
                .Include(m => m.CarData)
                .FirstOrDefault(m => m.HistoryId == id);

            if (history == null)
                return HttpNotFound();

            string lang = GetLang();

            var maintList = db.MaintenanceTypes.ToList();
            foreach (var m in maintList)
            {
                m.TaskName =
                    lang == "fr" ? m.TaskName_FR :
                    lang == "es" ? m.TaskName_ES :
                    m.TaskName_EN;
            }

            ViewBag.MaintId = new SelectList(maintList, "MaintId", "TaskName", history.MaintId);
            ViewBag.CustId = new SelectList(db.CustomerDatas, "CustomerId", "Name", history.CustId);
            ViewBag.CarId = new SelectList(db.CarDatas, "CarId", "Manufacturer", history.CarId);

            return View(history);
        }

        // ---------------------------
        // POST: MaintenanceHistories/Edit/5
        // ---------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistoryId,CarId,MaintId,Date,CustId,Cost")] MaintenanceHistory history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Rebuild dropdowns if validation fails
            string lang = GetLang();

            var maintList = db.MaintenanceTypes.ToList();
            foreach (var m in maintList)
            {
                m.TaskName =
                    lang == "fr" ? m.TaskName_FR :
                    lang == "es" ? m.TaskName_ES :
                    m.TaskName_EN;
            }

            ViewBag.MaintId = new SelectList(maintList, "MaintId", "TaskName", history.MaintId);
            ViewBag.CustId = new SelectList(db.CustomerDatas, "CustomerId", "Name", history.CustId);
            ViewBag.CarId = new SelectList(db.CarDatas, "CarId", "Manufacturer", history.CarId);

            return View(history);
        }

        // ---------------------------
        // GET: MaintenanceHistories/Delete/5
        // ---------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MaintenanceHistory history = db.MaintenanceHistories
                .Include(m => m.MaintenanceType)
                .Include(m => m.CustomerData)
                .Include(m => m.CarData)
                .FirstOrDefault(m => m.HistoryId == id);

            if (history == null)
                return HttpNotFound();

            TranslateMaintenanceType(history);

            return View(history);
        }

        // ---------------------------
        // POST: MaintenanceHistories/Delete/5
        // ---------------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MaintenanceHistory history = db.MaintenanceHistories.Find(id);

            if (history != null)
            {
                db.MaintenanceHistories.Remove(history);
                db.SaveChanges();
            }

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
