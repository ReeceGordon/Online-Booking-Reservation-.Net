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

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private static MySqlConnection connection = MySqlSetup.Connect();

        private MySqlConnection connectionNonStatic = MySqlSetup.Connect();

        private readonly ILogger<EmailController> logger;

        public EmailController(ILogger<EmailController> logger)
        {
            this.logger = logger;
        }
        //loads the email settings to be used when sending an email
        public static List<SettingsModel> EmailSettings()
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

        public List<SettingsModel> EmailSettingsNonStatic()
        {
            List<SettingsModel> settings = new List<SettingsModel>();
            try
            {
                connectionNonStatic.Open();
                string query = "SELECT * FROM Settings";
                var cmd = new MySqlCommand(query, connectionNonStatic);
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

                    });
                }
                connectionNonStatic.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return settings;
        }
        [HttpPost]

        public ResponseModel SendEmail(EmailModel email)
        {
            List<SettingsModel> settings = EmailSettingsNonStatic();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(settings[0].SmtpServer);

                mail.From = new MailAddress(settings[0].Email_Sender);
                mail.To.Add(email.To);
                mail.Subject = email.Subject;
                mail.Body = email.Body;
                mail.IsBodyHtml = email.HtmlBody;
                SmtpServer.Port = int.Parse(settings[0].Port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(settings[0].Email_Sender, settings[0].Email_Pass);
                if (settings[0].SSL == "true")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }


                SmtpServer.Send(mail);
                ResponseModel response = new ResponseModel
                {
                    Status = true
                };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }
        }
        //sends an email returns a response letting the front end know if it was successfull or not
        [Route("NoRoute")]
        public static ResponseModel Send(EmailModel email)
        {
            List<SettingsModel> settings = EmailSettings();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(settings[0].SmtpServer);

                mail.From = new MailAddress(settings[0].Email_Sender);
                mail.To.Add(email.To);
                mail.Subject = email.Subject;
                mail.Body = email.Body;
                mail.IsBodyHtml = email.HtmlBody;
                SmtpServer.Port = int.Parse(settings[0].Port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(settings[0].Email_Sender, settings[0].Email_Pass);
                if(settings[0].SSL == "true")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }
                

                SmtpServer.Send(mail);
                ResponseModel response = new ResponseModel
                {
                    Status = true
                };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }
        }

        [Route("Test")]
        //used to check if the email settings are correct and send a response based on the testing
        public static ResponseModel Test(EmailTesterModel Data)
        {
            Console.WriteLine(Data.Email_Sender);
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Data.SmtpServer);

                mail.From = new MailAddress(Data.Email_Sender);
                mail.To.Add(Data.To);
                mail.Subject = Data.Subject;
                mail.Body = Data.Body;
                mail.IsBodyHtml = Data.HtmlBody;
                SmtpServer.Port = int.Parse(Data.Port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(Data.Email_Sender, Data.Email_Pass);
                if (Data.SSL == "true")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }
                SmtpServer.Send(mail);
                ResponseModel response = new ResponseModel
                {
                    Status = true
                };
                return response;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ResponseModel response = new ResponseModel
                {
                    Status = false
                };
                return response;
            }
        }
    }
}