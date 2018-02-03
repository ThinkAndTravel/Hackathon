using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class UserPage
    {
        string _id { get; set; }

        string UserFullName { get; set; }

        string UserAvatar { get; set; }

        List<Post> Posts { get; set; } = new List<Post>();

        List<UserFriend> UserFriends { get; set; } = new List<UserFriend>();

        public UserPage(string id)
        {
            _id = id;
        }

    }
}
