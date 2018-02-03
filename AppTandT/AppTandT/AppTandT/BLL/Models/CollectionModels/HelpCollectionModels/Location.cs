using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models.CollectionModels.HelpCollectionModels
{
    /// <summary>
    /// Визначає географічні координати
    /// </summary>
    public class Location
    {
        #region VAR
        public string Country;//{ get; set; }

        /// <summary>
        /// Населений пункт
        /// </summary>       
        public string City;// { get; set; }

        /// <summary>
        /// Довгота
        /// </summary>        
        public double GeoLong; //{ get; set; } 

        /// <summary>
        /// Широта
        /// </summary>        
        public double GeoLat; //{ get; set; }

        #endregion


    }
}
