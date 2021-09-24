<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WildcardRequest.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.WildcardRequest" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server" defaultbutton="btnSearch">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

     <table style="width:380px;">
       <tr><td colspan="2">
            <div class="notice" id="RequestByPassportTitleNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>Запросы во время пиковой нагрузки системы, а также с широким диапазоном возможных вариантов могут закончиться ошибкой. В этом случае необходимо:</p>
				<ol style="margin-left:3em;"><li>повторить запрос после разгрузки системы;</li>
				<li>указать дополнительные параметры поиска свидетельств.</li></ol>
				<p>Для удобства поиска свидетельств допускается использовать маску: <b>%</b> - любое число любых символов, <b>_</b> - один любой символ. Так, например, можно задать диапазон поиска номеров: <b>01-0000033%-09</b> или поискать внутри фамилии: <b>%иванов%</b>.</p>
				<p>Результаты поиска отсортированы по году и номеру свидетельства в порядке возрастания сначала года, затем номера.</p>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>   
        </td></tr>
        <tr><td>
            <div class="form-l">
                <asp:TextBox runat="server" ID="txtNumber" CssClass="txt" />
                <input id="cNumber" value="Номер свидетельства" class ="txt h" style="display:none" />
                
                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt" />
                <input id="cLastName" value="Фамилия" class ="txt h" style="display:none" />
                                
                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt" />
                <input id="cFirstName" value="Имя" class ="txt h" style="display:none" />
                
                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt" />
                <input id="cPatronymicName" value="Отчество" class ="txt h" style="display:none" />
                
                <asp:TextBox runat="server" ID="txtDocSeries" CssClass="txt" />
                <input id="cDocSeries" value="Серия документа" class ="txt h" style="display:none" />

                <asp:TextBox runat="server" ID="txtDocNumber" CssClass="txt" />
                <input id="cDocNumber" value="Номер документа" class ="txt h" style="display:none" />

                <asp:TextBox runat="server" ID="txtTypographicNumber" CssClass="txt" />
                <input id="cTypographicNumber" value="Типографский номер" class ="txt h" style="display:none" />

                <script type="text/javascript" >
                    IntiInputWithDefaultValue("<%= txtLastName.ClientID %>", "cLastName");
                    IntiInputWithDefaultValue("<%= txtFirstName.ClientID %>", "cFirstName");
                    IntiInputWithDefaultValue("<%= txtPatronymicName.ClientID %>", "cPatronymicName");
                    IntiInputWithDefaultValue("<%= txtDocSeries.ClientID %>", "cDocSeries");
                    IntiInputWithDefaultValue("<%= txtDocNumber.ClientID %>", "cDocNumber");
                    IntiInputWithDefaultValue("<%= txtTypographicNumber.ClientID %>", "cTypographicNumber");
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

    <script type="text/javascript">
       InitNotice();
    </script>
    
</form>    
</asp:Content>
