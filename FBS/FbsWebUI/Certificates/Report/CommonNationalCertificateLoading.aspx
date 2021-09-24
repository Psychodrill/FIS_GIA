<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommonNationalCertificateLoading.aspx.cs"
    Inherits="Fbs.Web.Certificates.Report.CommonNationalCertificateLoading" MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%--Так как вывод очень сильно нагружает базу данных, то кэшируем эту страницу как минимум на 10 минут.--%>
<%@ OutputCache Duration="600" VaryByParam="none" %>
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    Дата&nbsp;последнего&nbsp;обновления:&nbsp;<asp:Label ID="LLastUpdate" runat="server"></asp:Label><br />
    <br />
    <asp:DataGrid runat="server" ID="dgResultsList" DataSourceID="dsResultsList" AutoGenerateColumns="false"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th"
        Width="50%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="left-th" />
                <HeaderTemplate>
                    <div title="Код региона">
                        Код</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <nobr><%# Eval("[Код региона]")%></nobr>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="40%" />
                <HeaderTemplate>
                    <div>
                        Регион</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <nobr><%# Eval("[Регион]")%></nobr>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Число загруженных сертификатов за 2008-ой год">
                        <nobr>2008</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств за 2008]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Число загруженных сертификатов за 2009-ой год">
                        <nobr>2009</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств за 2009]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Число загруженных сертификатов за 2010-ой год">
                        <nobr>2010</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств за 2010]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Число загруженных сертификатов за 2011-ой год">
                        <nobr>2011</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств за 2011]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Число загруженных сертификатов за 2012-ой год">
                        <nobr>2012</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств за 2012]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="right-th" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div title="Общее число загруженных сертификатов">
                        <nobr>Все</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("[Всего свидетельств]")%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <asp:ObjectDataSource runat="server" ID="dsResultsList" 
        TypeName="FbsServices.ReportingService" SelectMethod="ReportCertificateLoadShortTVF" >
    </asp:ObjectDataSource>
</asp:Content>
