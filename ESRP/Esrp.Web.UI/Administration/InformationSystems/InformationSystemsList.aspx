<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="InformationSystemsList.aspx.cs" Inherits="Esrp.Web.Administration.InformationSystems.InformationSystemsList" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.HtmlControls" Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    			<div class="main_table">
					<div class="sort table_header">
                        <div class="f_left">
                            <a runat="server" id="CreateInformationSystem" title="Добавить ИС" class="create_user">Добавить ИС</a>
						</div>
                        <div class="sorted">
                            <span class="filt f_left">
                            </span>
                            <div class="sort_page">
                            </div>
                        </div>
                        <div class="clear"></div>
					</div>
					<div class="clear"></div>
        <asp:DataGrid runat="server" 
            ID="lvInformationSystems" 
            DataSourceID="odsInformationSystems" 
            AutoGenerateColumns="false"
            EnableViewState="false" 
            ShowHeader="True" 
            GridLines="None" 
            UseAccessibleHeader="true"
            Width="100%"
            CssClass="table-th">
            <HeaderStyle CssClass="actions" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="30%" />
                    <HeaderTemplate>
                        <div>
                        Короткое наименование
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href='EditInformationSystems.aspx?SystemId=<%#Eval("SystemId")%>'><%# Eval("ShortName")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>            
                <asp:TemplateColumn>
                    <HeaderStyle Width="27%" />
                    <HeaderTemplate>
                        Полное наименование
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("FullName") %>            
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        Количество групп 
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("NumberGroups") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="25%" />
                    <HeaderTemplate>
                        <div>Доступно для регистрации</div>
                    </HeaderTemplate>
                    <ItemTemplate> 
                        <%# Eval("AvailableRegistration") %>
                    </ItemTemplate>
                 </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="lvInformationSystems">
            <Message>
                <p class="notfound">
                    Информационных систем нет</p>
            </Message>
        </web:NoRecordsText>

    </div>

        <asp:ObjectDataSource ID="odsInformationSystems" runat="server" DataObjectTypeName="Esrp.Web.ViewModel.InformationSystems.InformationSystemsView"
            TypeName="Esrp.Services.InformationSystemsService" SelectMethod="SelectInformationSystems" />
    </div>
</asp:Content>

