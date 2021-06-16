using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.Repositories
{
    public class ActivityRepository : IRepository<Activity>
    {
        private string _connectionString;

        public ActivityRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Activity item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Activities values(@activityName, @tax)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter activityName = new SqlParameter("@activityName", item.ActivityName);
                command.Parameters.Add(activityName);
                SqlParameter tax = new SqlParameter("@tax", item.Tax);
                command.Parameters.Add(tax);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Activity item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Delete Activities where activityId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.ActivityId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public Activity Get(int id)
        {
            Activity activity = new Activity();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select * from Activities where activityId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        activity.ActivityId = reader.GetInt32(0);
                        activity.ActivityName = reader.GetString(1);
                        activity.Tax = reader.GetDecimal(2);
                    }
                }
            }
            return activity;
        }

        public IEnumerable<Activity> GetAll()
        {
            List<Activity> activities = new List<Activity>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select * from Activities";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Activity activity = new Activity
                        {
                            ActivityId = reader.GetInt32(0),
                            ActivityName = reader.GetString(1),
                            Tax = reader.GetDecimal(2)

                        };
                        activities.Add(activity);
                    }
                }
            }
            return activities;
        }

        public void Update(Activity item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Update Activities set activityName = @name, tax = @tax where activityId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.ActivityId);
                command.Parameters.Add(idParam);
                SqlParameter name = new SqlParameter("@name", item.ActivityName);
                command.Parameters.Add(name);
                SqlParameter tax = new SqlParameter("@tax", item.Tax);
                command.Parameters.Add(tax);
                command.ExecuteNonQuery();
            }
        }
    }
}
