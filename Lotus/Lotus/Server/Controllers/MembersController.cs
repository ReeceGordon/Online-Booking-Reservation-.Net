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
    public class MembersController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();

        private readonly ILogger<MembersController> logger;

        public MembersController(ILogger<MembersController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        //gets all the members in the database and orders the by their name alphabetically
        //returns a list of members
        public IEnumerable<MembersModel> Get()
        {
            List<MembersModel> members = new List<MembersModel>();


            try
            {
                connection.Open();
                string query = "SELECT * FROM Members ORDER BY Name";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //var rand = new Random();
                    //int index = rand.Next(6);
                    members.Add(new MembersModel
                    {
                        Member_Id = reader.GetString(1),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Notes = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6)
                        //Border = BorderColorList[index],
                       // Color = BackgroundColorList[index]


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }


            return members;

        }

        [Route("Get/{Id}")]
        //get a single member by their id
        //returns a list of members containing one member
        public IEnumerable<MembersModel> GetAMember([FromRoute] string Id)
        {
            List<MembersModel> member = new List<MembersModel>();


            try
            {
                connection.Open();
                string query = "SELECT * FROM Members WHERE Member_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    member.Add(new MembersModel
                    {

                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Notes = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6)


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }



            return member;

        }

        [Route("Search/{Criteria}")]
        //returns the staff members that match the criteria
        public IEnumerable<MembersModel> Search([FromRoute] string Criteria)
        {
            List<MembersModel> members = new List<MembersModel>();


            try
            {
                connection.Open();
                string query = "";
                //searches by name and surname if there is a space between the criteria
                if (Criteria.ToLower().Contains(' '))
                {

                    string[] fullname = Criteria.Split(' ');
                    query = "SELECT * FROM Members WHERE Name LIKE '%" + fullname[0] + "%' AND Surname LIKE '%" + fullname[1] + "%' ORDER BY Name";
                }
                //searches by email if it has an @ sign
                else if (Criteria.ToLower().Contains('@'))
                {
                    query = "SELECT * FROM Members WHERE Email LIKE '%" + Criteria + "%' ORDER BY Name";
                }
                //searches by phone if there is a numerical number in the criteria
                else if (!Criteria.Any(c => c < '0' || c > '9'))
                {
                    query = "SELECT * FROM Members WHERE Phone_Number LIKE '%" + Criteria + "%' ORDER BY Name";
                }
                //searches by eithe surname or name if the above do not apply
                else
                {
                    query = "SELECT * FROM Members WHERE Name LIKE '%" + Criteria + "%' OR Surname LIKE '%" + Criteria + "%' ORDER BY Name";
                }

                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    members.Add(new MembersModel
                    {
                        Member_Id = reader.GetString(1),
                        Name = reader.GetString(2),
                        Surname = reader.GetString(3),
                        Notes = reader.GetString(4),
                        Email = reader.GetString(5),
                        Phone_Number = reader.GetInt64(6),


                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }



            return members;

        }

        [HttpPost]
        //adds a new member to the database
        public ResponseModel Post(MembersModel member)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Members(Member_Id, Name, Surname, Notes, Email, Phone_Number) VALUES(@Member_Id, @Name, @Surname, @Notes, @Email, @Phone_Number)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Member_Id", member.Member_Id);
                cmd.Parameters.AddWithValue("@Name", member.Name);
                cmd.Parameters.AddWithValue("@Surname", member.Surname);
                cmd.Parameters.AddWithValue("@Notes", member.Notes);
                cmd.Parameters.AddWithValue("@Email", member.Email);
                cmd.Parameters.AddWithValue("@Phone_Number", member.Phone_Number);
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
                Console.WriteLine("MY SQL EXCEPTION BELOW:");
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
        //deletes a member by their id
        public void Delete([FromRoute] string Id)
        {
            connection.Open();
            string query = "DELETE FROM Members WHERE Member_Id = @Member_Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Member_Id", Id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        [HttpPut]
        //updates a member by their id
        public void Update(MembersModel member)
        {

            connection.Open();
            string query = "UPDATE Members SET Name = @Name,Surname = @Surname, Notes = @Notes, Email = @Email, Phone_Number = @Phone_Number WHERE Member_Id = @Member_Id";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Member_Id", member.Member_Id);
            cmd.Parameters.AddWithValue("@Name", member.Name);
            cmd.Parameters.AddWithValue("@Surname", member.Surname);
            cmd.Parameters.AddWithValue("@Notes", member.Notes);
            cmd.Parameters.AddWithValue("@Email", member.Email);
            cmd.Parameters.AddWithValue("@Phone_Number", member.Phone_Number);
            cmd.ExecuteScalar();
            connection.Close();
        }


    }
}