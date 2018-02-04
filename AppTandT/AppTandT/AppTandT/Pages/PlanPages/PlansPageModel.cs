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
using AppTandT.BLL.Help;
using Acr.UserDialogs;

namespace AppTandT.Pages.PlanPages
{
    public class PlansPageModel : BasePageModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddPlanButtonCommand { get; set; }

        public ObservableCollection<Plan>  PlanItems { get; set; }
        public PlansPageModel()
        {
            List<Plan> list = new List<Plan>();
            list = ListPlan.GET();
            if (list == null)
            {

                var conf = new AlertConfig
                {
                    Message = "You don't have any plans"
                };
                UserDialogs.Instance.AlertAsync(conf);
                list = new List<Plan>();
            }
            PlanItems = new ObservableCollection<Plan>(list);

            AddPlanButtonCommand = new BaseCommand<SelectedItemChangedEventArgs>((arg) =>
            {
                var pageModel = new AddPlanPageModel();
                var page = this.GetPageFromCache<AddPlanPageModel>(pageModel);
                var masterDetailPage =
                    this.GetPageFromCache<MainMasterDetailPageModel>();

                masterDetailPage.GetPageModel().SetDetail(page);
            });
        }
    }
   
}

