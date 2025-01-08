using PersonalFinanceManager.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalFinanceManager.Controllers
{
    public class AuthenticationController : Controller
    {
        private PFMDBEntities1 db = new PFMDBEntities1();

        // GET: userInfoes/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: userInfoes/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "userId,userName,userPassword,userFullName,userMobile,userEmail")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                // Check if the username or email already exists
                bool userExists = db.userInfoes.Any(u => u.userName == userInfo.userName || u.userEmail == userInfo.userEmail);
                if (userExists)
                {
                    ModelState.AddModelError("", "Username or email already exists.");
                    return View(userInfo);
                }

                // Hash the password before saving
                userInfo.userPassword = HashPassword(userInfo.userPassword);

                db.userInfoes.Add(userInfo);
                db.SaveChanges();
                return RedirectToAction("SignIn");
            }

            return View(userInfo);
        }

        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to the Dashboard if the user is already authenticated
                return RedirectToAction("Dashboard", "Dashboard");
            }
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public ActionResult SignIn([Bind(Include = "userName,userPassword,userEmail")] userInfo userInfo, bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                // Hash the input password
                string hashedPassword = HashPassword(userInfo.userPassword);

                // Check if the user exists in the database
                var user = db.userInfoes
                    .FirstOrDefault(u => u.userName == userInfo.userName || u.userEmail == userInfo.userName && u.userPassword == hashedPassword);

                if (user != null)
                {
                    // Use FormsAuthentication to log in the user
                    FormsAuthentication.SetAuthCookie(user.userName, rememberMe);

                    // Store user details in TempData to pass to Dashboard
                    TempData["User"] = user;

                    // Redirect to a secure page after successful login
                    return RedirectToAction("Dashboard", "Dashboard");
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

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: userInfoes/Edit/5
        public ActionResult EditProfile(int? id)
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
        public ActionResult EditProfile([Bind(Include = "userId,userName,userPassword,userFullName,userMobile,userEmail")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving
                userInfo.userPassword = HashPassword(userInfo.userPassword);
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View(userInfo);
        }

        // GET: userInfoes/Delete/5
        public ActionResult DeleteProfile(int? id)
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
        [HttpPost, ActionName("DeleteProfile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userInfo userInfo = db.userInfoes.Find(id);
            db.userInfoes.Remove(userInfo);
            db.SaveChanges();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // Utility method to hash passwords
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
