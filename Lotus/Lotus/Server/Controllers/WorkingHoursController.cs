using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lotus.Shared;
using MySql.Data.MySqlClient;
using Lotus.Server;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkingHoursController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();
        private readonly ILogger<WorkingHoursController> logger;

        public WorkingHoursController(ILogger<WorkingHoursController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("{ID}")]
        //gets the working hours of a staff member using the id of that staff member
        //returns back a list of the staffs working hours
        public IEnumerable<WorkingHoursModel> Get([FromRoute] string ID)
        {
            List<WorkingHoursModel> workingHours = new List<WorkingHoursModel>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM working_hours WHERE Staff_Id = @ID";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", ID);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    workingHours.Add(new WorkingHoursModel
                    {
                        Id = reader.GetInt16(0),
                        Staff_Id = reader.GetString(1),
                        MondayHours = reader.GetString(2),
                        MondayClosed = reader.GetInt16(3) == 1 ? true: false,
                        TuesdayHours = reader.GetString(4),
                        TuesdayClosed = reader.GetInt16(5) == 1 ? true : false,
                        WednesdayHours = reader.GetString(6),
                        WednesdayClosed = reader.GetInt16(7) == 1 ? true : false,
                        ThursdayHours = reader.GetString(8),
                        ThursdayClosed = reader.GetInt16(9) == 1 ? true : false,
                        FridayHours = reader.GetString(10),
                        FridayClosed = reader.GetInt16(11) == 1 ? true : false,
                        SaturdayHours = reader.GetString(12),
                        SaturdayClosed = reader.GetInt16(13) == 1 ? true : false,
                        SundayHours = reader.GetString(14),
                        SundayClosed = reader.GetInt16(15) == 1 ? true : false,
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return workingHours;

        }
        [HttpPost]
        //used when adding a staff this is populated from the shops working hours
        public void Add(WorkingHoursModel workingHours)
        {
            connection.Open();

            try
            {

                string query = "INSERT INTO working_hours(Staff_Id,MondayHours,MondayClosed,TuesdayHours,TuesdayClosed,WednesdayHours,WednesdayClosed," +
                    "ThursdayHours,ThursdayClosed,FridayHours,FridayClosed,SaturdayHours,SaturdayClosed,SundayHours,SundayClosed)" +
                    " VALUES(@Staff_Id,@MondayHours,@MondayClosed,@TuesdayHours,@TuesdayClosed,@WednesdayHours,@WednesdayClosed,@ThursdayHours,@ThursdayClosed," +
                    "@FridayHours,@FridayClosed,@SaturdayHours,@SaturdayClosed,@SundayHours,@SundayClosed)";
                var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Staff_Id", workingHours.Staff_Id);
                cmd.Parameters.AddWithValue("@MondayHours", workingHours.MondayHours);
                cmd.Parameters.AddWithValue("@MondayClosed", workingHours.MondayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@TuesdayHours", workingHours.TuesdayHours);
                cmd.Parameters.AddWithValue("@TuesdayClosed", workingHours.TuesdayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@WednesdayHours", workingHours.WednesdayHours);
                cmd.Parameters.AddWithValue("@WednesdayClosed", workingHours.WednesdayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@ThursdayHours", workingHours.ThursdayHours);
                cmd.Parameters.AddWithValue("@ThursdayClosed", workingHours.ThursdayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@FridayHours", workingHours.FridayHours);
                cmd.Parameters.AddWithValue("@FridayClosed", workingHours.FridayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@SaturdayHours", workingHours.SaturdayHours);
                cmd.Parameters.AddWithValue("@SaturdayClosed", workingHours.SaturdayClosed == true ? 1 : 0);
                cmd.Parameters.AddWithValue("@SundayHours", workingHours.SundayHours);
                cmd.Parameters.AddWithValue("@SundayClosed", workingHours.SundayClosed == true ? 1 : 0);

                cmd.ExecuteScalar();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                connection.Close();
            }


        }

        [HttpPut]
        //updates the staff's working hours using the staff id
        public void Update(WorkingHoursModel workingHours)
        {
            connection.Open();

                try
                {

                    string query = "UPDATE working_hours SET  MondayHours = @MondayHours,MondayClosed = @MondayClosed," +
                        "TuesdayHours = @TuesdayHours,TuesdayClosed = @TuesdayClosed," +
                        "WednesdayHours = @WednesdayHours,WednesdayClosed = @WednesdayClosed," +
                        "ThursdayHours = @ThursdayHours,ThursdayClosed = @ThursdayClosed," +
                        "FridayHours = @FridayHours, FridayClosed = @FridayClosed," +
                        "SaturdayHours = @SaturdayHours, SaturdayClosed = @SaturdayClosed," +
                        "SundayHours = @SundayHours, SundayClosed = @SundayClosed WHERE Staff_Id = @Id";
                    var cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Id", workingHours.Staff_Id);
                    cmd.Parameters.AddWithValue("@MondayHours", workingHours.MondayHours);
                    cmd.Parameters.AddWithValue("@MondayClosed", workingHours.MondayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@TuesdayHours", workingHours.TuesdayHours);
                    cmd.Parameters.AddWithValue("@TuesdayClosed", workingHours.TuesdayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@WednesdayHours", workingHours.WednesdayHours);
                    cmd.Parameters.AddWithValue("@WednesdayClosed", workingHours.WednesdayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ThursdayHours", workingHours.ThursdayHours);
                    cmd.Parameters.AddWithValue("@ThursdayClosed", workingHours.ThursdayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@FridayHours", workingHours.FridayHours);
                    cmd.Parameters.AddWithValue("@FridayClosed", workingHours.FridayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@SaturdayHours", workingHours.SaturdayHours);
                    cmd.Parameters.AddWithValue("@SaturdayClosed", workingHours.SaturdayClosed == true ? 1 : 0);
                    cmd.Parameters.AddWithValue("@SundayHours", workingHours.SundayHours);
                    cmd.Parameters.AddWithValue("@SundayClosed", workingHours.SundayClosed == true ? 1 : 0);

                    cmd.ExecuteScalar();
                    connection.Close();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    connection.Close();
                }
            
            
        }
        /// <summary>
        /// deletes a the working hours of a staff member
        /// </summary>
        /// <param name="Id">the staff member id</param>
        [HttpDelete]
        [Route("{Id}")]
        //deletes a staff member by their id
        public void Delete([FromRoute] string Id)
        {
            connection.Open();
            try
            {
                
                string query = "DELETE FROM working_hours WHERE Staff_Id = @Staff_Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Staff_Id", Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                connection.Close();
            }

        }

    }
}
