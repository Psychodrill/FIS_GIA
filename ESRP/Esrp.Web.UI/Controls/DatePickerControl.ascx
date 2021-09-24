<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePickerControl.ascx.cs" Inherits="Esrp.Web.Controls.DatePickerControl" %>
<asp:TextBox runat="server" CssClass="txt date" ID="datePickerBox" />
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Неверный формат даты" 
            ControlToValidate="datePickerBox" EnableClientScript="true" Display="None"
            ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$" />
<asp:CustomValidator ID="cv1" runat="server" Display="None" OnServerValidate="ValidateRange" />
<script type="text/javascript">
    $(function () {
        $(".date").datepicker({showOn:'both',
            buttonImage: "/Common/Images/ico-datepicker-Esrp.gif",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            minDate:<%=this.MinDate==DateTime.MinValue?"null":"new Date("+this.MinDate.Subtract(new DateTime(1970, 1,1)).TotalMilliseconds+")" %>,
            maxDate:<%=this.MaxDate==DateTime.MinValue?"null":"new Date("+this.MaxDate.Subtract(new DateTime(1970, 1,1)).TotalMilliseconds+")" %>,
            yearRange:'c-30:c+30'
        });
    });
    </script>