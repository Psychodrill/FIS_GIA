<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestByPassport.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByPassport" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheck" Src="~/Controls/CommonNationalCertificates/HistoryCheckList.ascx" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheckCommon" Src="~/Controls/CommonNationalCertificates/HistoryCheckListCommon.ascx" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server" defaultbutton="btnSearch">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

     <table style="width:480px;">
       <tr><td colspan="2">
            <div class="notice" id="RequestByPassportTitleNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>Фамилию, Имя и Отчество можно вводить в произвольном регистре символов, это 
                    не влияет на результаты поиска. В поля «Серия документа» и «Номер документа» можно вводить подстановочные символы «*» (любая подстрока) и «?» (любой символ).</p>
                <%--<p>Буквы Е и Ё считаются различными.</p>--%>
                <p>Поиск и проверка свидетельств осуществляются по строгому соответствию параметров запроса «Серия документа», «Номер документа», «Фамилия», «Имя», «Отчество» параметрам, хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;. Поля «Номер документа» и «Фамилия» обязательны для заполнения.</p>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>   
        </td></tr>
        <tr><td>
            <div class="form-l">
                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt" />
                <input id="cLastName" value="Фамилия" class ="txt h" style="display:none" />
                                
                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt" />
                <input id="cFirstName" value="Имя" class ="txt h" style="display:none" />
                
                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt" />
                <input id="cPatronymicName" value="Отчество" class ="txt h" style="display:none" />
                
                <asp:TextBox runat="server" ID="txtSeries" CssClass="txt" />
                <input id="cSeries" value="Серия документа" class ="txt h" style="display:none" />

                <asp:TextBox runat="server" ID="txtNumber" CssClass="txt date" />
                <input id="cNumber" value="Номер документа" class ="txt h" style="display:none" />

                <br />

                <asp:RequiredFieldValidator ID="rfvLastname" runat="server" 
                    ControlToValidate="txtLastName" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Фамилия" обязательно для заполнения' />   
                    
                <asp:RequiredFieldValidator ID="rfvNumber" runat="server" 
                  ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                  ErrorMessage='Поле "Номер документа" обязательно для заполнения' />  
                  
                <asp:CustomValidator ID="vlEnchancedPassportNumber" runat="server" 
                    ErrorMessage="CustomValidator" Display="None"></asp:CustomValidator>
                  
                <%--<asp:RequiredFieldValidator ID="rfvSeries" runat="server" 
                    ControlToValidate="txtSeries" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Серия документа" обязательно для заполнения' />   --%>
                                     
                <script type="text/javascript" >
                    IntiInputWithDefaultValue("<%= txtLastName.ClientID %>", "cLastName");
                    IntiInputWithDefaultValue("<%= txtFirstName.ClientID %>", "cFirstName");
                    IntiInputWithDefaultValue("<%= txtPatronymicName.ClientID %>", "cPatronymicName");
                    IntiInputWithDefaultValue("<%= txtSeries.ClientID %>", "cSeries");
                    IntiInputWithDefaultValue("<%= txtNumber.ClientID %>", "cNumber");
                </script>
            </div>
        </td>
      </tr>
      <tr>
        <td class="t-line">
            <asp:Button runat="server" ID="btnReset" OnClick="btnReset_Click"
                Text="Очистить" CssClass="bt" />
          <asp:Button runat="server" ID="btnSearch" Text="Проверить" CssClass="bt"
              onclick="btnSearch_Click" />
        </td>
      </tr>
    </table>

    <%--<fbs:HistoryCheck runat="server" ID="historyCheck" 
        DataGridSelectCommandName="SelectCheckHystory" 
        PagerSelectCommandName="GetNEWebUICheckLog"
        Type="Passport"/>--%>
        
    <fbs:HistoryCheckCommon runat="server" ID="historyCheckCommon" CheckType="DocumentNumber" CheckMode="Interactive" />

    <script type="text/javascript">
       InitNotice();
    </script>
    
</form>    
</asp:Content>
