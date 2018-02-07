using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        private Stream stream = null;

        public TaskExecutionPage()
        {
            InitializeComponent();
            takePhoto.Text = "Take a photo!";
            takePhoto.Clicked += TakePhoto_Clicked;
            sendPhoto.Clicked += SendPhoto_Clicked;
           

         //   var pa = this.GetPageModel();
         //   Title.Text = pa.tvm.Title;
        }

        private async void SendPhoto_Clicked(object sender, EventArgs e)
        {
            if(stream != null)
            {
                await BLL.BlobManager.performBlobOperation(stream);
            }
        }

        protected override async void OnAppearing()
        {
            var pa = this.GetPageModel();
            var task = await BLL.Services.TaskService.GetTaskByIdAsync(pa.taskId);
            Title.Text = task.Title;
            LogoView.Source = @"https://i.imgur.com/in9OK8m.png";
            About.Text = task.About;
            var loc = await BLL.Geolocator.GetPositionAsync();
            sendPhoto.IsVisible = false;
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new Exception("No Camera:(");
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Directory = "TandT",
                Name = "hfghfhgf" + DateTime.Now.Ticks,
                PhotoSize = PhotoSize.Medium,
            });

            TakenPhoto.Source = file.AlbumPath;
            sendPhoto.IsVisible = true;

            stream = file.GetStream();
            //   await BLL.BlobManager.performBlobOperation("test.jpg", "yyy");
        }
    }
}