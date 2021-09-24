using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;
using System.Configuration;

namespace Esrp.Web.Extentions
{
    [Serializable]
    public class ReportServerCredentials : IReportServerCredentials
    {
        public WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(
                    ConfigurationManager.AppSettings["RSUserName"],
                    ConfigurationManager.AppSettings["RSPassword"],
                    ConfigurationManager.AppSettings["RSDomain"]);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;
            return false;
        }
    }
}