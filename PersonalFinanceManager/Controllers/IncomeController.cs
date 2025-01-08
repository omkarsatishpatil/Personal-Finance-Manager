using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Controllers
{
    public class IncomeController : Controller
    {
        private PFMDBEntities7 db = new PFMDBEntities7();

        // GET: Income/IncomeList
        public ActionResult IncomeList(string userName)
        {
            ViewBag.UserName = userName;
            var incomes = db.Incomes.Where(i => i.userName == userName).ToList(); // Filter by userName
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);  // or however you're getting the current user's info
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(incomes);
        }

        // GET: Income/Details/5
        public ActionResult Details(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(income);
        }

        // GET: Income/AddIncome
        public ActionResult AddIncome(string userName)
        {
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIncome([Bind(Include = "IncomeId,Source,Amount,DateReceived,Notes,userName")] Income income)
        {
            if (ModelState.IsValid)
            {
                db.Incomes.Add(income);
                db.SaveChanges();
                return RedirectToAction("IncomeList", new { userName = income.userName }); // Redirect to the filtered list
            }

            return View(income);
        }

        // GET: Income/Edit/5
        public ActionResult EditIncome(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(income);
        }

        // POST: Income/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIncome([Bind(Include = "IncomeId,Source,Amount,DateReceived,Notes,userName")] Income income)
        {
            if (ModelState.IsValid)
            {
                db.Entry(income).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IncomeList", new { userName = income.userName }); // Redirect to the filtered list
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(income);
        }

        // GET: Income/Delete/5
        public ActionResult Delete(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Incomes.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(income);
        }

        // POST: Income/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string userName)
        {
            Income income = db.Incomes.Find(id);
            db.Incomes.Remove(income);
            db.SaveChanges();
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return RedirectToAction("IncomeList", new { userName = userName }); // Redirect to the filtered list
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
