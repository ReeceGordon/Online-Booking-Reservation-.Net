using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lotus.Shared;
using Microsoft.AspNetCore.Hosting;

namespace Lotus.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        [HttpPost]
        //gets the image form context and stores it in the appropriate location
        //returns a response if the method is completed
        public ResponseModel Post()
        {
            
            var path = "";
            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    if(file.FileName.Contains("Image"))
                    {
                        path = Path.Combine(environment.ContentRootPath, "uploads/Carousel", file.FileName.Split(".")[0] + ".jpg");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    else if(file.FileName.Contains("Logo"))
                    {
                        
                        path = Path.Combine(environment.ContentRootPath, "uploads", "Logo.png");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            
                            file.CopyTo(stream);
                        }
                        
                        path = Path.Combine(environment.ContentRootPath, "uploads/Staff", "Logo.png");
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                             file.CopyTo(stream);
                        }
                    }

                }
            }
            ResponseModel response = new ResponseModel
            {
                Status = true
            };
            return response;


        }

        [Route("Staff")]
        //gets the image form context and stores it in the appropriate location
        //returns a response if the method is completed
        public ResponseModel PostStaff()
        {
            var path = "";
            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                   
                        path = Path.Combine(environment.ContentRootPath, "uploads/Staff", file.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                 

                }
            }
            ResponseModel response = new ResponseModel
            {
                Status = false
            };
            return response;


        }

        [HttpDelete]
        [Route("{Name}")]
        //deletes a staff member by their id
        public void Delete([FromRoute] string Name)
        {
            if(Name != "Logo.png")
            {
                try
                {
                    System.IO.File.Delete(environment.ContentRootPath + "/uploads/Staff/" + Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            
        }
    }
}