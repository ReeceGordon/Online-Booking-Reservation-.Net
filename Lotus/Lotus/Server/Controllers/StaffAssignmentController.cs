using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Lotus.Shared;
using NuGet.Frameworks;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StaffAssignmentController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();
        private readonly ILogger<StaffAssignmentController> logger;

        public StaffAssignmentController(ILogger<StaffAssignmentController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        //selects everything from the staff assignment
        //returns a list of assignments
        public IEnumerable<StaffAssignmentModel> Get()
        {
            List<StaffAssignmentModel> staffAssignments = new List<StaffAssignmentModel>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Staff_assignment";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staffAssignments.Add(new StaffAssignmentModel
                    {
                        Id = reader.GetInt16(0),
                        Category_Id = reader.GetString(1),
                        Staff_Id = reader.GetString(2)
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return staffAssignments;

        }
        [HttpGet]
        //gets all categories a staff member belongs to by using the staff members id
        //retuns a list with all the categories assigned to that staff member
        [Route("GetByStaffId/{ID}")]
        public IEnumerable<StaffAssignmentModel> GetByStaffId([FromRoute] string ID)
        {
            List<StaffAssignmentModel> staffAssignments = new List<StaffAssignmentModel>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Staff_assignment WHERE Staff_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", ID);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staffAssignments.Add(new StaffAssignmentModel
                    {
                        Category_Id = reader.GetString(1)
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return staffAssignments;

        }
        [Route("GetByCategoryId/{ID}")]
        //gets all staff assigned to a category using the category id
        //returns a list of all staff member belonging to the category
        public IEnumerable<StaffAssignmentModel> GetByCategoryId([FromRoute] string ID)
        {
            List<StaffAssignmentModel> staffAssignments = new List<StaffAssignmentModel>();

            try
            {
                connection.Open();
                string query = "SELECT * FROM Staff_assignment WHERE Category_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", ID);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staffAssignments.Add(new StaffAssignmentModel
                    {
                        Staff_Id = reader.GetString(2)
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return staffAssignments;

        }
        [HttpPost]
        //assignes a staff member to a category
        public void Post(StaffAssignmentModel data)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Staff_assignment(Category_Id, Staff_Id) VALUES(@Category_Id, @Staff_Id)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Category_Id", data.Category_Id);
                cmd.Parameters.AddWithValue("@Staff_Id", data.Staff_Id);
                cmd.ExecuteScalar();
                connection.Close();
            }

            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        [HttpDelete]
        //deletes all staff members from a category
        [Route("DeleteAll/{CategoryId}")]
        public void DeletewithCategory([FromRoute] string CategoryId)
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM Staff_assignment WHERE Category_Id = @Id";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", CategoryId);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [HttpDelete]
        //deletes a staff member from a category
        [Route("{StaffId}/{CategoryId}")]
        public void DeletewithStaff([FromRoute] string StaffId, string CategoryId)
        {
            Console.WriteLine(StaffId);
            try
            {
                connection.Open();
                string query = "DELETE FROM Staff_assignment WHERE Staff_Id = @Id AND Category_Id = @CategoryId";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", StaffId);
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}