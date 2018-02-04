using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;

namespace AppTandT.BLL.Model.CollectionModels
{
    public class Photo
    {
        public string _id { get; set; }

        public string URL { get; set; }

        public List<string> Likes { get; set; } = new List<string>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
