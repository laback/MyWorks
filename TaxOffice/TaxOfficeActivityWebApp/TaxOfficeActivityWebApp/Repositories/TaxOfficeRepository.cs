using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.Repositories
{
    public class TaxOfficeRepository : IRepository<TaxOffice>
    {
        private string _connectionString;

        public TaxOfficeRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(TaxOffice item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Insert into TaxOffices values(@districtId, @taxOfficeName)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter districtId = new SqlParameter("@districtId", item.DistrictId);
                command.Parameters.Add(districtId);
                SqlParameter taxOfficeName = new SqlParameter("@taxOfficeName", item.TaxOfficeName);
                command.Parameters.Add(taxOfficeName);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(TaxOffice item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Delete TaxOffices where taxOfficeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.TaxOfficeId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public TaxOffice Get(int id)
        {
            TaxOffice taxOffice = new TaxOffice();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select taxOfficeId, Districts.districtId, Districts.districtName, taxOfficeName from TaxOffices " +
                    "inner join Districts on Districts.districtId = TaxOffices.districtId " +
                    "where taxOfficeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        taxOffice.TaxOfficeId = reader.GetInt32(0);
                        taxOffice.DistrictId = reader.GetInt32(1);
                        taxOffice.District = new District();
                        taxOffice.District.DistrictName = reader.GetString(2);
                        taxOffice.TaxOfficeName = reader.GetString(3);
                    }
                }
            }
            return taxOffice;
        }

        public IEnumerable<TaxOffice> GetAll()
        {
            List<TaxOffice> taxOffices = new List<TaxOffice>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select taxOfficeId, Districts.districtId, Districts.districtName, taxOfficeName from TaxOffices " +
                    "inner join Districts on Districts.districtId = TaxOffices.districtId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TaxOffice taxOffice = new TaxOffice();
                        taxOffice.TaxOfficeId = reader.GetInt32(0);
                        taxOffice.DistrictId = reader.GetInt32(1);
                        taxOffice.District = new District();
                        taxOffice.District.DistrictName = reader.GetString(2);
                        taxOffice.TaxOfficeName = reader.GetString(3);
                        taxOffices.Add(taxOffice);
                    }
                }
            }
            return taxOffices;
        }

        public void Update(TaxOffice item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Update TaxOffices set districtId = @districtId, taxOfficeName = @taxOfficeName where taxOfficeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.TaxOfficeId);
                command.Parameters.Add(idParam);
                SqlParameter districtId = new SqlParameter("@districtId", item.DistrictId);
                command.Parameters.Add(districtId);
                SqlParameter taxOfficeName = new SqlParameter("@taxOfficeName", item.TaxOfficeName);
                command.Parameters.Add(taxOfficeName);
                command.ExecuteNonQuery();
            }
        }
    }
}
