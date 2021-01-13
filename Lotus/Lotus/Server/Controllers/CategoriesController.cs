using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lotus.Shared;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private MySqlConnection connection = MySqlSetup.Connect();

        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(ILogger<CategoriesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        //gets all categories
        //returns a list of all categories
        public IEnumerable<CategoriesModel> Get()
        {
            List<CategoriesModel> categories = new List<CategoriesModel>();


            try
            {
                connection.Open();
                string query = "SELECT * FROM Categories";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new CategoriesModel
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return categories;

        }

        [HttpPost]
        //adds a new category to the database
        public void Post(CategoriesModel categories)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Categories(Id, Name, Description) VALUES(@Id, @Name, @Description)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", categories.Id.Trim());
                cmd.Parameters.AddWithValue("@Name", categories.Name);
                cmd.Parameters.AddWithValue("@Description", categories.Description);
                cmd.ExecuteScalar();
                connection.Close();
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [HttpDelete]
        [Route("{Id}")]
        //deletes a category by its id
        public void Delete([FromRoute] string Id)
        {
            connection.Open();
            string query = "DELETE FROM Categories WHERE Id = @Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        [HttpPut]
        //updates a category
        public void Update(CategoriesModel categories)
        {
            connection.Open();
            string query = "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", categories.Id);
            cmd.Parameters.AddWithValue("@Name", categories.Name);
            cmd.Parameters.AddWithValue("@Description", categories.Description);
            cmd.ExecuteScalar();
            connection.Close();
        }
    }
}