using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test;
using test.Models;
using Rotativa;

namespace test.Controllers
{
    public class TVisitsController : Controller {
        private CapstoneEntities db = new CapstoneEntities();

        // GET: TVisits
        public ActionResult Index() {
            var tVisits = db.TVisits.Include(t => t.intVisitReasonID);
            return View(tVisits.ToList());
        }


        // GET: TVisits/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVisit tVisit = db.TVisits.Find(id);
            if (tVisit == null) {
                return HttpNotFound();
            }

            return View(tVisit);
        }


        public ActionResult PrintInvoice(int id) {

            var report = new Rotativa.ActionAsPdf("Invoice", new { id = id });
            return report;

        }
        public ActionResult Invoice(int? id) {



            Visits visitModel = new Visits();
  


            var tables = new Visits {
                Visit = db.TVisits.Where(a => a.intVisitID == id),
                VisitReason = db.TVisitReasons,
                Pet = db.TPets,
                Breed = db.TBreeds,
                Gender = db.TGenders,
                Owner = db.TOwners,
                VisitService = db.TVisitServices,
                ServType = db.TServiceTypes
            };


            var visit = (from Visit in db.TVisits
                         join VR in db.TVisitReasons
                         on Visit.intVisitReasonID equals VR.intVisitReasonID
                         join Pet in db.TPets
                         on Visit.intPetID equals Pet.intPetID
                         join Breed in db.TBreeds
                         on Pet.intBreedID equals Breed.intBreedID
                         join Gen in db.TGenders
                         on Pet.intGenderID equals Gen.intGenderID
                         join Owner in db.TOwners
                         on Pet.intOwnerID equals Owner.intOwnerID
                         where Visit.intVisitID == id
                         select new {
                             Visit.dtmDateOfVist,
                             VR.strVisitReason,
                             Pet.strPetNumber,
                             Pet.strPetName,
                             Pet.dblWeight,
                             Breed.strBreedName,
                             Gen.strGender,
                             Owner.intOwnerID,
                             Owner.strFirstName,
                             Owner.strLastName,
                             Owner.strAddress,
                             Owner.strCity,
                             Owner.strZip,
                             Owner.strPhoneNumber,
                             Owner.strEmail
                         }).FirstOrDefault();


            // Visit Model
            visitModel.dtmOfVisit = visit.dtmDateOfVist;
            visitModel.strVisitReason = visit.strVisitReason;
            visitModel.strOwnerName = visit.strFirstName + " " + visit.strLastName;
            visitModel.intOwnerNumber = visit.intOwnerID;
            visitModel.strAddress = visit.strAddress;
            visitModel.strCity = visit.strCity;
            visitModel.strZip = visit.strZip;
            visitModel.strPhone = visit.strPhoneNumber;
            visitModel.strEmail = visit.strEmail;
            visitModel.strPetName = visit.strPetName;
            visitModel.strPetNumber = visit.strPetNumber;
            visitModel.strBreed = visit.strBreedName;
            visitModel.dblWeight = visit.dblWeight;
            visitModel.strGender = visit.strGender;
            visitModel.VisitService = db.TVisitServices.Where(x => x.intVisitID == id).ToList();


            bool flag = db.TVisitMedications.Any(p => p.intVisitID == id);

            if (flag == true) {

                var medications = (from VisMeds in db.TVisitMedications
                                   join Meds in db.TMedications
                                   on VisMeds.intMedicationID equals Meds.intMedicationID
                                   where id != null && VisMeds.intVisitID == id
                                   select new {
                                       VisMeds.intQuantity,
                                       Meds.strMedicationName,
                                       Meds.strMedicationDesc,
                                       Meds.dblPrice
                                   }).FirstOrDefault();

                visitModel.strMedication = medications.strMedicationName;
                visitModel.strMedDesc = medications.strMedicationDesc;
                visitModel.intQuantity = medications.intQuantity;
                visitModel.dblMedPrice = medications.dblPrice;

                visitModel.VisitMed = db.TVisitMedications.Where(x => x.intVisitID == id).ToList();

            }






            return View(visitModel);
        }


        // GET: TVisits/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["intPetID"] = id;
            var petName = db.TPets.Where(x => x.intPetID == id).Select(x => x.strPetName).FirstOrDefault();
            var petID = db.TPets.Where(x => x.intPetID == id).Select(x => x.strPetNumber).FirstOrDefault();
            var ownerName = (from o in db.TOwners
                             join p in db.TPets
                             on o.intOwnerID equals p.intOwnerID
                             where p.intPetID == id
                             select new
                             {
                                 firstName = o.strFirstName,
                                 lastName = o.strLastName
                             }).FirstOrDefault();
            List<EmployeeInformation> doctorList = (from e in db.TEmployees
                                   join j in db.TJobTitles
                                   on e.intJobTitleID equals j.intJobTitleID
                                   where j.strJobTitleDesc == "Doctor"
                                   select new EmployeeInformation
                                   {
                                       intEmployeeID = e.intEmployeeID,
                                       intJobTitleID = j.intJobTitleID,
                                       strEmployeeName = "Dr. " + e.strFirstName + " " + e.strLastName
                                   }).ToList();

            if (petName == null)
            {
                return HttpNotFound();
            }

