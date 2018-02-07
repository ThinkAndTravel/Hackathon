using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using AppTandT.BLL.Model.CollectionModels.MainCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels
{
    public class Task
    {
        #region VAR

        public string _id { get; set; }

        public MainTask Main { get; set; } = new MainTask();

        public string Country { get; set; }

        public string City { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public bool? IsPersonal = null;

        #endregion


    }
}
