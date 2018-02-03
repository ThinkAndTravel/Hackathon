using System;
using System.Collections.Generic;
using System.Text;
using MongoManager.CollectionModels;

namespace BLL.User.ViewModel
{
    public class ViewPost
    {
        public List<string> URLPhoto;
        public string UserId;
        public string UserAvatar;
        public string About;
        public DateTime DateCreated;
        public string id;
        public int CountLikes;
        public bool Comment;
    }
}
