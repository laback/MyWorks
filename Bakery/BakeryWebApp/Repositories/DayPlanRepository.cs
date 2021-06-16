using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class DayPlanRepository : IRepository<DayPlan>
    {
        private string _connectionString;

        public DayPlanRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(DayPlan item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into DayPlans values(@date)";
                SqlCommand command = new(sql, connection);
                SqlParameter date = new("@date", item.Date);
                command.Parameters.Add(date);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(DayPlan item)
        {
            throw new NotImplementedException();
        }

        public DayPlan Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DayPlan> GetAll()
        {
            List<DayPlan> dayPlans = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select dayPlanId, date from DayPlans";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DayPlan dayPlan = new DayPlan();
                        dayPlan.DayPlanId = reader.GetInt32(0);
                        dayPlan.Date = reader.GetDateTime(1);
                        dayPlans.Add(dayPlan);
                    }
                }
            }
            return dayPlans;
        }

        public void Update(DayPlan item)
        {
            throw new NotImplementedException();
        }
    }
}
