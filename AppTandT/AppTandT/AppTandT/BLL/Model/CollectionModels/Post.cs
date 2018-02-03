using AppTandT.BLL.Model.CollectionModels.MainCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;

namespace AppTandT.BLL.Model.CollectionModels
{
    public class Post
    {
        #region VAR

        public MainPost Main { get; set; } = new MainPost();
        /// <summary>
        /// Список людей які є відмічені на фото
        /// </summary>
        public List<string> UserPost { get; set; } = new List<string>();

        /// <summary>
        /// Опис посту
        /// </summary>
        public string About { get; set; }


        /// <summary>
        /// UserID
        /// </summary>
        public List<string> Likes { get; set; } = new List<string>();

        public List<Comment> PostComments { get; set; } = new List<Comment>();
        #endregion


    }
}
