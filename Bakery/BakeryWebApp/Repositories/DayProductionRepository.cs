using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class DayProductionRepository : IRepository<DayProduction>
    {
        private string _connectionString;
        public DayProductionRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(DayProduction item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into DayProductions values(@date)";
                SqlCommand command = new(sql, connection);
                SqlParameter date = new("@date", item.Date);
                command.Parameters.Add(date);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(DayProduction item)
        {
            throw new NotImplementedException();
        }

        public DayProduction Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DayProduction> GetAll()
        {
            List<DayProduction> dayProductions = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select dayProductionId, date from DayProductions";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        DayProduction dayProduction = new DayProduction();
                        dayProduction.DayProductionId = reader.GetInt32(0);
                        dayProduction.Date = reader.GetDateTime(1);
                        dayProductions.Add(dayProduction);
                    }
                }
            }
            return dayProductions;
        }

        public void Update(DayProduction item)
        {
            throw new NotImplementedException();
        }
    }
}
