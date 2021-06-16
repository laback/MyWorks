using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.Repositories
{
    public class EntrepreneurRepository : IRepository<Entrepreneur>
    {
        private string _connectionString;

        public EntrepreneurRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(Entrepreneur item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Insert into Entrepreneurs values(@taxOfficeId, @fullName)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter taxOfficeId = new SqlParameter("@taxOfficeId", item.TaxOfficeId);
                command.Parameters.Add(taxOfficeId);
                SqlParameter fullName = new SqlParameter("@fullName", item.FullName);
                command.Parameters.Add(fullName);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Entrepreneur item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Delete Entrepreneurs where entrepreneurId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.TaxOfficeId);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();
            }
        }

        public Entrepreneur Get(int id)
        {
            Entrepreneur entrepreneur = new Entrepreneur();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select entrepreneurId, TaxOffices.taxOfficeId, TaxOffices.taxOfficeName, fullName from Entrepreneurs " +
                    "inner join TaxOffices on TaxOffices.taxOfficeId = Entrepreneurs.taxOfficeId " +
                    "where entrepreneurId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        entrepreneur.EntrepreneurId = reader.GetInt32(0);
                        entrepreneur.TaxOfficeId = reader.GetInt32(1);
                        entrepreneur.TaxOffice = new TaxOffice();
                        entrepreneur.TaxOffice.TaxOfficeName = reader.GetString(2);
                        entrepreneur.FullName = reader.GetString(3);
                    }
                }
            }
            return entrepreneur;
        }

        public IEnumerable<Entrepreneur> GetAll()
        {
            List<Entrepreneur> entrepreneurs = new List<Entrepreneur>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Select entrepreneurId, TaxOffices.taxOfficeId, TaxOffices.taxOfficeName, fullName from Entrepreneurs " +
                    "inner join TaxOffices on TaxOffices.taxOfficeId = Entrepreneurs.taxOfficeId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entrepreneur entrepreneur = new Entrepreneur();
                        entrepreneur.EntrepreneurId = reader.GetInt32(0);
                        entrepreneur.TaxOfficeId = reader.GetInt32(1);
                        entrepreneur.TaxOffice = new TaxOffice();
                        entrepreneur.TaxOffice.TaxOfficeName = reader.GetString(2);
                        entrepreneur.FullName = reader.GetString(3);
                        entrepreneurs.Add(entrepreneur);
                    }
                }
            }
            return entrepreneurs;
        }

        public void Update(Entrepreneur item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "Update Entrepreneurs set taxOfficeId = @taxOfficeId, fullName = @fullName where entrepreneurId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParam = new SqlParameter("@id", item.EntrepreneurId);
                command.Parameters.Add(idParam);
                SqlParameter taxOfficeId = new SqlParameter("@taxOfficeId", item.TaxOfficeId);
                command.Parameters.Add(taxOfficeId);
                SqlParameter fullName = new SqlParameter("@fullName", item.FullName);
                command.Parameters.Add(fullName);
                command.ExecuteNonQuery();
            }
        }
    }
}
