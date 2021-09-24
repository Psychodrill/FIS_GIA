<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master" AutoEventWireup="true" CodeBehind="CertificateListCommonDetails.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.CertificateListCommonDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"> </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <div class="notice" id="RequestByPassportResult">
        <div class="top">
            <div class="l">
            </div>
            <div class="r">
            </div>
            <div class="m">
            </div>
        </div>
        <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                x<span></span></div>
            <div class="txt-in">
                <p>
                    Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство.
                <br />
                    Срок действия результатов ЕГЭ определяется в соответствии с действующими нормативными документами.
                <br />
                    В 2015 году действительны результаты ЕГЭ 2012, 2013, 2014 и 2015 годов.
                <br />
Результаты 2011 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы.
<br />
Специальным знаком &laquo;!&raquo; перед баллом выделены баллы, которые меньше минимальных установленных Рособрнадзором значений.
<br />
Аннулированные и недействительные результаты ЕГЭ отображаются как 0 баллов.</p>
            </div>
        </div>
        <div class="bottom">
            <div class="l">
            </div>
            <div class="r">
            </div>
            <div class="m">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        InitNotice();
    </script>
   <style>
        #ResultContainer
        {
            overflow-x: auto;
        }
        html:first-child #ResultContainer
        {
            overflow: auto;
        }
        /* только для Opera */
        #ResultContainer td
        {
            white-space: nowrap;
        }
    </style>
    <form id="Form1" runat="server">
    <div id="ResultContainer" style="width: 100%; height: auto;">
        <asp:Panel runat="server" ID="searchDetails">
        <table width="100%">
            <tr>
                <td width="75px" style="font-weight: bold; white-space: nowrap">
                    Фамилия:
                </td>
                <td style="white-space: nowrap">
                    <asp:Label runat="server" ID="resultField_Surname"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold; white-space: nowrap">
                    Имя:
                </td>
                <td style="white-space: nowrap">
                    <asp:Label runat="server" ID="resultField_Name"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold; white-space: nowrap">
                    Отчество:
                </td>
                <td style="white-space: nowrap">
                    <asp:Label runat="server" ID="resultField_SecondName"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold; white-space: nowrap">
                    Документ:
                </td>
                <td style="white-space: nowrap">
                    <asp:Label runat="server" ID="resultField_DocumentSeries"></asp:Label>&nbsp;<asp:Label
                        runat="server" ID="resultField_DocumentNumber"></asp:Label>
                </td>
            </tr>
        </table>
        
            <div style="white-space: nowrap;margin-bottom: 16px;">
                <asp:HyperLink ID="printNoteLink" runat="server" Target="_blank" Text="Распечатать справку"></asp:HyperLink>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:HyperLink runat="server" ID="backToResultsLink" Text="Назад к списку проверок"></asp:HyperLink>
            </div>

        </asp:Panel>
        <asp:DataGrid runat="server" ID="searchDetailsGrid" AutoGenerateColumns="False"
            GridLines="None" CssClass="table-th" OnItemDataBound="dgView_RowBound" 
            EnableViewState="False">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="left-th"></HeaderStyle>
                    <HeaderTemplate>
                        <div>Предмет</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.SubjectName") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Балл">
                    <ItemTemplate>
                        <%# RenderMark(Container) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="UseYear" HeaderText="Год" ReadOnly="True" />
                <asp:BoundColumn DataField="RegionName" HeaderText="Регион" ReadOnly="True" />
                <asp:BoundColumn DataField="StatusName" HeaderText="Статус" ReadOnly="True" />
                <asp:TemplateColumn HeaderText="Апелляция/Перепроверка">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.HasAppeal", "{0}") == Boolean.TrueString ? "Да" : "Нет")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="LicenseNumber" HeaderText="Номер свидетельства" ReadOnly="True" />
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="right-th"></HeaderStyle>
                    <HeaderTemplate>
                        <div>ТН</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.TypographicNumber") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        
    </div>
    
    <asp:Panel ID="noResultPanel" runat="server" Visible="False">
            <p align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                По Вашему запросу ничего не найдено
             </p>
           <asp:HyperLink ID="noResultLink" runat="server" Target="_blank" Text="Распечатать" />
        </asp:Panel>
</form>
</asp:Content>
