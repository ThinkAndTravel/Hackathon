using AppTandT.BLL.Model.ViewModels;
using Xamvvm;

namespace AppTandT.Pages.TaskPages
{
    internal class TaskExecutionPageModel :  BasePageModel
    {
        public TaskViewModel tvm = null;

        public TaskExecutionPageModel(string TaskId = "t1:123")
        {
            TaskId = TaskId ?? "t1:123";
            tvm = BLL.Services.TaskService.GetTaskById(TaskId).Result;
        }
    }
}