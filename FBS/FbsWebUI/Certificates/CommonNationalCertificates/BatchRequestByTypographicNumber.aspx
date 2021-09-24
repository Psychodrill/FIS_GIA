<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByTypographicNumber.aspx.cs" 
         Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByTypographicNumber" 
         MasterPageFile="~/Common/Templates/Certificates.Master" %>
<%@ Import Namespace="Fbs.Core.Organizations" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheckCommon" Src="~/Controls/CommonNationalCertificates/HistoryCheckListCommon.ascx" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"> </script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
            <ul>
                <li><a href="<%=this.Request.Url.PathAndQuery%>" 
                       title="Обновить страницу">Обновить результаты</a></li>
            </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>    
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateContent">
        
    <form runat="server" enctype="multipart/form-data">
     
        <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
                               HeaderText="<p>Произошли следующие ошибки:</p>"/>

        <table class="form f600">
            <tr><td>
                    <div class="notice" id="BatchRequestByTypographicNumberSampleNotice">
                        <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                        <div class="cont">
                            <div dir="ltr" class="hide" title="Свернуть" onclick=" ToggleNoticeState(this); ">x<span></span></div>
                            <div class="txt-in">            
                                <p>
                                    <b>Пример файла:</b>
                                    (<a href="/Shared/BatchFileTypeNumberSample.csv" 
                                        title="скачать файл в формате csv">скачать файл</a>)
                                    <pre><fbs:FileView runat="server" 
                                                       FilePath="/Shared/BatchFileTypeNumberSample.csv" /></pre>
                                </p>
                                <p>
                                    <b>Формат файла:</b><br />
                                    <pre>
                        &lt;типографский&nbsp;номер&gt;%&lt;фамилия&gt;%&lt;имя&gt;%&lt;отчество&gt;
                        </pre>
                                </p>
                                <%--<p>Буквы Е и Ё считаются различными.</p>--%>
                                <p>Поиск и проверка свидетельств осуществляются по строгому соответствию параметров запроса «Типографский номер», «Фамилия», «Имя», «Отчество» параметрам, хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;.
                                    Поля «Типографский номер», «Фамилия» обязательны для заполнения.</p>    
                            </div>
                        </div>
                        <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                    </div>

                    <p>Укажите путь к файлу с данными для запроса</p>
                    <asp:FileUpload ID="fuData" runat="server" CssClass="content-in wlong" />
                </td></tr>
            <tr><td class="box-submit">
                    <asp:Button ID="StartParseBtn" runat="server" Text="Проверить" 
                                onclick="StartParseBtn_Click"/>
                </td></tr>
            <tr>
                <td>
                    <asp:Label ID="FileErrorMsg" Visible ="false"  runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table> 

        <style>
            #ResultContainer {overflow-x:auto;}
            html:first-child #ResultContainer {overflow:auto;} /* только для Opera */
            #ResultContainer td {white-space:nowrap;}
        </style>    
    
        <div id="ResultContainer" style="width:100%; height:auto; margin-bottom: 25px;">
        <asp:DataGrid runat="server" id="dgReportResultsList"
            OnPageIndexChanged="dgReportResultsList_PageIndexChanged"
            AutoGenerateColumns="false" 
            EnableViewState="true"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th-border table-th">
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
                        <div title="Номер свидетельства">Cв-во</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
            
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div title="Типографский номер">ТН</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# this.HighlightValue(this.Eval("Типографский номер"))%> 
                    </ItemTemplate>
            
                </asp:TemplateColumn>
            
                       
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Фамилия">Фамилия</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# this.HighlightValue(this.Eval("Фамилия"))%>
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Имя">Имя</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# this.HighlightValue(this.Eval("Имя"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Отчество">Отчество</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# this.HighlightValue(this.Eval("Отчество"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Серия паспорта">СП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
           
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Номер паспорта">НП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Регион">Регион</span>
                    </HeaderTemplate>
                    <ItemTemplate>
              
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Год выдачи свидетельства">Год</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Статус">Статус</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Русский язык">РЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по русскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
              
                    </ItemTemplate>
     
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Математика">М</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по математике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
           
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Физика">Ф</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>
           
         
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по физике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
           
                    </ItemTemplate>
           
                </asp:TemplateColumn>
                            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Химия">Х</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
         
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по химии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>            
                </asp:TemplateColumn>

                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Биология">Б</span>
                    </HeaderTemplate>
                    <ItemTemplate>
        
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по биологии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
          
                    </ItemTemplate>
         
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="История России">ИР</span>
                    </HeaderTemplate>
                    <ItemTemplate>
       
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по истории России">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
           
                    </ItemTemplate>
            
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="География">Г</span>
                    </HeaderTemplate>
                    <ItemTemplate>
    
                    </ItemTemplate>
          
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по географии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
              
                    </ItemTemplate>
          
                </asp:TemplateColumn>

                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Английский язык">АЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по английскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
           
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Немецкий язык">НЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>
           
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по немецкому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>
           
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Французский язык">ФЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
      
                    </ItemTemplate>
         
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по французскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
           
                    </ItemTemplate>
         
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Обществознание">О</span>
                    </HeaderTemplate>
                    <ItemTemplate>
            
                    </ItemTemplate>
           
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по обществознанию">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
         
                    </ItemTemplate>
                </asp:TemplateColumn>
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Литература">Л</span>
                    </HeaderTemplate>
                    <ItemTemplate>
  
                    </ItemTemplate>
                </asp:TemplateColumn>  
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по литературе">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
     
                    </ItemTemplate>
                </asp:TemplateColumn> 
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Испанский язык">ИЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
      
                    </ItemTemplate>
           
                </asp:TemplateColumn>   
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по испанскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
      
                    </ItemTemplate>
           
                </asp:TemplateColumn>                                                         
  
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Информатика">И</span>
                    </HeaderTemplate>
                    <ItemTemplate>
      
                    </ItemTemplate>
                </asp:TemplateColumn>   
            
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <div title="Апелляция по информатике">Ап</div>
                    </HeaderTemplate>
                    <ItemTemplate>
      
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
        </div>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="fuData" 
                                    EnableClientScript="false" Display="None"
                                    ErrorMessage="Не выбран файл для загрузки" />

        <asp:CustomValidator ID="cvFileEmpty" runat="server"
                             ControlToValidate="fuData" ValidateEmptyText="false"
                             EnableClientScript="false" Display="None"
                             OnServerValidate="cvFileEmpty_ServerValidate"
                             ErrorMessage="Вы пытаетесь загрузить пустой файл" />

        <asp:CustomValidator ID="cvFileTooLarge" runat="server"
                             ControlToValidate="fuData" ValidateEmptyText="false"        
                             EnableClientScript="false" Display="None"
                             OnServerValidate="cvFileTooLarge_ServerValidate"
                             ErrorMessage="Вы пытаетесь загрузить слишком большой файл. Максимальный размер файла {0} мегабайт." />
            
        <asp:CustomValidator ID="cvFileFormat" runat="server"
                             ControlToValidate="fuData" ValidateEmptyText="false"        
                             EnableClientScript="false" Display="None"
                             OnServerValidate="cvFileFormat_ServerValidate"
                             ErrorMessage="Ошибка в данных файла. Строка {0}." EnableViewState="false" />
                    
        <fbs:HistoryCheckCommon runat="server" ID="historyCheckCommon" CheckType="TypographicNumber" CheckMode="Batch" ShowDeleteHistoryLink="True" />
        
    </form>
</asp:Content>

<script runat="server" type="text/C#">

    
    public string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.Contains("[НЕВЕРЕН ФОРМАТ]"))
            return String.Format(
                "<span \" style=\"color:Red\">{0}</span>"
                ,
                value);
        else
            return value;

    }
</script>