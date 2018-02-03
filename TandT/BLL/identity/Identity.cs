using System;
using System.Security.Cryptography;
using System.Text;

using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace BLL
{
    public static class Identity
    {
        private static Telegram.Bot.TelegramBotClient client = new Telegram.Bot.TelegramBotClient("452391424:AAHSD9tc3POLy0f_P4ctkCzt0ZR2pNtBAOE");


        /// <summary>
        /// Виконує вхід користувача
        /// </summary>
        /// <param name="pas"> Пароль </param>
        /// <returns>
        /// 0 => параметри вірні
        /// 1 => не активований юзер
        /// 2 => не правильні параметри
        /// </returns>
        public static int  Login(string Email, string pas)
        {
            var id = DataManager.Users.GetMailIdAsync(Email.Trim().ToLower()).Result;

            if (id != null)
                if (DataManager.Users.GetUserPas(id).Result == GetHash(pas, id))
                {
                    if (DataManager.Users.IsActiveted(id).Result)
                    {

                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            return 2;

        }

        public static ClaimsIdentity GetIdentity(string Email, string password)
        {
            var id = DataManager.Users.GetMailIdAsync(Email.Trim().ToLower()).Result;
            if (id != null)
            {
                var user = DataManager.Users.ReadMain(id).HashPass;
                if (user == GetHash(password, id.Substring(3)))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, id),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
                    };
                    ClaimsIdentity claimsIdentity =
                   new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            // если пользователя не найдено
            return null;
        }

        /// <summary>
        /// Перевірка на унікальність моделі реєстрації
        /// </summary>
        /// <param name="RM"></param>
        /// <returns></returns>
        public static bool IsUserUnique(RegistryModel RM)
        {
            bool isMailUnique = DataManager.Users.GetMailIdAsync(RM.Email.Trim().ToLower()).Result == null;
            bool isLoginUnique = DataManager.Users.CheckLoginAsync(RM.Login).Result;
            return isLoginUnique && isMailUnique;
            //var id = DataManager.Users.GetMailIdAsync(RM.Email.Trim().ToLower()).Result;
            // return !(id != null || DataManager.Users.CheckLoginAsync(RM.Login).Result);
        }
        
        /// <summary>
        /// Реєструє юзера за такою моделлю
        /// </summary>
        /// <param name="RM"></param>
        /// <returns></returns>
        public static bool Registry(RegistryModel RM)
        {
            if(!IsUserUnique(RM))
                return false; 

            if (RM.Pas == RM.ConfirmPas)
            {
                var user = new MongoManager.CollectionModels.User()
                {

                    

                    Main = new MainUser()
                    {
                        FirstName = RM.FirstName,
                        LastName = RM.LastName,
                        Email = RM.Email,
                        HashPass = GetHash(RM.Pas, RM.Login)
                    },
                    DateCreated = DateTime.UtcNow,
                    Activated = false,
                    _id = RM.Login
                };
                return DataManager.Users.Create(user);
            }
            return false;
        }


        /// <summary>
        /// Відправлення повідомлення через Email (по заданому id)//Асинхронно
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static async Task<bool> SendEmailAsync(string id, string Message)
        {
            //пошук Email по id           
            var Email = await DataManager.Users.GetEmailAsync(id);

            //створення повідомлення
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Think&Travel",
                                        "Think.and.Travel@outlook.com"));
            emailMessage.To.Add(new MailboxAddress("", Email));
            emailMessage.Subject = "Think&Travel.Identity";
            emailMessage.Body = new TextPart("plain") { Text = Message };

            //відправлення повідомлення
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync("Think.And.Travel@outlook.com", "121314qw");
                if (client.SendAsync(emailMessage).IsCompleted)
                {
                    client.Disconnect(true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Відпраляє Повідомлення в чат Телеграмівського бота і юзера, якщо це можливо
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static async Task<bool> SendTlMesAsync(string id, string Message)
        {
            try
            {
                //var chatId = await DataManager.Users.GetUserChatIdAsync(id);
                //await client.SendTextMessageAsync(chatId, Message);
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Повертає хеш паролю
        /// </summary>
        /// <param name="stringData"> Пароль</param>
        /// <param name="StringKey"> Ключ для хешування</param>
        /// <returns></returns>
        private static string GetHash(string stringData, string StringKey)
        {
            var data = Encoding.ASCII.GetBytes(stringData);
            var key = Encoding.ASCII.GetBytes(StringKey);
            using (var hmac = new HMACSHA1(key))
            {
                var hash = hmac.ComputeHash(data);
                return Encoding.ASCII.GetString(hash);      
            }
        }

        /// <summary>
        /// Повертає, чи не залогінений хтось з даного чату,і чи можна там реєструватися
        /// </summary>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        public static async Task<bool> IsFreeTelegramChat(long ChatId)
        {

            return true;
        }

    }
}
