using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class ApplicationPrioritiesViewModel
    {
        /// <summary>
        /// Код заявления
        /// </summary>
        public int ApplicationId { get; set; }

        public bool ChangeCg { get; set; }

        private List<ApplicationPriorityViewModel> priorities;

        /// <summary>
        /// Конструктор. Создаёт список приоритетов для заявления
        /// </summary>
        public ApplicationPrioritiesViewModel()
        {
            priorities = new List<ApplicationPriorityViewModel>();
        }

        /// <summary>
        /// Список приоритетов для заявления
        /// </summary>
        public List<ApplicationPriorityViewModel> ApplicationPriorities
        {
            get { return priorities; }
            set { priorities = value; }
        }
    }
}