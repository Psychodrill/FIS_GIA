﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Fbs.Web.Certificates.SchoolLeavingCertificates
{
    public partial class BatchCheckResult : System.Web.UI.Page
    {
        public bool HasResults
        {
            get { return dgResultsList.Items.Count > 0; }
        }
    }
}
