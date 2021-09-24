<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/HashedCertificates.Master" AutoEventWireup="true" CodeBehind="BatchCheckByMarkSum.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.BatchCheckByMarkSum" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">
   
 <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

   
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

    <script type="text/javascript" src="/Common/Scripts/sha1.js"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCertificateActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server">
<asp:ValidationSummary runat="server" />

<asp:Panel ID="inputPanel" runat="server" style="margin-top:15px;margin-left:10px;border:1px solid gray; border-radius:8px;padding:10px"> 
    <div id="typeSelector" style="margin-bottom:10px">
        <asp:RadioButtonList runat="server" ID="rblTypeSelector"  RepeatLayout="Flow">
        <asp:ListItem Selected="True" Text="По ФИО и сумме баллов" Value="4"/>
        <asp:ListItem Text="По ФИО и баллам по предметам" Value="3" />
        <asp:ListItem Text="По ФИО и предметам" Value="5"/>

    </asp:RadioButtonList>
    </div>

    <div id="bySum">
        <asp:FileUpload runat="server" ID="fuUploader" />
    </div>
   
    <asp:Button style="margin-top:5px" CssClass="button" runat="server" OnClick="UploadChecks" Text="Проверить" />
     <asp:DataGrid runat="server" id="dgReportResultsList"
                          OnPageIndexChanged="dgReportResultsList_PageIndexChanged"
                          AutoGenerateColumns="false" 
                          EnableViewState="true"
                          ShowHeader="True" 
                          GridLines="None"
                          CssClass="table-th-border table-th" >
                <HeaderStyle CssClass="th" />
                <Columns>
                    <asp:TemplateColumn>
                        <HeaderStyle CssClass="left-th" />
                        <HeaderTemplate>
                            <div title="Номер строки">№</div>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%# this.HighlightValue(this.Eval("RowIndex"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    
 
                
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <span title="Фамилия">Фамилия</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# this.HighlightValue(this.Eval("ФИО"))%>
                        </ItemTemplate>
          
                    </asp:TemplateColumn>

                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <span title="Сумма баллов">СБ</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# this.HighlightValue(this.Eval("Сумма баллов"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
            
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <span title="Предметы">П</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# this.HighlightValue(this.Eval("Предмет"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
         
                    <asp:TemplateColumn>
            
                        <HeaderStyle CssClass="right-th" />
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderTemplate>
                            <div title="Комментарий">Комментарий</div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# this.HighlightValue(this.Eval("Комментарий"))%>
                        </ItemTemplate>
         
         
                    </asp:TemplateColumn>                                                         
                </Columns>
            </asp:DataGrid>
    <asp:CustomValidator runat="server" ErrorMessage="Выберите файл для загрузки" Display="None" OnServerValidate="ValidateInputFiles" />
    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Ошибка при валидации файла" Display="None" OnServerValidate="ValidateInputFiles" />
    <script runat="server">
        public string HighlightValue(object valueObj)
        {
            string value = Convert.ToString(valueObj);
            if (value.Contains("[НЕВЕРЕН ФОРМАТ]"))
                return String.Format(
                    "<span \" style=\"color:Red\">{0}</span>",
                    value);
            else
                return value;

        }
    </script>
</asp:Panel>

</form>
</asp:Content>
