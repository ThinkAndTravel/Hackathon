using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.MainCollectionModels
{
    public class MainTask
    {
        #region VAR

        public int? TypeTask = null;

        public Location Location { get; set; } = new Location();
        /// <summary>
        /// Список запитань для тестів
        /// </summary>

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<string> Tags { get; set; }

        public string About { get; set; }


        public string Title { get; set; }
        #endregion

    }
}
