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
    public class OpeningController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();
        private readonly ILogger<OpeningController> logger;

        public OpeningController(ILogger<OpeningController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        //gets all the working hours of the shop
        public IEnumerable<OpeningModel> Get()
        {
            List<OpeningModel> openings = new List<OpeningModel>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Opening_hours";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //used to convert a true false value to numerical as mysql does not have a bool type
                    bool ClosedValue = false;
                    if(reader.GetInt16(4) == 1)
                    {
                        ClosedValue = true;
                    }
                    openings.Add(new OpeningModel
                    {
                        Id = reader.GetInt16(0),
                        Day = reader.GetString(1),
                        Opening = reader.GetInt16(2),
                        Closing = reader.GetInt16(3),
                        Closed = ClosedValue,
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return openings;

        }

        

        [HttpPut]
        //updates the shops working hours
        public void Update(List<OpeningModel> opening)
        {
            connection.Open();
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    
                    string query = "UPDATE Opening_hours SET  Opening = @Opening,Closing = @Closing,Closed = @Closed WHERE Id = @Id";
                    var cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Id", i + 1);
                    cmd.Parameters.AddWithValue("@Opening", opening[i].Opening);
                    cmd.Parameters.AddWithValue("@Closing", opening[i].Closing);
                    cmd.Parameters.AddWithValue("@Closed", Convert.ToInt32(opening[i].Closed));
                    cmd.ExecuteScalar();
                    

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            connection.Close();
        }

    }
}