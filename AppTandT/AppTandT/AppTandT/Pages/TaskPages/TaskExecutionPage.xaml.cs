using Acr.UserDialogs;
using AppTandT.BLL.Help;
using AppTandT.Pages.Menu;
using AppTandT.Pages.UserPages;
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
                var pa = this.GetPageModel();
                double x, y;
                if (pa.taskId == "t1:777")
                {
                    x = 50.43;
                    y = 30.51;
                }
                else
                {
                    x = 50.38;
                    y = 30.48;
                }
                var loc = await BLL.Geolocator.GetPositionAsync();

                if (Math.Abs(loc.Latitude - x)<0.01 && Math.Abs(loc.Longitude - y) < 0.01)
                {

                    await BLL.BlobManager.performBlobOperation(stream);
                    try
                    {
                        await UserDialogs.Instance.AlertAsync("Task compleated. Post added!");
                        var page = this.GetPageFromCache<NewsPageModel>();
                        var masterDetailPage =
                            this.GetPageFromCache<MainMasterDetailPageModel>();
                        masterDetailPage.GetPageModel().SetDetail(page);
                    }
                    catch
                    {
                        await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
                    }
                } else
                {
                    try
                    {
                        await UserDialogs.Instance.AlertAsync("Task don't compleate");
                    }
                    catch
                    {
                        await UserDialogs.Instance.AlertAsync("An error has occurred. Try again later.");
                    }
                }
            }
        }

        protected override async void OnAppearing()
        {
            var pa = this.GetPageModel();
            var task = await BLL.Services.TaskService.GetTaskByIdAsync(pa.taskId);
            Title.Text = task.Title;
            LogoView.Source = @"https://i.imgur.com/in9OK8m.png";
            About.Text = task.About;
           // sendPhoto.IsVisible = false;
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

            if (file == null) return;

            TakenPhoto.Source = file.AlbumPath;
         //   sendPhoto.IsVisible = true;

            stream = file.GetStream();
            //await BLL.BlobManager.performBlobOperation(stream);
        }
    }
}