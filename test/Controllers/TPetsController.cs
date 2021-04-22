using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test;
using test.Models;

namespace test.Controllers {
    public class TPetsController : Controller {
        private CapstoneEntities db = new CapstoneEntities();

        // GET: TPets
        public ActionResult Index() {
            if (Session["intUserID"] == null ) 
            {
                //ViewBag.ErrorMessage = "Authorized Users Only";
                return RedirectToAction("Login", "Home");
            }

            var tPets = db.TPets
                .Include(t => t.TPetType)
                .Include(t => t.TOwner)
                .Include(t => t.TOwner.TState)
                .Include(t => t.TBreed)
                .Include(t => t.TGender)
                .Include(t => t.TPetImages);

            return View(tPets.ToList());
        }

        // GET: TPets/Details/5
        public ActionResult Details(int? id){
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType");
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender");
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName");
            ViewBag.intPetImageID = new SelectList(db.TPetImages, "intPetImageID", "imgContent");

            if (id == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
  
            Session["intPetID"] = id;
            // TPet tPet = db.TPets.Find(id);
            //TPetImage tPetImage = db.TPetImages.Find(db.);
            //
            TPet tPet = db.TPets.Include(s => s.TPetImages).SingleOrDefault(s => s.intPetID == id);

            if (tPet == null) {
				return HttpNotFound();
			}
			return View(tPet);
        }

        // GET: TPets/Create
        public ActionResult Create() {
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType");
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender");
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName");

            return View();
        }

        // POST: TPets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "intPetID,strPetNumber,strMicrochipID,strPetName,intPetTypeID,intGenderID,intBreedID,dtmDateofBirth,dblWeight,isBlind,isDeaf,isAggressive,isDeceased,isAllergic,strColor,strNotes,isDeceased,intOwnerID")] TPet tPet, HttpPostedFileBase upload) {
            try {
                if (ModelState.IsValid) {
                    if (upload != null && upload.ContentLength > 0) {
                        var image = new TPetImage {
                            strFileName = Path.GetFileName(upload.FileName),
                            strFileType = Path.GetExtension(upload.FileName),
                            strContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                            image.imgContent = reader.ReadBytes(upload.ContentLength);
                        }
                        tPet.TPetImages = new List<TPetImage> { image };
                    }
                    db.TPets.Add(tPet);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            catch (RetryLimitExceededException /* dex */) {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType", tPet.intPetTypeID);
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tPet.intGenderID);
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName", tPet.intOwnerID);
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName", tPet.intBreedID);

            return View(tPet);
        }

        // GET: TPets/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPet tPet = db.TPets.Find(id);
            if (tPet == null) {
                return HttpNotFound();
            }
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType", tPet.intPetTypeID);
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tPet.intGenderID);
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName", tPet.intOwnerID);
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName", tPet.intBreedID);
            return View(tPet);
        }


        //// POST: TPets/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(TPet tPet, int? id, HttpPostedFileBase upload) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var petToUpdate = db.TPets.Find(id);
            if (TryUpdateModel(petToUpdate, "",
               new string[] { "intPetID", "strPetNumber", "strMicrochipID", "strPetName", "intPetTypeID", "intGenderID", "intBreedID", "dtmDateofBirth", "dblWeight", "isBlind", "isDeaf", "isAggressive", "isDeceased", "isAllergic", "strColor", "strNotes", "isDeceased", "intOwnerID" })) {
                try {
                    if (upload != null && upload.ContentLength > 0) {
                        if (petToUpdate.TPetImages.Any(f => f.strFileType == ".jpg")) {
                            db.TPetImages.Remove(petToUpdate.TPetImages.First(f => f.strFileType == ".jpg"));
                        }
                        var avatar = new TPetImage {
                            strFileName = System.IO.Path.GetFileName(upload.FileName),
                            strFileType = System.IO.Path.GetExtension(upload.FileName),
                            strContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                            avatar.imgContent = reader.ReadBytes(upload.ContentLength);
                        }
                        petToUpdate.TPetImages = new List<TPetImage> { avatar };
                    }
                    db.Entry(petToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = tPet.intPetID });
                }
                catch (RetryLimitExceededException /* dex */) {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(petToUpdate);

        }

