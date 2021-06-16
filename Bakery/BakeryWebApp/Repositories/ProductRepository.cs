using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private string _connectionString;

        public ProductRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Product item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Products values(@name)";
                SqlCommand command = new(sql, connection);
                SqlParameter name = new("@name", item.ProductName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Product item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete Products where ProductId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

            }
        }
        public Product Get(int id)
        {
            Product product = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Products where ProductId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        product.ProductId = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                    }
                }
            }
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> products = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Products";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product = new()
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1)

                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public void Update(Product item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update Products set ProductName = @name where ProductId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductId);
                command.Parameters.Add(idParam);
                SqlParameter name = new("@name", item.ProductName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }
    }
}
