using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;

namespace AppTandT.Pages.TaskPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskExecutionPage : ContentPage, IBasePage<TaskExecutionPageModel>
    {
        public TaskExecutionPage()
        {
            InitializeComponent();
            takePhoto.Text = "Take a photo!";
            takePhoto.Clicked += TakePhoto_Clicked;
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await BLL.BlobManager.performBlobOperation("test.jpg", "yyy");
        }
    }
}