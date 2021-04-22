using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using test;
using SmtpClient = System.Net.Mail.SmtpClient;





namespace test.Controllers {
public class TOwnersController : Controller {
private CapstoneEntities db = new CapstoneEntities();





// GET: TOwners
public ActionResult Index()
{
var tOwners = db.TOwners.Include(t => t.TState);
return View(tOwners.ToList());
}





// GET: TOwners/Details/5
public ActionResult Details(int? id) {
if (id == null) {
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
TOwner tOwner = db.TOwners.Find(id);
if (tOwner == null) {
return HttpNotFound();
}
return View(tOwner);
}





// GET: TOwners/Create
public ActionResult Create() {
ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateCode");
ViewBag.intUserID = new SelectList(db.TUsers, "intUserID", "strUserName");
ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender");
return View();
}





// POST: TOwners/Create
// To protect from overposting attacks, enable the specific properties you want to bind to, for
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "intOwnerID,strFirstName,strLastName,intGenderID,strAddress,strCity,intStateID,strZip,strPhoneNumber,strEmail,strOwner2Name,strOwner2PhoneNumber,strOwner2Email,strNotes")] TOwner tOwner) {
if (ModelState.IsValid) {
ObjectParameter strUserName = new ObjectParameter("strUserName", typeof(string));
ObjectParameter strPassword = new ObjectParameter("strPassword", typeof(string));

var data = db.uspAddUserOwner(strUserName, strPassword, tOwner.strFirstName, tOwner.strLastName, tOwner.intGenderID, tOwner.strAddress, tOwner.strCity, tOwner.intStateID, tOwner.strZip, tOwner.strPhoneNumber,tOwner.strEmail, tOwner.strOwner2Name, tOwner.strOwner2PhoneNumber, tOwner.strOwner2Email, tOwner.strNotes);





string UserName = Convert.ToString(strUserName.Value);
string Password = Convert.ToString(strPassword.Value);





string from = "capstonepets2021@gmail.com";
string to = tOwner.strEmail;
MailMessage mm = new MailMessage(from, to);
mm.Subject = "Capstone Pets - Login Credentials";
mm.Body = "[ Username: " + UserName + " ] [ Password: " + Password + " ]";
mm.IsBodyHtml = false;





SmtpClient smtp = new SmtpClient();
smtp.Host = "smtp.gmail.com";
smtp.Port = 587;
smtp.EnableSsl = true;





NetworkCredential nc = new NetworkCredential("capstonepets2021@gmail.com", "capstonepets21");
smtp.UseDefaultCredentials = true;
smtp.Credentials = nc;
smtp.Send(mm);





return RedirectToAction("Index");
}





ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateCode", tOwner.intStateID);
return View(tOwner);
}





// GET: TOwners/Edit/5
public ActionResult Edit(int? id) {
if (id == null) {
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
TOwner tOwner = db.TOwners.Find(id);
if (tOwner == null) {
return HttpNotFound();
}
ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateCode", tOwner.intStateID);
ViewBag.intUserID = new SelectList(db.TUsers, "intUserID", "strUserName", tOwner.intUserID);
ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tOwner.intGenderID);
return View(tOwner);
}





// POST: TOwners/Edit/5
// To protect from overposting attacks, enable the specific properties you want to bind to, for
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit([Bind(Include = "intOwnerID,strFirstName,strLastName,intGenderID,strAddress,strCity,intStateID,strZip,strPhoneNumber,strEmail,strOwner2Name,strOwner2PhoneNumber,strOwner2Email,strNotes,intUserID")] TOwner tOwner) {
if (ModelState.IsValid) {
db.Entry(tOwner).State = EntityState.Modified;
db.SaveChanges();
return RedirectToAction("Index");
}
ViewBag.intStateID = new SelectList(db.TStates, "intStateID", "strStateCode", tOwner.intStateID);
ViewBag.intUserID = new SelectList(db.TUsers, "intUserID", "strUserName", tOwner.intUserID);
ViewBag.intGenderID = new SelectList(db.TGenders, "intGenderID", "strGender", tOwner.intGenderID);






return View(tOwner);
}





// GET: TOwners/Delete/5
public ActionResult Delete(int? id) {
if (id == null) {
return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
}
TOwner tOwner = db.TOwners.Find(id);
if (tOwner == null) {
return HttpNotFound();
}
return View(tOwner);
}





// POST: TOwners/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id) {
TOwner tOwner = db.TOwners.Find(id);
db.TOwners.Remove(tOwner);
db.SaveChanges();
return RedirectToAction("Index");
}





protected override void Dispose(bool disposing) {
if (disposing) {
db.Dispose();
}
base.Dispose(disposing);
}
}
}