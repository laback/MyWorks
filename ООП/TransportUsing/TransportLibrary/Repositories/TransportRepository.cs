using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportUsing.Entities;
using TransportUsing.Interfaces;

namespace TransportUsing.Repositories
{
    public class TransportRepository : IRepository<Transport>
    {
        private readonly string _connectionString;

        public TransportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Transport item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "insert into Transports values(@typeid, @markid)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeId = new SqlParameter("@typeid", item.TransportTypeId);
                command.Parameters.Add(typeId);
                SqlParameter markId = new SqlParameter("@markid", item.MarkId);
                command.Parameters.Add(markId);
                command.ExecuteNonQuery();
            }    
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "delete from Transports where transportId = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter transportId = new SqlParameter("@id", id);
                command.Parameters.Add(transportId);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Transport> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select transportId, Transports.transportTypeId, TransportTypes.transportTypeName, Transports.markId, Marks.markName " +
                    "from Transports " +
                    "inner join TransportTypes on TransportTypes.transportTypeId = Transports.transportTypeId " +
                    "inner join  Marks on Marks.markId = Transports.markId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<Transport> transports = new List<Transport>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Transport transport = new Transport();
                        transport.TransportId = reader.GetInt32(0);
                        transport.TransportTypeId = reader.GetInt32(1);
                        transport.TransportType = new TransportType();
                        transport.TransportType.TransportTypeName = reader.GetString(2);
                        transport.MarkId = reader.GetInt32(3);
                        transport.Mark = new Mark();
                        transport.Mark.MarkName = reader.GetString(4);
                        transports.Add(transport);
                    }
                }
                return transports;
            }
        }

        public Transport Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select transportId, Transports.transportTypeId, TransportTypes.transportTypeName, Transports.markId, Marks.markName " +
                    "from Transports " +
                    "inner join TransportTypes on TransportTypes.transportTypeId = Transports.transportTypeId " +
                    "inner join  Marks on Marks.markId = Transports.markId " +
                    "where transportId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter transportId = new SqlParameter("@id", id);
                command.Parameters.Add(transportId);
                SqlDataReader reader = command.ExecuteReader();
                Transport transport = new Transport();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        transport.TransportId = reader.GetInt32(0);
                        transport.TransportTypeId = reader.GetInt32(1);
                        transport.TransportType = new TransportType();
                        transport.TransportType.TransportTypeName = reader.GetString(2);
                        transport.MarkId = reader.GetInt32(3);
                        transport.Mark = new Mark();
                        transport.Mark.MarkName = reader.GetString(4);
                    }
                }
                return transport;
            }
        }

        public void Update(Transport item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "update Transports set transportTypeId = @typeid, markId = @markid where transportId = @id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeId = new SqlParameter("@typeid", item.TransportTypeId);
                command.Parameters.Add(typeId);
                SqlParameter markId = new SqlParameter("@markid", item.MarkId);
                command.Parameters.Add(markId);
                SqlParameter id = new SqlParameter("@id", item.TransportId);
                command.Parameters.Add(id);
                int number = command.ExecuteNonQuery();
            }
        }
    }
}
