<%@ Control Language="C#" Inherits="ViewUserControl<OlympicDiplomantEditViewModel>" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Data.Model" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>
<%@ Import Namespace="GVUZ.Data.Helpers" %>



<div>
    <h4 class="text-info">Сведения о результатах поиска в базе участников ЕГЭ</h4>
</div>

<div>
    <% 
                string StatusStr = ""; 
                if(Model.Data.OlympicDiplomantStatus != null)
                    StatusStr = Model.Data.OlympicDiplomantStatus.Name;

                string BirthDayStr = ""; 
                if(Model.Data.RVIPersons.BirthDay != null)
                    BirthDayStr = Model.Data.RVIPersons.BirthDay.Value.ToShortDateString();

                string DocStr =  "";
                string NumStr =  "";
                string SerStr =  "";
                if(Model.Data.RVIPersons.RVIPersonIdentDocs != null && Model.Data.RVIPersons.RVIPersonIdentDocs.FirstOrDefault() != null)
                {
                    DocStr = Model.Data.RVIPersons.RVIPersonIdentDocs.FirstOrDefault().RVIDocumentTypes.DocumentTypeName;
                    SerStr = Model.Data.RVIPersons.RVIPersonIdentDocs.FirstOrDefault().DocumentSeries;
                    NumStr = Model.Data.RVIPersons.RVIPersonIdentDocs.FirstOrDefault().DocumentNumber;
                }

                string participantStr =  "";
                if(Model.Data.PersonId != null)
                {
                    participantStr = "ФИО: " + Model.Data.RVIPersons.NormSurname + " " + 
                                     Model.Data.RVIPersons.NormName + " " +
                                     Model.Data.RVIPersons.NormSecondName  + "; "+
                                     "ДР: " + BirthDayStr + "; "+
                                     "Документ: " + DocStr + "; серия: " + SerStr + " номер: " + NumStr;
                }

    %>


    <p>Статус: <%= StatusStr %></p>
    <button type="button" id="calcButton">Запустить повторно</button>


    <% if (Model.Data.StatusID < 3) {%>
                
    <div id="detailInfo1">
        <p>PersonId: <%= Model.Data.PersonId %></p>
        <p>Участник ЕГЭ: <%= participantStr %></p>
    </div>
				
    
    <%} else {%> 


    <div id="detailInfo2">
        <%=Html.LabelFor(m => m.Data.AdoptionUnfoundedComment)%>:
        <%= Html.TextBoxExFor(m => m.Data.AdoptionUnfoundedComment, new { @id="editAdoptionUnfoundedComment"})%>
    </div>

    <%}%> 

</div>

