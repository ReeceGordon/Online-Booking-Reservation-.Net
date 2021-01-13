using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Lotus.Shared.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Lotus.Shared;
using Google.Api;

namespace Lotus.Server.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private static string lastUserID;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<AccountsController> _logger;

        public AccountsController(UserManager<ApplicationUser> userManager,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [Route("GrabLast_ID")]
        [HttpGet]
        public string GetLastID()
        {
            return lastUserID;
        }

        [Authorize]
        [Route("Grab_ID")]
        [HttpGet]
        public async Task<string> Get()
        {
            ApplicationUser user = await GetCurrentUserByNameAsync();
            //ApplicationUser user = await _userManager.GetUserAsync(User);

            //var usr = await _userManager.Get

            //This authorisation needs to be changed to role based rather than ID based.
            if (user?.Id == "NULL")
            {
                Console.WriteLine("Grab_ID Call has failed at string NULL Check");
                return "ACCESS DENIED.";
            }
            else if (user == null)
            {
                Console.WriteLine("Grab_ID Call has failed at Null Check");
                return "ACCESS DENIED.";
            }
            else {
                return user?.Id;
            }
            //return "TEST RETURN FAM";
        }

        [Authorize]
        [Route("Grab_FNAME")]
        [HttpGet]
        public async Task<string> GetFNAME()
        {
            ApplicationUser user = await GetCurrentUserByNameAsync();
            if (user == null)
            {
                Console.WriteLine("Grab_FNAME Call has failed at Null Check");
                return "ACCESS DENIED.";
            }
            else
            {
                return user?.FirstName;
            }
        }

        [Authorize]
        [Route("Grab_LNAME")]
        [HttpGet]
        public async Task<string> GetLNAME()
        {
            ApplicationUser user = await GetCurrentUserByNameAsync();
            if (user == null)
            {
                Console.WriteLine("Grab_LNAME Call has failed at Null Check");
                return "ACCESS DENIED.";
            }
            else
            {
                return user?.LastName;
            }
        }

        [Authorize]
        [Route("Grab_EMAIL")]
        [HttpGet]
        public async Task<string> GetEMAIL()
        {
            ApplicationUser user = await GetCurrentUserByNameAsync();
            if (user == null)
            {
                Console.WriteLine("Grab_EMAIL Call has failed at Null Check");
                return "ACCESS DENIED.";
            }
            else
            {
                return user?.Email;
            }
        }

        [Authorize]
        [Route("Grab_MOBILE")]
        [HttpGet]
        public async Task<string> GetMOBILE()
        {
            ApplicationUser user = await GetCurrentUserByNameAsync();
            if (user == null)
            {
                Console.WriteLine("Grab_MOBILE Call has failed at Null Check");
                return "ACCESS DENIED.";
            }
            else
            {
                return user?.PhoneNumber;
            }
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User); //Gets current User, for some reason this doesn't work
        private Task<ApplicationUser> GetCurrentUserByNameAsync() => _userManager.FindByNameAsync(HttpContext.User.Identity.Name); //Gets current User

        [AcceptVerbs("Get")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            ApplicationUser user =  await _userManager.FindByEmailAsync(email);

            if(user == null){
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult($"Email {email} is already in use");
            }
        }

        [Route("IsEmailValidated/{ID}")]
        [HttpGet]
        public async Task<string> IsEmailValidated([FromRoute] string ID)
        {
            var user = await _userManager.FindByIdAsync(ID);

            if(user == null)
            {
                return "NOT-VERIFIED";
            }

            if (user.EmailConfirmed)
            {
                return "TRUE-VERIFIED";
            }
            else
            {
                return "NOT-VERIFIED";
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel login)
        {

            //Grab user by email
            var user = await GetCurrentUserByNameAsync();

            if (user == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "No user found." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "No user found." } });
            }

            //Confirm user email based on given token.
            var result = await _userManager.ChangePasswordAsync(user, login.OldPassword, login.ConfirmNewPassword);
            if (result.Succeeded)
            {
                IEnumerable<string> errEnum = new string[] { "Password successfully changed." };
                Console.WriteLine("Password CHANGED!");
                return Ok(new RegisterResult { Successful = true, Errors = errEnum });
            }
            else
            {
                IEnumerable<string> errEnum = new string[] { "Incorrect current password." };
                Console.WriteLine("Password NOT CHANGED!");
                return Ok(new RegisterResult { Successful = false, Errors = errEnum });
            }

        }

        [Route("ConfirmEmail/{ID}/{TOKEN}")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromRoute] string ID, [FromRoute] string TOKEN)
        {
            var decodeTOKEN = HttpUtility.UrlDecode(TOKEN);
            Console.WriteLine("Decoded TOKEN: " + decodeTOKEN);
            string fixedToken = decodeTOKEN.Replace(' ', '+');
            Console.WriteLine("FIXED TOKEN: " + fixedToken);

            if (ID == null || TOKEN == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "Couldn't conform email." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "Couldn't conform email." } });
            }

            var user = await _userManager.FindByIdAsync(ID);

            if(user == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "No user found." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "No user found." } });
            }

            //Confirm user email based on given token.
            var result = await _userManager.ConfirmEmailAsync(user, fixedToken);

            if (result.Succeeded)
            {
                return LocalRedirect("/emailVerified/" + "TRUE");

                //RegisterResult obj = new RegisterResult { Successful = true };
                //return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = true });

            }
            else
            {
                var errors = result.Errors.Select(x => x.Description);
                Console.WriteLine("ERROR REG TOKEN: " + TOKEN);
                Console.WriteLine("ERROR FIXED TOKEN: " + fixedToken);

                RegisterResult obj = new RegisterResult { Successful = false, Errors = errors };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "Unknown Email Confirmation error." } });
            }
        }

        [Route("ResendEmailValidation")]
        [HttpPost]
        public async Task<IActionResult> ResendEmailValidation([FromBody] LoginModel login)
        {
            //Grab user by email
            var user = await _userManager.FindByEmailAsync(login.Email);

            if(user == null)
            {
                //User doesn't exist.
                IEnumerable<string> errEnum = new string[] { "Invalid email, please enter a valid email." };
                return Ok(new RegisterResult { Successful = false, Errors = errEnum });
            }
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { ID = user.Id, TOKEN = token }, Request.Scheme);

                IEnumerable<string> errEnum = new string[] { "Email confirmation has been resent." };

                List<SettingsModel> settings = EmailController.EmailSettings();
                string EmailBody = Properties.Resources.Email;
                EmailBody = EmailBody.Replace("[Title]", "Your Email needs to be verified");
                EmailBody = EmailBody.Replace("[Header]", "Hi " + user.FirstName + ", Hope this email finds you well!");
                EmailBody = EmailBody.Replace("[Message]", "This Email is sent to you to let you know that you will not be able to use our system untill you verify your email.");
                EmailBody = EmailBody.Replace("[Image]", "https://i.gyazo.com/f51f8725a57053347218537954c0954b.jpg");
                EmailBody = EmailBody.Replace("[Footer]", "Please click the link below to verify you email <br/> [VerifyLink]");
                EmailBody = EmailBody.Replace("[VerifyLink]", "<a href='[Link]'>Click to Verify</a>");
                EmailBody = EmailBody.Replace("[Link]", confirmationLink);
                EmailBody = EmailBody.Replace("[ClosingRemarks]", "Cheers,");
                EmailBody = EmailBody.Replace("[End]", settings[0].Company_Name);
                var email = new EmailModel
                {
                    Subject = "Email approval required!",
                    To = user.Email,
                    Body = EmailBody,
                    HtmlBody = true
                };
                EmailController.Send(email);

                return Ok(new RegisterResult { Successful = true, Errors = errEnum });
            }
        }

        [Route("SendPasswordReset")]
        [HttpPost]
        public async Task<IActionResult> SendPasswordReset([FromBody] LoginModel login)
        {

            var tempPassword = Lotus.Client.Shared.Tools.Id_Generator.GeneratePassword();
            //var tempPassword = "TestPass123!";

            //Grab user by email
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                //User doesn't exist.
                IEnumerable<string> errEnum = new string[] { "Invalid email, please enter a valid email." };
                return Ok(new RegisterResult { Successful = false, Errors = errEnum });
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var confirmationLink = Url.Action("PasswordReset", "Accounts", new { ID = user.Id, TOKEN = token, TEMPPASSWORD = tempPassword }, Request.Scheme);

                IEnumerable<string> errEnum = new string[] { "Password reset email has been sent." };

                List<SettingsModel> settings = EmailController.EmailSettings();
                string EmailBody = Properties.Resources.Email;
                EmailBody = EmailBody.Replace("[Title]", "Your Password has been reset");
                EmailBody = EmailBody.Replace("[Header]", "Hi " + user.FirstName + ", Hope this email finds you well!");
                EmailBody = EmailBody.Replace("[Message]", "This Email is sent to you to let you know that your password has been reset.");
                EmailBody = EmailBody.Replace("[Image]", "https://i.gyazo.com/7bf8bee1aaa147d794dae385be5c7db8.jpg");
                EmailBody = EmailBody.Replace("[Footer]", "Please click the link below to reset your password <br/> [VerifyLink]");
                EmailBody = EmailBody.Replace("[VerifyLink]", "<a href='[Link]'>Click to Verify</a>");
                EmailBody = EmailBody.Replace("[Link]", confirmationLink);
                EmailBody = EmailBody.Replace("[ClosingRemarks]", "Cheers,");
                EmailBody = EmailBody.Replace("[End]", settings[0].Company_Name);
                var email = new EmailModel
                {
                    Subject = "Password has been reset",
                    To = user.Email,
                    Body = EmailBody,
                    HtmlBody = true
                };
                EmailController.Send(email);

                return Ok(new RegisterResult { Successful = true, Errors = errEnum });
            }
        }

        [Route("PasswordReset/{ID}/{TOKEN}/{TEMPPASSWORD}")]
        [HttpGet]
        public async Task<IActionResult> PasswordReset([FromRoute] string ID, [FromRoute] string TOKEN, [FromRoute] string TEMPPASSWORD)
        {
            var decodeTOKEN = HttpUtility.UrlDecode(TOKEN);
            Console.WriteLine("Decoded TOKEN: " + decodeTOKEN);
            string fixedToken = decodeTOKEN.Replace(' ', '+');
            Console.WriteLine("FIXED TOKEN: " + fixedToken);

            if (ID == null || TOKEN == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "Couldn't conform email." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "Couldn't conform email." } });
            }

            var user = await _userManager.FindByIdAsync(ID);

            if (user == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "No user found." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "No user found." } });
            }

            //Reset user password to testPassword
            var result = await _userManager.ResetPasswordAsync(user, fixedToken, TEMPPASSWORD);

            if (result.Succeeded)
            {
                Console.WriteLine("Password has been reset.");
                return LocalRedirect("/PasswordReset/" + TEMPPASSWORD);

                //RegisterResult obj = new RegisterResult { Successful = true };
                //return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = true });

            }
            else
            {
                Console.WriteLine("Password WAS NOT reset.");

                var errors = result.Errors.Select(x => x.Description);
                Console.WriteLine("ERROR REG TOKEN: " + TOKEN);
                Console.WriteLine("ERROR FIXED TOKEN: " + fixedToken);

                RegisterResult obj = new RegisterResult { Successful = false, Errors = errors };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "Unknown Email Confirmation error." } });
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterModel model)
        {
            var newUser = new ApplicationUser 
            { 
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.MobileNumber,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            //Add all new users to the new default User role
            await _userManager.AddToRoleAsync(newUser, "User");

            // Add new users whose email starts with 'admin' to the Admin role
            // Obviously we don't actually want this, just an example to show you two
            if (newUser.Email.StartsWith("admin"))
            {
                //await _userManager.AddToRoleAsync(newUser, "Admin");
            }

            ApplicationUser lastUserAdded = await _userManager.FindByNameAsync(newUser.UserName);
            lastUserID = lastUserAdded?.Id;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(lastUserAdded);
            var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new {ID = lastUserAdded.Id, TOKEN = token}, Request.Scheme);

            string htmlTOKEN = HttpUtility.UrlEncode(token);
            Console.WriteLine("ConfirmationLink:  " + confirmationLink);
            Console.WriteLine("User ID: " + lastUserAdded.Id);
            Console.WriteLine("EMAIL HTML TOKEN FOR USER: " + lastUserAdded.UserName + " : " + htmlTOKEN);
            Console.WriteLine("EMAIL NORM TOKEN FOR USER: " + lastUserAdded.UserName + " : " + token);

            IEnumerable<string> errEnum = new string[] { "Email confirmation is required before login." };

            List<SettingsModel> settings = EmailController.EmailSettings();
            string EmailBody = Properties.Resources.Email;
            EmailBody = EmailBody.Replace("[Title]", "Your Email needs to be verified");
            EmailBody = EmailBody.Replace("[Header]", "Hi " + lastUserAdded.FirstName + ", Hope this email finds you well!");
            EmailBody = EmailBody.Replace("[Message]", "This Email is sent to you to let you know that you will not be able to use our system untill you verify your email.");
            EmailBody = EmailBody.Replace("[Image]", "https://i.gyazo.com/f51f8725a57053347218537954c0954b.jpg");
            EmailBody = EmailBody.Replace("[Footer]", "Please click the link below to verify you email <br/> [VerifyLink]");
            EmailBody = EmailBody.Replace("[VerifyLink]", "<a href='[Link]'>Click to Verify</a>");
            EmailBody = EmailBody.Replace("[Link]", confirmationLink);
            EmailBody = EmailBody.Replace("[ClosingRemarks]", "Cheers,");
            EmailBody = EmailBody.Replace("[End]", settings[0].Company_Name);
            var email = new EmailModel
            {
                Subject = "Email approval required!",
                To = lastUserAdded.Email,
                Body = EmailBody,
                HtmlBody = true
            };
            EmailController.Send(email);

            //Test to see if email can be confirmed upon registration
           // var result2 = await _userManager.ConfirmEmailAsync(lastUserAdded, token);
           // if (result2.Succeeded)
           // {
            //    Console.WriteLine("New User email validated.");
            //}
            //else
            //{
            //    Console.WriteLine("FAILED EMAIL VALIDATION OF NEW USER");
            //}

            return Ok(new RegisterResult { Successful = true, Errors = errEnum });
        }


        [Route("Staff")]
        public async Task<IActionResult> PostStaff([FromBody]RegisterModel model)
        {
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.MobileNumber,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            //Add all new users to the new default User role
            await _userManager.AddToRoleAsync(newUser, "Staff");

            // Add new users whose email starts with 'admin' to the Admin role
            // Obviously we don't actually want this, just an example to show you two
            if (newUser.Email.StartsWith("admin"))
            {
                //await _userManager.AddToRoleAsync(newUser, "Admin");
            }

            ApplicationUser lastUserAdded = await _userManager.FindByNameAsync(newUser.UserName);
            lastUserID = lastUserAdded?.Id;
            return Ok(new RegisterResult { Successful = true });
        }

        [Route("StaffToUser")]
        public async Task<IActionResult> StaffToUser([FromBody] string ID)
        {
            //Grab user by ID
            var user = await _userManager.FindByIdAsync(ID);

            if (user == null)
            {
                RegisterResult obj = new RegisterResult { Successful = false, Errors = new string[] { "No user found." } };
                return new JsonResult(obj);
                //return Ok(new RegisterResult { Successful = false, Errors = new string[] { "No user found." } });
            }

            //Add user to User role.
            await _userManager.AddToRoleAsync(user, "User");
            await _userManager.RemoveFromRoleAsync(user, "Staff");

            return Ok(new RegisterResult { Successful = true });
        }
    }
}