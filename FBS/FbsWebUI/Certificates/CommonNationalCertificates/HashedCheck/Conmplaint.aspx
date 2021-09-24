<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Conmplaint.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.Conmplaint"
    MasterPageFile="~/Common/Templates/HashedCertificates.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form id="Form1" runat="server" defaultbutton="btnSend">
    <div id="DInputs">
        <br />
        <table style="width: 620px;">
            <tr>
                <td>
                    <div>
                        <table class="form">
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    Фамилия участника ЕГЭ
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TBLastName" CssClass="txt" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    Имя участника ЕГЭ:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TBFirstName" CssClass="txt" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    Отчество участника ЕГЭ:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TBPatronymicName" CssClass="txt" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <script type="text/javascript">
                            //IntiInputWithDefaultValue("TBLastName", "cLastName");
                            //IntiInputWithDefaultValue("TBFirstName", "cFirstName");
                            //IntiInputWithDefaultValue("TBPatronymicName", "cPatronymicName");
                        </script>
                    </div>
                </td>
                <td>
                    <div id="secondColumn">
                        <table class="form">
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    ОУ:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TBEducationPlace" CssClass="txt" TextMode="MultiLine"
                                        Height="60px" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" nowrap="nowrap">
                                    № проверки:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="TBRequestNumber" CssClass="txt" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="t-line">
                    <table>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Фамилия:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillLastName" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Имя:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillFirstName" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Отчество:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillPapaName" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Индекс и почтовый адрес:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillAddress" CssClass="txt" Width="200px" TextMode="MultiLine" Height="60px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Факс:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillFax" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Email:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillEmail" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Телефон:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillPhone" CssClass="txt" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                Комментарий:
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" ID="TBFillComment" CssClass="txt" Width="400px" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="t-line">
                    <asp:Button runat="server" ID="btnReset" Text="Очистить" CssClass="bt" />
                    <asp:Button runat="server" ID="btnSend" Text="Отправить" CssClass="bt" OnClientClick="ProcessPage();"
                        OnClick="btnSend_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
