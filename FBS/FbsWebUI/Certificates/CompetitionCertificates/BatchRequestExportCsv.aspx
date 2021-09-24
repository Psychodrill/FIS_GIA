<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestExportCsv.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CompetitionCertificates.BatchRequestExportCsv" 
%><%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI"
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("CompetitionCertificatesBatchRequest.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server" DataSourceID="dsResultsList">
    <HeaderTemplate>Степень;Олимпиада;Фамилия;Имя;Отчество;Регион;Город;Школа;Класс<%= "\r\n" %></HeaderTemplate>
    <ItemTemplate><%# 
        String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
            Convert.ToBoolean(Eval("IsExist")) ? Convert.ToString(Eval("Degree")) : "Не найдено",
            Eval("CompetitionTypeName"),
            Eval("LastName"),
            Eval("FirstName"),
            Eval("PatronymicName"),
            Eval("RegionName"),
            Eval("City"),
            Eval("School"),
            Eval("Class"))
    %><%= "\r\n" %></ItemTemplate>
</asp:Repeater><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCompetitionCertificateRequest"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"><SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
</SelectParameters></asp:SqlDataSource>