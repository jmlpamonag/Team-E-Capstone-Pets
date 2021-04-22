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

namespace test.Controllers
{
    public class TVaccinationsController : Controller
    {
        private CapstoneEntities db = new CapstoneEntities();

        // GET: TVaccinations
        public ActionResult Index()
        {
            var tVaccinations = db.TVaccinations.Include(t => t.TVisitService);
            return View(tVaccinations.ToList());
        }

        // GET: TVaccinations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVaccination tVaccination = db.TVaccinations.Find(id);
            if (tVaccination == null)
            {
                return HttpNotFound();
            }
            return View(tVaccination);
        }

        // GET: TVaccinations/Create
        public ActionResult Create(int serviceID)
        {
            int intVisitId = (int)Session["intVisitId"];
            int intPetId = (int)Session["intPetID"];
            Session["intServiceId"] = serviceID;

            int rabiesVaccineServiceId = db.TServices.Where(x => x.strServiceDesc == "Rabies Vaccine").Select(z => z.intServiceID).FirstOrDefault();
            string strServiceDesc = db.TServices.Where(x => x.intServiceID == serviceID).Select(z => z.strServiceDesc).FirstOrDefault();
            Session["isRabiesVaccine"] = null;
            ViewBag.Name = db.TPets.Where(x => x.intPetID == intPetId).Select(z => z.strPetName).FirstOrDefault();

            if (serviceID == rabiesVaccineServiceId)
            {
                Session["isRabiesVaccine"] = true;
            }



            DateTime today = DateTime.Now;
            VisitVaccination visitVaccination = new VisitVaccination()
            {
                intServiceId = serviceID,
                intVisitServiceId = serviceID,
                strServiceName = strServiceDesc,
                dtmDateofVaccination = today.Month + "/" + today.Day + "/" + today.Year,
                dtmDateOfExpiration = today.Month + "/" + today.Day + "/" + (today.Year + 1),
                strVaccineNotes = " ",
                strRabiesNumber = " "
            };

            return View(visitVaccination);
        }

        // POST: TVaccinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VisitVaccination vaccination)
        {
            int intVisitId = (int)Session["intVisitId"];
            int intPetId = (int)Session["intPetID"];
            int serviceID = (int)Session["intServiceId"];

            if (ModelState.IsValid)
            {
                TVisitService newVisitService = new TVisitService()
                {
                    intVisitID = intVisitId,
                    intServiceID = serviceID
                };

                db.TVisitServices.Add(newVisitService);
                db.SaveChanges();

                int lastInsertedVisitServiceID = db.TVisitServices.Max(v => v.intVisitServiceID);
                if (vaccination.strRabiesNumber == null)
                {
                    vaccination.strRabiesNumber = string.Empty;
                }
                TVaccination newVisitVaccination = new TVaccination()
                {
                    intVisitServiceID = lastInsertedVisitServiceID,
                    dtmDateOfVaccination = Convert.ToDateTime(vaccination.dtmDateofVaccination),
                    dtmDateOfExpiration = Convert.ToDateTime(vaccination.dtmDateOfExpiration),
                    strVaccineDesc = vaccination.strVaccineNotes,
                    strRabiesNumber = vaccination.strRabiesNumber
                };

                db.TVaccinations.Add(newVisitVaccination);
                db.SaveChanges();

                return RedirectToAction("Index", "VisitServices");
            }

            return View(vaccination);
        }

