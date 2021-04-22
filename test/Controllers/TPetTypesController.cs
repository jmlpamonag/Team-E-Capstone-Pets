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
    public class TPetTypesController : Controller
    {

        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TPetTypes
        public ActionResult Index() {
            //return View(db.TPetTypes.ToList());
            return View();

        }


        public JsonResult GetPetTypes() {
            List<TPetType> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                dc.Configuration.ProxyCreationEnabled = false;

                var petType = from a in dc.TPetTypes
                              select new {
                                  a
                              };

                if (petType != null) {

                    all = new List<TPetType>();
                    foreach (var i in petType) {

                        TPetType con = i.a;

                        all.Add(con);
                    }
                }
            }

            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Get Service Type by ID  
        public TPetType GetPetType(int intPetTypeID) {

            TPetType petType = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TPetTypes
                         where a.intPetTypeID.Equals(intPetTypeID)
                         select new {
                             a
                         }).FirstOrDefault();

                if (v != null) {
                    petType = v.a;
                }
                return petType;
            }
        }

        //for get view for Save service type  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetPetType(id);

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
        public ActionResult Save(TPetType c) {

            string message = "";
            bool status = false;

            if (ModelState.IsValid) {

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intPetTypeID > 0) {
                        var v = dc.TPetTypes.Where(a => a.intPetTypeID.Equals(c.intPetTypeID)).FirstOrDefault();
                        if (v != null) {
                            v.strPetType = c.strPetType;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TPetTypes.Add(c);
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
                var c = GetPetType(id);
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
        public ActionResult Update(TPetType c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intPetTypeID > 0) {
                        var v = dc.TPetTypes.Where(a => a.intPetTypeID.Equals(c.intPetTypeID)).FirstOrDefault();
                        if (v != null) {
                            v.strPetType = c.strPetType;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TPetTypes.Add(c);
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

            var c = GetPetType(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST method for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeletePetType(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TPetTypes.Where(a => a.intPetTypeID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TPetTypes.Remove(v);
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

//private CapstoneEntities db = new CapstoneEntities();

//        // GET: TPetTypes
//        public ActionResult Index()
//        {
//            return View(db.TPetTypes.ToList());
//        }

//        // GET: TPetTypes/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TPetType tPetType = db.TPetTypes.Find(id);
//            if (tPetType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tPetType);
//        }

//        // GET: TPetTypes/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: TPetTypes/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "intPetTypeID,strPetType")] TPetType tPetType)
//        {
//            if (ModelState.IsValid)
//            {
//                db.TPetTypes.Add(tPetType);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(tPetType);
//        }

//        // GET: TPetTypes/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TPetType tPetType = db.TPetTypes.Find(id);
//            if (tPetType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tPetType);
//        }

//        // POST: TPetTypes/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "intPetTypeID,strPetType")] TPetType tPetType)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(tPetType).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(tPetType);
//        }

//        // GET: TPetTypes/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TPetType tPetType = db.TPetTypes.Find(id);
//            if (tPetType == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tPetType);
//        }

//        // POST: TPetTypes/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            TPetType tPetType = db.TPetTypes.Find(id);
//            db.TPetTypes.Remove(tPetType);
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
