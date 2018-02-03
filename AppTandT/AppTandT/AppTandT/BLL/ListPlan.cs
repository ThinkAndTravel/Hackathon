using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using Newtonsoft.Json;


namespace AppTandT.BLL
{
    public class ListPlan
    {
        public static string Plans
        {
            get => AppSettings.GetValueOrDefault(nameof(Plans), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(Plans), value);
        }
        public void SET(List<Plan> list)
        {
            Plans = JsonConvert.SerializeObject(list);
        }
        public List<Plan> GET()
        {
            return JsonConvert.DeserializeObject<List<Plan>>(Plans);
        }
        private static ISettings AppSettings
        {
            get
            {
                if (CrossSettings.IsSupported)
                    return CrossSettings.Current;

                return null; // or your custom implementation 
            }
        }

    }
}
