using System;
using System.Collections.Generic;
using System.Text;
using MongoManager.CollectionModels;

namespace BLL
{
    public class NoticeView
    {
        public string id;
        public string Name;
        public string Avatar;
        public DateTime DateCreated;
        public Plan plan;
    }
}
