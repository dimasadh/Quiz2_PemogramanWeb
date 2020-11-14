using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieList2.Models;
using System.Data.SqlClient;
using System.Data;

namespace MovieList2.Controllers
{
    public class UserController : Controller
    {
        string connectionString = @"Data Source = MSI\SQLDEV2019; Initial Catalog = MovieList; Integrated Security=True";

        // POST: User
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            UserModels userModel = new UserModels();
            return View(userModel);
        }
        [HttpPost]
        public ActionResult AddOrEdit(UserModels userModels)
        {
            DataTable dtblUser = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Users WHERE Username=@Username";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Username", userModels.Username);
                sqlDa.Fill(dtblUser);

                if (dtblUser.Rows.Count == 0)
                {
                    string query2 = "INSERT INTO Users VALUES(@Username,@Password)";
                    SqlCommand sqlCmd = new SqlCommand(query2, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Username", userModels.Username);
                    sqlCmd.Parameters.AddWithValue("@Password", userModels.Password);
                    sqlCmd.ExecuteNonQuery();
                }
                else
                {
                    ViewBag.DuplicateMessage = "Username already exist.";
                    return View("AddOrEdit", userModels);
                }
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";
            return RedirectToAction("Index", "Movie");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(UserModels userModels)
        {
            DataTable dtblUser = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Users WHERE Username=@Username and Password=@Password";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Username", userModels.Username);
                sqlDa.SelectCommand.Parameters.AddWithValue("@Password", userModels.Password);
                sqlDa.Fill(dtblUser);

                if (dtblUser.Rows.Count == 1)
                {
                    Session["userID"] = userModels.UserID;
                    ViewBag.LoginSuccessMessage = "Login Successfull";
                    return RedirectToAction("Index", "Movie");
                }
                else
                {
                    ViewBag.LoginFailedMessage = "Wrong Username or Password!";
                    return View("Login", userModels);
                }
            }
        }

        public ActionResult Logout ()
        {
            Session["userID"] = null;
            return RedirectToAction("Login", "User");
        }
    }
}