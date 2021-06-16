using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class NormRepository : IRepository<Norm>
    {
        private string _connectionString;

        public NormRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Norm item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Norms values(@rawId, @productId, @quantity)";
                SqlCommand command = new(sql, connection);
                SqlParameter rawId = new("@rawId", item.RawId);
                command.Parameters.Add(rawId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter quanitity = new("@quantity", item.Quantity);
                command.Parameters.Add(quanitity);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Norm item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete Norms where NormId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.NormId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

            }
        }

        public Norm Get(int id)
        {
            Norm norm = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select normId, Norms.rawId, Norms.productId, quantity, productName, rawName from Norms " +
                    "inner join Products on Products.productId = Norms.ProductId " +
                    "inner join Raws on Raws.rawId = Norms.rawId " +
                    "where Norms.normId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        norm.NormId = reader.GetInt32(0);
                        norm.RawId = reader.GetInt32(1);
                        norm.ProductId = reader.GetInt32(2);
                        norm.Quantity = reader.GetDouble(3);
                        norm.Product = new Product();
                        norm.Product.ProductId = (int)norm.ProductId;
                        norm.Product.ProductName = reader.GetString(4);
                        norm.Raw = new Raw();
                        norm.Raw.RawId = (int)norm.RawId;
                        norm.Raw.RawName = reader.GetString(5);
                    }
                }
            }
            return norm;
        }

        public IEnumerable<Norm> GetAll()
        {
            List<Norm> norms = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select normId, Norms.rawId, Norms.productId, quantity, productName, rawName from Norms " +
                    "inner join Products on Products.productId = Norms.productId " +
                    "inner join Raws on Raws.rawId = Norms.rawId ";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Norm norm = new Norm();
                        norm.NormId = reader.GetInt32(0);
                        norm.RawId = reader.GetInt32(1);
                        norm.ProductId = reader.GetInt32(2);
                        norm.Quantity = reader.GetDouble(3);
                        norm.Product = new Product();
                        norm.Product.ProductId = (int)norm.ProductId;
                        norm.Product.ProductName = reader.GetString(4);
                        norm.Raw = new Raw();
                        norm.Raw.RawId = (int)norm.RawId;
                        norm.Raw.RawName = reader.GetString(5);
                        norms.Add(norm);
                    }
                }
            }
            return norms;
        }

        public void Update(Norm item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update Norms set rawId = @rawId, productId = @productId, quantity = @quantity where NormId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.NormId);
                command.Parameters.Add(idParam);
                SqlParameter rawId = new("@rawId", item.RawId);
                command.Parameters.Add(rawId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter quanitity = new("@quantity", item.Quantity);
                command.Parameters.Add(quanitity);
                command.ExecuteNonQuery();
            }
        }
    }
}
