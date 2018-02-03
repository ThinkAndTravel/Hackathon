using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamvvm;

namespace AppTandT.Pages.Menu
{
    public class MainMasterDetailPageModel : BasePageModel
    {
        public void SetDetail<TPageModel>(IBasePage<TPageModel> page)
            where TPageModel : class, IBasePageModel
        {
            var masterDetailPage = this.GetCurrentPage() as MasterDetailPage;
            masterDetailPage.Detail = new NavigationPage(page as Page);
            masterDetailPage.IsPresented = false;
        }
    }
}

