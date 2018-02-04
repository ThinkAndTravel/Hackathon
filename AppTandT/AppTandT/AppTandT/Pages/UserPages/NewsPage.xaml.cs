using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamvvm;

namespace AppTandT.Pages.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage, IBasePage<NewsPageModel>
    {
        public NewsPage()
        {
            InitializeComponent();
        }
    }
}