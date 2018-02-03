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
    public partial class TaskViewCell : ViewCell
    {
        public TaskViewCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var item = BindingContext as TaskItem;
            if (item == null)
                return;
            LogoView.Source = item.LogoUrl;
            Title.Text = item.Title;
            About.Text = item.About;

        }
    }
}