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
using Microsoft.EntityFrameworkCore;

namespace Lotus.Server.Controllers.Identity
{


    //VERY IMPORTANT TO USE THESE AUTHORIZATION TAGS IN OUR SERVER SIDE CONTROLLERS
    //THIS IS BECAUSE BLAZER CLIENT SIDE CODE CAN BE MODIFIED AND INJECTED WITH MALICIOUS CODE
    //SO A HACKER COULD POTENTIALLY PRETEND THEY'RE IN ROLE 'ADMIN' TO BYPASS CLIENT AUTHORIZATION
    //BY ALSO INCLUDING A CHECK HERE ON THE SERVER CONTROLLER, THEN THE CLIENT PAGE WOULD LOSE ALL 
    //FUNCTIONALITY EVEN IF A USER MODIFIED CLIENT CODE TO PRETEND TO BE IN 'x' ROLE!!!
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleModel model)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };


            var result = await roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterRoleResult { Successful = false, Errors = errors });

            }

            return Ok(new RegisterRoleResult { Successful = true });

        }

        [Route("ListRoles")]
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var roles = roleManager.Roles.ToList();

            List<RoleListModel> listRoles = new List<RoleListModel>();

            foreach(var role in roles)
            {
                listRoles.Add(new RoleListModel { id = role.Id, name = role.Name });
            }

            return new JsonResult(roles);
        }

        [HttpGet]
        [Route("EditRole/{ID}")]
        public async Task<IActionResult> EditRole([FromRoute] string ID)
        {
            var role = await roleManager.FindByIdAsync(ID);


            if (role == null)
            {
                Console.WriteLine("NO ROLE FOUND");
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO USER FOUND" });
            }

            var model = new EditRoleModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach(var user in await userManager.GetUsersInRoleAsync(role.Name))
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    //model.Users.Add(user.UserName);
                    model.Users.Add(
                        new UserRoleModel
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            IsSelected = true
                        }
                        );
                }
            }


            return new JsonResult(model);
            // return Ok(model);;
            //return Ok(new EditRoleResult { Successful = true });
        }

        [HttpPost]
        [Route("PostRole")]
        public async Task<IActionResult> PostRole(EditRoleModel newRole)
        {
            var role = await roleManager.FindByIdAsync(newRole.Id);

            if (role == null)
            {
                Console.WriteLine("NO USER FOUND");
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO USER FOUND" });
            }
            else
            {
                role.Name = newRole.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return new JsonResult($"Successsfully Updated Role: " + role.Name);
                }
                else
                {
                    var errors = result.Errors.Select(x => x.Description);

                    return Ok(new RegisterRoleResult { Successful = false, Errors = errors });

                }

                //return new JsonResult($"Failed Updating Role: " + role.Name);
            }
        }

        [HttpDelete]
        [Route("DelRole/{ID}")]
        public async Task<IActionResult> DelRole([FromRoute] string ID)
        {
            var role = await roleManager.FindByIdAsync(ID);
            if (role == null)
            {
                Console.WriteLine("NO ROLE FOUND");
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO ROLE FOUND" });
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return new JsonResult($"Successsfully Deleted Role: " + role.Name);
                }
                else
                {
                    var errors = result.Errors.Select(x => x.Description);
                    return Ok(new RegisterRoleResult { Successful = false, Errors = errors });
                }
            }
        }

        [HttpGet]
        [Route("GetUsersInRole/{ID}")]
        public async Task<IActionResult> GetUsersInRole([FromRoute] string ID)
        {
            var role = await roleManager.FindByIdAsync(ID);
            if (role == null)
            {
                Console.WriteLine("NO ROLE FOUND");
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO ROLE FOUND" });
            }

            var model = new List<UserRoleModel>();

            foreach(var user in await userManager.Users.ToListAsync()) //For each user in system
            {
                var UserRoleModel = new UserRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await userManager.IsInRoleAsync(user, role.Name)) //Check if user is already member of role
                {
                    UserRoleModel.IsSelected = true;
                }
                else
                {
                    UserRoleModel.IsSelected = false;
                }

                model.Add(UserRoleModel);
            }

            return new JsonResult(model);
        }

        [HttpPost]
        [Route("EditUsersInRole/{ID}")]
        public async Task<IActionResult> EditUsersInRole([FromRoute] string ID, List<UserRoleModel> model)
        {
            var role = await roleManager.FindByIdAsync(ID);
            string resultTest = "DEFAULT";
            bool initResult = false;
            if (role == null)
            {
                Console.WriteLine("NO ROLE FOUND");
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO ROLE FOUND" });
            }

            for(int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name))) //If isSelected is true and user isn't already in role
                {
                    result = await userManager.AddToRoleAsync(user, role.Name); //Add user to role
                    resultTest = result.Succeeded.ToString();
                    initResult = result.Succeeded;
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name))) //if isSelected is false and user is already in role
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name); //Remove user from role
                    resultTest = result.Succeeded.ToString();
                    initResult = result.Succeeded;
                }
                else
                {
                    continue; 
                    //If isSelected is true and user is in role, do nothing
                    //If isSelected is false and user is not in role, do nothing
                }

                Console.WriteLine("iteration i: " + i);
                Console.WriteLine("Current model.Count -1: " + (model.Count - 1));

                if (result.Succeeded)
                {
                    Console.WriteLine("result.Succeeded Value: " + result.Succeeded.ToString());
                    if(i < (model.Count - 1)){
                        Console.WriteLine("Continue Called");
                        continue; //There are more objects in model that need to be processed
                    }
                    else
                    {
                        Console.WriteLine("SUCCESS ADDED ROLE");
                        return new JsonResult($"Successsfully Updated Users in Role: " + role.Name);
                    }
                }
                else
                {
                    Console.WriteLine("result.Succeeded FAILED Value: " + result.Succeeded.ToString());
                }
            }

            Console.WriteLine("NO LIST FOUND, Succeeded: " + resultTest);
            Console.WriteLine("Model Size: " + model.Count());

            if (initResult)
            {
                return Ok(new EditRoleResult { Successful = true, Error = "Success." });
            }
            else
            {
                return BadRequest(new EditRoleResult { Successful = false, Error = "NO LIST FOUND" });
            }
        }


    }
}