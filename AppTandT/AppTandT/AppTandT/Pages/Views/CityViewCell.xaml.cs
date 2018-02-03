using AppTandT.Pages.TaskPages;
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
    public partial class CityViewCell : ViewCell
    {
        public CityViewCell()
        {
            InitializeComponent();

            /*View.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new BaseCommand((arg) =>
                {
                    var item = BindingContext as CityItem;



                    //тут потрібно змінювати місто
                    System.Diagnostics.Debug.WriteLine("CITY TAPPED");
                })
            });*/
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var item = BindingContext as CityItem;
            if (item == null)
                return;
            LogoView.Source = item.LogoUrl;
            NameView.Text = item.Name;

        }
    }
}