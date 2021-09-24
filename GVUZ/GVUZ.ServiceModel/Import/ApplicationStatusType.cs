using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import
{
    public partial class ApplicationStatusType
    {
        // FIS-1453: Удалены статусы 5 и 7
        /// <summary>
        /// Редактируется
        /// </summary>
        public const int Draft = 1;
        /// <summary>
        /// Новое
        /// </summary>
        public const int New = 2;
        /// <summary>
        /// Не прошедшее проверку
        /// </summary>
        public const int Failed = 3;
        /// <summary>
        /// Принято
        /// </summary>
        public const int Accepted = 4;

        //public const int Removed = 5;

        /// <summary>
        /// Отозвано
        /// </summary>
        public const int Denied = 6;
        /// <summary>
        /// В приказе
        /// </summary>
        public const int InOrder = 8;
        
    }
}
