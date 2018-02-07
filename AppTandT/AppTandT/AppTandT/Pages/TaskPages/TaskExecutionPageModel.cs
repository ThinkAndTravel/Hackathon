using AppTandT.BLL.Model.ViewModels;
using Xamvvm;

namespace AppTandT.Pages.TaskPages
{
    internal class TaskExecutionPageModel :  BasePageModel
    {
        public string taskId { get; set; } = null;

        public TaskExecutionPageModel(string TaskId = "t1:123")
        {
            this.taskId = TaskId ?? "t1:123";
        }
    }
}