using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MovieList2.Models;

namespace MovieList2.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        string connectionString = @"Data Source = MSI\SQLDEV2019; Initial Catalog = MovieList; Integrated Security=True";
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblMovie = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Movies", sqlCon);
                sqlDa.Fill(dtblMovie) ;
            }
            return View(dtblMovie);
        }

        // GET: Movie/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new MovieModels());
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(MovieModels movieModels)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Movies VALUES(@Name,@Year,@Director)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Name", movieModels.Name);
                sqlCmd.Parameters.AddWithValue("@Year", movieModels.Year);
                sqlCmd.Parameters.AddWithValue("@Director", movieModels.Director);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            MovieModels movieModels = new MovieModels();
            DataTable dtblMovie = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "SELECT * FROM Movies WHERE id=@id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@id", id);
                sqlDa.Fill(dtblMovie);
            }
            if(dtblMovie.Rows.Count == 1)
            {
                movieModels.id = Convert.ToInt32(dtblMovie.Rows[0][0].ToString());
                movieModels.Name = dtblMovie.Rows[0][1].ToString();
                movieModels.Year = dtblMovie.Rows[0][2].ToString();
                movieModels.Director = dtblMovie.Rows[0][3].ToString();
                return View(movieModels);
            }
            else
            {
                return View("Index");
            }
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(MovieModels movieModels)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Movies SET Name = @Name,Year = @Year,Director = @Director WHERE @id = id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id", movieModels.id);
                sqlCmd.Parameters.AddWithValue("@Name", movieModels.Name);
                sqlCmd.Parameters.AddWithValue("@Year", movieModels.Year);
                sqlCmd.Parameters.AddWithValue("@Director", movieModels.Director);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Movies WHERE @id = id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
