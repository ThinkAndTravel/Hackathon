using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTandT.Pages.Menu
{
	
        [XamlCompilation(XamlCompilationOptions.Compile)]
        public partial class MenuListCell : ViewCell
        {
            public MenuListCell()
            {
                InitializeComponent();
            }

            protected override void OnBindingContextChanged()
            {
                base.OnBindingContextChanged();

                var item = BindingContext as MenuPageMasterModel.MenuItem;
                if (item == null)
                    return;

                Text.Text = item.Title;
                Icon.Source = "https://scontent-waw1-1.xx.fbcdn.net/v/t1.0-9/21761765_180870455791802_8497190410053769062_n.jpg?oh=dddc78e9d198285ba10b7a83b127a165&oe=5A9923E1";
                if (item.Icon.Length > 0)
                    Icon.Source = item.Icon;

                View.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = item.Command
                });

                if (item.Details.Length > 0)
                {
                    Detail.Text = item.Details;
                }

                rowGrid.BackgroundColor = item.BackColor;

            }
        }
    
}