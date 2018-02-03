using MongoManager.CollectionModels;
using MongoManager.CollectionModels.HelpCollectionModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.User.ViewModel
{
    public class ViewTask
    {
        public string _id;
        public string Logo;
        public List<string> PicturesAbout { get; set; }
        public int[] Ranks = new int[6];
        public string About { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<string> Tags { get; set; }
        public string Title { get; set; }
    }
}
