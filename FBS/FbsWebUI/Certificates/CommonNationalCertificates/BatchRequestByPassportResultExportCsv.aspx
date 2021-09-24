<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultExportCsv.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultExportCsv" 
%><%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" 
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("BatchRequestResult.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server"
    DataSourceID="dsResultsList">
<HeaderTemplate>Номер свидетельства;Фамилия;Имя;Отчество;Серия документа;Номер документа;Регион<%= "\n" %></HeaderTemplate>
<ItemTemplate><%# Convert.ToBoolean(Eval("IsDeny")) ? string.Format("АННУЛИРОВАНО {0}: {1}", Eval("CertificateNumber"), Convert.ToString(Eval("DenyComment"))) :
                  (!Convert.ToBoolean(Eval("IsExist")) ? string.Format("НЕ НАЙДЕНО") : Eval("CertificateNumber"))
                    %>;<%# Eval("LastName") 
                    %>;<%# Eval("FirstName")
                    %>;<%# Eval("PatronymicName")
                    %>;<%# Eval("PassportSeria")
                    %>;<%# Eval("PassportNumber")
                    %>;<%# Convert.ToString(Eval("RegionName")).ToUpperInvariant()
                    %><%= "\n" %></ItemTemplate>
</asp:Repeater><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateRequest" CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters>
</asp:SqlDataSource>