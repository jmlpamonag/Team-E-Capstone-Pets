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
    public class TMethodsController : Controller
    {

        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TMethods
        public ActionResult Index() {
            //return View(db.TMethods.ToList());
            return View();

        }


        public JsonResult GetMethods() {
            List<TMethod> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                dc.Configuration.ProxyCreationEnabled = false;

                var method = from a in dc.TMethods
                             select new {
                                 a
                             };

                if (method != null) {

                    all = new List<TMethod>();
                    foreach (var i in method) {

                        TMethod con = i.a;

                        all.Add(con);
                    }
                }
            }

            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Get Service Type by ID  
        public TMethod GetMethod(int intMethodID) {

            TMethod method = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TMethods
                         where a.intMethodID.Equals(intMethodID)
                         select new {
                             a
                         }).FirstOrDefault();

                if (v != null) {
                    method = v.a;
                }
                return method;
            }
        }

        //for get view for Save service type  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetMethod(id);

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
        public ActionResult Save(TMethod c) {

            string message = "";
            bool status = false;

            if (ModelState.IsValid) {

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intMethodID > 0) {
                        var v = dc.TMethods.Where(a => a.intMethodID.Equals(c.intMethodID)).FirstOrDefault();
                        if (v != null) {
                            v.strMethod = c.strMethod;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TMethods.Add(c);
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
                var c = GetMethod(id);
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
        public ActionResult Update(TMethod c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intMethodID > 0) {
                        var v = dc.TMethods.Where(a => a.intMethodID.Equals(c.intMethodID)).FirstOrDefault();
                        if (v != null) {
                            v.strMethod = c.strMethod;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TMethods.Add(c);
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

            var c = GetMethod(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST method for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteMethod(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TMethods.Where(a => a.intMethodID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TMethods.Remove(v);
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





//        private CapstoneEntities db = new CapstoneEntities();

//        // GET: TMethods
//        public ActionResult Index()
//        {
//            return View(db.TMethods.ToList());
//        }

//        // GET: TMethods/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TMethod tMethod = db.TMethods.Find(id);
//            if (tMethod == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tMethod);
//        }

//        // GET: TMethods/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: TMethods/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "intMethodID,strMethod")] TMethod tMethod)
//        {
//            if (ModelState.IsValid)
//            {
//                db.TMethods.Add(tMethod);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(tMethod);
//        }

//        // GET: TMethods/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TMethod tMethod = db.TMethods.Find(id);
//            if (tMethod == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tMethod);
//        }

//        // POST: TMethods/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "intMethodID,strMethod")] TMethod tMethod)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(tMethod).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(tMethod);
//        }

//        // GET: TMethods/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TMethod tMethod = db.TMethods.Find(id);
//            if (tMethod == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tMethod);
//        }

//        // POST: TMethods/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            TMethod tMethod = db.TMethods.Find(id);
//            db.TMethods.Remove(tMethod);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
