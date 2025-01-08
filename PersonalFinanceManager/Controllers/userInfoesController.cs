using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Controllers
{
    
    public class userInfoesController : Controller
    {
        private PFMDBEntities7 db = new PFMDBEntities7();

        // GET: userInfoes
        public ActionResult Index()
        {
            return View(db.userInfoes.ToList());
        }

        // GET: userInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // GET: userInfoes/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: userInfoes/SignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "userId,userName,userPassword,userFullName,userMobile,userEmail")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.userInfoes.Add(userInfo);
                db.SaveChanges();
                return RedirectToAction("SignIn");
            }

            return View(userInfo);
        }

        // GET: userInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: userInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,userName,userPassword,userFullName,userMobile,userEmail")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }

        // GET: userInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfoes.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: userInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userInfo userInfo = db.userInfoes.Find(id);
            db.userInfoes.Remove(userInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SignIn()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public ActionResult SignIn([Bind(Include = "userName,userPassword")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists in the database
                var user = db.userInfoes.FirstOrDefault(u => u.userName == userInfo.userName && u.userPassword == userInfo.userPassword);

                if (user != null)
                {
                    // You can use FormsAuthentication to log in the user
                    FormsAuthentication.SetAuthCookie(user.userName, false);
                    
                    // Redirect to a secure page after successful login
                    return RedirectToAction("Index","Dashboard");
                }
                else
                {
                    // If the credentials are invalid, add a model error
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed; redisplay the form
            return View(userInfo);
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
