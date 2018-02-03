using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTandT.Pages.TaskPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityTasksPage : ContentPage, IBasePage<CityTasksPageModel>
    {
        public CityTasksPage()
        {
            InitializeComponent();
        }
    }
}