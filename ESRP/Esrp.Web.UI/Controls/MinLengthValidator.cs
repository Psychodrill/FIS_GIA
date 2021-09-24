using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;

namespace Esrp.Web.Controls
{
    /// <summary>
    /// Summary description for MinLengthValidator.
    /// </summary>
    public class MinLengthValidator : BaseValidator
    {
        public MinLengthValidator()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        protected double _minLength = 0;
        public Double MinLength
        {
            get
            {
                return _minLength;
            }
            set
            {
                _minLength = value;
            }
        }


        protected override bool ControlPropertiesValid()
        {
            return true;
        }




        protected override bool EvaluateIsValid()
        {
            TextBox _tb = ((TextBox)this.FindControl(this.ControlToValidate));
            return (_tb.Text.Trim().Length >= this.MinLength);
        }


        protected override void OnPreRender(EventArgs e)
        {
            if (this.EnableClientScript) { this.ClientScript(); }
            base.OnPreRender(e);
        }


        protected void ClientScript()
        {
            this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "ctb_minlen");
            this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "minlength", MinLength.ToString());
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ctb_minlen"))
            {
                //this.Attributes["evaluationfunction"] = "ctb_minlen";
                //this.Attributes["minlength"] = this.MinLength.ToString();


                StringBuilder sb_Script = new StringBuilder();

                sb_Script.Append("function ctb_minlen(val) {");
                sb_Script.Append("\r");
                sb_Script.Append("var tb = document.all[document.all[val.id].controltovalidate];");
                sb_Script.Append("\r");
                sb_Script.Append("return(tb.value.length >= document.all[val.id].minlength);");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ctb_minlen", sb_Script.ToString(), true);
            }
        }
    }
}
