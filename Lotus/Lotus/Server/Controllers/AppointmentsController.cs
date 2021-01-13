using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lotus.Shared;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using NuGet.Frameworks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();

        private readonly ILogger<AppointmentsController> logger;

        public AppointmentsController(ILogger<AppointmentsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        
        [Route("Get/{From}/{To}/{Id}")]

        // used to get the appointments a staff member has from one date to another
        //returns a list of appointments
        public IEnumerable<AppointmentsModel> Get([FromRoute] string From, string To, string Id)
        {
            List<AppointmentsModel> appointments = new List<AppointmentsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Appointments WHERE App_Date >= '" + From + "' AND App_Date <= '" + To + "' ORDER BY Start";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(2) == Id)
                    {

                        appointments.Add(new AppointmentsModel
                        {
                            Id = reader.GetInt16(0),
                            Member_Id = reader.GetString(1),
                            Staff_Id = reader.GetString(2),
                            Type = reader.GetString(3),
                            Full_Name = reader.GetString(4),
                            Start = reader.GetString(5),
                            Duration = reader.GetInt16(6),
                            End = reader.GetString(7),
                            App_Date = reader.GetDateTime(8),
                            Status = reader.GetString(9)
                        });
                    }
                }
                connection.Close();
                Console.WriteLine(appointments.Count());
                Console.WriteLine(Id);
                Console.WriteLine(From);
                Console.WriteLine(query);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;

        }
        [Route("GetAllByMemberID/{Id}")]
        //used to get all the appointment of a member by using their id as the fitler
        //returns a list of appointments
        public IEnumerable<AppointmentsModel> GetAllByMemberID([FromRoute] string Id)
        {
            List<AppointmentsModel> appointments = new List<AppointmentsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Appointments WHERE Member_Id = '" + Id + "' ORDER BY App_Date,Start";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    appointments.Add(new AppointmentsModel
                        {
                            Id = reader.GetInt16(0),
                            Member_Id = reader.GetString(1),
                            Staff_Id = reader.GetString(2),
                            Type = reader.GetString(3),
                            Full_Name = reader.GetString(4),
                            Start = reader.GetString(5),
                            Duration = reader.GetInt16(6),
                            End = reader.GetString(7),
                            App_Date = reader.GetDateTime(8),
                            Status = reader.GetString(9)
                        });
                    
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;

        }
        [Route("GetByDate/{Date}/{ID}")]
        //gets all appointments that are of that day and filters them by the staffs_id this is used to call back less records than calling
        //all of the appoitnmets for the whole month which in turn results into to a more efficient system overall
        //returns a list of appointments
        public IEnumerable<AppointmentsModel> GetByDate([FromRoute] string Date, string ID)
        {
            List<AppointmentsModel> appointments = new List<AppointmentsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Appointments WHERE App_Date = '" + Date + "' ORDER BY Start";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(2) == ID)
                    {

                        appointments.Add(new AppointmentsModel
                        {
                            Id = reader.GetInt16(0),
                            Member_Id = reader.GetString(1),
                            Staff_Id = reader.GetString(2),
                            Type = reader.GetString(3),
                            Full_Name = reader.GetString(4),
                            Start = reader.GetString(5),
                            Duration = reader.GetInt16(6),
                            End = reader.GetString(7),
                            App_Date = reader.GetDateTime(8),
                            Status = reader.GetString(9)
                        });
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;

        }
        
        [Route("GetReview/{ID}")]
        //gets the appointment based on the id used for when the staff is sent and email to either approve or reject an appointment
        //returns a list of appointments containing one appointment
        public IEnumerable<AppointmentsModel> GetReview([FromRoute] string ID)
        {
            List<AppointmentsModel> appointments = new List<AppointmentsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Appointments WHERE Id = '" + ID + "' AND Status = 'Waiting'";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                        appointments.Add(new AppointmentsModel
                        {
                            Id = reader.GetInt16(0),
                            Member_Id = reader.GetString(1),
                            Staff_Id = reader.GetString(2),
                            Type = reader.GetString(3),
                            Full_Name = reader.GetString(4),
                            Start = reader.GetString(5),
                            Duration = reader.GetInt16(6),
                            End = reader.GetString(7),
                            App_Date = reader.GetDateTime(8),
                            Status = reader.GetString(9)
                        });  
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;

        }
        [Route("GetWaiting/{Id}")]
        //used to load all the appointments that have not been approved filtering them by the staff id
        //returns a list of appointments
        public IEnumerable<AppointmentsModel> GetWaiting([FromRoute] string Id)
        {
            List<AppointmentsModel> appointments = new List<AppointmentsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Appointments WHERE Status = 'Waiting' ORDER BY Start";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(2) == Id)
                    {

                        appointments.Add(new AppointmentsModel
                        {
                            Id = reader.GetInt16(0),
                            Member_Id = reader.GetString(1),
                            Staff_Id = reader.GetString(2),
                            Type = reader.GetString(3),
                            Full_Name = reader.GetString(4),
                            Start = reader.GetString(5),
                            Duration = reader.GetInt16(6),
                            End = reader.GetString(7),
                            App_Date = reader.GetDateTime(8),
                            Status = reader.GetString(9)
                        });
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;

        }
        [HttpPost]
        //adds a new appointment to the database
        //returns a respone to as if the appointment was made successfully or not
        public ResponseModel Add(AppointmentsModel data)
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO Appointments (Member_Id, Staff_Id, Type, Full_Name,Start,Duration,End,App_Date,Status) VALUES(@Member_Id, @Staff_Id, @Type, @Full_Name,@Start,@Duration,@End,@App_Date,@Status)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Member_Id", data.Member_Id);
                cmd.Parameters.AddWithValue("@Staff_Id", data.Staff_Id);
                cmd.Parameters.AddWithValue("@Type", data.Type);
                cmd.Parameters.AddWithValue("@Full_Name", data.Full_Name);
                cmd.Parameters.AddWithValue("@Start", data.Start);
                cmd.Parameters.AddWithValue("@Duration", data.Duration);
                cmd.Parameters.AddWithValue("@End", data.End);
                cmd.Parameters.AddWithValue("@App_Date", data.App_Date);
                cmd.Parameters.AddWithValue("@Status", data.Status);

                cmd.ExecuteScalar();

                if (cmd.LastInsertedId != null) cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                connection.Close();
                ResponseModel response = new ResponseModel
                {
                    Status = true,
                    Message = Convert.ToInt32(cmd.Parameters["@newId"].Value).ToString()

                };
                return response;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }

        }
        [HttpPut]
        [Route("Approve")]
        //approves an appointmend based on the id of the appointment 
        //returns a response to know if the action was carried out succesfully
        public ResponseModel Update(AppointmentsModel data)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Appointments SET Status = 'Confirmed' WHERE Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteScalar();
                connection.Close();
                ResponseModel response = new ResponseModel
                {
                    Status = true
                };
                return response;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }

        }

        [Route("Update")]
        //updates the appointment
        //returns a response to know if the action was carried out succesfully
        public ResponseModel UpdateTimeSlot(AppointmentsModel data)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Appointments SET App_Date = @app_date, Start = @Start, End = @End WHERE Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@app_date", data.App_Date);
                cmd.Parameters.AddWithValue("@Start", data.Start);
                cmd.Parameters.AddWithValue("@End", data.End);
                cmd.Parameters.AddWithValue("@Id", data.Id);

                cmd.ExecuteScalar();
                connection.Close();
                ResponseModel response = new ResponseModel
                {
                    Status = true
                };
                return response;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }

        }

        [HttpDelete]
        [Route("{Id}")]
        //deletes an appointment base on the appointments id
        public void Delete([FromRoute] string Id)
        {
            connection.Open();
            string query = "DELETE FROM Appointments WHERE Id = @Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            connection.Close();


        }
    }
}