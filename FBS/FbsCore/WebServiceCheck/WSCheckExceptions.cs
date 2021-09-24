using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core.WebServiceCheck
{
    public class WSCheckException : Exception
    {
        public WSCheckException() : base() { }
        public WSCheckException(string message) : base(message) { }
    }

    // Количество ошибок в формате XML-запроса превышает установленный предел
    public class WSErrorCountException : WSCheckException
    {
        public WSErrorCountException() : base() { }
        public WSErrorCountException(string message) : base(message) { }
    }

    // Ошибка в структуре тэга <query>
    public class WSQueryElementException : WSCheckException
    {
        public WSQueryElementException() : base() { }
        public WSQueryElementException(string message) : base(message) { }
    }

}
