using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;


namespace BLL.User.userLogic.extension
{
    public static class AvatarLogic
    {
        /// <summary>
        ///  Асинхронно додає новий аватар юзера
        /// </summary>
        /// <returns></returns>
        public static void NewAvatar(this UserLogic UL, string id, string id_photo)
        {
            var user = DataManager.Users.ReadMain(id);
            user.Avatar = id_photo;
            DataManager.Users.UpdateMain(id, user);
        }
        public static string GetAvatar(this UserLogic UL, string id)
        {
            return DataManager.Users.ReadMain(id).Avatar;
        }
    }
}
