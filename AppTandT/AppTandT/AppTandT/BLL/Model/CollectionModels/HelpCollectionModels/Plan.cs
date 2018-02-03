using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.HelpCollectionModels
{
    public class Plan
    {
        #region VAR

        public string PlanTask { get; set; }

        /// <summary>
        /// Дата ймовірного початку виконання завдання
        /// </summary>        
        public DateTime? DateStart = null; //{ get; set; }

        /// <summary>
        /// Дата ймовірного закінчення виконання завдання 
        /// </summary>        
        public DateTime? DateClose = null; // { get; set; }

        /// <summary>
        /// Якийсь опис юзера
        /// </summary>        
        public string About { get; set; }


        #endregion


    }
}