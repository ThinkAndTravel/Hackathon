using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels
{
    public class Photo
    {
        public string CloudID { get; set; }

        public List<string> Likes { get; set; } = new List<string>();

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
