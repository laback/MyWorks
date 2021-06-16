using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class RawRepository : IRepository<Raw>
    {
        private string _connectionString;
        
        public RawRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Raw item)
        {
            using(SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Raws values(@name)";
                SqlCommand command = new(sql, connection);
                SqlParameter name = new("@name", item.RawName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Raw item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete Raws where RawId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.RawId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public Raw Get(int id)
        {
            Raw raw = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Raws where RawId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        raw.RawId = reader.GetInt32(0);
                        raw.RawName = reader.GetString(1);
                    }
                }
            }
            return raw;
        }



        public IEnumerable<Raw> GetAll()
        {
            List<Raw> raws = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select * from Raws";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Raw raw = new()
                        {
                            RawId = reader.GetInt32(0),
                            RawName = reader.GetString(1)
                            
                        };
                        raws.Add(raw);
                    }
                }
            }

            return raws;
        }

        public void Update(Raw item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update Raws set RawName = @name where RawId = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.RawId);
                command.Parameters.Add(idParam);
                SqlParameter name = new("@name", item.RawName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }
    }
}
