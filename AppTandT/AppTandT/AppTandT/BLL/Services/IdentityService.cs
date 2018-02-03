using AppTandT.BLL.Help;
using AppTandT.BLL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Services
{
    public class IdentityService : ClientService
    {

        class jsonLoginModel
        {
            public string username { get; set; }

            public string access_token { get; set; }
        };

        /// <summary>
        /// підключається до сервера та робить логін для юзера
        /// отримує JWT, та зберігає
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>
        /// string => опис помилки
        /// null => коли все вірно
        /// </returns>
        public static async Task<string> LoginAsync(LoginModel loginModel)
        {

            var client = GetClient();
            var response = await client.PostAsync(App.ApiUri + "api/auth/token",
               new StringContent(
                    JsonConvert.SerializeObject(loginModel),
                    Encoding.UTF8, "application/json")
                );
            var jwt = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException(jwt);


            jsonLoginModel model = JsonConvert.DeserializeObject<jsonLoginModel>(jwt);

            Dekanat.newSesion(model.username, loginModel.Username, model.access_token);

            return null;
        }

        /// <summary>
        /// Реєструє користувача, або виводить помилку
        /// </summary>
        /// <returns>
        /// string => значення помилки
        /// null => все пройшло успішно
        /// </returns>
        public static async Task<string> RegistryAsync(RegistryModel registryModel)
        {
            var client = GetClient();
            var response = client.PostAsync(App.ApiUri + "api/registry",
               new StringContent(
                    JsonConvert.SerializeObject(registryModel),
                    Encoding.UTF8, "application/json")).Result;

            var jwt = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var jwtobj = JsonConvert.DeserializeObject<JwtModel>(jwt);
                throw new ServiceException(jwtobj.message);
            }
            return null;
        }
    }
}
