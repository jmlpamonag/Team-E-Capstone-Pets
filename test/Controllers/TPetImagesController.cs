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

namespace test.Controllers
{
    public class TPetImagesController : Controller
    {
        private CapstoneEntities db = new CapstoneEntities();

        // GET: TPetImages
        public ActionResult Index()
        {
            var tPetImages = db.TPetImages
                .Include(t => t.TPet)
                .Include(t => t.TPet.TPetType)
                .Include(t => t.TPet.TOwner)
                .Include(t => t.TPet.TBreed)
                .Include(t => t.TPet.TGender);

            return View(tPetImages.ToList());
        }

        // GET: TPetImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPetImage tPetImage = db.TPetImages.Find(id);
             
            if (tPetImage == null)
            {
                return HttpNotFound();
            }


            return View(tPetImage);
        }

        // GET: TPetImages/Create
        public ActionResult Create()
        {
            ViewBag.intPetID = new SelectList(db.TPets, "intPetID", "strPetNumber");
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType");
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender");
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName");
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName");
            return View();
        }

        // POST: TPetImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "intPetID,strPetNumber,strMicrochipID,strPetName,intPetTypeID,intGenderID,intBreedID,dtmDateofBirth,dblWeight,isBlind,isDeaf,isAggressive,isDeceased,isAllergic,strColor,strNotes,isDeceased,intOwnerID")] TPet tPet, HttpPostedFileBase upload) {
            try {
                if (ModelState.IsValid) {
                    //Other Pet Profile info
                    SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@strPetName", tPet.strPetName),
                    new SqlParameter("@strPetNumber",  tPet.strPetNumber),
                    new SqlParameter("@strMicrochipID",  tPet.strMicrochipID),
                    new SqlParameter("@intPetTypeID", tPet.TPetType.intPetTypeID),
                    //.intPetTypeID),
                    new SqlParameter("@intGenderID",  tPet.intGenderID),
                    new SqlParameter("@intBreedID",  tPet.intBreedID),
                    new SqlParameter("@dtmDateofBirth",  tPet.dtmDateofBirth),
                    new SqlParameter("@dblWeight",  tPet.dblWeight),
                    new SqlParameter("@isBlind",  tPet.isBlind),
                    new SqlParameter("@isDeaf",  tPet.isDeaf),
                    new SqlParameter("@isAggressive",  tPet.isAggressive),
                    new SqlParameter("@isDeceased",  tPet.isDeceased),
                    new SqlParameter("@isAllergic",  tPet.isAllergic),
                    new SqlParameter("@strColor",  tPet.strColor),
                    new SqlParameter("@strNotes",  tPet.strNotes),
                    new SqlParameter("@intOwnerID",  tPet.intOwnerID)
                };
                    db.Database.ExecuteSqlCommand("uspAddPets @strPetName, @strPetNumber, @strMicrochipID, @intPetTypeID, @intGenderID, @intBreedID, @dtmDateofBirth, @dblWeight, @isBlind, @isDeaf, @isAggressive, @isDeceased, @isAllergic, @strColor, @strNotes, @intOwnerID", param);
                    
                    //PetImage
                    if (upload != null && upload.ContentLength > 0) {

                        var image = new TPetImage {
                            strFileName = Path.GetFileName(upload.FileName),
                            strFileType = Path.GetExtension(upload.FileName),
                            strContentType = upload.ContentType,
                            intPetID = tPet.intPetID
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                            image.imgContent = reader.ReadBytes(upload.ContentLength);
                        }
                        tPet.TPetImages = new List<TPetImage> { image };
                        
                    }
                    db.TPetImages.Add(tPet.TPetImage);
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

            ViewBag.intPetID = new SelectList(db.TPets, "intPetID", "strPetNumber", tPet.intPetID);
                return View(tPet.TPetImage);
            }

		private ActionResult View(object petImage) {
			throw new NotImplementedException();
		}


		// GET: TPetImages/Edit/5
		public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPetImage tPetImage = db.TPetImages.Find(id);
            if (tPetImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.intPetTypeID = new SelectList(db.TPetTypes, "intPetTypeID", "strPetType", tPetImage.TPet.intPetTypeID);
            ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tPetImage.TPet.intGenderID);
            ViewBag.intOwnerID = new SelectList(db.TOwners, "intOwnerID", "strLastName", tPetImage.TPet.intOwnerID);
            ViewBag.intBreedID = new SelectList(db.TBreeds, "intBreedID", "strBreedName", tPetImage.TPet.intBreedID);

