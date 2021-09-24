<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckPassportResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchCheckPassportResult" 
    MasterPageFile="~/Common/Templates/TestBatchCheck.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

    <div class="notice" id="BatchCheckResultSubjectNotice">
    <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
    <div class="cont">
    <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
    <div class="txt-in">
    <p><nobr>Ф - фамилия;</nobr> <nobr>И - имя; Ф - очтество;</nobr> <nobr>Серия - серия паспрта;</nobr> 
    <nobr>Номер - номер паспорта;</nobr> </p>
    <p>Коды ошибок:
    <nobr>Коды ошибок: П - неверное число полей, поля должны разделяться символом "%'; </nobr>
    <nobr>Ф - поле Фамилия должно быть заполнено и должно содержать только русские буквы; </nobr>
    <nobr>И - поле Имя должно быть пусто или содержать только русские буквы; </nobr>
    <nobr>О - поле Отчество должно быть пусто или содержать только русские буквы; </nobr>
    <nobr>СП - поле Серия паспорта должно быть заполнено; </nobr>
    <nobr>НП - поле Номер паспорта должно быть заполнено.</nobr>

    </p>
    <p>Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
    </div>
    </div>
    <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
    </div>
    

    
    <style>
        #ResultContainer {overflow-x:scroll;}
        html:first-child #ResultContainer {overflow:scroll;} /* только для Opera */
    </style>    
        <form id="FormFileFormatExport" runat="server" enctype="multipart/form-data">
    <div id="ResultContainer" style="width:100%; height:auto;">
<asp:DataGrid runat="server" id="dgResultsList"
           OnPageIndexChanged="dgResultsList_PageIndexChanged"
            AutoGenerateColumns="false" 
            EnableViewState="true"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th-border table-th"
      
             AllowPaging="True" PagerStyle-Position="Top" PagerStyle-HorizontalAlign="Left" PagerStyle-Mode="NumericPages" PagerStyle-ForeColor="#0066FF">
        <HeaderStyle CssClass="th" />
      <Columns>
         <asp:TemplateColumn>
            <HeaderStyle CssClass="left-th" />
            <HeaderTemplate>
                <div title="Номер строки">№</div>
            </HeaderTemplate>
            <ItemStyle HorizontalAlign="Left" />
               <ItemTemplate>
                <%# HighlightValue(Eval("RowIndex"))%>
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
               
            </ItemTemplate>
            
            </asp:TemplateColumn>
 
                
            <asp:TemplateColumn>
            <HeaderTemplate>
              <span title="Фамилия">Фамилия</span>
            </HeaderTemplate>
                 <ItemTemplate>
                   <%# HighlightValue(Eval("Фамилия"))%>
            </ItemTemplate>
          
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
              <span title="Имя">Имя</span>
            </HeaderTemplate>
             <ItemTemplate>
                  <%# HighlightValue(Eval("Имя"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
              <HeaderTemplate>
              <span title="Отчество">Отчество</span>
            </HeaderTemplate>
             <ItemTemplate>
                 <%# HighlightValue(Eval("Отчество"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
                <asp:TemplateColumn>
               <HeaderTemplate>
              <span title="Серия паспорта">СП</span>
            </HeaderTemplate>
             <ItemTemplate>
                  <%# HighlightValue(Eval("Серия паспорта"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
                <HeaderTemplate>
              <span title="Номер паспорта">НП</span>
            </HeaderTemplate>
             <ItemTemplate>
                  <%# HighlightValue(Eval("Номер паспорта"))%>
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
                 <%# HighlightValue(Eval("Комментарий"))%>
            </ItemTemplate>
         
         
            </asp:TemplateColumn>                                                         
        </Columns>
    </asp:DataGrid>
    </div>
    </form>

    <web:NoRecordsText runat="server" ControlId="dgResultsList">
        <Message><style type="text/css">#ExportContainer, #ResultContainer {display:none;}</style> 
        <p class="notfound">Не обработано ни одной записи</p></Message>
    </web:NoRecordsText>    
    
    

 

    <script type="text/javascript">
        InitNotice();
    </script>
</asp:Content>

<script runat="server">
    public string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.Contains("[НЕВЕРЕН ФОРМАТ]"))
            return String.Format("<span \" style=\"color:Red\">{0}</span>",
                value);
        else
            return value;

    }
</script>