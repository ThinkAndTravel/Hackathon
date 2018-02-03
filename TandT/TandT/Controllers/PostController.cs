using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoManager;
using MongoManager.CollectionModels;
using BLL.User.ViewModel;
using BLL.userLogic.extension;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TandT.API.Controllers.Identity
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        // GET: api/values
        [Route("posts")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getposts")]
        [HttpGet("{id,k}")]
        public List<ViewPost> GetPost(string id, int k)
        {
            return PostLogic.GetNextTenPost(id, k);
        }
        // POST api/values

        [Route("addpost")]
        [HttpPost]
        public IActionResult AddPost([FromBody] Post value)
        {
            if (value.Main.Photos.Count == 0)
            {
                return this.BadRequest(new { success = false, message = "Post has no photos" });
            }
            if (value.Main.CurTask == null)
            {
                return this.BadRequest(new { success = false, message = "Post has no task" });
            }
            PostLogic.AddPost(value._id, value);
            return this.Ok(new { success = true, message = "OK" });
        }

        [Route("likepost")]
        [HttpPost]
        public IActionResult ClickLikePost([FromBody] string id, string UserId)
        {
            try
            {
                PostLogic.ClickLikePost(UserId, id);
                return this.Ok(new { success = true, message = "OK" });
            }
            catch
            {
                return this.BadRequest(new {success = false, message = "Sorry, Let's try again"});
            }
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
