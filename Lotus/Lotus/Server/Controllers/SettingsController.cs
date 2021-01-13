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
    public class SettingsController : ControllerBase
    {
        private MySqlConnection connection = MySqlSetup.Connect();

        private readonly ILogger<SettingsController> logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        //return a list of the companies settings
        public IEnumerable<SettingsModel> GetAllSettings()
        {
            List<SettingsModel> settings = new List<SettingsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Settings";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    settings.Add(new SettingsModel
                    {
                        Email = reader.GetString(1),
                        Address = reader.GetString(2),
                        Phone_Number = reader.GetInt64(3),
                        Currency = reader.GetString(4),
                        Password = reader.GetString(5),
                        Email_Sender = reader.GetString(6),
                        SmtpServer = reader.GetString(7),
                        Port = reader.GetString(8),
                        Email_Pass = reader.GetString(9),
                        SSL = reader.GetString(10),
                        Company_Name = reader.GetString(11),
                        Require_Aproval = Convert.ToBoolean(reader.GetInt32(12))

                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return settings;

        }
        [Route("ContactDetails")]
        //returns the contact details of the company this ensures passwords and other email sensitive information are not sent to non authorized members
        public IEnumerable<SettingsModel> GetContactDetails()
        {
            List<SettingsModel> settings = new List<SettingsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Settings";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    settings.Add(new SettingsModel
                    {
                        Email = reader.GetString(1),
                        Address = reader.GetString(2),
                        Phone_Number = reader.GetInt64(3)
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return settings;

        }

        [Route("GetPassword")]

        public IEnumerable<SettingsModel> GetPassword()
        {
            List<SettingsModel> settings = new List<SettingsModel>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Settings";
                var cmd = new MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    settings.Add(new SettingsModel
                    {
                        Password = reader.GetString(5),
                        
                    });
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return settings;

        }

        [HttpPut]
        //updates the settings 
        public void Update(SettingsModel settings)
        {

            connection.Open();

            string query = "UPDATE Settings SET Email = @Email,Address = @Address, Phone_Number = @Phone_Number, Currency = @Currency, Password = @Password " +
                ", Email_Sender = @Email_Sender, SmtpServer = @SmtpServer, Port = @Port, Email_Pass = @Email_Pass, Company_Name = @Company_Name,SSLsetting = @SSL,Require_Aproval = @Require_Aproval WHERE Id = 1";
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", settings.Email);
            cmd.Parameters.AddWithValue("@Address", settings.Address);
            cmd.Parameters.AddWithValue("@Phone_Number", settings.Phone_Number);
            cmd.Parameters.AddWithValue("@Currency", settings.Currency);
            cmd.Parameters.AddWithValue("@Password", settings.Password);
            cmd.Parameters.AddWithValue("@Email_Sender", settings.Email_Sender);
            cmd.Parameters.AddWithValue("@SmtpServer", settings.SmtpServer);
            cmd.Parameters.AddWithValue("@Port", settings.Port);
            cmd.Parameters.AddWithValue("@Email_Pass", settings.Email_Pass);
            cmd.Parameters.AddWithValue("@SSL", settings.SSL);
            cmd.Parameters.AddWithValue("@Company_Name", settings.Company_Name);
            if(settings.Require_Aproval)
            {
                cmd.Parameters.AddWithValue("@Require_Aproval", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Require_Aproval", 0);
            }
            
            cmd.ExecuteScalar();
            connection.Close();
        }
    }
}