using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using System.Web.Security;
using System.Data.Entity;
using System.Net;
using System.Data.SqlClient;
using System.Drawing;
using test.Models;

namespace test.Controllers {
	public class HomeController : Controller {

		private CapstoneEntities db = new CapstoneEntities();


		public ActionResult Index(int? id) {
			var owner = db.TOwners
				.Include(t => t.TPets)
				.Include(t => t.TGender)
				.Include(t => t.TState);

			if (id == null) { Logout(); return RedirectToAction("Login", "Home"); }
			TUser user = new TUser();
			user.intUserID = (int)id;
			var intRoleID = user.intRoleID;


			if (intRoleID == 1) {
				return RedirectToAction("OwnerHome", new { @id = id });
				//	return RedirectToAction("OwnerHome", "Home");
			}
			else if (intRoleID == 2) {
				return RedirectToAction("Index", "Home", new { @id = id });
			}
			else {
				ViewBag.ErrorMessage = "Unauthorized Role Assignment. Please contact the Help Desk.";
			}
			return View();
		}

		public ActionResult Home(int? id) {
			var owner = db.TOwners
				.Include(t => t.TPets)
				.Include(t => t.TGender)
				.Include(t => t.TState);
			return View();

			TUser user = new TUser();
			user.intUserID = (int)id;
			var roleID = user.intRoleID;

			if (roleID != 2) {
				ViewBag.ErrorMessage = "Authorized Users Only";
				Logout();
			}


		}



		public ActionResult Login() {
			TUser u = new TUser();
			return View();


		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(TUser objUser) {
			
			if (ModelState.IsValid) {
				using (CapstoneEntities db = new CapstoneEntities()) {
					var obj = db.TUsers.Where(a => a.strUserName.Equals(objUser.strUserName) && a.strPassword.Equals(objUser.strPassword)).FirstOrDefault();
					if (obj != null) {
						Session["intUserID"] = obj.intUserID.ToString();
						Session["strUserName"] = obj.strUserName.ToString();
						Session["intRoleID"] = obj.intRoleID.ToString();
						
						if (obj.intRoleID == 1)
						{
							var ownerinfo = (from o in db.TOwners
											 where o.intUserID == obj.intUserID
											 select new {
												 intOwnerID = o.intOwnerID
											 }).FirstOrDefault();
							var intOwnerID = ownerinfo.intOwnerID;

							return RedirectToAction("OwnerHome", new { @id = intOwnerID });
						}

						if (obj.intRoleID == 2) {
							return RedirectToAction("Index", new { @id = obj.intUserID });
						}
					}
				}
			}
			return View(objUser);

		}


		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Login(FormCollection col) {
		//	CapstoneEntities db = new CapstoneEntities();
		//	try {
		//		TUser u = new TUser();
		//		u.strUserName = col["strUserName"];
		//		u.strPassword = col["strPassword"];

		//		if (u.strUserName.Length == 0 || u.strPassword.Length == 0) {
		//			ViewBag.ErrorMessage = "Missing Required Fields";
		//			return View(u);
		//		}
		//		else {
		//			if (col["btnSubmit"] == "signin") {
		//				//u.UserID = col["UserID"];
		//				//u.Password = col["Password"];
		//				int? intUserID = db.Validate_User9(u.strUserName, u.strPassword).FirstOrDefault();
		//				//var uinfo = db.Database.ExecuteSqlCommand("SELECT * FROM TUsers WHERE intUserID = @intUserID", intUserID);
		//				var uInfo = (from us in db.TUsers
		//								where us.intUserID == intUserID
		//								select new {

		//											 strUserName = us.strUserName,
		//											 strPassword = us.strPassword,
		//											 intRoleID = us.intRoleID,
		//										 }).FirstOrDefault();
		//				u.intUserID = (int)intUserID;
		//				u.strUserName = uInfo.strUserName;
		//				u.strPassword = uInfo.strPassword;
		//				u.intRoleID = uInfo.intRoleID;




		//				if (u != null && u.intUserID > 0) {
		//					u.SaveUserSession();
		//					if (u.intRoleID == 1) {
		//						//int? intOwnerID = db.uspGetOwnerID(intUserID).FirstOrDefault();
		//						var ownerinfo = (from o in db.TOwners
		//										 where o.intUserID == u.intUserID
		//										 select new {
		//											 intOwnerID = o.intOwnerID
		//										 }).FirstOrDefault();
		//						var intOwnerID = ownerinfo.intOwnerID;
		//						return RedirectToAction("OwnerHome", new { @id = intOwnerID });
		//						//	return RedirectToAction("OwnerHome", "Home");
		//					}
		//					else if (u.intRoleID == 2) {
		//						return RedirectToAction("Index", "Home", new { @id = intUserID });
		//					}
		//					else {
		//						ViewBag.ErrorMessage = "Unauthorized Role Assignment. Please contact the Help Desk.";
		//					}
		//					return RedirectToAction("Index");
		//				}
		//				else {
		//					u = new TUser();
		//					u.strUserName = col["strUserName"];
		//					ViewBag.ErrorMessage = "Login Failed. Please try again.";
		//					Logout();
		//				}
		//			}
		//			return View(u);
		//		}
		//	}
		//	catch (Exception) {
		//		TUser u = new TUser();
		//		return View(u);
		//	}
		//}




		public ActionResult OwnerHome(int? id) {
			TOwner owner = new TOwner();
			if (Session["intUserID"] == null) {
				//ViewBag.ErrorMessage = "Authorized Users Only";
				return RedirectToAction("Login");
			}
			else {
				int? intOwnerID = db.uspGetOwnerID3(id).FirstOrDefault();
				OwnerHome ownerHome = new OwnerHome();
				owner = db.TOwners.Include(s => s.TPets).SingleOrDefault(s => s.intOwnerID == id);
				if (Session["intUserID"].ToString() != owner.intUserID.ToString()) 
				{
					//ViewBag.ErrorMessage = "Authorized Users Only";
					return RedirectToAction("Login"); 
				}
				else {
					if (owner.TUser.intRoleID != 1) {
						ViewBag.ErrorMessage = "Authorized Users Only";
						Logout();
					}


					List<PetOwnerImage> list = (from o in db.TOwners
												join p in db.TPets
												 on o.intOwnerID equals p.intOwnerID
												join i in db.TPetImages
												 on p.intPetID equals i.intPetID
												join s in db.TStates
												 on o.intStateID equals s.intStateID
												where o.intOwnerID == owner.intOwnerID
												select new PetOwnerImage {
													intOwnerID = owner.intOwnerID,
													strFirstName = o.strFirstName,
													strLastName = o.strLastName,
													intPetID = p.intPetID,
													strPetName = p.strPetName,
													intPetImageID = i.intPetImageID,
												}).ToList();

					ownerHome.PetImageData = list;

					ownerHome.strFirstName = owner.strFirstName;
					ownerHome.strLastName = owner.strLastName;

					return View(ownerHome);
				}
			}
		}
		private IDisposable petContext() {
			throw new NotImplementedException();
		}


		public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
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

		public ActionResult Settings() {


			return View();


		}

		public ActionResult About() {

			return View();


		}

		public ActionResult Help() {

			return View();


		}

	}
}