using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckWebService
{
    public static class Settings
    {
        public static string CheckWSUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["CheckWSUserName"] ;
            }
        }

        public static string CheckWSPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["CheckWSPassword"];
            }
        }

        public static string CheckWSClient
        {
            get
            {
                return ConfigurationManager.AppSettings["CheckWSClient"];
            }
        }
    }
}
