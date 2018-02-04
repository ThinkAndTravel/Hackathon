using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.User.ViewModel;
using BLL.userLogic.extension;
using MongoManager.CollectionModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TandT.API.Controllers.Identity
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        // GET: api/values
        [Route("tasks")]
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

        [Route("GetTaskById")]
        [HttpGet("{id}")]
        public async Task<ViewTask> GetTaskById(string id)
        {
            return await TaskLogic.GetTaskById(id);
        }

        [Route("gettfc")]
        [HttpGet("{id,City}")]
        public List<ViewTask> GetTaskForCity(string id, string City)
        {
            return TaskLogic.GetTaskForCity(id,City);
        }

        [Route("gettfcl")]
        [HttpGet("{id,loclat,loclong}")]
        public List<ViewTask> GetTaskForCurLoc(string id, double loclat, double loclong)
        {
            Location loc = new Location();
            loc.GeoLat = loclat;
            loc.GeoLong = loclong;
            return TaskLogic.GetTaskForCurLoc(id, loc, 2);
        }

        [Route("gettaska")]
        [HttpGet("{UserId,k}")]
        public List<ViewTask> GetTaskA(string UserId, int k)
        {
            return TaskLogic.GetNextTenActiveTask(UserId,k);
        }
        [Route("check")]
        [HttpGet("{id,taskId,City}")]
        public int Check(string id, string taskId, string City)
        {
            return TaskLogic.CheckTask(id, taskId, City);
        }

        public class TaskModel
        {
            public string UserId { get; set; }
            public string TaskId { get; set; }
            public int rank { get; set; }
        }
        [Route("addrank")]
        [HttpPost()]
        public IActionResult AddRank([FromBody] TaskModel task)
        {
            try
            {
                TaskLogic.AddRanks(task.UserId, task.TaskId, task.rank);
                return this.Ok(new { success = true, message = "OK" });
            }
            catch
            {
                return this.BadRequest(new { success = false, message = "Sorry, Let's try again" });
            }
        }
        [Route("addtaska")]
        [HttpPost()]
        public IActionResult AddTaskA([FromBody] TaskModel task)
        {
            try
            {
                TaskLogic.AddTaskToActive(task.UserId, task.TaskId);
                return this.Ok(new { success = true, message = "OK" });
            }
            catch
            {
                return this.BadRequest(new { success = false, message = "Sorry, Let's try again" });
            }
        }

        [Route("gettaskinfo")]
        [HttpGet("{taskid}")]
        public ViewTask GetTaskInfo( string taskId)
        {
            return null;// TaskLogic.CheckTask(id, taskId, City);
        }


    }
}
