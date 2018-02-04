using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.ViewModels
{
    public class PostViewModel
    {
        public List<string> Photo, URLphoto;
        public string UserId;
        public string UserAvatar;
        public DateTime DateCreated;
        public string id;
        public int CountLikes;
        public string About;
        public bool Comment;
    }
}
