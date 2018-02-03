using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.MainCollectionModels
{
    public class MainPost
    {
        #region VAR
        /// <summary>
        /// Id автора посту
        /// </summary>
        public string UserId { get; set; }

        public DateTime? DatePost = null;

        /// <summary>
        /// id photo
        /// </summary>
        public List<string> Photos { get; set; } = new List<string>();

        public Location Location = new Location();

        public Task CurTask { get; set; } = new Task();
        #endregion

    }
}