            ViewBag.intPetID = new SelectList(db.TPets, "intPetID", "strPetNumber", tPetImage.intPetID);
            return View(tPetImage);
        }

        // POST: TPetImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "intPetID,strPetNumber,strMicrochipID,strPetName,intPetTypeID,intGenderID,intBreedID,dtmDateofBirth,dblWeight,isBlind,isDeaf,isAggressive,isDeceased,isAllergic,strColor,strNotes,isDeceased,intOwnerID")] TPet tPet, HttpPostedFileBase upload) 
        {
            try {

                if (ModelState.IsValid) {
                    SqlParameter[] param = new SqlParameter[] {
                        new SqlParameter("@intPetID", tPet.intPetID),
                        new SqlParameter("@strPetName", tPet.strPetName),
                        new SqlParameter("@strPetNumber",  tPet.strPetNumber),
                        new SqlParameter("@strMicrochipID",  tPet.strMicrochipID),
                        new SqlParameter("@intPetTypeID", tPet.intPetTypeID),
                        //.intPetTypeID),
                        new SqlParameter("@intGenderID",  tPet.intGenderID),
                        new SqlParameter("@intBreedID",  tPet.intBreedID),
                        new SqlParameter("@dtmDateofBirth",  tPet.dtmDateofBirth),
                        new SqlParameter("@dblWeight",  tPet.dblWeight),
                        new SqlParameter("@isBlind",  tPet.isBlind),
                        new SqlParameter("@isDeaf",  tPet.isDeaf),
                        new SqlParameter("@isAggressive",  tPet.isAggressive),
                        new SqlParameter("@isDeceased",  tPet.isDeceased),
                        new SqlParameter("@isAllergic",  tPet.isAllergic),
                        new SqlParameter("@strColor",  tPet.strColor),
                        new SqlParameter("@strNotes",  tPet.strNotes),
                        new SqlParameter("@intOwnerID",  tPet.intOwnerID)
                };
                    db.Database.ExecuteSqlCommand("uspUpdatePets @strPetName, @strPetNumber, @strMicrochipID, @intPetTypeID, @intGenderID, @intBreedID, @dtmDateofBirth, @dblWeight, @isBlind, @isDeaf, @isAggressive, @isDeceased, @isAllergic, @strColor, @strNotes, @intOwnerID", param);

                    //PetImage
                    if (upload != null && upload.ContentLength > 0) {

                        var image = new TPetImage {
                            strFileName = Path.GetFileName(upload.FileName),
                            strFileType = Path.GetExtension(upload.FileName),
                            strContentType = upload.ContentType,
                            intPetID = tPet.intPetID
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                            image.imgContent = reader.ReadBytes(upload.ContentLength);
                        }
                        tPet.TPetImages = new List<TPetImage> { image };

                    }

                    db.Entry(tPet.TPetImage).State = EntityState.Modified;
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

                ViewBag.intPetID = new SelectList(db.TPets, "intPetID", "strPetNumber", tPet.intPetID);
                return View(tPet.TPetImage);
            }
        
        // GET: TPetImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPetImage tPetImage = db.TPetImages.Find(id);
            if (tPetImage == null)
            {
                return HttpNotFound();
            }
            return View(tPetImage);
        }

        // POST: TPetImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TPetImage tPetImage = db.TPetImages.Find(id);
            db.TPetImages.Remove(tPetImage);
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
        // To convert the Byte Array to the author Image
        public FileContentResult getImg(int intPetImageID) {
            byte[] byteArray = db.TPetImages.Find(intPetImageID).imgContent;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }


        // GET: TPetImages
        public ActionResult PetOwnerHome() {
            var tPetImages = db.TPetImages
                .Include(t => t.TPet)
                .Include(t => t.TPet.TPetType)
                .Include(t => t.TPet.TOwner)
                .Include(t => t.TPet.TBreed)
                .Include(t => t.TPet.TGender);
            //.Include(t => t.imgContent);

            return View(tPetImages.ToList());
        }

        //public byte[] ImageToByteArray(System.Drawing.Image imageIn) {
        //    using (var ms = new MemoryStream()) {
        //        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

        //        return ms.ToArray();
        //    }
        //}

        //public Image ByteArrayToImage(byte[] byteArrayIn) {
        //    using (var ms = new MemoryStream(byteArrayIn)) {
        //        var returnImage = Image.FromStream(ms);

        //        return returnImage;
        //    }
        //}


        //public ActionResult GetImage(int id) {
        //    // fetch image data from database

        //    return File(imgContent, "image/jpg");
        //}

        //public Image byteArrayToImage(byte[] byteArrayIn) {
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
    }
}
