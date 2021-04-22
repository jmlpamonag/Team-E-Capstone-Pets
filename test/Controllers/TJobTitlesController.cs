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
    public class TJobTitlesController : Controller
    {
        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TJobTitles
        public ActionResult Index()
        {
            //return View(db.TJobTitles.ToList());
            return View();

        }


        public JsonResult GetJobTitles() {
            List<TJobTitle> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                dc.Configuration.ProxyCreationEnabled = false;

                var jobTitle = from a in dc.TJobTitles
                                   select new {
                                       a
                                   };

                if (jobTitle != null) {

                    all = new List<TJobTitle>();
                    foreach (var i in jobTitle) {

                        TJobTitle con = i.a;

                        all.Add(con);
                    }
                }
            }

            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Get Service Type by ID  
        public TJobTitle GetJobTitle(int intJobTitleID) {

            TJobTitle jobTitle = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TJobTitles
                         where a.intJobTitleID.Equals(intJobTitleID)
                         select new {
                             a
                         }).FirstOrDefault();

                if (v != null) {
                    jobTitle = v.a;
                }
                return jobTitle;
            }
        }

        //for get view for Save service type  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetJobTitle(id);

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
        public ActionResult Save(TJobTitle c) {

            string message = "";
            bool status = false;

            if (ModelState.IsValid) {

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intJobTitleID > 0) {
                        var v = dc.TJobTitles.Where(a => a.intJobTitleID.Equals(c.intJobTitleID)).FirstOrDefault();
                        if (v != null) {
                            v.strJobTitleDesc = c.strJobTitleDesc;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TJobTitles.Add(c);
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
                var c = GetJobTitle(id);
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
        public ActionResult Update(TJobTitle c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intJobTitleID > 0) {
                        var v = dc.TJobTitles.Where(a => a.intJobTitleID.Equals(c.intJobTitleID)).FirstOrDefault();
                        if (v != null) {
                            v.strJobTitleDesc = c.strJobTitleDesc;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TJobTitles.Add(c);
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

            var c = GetJobTitle(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST method for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteJobTitle(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TJobTitles.Where(a => a.intJobTitleID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TJobTitles.Remove(v);
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




























        //// GET: TJobTitles/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TJobTitle tJobTitle = db.TJobTitles.Find(id);
        //    if (tJobTitle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tJobTitle);
        //}

        //// GET: TJobTitles/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TJobTitles/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "intJobTitleID,strJobTitleDesc")] TJobTitle tJobTitle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TJobTitles.Add(tJobTitle);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tJobTitle);
        //}

        //// GET: TJobTitles/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TJobTitle tJobTitle = db.TJobTitles.Find(id);
        //    if (tJobTitle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tJobTitle);
        //}

        //// POST: TJobTitles/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "intJobTitleID,strJobTitleDesc")] TJobTitle tJobTitle)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tJobTitle).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tJobTitle);
        //}

        //// GET: TJobTitles/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TJobTitle tJobTitle = db.TJobTitles.Find(id);
        //    if (tJobTitle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tJobTitle);
        //}

        //// POST: TJobTitles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TJobTitle tJobTitle = db.TJobTitles.Find(id);
        //    db.TJobTitles.Remove(tJobTitle);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
