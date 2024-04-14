using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LateNight.Models;
using System.Linq;
using MySql.Data.MySqlClient; // Necessary for LINQ operations
using System.Data;
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
 
       
        
            [HttpGet]
            public ActionResult Login()
            {
                return View(new User()); // Pass a new User model to the view
            }

            [HttpPost]
            public ActionResult Login(User user)
            {
                if (ModelState.IsValid)
                {
                    if (AuthenticateUser(user.Username, user.Password))
                    {
                        // Authentication successful, proceed to redirect or set session
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                return View(user);
            }

            private bool AuthenticateUser(string username, string password)
            {
                string connectionString = "server=localhost;port=3306;database=secondattempt;user=root;password=root";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT COUNT(1) FROM users WHERE username = @username AND password = @password";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        return result == 1;
                    }
                }
            }
            
  
     [HttpGet]
    public ActionResult Signup()
    {
        return View();
    }

    // POST: Handle the Signup
    [HttpPost]
    public ActionResult Signup(string newUsername, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            ViewBag.Message = "Password and Confirm Password do not match.";
            ViewBag.MessageType = "error"; // Indicate that this is an error message
            return View();
        }

        if (IsUsernameExists(newUsername))
        {
            ViewBag.Message = "Username already exists. Please choose another username.";
            ViewBag.MessageType = "error"; // Indicate that this is an error message
            return View();
        }

        if (InsertNewUser(newUsername, newPassword))
        {
            ViewBag.Message = "Signed up successfully.";
            ViewBag.MessageType = "success"; // Indicate that this is a success message
            return View();
        }

        ViewBag.Message = "Unable to register. Please try again.";
        ViewBag.MessageType = "error"; // Indicate that this is an error message
        return View();
    }

    private bool IsUsernameExists(string username)
    {
        string connectionString = "server=localhost;port=3306;database=secondattempt;user=root;password=root";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT COUNT(1) FROM users WHERE username = @username";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result >= 1;
            }
        }
    }

    private bool InsertNewUser(string username, string password)
    {
        string connectionString = "server=localhost;port=3306;database=secondattempt;user=root;password=root";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO users (username, password) VALUES (@username, @password)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
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