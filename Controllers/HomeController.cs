using System.Collections.Generic;
using System.Web.Mvc;
using LateNight.Models;
namespace LaneNight.Controllers
{
    public class HomeController : Controller
    {
        static List<Expense> expenses = new List<Expense>();

        public ActionResult Index()
        {
            return View(expenses);
        }

        [HttpPost]
        public ActionResult AddExpense(Expense expense)
        {
            expenses.Add(expense);
            return RedirectToAction("Index");
        }
    }
}