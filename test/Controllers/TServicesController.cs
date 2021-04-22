using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test;


namespace test.Controllers
{
    public class TServicesController : Controller
    {
        private CapstoneEntities db = new CapstoneEntities();

        // GET: TServices
        public ActionResult Index()
        {
            var tService = db.TServices.Include(t => t.TServiceType);
            return View(tService.ToList());
        }

        // GET: TServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TService tService = db.TServices.Find(id);
            if (tService == null)
            {
                return HttpNotFound();
            }
            return View(tService);
        }

        // GET: TServices/Create
        public ActionResult Create()
        {
            ViewBag.intServiceTypeID = new SelectList(db.TServiceTypes, "intServiceTypeID", "strServiceType");
            return View();
        }

        // POST: TServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "intServiceID,strServiceDesc,intServiceTypeID,dblPrice")] TService tService)
        {
            if (ModelState.IsValid)
            {
                db.TServices.Add(tService);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.intServiceTypeID = new SelectList(db.TServiceTypes, "intServiceTypeID", "strServiceType", tService.intServiceTypeID);
            return View(tService);
        }

        // GET: TServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TService tService = db.TServices.Find(id);
            if (tService == null)
            {
                return HttpNotFound();
            }
            ViewBag.intServiceTypeID = new SelectList(db.TServiceTypes, "intServiceTypeID", "strServiceType", tService.intServiceTypeID);
            return View(tService);
        }

        // POST: TServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "intServiceID,strServiceDesc,intServiceTypeID,dblPrice")] TService tService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tService).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.intServiceTypeID = new SelectList(db.TServiceTypes, "intServiceTypeID", "strServiceType", tService.intServiceTypeID);
            return View(tService);
        }

        // GET: TServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TService tService = db.TServices.Find(id);
            if (tService == null)
            {
                return HttpNotFound();
            }
            return View(tService);
        }

        // POST: TServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TService tService = db.TServices.Find(id);
            db.TServices.Remove(tService);
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
