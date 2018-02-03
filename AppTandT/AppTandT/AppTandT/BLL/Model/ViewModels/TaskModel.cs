using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.ViewModels
{
    public class TaskViewModel
    {
        public string _id;
        public string About { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public List<string> PicturesAbout { get; set; }
        public int[] Ranks = new int[6];
        public bool isValid() { return true; }
    }
    public class TaskServiceModel
    {
        string UserId;
        string TaskId;
        int rank;
    }
}
