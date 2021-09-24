<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Request.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CompetitionCertificates.Request"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server" defaultbutton="btnCheck">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
     <table style="width:380px;">
       <tr><td colspan="2">
            <div class="notice" id="CompetitionCertificatesRequestNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>Фамилию, Имя и Отчество можно вводить в произвольном регистре символов, это 
                    не влияет на результаты поиска.</p>
                <p>Буквы Е и Ё считаются различными.</p>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>   
        </td></tr>     
        <tr><td>
            <div class="form-l">
                <asp:DropDownList runat="server" ID="ddlCompetitionType" CssClass="sel" AppendDataBoundItems="true"
                    DataSourceID="dsCompetitionType" DataTextField="Name" DataValueField="Id" >
                    <asp:ListItem Value="">&lt;Олимпиады&gt;</asp:ListItem>
                </asp:DropDownList><br />
            
                <asp:DropDownList runat="server" ID="ddlRegion" CssClass="sel long" AppendDataBoundItems="true" 
                        DataSourceID="dsRegion" DataValueField="RegionId" DataTextField="Name">
                    <asp:ListItem Value="">&lt;Регион&gt;</asp:ListItem>
                </asp:DropDownList><br />
                
                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt" />
                <input id="cLastName" value="Фамилия" class ="txt" style="display:none" /><br />
                
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
                    EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Фамилия" обязательно для заполнения' />  
            
                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt" />
                <input id="cFirstName" value="Имя" class ="txt" style="display:none" /><br />
                
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" 
                    EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Имя" обязательно для заполнения' />      
                         
                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt" />
                <input id="cPatronymicName" value="Отчество" class ="txt" style="display:none" />
                
                <script type="text/javascript" >
                    IntiInputWithDefaultValue("<%= txtLastName.ClientID %>", "cLastName");
                    IntiInputWithDefaultValue("<%= txtFirstName.ClientID %>", "cFirstName");
                    IntiInputWithDefaultValue("<%= txtPatronymicName.ClientID %>", "cPatronymicName");
                </script>
            </div>
        </td></tr>
        <tr><td colspan="2" class="t-line">
            <asp:Button runat="server" ID="btnReset" OnClick="btnReset_Click"
                Text="Очистить" CssClass="bt" />
            <asp:Button runat="server" ID="btnCheck" OnClick="btnCheck_Click"
                Text="Проверить" CssClass="bt" />
        </td></tr>
    </table>

    <script type="text/javascript">
       InitNotice();
    </script>    
    
    <asp:SqlDataSource  runat="server" ID="dsCompetitionType"
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCompetitionType"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" /> 
        
    <asp:SqlDataSource  runat="server" ID="dsRegion"
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchRegion"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" /> 
        
    </form>
</asp:Content>
