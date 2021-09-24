<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckResultExportCsv.aspx.cs" 
    Inherits="Fbs.Web.Certificates.SchoolLeavingCertificates.BatchCheckResultExportCsv" 
%><%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI"
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("SchoolLeavingCertificatesBatchCheckResult.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server" DataSourceID="dsResultsList">
    <HeaderTemplate>Номер аттестата;Статус;Источники<%= "\r\n" %></HeaderTemplate>
    <ItemTemplate><%# Eval("CertificateNumber")%>;<%# 
        Convert.ToBoolean(Eval("IsDeny")) ? String.Format("подделка;{0}", Eval("DenyComment")) : ";"
    %><%= "\r\n" %></ItemTemplate>
</asp:Repeater><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchSchoolLeavingCertificateCheck"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"><SelectParameters><fbs:CurrentUserParameter
        Name="login" Type="String" /><asp:QueryStringParameter
        Name="batchId" QueryStringField="id" Type="Int64" /></SelectParameters></asp:SqlDataSource>