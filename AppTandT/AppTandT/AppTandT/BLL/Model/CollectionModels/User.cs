using AppTandT.BLL.Model.CollectionModels.HelpCollectionModels;
using AppTandT.BLL.Model.CollectionModels.MainCollectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model.CollectionModels
{
    public class User
    {
        #region VAR
        public string _id { get; set; }
        public MainUser Main { get; set; } = new MainUser();

        /// <summary>
        /// Дата реєстрації юзера
        /// </summary>
        public DateTime? DateCreated = null;

        /// <summary>
        /// Остання сесія
        /// </summary>
        public DateTime? LastLoginTime = null;

        /// <summary>
        /// Для перевірки підтвердження реєстрації
        /// </summary>
        public bool Activated;

        /// <summary>
        /// Всі підписки на зовнішні сервіси
        /// </summary>
        public Confirm Confirm { get; set; } = new Confirm();

        public double Money { get; set; }

        public List<Notice> Notices { get; set; } = new List<Notice>();

        /// <summary>
        /// Підписки юзера
        /// </summary>
        public List<string> Subscriptions { get; set; } = new List<string>();

        /// <summary>
        /// Список друзів
        /// </summary>
        public List<string> Friends { get; set; } = new List<string>();

        /// <summary>
        /// Завдання доступні до виконання
        /// </summary>
        public List<string> OpenTasks { get; set; } = new List<string>();

        /// <summary>
        /// Виконані(або просрочені) завдання
        /// </summary>
        public List<string> CloseTasks { get; set; } = new List<string>();

        /// <summary>
        /// Список id постів
        /// </summary>
        public List<string> Posts { get; set; } = new List<string>();

        /// <summary>
        /// Список id на яких зображений користувач
        /// </summary>
        public List<string> Photos { get; set; } = new List<string>();
        public List<Plan> Plans { get; set; } = new List<Plan>();

        #endregion


    }
}
