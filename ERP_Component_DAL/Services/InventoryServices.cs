using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace ERP_Component_DAL.Services
{
    public class InventoryServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;


        public InventoryServices(IConfiguration config)
        {
            this.configuration = config;
        }



        public bool AddCategory(Category category)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO Categories([CategoryName])  VALUES(@CategoryName)";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@CategoryName", category.categoryName);                 

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }


        public List<Category> ViewProductCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from Categories order by categoryId desc";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                       


                    });
                }

                return cat;

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

        public List<Category> ViewMaterialCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from Categories Where Type = 'material'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        categoryCode = reader["CategoryCode"] != DBNull.Value ? (string)reader["CategoryCode"] : string.Empty,
                        categoryDescription = reader["CategoryDescription"] != DBNull.Value ? (string)reader["CategoryDescription"] : string.Empty,
                        isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty,
                        createdOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),


                    });
                }

                return cat;

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






        public Category GetEditCategory(int categoryId)
        {
            try
            {
                Category c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName From Categories where CategoryID = '{categoryId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;
                    c.categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0;

                  
                }


                return c;

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
        
        public bool UpdateCategory(Category category)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Categories set  CategoryName = @CategoryName  where CategoryID = @CategoryID";
                cmd.Parameters.AddWithValue("@CategoryName", category.categoryName);
                cmd.Parameters.AddWithValue("@CategoryID", category.categoryId);
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

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  Categories  where CategoryID = {categoryId}";

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

        //SubCategory

        public bool AddSubCategory(Category category)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO SubCategories([SubCategoryName],[CategoryID])  VALUES('{category.subCategoryName}',{category.categoryId})";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        //cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                        //cmd.Parameters.AddWithValue("@categoryCode", category.categoryCode);
                        //cmd.Parameters.AddWithValue("@categoryDesc", category.categoryDescription);
                        //cmd.Parameters.AddWithValue("@isActive", category.isActive);




                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public List<Category> ViewSubCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId= reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                     


                    });
                }

                return cat;

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

        public List<Category> ViewMaterialSubCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName,sc.SubCategoryCode, sc.SubCategoryDescription, sc.IsActive, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID Where c.Type = 'material'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        categoryCode = reader["SubCategoryCode"] != DBNull.Value ? (string)reader["SubCategoryCode"] : string.Empty,
                        categoryDescription = reader["SubCategoryDescription"] != DBNull.Value ? (string)reader["SubCategoryDescription"] : string.Empty,
                        isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty


                    });
                }

                return cat;

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

        public List<Category> CategoryProductNames()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName from Categories ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        

                    });
                }

                return cat;

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

        public List<Category> CategoryMaterialNames()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName from Categories  ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


                    });
                }

                return cat;

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

        public Category GetEditSubCategory(int subCategoryId)
        {
            try
            {
                Category c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName,sc.SubCategoryCode, sc.SubCategoryDescription, sc.IsActive, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID Where sc.SubCategoryID = {subCategoryId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0;
                    c.categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;
                    c.categoryCode = reader["SubCategoryCode"] != DBNull.Value ? (string)reader["SubCategoryCode"] : string.Empty;
                    c.subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty;
                    c.categoryDescription = reader["SubCategoryDescription"] != DBNull.Value ? (string)reader["SubCategoryDescription"] : string.Empty;
                    c.isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty;

                    c.categoryId = reader["CategoryID"] != DBNull.Value ? (Int32)reader["CategoryID"] : 0;





                }


                return c;

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

        public bool UpdateSubCategory(Category category)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update SubCategories set  SubCategoryName ='{category.subCategoryName}', SubCategoryCode ='{category.categoryCode}', SubCategoryDescription='{category.categoryDescription}' ,IsActive='{category.isActive}'  where SubCategoryID = '{category.SubCategoryId}'";

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


        public bool DeleteSubCategory(int subCategoryId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  SubCategories  where SubCategoryID = {subCategoryId}";

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


       

            public bool AddWarehouses(Warehouse warehouse)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Address ([AddressLine1],[AddressLine2],[Country],[State] ,[City],[Area],[Pincode],[CreatedOn]) OUTPUT Inserted.AddressID values(@address1,@address2,@country,@state,@district,@area,@postalcode,'{DateTime.Now.ToShortDateString()}')";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@address1", warehouse.Address1);
                cmd.Parameters.AddWithValue("@address2", warehouse.Address2);
                cmd.Parameters.AddWithValue("@country", warehouse.Country);
                cmd.Parameters.AddWithValue("@state", warehouse.State);
                cmd.Parameters.AddWithValue("@district", warehouse.District);
                cmd.Parameters.AddWithValue("@area", warehouse.Area);
                cmd.Parameters.AddWithValue("@postalcode", warehouse.PostalCode);
                connection.Open();
                int addressId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"Insert into DistributionCenter([CenterType],[CenterName],[AddressId]) values(@centerType,@centerName,@addressId)";
                cmd2.Connection = connection;
                cmd2.Parameters.AddWithValue("@centerType", warehouse.type);
                cmd2.Parameters.AddWithValue("@centerName", warehouse.warehouseName);
                cmd2.Parameters.AddWithValue("@addressId", addressId);





                connection.Open();
                cmd2.ExecuteNonQuery();
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



        public List<Warehouse> ViewWarehouse()
        {
            try
            {
                List<Warehouse> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.AddressID,ad.Pincode,ad.CreatedOn,dc.CenterName FROM DistributionCenter dc JOIN  Address ad ON dc.AddressID = ad.AddressID Where dc.CenterType=1";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Warehouse()
                    {
                        addressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0,
                        //PostalCode = reader["Pincode"] != DBNull.Value ? ()reader["Pincode"] : 0,
                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,
                       
                        Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,
                        Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

                        PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty,

                    });
                }

                return cat;

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

        public Warehouse GetWarehouse(int addressId)
        {
            try
            {
                Warehouse c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.Pincode,ad.AddressID,ad.CreatedOn,dc.CenterName FROM  DistributionCenter dc JOIN Address ad ON dc.AddressID = ad.AddressID Where ad.AddressID = {addressId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.addressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0;
                   
                    c.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;
                    c.PostalCode = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;
                    c.warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;
                    //c.warehouseType = reader["WarehouseType"] != DBNull.Value ? (string)reader["WarehouseType"] : string.Empty;
                    c.Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    c.Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty;
                    c.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    c.District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                    c.Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty;
                    c.CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime);

                    c.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;





                }


                return c;

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


        public List<Warehouse> getWarehouseName()
        {
            try
            {
                List<Warehouse> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CenterName, CenterId from DistributionCenter Where CenterType = 1";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Warehouse()
                    {
                        warehouseId = reader["CenterId"] != DBNull.Value ? (Guid)reader["CenterId"] : Guid.Empty,


                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,


                    });
                }

                return cat;

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

        //public List<Stock> WarehouseName()
        //{
        //    try
        //    {
        //        List<Stock> cat = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Select WarehouseName from Warehouses Where WarehouseId = ";
        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            cat.Add(new Stock()
        //            {
        //                warehouseId = reader["warehouseId"] != DBNull.Value ? (Guid)reader["warehouseId"] : Guid.Empty,


        //                warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty,


        //            });
        //        }

        //        return cat;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        public List<Items> GetMaterialNamesFromItems()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items where ItemType = 2";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
                }

                return cat;

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


        public List<Items> GetProductNamesFromItems()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items where ItemType = 1";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
                }

                return cat;

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


        public List<Items> GetStockTransaction(Ledger ledger)
        {
            var items = new List<Items>();

            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetItemLedger", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.AddWithValue("@ItemId", ledger.itemID);
                    command.Parameters.AddWithValue("@FromDate", ledger.startDate);
                    command.Parameters.AddWithValue("@ToDate", ledger.endDate);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Items
                            {
                                StockDate = reader["TransactionDate"] as DateTime? ?? DateTime.MinValue,
                                Perticulars = reader["Perticular"] as string ?? string.Empty,
                            
                                stockiin = reader["StockIn"] as int? ?? 0,

                                stockout = reader["StockOut"] as int? ?? 0,
                                quantity = reader["Balance"] as int? ?? 0,
                            });
                        }
                    }
                }
            }
            catch
            {
                // Consider logging the error
                throw;
            }

            return items;
        }


        public List<Items> ExpiryReport()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items where expiryDate < '{DateTime.Now}'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
                }

                return cat;

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


        public List<Items> GetItemsNames()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
                }

                return cat;

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



        //public bool UpdateWarehouse(Warehouse wh)
        //{
        //    try
        //    {
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Update Warehouses set  WarehouseName ='{wh.warehouseName}', WarehouseType ='{wh.warehouseType}'  where AddressID = '{wh.addressId}'";

        //        cmd.Connection = connection;
        //        connection.Open();
        //        cmd.ExecuteScalar();
        //        connection.Close();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        public bool UpdateWarehouse(Warehouse wh)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);

                string query1 = $"UPDATE DistributionCenter SET CenterName = '{wh.warehouseName}'WHERE AddressID = '{wh.addressId}'";

               
                string query2 = $"UPDATE Address SET AddressLine1 = '{wh.Address1}',AddressLine2='{wh.Address2}', State='{wh.State}',City='{wh.District}',Area='{wh.Area}' WHERE AddressID = '{wh.addressId}'";

                SqlCommand cmd = new SqlCommand(query1 + ";" + query2, connection);
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery(); 
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


        public bool DeleteWarehouse(int addressId)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                   
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            
                            SqlCommand cmd1 = new SqlCommand("DELETE FROM DistributionCenter WHERE AddressID = @addressId", connection, transaction);
                            cmd1.Parameters.AddWithValue("@addressId", addressId);
                            cmd1.ExecuteNonQuery();

                            
                            SqlCommand cmd2 = new SqlCommand("DELETE FROM Address WHERE AddressID = @addressId", connection, transaction);
                            cmd2.Parameters.AddWithValue("@addressId", addressId);
                            cmd2.ExecuteNonQuery();

                        
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; 
            }
        }


        public bool AddWarrenty(Warrenty war)
        {
            try
            {
                var x=  int.Parse(Regex.Match(war.PeriodName, @"\d+").Value);

                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Insert into  WarrentyPeriod (PeriodName, PeriodInMonths) VALUES (@warrentyPeriod, @warrentyPeriodinMonth)";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@warrentyPeriod", war.PeriodName);
                cmd.Parameters.AddWithValue("@warrentyPeriodinMonth",  x);
                cmd.CommandTimeout = 300;
                connection.Open();
                cmd.ExecuteNonQuery();


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
        public bool SaveUOMToDatabase(UOM uom)
        {
            try
            {
                
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Insert into  UnitsOfMeasure (UOMCode, UOMName) VALUES (@uomCode, @uomName)";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@uomCode", uom.uomcode);
                cmd.Parameters.AddWithValue("@uomName", uom.uomname);
                cmd.CommandTimeout = 300;
                connection.Open();
                 cmd.ExecuteNonQuery();
              

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
        public List<UOM> getProductUOM()
        {
             try
            {
                List<UOM> uom = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select uomCODE, uomName from UnitsOfMeasure ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    uom.Add(new UOM()
                    {
                        uomcode= reader["uomCODE"] != DBNull.Value ? (string)reader["uomCODE"] : string.Empty,

                        uomname = reader["uomName"] != DBNull.Value ? (string)reader["uomName"] : string.Empty,


                    });
                }

                return uom;

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


        public List<Warrenty> getProductWarrenty()
        {
            try
            {
                List<Warrenty> warrenty = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select warrentyID, PeriodName from WarrentyPeriod ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    warrenty.Add(new Warrenty()
                    {
                       warrentyID  = reader["warrentyID"] != DBNull.Value ? (int)reader["warrentyID"] : 0,

                        PeriodName = reader["PeriodName"] != DBNull.Value ? (string)reader["PeriodName"] : string.Empty,


                    });
                }

                return warrenty;

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

        public List<GST> getProductGST()
        {
            try
            {
                List<GST> GST = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select GstId, GstName,TotalGstRate from GstRates ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GST.Add(new GST()
                    {
                        GSTID = reader["GstId"] != DBNull.Value ? (int)reader["GstId"] : 0,

                        GSTRate = reader["TotalGstRate"] != DBNull.Value ? Convert.ToString((decimal)reader["TotalGstRate"]) : string.Empty,

                        GSTName = reader["GstName"] != DBNull.Value ? (string)reader["GstName"] : string.Empty,
                    });
                }

                return GST;

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


        public List<Category> getProductCategoriesName()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
              
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                       

                    });
                }

                return cat;

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

        public List<Category> getMaterialCategoriesName()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories Where Type = 'material'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,

                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


                    });
                }

                return cat;

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




        public List<Category> getSubCategoriesName(int categoryId)
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select SubCategoryName, SubCategoryID from SubCategories Where CategoryID = {categoryId}";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,

                        categoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,


                    });
                }

                return cat;

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


        public bool AddItem(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Items([ItemName],[CategoryId],[SubCategoryId],[Specification],[UnitOfMeasure],[GstRate],[ItemType],[ProductCode],[minimumQty],[brand]) OUTPUT Inserted.ItemId values('{item.itemName}','{item.categoryId}','{item.subCategoryId}','{item.specification}','{item.UOM}','{item.gst}','{item.itemType}','{item.itemCode}','{item.stockAlert}','{item.brand}')";
               
                cmd.Connection = connection;
              
                connection.Open();
                Guid ItemId = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"Insert into LotBatch ([ItemId],[InvoiceDate],[Quantity],[ExpiryDate],[unitPrice],[Type],[LotSeries],[Perticular]) values(@ItemId,'{item.InvoiceDate}','{item.quantity}','{item.ExpiryDate}','{item.unitPrice}','{item.type}','{item.lotSeries}','{item.Perticulars}')";

                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@ItemId", ItemId);
                //Guid lotId = (Guid)cmd1.ExecuteScalar();

                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();



                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"INSERT INTO Inventory([ItemId],[CurrentQuantity],[InventoryType],[UpdateDescription]) VALUES(@ItemId,'{item.quantity}','{item.itemType}','{item.Perticulars}')";

                cmd2.Connection = connection;

                cmd2.Parameters.AddWithValue("@ItemId", ItemId);
                //cmd2.Parameters.AddWithValue("@lotId", lotId);

                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = System.Data.CommandType.Text;

                cmd3.CommandText = $"INSERT INTO StockTransactions ([ItemId],[Quantity],[Createdby],[TransactionType],[TransactionDate],[SourceDC],[DestinationDC],[Perticular] ) VALUES(@ItemId,'{item.quantity}','{item.CreatedBY}','{item.TransactionType}','{item.StockDate}','{item.SourceDC}','{item.DestinationDC}','{item.Perticulars}')";

                cmd3.Connection = connection;

                cmd3.Parameters.AddWithValue("@ItemId", ItemId);
                //cmd2.Parameters.AddWithValue("@lotId", lotId);

                connection.Open();
                cmd3.ExecuteScalar();
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

        public List<Items> ViewMaterial()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate,  dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId WHERE it.ItemType = 2";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                      
                        CurrentQuantity = reader["CurrentQuantity"] != DBNull.Value ? (int)reader["CurrentQuantity"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                      
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                       



                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,

                        StockDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                        ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]) : default,



                    });
                }

                return cat;

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

        public bool StockTransfer(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
 

                cmd.CommandText = $"Insert into StockTransactions(Quantity,CreatedBy, ItemId, TransactionType, TransactionDate,SourceDC, DestinationDC, Perticular)values('{item.quantity}','{item.CreatedBY}','{item.itemId}','{item.TransactionType}','{item.StockDate}','{item.SourceDC}','{item.DestinationDC}','{item.Perticulars}')";

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

        public bool InventoryUpdate(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    // Step 1: Update Inventory table
                    string updateInventoryQuery = @"
                UPDATE Inventory
                SET LastUpdated = GETDATE(),
                    CurrentQuantity = CurrentQuantity - @ReleasedQuantity
                WHERE ItemId = @ItemId";

                    using (SqlCommand inventoryCmd = new SqlCommand(updateInventoryQuery, connection))
                    {
                        inventoryCmd.Parameters.AddWithValue("@ReleasedQuantity", item.quantity);
                        inventoryCmd.Parameters.AddWithValue("@ItemId", item.itemId);
                        inventoryCmd.ExecuteNonQuery();
                    }

                    // Step 2: Update LotBatch for each assigned lot
                    foreach (var lot in item.AssignedLots)
                    {
                        string updateLotQuery = @"
                    UPDATE LotBatch
                    SET Quantity = Quantity - @AssignedQuantity
                    WHERE ID = @LotId AND Quantity >= @AssignedQuantity";

                        using (SqlCommand lotCmd = new SqlCommand(updateLotQuery, connection))
                        {
                            lotCmd.Parameters.AddWithValue("@LotId", lot.lotbatchid);
                            lotCmd.Parameters.AddWithValue("@AssignedQuantity", lot.AssignedQuantity);
                            int affected = lotCmd.ExecuteNonQuery();

                            if (affected == 0)
                            {
                                throw new Exception($"Lot {lot.lotbatchid} does not have enough quantity.");
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error here
                throw;
            }
        }


        public List<LotBatch> GetLotBatch(Guid itemid) {

            try
            {
                List<LotBatch> lots = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ID, lotseries, quantity, Unitprice, expirydate,invoicedate FROM LotBatch WHERE ItemId = @ItemId AND Quantity > 0 ORDER BY StockEntryDate;";

                cmd.Parameters.AddWithValue("@ItemId", itemid);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lots.Add(new LotBatch()
                    {
                        lotbatchid = reader["ID"] != DBNull.Value ? (Guid)(reader["ID"]) : Guid.Empty,
                        quantity = reader["quantity"] != DBNull.Value ? (decimal)(reader["quantity"]) : 0,

                        invoicedate = reader["invoicedate"] != DBNull.Value ? (DateTime)reader["invoicedate"] : DateTime.MinValue,
                        Unitprice = reader["Unitprice"] != DBNull.Value ? (decimal)reader["Unitprice"] : 0m,
                        expirydate = reader["expirydate"] != DBNull.Value ? (DateTime)reader["expirydate"] : DateTime.MinValue,
                        lotseries = reader["lotseries"] != DBNull.Value ? (string)reader["lotseries"] : string.Empty,


                    });
                }

                return lots;

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

        public List<Items> ViewProductByCategory(int SubCategoryID)
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, iv.CurrentQuantity  FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId   WHERE it.subcategoryid = '{SubCategoryID}' ";
                ;
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        
                       
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        quantity = reader["CurrentQuantity"] != DBNull.Value ? (int)reader["CurrentQuantity"] : 0,
                  
                    });
                }

                return cat;

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
        public List<Items> ViewProduct()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate, l.CostPrice, l.SellingPrice, l.MRP FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  WHERE it.ItemType = 1 ";
;
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                    
                        CurrentQuantity = reader["CurrentQuantity"] != DBNull.Value ? (int)reader["CurrentQuantity"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                      
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,
                      
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

                       
                     
                      
                       
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,

                        StockDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                         ExpiryDate = reader["ExpiryDate"] != DBNull.Value? ((DateTime)reader["ExpiryDate"]): default,
                       

                    });
                }

                return cat;

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



        //public bool AddStock(Stock stock)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO StockIn([ItemId],[BatchSeries],[InvoiceNo],[StockDate],[ManufacturingDate],[ExpiryDate],[WarehouseId],[UnitPrice],[TotalPrice],[QuantityPurchased])  VALUES('{stock.itemId}','{stock.batchSeries}','{stock.invoice}',{stock.stockDate},{stock.manufacture},{stock.expiry},'{stock.warehouseId}','{stock.unitPrice}','{stock.totalPrice}','{stock.quantity}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;





        //                connection.Open();
        //                 cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}


        public bool StockReturnUpdate(Asset asset )
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandType = System.Data.CommandType.Text;

               
                //cmd.CommandText = $"INSERT INTO LotBatch([ItemId],[ArrivalDate], [ExpiryDate],[CostPrice],[Quantity],[Type]) OUTPUT Inserted.Id VALUES ('{stock.itemId}', {DateTime.Now.ToShortDateString()},'{stock.expiry}', '{stock.costPrice}', '{stock.quantity}','{stock.itemType}')";

                //cmd.Connection = connection;

                //connection.Open();
           
                //cmd.ExecuteScalar();
                //connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO StockTransactions([ItemId],[TransactionDate], [TransactionType],[Quantity],[Reason]) VALUES ('{asset.itemId}', {DateTime.Now.ToShortDateString()},'stockreturn', '{asset.Quantity}','{asset.description}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"UPDATE Inventory SET InStock = InStock + @NewQuantity WHERE ItemId = @ItemId ";
                cmd2.Parameters.AddWithValue("@NewQuantity", asset.Quantity);
                cmd2.Parameters.AddWithValue("@ItemId", asset.itemId);
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();
                return true;

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

        public List<Items> GetInventoryReport()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ProductCode, it.ItemName,it.minimumQty, it.UnitOfMeasure,  iv.CurrentQuantity,lb.unitprice,lb.stockentrydate FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId OUTER APPLY ( SELECT TOP 1 * FROM LotBatch lb    WHERE lb.ItemId = it.ItemId ORDER BY lb.stockentryDate DESC) lb ";
                ;
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                       itemCode = reader["ProductCode"] != DBNull.Value ? (string)reader["ProductCode"] : string.Empty,
                        CurrentQuantity = reader["CurrentQuantity"] != DBNull.Value ? (int)reader["CurrentQuantity"] : 0,
                        stockAlert = reader["minimumQty"] != DBNull.Value ? (int)reader["minimumQty"] : 0,
                     
 

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,




                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,

                    
                        unitPrice = reader["unitprice"] != DBNull.Value ? (decimal)reader["unitprice"] : 0,


                        StockDate = reader["stockentrydate"] != DBNull.Value ? ((DateTime)reader["stockentrydate"]).Date : default(DateTime),



                    });
                }

                return cat;

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
        public bool AddStock(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

         
                
                cmd.CommandText = $"INSERT INTO LotBatch ([ItemId],[InvoiceDate],[Quantity],[UnitPrice],[Type],[ExpiryDate],[LotSeries],[Perticular],[stockentrydate]) OUTPUT Inserted.Id VALUES ('{item.itemId}', '{item.InvoiceDate}','{item.quantity}', '{item.unitPrice}','{item.type}', '{item.ExpiryDate}','{item.lotSeries}','{item.Perticulars}','{item.StockDate}')";

                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO StockTransactions([Quantity],[CreatedBy],[ItemID], [TransactionType],[TransactionDate],[SourceDC],[DestinationDC],[Perticular]) VALUES ('{item.quantity}', '{item.CreatedBY}','{item.itemId}','{item.TransactionType}','{item.StockDate}','{item.SourceDC}', '{item.DestinationDC}','{item.Perticulars}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"Update  Inventory Set lastUpdated = getdate(), currentQuantity = (Select Sum(Quantity) As Quantity From LotBatch Where ItemId = '{item.itemId}')";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();
                return true;

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

        //public bool AddStock(Stock stock)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = @"INSERT INTO LotBatch([ItemId],[ArrivalDate], [ExpiryDate],[CostPrice],[Quantity]) VALUES
        //        (@ItemId, @ArrivalDate, @ExpiryDate, @CostPrice, @Quantity)";

        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@ItemId", stock.itemId);


        //                cmd.Parameters.AddWithValue("@ArrivalDate", DateTime.Now.ToShortDateString());
                       
        //                cmd.Parameters.AddWithValue("@ExpiryDate", stock.expiry);
                      
        //                cmd.Parameters.AddWithValue("@CostPrice", stock.costPrice);
                        
        //                cmd.Parameters.AddWithValue("@Quantity", stock.quantity);

        //                connection.Open();
        //                cmd.ExecuteNonQuery();
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw;
        //    }
        //}


        public List<Items> ViewProductStock()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT s.ItemId,s.Id, s.unitPrice, s.InvoiceDate, s.ExpiryDate, st.Quantity,  i.ItemName       
    FROM LotBatch s    LEFT JOIN Items i ON s.ItemId = i.ItemId    LEFT JOIN StockTransactions st ON s.ItemId = st.ItemId  ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                     
                       
                        //quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        quantity = reader["Quantity"] != DBNull.Value ? (int)(reader["Quantity"]) : 0,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

                        ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        InvoiceDate = reader["InvoiceDate"] != DBNull.Value ? ((DateTime)reader["InvoiceDate"]).Date : default(DateTime),
                       
                        unitPrice = reader["unitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["unitPrice"]) : 0m,
                        


                    });
                }

                return cat;

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










        //public Stock GetStock(Guid stockId)
        //{
        //    try
        //    {
        //        Stock c = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $" SELECT CostPrice,ExpiryDate,Quantity,Id from LotBatch where Id = '{stockId}'";

        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            c.stockId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        
        //                c.quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m;

        //            c.expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime);
                   
        //            c.costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m;
                    





        //        }


        //        return c;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }


        //}

        //public bool UpdateStock(Stock stock)
        //{
        //    try
        //    {
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Update LotBatch set  ItemId ='{stock.itemId}',CostPrice='{stock.costPrice}',ExpiryDate='{stock.expiry}',Quantity='{stock.quantity}' where Id = '{stock.stockId}'";

        //        cmd.Connection = connection;
        //        connection.Open();
        //        cmd.ExecuteScalar();
        //        connection.Close();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        //public bool DeleteStock(Guid stockId)
        //{
        //    try
        //    {
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Delete From  LotBatch  where Id = '{stockId}'";

        //        cmd.Connection = connection;
        //        connection.Open();
        //        cmd.ExecuteScalar();
        //        connection.Close();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}


        public bool AddStockTransfer(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                
                cmd.CommandText = $"INSERT INTO LotBatch([ItemId], [ArrivalDate], [Quantity], [SellingPrice])  VALUES('{order.itemId}', '{DateTime.Now.ToShortDateString()}', '{order.quantity}', '{order.sellingPrice}')";


                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO StockTransactions([ItemId],[TransactionDate], [Transaction],[Quantity]) VALUES ('{order.itemId}', {DateTime.Now.ToShortDateString()},'{order.type}', '{order.quantity}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();


                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"Update  Inventory Set InStock = (Select Sum(Quantity) As Quantity From LotBatch Where ItemId = '{order.itemId}') where ItemId = '{order.itemId}'";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
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



        //public bool AddStockTransfer(Stock order)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO LotBatch([ItemId],[ArrivalDate],[Quantity],[SellingPrice])  VALUES('{order.itemId}','{DateTime.Now.ToShortDateString()}','{order.quantity}','{order.sellingPrice}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;





        //                connection.Open();
        //                cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}


        public List<Order> ViewStockTransfer()
        {
            try
            {
                List<Order> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT 
        lb.ItemId,
        lb.Id,
        
        lb.ArrivalDate,
        lb.SellingPrice,
        lb.ExpiryDate,
        
        lb.Quantity,
    
       
        i.ItemName
       
    FROM LotBatch lb
    LEFT JOIN Items i ON lb.ItemId = i.ItemId 
    LEFT JOIN StockTransactions st ON lb.ItemId = st.ItemId Where st.TransactionType = 'stockOut'; 
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Order()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        OrderId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                       
                       
                     
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        
                        
                       
                        orderDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        //...unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,


                    });
                }

                return cat;

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
        public Order GetStockTransfer(Guid orderId)
        {
            try
            {
                Order c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT OrderId,CustomerName,TotalPrice,AvailableStock,OrderDate,UnitPrice,QuantityOrdered,BatchSeries from StockOut where orderId = '{orderId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.OrderId = reader["OrderId"] != DBNull.Value ? (Guid)reader["OrderId"] : Guid.Empty;

                    c.quantity = reader["QuantityOrdered"] != DBNull.Value ? (int)reader["QuantityOrdered"] : 0;
                    c.available = reader["AvailableStock"] != DBNull.Value ? (int)reader["AvailableStock"] : 0;
                   
                    //c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    //c.warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty;
                    c.customerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;
                    c.batchSeries = reader["BatchSeries"] != DBNull.Value ? (string)reader["BatchSeries"] : string.Empty;
                   
                    c.orderDate = reader["OrderDate"] != DBNull.Value ? ((DateTime)reader["OrderDate"]).Date : default(DateTime);
                    //c.unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m;
                    c.totalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m;





                }


                return c;

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


        public Items GetMaterialData(Guid itemId)
        {
            try
            {
                Items c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate,  dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId where it.ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                        c.lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        c.inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty;
                      
                        c.CurrentQuantity = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;
                        c.stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0;
                        
                        c.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;

                        c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                      
                        c.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
                        c.UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty;

                     
                        c.StockDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime);
              
                        c.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]) : default;




                }


                return c;

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


        public List<Items> GetProductDataBasedonCategory(int subCategoryID)
        {
            try
            {
               List< Items> c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from items where subcategoryid = '{subCategoryID}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.Add( new Items
                    { 
                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,                    
                     
                    itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                
                     });
                }


                return c;

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

        public Items GetProductData(Guid itemId)
        {
            try
            {
                Items c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate, l.CostPrice, l.SellingPrice, l.MRP, dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId  where it.ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                        c.lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        c.inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty;
                      
                        c.CurrentQuantity = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;
                        c.stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0;
                       
                        c.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;
                      
                        c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                     
                        c.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
                        c.UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty;
                       
                      
                        c.StockDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime);
                      
                        c.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]) : default;
                       



                }


                return c;

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

        public bool UpdateStockTransfer(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update StockOut set  ItemId ='{order.itemId}',TotalPrice='{order.totalPrice}',OrderDate='{order.orderDate}',QuantityOrdered='{order.quantity}',AvailableStock='{order.available}',CustomerName='{order.customerName}',BatchSeries='{order.batchSeries}',WarehouseId='{order.warehouseId}' where OrderId = '{order.OrderId}'";

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


        public bool DeleteStockTransfer(Guid orderId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  StockOut  where OrderId = '{orderId}'";

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

        public bool UpdateMaterialItem(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Items set  ItemName ='{item.itemName}',Specification='{item.specification}',UnitOfMeasure='{item.UOM}',GstRate='{item.gst}' where ItemId = '{item.itemId}'";

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

        public bool UpdateInventory(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Inventory set  CurrentQuantity='{item.CurrentQuantity}',StockAlert='{item.stockAlert}' where ItemId = '{item.itemId}'";

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

        
        public bool UpdateProductPrice(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update LotBatch set CostPrice='{item.unitPrice}' where ItemId = '{item.itemId}'";

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

        //public bool AddStockAdjustment(Order order)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO StockTransactions([ItemId],[SourceDC],[DestinationDC],[Quantity],[Reason],[TransactionType])  VALUES('{order.itemId}','{order.sourceDc}','{order.destinationDc}','{order.quantity}','{order.reason}','{order.type}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;

        //                connection.Open();
        //                cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}


        public bool AddStockAdjustment(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                //cmd.CommandText = $"Insert into StockTransactions([ItemId],[ItemType]) OUTPUT Inserted.ItemId values('{asset.itemName}','{asset.itemType}')";
                cmd.CommandText = $"Update  Inventory Set InStock = InStock - '{order.quantity}' where ItemId = '{order.itemId}'";

                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                //SqlCommand cmd1 = new SqlCommand();
                //cmd1.CommandType = System.Data.CommandType.Text;
                //cmd1.CommandText = $"INSERT INTO Inventory([ItemId],[InStock],[CenterId]) VALUES ('{order.itemId}', '{order.quantity}','{order.destinationDc}')";

                //cmd1.Connection = connection;
                ////cmd1.Parameters.AddWithValue("@Id", Id);


                //connection.Open();
                //cmd1.ExecuteScalar();
                //connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"INSERT INTO StockTransactions([ItemId],[SourceDC],[DestinationDC],[Quantity],[Reason],[TransactionType])  VALUES('{order.itemId}','{order.sourceDc}','{order.destinationDc}','{order.quantity}','{order.reason}','{order.type}')";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();
                return true;

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


        public int GetCurrentStock(Guid itemId)
        {
            try
            {
                int currentStock = 0;
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select InStock from Inventory where ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                   currentStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;




                }


                return currentStock;

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

        public bool OpeningStockEntryForm(Items item)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO Inventory([ItemId],[CurrentQuantity])  VALUES('{item.itemId}','{item.CurrentQuantity}')";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }



        public List<Items> ViewInventoryData()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"SELECT  i.ItemId,i.ItemName, i.SKU,i.Specification,i.HSN, iv.InStock,iv.InventoryId FROM Items  LEFT JOIN Inventory iv ON i.ItemId = iv.ItemId";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,


                        CurrentQuantity = reader["CurrentQuantity"] != DBNull.Value ? (int)reader["CurrentQuantity"] : 0,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                     



                    });
                }

                return cat;

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