        // GET: TPets/Delete/5
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPet tPet = db.TPets.Find(id);
            if (tPet == null) {
                return HttpNotFound();
            }
            return View(tPet);
        }

        // POST: TPets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            TPet tPet = db.TPets.Find(id);


            db.TPets.Remove(tPet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //// GET: TPets/OwnerHome/5
        //public ActionResult OwnerHome(int? id) {
        //    OwnerHome myModel = new OwnerHome();
        //    ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
        //    ViewBag.intPetImageID = new SelectList(db.TPetImages, "intPetImageID", "imgContent");


        //    if (id == null) {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    TPet tPet = db.TPets.Include(s => s.TPetImages).SingleOrDefault(s => s.intPetID == id);

        //    if (tPet == null) {
        //        return HttpNotFound();
        //    }
        //    return View(tPet);
        //    //return View();
        //}



        //// GET: TPets/OwnerHome/5
        //public ActionResult OwnerHome(int? id) {
        //    ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
        //    ViewBag.intPetImageID = new SelectList(db.TPetImages, "intPetImageID", "imgContent");

        //    if (id == null) {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    TPet tPet = db.TPets.Include(s => s.TPetImages).SingleOrDefault(s => s.intPetID == id);

        //    if (tPet == null) {
        //        return HttpNotFound();
        //    }
        //    return View(tPet);
        //    //return View();
        //}

        // GET: TPets/Details/5
        public ActionResult PetProfile(int? id) {
            TPet pet = new TPet();
            if (Session["intUserID"] == null) {
                //ViewBag.ErrorMessage = "Authorized Users Only";
                return RedirectToAction("Login", "Home");
            }
            else {
                var oInfo = (from o in db.TOwners
                             join p in db.TPets
                              on o.intOwnerID equals p.intOwnerID
                             join u in db.TUsers
                              on o.intUserID equals u.intUserID
                             where p.intPetID == (int)id
                             select new {
                                 intOwnerID = o.intOwnerID,
                                 strFirstName = o.strFirstName,
                                 strLastName = o.strLastName,
                                 intUserID = u.intUserID
                             }).FirstOrDefault();
                if (Session["intUserID"].ToString() != oInfo.intUserID.ToString()) {
                    //ViewBag.ErrorMessage = "Authorized Users Only";
                    return RedirectToAction("Login", "Home");
                }
                else {

                    ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType");
                    ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender");
                    ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
                    ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName");
                    ViewBag.intPetImageID = new SelectList(db.TPetImages, "intPetImageID", "imgContent");
                    ViewBag.intVisitID = new SelectList(db.TVisits, "intVisitID", "dtmDateOfVist");

                    if (id == null) {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    TPet tPet = db.TPets.Include(s => s.TPetImages).SingleOrDefault(s => s.intPetID == id);

                    //Get List of Visits and put in PetProfile View


                    if (tPet == null) {
                        return HttpNotFound();
                    }

                    return View(tPet);
                }
            }
        }
        // GET: TPets/Edit/5
        public ActionResult PetProfileEdit(int? id) {
            if (Session["intUserID"] == null) {
                //ViewBag.ErrorMessage = "Authorized Users Only";
                return RedirectToAction("Login");
            }
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                var oInfo = (from o in db.TOwners
                             join p in db.TPets
                              on o.intOwnerID equals p.intOwnerID
                             join u in db.TUsers
                              on o.intUserID equals u.intUserID
                             where p.intPetID == (int)id
                             select new {
                                 intOwnerID = o.intOwnerID,
                                 strFirstName = o.strFirstName,
                                 strLastName = o.strLastName,
                                 intUserID = u.intUserID
                             }).FirstOrDefault();
                if (Session["intUserID"].ToString() != oInfo.intUserID.ToString()) {
                    //ViewBag.ErrorMessage = "Authorized Users Only";
                    return RedirectToAction("Login", "Home");
                }
                else {
                    TPet tPet = db.TPets.Find(id);
                    if (tPet == null) {
                        return HttpNotFound();
                    }
                    ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateName", tPet.TOwner.TState.intStateID);
                    ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType", tPet.intPetTypeID);
                    ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tPet.intGenderID);
                    ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName", tPet.intOwnerID);
                    ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName", tPet.intBreedID);
                    return View(tPet);
                }
            }
        }
        //// POST: TPets/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("PetProfileEdit")]
        [ValidateAntiForgeryToken]
        public ActionResult PetProfileEditPost(TPet tPet, int? id, HttpPostedFileBase upload) {
            if (Session["intUserID"] == null) {
                //ViewBag.ErrorMessage = "Authorized Users Only";
                return RedirectToAction("Login");
            }
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                var oInfo = (from o in db.TOwners
                             join p in db.TPets
                              on o.intOwnerID equals p.intOwnerID
                             join u in db.TUsers
                              on o.intUserID equals u.intUserID
                             where p.intPetID == (int)id
                             select new {
                                 intOwnerID = o.intOwnerID,
                                 strFirstName = o.strFirstName,
                                 strLastName = o.strLastName,
                                 intUserID = u.intUserID
                             }).FirstOrDefault();
                if (Session["intUserID"].ToString() != oInfo.intUserID.ToString()) {
                    //ViewBag.ErrorMessage = "Authorized Users Only";
                    return RedirectToAction("Login", "Home");
                }
                else {
                    if (id == null) {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var petToUpdate = db.TPets.Find(id);
                    var ownerToUpdate = db.TOwners.Find(petToUpdate.intOwnerID);
                    if (TryUpdateModel(petToUpdate, "",
                       new string[] { "intPetID", "strPetNumber", "strMicrochipID", "strPetName", "intPetTypeID", "intGenderID", "intBreedID", "dtmDateofBirth", "dblWeight", "isBlind", "isDeaf", "isAggressive", "isDeceased", "isAllergic", "strColor", "strNotes", "isDeceased", "intOwnerID" })) {
                        try {
                            if (upload != null && upload.ContentLength > 0) {
                                if (petToUpdate.TPetImages.Any(f => f.strFileType == ".jpg")) {
                                    db.TPetImages.Remove(petToUpdate.TPetImages.First(f => f.strFileType == ".jpg"));
                                }
                                var avatar = new TPetImage {
                                    strFileName = System.IO.Path.GetFileName(upload.FileName),
                                    strFileType = System.IO.Path.GetExtension(upload.FileName),
                                    strContentType = upload.ContentType
                                };
                                using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                                    avatar.imgContent = reader.ReadBytes(upload.ContentLength);
                                }
                                petToUpdate.TPetImages = new List<TPetImage> { avatar };
                            }
                            db.Entry(petToUpdate).State = EntityState.Modified;
                            // db.Entry(ownerToUpdate).State = EntityState.Modified;
                            db.SaveChanges();

                            // return RedirectToAction("PetProfile", new { id = tPet.intPetID });
                        }
                        catch (RetryLimitExceededException /* dex */) {
                            //Log the error (uncomment dex variable name and add a line here to write a log.
                            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        }

                        if (TryUpdateModel(ownerToUpdate, "",
                      new string[] { "intOwnerID", "strFirstName", "strLastName", "intGenderID", "intPetTypeID", "intGenderID", "strAddress", "strCity", "intStateID", "strZip", "isDeaf", "strPhoneNumber", "strEmail", "strOwner2Name", "strOwner2PhoneNumber", "strOwner2Email", "strNotes", "intUserID" })) {
                            ownerToUpdate.strAddress = tPet.TOwner.strAddress;
                            ownerToUpdate.strCity = tPet.TOwner.strCity;
                            ownerToUpdate.intStateID = petToUpdate.TOwner.intStateID;
                            ownerToUpdate.strZip = tPet.TOwner.strZip;
                            ownerToUpdate.strPhoneNumber = tPet.TOwner.strPhoneNumber;

                            if (ModelState.IsValid) {
                                db.Entry(ownerToUpdate).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateCode", ownerToUpdate.intStateID);
                            ViewBag.intUserID = new SelectList(db.TUsers, "intUserID", "strUserName", ownerToUpdate.intUserID);
                            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", ownerToUpdate.intGenderID);
                        }
                        return RedirectToAction("PetProfile", new { id = tPet.intPetID });
                    }
                    return View(petToUpdate);
                }
            }
        }

        // To convert the Byte Array to the author Image
        public FileContentResult getImg(int intPetID) {
            byte[] byteArray = db.TPetImages.Find(intPetID).imgContent;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        public Image byteArrayToImage(byte[] byteArrayIn) {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
