using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esrp.Web.Administration.Accounts.Users
{
    public static class Constants
    {
        public struct OrganizationType2010
        {
            public const int VUZ = 1;    //ВУЗ
            public const int SSUZ = 2;   //ССУЗ
            public const int RCOI = 3;   //РЦОИ
            public const int OUO = 4;    //Орган управления образованием
            public const int Other = 5;  //Другое
            public const int Owner = 6;   //Учредитель
        }

        public struct Systems
        {
            public const int ESRP = 1;   //ЕСРП
            public const int FBS = 2;    //ИС ФБС
            public const int FBD = 3;    //ФИС ЕГЭ и приема
            public const int Flex = 5;   //Flex
            public const int Olymp = 6;   //ФБД ОШ
        }
    }
}