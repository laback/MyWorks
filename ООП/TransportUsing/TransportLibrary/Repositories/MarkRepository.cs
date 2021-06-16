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
    public class MarkRepository : IRepository<Mark>
    {
        private readonly string _connectionString;

        public MarkRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Mark item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "insert into Marks values (@mark)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter mark = new SqlParameter("@mark", item.MarkName);
                command.Parameters.Add(mark);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "delete from Marks where markId = @id;";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter markid = new SqlParameter("@id", id);
                command.Parameters.Add(markid);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Mark> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from Marks";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<Mark> marks = new List<Mark>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        marks.Add(new Mark
                        {
                            MarkId = reader.GetInt32(0),
                            MarkName = reader.GetString(1)
                        });


                    }
                };
                return marks;
            }
        }

        public Mark Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select * from Marks where markId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter markId = new SqlParameter("@id", id);
                command.Parameters.Add(markId);
                SqlDataReader reader = command.ExecuteReader();
                Mark mark = new Mark();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mark.MarkId = reader.GetInt32(0);
                        mark.MarkName = reader.GetString(1);
                    }
                }
                return mark;
            }
        }

        public void Update(Mark item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "update Marks set markName = @mark where markId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter markname = new SqlParameter("@mark", item.MarkName);
                command.Parameters.Add(markname);
                SqlParameter id = new SqlParameter("@id", item.MarkId);
                command.Parameters.Add(id);
                command.ExecuteNonQuery();
            }
        }
    }
}
