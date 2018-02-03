using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using AppTandT.BLL.Model.CollectionModels.MainCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels
{
    public class Tour
    {
        #region VAR

        public MainTour Main { get; set; } = new MainTour();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<string> Tourists { get; set; } = new List<string>();

        public List<Condition> Conditions { get; set; } = new List<Condition>();

        #endregion      
    }
}
