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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();
        

        private readonly ILogger<StaffController> logger;

        public StaffController(ILogger<StaffController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        //returns all the staff members int he database
        public IEnumerable<StaffModel> Get()
        {
            List<StaffModel> staff = new List<StaffModel>();


            try
            {
                connection.Open();
                string query = "SELECT * FROM Staff ORDER BY Name";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staff.Add(new StaffModel
                    {
                        Staff_Id = reader.GetString(1),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Details = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6),
                        Image = reader.GetString(7),


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }



            return staff;

        }

        [Route("Get/{Id}")]
        //returns a singular staff member using their id
        public IEnumerable<StaffModel> GetAStaff([FromRoute] string Id)
        {
            List<StaffModel> staff = new List<StaffModel>();


            try
            {
                connection.Open();
                string query = "SELECT * FROM Staff WHERE Staff_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staff.Add(new StaffModel
                    {
                        
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Details = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6),
                        Image = reader.GetString(7),


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }



            return staff;

        }
        //returns the staff members that match the criteria
        [Route("Search/{Criteria}")]
        public IEnumerable<StaffModel> Search([FromRoute] string Criteria)
        {
            List<StaffModel> staff = new List<StaffModel>();


            try
            {
                connection.Open();
                string query = "";
                //searches by name and surname if there is a space between the criteria
                if (Criteria.ToLower().Contains(' '))
                {

                    string[] fullname = Criteria.Split(' ');
                    query = "SELECT * FROM Staff WHERE Name LIKE '%" + fullname[0] + "%' AND Surname LIKE '%" + fullname[1] + "%' ORDER BY Name";
                }
                //searches by email if it has an @ sign
                else if (Criteria.ToLower().Contains('@'))
                {
                    query = "SELECT * FROM Staff WHERE Email LIKE '%" + Criteria + "%' ORDER BY Name";
                }
                //searches by phone if there is a numerical number in the criteria
                else if (!Criteria.Any(c => c < '0' || c > '9'))
                {
                    query = "SELECT * FROM Staff WHERE Phone_Number LIKE '%" + Criteria + "%' ORDER BY Name";
                }
                //searches by eithe surname or name if the above do not apply
                else
                {
                    query = "SELECT * FROM Staff WHERE Name LIKE '%" + Criteria + "%' OR Surname LIKE '%" + Criteria + "%' ORDER BY Name";
                }
                
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staff.Add(new StaffModel
                    {
                        Staff_Id = reader.GetString(1),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Details = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6),
                        Image = reader.GetString(7),


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }



            return staff;

        }

        [HttpPost]
        //adds a new staff to the database
        public ResponseModel Post(StaffModel staff)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Staff(Staff_Id, Name, Surname, Details, Email, Phone_Number, Image) VALUES(@Staff_Id, @Name, @Surname, @Details, @Email, @Phone_Number, @Image)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Staff_Id", staff.Staff_Id);
                cmd.Parameters.AddWithValue("@Name", staff.Name);
                cmd.Parameters.AddWithValue("@Surname", staff.Surname);
                cmd.Parameters.AddWithValue("@Details", staff.Details);
                cmd.Parameters.AddWithValue("@Email", staff.Email);
                cmd.Parameters.AddWithValue("@Phone_Number", staff.Phone_Number);
                cmd.Parameters.AddWithValue("@Image", staff.Image);
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
        //deletes a staff member by their id
        public void Delete([FromRoute] string Id)
        {
            connection.Open();
            string query = "DELETE FROM Staff WHERE Staff_Id = @Staff_Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Staff_Id", Id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        [HttpPut]
        //updates a staff member by their id
        public void Update(StaffModel staff)
        {

            connection.Open();
             
            string query = "UPDATE Staff SET Name = @Name,Surname = @Surname, Details = @Details, Email = @Email, Phone_Number = @Phone_Number, Image = @Image WHERE Staff_Id = @Staff_Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Staff_Id", staff.Staff_Id);
            cmd.Parameters.AddWithValue("@Name", staff.Name);
            cmd.Parameters.AddWithValue("@Surname", staff.Surname);
            cmd.Parameters.AddWithValue("@Details", staff.Details);
            cmd.Parameters.AddWithValue("@Email", staff.Email);
            cmd.Parameters.AddWithValue("@Phone_Number", staff.Phone_Number);
            cmd.Parameters.AddWithValue("@Image", staff.Image);
            cmd.ExecuteScalar();
            connection.Close();
        }
    

    }
}