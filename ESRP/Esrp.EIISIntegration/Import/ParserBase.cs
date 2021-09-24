using System;
using System.Linq;

namespace Esrp.EIISIntegration.Import
{
    internal abstract class ParserBase
    {
        public string ErrorDescription { get; private set; }

        public bool ResponseIsError { get; private set; }

        public bool ResponseIsCriticalError { get; private set; }

        protected string response_;
        public ParserBase(string response)
        {
            if (String.IsNullOrEmpty(response))
                throw new ArgumentException("response");

            response_ = response;

            ProcessResponse();
        }

        protected abstract void ProcessResponseInternal();

        private void ProcessResponse()
        {
            if (response_ == "0321")
            {
                ErrorDescription = "Неверный логин или пароль";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "0322")
            {
                ErrorDescription = "Неверный идентификатор сессии";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "033")
            {
                ErrorDescription = "Информация по объекту недоступна";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "034")
            {
                ErrorDescription = "Объект не объявлен";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "035")
            {
                ErrorDescription = "Отсутствует право доступа к объекту";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "0541")
            {
                ErrorDescription = "Пакет не найден";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "0542")
            {
                ErrorDescription = "Не найдена часть пакета";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "053")
            {
                ErrorDescription = "Пакет не сформирован";
                ResponseIsCriticalError = false;
                ResponseIsError = true;
            }
            else if (response_ == "074")
            {
                ErrorDescription = "Информация временно недоступна";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_ == "100")
            {
                ErrorDescription = "Внутренняя ошибка системы";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }
            else if (response_.All(obj => Char.IsDigit(obj)))
            {
                ErrorDescription = "Неизвестная ошибка ЕИИС (" + response_ + ")";
                ResponseIsCriticalError = true;
                ResponseIsError = true;
            }

            if (!ResponseIsError)
            {
                ProcessResponseInternal();
            }
        }
    }
}
