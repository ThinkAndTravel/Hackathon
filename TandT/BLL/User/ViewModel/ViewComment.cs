using System;
using System.Collections.Generic;
using System.Text;
using MongoManager.CollectionModels;


namespace BLL.User.ViewModel
{
    public class ViewComment
    {
        public string UserId;
        public string UserName;
        public string UserAvatar;
        public DateTime DateCreated;
        public int CountLike;
        public string Value;
    }
}
