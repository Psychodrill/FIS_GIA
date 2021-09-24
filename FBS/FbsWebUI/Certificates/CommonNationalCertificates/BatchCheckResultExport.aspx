<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckResultExport.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchCheckResultExport" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Utility" %>
<% 
    
    // Установлю хидеры страницы
    ResponseWriter.PrepareHeaders("BatchRequestResult.xls", "application/vnd.xls", 
          Encoding.GetEncoding(1251));

%>
<html>
<body>
    <asp:DataGrid runat="server"
        DataSourceID="dsResultsList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True">
            
        <Columns>
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Cвидетельство</div>
            </HeaderTemplate>
            <ItemStyle HorizontalAlign="Center" />
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsDeny")) ? string.Format("<span style=\"color:Red\" title='Свидетельство №{0} аннулировано по следующей причине: {1}'>{0} (аннулировано)</span>", Eval("CertificateNumber"), Convert.ToString(Eval("DenyComment"))) :
                    (!Convert.ToBoolean(Eval("IsExist")) ? string.Format("<span style=\"color:Red\" title='Свидетельство №{0} не найдено'>{0} (не&nbsp;найдено)</span>", Eval("CertificateNumber")) : Eval("CertificateNumber"))%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Фамилия</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("LastName"), Eval("CheckLastName"), Eval("LastNameIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>Имя</HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("FirstName"), Eval("CheckFirstName"), Eval("FirstNameIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>Отчество</HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("PatronymicName"), Eval("CheckPatronymicName"), Eval("PatronymicNameIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <nobr>РУССКИЙ ЯЗЫК</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("RussianMark"), Eval("RussianCheckMark"), Eval("RussianMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("RussianHasAppeal")) ? string.Empty : 
                    (!Convert.IsDBNull(Eval("RussianHasAppeal")) && Convert.ToBoolean(Eval("RussianHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                МАТЕМАТИКА
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("MathematicsMark"), Eval("MathematicsCheckMark"), Eval("MathematicsMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("MathematicsHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("MathematicsHasAppeal")) && Convert.ToBoolean(Eval("MathematicsHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                ФИЗИКА
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("PhysicsMark"), Eval("PhysicsCheckMark"), Eval("PhysicsMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("PhysicsHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("PhysicsHasAppeal")) && Convert.ToBoolean(Eval("PhysicsHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
                            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                ХИМИЯ
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("ChemistryMark"), Eval("ChemistryCheckMark"), Eval("ChemistryMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("ChemistryHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("ChemistryHasAppeal")) && Convert.ToBoolean(Eval("ChemistryHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                БИОЛОГИЯ
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("BiologyMark"), Eval("BiologyCheckMark"), Eval("BiologyIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("BiologyHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("BiologyHasAppeal")) && Convert.ToBoolean(Eval("BiologyHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <nobr>ИСТОРИЯ РОССИИ</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("RussiaHistoryMark"), Eval("RussiaHistoryCheckMark"), Eval("RussiaHistoryMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("RussiaHistoryHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("RussiaHistoryHasAppeal")) && Convert.ToBoolean(Eval("RussiaHistoryHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                ГЕОГРАФИЯ
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("GeographyMark"), Eval("GeographyCheckMark"), Eval("GeographyMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("GeographyHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("GeographyHasAppeal")) && Convert.ToBoolean(Eval("GeographyHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <nobr>АНГЛИЙСКИЙ ЯЗЫК</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("EnglishMark"), Eval("EnglishCheckMark"), Eval("EnglishMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("EnglishHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("EnglishHasAppeal")) && Convert.ToBoolean(Eval("EnglishHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <nobr>НЕМЕЦКИЙ ЯЗЫК</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("GermanMark"), Eval("GermanCheckMark"), Eval("GermanMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("GermanHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("GermanHasAppeal")) && Convert.ToBoolean(Eval("GermanHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <nobr>ФРАНЦУЗСКИЙ ЯЗЫК</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("FranchMark"), Eval("FranchCheckMark"), Eval("FranchMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("FranchHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("FranchHasAppeal")) && Convert.ToBoolean(Eval("FranchHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                ОБЩЕСТВОЗНАНИЕ
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("SocialScienceMark"), Eval("SocialScienceCheckMark"), Eval("SocialScienceMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("SocialScienceHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("SocialScienceHasAppeal")) && Convert.ToBoolean(Eval("SocialScienceHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                ЛИТЕРАТУРА
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("LiteratureMark"), Eval("LiteratureCheckMark"), Eval("LiteratureMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>  
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                Апелляция
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("LiteratureHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("LiteratureHasAppeal")) && Convert.ToBoolean(Eval("LiteratureHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
               <nobr>ИСПАНСКИЙ ЯЗЫК</nobr>
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("SpanishMark"), Eval("SpanishCheckMark"), Eval("SpanishMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>   
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <div>Апелляция</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("SpanishHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("SpanishHasAppeal")) && Convert.ToBoolean(Eval("SpanishHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>                                                         
  
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
               ИНФОРМАТИКА
            </HeaderTemplate>
            <ItemTemplate>
                <%# HighlightValue(Eval("InformationScienceMark"), Eval("InformationScienceCheckMark"), Eval("InformationScienceMarkIsCorrect"))%>
            </ItemTemplate>
            </asp:TemplateColumn>   
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <div>Апелляция&nbsp;</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("InformationScienceHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("InformationScienceHasAppeal")) && Convert.ToBoolean(Eval("InformationScienceHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
            </ItemTemplate>
            </asp:TemplateColumn>                                                         
        </Columns>
    </asp:DataGrid>
</body>
</html>

<asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateCheck"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters>
</asp:SqlDataSource>

<script runat="server">
    public string HighlightValue(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        string checkValue = Convert.ToString(checkValueObj);
        checkValue = String.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
        value = String.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

        return String.Format("<span style=\"color:Red\">Ошибка: {0}</span> ({1})",
            checkValue, value);
    }
</script>