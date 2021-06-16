using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.Repositories
{
    public class DistrictRepository : IRepository<District>
    {
        private string _connectionString;

        public DistrictRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(District item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Districts values(@DistrictName)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter DistrictName = new SqlParameter("@DistrictName", item.DistrictName);
                command.Parameters.Add(DistrictName);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(District item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Delete Districts where DistrictId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.DistrictId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public District Get(int id)
        {
            District District = new District();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select * from Districts where DistrictId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        District.DistrictId = reader.GetInt32(0);
                        District.DistrictName = reader.GetString(1);
                    }
                }
            }
            return District;
        }

        public IEnumerable<District> GetAll()
        {
            List<District> Districts = new List<District>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select * from Districts";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        District District = new District
                        {
                            DistrictId = reader.GetInt32(0),
                            DistrictName = reader.GetString(1)

                        };
                        Districts.Add(District);
                    }
                }
            }
            return Districts;
        }

        public void Update(District item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Update Districts set DistrictName = @name where DistrictId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.DistrictId);
                command.Parameters.Add(idParam);
                SqlParameter name = new SqlParameter("@name", item.DistrictName);
                command.Parameters.Add(name);
                command.ExecuteNonQuery();
            }
        }
    }
}