            ViewBag.PetName = petName;
            ViewBag.PetID = petID;
            ViewBag.OwnerName = ownerName.firstName + " " + ownerName.lastName;
            ViewBag.intVisitReasonID = new SelectList(db.TVisitReasons, "intVisitReasonID", "strVisitReason");
            ViewBag.intEmployeeID = new SelectList(doctorList, "intEmployeeID", "strEmployeeName");

            return View();
        }

        // POST: TVisits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVisit tVisit)
        {
            int petID = (int)Session["intPetID"];
            if (ModelState.IsValid) {
                TVisit newPetVisit = new TVisit()
                {
                    intPetID = petID,
                    dtmDateOfVist = DateTime.Now,
                    intVisitReasonID = tVisit.intVisitReasonID
                };
                db.TVisits.Add(newPetVisit);
                db.SaveChanges();


                int lastInsertedVisitID = db.TVisits.Max(v => v.intVisitID);
                Session["intVisitId"] = lastInsertedVisitID;

                TVisitEmployee newPetVisitEmployee = new TVisitEmployee()
                {
                    intVisitID = lastInsertedVisitID,
                    intEmployeeID = tVisit.intEmployeeID
                };

                db.TVisitEmployees.Add(newPetVisitEmployee);
                db.SaveChanges();

                //Remove existing data from session for pet id

                int wellnessExam = db.TVisitReasons.Where(x => x.strVisitReason == "Wellness Exam").Select(z => z.intVisitReasonID).FirstOrDefault();
                int medicationExam = db.TVisitReasons.Where(x => x.strVisitReason == "Medication").Select(z => z.intVisitReasonID).FirstOrDefault();

                if (newPetVisit.intVisitReasonID == wellnessExam)
                {
                    return RedirectToAction("Create", "THealthExam", new { id = petID });
                }
                else if(newPetVisit.intVisitReasonID == medicationExam)
                {
                    return RedirectToAction("Index", "VisitMedications");
                }
                else
                {
                    return RedirectToAction("Index", "VisitServices");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: TVisits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVisit tVisit = db.TVisits.Find(id);
            if (tVisit == null)
            {
                return HttpNotFound();
            }
            ViewBag.intVisitReasonID = new SelectList(db.TVisitReasons, "intVisitReasonID", "strVisitReason", tVisit.intVisitReasonID);
            return View(tVisit);
        }

        // POST: TVisits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "intVisitID,intPetID,intVisitReasonID,dtmDateOfVist")] TVisit tVisit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tVisit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.intVisitReasonID = new SelectList(db.TVisitReasons, "intVisitReasonID", "strVisitReason", tVisit.intVisitReasonID);
            return View(tVisit);
        }

        // GET: TVisits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVisit tVisit = db.TVisits.Find(id);
            if (tVisit == null)
            {
                return HttpNotFound();
            }
            return View(tVisit);
        }

        // POST: TVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVisit tVisit = db.TVisits.Find(id);
            db.TVisits.Remove(tVisit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PetVisits(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["intPetID"] = id;
            var petName = db.TPets.Where(x => x.intPetID == id).Select(x => x.strPetName).FirstOrDefault();
            var tVisits = db.TVisits.Where(x => x.intPetID == id);

            if (petName == null)
            {
                return HttpNotFound();
            }
            ViewBag.PetName = petName;
            
            return View(tVisits);
        }








        //public ActionResult PrintInvoice(int intVisitId, int intPetId) {

        //    VisitSummary myModel = new VisitSummary();

        //    //General Information
        //    var informationPacket = (from o in db.TOwners
        //                             join p in db.TPets
        //                             on o.intOwnerID equals p.intOwnerID
        //                             join s in db.TStates
        //                             on o.intStateID equals s.intStateID
        //                             join v in db.TVisits
        //                             on p.intPetID equals v.intPetID
        //                             where p.intPetID == intPetId
        //                             select new {
        //                                 ownerName = o.strFirstName + " " + o.strLastName,
        //                                 address = o.strAddress + ", " + o.strCity + ", " + s.strStateName + " " + o.strZip,
        //                                 phoneNumber = o.strPhoneNumber,
        //                                 petName = p.strPetName,
        //                                 petNumber = p.strPetNumber,
        //                                 clientNumber = o.intOwnerID,
        //                                 dateOfVisit = v.dtmDateOfVist
        //                             }).FirstOrDefault();

        //    //Save to model
        //    myModel.strOwnerName = informationPacket.ownerName;
        //    myModel.strAddress = informationPacket.address;
        //    myModel.strPhoneNumber = informationPacket.phoneNumber;
        //    myModel.intOwnerNumber = informationPacket.clientNumber;
        //    myModel.strPetName = informationPacket.petName;
        //    myModel.strPetNumber = informationPacket.petNumber;
        //    myModel.dtmOfVisit = informationPacket.dateOfVisit;
        //    myModel.PetVisitServices = db.TVisitServices.Where(x => x.intVisitID == intVisitId).ToList();
        //    myModel.PetVisitMedications = db.TVisitMedications.Where(x => x.intVisitID == intVisitId).ToList();

        //    return new ActionAsPdf(
        //                   "Invoice",
        //                   new { myModel = myModel }) { FileName = "Invoice.pdf" };

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
