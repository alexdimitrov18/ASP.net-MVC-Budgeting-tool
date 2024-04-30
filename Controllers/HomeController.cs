using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LateNight.Models;
using System.Linq;
using MySql.Data.MySqlClient; 
using System.Data;
namespace LateNight.Controllers
{
    public class HomeController : Controller
    {
        static List<Expense> expenses = new List<Expense>();
        static List<decimal> budgets = new List<decimal>(); // A list to store multiple budgets
       

        public ActionResult Index()
        {
            ViewBag.TotalExpenses = expenses.Sum(e => e.Amount);
            ViewBag.TotalBudgets = budgets.Sum(); // Total of all budgets
            ViewBag.Budgets = budgets; // Passing the list of budgets
            ViewBag.OverBudget = ViewBag.TotalExpenses > ViewBag.TotalBudgets ? ViewBag.TotalExpenses - ViewBag.TotalBudgets : 0;

            return View(expenses);
        }
        public ActionResult About()
        {
            return View();
        }
 
       
        
            [HttpGet]
            public ActionResult Login()
            {
                return View(new User()); // Pass a new User to the view
            }

            [HttpPost]
            public ActionResult Login(User user)
            {
                if (ModelState.IsValid)
                {
                    if (AuthenticateUser(user.Username, user.Password))
                    {
                        // If authentication is successful - redirect and set session
                        Session["Username"] = user.Username;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Wrong username or password.";
                        ViewBag.MessageType = "error"; 
                        return View();
                    }
                }
                return View(user);
            }

            private bool AuthenticateUser(string username, string password)
            {
                //fixing this connection string and mysql configuration took me 2 days to fix...
                string connectionString = "server=localhost;port=3306;database=secondattempt;user=root;password=root";
                using (var connection = new MySqlConnection(connectionString))
                {
                    //we make a query to see if we have a matching username and password combo in the database
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
            ViewBag.MessageType = "error"; // we throw an error message
            return View();
        }

        if (IsUsernameExists(newUsername))
        {
            ViewBag.Message = "Username already exists. Please choose another username.";
            ViewBag.MessageType = "error"; // same here
            return View();
        }

        if (InsertNewUser(newUsername, newPassword))
        {
            ViewBag.Message = "Signed up successfully.";
            ViewBag.MessageType = "success"; // if everything is ok, we put it in the DB
            return View();
        }

        ViewBag.Message = "Unable to register. Please try again.";// added a default error, a good practice from the course i was watching
        ViewBag.MessageType = "error"; 
        return View();
    }

    private bool IsUsernameExists(string username) // duplicate username check
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

    private bool InsertNewUser(string username, string password)// separated the insertion for clarity
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
    
    public ActionResult Logout()
    {
        Session.Clear();  // it clears the session and redirects to login again
        return RedirectToAction("Login", "Home");
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
        budgets.Add(budget); // Add new budget to the list
        return RedirectToAction("Index");
    }
    }
}