<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.Check"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="fbs" TagName="HistoryCheck" Src="~/Controls/CommonNationalCertificates/HistoryCheckList.ascx" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheckCommon" Src="~/Controls/CommonNationalCertificates/HistoryCheckListCommon.ascx" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls.CommonNationalCertificates" Assembly="FbsWebUI" %>
    
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

<form runat="server" defaultbutton="btnCheck">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
     <table style="width:620px;">
       <tr><td colspan="2">
            <div class="notice" id="CheckTitleNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>Фамилию, Имя и Отчество можно вводить в произвольном регистре символов, это 
                    не влияет на результаты поиска.</p>
                <%--<p>Буквы Е и Ё считаются различными.</p>--%>
                <p>Поиск и проверка свидетельств осуществляются по строгому соответствию параметров запроса «Регистрационный номер свидетельства», «Фамилия», «Имя», «Отчество» параметрам, хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;.
                   Поля «Регистрационный номер свидетельства» и «Фамилия» обязательны для заполнения.
                <%--<p>Для проведения проверки необходимо указать баллы хотя бы по одному предмету.</p>--%>

            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>   
        </td></tr>
        <tr><td>
            <div class="form-l">

                <asp:TextBox runat="server" ID="txtNumber" CssClass="txt" />
                <input id="cNumber" value="Номер свидетельства" class ="txt h" style="display:none" />

                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Номер свидетельства" обязательно для заполнения' />
                    
                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtLastName" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Фамилия" обязательно для заполнения' />    
                    
                 <asp:RegularExpressionValidator runat="server"
                    ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                    ErrorMessage='Номер свидетельства должен быть в формате XX-XXXXXXXXX-XX' 
                    ValidationExpression="\d{2}-\d{9}-\d{2}" />
                 
                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt" />
                <input id="cLastName" value="Фамилия" class ="txt h" style="display:none" />

                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt" />
                <input id="cFirstName" value="Имя" class ="txt h" style="display:none" />

                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt" />
                <input id="cPatronymicName" value="Отчество" class ="txt h" style="display:none" />
                
                <script type="text/javascript">
                    IntiInputWithDefaultValue("<%= txtNumber.ClientID %>", "cNumber");
                    IntiInputWithDefaultValue("<%= txtLastName.ClientID %>", "cLastName");
                    IntiInputWithDefaultValue("<%= txtFirstName.ClientID %>", "cFirstName");
                    IntiInputWithDefaultValue("<%= txtPatronymicName.ClientID %>", "cPatronymicName");
                </script>
            </div>
        </td></tr>
        <tr><td colspan="2" class="t-line">
            <asp:Button runat="server" ID="btnReset" OnClick="BtnResetClick" 
                Text="Очистить" CssClass="bt" />
            <asp:Button runat="server" ID="btnCheck" OnClick="BtnCheckClick"
                Text="Проверить" CssClass="bt" />
        </td></tr>
    </table>

    <%--<fbs:HistoryCheck runat="server" ID="historyCheck" 
        DataGridSelectCommandName="SelectCheckHystory" 
        PagerSelectCommandName="GetNEWebUICheckLog"
        Type="CNENumber"/>
        
    <hr />--%>
    
    <fbs:HistoryCheckCommon runat="server" ID="historyCheckCommon" CheckType="CertificateNumber" CheckMode="Interactive" />

    <script type="text/javascript">
        InitNotice();
    </script>   

    <%--  
        Для восстановления состояния контролов необходим DataBind репитера. Второй раз DataBind 
        вызывается самим asp.net при окончательной отрисовкой страницы.
        Кэшированием датасорса я предотвращаю выполнение повторного запроса к базе при DataBind.
    --%>     
    <asp:SqlDataSource runat="server" ID="dsSubjects" 
        EnableCaching="true" CacheDuration="1"
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.GetSubject"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
    </asp:SqlDataSource>
    
    </form>
</asp:Content>