using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;

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
            
           var item = BindingContext as Plan;
            if (item == null)
                return;
            TaskView.Source = item.PlanTask;
            About.Text = item.About;
            Time.Text = item.DateStart.ToString() + " - " + item.DateFinish.ToString();
            WeatherView.Source = item.Weather;

        }
    }
}