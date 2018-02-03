using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TandT.API.Controllers.Identity
{
    [Route("api/[controller]")]
    public class RegistryController : Controller
    {
              
        [HttpPost]
        public IActionResult Post([FromBody] RegistryModel value)
        {
            if (string.IsNullOrEmpty(value.Login))
            {
                return this.BadRequest(new {success = false , message = "Login field can not be empty" });
            }

            if (string.IsNullOrEmpty(value.Email))
            {
                return this.BadRequest(new {success = false , message = "Email field can not be empty."});
            }

            if (string.IsNullOrEmpty(value.Pas) || string.IsNullOrEmpty(value.ConfirmPas))
            {
                return this.BadRequest(new {success = false , message = "Password field can not be empty."});
            }

            if (!value.Pas.Equals(value.ConfirmPas))
            {
                return this.BadRequest(new {success = false , message = "You have entered two different passwords."});
            }

            if (string.IsNullOrEmpty(value.FirstName))
            {
                return this.BadRequest(new {success = false , message = "First name field can not be empty."});
            }

            if (string.IsNullOrEmpty(value.LastName))
            {
                return this.BadRequest(new {success = false , message = "Last name field can not be empty."});
            }

            if (!BLL.Identity.IsUserUnique(value))
            {
                return this.BadRequest(new {success = false , message = "Such user already exists."});
            }

            bool result = BLL.Identity.Registry(value);
            if (!result)
            {
                return this.BadRequest(new {success = false , message = "An error occurred. Please, try again later."});
            }

            return this.Ok(new {success = true , message = "OK"});
        }
    }
}
