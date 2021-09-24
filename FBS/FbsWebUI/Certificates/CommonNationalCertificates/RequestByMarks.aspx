<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestByMarks.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByMarks"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="fbs" TagName="HistoryCheck" Src="~/Controls/CommonNationalCertificates/HistoryCheckList.ascx" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheckCommon" Src="~/Controls/CommonNationalCertificates/HistoryCheckListCommon.ascx" %>

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
                    не влияет на результаты поиска.
                <%--<p>Буквы Е и Ё считаются различными.</p>--%>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>   
        </td></tr>     
        <tr><td>
            <div class="form-l">
                <asp:TextBox runat="server" ID="txtLastName" CssClass="txt" />
                <input id="cLastName" value="Фамилия" class="txt h" style="display:none" /><br />

                <%--<asp:CustomValidator runat="server" ID="cvLastName" EnableClientScript="false"
                    ControlToValidate="txtLastName" Display="None" 
                    ErrorMessage='Поле "Фамилия" обязательно для заполнения' 
                    onservervalidate="CheckNameValue" />--%>
                                    
                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtLastName" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Фамилия" обязательно для заполнения' />    

                <asp:TextBox runat="server" ID="txtFirstName" CssClass="txt" />
                <input id="cFirstName" value="Имя" class="txt h" style="display:none" /><br />
                
                <%--<asp:CustomValidator runat="server" ID="cvFirstName" EnableClientScript="false"
                    ControlToValidate="txtFirstName" Display="None" 
                    ErrorMessage='Поле "Имя" обязательно для заполнения' 
                    onservervalidate="CheckNameValue" />--%>

                <asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtFirstName" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Имя" обязательно для заполнения' />    

                <asp:TextBox runat="server" ID="txtPatronymicName" CssClass="txt" />
                <input id="cPatronymicName" value="Отчество" class="txt h" style="display:none" /><br />
                
                <%--<asp:CustomValidator runat="server" ID="cvPatronymicName" EnableClientScript="false"
                    ControlToValidate="txtPatronymicName" Display="None" 
                    ErrorMessage='Поле "Отчество" обязательно для заполнения' 
                    onservervalidate="CheckNameValue" />--%>
                    
                <%--<asp:RequiredFieldValidator runat="server" 
                    ControlToValidate="txtPatronymicName" EnableClientScript="false" Display="None"
                    ErrorMessage='Поле "Отчество" обязательно для заполнения' />    --%>

                <script type="text/javascript" >
                    IntiInputWithDefaultValue("<%= txtLastName.ClientID %>", "cLastName");
                    IntiInputWithDefaultValue("<%= txtFirstName.ClientID %>", "cFirstName");
                    IntiInputWithDefaultValue("<%= txtPatronymicName.ClientID %>", "cPatronymicName");
                </script>
                
                <%--<asp:CustomValidator runat="server" ID="cvChoiceExt" EnableClientScript="false"
                    Display="None"
                    EnableViewState="false" ErrorMessage='Укажите Фамилию или Имя' 
                    onservervalidate="cvChoiceExt_ServerValidate" />     --%>
                
            </div>
        </td><td>
            <asp:Repeater runat="server"  ID="rpSubjects" DataSourceID="dsSubjects">
                <HeaderTemplate>
                    <table class="form-r">
                        <tr><th colspan="2"><div><div>Баллы</div></div></th></tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="odd"><td class="left-td">
                        <%#Eval("Name").ToString()%>
                    </td><td>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                        <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="7" />
                    </td></tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr><td class="left-td">
                        <%#Eval("Name").ToString()%>
                    </td><td>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                        <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="7" />
                    </td></tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
                
            <asp:CustomValidator runat="server" ID="cvSubjectMarks"
                EnableClientScript="false" Display="None"
                ErrorMessage="Необходимо указать баллы хотя бы по одному предмету. Для предметов 'Сочинение' и 'Изложение' указываются значения 1, 0, 'зачет' или 'незачет'" />
            
        </td></tr>
        <tr><td colspan="2" class="t-line">
            <asp:Button runat="server" ID="btnReset" OnClick="btnReset_Click"
                Text="Очистить" CssClass="bt" />
            <asp:Button runat="server" ID="btnCheck" OnClick="btnCheck_Click"
                Text="Проверить" CssClass="bt" />
                        
        </td></tr>
    </table>

    <%--<fbs:HistoryCheck runat="server" ID="historyCheck" 
        DataGridSelectCommandName="SelectCheckHystory" 
        PagerSelectCommandName="GetNEWebUICheckLog"
        Type="Marks"/>--%>
        
    <fbs:HistoryCheckCommon runat="server" ID="historyCheckCommon" CheckType="NameAndMarks" CheckMode="Interactive" />

    <script type="text/javascript">
       InitNotice();
    </script>    
    
    <%--  
        Для восстановления состояния контролов необходим DataBind репитера. Второй раз DataBind 
        вызывается самим asp.net при окончательной отрисовкой страницы.
        Кэшированием датасорса я предотвращаю выполнение повторного запроса к базе при DataBind.
    --%>     
    <asp:SqlDataSource  runat="server" ID="dsSubjects"
        EnableCaching="true" CacheDuration="1" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="GetSubject"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
    </asp:SqlDataSource>
    
    </form>
</asp:Content>
