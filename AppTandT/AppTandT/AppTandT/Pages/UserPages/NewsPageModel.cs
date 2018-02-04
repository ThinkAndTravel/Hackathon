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

namespace AppTandT.Pages.UserPages
{
    public class NewsPageModel : BasePageModel
    {
        //   public ObservableCollection<PostItem> Items { get; set; }

        public NewsPageModel()
        {
            PostSelectedCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
            {
                SelectedPost = null;
            });
            Reload();
        }
        public PostItem SelectedPost { get; set; }
        public ICommand PostSelectedCommand { get; set; }

        public ObservableCollection<PostItem> Items { get; set; }

        public async Task Reload()
        {
            if (Items == null)
                Items = new ObservableCollection<PostItem>();

                var list = await BLL.Services.PostService.GetPostsPageAsync(Sesion._id);

                foreach (var cur in list)
                 Items.Add(new PostItem (cur));
        }
    }
}