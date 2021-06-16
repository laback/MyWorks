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
    public class UserTypeRepository : IRepository<UserType>
    {
        private readonly string _connectionString;

        public UserTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(UserType item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserType> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from UserTypes";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<UserType> usertype = new List<UserType>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usertype.Add(new UserType
                        {
                            TypeId = reader.GetInt32(0),
                            TypeName = reader.GetString(1)
                        });
                    }
                };
                return usertype;

            }
        }

        public UserType Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from UserTypes where TypeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter typeId = new SqlParameter("@id", id);
                command.Parameters.Add(typeId);
                SqlDataReader reader = command.ExecuteReader();
                UserType userType = new UserType();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userType.TypeId = reader.GetInt32(0);
                        userType.TypeName = reader.GetString(1);
                    }
                };
                return userType;
            }
        }

        public void Update(UserType item)
        {
            throw new NotImplementedException();
        }
    }
}
