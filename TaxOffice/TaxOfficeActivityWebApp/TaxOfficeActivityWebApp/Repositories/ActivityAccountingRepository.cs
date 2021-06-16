using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.Repositories
{
    public class ActivityAccountingRepository : IRepository<ActivityAccounting>
    {
        private string _connectionString;

        public ActivityAccountingRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(ActivityAccounting item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Insert into ActivitiesAccounting values(@entrepreneurId, @activityId, @year, @quarter, @income, @isTaxPaid)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter entrepreneurId = new SqlParameter("@entrepreneurId", item.EntrepreneurId);
                command.Parameters.Add(entrepreneurId);
                SqlParameter activityId = new SqlParameter("@activityId", item.ActivityId);
                command.Parameters.Add(activityId);
                SqlParameter year = new SqlParameter("@year", item.Year);
                command.Parameters.Add(year);
                SqlParameter quarter = new SqlParameter("@quarter", item.Quarter);
                command.Parameters.Add(quarter);
                SqlParameter income = new SqlParameter("@income", item.Income);
                command.Parameters.Add(income);
                SqlParameter isTaxPaid = new SqlParameter("@isTaxPaid", item.IsTaxPaid);
                command.Parameters.Add(isTaxPaid);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(ActivityAccounting item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Delete ActivitiesAccounting where activityAccountingId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.ActivityAccountingId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public ActivityAccounting Get(int id)
        {
            ActivityAccounting activityAccounting = new ActivityAccounting();
            EntrepreneurRepository eRepository = new EntrepreneurRepository(_connectionString);
            ActivityRepository aRepository = new ActivityRepository(_connectionString);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select activityAccountingId, entrepreneurId, activityId, year, quarter, income, isTaxPaid from ActivitiesAccounting " +
                    "where activityAccountingId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        activityAccounting.ActivityAccountingId = reader.GetInt32(0);
                        activityAccounting.EntrepreneurId = reader.GetInt32(1);
                        activityAccounting.Entrepreneur = eRepository.Get(reader.GetInt32(1));
                        activityAccounting.ActivityId = reader.GetInt32(2);
                        activityAccounting.Activity = aRepository.Get(reader.GetInt32(2));
                        activityAccounting.Year = reader.GetInt32(3);
                        activityAccounting.Quarter = reader.GetInt32(4);
                        activityAccounting.Income = reader.GetDecimal(5);
                        activityAccounting.IsTaxPaid = reader.GetBoolean(6);
                    }
                }
            }
            return activityAccounting;
        }

        public IEnumerable<ActivityAccounting> GetAll()
        {
            List<ActivityAccounting> activitiesAccountings = new List<ActivityAccounting>();
            EntrepreneurRepository eRepository = new EntrepreneurRepository(_connectionString);
            ActivityRepository aRepository = new ActivityRepository(_connectionString);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select activityAccountingId, entrepreneurId, activityId, year, quarter, income, isTaxPaid from ActivitiesAccounting";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ActivityAccounting activityAccounting = new ActivityAccounting();
                        activityAccounting.ActivityAccountingId = reader.GetInt32(0);
                        activityAccounting.EntrepreneurId = reader.GetInt32(1);
                        activityAccounting.Entrepreneur = eRepository.Get(reader.GetInt32(1));
                        activityAccounting.ActivityId = reader.GetInt32(2);
                        activityAccounting.Activity = aRepository.Get(reader.GetInt32(2));
                        activityAccounting.Year = reader.GetInt32(3);
                        activityAccounting.Quarter = reader.GetInt32(4);
                        activityAccounting.Income = reader.GetDecimal(5);
                        activityAccounting.IsTaxPaid = reader.GetBoolean(6);
                        activitiesAccountings.Add(activityAccounting);
                    }
                }
            }
            return activitiesAccountings;
        }

        public void Update(ActivityAccounting item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Update ActivitiesAccounting set entrepreneurId = @entrepreneurId, activityId = @activityId, year = @year, " +
                    "quarter = @quarter, income = @income, isTaxPaid = @isTaxPaid where activityAccountingId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.ActivityAccountingId);
                command.Parameters.Add(idParam);
                SqlParameter entrepreneurId = new SqlParameter("@entrepreneurId", item.EntrepreneurId);
                command.Parameters.Add(entrepreneurId);
                SqlParameter activityId = new SqlParameter("@activityId", item.ActivityId);
                command.Parameters.Add(activityId);
                SqlParameter year = new SqlParameter("@year", item.Year);
                command.Parameters.Add(year);
                SqlParameter quarter = new SqlParameter("@quarter", item.Quarter);
                command.Parameters.Add(quarter);
                SqlParameter income = new SqlParameter("@income", item.Income);
                command.Parameters.Add(income);
                SqlParameter isTaxPaid = new SqlParameter("@isTaxPaid", item.IsTaxPaid);
                command.Parameters.Add(isTaxPaid);
                command.ExecuteNonQuery();
            }
        }
    }
}
