<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckFileFormat.aspx.cs" 
    Inherits="Fbs.Web.AllUsers.BatchCheckFileFormat"
    MasterPageFile="~/Common/Templates/TestBatchCheck.Master" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="System.Configuration" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateContent">

    <form runat="server" enctype="multipart/form-data">
    
        <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
            HeaderText="<p>Произошли следующие ошибки:</p>"/>
        <table class="form f600">
            <tr><td>
                <div class="notice" id="BatchCheckSampleNotice">
                <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                <div class="cont">
                <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
                <div class="txt-in">
                    <p>
                        <b>Пример файла:</b> 
                        (<a href="/Shared/BatchBaseFileSampleNew.csv" 
                            title="скачать файл в формате csv">скачать файл</a>)
                        <pre><fbs:FileView runat="server" 
                            FilePath="/Shared/BatchBaseFileSample.csv" /></pre>
                    </p>
                    <p>
                        <b>Формат файла:</b><br />
                        <pre>
                        &lt;Регистрационный номер свидетельства&gt;%&lt;Фамилия&gt;%&lt;Имя&gt;%&lt;Отчество&gt;
                        </pre>
                    </p>
                    <p>
                    <nobr>Обязательными являются параметры: </nobr>
                    <nobr>«Регистрационный номер свидетельства», «Фамилия». </nobr>
                    </p>
            
                </div>
                </div>
                <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                </div>
                
                <p>Укажите путь к файлу с данными для проверки</p>
                <p><asp:Label ID="Label1" runat="server" Font-Size="Smaller" Text="Допускаются файлы в кодировке Windows-1251"></asp:Label></p>
                <asp:FileUpload ID="fuData" runat="server" CssClass="content-in wlong" />
            
                </td>
            </tr>
            <tr>
                <td>
                <asp:Label ID="FileErrorMsg" Visible ="false"  runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr><td class="box-submit">
		        <!--input type="submit" value="Проверить" class="bt" / -->
                
                
		 
		    </td></tr>
        </table> 

           <asp:Button ID="StartParseBtn" runat="server" Text="Проверить" onclick="StartParseBtn_click"  />
    

    
    
        <asp:RequiredFieldValidator runat="server" ControlToValidate="fuData" 
            EnableClientScript="false" Display="None"
            ErrorMessage="Не выбран файл для загрузки" /> 
            
            
            
    

        <script type="text/javascript">
            InitNotice();
        </script>


 <br/>
 
 
      <p><asp:Label ID="resultLbl" runat="server" Font-Size="Medium" Text="Результат" Visible="false"></asp:Label></p>

       </form>

</asp:Content>

