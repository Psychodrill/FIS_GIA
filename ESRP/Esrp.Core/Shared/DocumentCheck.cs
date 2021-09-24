using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Core.Shared
{
    public class DocumentCheck
    {
        public static List<string> DocSeriesCheck(string item)
        {
            List<string> resultError = new List<string>();

            string series = item.FullTrim();

            // Проверяем поле "Серия паспорта" на наличие пробелов
            if (series.IndexOf(" ") >= 0)
            {
                resultError.Add("В поле \"Серия документа\" не должно быть пробелов");
            }

            return resultError;
        }

        public static List<string> DocNumberCheck(string item)
        {
            List<string> resultError = new List<string>();

            string withPattern = item.FullTrim();
            string withoutPattern = item.FullTrim().Replace("*", "").Replace("?", "");

            if (withPattern.Length == 0)
            {
                return resultError;
            }

            // Проверяем поле "Номер паспорта" на количество символов
            // не менее 6 и не более 10
            if (withoutPattern.Length == withPattern.Length && (withPattern.Length < 6 || withPattern.Length > 10))
            {
                resultError.Add(string.Format("Поле \"Номер документа\" должно быть длиной от 6 до 10 символов. Сейчас {0}", withPattern.Length));
            }

            // Проверяем поле "Номер паспорта" на количество значимых символов
            // не менее 4
            if (withoutPattern.Length < 4)
            {
                resultError.Add(string.Format("В поле \"Номер документа\" должно быть не менее 4 значимых символов. Сейчас {0}", withoutPattern.Length));
            }

            return resultError;
        }
    }
}
