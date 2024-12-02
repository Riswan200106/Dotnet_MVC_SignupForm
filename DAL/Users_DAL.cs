using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SignupForm.Models;

namespace SignupForm.DAL
{
    public class Users_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["AccountConnection"].ToString();

        //Function to read user data

        public List<Users> GetAllUsers()
        {
            List<Users> usersList = new List<Users>();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetAllUsers";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataTable dtUsers = new DataTable();

                conn.Open();
                sqlDA.Fill(dtUsers);
                conn.Close();


                foreach (DataRow dr in dtUsers.Rows)
                {
                    usersList.Add(new Users
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["Email"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        Password = dr["Password"].ToString()
                    });
                }
            }

                return usersList;
        }


        //to create new users
        public void AddUser(Users user)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_UserSignup";

                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }



        // to delete a user by UserID
        public bool DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_DeleteUser"; // Ensure you have this stored procedure created

                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
        }


    }

}