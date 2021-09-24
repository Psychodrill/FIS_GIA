<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryCertificate.ascx.cs" Inherits="Fbs.Web.Controls.CommonNationalCertificates.HistoryCertificate" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Panel runat="server" ID="pHistoryCertificate">
    <h1>Другие свидетельства</h1>
    <asp:GridView runat="server" ID="gvCertificateList" AutoGenerateColumns="false"
        EnableViewState="false" DataSourceID="odsCertificateList" GridLines="None" CssClass="table-th" >
        <HeaderStyle CssClass="th" >
        </HeaderStyle>
        <Columns>
            <%-- Ссылка на свидетельство --%>
            <asp:TemplateField>
                <HeaderStyle CssClass="left-th" />
                <HeaderTemplate>
                    <div>Свидетельство</div>
                </HeaderTemplate>
			    <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%#Eval("Certificate.Url") %>' Text='<%#Eval("Certificate.Text") %>'></asp:HyperLink>
			    </ItemTemplate>
		    </asp:TemplateField>
            <%-- Год --%>
            <asp:TemplateField HeaderText="Год">
			    <ItemTemplate>
                    <%#Eval("Year") %>
			    </ItemTemplate>
		    </asp:TemplateField>
            <%-- Статус --%>
            <asp:TemplateField>
                <HeaderStyle CssClass="right-th" />
                <HeaderTemplate>
                    <div>Статус</div>
                </HeaderTemplate>
			    <ItemTemplate>
                    <%#Eval("Status") %>
			    </ItemTemplate>
		    </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:ObjectDataSource runat="server"
        ID="odsCertificateList"
        TypeName="FbsServices.CNECService"
        DataObjectTypeName="FbsWebViewModel.CNEC.HistoryCertificateView"
        SelectMethod="GetCertificateForUser"
        OnSelecting="OnSelectingCertificateList"
        OnSelected="OnSelectedCertificateList">
    </asp:ObjectDataSource>
</asp:Panel>
