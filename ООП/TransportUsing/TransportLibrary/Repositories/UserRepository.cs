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
    public class UserRepository : IRepository<User>
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(User item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "insert into Users values (@username, @password, @FIO, @typeid)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter userName = new SqlParameter("@username", item.UserName);
                command.Parameters.Add(userName);
                SqlParameter password = new SqlParameter("password", item.Password);
                command.Parameters.Add(password);
                SqlParameter FIO = new SqlParameter("@FIO", item.FIO);
                command.Parameters.Add(FIO);
                SqlParameter userType = new SqlParameter("@typeid", item.TypeId);
                command.Parameters.Add(userType);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "delete from Users where UserId = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idname = new SqlParameter("@id", id);
                command.Parameters.Add(idname);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<User> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select userId, username, password, FIO, Users.typeId, UserTypes.typeName " +
                    "from Users " +
                    "inner join UserTypes on Users.typeId = UserTypes.typeId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<User> users = new List<User>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.UserId = reader.GetInt32(0);
                        user.UserName = reader.GetString(1);
                        user.Password = reader.GetString(2);
                        user.FIO = reader.GetString(3);
                        user.TypeId = reader.GetInt32(4);
                        user.UserType = new UserType();
                        user.UserType.TypeName = reader.GetString(5);
                        users.Add(user);
                    }
                };
                return users;
            }
        }

        public User Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select userId, username, password, FIO, Users.typeId, UserTypes.typeName " +
                    "from Users " +
                    "inner join UserTypes on Users.typeId = UserTypes.typeId " +
                    "where userId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter userId = new SqlParameter("@id", id);
                command.Parameters.Add(userId);
                SqlDataReader reader = command.ExecuteReader();
                User user = new User();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.UserId = reader.GetInt32(0);
                        user.UserName = reader.GetString(1);
                        user.Password = reader.GetString(2);
                        user.FIO = reader.GetString(3);
                        user.TypeId = reader.GetInt32(4);
                        user.UserType = new UserType();
                        user.UserType.TypeName = reader.GetString(5);
                    }
                };
                return user;
            }
        }

        public void Update(User item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "update Users set userName = @username, password = @password, FIO = @FIO, typeId = @typeId where userId = @id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter username = new SqlParameter("@username", item.UserName);
                command.Parameters.Add(username);
                SqlParameter password = new SqlParameter("@password", item.Password);
                command.Parameters.Add(password);
                SqlParameter FIO = new SqlParameter("@FIO", item.FIO);
                command.Parameters.Add(FIO);
                SqlParameter typeId = new SqlParameter("@typeId", item.TypeId);
                command.Parameters.Add(typeId);
                SqlParameter id = new SqlParameter("@id", item.UserId);
                command.Parameters.Add(id);
                command.ExecuteNonQuery();
            }
        }
    }
}
