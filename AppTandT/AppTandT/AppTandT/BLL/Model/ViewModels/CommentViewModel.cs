using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.ViewModels
{
    public class CommentViewModel
    {
        public string UserId;
        public string UserName;
        public string UserAvatar;
        public DateTime DateCreated;
        public int CountLike;
        public string Value;


        public bool isValid() { return true; }
    }
}
