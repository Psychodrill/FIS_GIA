﻿using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchCheckFileFormatExportCsv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string FileName = Request.QueryString["FileName"];
            if (!String.IsNullOrEmpty(FileName))
            {
                // Если была ошибка "Maximum request length exceeded"
                Repeater1.DataSource = Application[FileName];
                Repeater1.DataBind();

            }


//            Repeater1.DataSource = Application["reportTable"];
  //          Repeater1.DataBind();
        }
    }
}
