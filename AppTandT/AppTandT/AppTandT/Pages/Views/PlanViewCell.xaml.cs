using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTandT.Pages.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlanViewCell : ViewCell
	{
		public PlanViewCell ()
		{
			InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            /*
           var item = BindingContext as PlanItems;
            if (item == null)
                return;
            TaskView.Source = item.Url;
            About = item.About;
            Time = item.DateStart + item.DateFinish;
            WeatherView.Source = item.Weather;*/

        }
    }
}