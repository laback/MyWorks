using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportUsing.Interfaces;
using TransportUsing.Entities;
using System.Data.SqlClient;

namespace TransportUsing.Repositories
{
    public class TransportTypeRepository : IRepository<TransportType>
    {
        private readonly string _connectionString;

        public TransportTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(TransportType item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "insert into TransportTypes values (@type)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter transportType = new SqlParameter("@type", item.TransportTypeName);
                command.Parameters.Add(transportType);
                int number = command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "delete from TransportTypes where TransportTypeId = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeId = new SqlParameter("@id", id);
                command.Parameters.Add(typeId);
                int number = command.ExecuteNonQuery();
            }
        }

        public IEnumerable<TransportType> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from TransportTypes";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<TransportType> transportTypes = new List<TransportType>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        transportTypes.Add(new TransportType
                        {
                            TransportTypeId = reader.GetInt32(0),
                            TransportTypeName = reader.GetString(1)
                        });
                    }
                }
                return transportTypes;
            }
        }

        public TransportType Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from TransportTypes where transportTypeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeId = new SqlParameter("@id", id);
                command.Parameters.Add(typeId);
                SqlDataReader reader = command.ExecuteReader();
                TransportType transportType = new TransportType();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        transportType.TransportTypeId = reader.GetInt32(0);
                        transportType.TransportTypeName = reader.GetString(1);
                    }
                }
                return transportType;
            }
        }

        public void Update(TransportType item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "update TransportTypes set transportTypeName = @type where transportTypeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeName = new SqlParameter("@type", item.TransportTypeName);
                command.Parameters.Add(typeName);
                SqlParameter id = new SqlParameter("@id", item.TransportTypeId);
                command.Parameters.Add(id);
            }
        }
    }
}
