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
        public class TUsersController : Controller {
            //private CapstoneEntities db = new CapstoneEntities();

            // GET: TUsers
            public ActionResult Index() {
                //return View(db.TUsers.ToList());
                return View();

            }


            public JsonResult GetUsers() {
                List<TUser> all = null;

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    dc.Configuration.ProxyCreationEnabled = false;

                    var User = from a in dc.TUsers
                               select new {
                                   a,
                               };

                    if (User != null) {

                        all = new List<TUser>();
                        foreach (var i in User) {

                            TUser con = i.a;

                            all.Add(con);
                        }
                    }
                }

                return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



            //Get User by ID  
            public TUser GetUser(int intUserID) {

                TUser user = null;

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    var v = (from a in dc.TUsers
                             where a.intUserID.Equals(intUserID)
                             select new {
                                 a,
                             }).FirstOrDefault();

                    if (v != null) {
                        user = v.a;
                    }
                    return user;
                }
            }

            ////for get view for Save User  
            //public ActionResult Save(int id = 0) {

            //    CapstoneEntities dc = new CapstoneEntities();

            //    if (id > 0) {
            //        var c = GetUser(id);

            //        if (c != null) {
            //        }
            //        else {
            //            return HttpNotFound();
            //        }
            //        return PartialView("Save", c);

            //    }
            //    else {
            //        return PartialView("Save");
            //    }

            //}

            ////for POST User for Save records to the database.  
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public ActionResult Save(TUser c) {

            //    string message = "";
            //    bool status = false;

            //    if (ModelState.IsValid) {

            //        using (CapstoneEntities dc = new CapstoneEntities()) {

            //            if (c.intUserID > 0) {
            //                var v = dc.TUsers.Where(a => a.intUserID.Equals(c.intUserID)).FirstOrDefault();
            //                if (v != null) {
            //                    v.strUserName = c.strUserName;
            //                    v.strPassword = c.strPassword;
            //                }
            //                else {
            //                    return HttpNotFound();
            //                }
            //            }
            //            else {
            //                dc.TUsers.Add(c);
            //            }
            //            dc.SaveChanges();
            //            status = true;
            //            message = "Data Is Successfully Saved.";
            //        }
            //    }
            //    else {
            //        message = "Error! Please try again.";
            //    }

            //    return new JsonResult { Data = new { status = status, message = message } };
            //}



            // Get view for Save User
            public ActionResult Update(int id = 0) {


                if (id > 0) {

                    var c = GetUser(id);

                    if (c == null) 
                    {
                        return HttpNotFound();
                    }
                    else 
                        return PartialView("Update", c);
                    }                    
                else {
                    return PartialView("Update");
                }
            }

                //for POST User for update records to the database.  
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Update(TUser c) {

                string message = "";
                bool status = false;

                if (ModelState.IsValid) {

                    using (CapstoneEntities dc = new CapstoneEntities()) {

                        if (c.intUserID > 0) {

                            var v = dc.TUsers.Where(a => a.intUserID.Equals(c.intUserID)).FirstOrDefault();
                            
                            if (v != null) {

                                v.strUserName = c.strUserName;
                                v.strPassword = c.strPassword;
                            }
                            else {
                                return HttpNotFound();
                            }
                        }
                        else {
                            dc.TUsers.Add(c);
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

                var c = GetUser(id);
                if (c == null) {
                    return HttpNotFound();
                }
                return PartialView(c);
            }

            //for POST User for delete records from the database.  
            [HttpPost]
            [ValidateAntiForgeryToken]
            [ActionName("Delete")]
            public ActionResult DeleteUser(int id) {
                bool status = false;
                string message = "";
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    var v = dc.TUsers.Where(a => a.intUserID.Equals(id)).FirstOrDefault();
                    if (v != null) {
                        dc.TUsers.Remove(v);
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
}




//    public class TUsersController : Controller
//    {
//        private CapstoneEntities db = new CapstoneEntities();

//        // GET: TUsers
//        public ActionResult Index()
//        {
//            var tUsers = db.TUsers.Include(t => t.TRole);
//            return View(tUsers.ToList());
//        }

//        // GET: TUsers/Details/5

//        [Authorize(Roles ="Admin, Doctor")]
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TUser tUser = db.TUsers.Find(id);
//            if (tUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tUser);
//        }

//        // GET: TUsers/Create


//        public ActionResult Create()
//        {
//            ViewBag.intRoleID = new SelectList(db.TRoles, "intRoleID", "strRoleName");
//            return View();
//        }

//        // POST: TUsers/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "intUserID,strUserName,strPassword,intRoleID")] TUser tUser)
//        {
//            if (ModelState.IsValid)
//            {
//                db.TUsers.Add(tUser);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.intRoleID = new SelectList(db.TRoles, "intRoleID", "strRoleName", tUser.intRoleID);
//            return View(tUser);
//        }

//        // GET: TUsers/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TUser tUser = db.TUsers.Find(id);
//            if (tUser == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.intRoleID = new SelectList(db.TRoles, "intRoleID", "strRoleName", tUser.intRoleID);
//            return View(tUser);
//        }

//        // POST: TUsers/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "intUserID,strUserName,strPassword,intRoleID")] TUser tUser)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(tUser).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.intRoleID = new SelectList(db.TRoles, "intRoleID", "strRoleName", tUser.intRoleID);
//            return View(tUser);
//        }

//        // GET: TUsers/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            TUser tUser = db.TUsers.Find(id);
//            if (tUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tUser);
//        }

//        // POST: TUsers/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            TUser tUser = db.TUsers.Find(id);
//            db.TUsers.Remove(tUser);
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
