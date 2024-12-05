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

        /// <summary>
        /// Displays the list of all signup users list
        /// </summary>
        /// <returns> return to the index view with list of users list if it is success
        /// if an error occur return the empty list with an error message 
        /// </returns>
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

        /// <summary>
        /// Displays the Create view for submitting a user details to signup.
        /// </summary>
        /// <returns> Returns the Create view. </returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the post request of the signup user
        /// </summary>
        /// <param name="user">The model object contains the form data</param>
        /// <returns> Redirects to the Index action on success. 
        /// If an error occurs, returns to the Create view with an error message.
        /// </returns>
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

        /// <summary>
        /// Display the delete confirmation view of the details to be deleted
        /// </summary>
        /// <param name="id"> unique identifier that is need to be deleted</param>
        /// <returns> return the delete confirmation view with details
        /// if no id is founded it return to index with an error message
        /// </returns>
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
                return View(user); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while fetching user: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Handles the deletion of user details of selected id
        /// </summary>
        /// <param name="id"> unique identifier of user to be deleted</param>
        /// <returns> Returns to the index view after attempting the deletion
        /// if deleted show the success message otherwise show the error message
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                bool isDeleted = _usersDAL.DeleteUser(id); 
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


        /// <summary>
        /// Displays the details of the selected id user details
        /// </summary>
        /// <param name="id">unique identifier to be selected</param>
        /// <returns> return the detail view of user details
        /// if student not founded it show an error message and redirect to index view
        /// </returns>
        public ActionResult Details(int id)
        {
            try
            {
                // Fetch user by ID from the DAL (Data Access Layer)
                var user = _usersDAL.GetAllUsers().FirstOrDefault(u => u.UserID == id);

                if (user == null)
                {
                    TempData["InfoMessage"] = "User not available with Id " + id.ToString();
                    return RedirectToAction("Index"); 
                }
               return View(user); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message; 
                return RedirectToAction("Index");
            }
        }
    }
}
