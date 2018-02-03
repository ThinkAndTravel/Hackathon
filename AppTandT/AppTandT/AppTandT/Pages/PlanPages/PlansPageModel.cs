using AppTandT.BLL.Services;
using AppTandT.Pages.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamvvm;
using AppTandT.BLL;
using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;

namespace AppTandT.Pages.PlanPages
{
    public class PlansPageModel : BasePageModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Plan>  PlanItems { get; set; }
        public PlansPageModel()
        {
            List<Plan> list = ListPlan.GET();
            PlanItems = new ObservableCollection<Plan>(list);
        }
    }
   
}

