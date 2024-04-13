using System.Collections.Generic;
using System.Web.Mvc;
using LateNight.Models;
using System.Linq; // Necessary for LINQ operations

namespace LateNight.Controllers
{
    public class HomeController : Controller
    {
        static List<Expense> expenses = new List<Expense>();
        static decimal monthlyBudget = 0; // Static field to store the monthly budget
        static bool isBudgetSet = false; // Flag to check if budget is already set

        public ActionResult Index()
        {
            ViewBag.TotalExpenses = expenses.Sum(e => e.Amount);
            ViewBag.Budget = monthlyBudget;
            ViewBag.OverBudget = ViewBag.TotalExpenses > ViewBag.Budget ? ViewBag.TotalExpenses - ViewBag.Budget : 0;
            ViewBag.IsBudgetSet = isBudgetSet; // Ensure this is always set

            return View(expenses);
        }
        public ActionResult About()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            expenses.Add(expense);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetBudget(decimal budget)
        {
            if (!isBudgetSet) // Only set the budget if it hasn't been set before
            {
                monthlyBudget = budget;
                isBudgetSet = true;
            }
            return RedirectToAction("Index");
        }
    }
}