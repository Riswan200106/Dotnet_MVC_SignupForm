using System;
using System.Linq;
using System.Web.Mvc;
using SignupForm.DAL;
using SignupForm.Models;

namespace SignupForm.Controllers
{
    public class AccountController : Controller
    {
        Users_DAL _usersDAL = new Users_DAL();

        // GET: Account
        public ActionResult Index()
        {
            try
            {
                var userList = _usersDAL.GetAllUsers();
                return View(userList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching users: " + ex.Message;
                return View();
            }
        }

        // Create method
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usersDAL.AddUser(user); // AddUser method to insert data into the database
                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while creating user: " + ex.Message;
            }
            return View(user);
        }

        // Action to delete a user
        // GET: Account/Delete
        public ActionResult Delete(int id)
        {
            try
            {
                var user = _usersDAL.GetAllUsers().FirstOrDefault(u => u.UserID == id);

                if (user == null)
                {
                    TempData["InfoMessage"] = "User not available with Id " + id.ToString();
                    return RedirectToAction("Index");
                }

                return View(user); // Render a confirmation view for deletion
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while fetching user: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Account/Delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                bool isDeleted = _usersDAL.DeleteUser(id); // Call the DAL to delete the user

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "User deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the user.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while deleting user: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        // GET: Account/Details
        public ActionResult Details(int id)
        {
            try
            {
                // Fetch user by ID from the DAL (Data Access Layer)
                var user = _usersDAL.GetAllUsers().FirstOrDefault();

                if (user == null)
                {
                    TempData["InfoMessage"] = "User not available with Id " + id.ToString();
                    return RedirectToAction("Index"); // Redirect to Index if the user is not found
                }

                return View(user); // Render the details page with user data
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message; // If there's an error, show the error message
                return RedirectToAction("Index");
            }
        }



    }
}
