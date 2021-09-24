<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentUpload.aspx.cs"
    Inherits="Esrp.Web.Personal.Profile.DocumentUpload" MasterPageFile="~/Common/Templates/Main.master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript">
        postID = '<%= FileId %>';
        formUpload = '<%= FormUpload.ClientID %>';
        fileUpload = '<%= fuDocument.ClientID %>';
        redirUrl = '<%= SuccessUri %>';
    </script>
    <script src="/Common/Scripts/Prototype.js" type="text/javascript"></script>
    <script src="/Common/Scripts/JsHttpRequest.js" type="text/javascript"></script>
    <script src="/Common/Scripts/FileUpload.js" type="text/javascript"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
    <h1>
        Шаг 2 из 3: «Формирование и загрузка заявки на регистрацию»</h1>
    <br />
    <p>
        Распечатайте <a href="/Profile/DocumentForConfirmation.aspx" title="Бланк документ регистрации"> бланк заявки на регистрацию</a>, заверьте его подписями и печатями, загрузите отсканированную копию в систему.
    </p>
    <p>
        <b>Оригинал заявки также необходимо отправить по адресу 119049, Москва, Ленинский проспект, д. 2А. в ФГУ «Федеральный центр тестирования»</b>
    </p>
    <br>
    <form runat="server" id="FormUpload" method="post" enctype="multipart/form-data">
    <asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="fuDocument" EnableClientScript="false"
        Display="None" ErrorMessage="не выбран файл для загрузки" />
    <asp:CustomValidator ID="cvFileTooLarge" runat="server" EnableClientScript="false"
        Display="None" ErrorMessage="Вы пытаетесь загрузить слишком большой документ. Максимальный размер документа 10 мегабайт." />
    <div class="statement_table">
        <table width="100%">
            <% if (!String.IsNullOrEmpty(Request.QueryString["Simple"]) && !Page.IsPostBack)
               { %>
            <tr>
                <td colspan="2" class="left">
                    <span style="color: red;">При загрузке документа произошла ошибка. Пожайлуйста, попробуйте еще раз.</span>
                </td>
            </tr>
            <% } %>
            <tr>
                <th>
                    Укажите путь к отсканированной копии заверенной заявки на регистрацию
                </th>
                <td>
                    <asp:FileUpload ID="fuDocument" runat="server" CssClass="content-in wlong" /><br />
                    <small>Файл в форматах jpeg, gif, png, bmp</small>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                    <input type="submit" value="Загрузить" class="bt" <%= (!IsSimpleForm ? "onclick=\"doPost(); return false;\"" : "") %> />
                </td>
            </tr>
        </table>
    </div>
    </form>
    </div>
    <div id="TransferInfo" style="width: 300px; display: none; border: solid 1px black;
        padding: 5px;">
        <div id="TransferPercentage" style="margin-bottom: 5px; font-size: 60%;">
            0%</div>
        <div style="border: solid 1px black; background: gray; width: 100%; margin-bottom: 5px">
            <div id="ProgressBar" style="height: 5px; background: blue; width: 0%;">
            </div>
        </div>
        <div style="text-align: center;">
            <input type="button" value="Отмена" onclick="cancelTransfer()" />
        </div>
    </div>
</asp:Content>
