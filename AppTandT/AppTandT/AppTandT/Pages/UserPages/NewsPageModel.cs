using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;
using AppTandT.BLL.Model.ViewModels;
using AppTandT.BLL;
using System.Net.Http;

namespace AppTandT.Pages.UserPages
{
    public class NewsPageModel : BasePageModel
    {
        //   public ObservableCollection<PostItem> Items { get; set; }

        public  NewsPageModel()
        {
            PostSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
            {
                SelectedPost = null;
            });
            Reload().GetAwaiter().GetResult();
        }
        public PostItem SelectedPost { get; set; }
        public ICommand PostSelectedCommand { get; set; }

        public ObservableCollection<PostItem> Items { get; set; }

        public async Task Reload()
        {
            if (Items == null)
                Items = new ObservableCollection<PostItem>();

            //HttpClient client = new HttpClient();
            //client.Timeout = System.TimeSpan.FromMilliseconds(11111);
            
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            //var r = client.GetAsync(new Uri(@"http://tandt20180203091155.azurewebsites.net/api/post/getnews/")).Result;

            var list = BLL.Services.PostService.GetNewsAsync().Result;

            foreach (var cur in list)
                Items.Add(new PostItem (cur));
        }


    }


}