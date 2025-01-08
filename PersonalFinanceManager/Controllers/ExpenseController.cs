using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Controllers
{
    public class ExpenseController : Controller
    {
        private PFMDBEntities7 db = new PFMDBEntities7();

        // GET: Expense
        public ActionResult ExpenseList(string userName)
        {
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            var expenses = db.Expenses.Where(e => e.userName == userName).ToList(); // Filter by userName
            return View(expenses);
        }

        // GET: Expense/Details/5
        public ActionResult Details(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expens expens = db.Expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(expens);
        }

        // GET: Expense/Create
        public ActionResult AddExpense(string userName)
        {
            // Fetch the list of categories from the database
            var categories = db.ExpenseCategories
                                .Select(c => new SelectListItem
                                {
                                    Value = c.CategoryID.ToString(),
                                    Text = c.CategoryName
                                }).ToList();

            // Pass the list to the view using ViewBag or ViewData
            ViewBag.CategoryList = categories;

            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpense([Bind(Include = "ExpenseID,ExpenseName,Amount,Category,ExpenseDate,Notes,userName")] Expens expens)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the category name based on the selected category ID
                var selectedCategory = db.ExpenseCategories
                                         .FirstOrDefault(c => c.CategoryID.ToString() == expens.Category);

                if (selectedCategory != null)
                {
                    // Store the category name instead of the ID
                    expens.Category = selectedCategory.CategoryName;
                }

                db.Expenses.Add(expens);
                db.SaveChanges();

                var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
                ViewBag.UserInfo = userInfo;
                ViewBag.UserName = userInfo.userName;

                return RedirectToAction("ExpenseList", new { userName = expens.userName });
            }

            // Reload the category list if there's an error
            ViewBag.CategoryList = db.ExpenseCategories
                                      .Select(c => new SelectListItem
                                      {
                                          Value = c.CategoryID.ToString(),
                                          Text = c.CategoryName
                                      }).ToList();

            return View(expens);
        }


        // GET: Expense/Edit/5
        public ActionResult EditExpense(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expens expens = db.Expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }

            // Retrieve the category list and set the selected category based on the stored value
            var categories = db.ExpenseCategories
                               .Select(c => new SelectListItem
                               {
                                   Value = c.CategoryName, // Store the CategoryName as the value
                                   Text = c.CategoryName,
                                   Selected = c.CategoryName == expens.Category // Mark the saved category as selected
                               }).ToList();

            ViewBag.CategoryList = categories;

            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(expens);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExpense([Bind(Include = "ExpenseID,ExpenseName,Amount,Category,ExpenseDate,Notes,userName")] Expens expens)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the category name based on the selected category ID
                var selectedCategory = db.ExpenseCategories
                                         .FirstOrDefault(c => c.CategoryID.ToString() == expens.Category);

                if (selectedCategory != null)
                {
                    // Store the category name instead of the ID
                    expens.Category = selectedCategory.CategoryName;
                }

                db.Entry(expens).State = EntityState.Modified;
                db.SaveChanges();
                var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
                ViewBag.UserInfo = userInfo;
                ViewBag.UserName = userInfo.userName;
                return RedirectToAction("ExpenseList", new { userName = expens.userName });
            }

            // Reload the category list if there's an error
            ViewBag.CategoryList = db.ExpenseCategories
                                      .Select(c => new SelectListItem
                                      {
                                          Value = c.CategoryID.ToString(),
                                          Text = c.CategoryName
                                      }).ToList();

            return View(expens);
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int? id, string userName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expens expens = db.Expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return View(expens);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string userName)
        {
            Expens expens = db.Expenses.Find(id);
            db.Expenses.Remove(expens);
            db.SaveChanges();
            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;
            return RedirectToAction("ExpenseList", new { userName = userName });
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
