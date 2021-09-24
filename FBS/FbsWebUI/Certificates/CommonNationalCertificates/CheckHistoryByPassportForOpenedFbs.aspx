<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckHistoryByPassportForOpenedFbs.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.CheckHistoryByPassportForOpenedFbs"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphCertificateContent" ID="cphActions">
    <form runat="server">
    <br />
    <div class="form">
        <fbs:Pager_Simple runat="server" ID="orgPager" PageSizes="5,10,20" PageSize="5" />
    </div>
    <asp:ListView runat="server" ID="historyListView" OnItemDataBound="historyListView_OnItemDataBound"
        DataSourceID="historyDataSource">
        <LayoutTemplate>
            <table class="form" runat="server" id="historyTable">
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <div style="font-weight: bold; width: 800px">
                        <asp:Literal ID="lblOrgName" runat="server" Text='<%#Eval("OrganizationFullName") %>' />
                    </div>
                    <div class="innertable">
                        <asp:GridView GridLines="None" EnableViewState="false" ID="gvChecks" ShowHeader="false"
                            AllowSorting="false" DataSource='<%# Eval("CheckEntries") %>' AutoGenerateColumns="false"
                            runat="server">
                            <Columns>
                                <asp:BoundField DataField="CheckTypeView" />
                                <asp:BoundField DataField="Date" DataFormatString="{0:HH:mm dd.MM.yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="historyDataSource" runat="server" DataObjectTypeName="FbsWebViewModel.CNEC.CNECCheckHistoryView"
        TypeName="FbsServices.CNECService" SelectMethod="SelectCNECCheckHystory" OnSelecting="historyDataSource_OnSelecting" />
    </form>
</asp:Content>
