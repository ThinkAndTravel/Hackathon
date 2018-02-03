using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels.HelpCollectionModels
{
    public class Comment
    {
        public string Value { get; set; }

        public DateTime? DateComent = null;//{ get; set; }

        /// <summary>
        /// _id автора коментара
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// _id юзерів, які лайкнули коментар
        /// </summary>
        public List<string> Likes { get; set; }

    }
}
