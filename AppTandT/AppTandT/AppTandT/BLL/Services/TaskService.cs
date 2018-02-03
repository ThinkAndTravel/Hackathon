using AppTandT.BLL.Help;
using AppTandT.BLL.Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTandT.BLL.Services
{
    public class TaskService : ClientService
    {
        //public delegate void ShowAlert(string title, string message, string cancel, string accept = null);
        //public event ShowAlert OnShowAlert;

        /// <summary>
        /// Returns list of TaskModel 
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="City">City</param>
        /// <returns></returns>
        public static async Task<List<TaskViewModel>> GetTasksForCityAsync(string id, string City)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/gettfc?id=" + id + "&city=" + City;
            var response = await client.GetAsync(url);


            //  if(OnShowAlert != null)OnShowAlert("внимание внимание", "тупо месседж из таск сервиса", "оке");
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<List<TaskViewModel>>(str);
            return arr;
        }


        /// <summary>
        /// Returns list of TaskModel
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="locLat">Location Latitude</param>
        /// <param name="locLong">Location Longitude</param>
        /// <returns></returns>
        public static async Task<List<TaskViewModel>> GetTasksForCurLocAsync(string id, double locLat, double locLong)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/gettfcl?id=" + id + "&loclat=" + locLat.ToString().Replace(',', '.')
                                 + "&loclong=" + locLong.ToString().Replace(',', '.');
            var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<List<TaskViewModel>>(str);
            return arr;
        }

        public static async Task<List<TaskViewModel>> GetActiveTasks(string UserId, int k)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/gettaska?userid=" + UserId + "&k=" + k.ToString();
            var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            var arr = JsonConvert.DeserializeObject<List<TaskViewModel>>(str);
            return arr;
        }

        /// <summary>
        /// Перевіряє чи це завдання пдходить для користувача 
        /// тобто він вже не виконав його раніше і це завдання 
        /// з обраної локації
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="taskId">Task ID</param>
        /// <param name="City">City</param>
        /// <returns></returns>
        public static async Task<int?> CheckAsync(string id, string taskId, string City)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/check?id=" + id + "&taskid=" + taskId + "&city=" + City;
            var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException("Can't connect to the server! Try again later.", title: "Ooops");

            var str = await response.Content.ReadAsStringAsync();
            int? ck = JsonConvert.DeserializeObject<int?>(str);
            return ck;
        }

        public static async Task<bool> AddRankAsync(TaskServiceModel model)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/addrank";
            var response = await client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json")
                    );

            if (response.IsSuccessStatusCode) return true;

            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<JwtModel>(str);
            throw new ServiceException(obj.message);
        }
        public static async Task<bool> AddTaskA(TaskServiceModel model)
        {
            var client = GetClient();
            var url = App.ApiUri + "api/task/addtaska";
            var response = await client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json")
                    );

            if (response.IsSuccessStatusCode) return true;

            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<JwtModel>(str);
            throw new ServiceException(obj.message);
        }
    }
}
