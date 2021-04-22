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
    public class TBreedsController : Controller
    {
        //private CapstoneEntities db = new CapstoneEntities();

        // GET: TBreeds
        public ActionResult Index()
        {
            //return View(db.TBreeds.ToList());
            return View();

        }

        // Get JSON 
        public JsonResult GetBreeds() {
            List<TBreed> all = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

				dc.Configuration.ProxyCreationEnabled = false;

				var breeds = from a in dc.TBreeds
                             select new {
                                       a
                                   };

                if (breeds != null) {

                    all = new List<TBreed>();

                    foreach (var i in breeds) {

                        TBreed con = i.a;

                        all.Add(con);
                    }
                }
            }

            return new JsonResult { Data = all, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        //Get Breed by ID  
        public TBreed GetBreed(int intBreedID) {

            TBreed breeds = null;

            using (CapstoneEntities dc = new CapstoneEntities()) {

                var v = (from a in dc.TBreeds
                         where a.intBreedID.Equals(intBreedID)
                         select new {
                             a
                         }).FirstOrDefault();

                if (v != null) {
                    breeds = v.a;
                }
                return breeds;
            }
        }




        // Get partial view for Save Breed  
        public ActionResult Save(int id = 0) {

            if (id > 0) {
                var c = GetBreed(id);

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


        // POST method for Save records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TBreed c) {

            string message = "";
            bool status = false;

            if (ModelState.IsValid) {

                using (CapstoneEntities dc = new CapstoneEntities()) {

                    if (c.intBreedID > 0) {
                        var v = dc.TBreeds.Where(a => a.intBreedID.Equals(c.intBreedID)).FirstOrDefault();
                        if (v != null) {
                            v.strBreedName = c.strBreedName;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TBreeds.Add(c);
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


        // Get partial view for update breed
        public ActionResult Update(int id = 0) {

            if (id > 0) {
                var c = GetBreed(id);
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



        // POST method for update records to the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(TBreed c) {
            string message = "";
            bool status = false;
            if (ModelState.IsValid) {
                using (CapstoneEntities dc = new CapstoneEntities()) {
                    if (c.intBreedID > 0) {
                        var v = dc.TBreeds.Where(a => a.intBreedID.Equals(c.intBreedID)).FirstOrDefault();
                        if (v != null) {
                            v.strBreedName = c.strBreedName;
                        }
                        else {
                            return HttpNotFound();
                        }
                    }
                    else {
                        dc.TBreeds.Add(c);
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



        // Get partial view for delete 
        public ActionResult Delete(int id) {

            var c = GetBreed(id);
            if (c == null) {
                return HttpNotFound();
            }
            return PartialView(c);
        }

        //for POST method for delete records from the database.  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteBreed(int id) {
            bool status = false;
            string message = "";
            using (CapstoneEntities dc = new CapstoneEntities()) {
                var v = dc.TBreeds.Where(a => a.intBreedID.Equals(id)).FirstOrDefault();
                if (v != null) {
                    dc.TBreeds.Remove(v);
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



//// GET: TBreeds/Details/5
//public ActionResult Details(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    TBreed tBreed = db.TBreeds.Find(id);
//    if (tBreed == null)
//    {
//        return HttpNotFound();
//    }
//    return View(tBreed);
//}

//// GET: TBreeds/Create
//public ActionResult Create()
//{
//    return View();
//}

//// POST: TBreeds/Create
//// To protect from overposting attacks, enable the specific properties you want to bind to, for 
//// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Create([Bind(Include = "intBreedID,strBreedName")] TBreed tBreed)
//{
//    if (ModelState.IsValid)
//    {
//        db.TBreeds.Add(tBreed);
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }

//    return View(tBreed);
//}

//// GET: TBreeds/Edit/5
//public ActionResult Edit(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    TBreed tBreed = db.TBreeds.Find(id);
//    if (tBreed == null)
//    {
//        return HttpNotFound();
//    }
//    return View(tBreed);
//}

//// POST: TBreeds/Edit/5
//// To protect from overposting attacks, enable the specific properties you want to bind to, for 
//// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit([Bind(Include = "intBreedID,strBreedName")] TBreed tBreed)
//{
//    if (ModelState.IsValid)
//    {
//        db.Entry(tBreed).State = EntityState.Modified;
//        db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    return View(tBreed);
//}

//// GET: TBreeds/Delete/5
//public ActionResult Delete(int? id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    TBreed tBreed = db.TBreeds.Find(id);
//    if (tBreed == null)
//    {
//        return HttpNotFound();
//    }
//    return View(tBreed);
//}

//// POST: TBreeds/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(int id)
//{
//    TBreed tBreed = db.TBreeds.Find(id);
//    db.TBreeds.Remove(tBreed);
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

