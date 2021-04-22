using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test;
using System.IO;



namespace test.Controllers
{
    public class TVisitReasonsController : Controller
    {
        private CapstoneEntities db = new CapstoneEntities();


            // GET: TVisitReasons
            public ActionResult Index() {
                return View();

            }


            public JsonResult GetVisitReasons() {
                List<TVisitReason> all = null;

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    dc.Configuration.ProxyCreationEnabled = false;

                    var visitReason = from a in dc.TVisitReasons
                                      select new {
                                          a
                                      };

                    if (visitReason != null) {

                        all = new List<TVisitReason>();
                        foreach (var i in visitReason) {

                            TVisitReason con = i.a;

                            all.Add(con);
                        }
                    }
                }

                return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            // Get VisitReason by ID  
            public TVisitReason GetVisitReason(int intVisitReasonID) {

                TVisitReason visitReason = null;

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    var v = (from a in dc.TVisitReasons
                             where a.intVisitReasonID.Equals(intVisitReasonID)
                             select new {
                                 a
                             }).FirstOrDefault();

                    if (v != null) {
                        visitReason = v.a;
                    }
                    return visitReason;
                }
            }

            // Get view for Save VisitReason  
            public ActionResult Save(int id = 0) {

                if (id > 0) {

                    var c = GetVisitReason(id);

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

            // POST VisitReason for Save   
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Save(TVisitReason c) {

                string message = "";
                bool status = false;

                if (ModelState.IsValid) {

                    using (CapstoneEntities dc = new CapstoneEntities()) {

                        if (c.intVisitReasonID > 0) {
                            var v = dc.TVisitReasons.Where(a => a.intVisitReasonID.Equals(c.intVisitReasonID)).FirstOrDefault();
                            if (v != null) {
                                v.strVisitReason = c.strVisitReason;
                            }
                            else {
                                return HttpNotFound();
                            }
                        }
                        else {
                            dc.TVisitReasons.Add(c);
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



            // Get view for update VisitReason
            public ActionResult Update(int id = 0) {

                if (id > 0) {

                    var c = GetVisitReason(id);

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

            // POST VisitReason for update 
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Update(TVisitReason c) {
                string message = "";
                bool status = false;
                if (ModelState.IsValid) {
                    using (CapstoneEntities dc = new CapstoneEntities()) {
                        if (c.intVisitReasonID > 0) {
                            var v = dc.TVisitReasons.Where(a => a.intVisitReasonID.Equals(c.intVisitReasonID)).FirstOrDefault();
                            if (v != null) {
                                v.strVisitReason = c.strVisitReason;
                            }
                            else {
                                return HttpNotFound();
                            }
                        }
                        else {
                            dc.TVisitReasons.Add(c);
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

            // For delete records view
            public ActionResult Delete(int id) {

                var c = GetVisitReason(id);
                if (c == null) {
                    return HttpNotFound();
                }
                return PartialView(c);
            }

            // POST VisitReason for delete
            [HttpPost]
            [ValidateAntiForgeryToken]
            [ActionName("Delete")]
            public ActionResult DeleteVisitReason(int id) {
                bool status = false;
                string message = "";
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    var v = dc.TVisitReasons.Where(a => a.intVisitReasonID.Equals(id)).FirstOrDefault();
                    if (v != null) {
                        dc.TVisitReasons.Remove(v);
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



    //// GET: TVisitReasons/Details/5
    //public ActionResult Details(int? id)
    //{
    //    if (id == null)
    //    {
    //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //    }
    //    TVisitReason tVisitReason = db.TVisitReasons.Find(id);
    //    if (tVisitReason == null)
    //    {
    //        return HttpNotFound();
    //    }
    //    return View(tVisitReason);
    //}

    //// GET: TVisitReasons/Create
    //public ActionResult Create()
    //{
    //    return View();
    //}

    //// POST: TVisitReasons/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create([Bind(Include = "intVisitReasonID,strVisitReason")] TVisitReason tVisitReason)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        db.TVisitReasons.Add(tVisitReason);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    return View(tVisitReason);
    //}

    //// GET: TVisitReasons/Edit/5
    //public ActionResult Edit(int? id)
    //{
    //    if (id == null)
    //    {
    //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //    }
    //    TVisitReason tVisitReason = db.TVisitReasons.Find(id);
    //    if (tVisitReason == null)
    //    {
    //        return HttpNotFound();
    //    }
    //    return View(tVisitReason);
    //}

    //// POST: TVisitReasons/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
    //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit([Bind(Include = "intVisitReasonID,strVisitReason")] TVisitReason tVisitReason)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        db.Entry(tVisitReason).State = EntityState.Modified;
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }
    //    return View(tVisitReason);
    //}

    //// GET: TVisitReasons/Delete/5
    //public ActionResult Delete(int? id)
    //{
    //    if (id == null)
    //    {
    //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //    }
    //    TVisitReason tVisitReason = db.TVisitReasons.Find(id);
    //    if (tVisitReason == null)
    //    {
    //        return HttpNotFound();
    //    }
    //    return View(tVisitReason);
    //}

    //// POST: TVisitReasons/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public ActionResult DeleteConfirmed(int id)
    //{
    //    TVisitReason tVisitReason = db.TVisitReasons.Find(id);
    //    db.TVisitReasons.Remove(tVisitReason);
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


