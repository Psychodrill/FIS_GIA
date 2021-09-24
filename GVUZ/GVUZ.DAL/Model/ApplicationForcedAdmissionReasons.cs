using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Model
{
    public static class ApplicationForcedAdmissionReasons
    {
        /// <summary>
        /// Ошибка в реквизитах документа, удостоверяющего личность, в подсистеме "Результаты ЕГЭ"
        /// </summary>
        public const int IdentityDocumentError = 1;

        /// <summary>
        /// Несоответствие результатов ЕГЭ в подсистеме "Результаты ЕГЭ" и ФИС ГИА и Приема
        /// </summary>
        public const int EgeResultsError = 2;
    }
}
