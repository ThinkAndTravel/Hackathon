using AppTandT.BLL.Model.ViewModels;
using Xamvvm;

namespace AppTandT.Pages.TaskPages
{
    internal class TaskExecutionPageModel :  BasePageModel
    {
        public string taskId { get; set; } = null;

        public TaskExecutionPageModel()
        {
        }

        public void SetTaskId(string taskId = "t1:123")
        {
            this.taskId = taskId ?? "t1:123";
        }
    }
}