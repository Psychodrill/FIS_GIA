<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadEntrantRenunciations.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.LoadEntrantRenunciations" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
     <table class="form f600">
        <tr><td>
            <div class="notice" id="LoadEntrantRenunciationsSampleNotice">
            <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            <div class="cont">
            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
            <div class="txt-in">
                <p>
                    <b>Пример файла:</b> 
                    (<a href="/Shared/LoadEntrantRenunciationsFileSample.txt" 
                        title="скачать файл в формате csv">скачать файл</a>)
                    <pre><fbs:FileView runat="server" 
                        FilePath="/Shared/LoadEntrantRenunciationsFileSample.txt" /></pre>
                </p>
                <p>
                    <b>Формат файла:</b><br />
                    <pre>&lt;фамилия&gt;%&lt;имя&gt;%&lt;отчество&gt;%&lt;номер документа&gt;%&lt;серия документа&gt;</pre>
                </p>
                <p>Буквы Е и Ё считаются различными.</p>
            </div>
            </div>
            <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
            </div>

            <p>Укажите путь к файлу со списком</p>
            <asp:FileUpload ID="fuData" runat="server" CssClass="content-in wlong" />
        </td></tr>
        <tr><td class="box-submit">
	        <input type="submit" value="Загрузить" class="bt" />
	    </td></tr>
    </table> 
    
    <asp:RequiredFieldValidator runat="server" ControlToValidate="fuData" 
        EnableClientScript="false" Display="None"
        ErrorMessage="Не выбран файл для загрузки" /> 
        
    <asp:CustomValidator ID="cvFileEmpty" runat="server"
        ControlToValidate="fuData" ValidateEmptyText="false"
        EnableClientScript="false" Display="None"
        OnServerValidate="cvFileEmpty_ServerValidate"
        ErrorMessage="Вы пытаетесь загрузить пустой файл" />

    <asp:CustomValidator ID="cvFileFormat" runat="server"
        ControlToValidate="fuData" ValidateEmptyText="false"        
        EnableClientScript="false" Display="None"
        OnServerValidate="cvFileFormat_ServerValidate"
        ErrorMessage="{0}. Строка {1}." EnableViewState="false" />
</form>
    
<script type="text/javascript">
    InitNotice();
</script>
    
</asp:Content>
