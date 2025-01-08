using PersonalFinanceManager.Models;
using PersonalFinanceManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class DashboardController : Controller
    {
        private PFMDBEntities7 db = new PFMDBEntities7();
        // GET: Dashboard
        public ActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to the SignIn page if the user is not authenticated
                return RedirectToAction("SignIn", "Authentication");
            }

            // If the user is authenticated, proceed to the dashboard
            var userName = User.Identity.Name;
            var user = db.userInfoes.FirstOrDefault(u => u.userName == userName);

            if (user == null)
            {
                // Optionally handle the case where user data is not found
                return RedirectToAction("SignIn", "Authentication");
            }

            // Pass the user details to the view
            return View(user);
        }

        public ActionResult AddIncome() { 
            return View();
        }

        public ActionResult WalletBalance(string userName)
        {
            var totalIncome = db.Incomes
                   .Where(i => i.userName == userName)
                   .Select(i => (decimal?)i.Amount)
                   .Sum() ?? 0;

            var totalExpense = db.Expenses
                                 .Where(e => e.userName == userName)
                                 .Select(e => (decimal?)e.Amount)
                                 .Sum() ?? 0;


            var model = new CalculateBalance
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            };

            return PartialView("_WalletBalance", model);
        }

        public ActionResult TransactionHistory(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Query the SQL view directly
            var transactionHistory = db.Database.SqlQuery<TransactionHistory>(
                "SELECT TransactionDate, TransactionType, Description, Amount, Category FROM TransactionHistory WHERE userName = @p0 ORDER BY TransactionDate DESC",
                userName).ToList();

            // Check if any transactions were found
            if (transactionHistory == null || !transactionHistory.Any())
            {
                return HttpNotFound();
            }

            var userInfo = db.userInfoes.FirstOrDefault(u => u.userName == User.Identity.Name);
            ViewBag.UserInfo = userInfo;
            ViewBag.UserName = userInfo.userName;

            // Pass the data to the view
            return View(transactionHistory);
        }




    }
}