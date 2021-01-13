using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Lotus.Shared;
namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();
        private readonly ILogger<ServiceController> logger;

        public ServiceController(ILogger<ServiceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("{Id}")]
        //gets all the services that belong to a category
        //returns a list of services
        public IEnumerable<ServiceModel> Get([FromRoute] string Id)
        {
            List<ServiceModel> services = new List<ServiceModel>();
            Console.WriteLine(Id);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Services WHERE Category_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    services.Add(new ServiceModel
                    {
                        Id = reader.GetInt16(0),
                        Category_Id = reader.GetString(1),
                        Name = reader.GetString(2),
                        Duration = reader.GetInt16(3),
                        Price = reader.GetDouble(4)
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return services;

        }

        [HttpPost]
        //adds a new service to a category
        public void Post(ServiceModel service)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Services(Category_Id, Name, Duration, Price) VALUES(@Category_Id, @Name, @Duration, @Price)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Category_Id",service.Category_Id.Trim());
                cmd.Parameters.AddWithValue("@Name", service.Name);
                cmd.Parameters.AddWithValue("@Duration", service.Duration);
                cmd.Parameters.AddWithValue("@Price", service.Price);
                cmd.ExecuteScalar();
                connection.Close();
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [HttpDelete]
        [Route("DeleteAll/{Id}")]
        //deletes all services belonging to a category that match the id passed as an argument
        public void DeletewithCategory([FromRoute] string Id)
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM Services WHERE Category_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Id}")]
        //deletes a service based on the id of the service
        public void DeletewithService([FromRoute] string Id)
        {

            try
            {
                connection.Open();
                string query = "DELETE FROM Services WHERE Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [HttpPut]
        //updates a service
        public void Update(ServiceModel service)
        {
            try
            {
                connection.Open();
                string query = "UPDATE Services SET Name = @Name, Duration = @Duration, Price = @Price WHERE Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", service.Id);
                cmd.Parameters.AddWithValue("@Name", service.Name);
                cmd.Parameters.AddWithValue("@Duration", service.Duration);
                cmd.Parameters.AddWithValue("@Price", service.Price);
                cmd.ExecuteScalar();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}