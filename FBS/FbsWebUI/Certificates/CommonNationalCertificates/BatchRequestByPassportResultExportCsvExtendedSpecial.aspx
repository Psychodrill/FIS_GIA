<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultExportCsvExtendedSpecial.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultExportCsvExtendedSpecial" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI"
%><%@ Import Namespace="Fbs.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("BatchRequestResultExtendedSpecial.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%><asp:Repeater runat="server" id="rptResultsList">
<HeaderTemplate>Номер свидетельства;Регион;Фамилия;Имя;Отчество;Серия. док.;Номер док.;Русский язык;Апелляция;Математика;Апелляция;Физика;Апелляция;Химия;Апелляция;Биология;Апелляция;История россии;Апелляция;География;Апелляция;Английский язык;Апелляция;Немецкий язык;Апелляция;Французский язык;Апелляция;Обществознание;Апелляция;Литература;Апелляция;Испанский язык;Апелляция;Информатика;Апелляция<%= "\n" %></HeaderTemplate>
<ItemTemplate><%# Convert.ToBoolean(Eval("IsDeny")) ? string.Format("Аннулировано {0}: Аннулировано в связи с внесением изменений. Актуальное свидетельство: {1}", Eval("CertificateNumber"), Convert.ToString(Eval("DenyNewCertificateNumber"))) :
           (!Convert.ToBoolean(Eval("IsExist")) ? string.Format("Не найдено") : Eval("CertificateNumber"))
    %>;<%# string.Format(!Convert.ToBoolean(Eval("IsExist")) ? "{1};{2};{3};{4};{5}" : "{0};{1};{2};{3};{4};{5}", 
           Convert.ToString(Eval("RegionName")).ToUpperInvariant(),
           Eval("LastName"), 
           Eval("FirstName"),
           Eval("PatronymicName"),
           Eval("PassportSeria"),
           Eval("PassportNumber"))
    %><%# !Convert.ToBoolean(Eval("IsExist")) ? string.Empty :
          string.Format(";{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27}",
                Eval("RussianMark"),
                Convert.IsDBNull(Eval("RussianHasAppeal")) || !Convert.ToBoolean(Eval("RussianHasAppeal")) ? "0" : "1",
                Eval("MathematicsMark"),
                Convert.IsDBNull(Eval("MathematicsHasAppeal")) || !Convert.ToBoolean(Eval("MathematicsHasAppeal")) ? "0" : "1",
                Eval("PhysicsMark"), 
                Convert.IsDBNull(Eval("PhysicsHasAppeal")) || !Convert.ToBoolean(Eval("PhysicsHasAppeal")) ? "0" : "1",
                Eval("ChemistryMark"), 
                Convert.IsDBNull(Eval("ChemistryHasAppeal")) || !Convert.ToBoolean(Eval("ChemistryHasAppeal")) ? "0" : "1",
                Eval("BiologyMark"), 
                Convert.IsDBNull(Eval("BiologyHasAppeal")) || !Convert.ToBoolean(Eval("BiologyHasAppeal")) ? "0" : "1",
                Eval("RussiaHistoryMark"), 
                Convert.IsDBNull(Eval("RussiaHistoryHasAppeal")) || !Convert.ToBoolean(Eval("RussiaHistoryHasAppeal")) ? "0" : "1",
                Eval("GeographyMark"), 
                Convert.IsDBNull(Eval("GeographyHasAppeal")) || !Convert.ToBoolean(Eval("GeographyHasAppeal")) ? "0" : "1",
                Eval("EnglishMark"), 
                Convert.IsDBNull(Eval("EnglishHasAppeal")) || !Convert.ToBoolean(Eval("EnglishHasAppeal")) ? "0" : "1",
                Eval("GermanMark"), 
                Convert.IsDBNull(Eval("GermanHasAppeal")) || !Convert.ToBoolean(Eval("GermanHasAppeal")) ? "0" : "1",
                Eval("FranchMark"), 
                Convert.IsDBNull(Eval("FranchHasAppeal")) || !Convert.ToBoolean(Eval("FranchHasAppeal")) ? "0" : "1",
                Eval("SocialScienceMark"), 
                Convert.IsDBNull(Eval("SocialScienceHasAppeal")) || !Convert.ToBoolean(Eval("SocialScienceHasAppeal")) ? "0" : "1",
                Eval("LiteratureMark"), 
                Convert.IsDBNull(Eval("LiteratureHasAppeal")) || !Convert.ToBoolean(Eval("LiteratureHasAppeal")) ? "0" : "1",
                Eval("SpanishMark"), 
                Convert.IsDBNull(Eval("SpanishHasAppeal")) || !Convert.ToBoolean(Eval("SpanishHasAppeal")) ? "0" : "1",
                Eval("InformationScienceMark"), 
                Convert.IsDBNull(Eval("InformationScienceHasAppeal")) || !Convert.ToBoolean(Eval("InformationScienceHasAppeal")) ? "0" : "1")
          %><%= "\n" %></ItemTemplate>
</asp:Repeater><%= "\n" %>