<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAccountActivityCommon.aspx.cs" 
    MasterPageFile="~/Common/Templates/Administration.Master" 
    Inherits="Fbs.Web.Administration.Reports.UserAccountActivityCommon" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
<form runat="server">
    <asp:Repeater runat="server" id="rReport"
        DataSourceID="dsReport"
        EnableViewState="false" >
        <ItemTemplate>
            <table class="form" style="width:100%" >
                <tr>
                    <td style="width:55%">Число свидетельств в базе</td>
                    <td><%# Eval("Всего свидетельств")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Общее количество зарегистрированных пользователей</td>
                    <td><%# Eval("Зарегистрировано пользователей")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Общее число проверенных свидетельств</td>
                    <td><%# Eval("Проверок всего")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Число проверенных свидетельств (уникальных)</td>
                    <td><%# Eval("Уникальных проверок")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Проверено свидетельств в пакетном режиме (уникальных)</td>
                    <td><%# Eval("Уникальных пакетных проверок")%></td>
                </tr>
                 <tr>
                    <td style="width:55%">Проверено интерактивном в пакетном режиме (уникальных)</td>
                    <td><%# Eval("Уникальных интерактивных проверок")%></td>
                </tr>
                <%--<tr>
                    <td style="width:55%">Количество запросов к Подсистеме ФИС Результаты ЕГЭ</td>
                    <td><%# Eval("CommonNationalCertificateCheckCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество вузов, предоставивших информацию о зачислении</td>
                    <td><%# Eval("EntrantAcademyCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество ссузов, предоставивших информацию о зачислении</td>
                    <td><%# Eval("EntrantCollegeCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество записей о зачисленных по результатам ЕГЭ</td>
                    <td><%# Eval("EntrantCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество вузов, предоставивших информацию о забравших документы</td>
                    <td><%# Eval("EntrantRenunciationAcademyCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество ссузов, предоставивших информацию о забравших документы</td>
                    <td><%# Eval("EntrantRenunciationCollegeCount")%></td>
                </tr>
                <tr>
                    <td style="width:55%">Количество записей о забравших документы</td>
                    <td><%# Eval("EntrantRenunciationCount")%></td>
                </tr>--%>          
            </table>
        </ItemTemplate>
    </asp:Repeater>

    <web:NoRecordsText runat="server" ControlId="rReport">
        <p class="notfound">Нет данных</p>
    </web:NoRecordsText>

    <asp:SqlDataSource runat="server" ID="dsReport" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SELECT * FROM ReportCommonStatisticsTVF (null ,null)" 
        CancelSelectOnNullParameter="false" onselecting="dsReport_Selecting"
        >
    </asp:SqlDataSource>

</form>
</asp:Content>