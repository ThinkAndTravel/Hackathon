using AppTandT.Pages.TaskPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamvvm;

namespace AppTandT.Pages.Menu
{
    public class MainMasterDetailPage : MasterDetailPage, IBasePage<MainMasterDetailPageModel>
    {
        public MainMasterDetailPage()
        {
            Master = this.GetPageFromCache<MenuPageMasterModel>() as Page;
            Detail = new NavigationPage(this.GetPageFromCache<CityTasksPageModel>() as Page);
        }
    }
}