        // GET: TVaccinations/Edit/5
        public ActionResult Edit(int visitServiceId)
        {
            TVaccination tVaccination = db.TVaccinations.Where(x => x.intVisitServiceID == visitServiceId).FirstOrDefault();
            if (tVaccination == null)
            {
                return HttpNotFound();
            }

            int intPetId = (int)Session["intPetID"];
            int serviceId = db.TVisitServices.Where(x => x.intVisitServiceID == visitServiceId).Select(z => z.intServiceID).FirstOrDefault();
            int rabiesVaccineServiceId = db.TServices.Where(x => x.strServiceDesc == "Rabies Vaccine").Select(z => z.intServiceID).FirstOrDefault();
            string serviceName = db.TVisitServices.Where(x => x.intVisitServiceID == visitServiceId).Select(z => z.TService.strServiceDesc).FirstOrDefault();
            Session["intVisitServiceId"] = visitServiceId;
            Session["isRabiesVaccine"] = null;
            DateTime dateOfVaccination = tVaccination.dtmDateOfVaccination;
            DateTime dateOfExpiration = tVaccination.dtmDateOfExpiration;

            if (serviceId == rabiesVaccineServiceId)
            {
                Session["isRabiesVaccine"] = true;

            }

            VisitVaccination visitVaccination = new VisitVaccination()
                {
                    intServiceId = serviceId,
                    intVisitServiceId = visitServiceId,
                    strServiceName = serviceName,
                    dtmDateofVaccination = dateOfVaccination.Month + "/" + dateOfVaccination.Day + "/" + dateOfVaccination.Year,
                    dtmDateOfExpiration = dateOfExpiration.Month + "/" + dateOfExpiration.Day + "/" + dateOfExpiration.Year,
                    strVaccineNotes = tVaccination.strVaccineDesc,
                    strRabiesNumber = tVaccination.strRabiesNumber
                };

            ViewBag.Name = db.TPets.Where(x => x.intPetID == intPetId).Select(z => z.strPetName).FirstOrDefault();
            return View(visitVaccination);

        }

        // POST: TVaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VisitVaccination visitVaccination)
        {
            int intPetId = (int)Session["intPetID"];
            int vaccinationId = db.TVaccinations.Where(x => x.intVisitServiceID == visitVaccination.intVisitServiceId).Select(z => z.intVaccinationID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (visitVaccination.strRabiesNumber == null) { visitVaccination.strRabiesNumber = string.Empty; };
                TVaccination vaccination = new TVaccination()
                {
                    intVaccinationID = vaccinationId,
                    intVisitServiceID = visitVaccination.intVisitServiceId,
                    dtmDateOfVaccination = Convert.ToDateTime(visitVaccination.dtmDateofVaccination),
                    dtmDateOfExpiration = Convert.ToDateTime(visitVaccination.dtmDateOfExpiration),
                    strVaccineDesc = visitVaccination.strVaccineNotes,
                    strRabiesNumber = visitVaccination.strRabiesNumber
                };

                db.Entry(vaccination).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "VisitServices");
            };

            int serviceId = db.TVisitServices.Where(x => x.intVisitServiceID == visitVaccination.intVisitServiceId).Select(z => z.intServiceID).FirstOrDefault();
            int rabiesVaccineServiceId = db.TServices.Where(x => x.strServiceDesc == "Rabies Vaccine").Select(z => z.intServiceID).FirstOrDefault();
            Session["isRabiesVaccine"] = null;
            if (serviceId == rabiesVaccineServiceId)
            {
                Session["isRabiesVaccine"] = true;

            }

            ViewBag.Name = db.TPets.Where(x => x.intPetID == intPetId).Select(z => z.strPetName).FirstOrDefault();
            return View(visitVaccination);
        }

        // GET: TVaccinations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TVaccination tVaccination = db.TVaccinations.Find(id);
            if (tVaccination == null)
            {
                return HttpNotFound();
            }
            return View(tVaccination);
        }

        public ActionResult PetVaccinations(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["intPetID"] = id;
            var petName = db.TPets.Where(x => x.intPetID == id).Select(x => x.strPetName).FirstOrDefault();
            List<Vaccination> tVaccinations = (from vc in db.TVaccinations
                           join vs in db.TVisitServices
                           on vc.intVisitServiceID equals vs.intVisitServiceID
                           join v in db.TVisits
                           on vs.intVisitID equals v.intVisitID
                           where v.intPetID == id
                           select new Vaccination
                           {
                               intVaccinationID = vc.intVaccinationID,
                               intVisitServiceID = vc.intVisitServiceID,
                               dtmDateOfVaccination = vc.dtmDateOfVaccination,
                               dtmDateOfExpiration = vc.dtmDateOfExpiration,
                               strVaccineDesc = vc.strVaccineDesc,
                               strRabiesNumber = vc.strRabiesNumber
                           }).ToList();

            if (petName == null)
            {
                return HttpNotFound();
            }
            ViewBag.PetName = petName;

            return View(tVaccinations);
        }

        // POST: TVaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TVaccination tVaccination = db.TVaccinations.Find(id);
            db.TVaccinations.Remove(tVaccination);
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
    }
}
