using MongoManager.CollectionModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class UserLogic
    {
        public static UserPage UserPage(string id)
        {
            var UP = new UserPage(id);
            return UP;
        }

        public static List<Post> GetNext10Post (int page)
        {
            return null;
        }

        
    }
}
