using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels.HelpCollectionModels
{
    /// <summary>
    /// Зберігає питання і відповіді(до цього питання) для Task
    /// </summary>
    public class Question
    {
        #region VAR

        /// <summary>
        /// Саме питання
        /// </summary>        
        public string Value { get; set; }

        /// <summary>
        /// Неправильні варіанти відповіді
        /// </summary>        
        public List<string> Answers { get; set; }

        /// <summary>
        /// Правильна відповідь
        /// </summary>        
        public string TrueAnswer { get; set; }

        #endregion
    }
}