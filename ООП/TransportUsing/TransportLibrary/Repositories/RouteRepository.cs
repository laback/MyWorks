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
    public class RouteRepository : IRepository<Route>
    {
        private readonly string _connectionString;

        public RouteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Route item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "insert into routes values (@userid, @distance, @date, @transport, @passengers, @cargos)";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter userid = new SqlParameter("@userid", item.UserId);
                command.Parameters.Add(userid);
                SqlParameter distance = new SqlParameter("@distance", item.Distance);
                command.Parameters.Add(distance);
                SqlParameter date = new SqlParameter("@date", item.Date);
                command.Parameters.Add(date);
                SqlParameter transport = new SqlParameter("@transport", item.TransportId);
                command.Parameters.Add(transport);
                SqlParameter passengers = new SqlParameter("@passengers", item.Passengers);
                command.Parameters.Add(passengers);
                SqlParameter cagros = new SqlParameter("@cargos", item.Cargo);
                command.Parameters.Add(cagros);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "delete from Routes where routeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter routeId = new SqlParameter("@id", id);
                command.Parameters.Add(routeId);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Route> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select routeId, Routes.userId, Users.FIO, distance, date, Routes.transportId, passengers, cargo " +
                    "from Routes " +
                    "inner join Users on Users.userId = Routes.userId " +
                    "inner join Transports on Transports.transportId = Routes.transportId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<Route> routes = new List<Route>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Route route = new Route();
                        route.RouteId = reader.GetInt32(0);
                        route.UserId = reader.GetInt32(1);
                        route.User = new User();
                        route.User.FIO = reader.GetString(2);
                        route.Distance = reader.GetInt32(3);
                        route.Date = reader.GetDateTime(4);
                        route.TransportId = reader.GetInt32(5);
                        route.Passengers = reader.GetInt32(6);
                        route.Cargo = reader.GetInt32(7);
                        routes.Add(route);
                    }
                };
                return routes;
            }
        }

        public Route Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select routeId, Routes.userId, Users.FIO, distance, date, Routes.transportId, passengers, cargo " +
                    "from Routes " +
                    "inner join Users on Users.userId = Routes.userId " +
                    "inner join Transports on Transports.transportId = Routes.transportId " +
                    "where routeId = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter routeId = new SqlParameter("@id", id);
                command.Parameters.Add(routeId);
                SqlDataReader reader = command.ExecuteReader();
                Route route = new Route();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        route.RouteId = reader.GetInt32(0);
                        route.UserId = reader.GetInt32(1);
                        route.User = new User();
                        route.User.FIO = reader.GetString(2);
                        route.Distance = reader.GetInt32(3);
                        route.Date = reader.GetDateTime(4);
                        route.TransportId = reader.GetInt32(5);
                        route.Transport = new Transport();
                        route.Passengers = reader.GetInt32(6);
                        route.Cargo = reader.GetInt32(7);
                    }
                };
                return route;
            }
        }

        public void Update(Route item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "update Routes set userId = @userid, distance = @distance, date = @date, transportId = @transportid, passengers = @passengers, cargo = @cargo where routeId = @id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter userid = new SqlParameter("@userid", item.UserId);
                command.Parameters.Add(userid);
                SqlParameter id = new SqlParameter("@id", item.RouteId);
                command.Parameters.Add(id);
                SqlParameter distance = new SqlParameter("@distance", item.Distance);
                command.Parameters.Add(distance);
                SqlParameter date = new SqlParameter("@date", item.Date);
                command.Parameters.Add(date);
                SqlParameter transport = new SqlParameter("@transportid", item.TransportId);
                command.Parameters.Add(transport);
                SqlParameter passengers = new SqlParameter("@passengers", item.Passengers);
                command.Parameters.Add(passengers);
                SqlParameter cagros = new SqlParameter("@cargo", item.Cargo);
                command.Parameters.Add(cagros);
                command.ExecuteNonQuery();
            }
        }
    }
}
