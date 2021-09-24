<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultExportCsvExtendedByExam.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultExportCsvExtendedByExam" 
%><%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" 
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("BatchRequestResultExtendedByExam.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server"   
    DataSourceID="dsResultsList"><ItemTemplate><%# 
    string.Format("{0};{1};{2};{3};{4};", 
       Eval("LastName"), 
       Eval("FirstName"),
       Eval("PatronymicName"),
       Eval("PassportSeria"),
       Eval("PassportNumber"))
%><%# 
    Convert.ToBoolean(Eval("IsExist")) ? 
            String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                Eval("RussianHasExam"),
                Eval("MathematicsHasExam"),
                Eval("PhysicsHasExam"), 
                Eval("ChemistryHasExam"), 
                Eval("BiologyHasExam"), 
                Eval("RussiaHistoryHasExam"), 
                Eval("GeographyHasExam"), 
                Eval("EnglishHasExam"), 
                Eval("GermanHasExam"), 
                Eval("FranchHasExam"), 
                Eval("SocialScienceHasExam"), 
                Eval("LiteratureHasExam"), 
                Eval("SpanishHasExam"), 
                Eval("InformationScienceHasExam")
            ) : "НЕ НАЙДЕНО"
%>
</ItemTemplate></asp:Repeater><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateRequestExtendedByExam" CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters>
</asp:SqlDataSource>