using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels.MainCollectionModels
{
    public class MainTour
    {

        public string Country { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public int? Price = null;

        public string About { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

    }
}
