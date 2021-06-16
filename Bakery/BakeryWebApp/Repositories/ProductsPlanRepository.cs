using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class ProductsPlanRepository : IRepository<ProductsPlan>
    {
        private string _connectionString;

        public ProductsPlanRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(ProductsPlan item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into ProductsPlans values(@dayPlanId, @productId, @count)";
                SqlCommand command = new(sql, connection);
                SqlParameter dayPlanId = new("@dayPlanId", item.DayPlanId);
                command.Parameters.Add(dayPlanId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter count = new("@count", item.Count);
                command.Parameters.Add(count);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(ProductsPlan item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete ProductsPlans where productPlan = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductPlan);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

            }
        }

        public ProductsPlan Get(int id)
        {
            ProductsPlan productsPlan = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productPlan, ProductsPlans.dayPlanId, ProductsPlans.productId, count, productName, date from ProductsPlans " +
                    "inner join Products on Products.productId = ProductsPlans.ProductId " +
                    "inner join DayPlans on DayPlans.dayPlanId = ProductsPlans.dayPlanId " +
                    "where ProductsPlans.productPlan = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productsPlan.ProductPlan = reader.GetInt32(0);
                        productsPlan.DayPlanId = reader.GetInt32(1);
                        productsPlan.ProductId = reader.GetInt32(2);
                        productsPlan.Count = reader.GetInt32(3);
                        productsPlan.Product = new Product();
                        productsPlan.Product.ProductId = (int)productsPlan.ProductId;
                        productsPlan.Product.ProductName = reader.GetString(4);
                        productsPlan.DayPlan = new DayPlan();
                        productsPlan.DayPlan.DayPlanId = (int)productsPlan.DayPlanId;
                        productsPlan.DayPlan.Date = reader.GetDateTime(5);
                    }
                }
            }
            return productsPlan;
        }

        public IEnumerable<ProductsPlan> GetAll()
        {
            List<ProductsPlan> productsPlans = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productPlan, ProductsPlans.dayPlanId, ProductsPlans.productId, count, productName, date from ProductsPlans " +
                   "inner join Products on Products.productId = ProductsPlans.ProductId " +
                   "inner join DayPlans on DayPlans.dayPlanId = ProductsPlans.dayPlanId ";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsPlan productsPlan = new ProductsPlan();
                        productsPlan.ProductPlan = reader.GetInt32(0);
                        productsPlan.DayPlanId = reader.GetInt32(1);
                        productsPlan.ProductId = reader.GetInt32(2);
                        productsPlan.Count = reader.GetInt32(3);
                        productsPlan.Product = new Product();
                        productsPlan.Product.ProductId = (int)productsPlan.ProductId;
                        productsPlan.Product.ProductName = reader.GetString(4);
                        productsPlan.DayPlan = new DayPlan();
                        productsPlan.DayPlan.DayPlanId = (int)productsPlan.DayPlanId;
                        productsPlan.DayPlan.Date = reader.GetDateTime(5);
                        productsPlans.Add(productsPlan);
                    }
                }
            }
            return productsPlans;
        }

        public void Update(ProductsPlan item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update ProductsPlans set dayPlanId = @dayPlanId, productId = @productId, count = @count where productPlan = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductId);
                command.Parameters.Add(idParam);
                SqlParameter dayPlanId = new("@dayPlanId", item.DayPlanId);
                command.Parameters.Add(dayPlanId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter count = new("@count", item.Count);
                command.Parameters.Add(count);
                command.ExecuteNonQuery();
            }
        }
    }
}
