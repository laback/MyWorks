using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class ProductsProductionRepository : IRepository<ProductsProduction>
    {
        private string _connectionString;
        public ProductsProductionRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(ProductsProduction item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into ProductsProductions values(@productProductionId, @productId, @count)";
                SqlCommand command = new(sql, connection);
                SqlParameter productProductionId = new("@productProductionId", item.DayProductionId);
                command.Parameters.Add(productProductionId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter count = new("@count", item.Count);
                command.Parameters.Add(count);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(ProductsProduction item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete ProductsProductions where productProductionId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductProductionId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

            }
        }

        public ProductsProduction Get(int id)
        {
            ProductsProduction productsProduction = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productProductionId, ProductsProductions.dayProductionId, ProductsProductions.productId, count, productName, date from ProductsProductions " +
                    "inner join Products on Products.productId = ProductsProductions.ProductId " +
                    "inner join DayProductions on DayProductions.dayProductionId = ProductsProductions.dayProductionId " +
                    "where productProductionId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productsProduction.ProductProductionId = reader.GetInt32(0);
                        productsProduction.DayProductionId = reader.GetInt32(1);
                        productsProduction.ProductId = reader.GetInt32(2);
                        productsProduction.Count = reader.GetInt32(3);
                        productsProduction.Product = new Product();
                        productsProduction.Product.ProductId = (int)productsProduction.ProductId;
                        productsProduction.Product.ProductName = reader.GetString(4);
                        productsProduction.DayProduction = new DayProduction();
                        productsProduction.DayProduction.DayProductionId = (int)productsProduction.DayProductionId;
                        productsProduction.DayProduction.Date = reader.GetDateTime(5);
                    }
                }
            }
            return productsProduction;
        }

        public IEnumerable<ProductsProduction> GetAll()
        {
            List<ProductsProduction> productsProductions = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productProductionId, ProductsProductions.dayProductionId, ProductsProductions.productId, count, productName, date from ProductsProductions " +
                    "inner join Products on Products.productId = ProductsProductions.ProductId " +
                    "inner join DayProductions on DayProductions.dayProductionId = ProductsProductions.dayProductionId";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsProduction productsProduction = new ProductsProduction();
                        productsProduction.ProductProductionId = reader.GetInt32(0);
                        productsProduction.DayProductionId = reader.GetInt32(1);
                        productsProduction.ProductId = reader.GetInt32(2);
                        productsProduction.Count = reader.GetInt32(3);
                        productsProduction.Product = new Product();
                        productsProduction.Product.ProductId = (int)productsProduction.ProductId;
                        productsProduction.Product.ProductName = reader.GetString(4);
                        productsProduction.DayProduction = new DayProduction();
                        productsProduction.DayProduction.DayProductionId = (int)productsProduction.DayProductionId;
                        productsProduction.DayProduction.Date = reader.GetDateTime(5);
                        productsProductions.Add(productsProduction);
                    }
                }
            }
            return productsProductions;
        }

        public void Update(ProductsProduction item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update ProductsProductions set dayProductionId = @dayProductionId, productId = @productId, count = @count where productProductionId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductId);
                command.Parameters.Add(idParam);
                SqlParameter dayProductionId = new("@dayProductionId", item.DayProductionId);
                command.Parameters.Add(dayProductionId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter count = new("@count", item.Count);
                command.Parameters.Add(count);
                command.ExecuteNonQuery();
            }
        }
    }
}
