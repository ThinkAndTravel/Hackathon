using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels
{
    public class Task
    {
        #region VAR

        public MainTask Main { get; set; } = new MainTask();

        public string Country { get; set; }

        public string City { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public bool? IsPersonal = null;

        #endregion


    }
}
