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
    public class TRolesController : Controller {
        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TRoles
        public ActionResult Index() {
            //return View(db.TRoles.ToList());
            return View();

        }


        public JsonResult GetRoles() {
            List<TRole> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                dc.Configuration.ProxyCreationEnabled = false;

                var role = from a in dc.TRoles
                           select new {
                               a
                           };

                if (role != null) {

                    all = new List<TRole>();
                    foreach (var i in role) {

                        TRole con = i.a;

                        all.Add(con);
                    }
                }
            }

            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Get Role by ID  
        public TRole GetRole(int intRoleID) {

            TRole role = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TRoles
                         where a.intRoleID.Equals(intRoleID)
                         select new {
                             a
                         }).FirstOrDefault();

                if (v != null) {
                    role = v.a;
                }
                return role;
            }
        }

        //for get view for Save role  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetRole(id);

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

        //for POST Role for Save records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TRole c) {

            string message = "";
            bool status = false;

            if (ModelState.IsValid) {

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intRoleID > 0) {
                        var v = dc.TRoles.Where(a => a.intRoleID.Equals(c.intRoleID)).FirstOrDefault();
                        if (v != null) {
                            v.strRoleName = c.strRoleName;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TRoles.Add(c);
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



        //for get view for update role
        public ActionResult Update(int id = 0) {

            if (id > 0) {
                var c = GetRole(id);
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

        //for POST Role for update records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(TRole c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intRoleID > 0) {
                        var v = dc.TRoles.Where(a => a.intRoleID.Equals(c.intRoleID)).FirstOrDefault();
                        if (v != null) {
                            v.strRoleName = c.strRoleName;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TRoles.Add(c);
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

            var c = GetRole(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST Role for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteRole(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TRoles.Where(a => a.intRoleID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TRoles.Remove(v);
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

//        // GET: TRoles
//        public ActionResult Index()
//        {
//            return View(db.TRoles.ToList());
//        }

//        // GET: TRoles/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TRole tRole = db.TRoles.Find(id);
//            if (tRole == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tRole);
//        }

//        // GET: TRoles/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: TRoles/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "intRoleID,strRoleName")] TRole tRole)
//        {
//            if (ModelState.IsValid)
//            {
//                db.TRoles.Add(tRole);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(tRole);
//        }

//        // GET: TRoles/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TRole tRole = db.TRoles.Find(id);
//            if (tRole == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tRole);
//        }

//        // POST: TRoles/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "intRoleID,strRoleName")] TRole tRole)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(tRole).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(tRole);
//        }

//        // GET: TRoles/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TRole tRole = db.TRoles.Find(id);
//            if (tRole == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tRole);
//        }

//        // POST: TRoles/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            TRole tRole = db.TRoles.Find(id);
//            db.TRoles.Remove(tRole);
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
