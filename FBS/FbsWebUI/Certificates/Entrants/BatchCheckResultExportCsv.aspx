<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckResultExportCsv.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.BatchCheckResultExportCsv" 
%><%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI"
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("EntrantBatchCheckResult.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server" DataSourceID="dsResultsList">
    <HeaderTemplate>Номер свидетельства;Образовательное учреждение;Фамилия;Имя;Отчество;Дата<%= "\r\n" %></HeaderTemplate>
    <ItemTemplate><%# Eval("CertificateNumber")%>;<%# !Convert.ToBoolean(Eval("IsExist")) ? 
        "НЕТ ДАННЫХ;" : 
        String.Format("{0};{1};{2};{3};{4}",
            Eval("OrganizationName"),
            Eval("LastName"),
            Eval("FirstName"),
            Eval("PatronymicName"),
            Convert.ToDateTime(Eval("EntrantCreateDate")).ToShortDateString()
        )%><%= "\r\n" %></ItemTemplate>
</asp:Repeater><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchEntrantCheck"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"><SelectParameters><fbs:CurrentUserParameter
        Name="login" Type="String" /><asp:QueryStringParameter
        Name="batchId" QueryStringField="id" Type="Int64" /></SelectParameters></asp:SqlDataSource>