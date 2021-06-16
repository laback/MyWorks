using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class MaterialRepository: IRepository<Material>
    {
        private string _connectionString;

        public MaterialRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Material item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Materials values(@name)";
                SqlCommand command = new(sql, connection);
                SqlParameter name = new("@name", item.MaterialName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Material item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete Materials where MaterialId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.MaterialId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public Material Get(int id)
        {
            Material material = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Materials where MaterialId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        material.MaterialId = reader.GetInt32(0);
                        material.MaterialName = reader.GetString(1);
                    }
                }
            }
            return material;
        }

        public IEnumerable<Material> GetAll()
        {
            List<Material> materials = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Materials";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Material material = new()
                        {
                            MaterialId = reader.GetInt32(0),
                            MaterialName = reader.GetString(1)

                        };
                        materials.Add(material);
                    }
                }
            }

            return materials;
        }

        public void Update(Material item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update Materials set MaterialName = @name where MaterialId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.MaterialId);
                command.Parameters.Add(idParam);
                SqlParameter name = new("@name", item.MaterialName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }
    }
}
