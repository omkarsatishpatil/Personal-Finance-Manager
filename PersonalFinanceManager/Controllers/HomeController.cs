using PersonalFinanceManager.Models;
using PersonalFinanceManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class HomeController : Controller
    {
        private PFMDBEntities7 db = new PFMDBEntities7();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to WalletWise, a Personal Finance Manager!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserCount()
        {
            // Query to count users
            var userCount = db.Database.SqlQuery<int>(
                "SELECT COUNT(*) FROM userInfo").Single();

            // Query to count transactions
            var transactionCount = db.Database.SqlQuery<int>(
                "SELECT COUNT(*) FROM TransactionHistory").Single();

            // Create a model to hold both counts
            var model = new UserTransactionCounts
            {
                UserCount = userCount,
                TransactionCount = transactionCount
            };

            // Pass the model to the partial view
            return PartialView("_UserCount", model);
        }


    }
}