<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="CommonNationalCertificateLoadsErrorsExport.aspx.cs" 
    Inherits="Fbs.Web.Administration.Certificates.CommonNationalCertificateLoadsErrorsExport" %>
<%@ Import Namespace="Fbs.Utility" %>
<% 
    
    // Установлю хидеры страницы
    ResponseWriter.PrepareHeaders("LoadsErrors.xls", "application/vnd.xls", 
          Encoding.GetEncoding(1251));

%>    
<html>
<body>
	<asp:DataGrid runat="server" 
	    DataSourceID="dsExport"
	    AutoGenerateColumns="false" 
	    ShowHeader="false">
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <%# Eval("Error") %> <%# GetRowIndex(Eval("RowIndex"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
	</asp:DataGrid>
</body>
</html>

<asp:SqlDataSource  runat="server" ID="dsExport" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateLoadingTaskError"
    CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <asp:QueryStringParameter Name="taskId" Type="Int64" QueryStringField="id" />
    </SelectParameters>
</asp:SqlDataSource>

<script runat="server">
    public string GetRowIndex(object index)
    {
        if (Convert.IsDBNull(index))
            return String.Empty;
        return String.Format("({0})", Convert.ToInt64(index));
    }
</script>