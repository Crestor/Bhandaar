using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;

namespace ERP_Component_DAL.Services
{
    public class UserServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;
        //private Microsoft.Extensions.Configuration.IConfiguration configuration1;

        public UserServices(IConfiguration config)
        {
            this.configuration = config;
        }

        //public UserServices(Microsoft.Extensions.Configuration.IConfiguration configuration)
        //{
        //    this.configuration1 = configuration;
        //}

        public User Authentication(User users)
        {
            try
            {
               User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select loginId, UserName,Password,RoleId from LoginCredentials where UserName = @username And Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = users.password;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0;
                                user.EmployeeID = reader["loginId"] != DBNull.Value ? (Guid)(reader["loginId"]) : Guid.Empty;


                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


     
        public User HandleAdmin(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,UserRole,FirstName,LastName from Users where UserName = @username And Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = users.password;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["UserRole"] != DBNull.Value ? (int)reader["UserRole"] : 0;
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }

        public User HandleManager(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,Role from Users where UserName = @username ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["Role"] != DBNull.Value ? (int)reader["Role"] : 0;


                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }

        public User HandleUsers(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,RoleId from LoginCredentials where UserName = @username ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0;


                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }


        public bool UpdatePassword(User login)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"UPDATE LoginCredentials  SET password = '{login.NewUsername}' WHERE UserName = '{login.userName}'";

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }



        public bool UsernameExists(string username)
        {
            bool exists = false;
            string query = "SELECT 1 FROM LoginCredentials WHERE Username = @Username";
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        exists = reader.HasRows;
                    }
                }

                return exists;
            }

        }
        public bool UpdateUsername(User login)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"UPDATE LoginCredentials  SET UserName = '{login.NewUsername}' WHERE UserName = '{login.userName}' ";

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool addUser(User user)
        {
            try
            {


                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = $"Insert into Users([FullName],[ContactNumber],[Department],[IdentificatioProof],[ProofID])  values(@FullName,@ContactNumber,@Department,@IdProof,@ProofDetail)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value = user.Fullname;
                        cmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar).Value = user.contactNumber;
                        cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = user.department;
                        cmd.Parameters.Add("@IdProof", SqlDbType.VarChar).Value = user.proofID;
                        cmd.Parameters.Add("@ProofDetail", SqlDbType.VarChar).Value = user.iddetails;
                        connection.Open();
                        cmd.ExecuteNonQuery();



                    }
                    return true;
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }

        public bool addDepartment(Departments dept)
        {
            try
            {


                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = $"Insert into Departments  values(@departmentName, @isActive, @departmentId)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@departmentName", SqlDbType.VarChar).Value = dept.departmentName;
                        cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = 1;
                        cmd.Parameters.Add("@departmentId", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();

                        connection.Open();
                        cmd.ExecuteNonQuery();



                    }
                    return true;
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Department details", ex);
            }
        }

        public List<Departments> GetDepartments()
        {
            try
            {
                List<Departments> dept = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT departmentID , departmentName , DepartmentGuid from Departments";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dept.Add(new Departments()
                    {
                        departmentId = reader["departmentID"] != DBNull.Value ? Convert.ToInt32(reader["departmentID"]) : 0,

                        departmentName = reader["departmentName"] != DBNull.Value ? (string)reader["departmentName"] : string.Empty,
                        DepartmentGuid = reader["DepartmentGuid"] != DBNull.Value ? (Guid)reader["DepartmentGuid"] : Guid.Empty

                    });
                }

                return dept;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }

        public List<User> GetEmployees()
        {
            try
            {
                List<User> employees = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT e.employeeid, e.FullName , d.departmentName, e.contactNumber,e.uniqueID,e.uniqueIDDetails from Employees e join departments d on d.departmentid = e.departmentid";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new User()
                    {
                        EmployeeID = reader["employeeID"] != DBNull.Value ? (Guid)(reader["employeeID"]) : Guid.Empty,
                        contactNumber = reader["contactNumber"] != DBNull.Value ? (string)(reader["contactNumber"]) : string.Empty,

                        Fullname = reader["FullName"] != DBNull.Value ? (string)reader["FullName"] : string.Empty,
                        department = reader["departmentName"] != DBNull.Value ? (string)reader["departmentName"] : string.Empty,
                        proofID = reader["uniqueID"] != DBNull.Value ? (string)reader["uniqueID"] : string.Empty,
                        iddetails = reader["uniqueIDDetails"] != DBNull.Value ? (string)reader["uniqueIDDetails"] : string.Empty,
                     

                    });
                }

                return employees;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public bool addEmployee(User user)
        {
            try
            {


                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = $"Insert into Employees([FullName],[ContactNumber],[departmentid],[UniqueID],[UniqueIDDetails])  values(@FullName,@ContactNumber,@DepartmentID,@UniqueID,@UniqueIDDetails)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@FullName", SqlDbType.VarChar).Value = user.Fullname;
                        cmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar).Value = user.contactNumber;
                        cmd.Parameters.Add("@DepartmentID", SqlDbType.VarChar).Value = user.departmentid;
                        cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar).Value = user.proofID;
                        cmd.Parameters.Add("@UniqueIDDetails", SqlDbType.VarChar).Value = user.iddetails;
                        connection.Open();
                        cmd.ExecuteNonQuery();



                    }
                    return true;
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }


        public Departments  GetDepartmentbyID(int employeeID)
        {
            try
            {
                Departments dept = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT departmentID , departmentName from Departments where departmentid = '{employeeID}'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    dept.departmentId = reader["departmentID"] != DBNull.Value ? Convert.ToInt32(reader["departmentID"]) : 0;

                    dept.departmentName = reader["departmentName"] != DBNull.Value ? (string)reader["departmentName"] : string.Empty;

                 
                }

                return dept;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateDepartment(Departments dept)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Departments set  departmentName = @deptName  where departmentID = @deptID";
                cmd.Parameters.AddWithValue("@deptName", dept.departmentName);
                cmd.Parameters.AddWithValue("@deptID", dept.departmentId);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public bool DeleteDepartment(int departmentId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  Departments  where departmentID = {departmentId}";

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}




