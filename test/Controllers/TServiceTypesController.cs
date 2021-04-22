using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test;


namespace test.Controllers {

    public class TServiceTypesController : Controller {

        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TServiceTypes
        public ActionResult Index() {

            return View();
        }

        public JsonResult GetServiceTypes() {
            List<TServiceType> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                dc.Configuration.ProxyCreationEnabled = false;

                var serviceTypes = from a in dc.TServiceTypes
                                   select new {
                                       a
                                   };

				if (serviceTypes != null) {

					all = new List<TServiceType>();
					foreach (var i in serviceTypes) {

						TServiceType con = i.a;

						all.Add(con);
					}
				}
			}

			return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
        



        //Get Service Type by ID  
        public TServiceType GetServiceType(int intServiceTypeID) {

            TServiceType serviceTypes = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TServiceTypes 
                         where a.intServiceTypeID.Equals(intServiceTypeID)
                         select new { 
                            a }).FirstOrDefault();

                if (v != null) {
                    serviceTypes = v.a;
                }
                return serviceTypes;
            }
        }

        //for get view for Save service type  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetServiceType(id);

                if (c == null) {

                    return HttpNotFound();
                }
                else

                    return PartialView("Save", c);
            }
            else {
                return PartialView("Save");
            }
        }

        //for POST method for Save records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TServiceType c) {

            string message = "";
            bool status = false;

			if (ModelState.IsValid) {

				using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intServiceTypeID > 0) {
                        var v = dc.TServiceTypes.Where(a => a.intServiceTypeID.Equals(c.intServiceTypeID)).FirstOrDefault();
                        if (v != null) {
                            v.strServiceType = c.strServiceType;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TServiceTypes.Add(c);
                    }
                    dc.SaveChanges();
                    status = true;
                    message = "Data Is Successfully Saved.";
                }
			}
			else {
				message = "Error! Please try again.";
			}

			return new JsonResult { Data = new { status = status, message = message } };
        }

        //for get view for update service type
        public ActionResult Update(int id = 0) {

            if (id > 0) {
                var c = GetServiceType(id);
                if (c == null) {
                    return HttpNotFound();
                }
                else
                    return PartialView("Update", c);
            }
            else {
                return PartialView("Update");
            }
        }

        //for POST method for update records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(TServiceType c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intServiceTypeID > 0) {
                        var v = dc.TServiceTypes.Where(a => a.intServiceTypeID.Equals(c.intServiceTypeID)).FirstOrDefault();
                        if (v != null) {
                            v.strServiceType = c.strServiceType;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TServiceTypes.Add(c);
                    }
                    dc.SaveChanges();
                    status = true;
                    message = "Data Is Successfully Updated.";
                }
            }
            else {
                message = "Error! Please try again.";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }

        //For delete records view
        public ActionResult Delete(int id) {

            var c = GetServiceType(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST method for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteServiceType(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TServiceTypes.Where(a => a.intServiceTypeID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TServiceTypes.Remove(v);
                    dc.SaveChanges();
                    status = true;
                    message = "Data Is Successfully Deleted!";
                }
                else {
                    return HttpNotFound();
                }
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }
    }
}



    // GET: TServiceTypes/Details/5
    //public ActionResult Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        TServiceType tServiceType = db.TServiceTypes.Find(id);
    //        if (tServiceType == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(tServiceType);
    //    }

    //    // GET: TServiceTypes/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: TServiceTypes/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "intServiceTypeID,strServiceType")] TServiceType tServiceType)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.TServiceTypes.Add(tServiceType);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(tServiceType);
    //    }

    //    // GET: TServiceTypes/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        TServiceType tServiceType = db.TServiceTypes.Find(id);
    //        if (tServiceType == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(tServiceType);
    //    }

    //    // POST: TServiceTypes/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "intServiceTypeID,strServiceType")] TServiceType tServiceType)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(tServiceType).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(tServiceType);
    //    }

    //    // GET: TServiceTypes/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        TServiceType tServiceType = db.TServiceTypes.Find(id);
    //        if (tServiceType == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(tServiceType);
    //    }

    //    // POST: TServiceTypes/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        TServiceType tServiceType = db.TServiceTypes.Find(id);
    //        db.TServiceTypes.Remove(tServiceType);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    
