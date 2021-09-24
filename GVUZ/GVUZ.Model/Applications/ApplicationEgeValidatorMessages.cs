using System;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.Model.Applications
{
    internal static class ApplicationEgeValidatorMessages
    {
        public static string StatusForSubject(decimal mark, string subject, string statusName)
        {
            return String.Format("Для результата {0}, указанного по предмету {1}, свидетельство в АИС ФБС имеет статус {2}.", mark, subject, statusName);
        }

        public static string NoResultValueForSubject(string subject)
        {
            return String.Format("По предмету {0} не указан результат ResultValue в БД", subject);
        }

        public static string CertNotFoundForSubject(decimal mark, string subject)
        {
            return String.Format("Для результата {0}, указанного по предмету {1}, не найдено свидетельство в АИС ФБС.", mark, subject);
        }

        public static string EgeErrorPrefix(EGEDocumentViewModel viewModel = null)
        {
            if (viewModel == null)
                return "Ошибки при проверке свидетельств о ЕГЭ: ";
            else 
            return String.Format("Ошибки при проверке свидетельства о ЕГЭ {0}: ", viewModel.DocumentNumber);
        }
    }
}
