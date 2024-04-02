using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using ADO.Models;
using System.Data;

namespace ADO.DataAccessLayer
{
    public class Product_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["ProductConnectionString"].ToString();
        
        //Get All Products

        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            using(SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetAllProducts";
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                con.Open();
                sqlDA.Fill(dataTable);
                con.Close();

                foreach(DataRow dr in dataTable.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Remarks = (dr["Remarks"].ToString())
                    });
                }
            }
            return productList;
        }

        public bool InsertProduct(Product product)
        {
            int id = 0;
            using(SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProducts",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductName",product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Remarks", product.Remarks);

                con.Open();
                id = cmd.ExecuteNonQuery();
                con.Close();
            }
            if(id> 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<Product> GetProductsById(int ProductID)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetProductByID";
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                con.Open();
                sqlDA.Fill(dataTable);
                con.Close();

                foreach (DataRow dr in dataTable.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Remarks = (dr["Remarks"].ToString())
                    });
                }
            }
            return productList;
        }

        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Remarks", product.Remarks);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string DeleteProduct(int productid)
        {
            string result = "";

            using(SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProducts", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@productid", productid);
                cmd.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@OutputMessage"].Value.ToString();
                con.Close();
            }
            return result;
        }

    }
}