using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using test;
using test.Models;

namespace test.Controllers {
    public class OwnerHomeController : Controller {
        private CapstoneEntities db = new CapstoneEntities();


        public ActionResult Index(int? id) {

			

				if (id == null) {
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}

				OwnerHome myModel = new OwnerHome();
				int intOwnerID = (int)Session["intOwnerID"];
				//int intPetImageID = (int)Session["intPetImageID"];

				//General Information
				var ownerPetImage = (from i in db.TPetImages
									 join p in db.TPets
									 on i.intPetID equals p.intPetID
									 join o in db.TOwners
									 on p.intOwnerID equals o.intOwnerID
									 where p.intOwnerID == o.intOwnerID
									 select new {
										 ownerName = o.strFirstName + " " + o.strLastName,
										 phoneNumber = o.strPhoneNumber,
										 petName = p.strPetName,
										 intPetID = p.intPetID,										 
										 intPetImageID = i.intPetImageID,//Could be from TPets, not images
										 strFileName = i.strFileName ,
										 imgContent = i.imgContent,
										 strContentType = i.strContentType,
										 strFileType = i.strFileType
									 }).FirstOrDefault();

				//var doctor = (from e in db.TEmployees
				//			  join ve in db.TVisitEmployees
				//			  on e.intEmployeeID equals ve.intEmployeeID
				//			  join j in db.TJobTitles
				//			  on e.intJobTitleID equals j.intJobTitleID
				//			  where ve.intVisitID == intVisitId
				//			  where j.intJobTitleID == 4
				//			  select new {
				//				  doctorName = "Dr. " + e.strFirstName + " " + e.strLastName
				//			  }).FirstOrDefault();

				//Save to model
				//myModel.strOwnerName = ownerPetImage.ownerName;
				//myModel.strPhoneNumber = ownerPetImage.phoneNumber;
				////myModel.intOwnerNumber = ownerPetImage.clientNumber;
				//myModel.strPetName = ownerPetImage.petName;
				//myModel.intPetID = ownerPetImage.intPetID;

				//myModel.intPetImageID = ownerPetImage.intPetImageID;
				//myModel.strFileName = ownerPetImage.strFileName;
				//myModel.imgContent = ownerPetImage.imgContent;
				//myModel.strContentType = ownerPetImage.strContentType;
				//myModel.strFileType = ownerPetImage.strFileType;
				//myModel.PetVisitServices = db.TVisitServices.Where(x => x.intVisitID == intVisitId).ToList();
				//myModel.PetVisitMedications = db.TVisitMedications.Where(x => x.intVisitID == intVisitId).ToList();

				//ViewBag.Name = informationPacket.petName;
				//ViewBag.Total = db.TVisitServices
				//				.Where(x => x.intVisitID == intVisitId)
				//				.Select(z => z.TService.dblPrice)
				//				.DefaultIfEmpty()
				//				.Sum();

				return View(myModel);

				////var pet = db.TPets.Include("TPetImages")
				////		.Where(s => s.TOwner.intOwnerID == id)
				////		.FirstOrDefault<TPet>();

				//TOwner owner = db.TOwners.Include(s => s.TPets).SingleOrDefault(s => s.intOwnerID == id);

				//return View(owner);


		}

		public FileContentResult DisplayImagePage(int id) {
            TPetImage document = db.TPetImages.Find(id);
            return new FileContentResult(document.imgContent, document.strContentType);
        }

        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Settings() {


            return View();


        }

        public ActionResult About() {

            return View();

        }


        public ActionResult Help() {

            return View();

        }


        public ActionResult PetMedication()
        {

            return View();

        }

    }
}
