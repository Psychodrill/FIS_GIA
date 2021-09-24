<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.UniDocuments.UniDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Model.Entrants.UniDocuments" %>
<style>
    .ui-dialog, .ui-dialog-content { overflow: visible !important;}
    tr.spaceUnder > td { padding-bottom: 0.5em; }
    .validation-error { BORDER: #d70200 2px solid !important; }
}
    .auto-style1 {
        height: 30px;
    }
    .auto-style3 {
        width: 60%;
        height: 27px;
    }
    .auto-style4 {
        width: 25%;
        height: 27px;
    }
    .auto-style5 {
        width: 40px;
        height: 27px;
    }
    .auto-style6 {
        width: 103px;
    }

    .auto-style8 {
        width: 290px;
    }
    .auto-style10 {
        height: 23px;
    }
    .auto-style11 {
        width: 306px;
    }

</style>
<table id="UniDForm" class="data">
    <thead>
        <tr>
            <th class="caption"></th>
            <th colspan="4"></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(m => m.UniD.DocumentTypeID)%></td>
            <td colspan="4"><b><%= Model.UniD.DocumentTypeName%></b>
                <%=Html.Hidden("UniD_DocumentTypeID",   Model.UniD.DocumentTypeID)%>
                <%=Html.Hidden("UniD_EntrantID",        Model.UniD.EntrantID)%>
                <%=Html.Hidden("UniD_EntrantDocumentID",Model.UniD.EntrantDocumentID)%>
            </td>
        </tr>

        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentName", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption"><%= Html.TableLabelReqFor(m => m.UniD.DocumentName)%></td>
            <td colspan="4"><%= Html.TextBoxExFor(m => m.UniD.DocumentName)%></td>
        </tr>
        <% } %>
        
        <tr>
            <td class="caption"><%= Html.TableLabelReqFor(m => m.UniD.UID)%></td>
            <td colspan="4"><%= Html.TextBoxExFor(m => m.UniD.UID)%></td>
        </tr>

        <% if (Model.UniD.EntDocIdentity != null) { %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("LastName", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.LastName)%> <span class="required">*</span>
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocIdentity.LastName)%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("FirstName", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.FirstName)%> <span class="required">*</span>
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocIdentity.FirstName)%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("MiddleName", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.MiddleName)%> 
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocIdentity.MiddleName)%>
                </td>
            </tr>
            <% } %>
        <% } %>



        <% if (EntrantDocumentMatrix.isFieledInDoc("IdentityDocumentTypeID", Model.UniD.DocumentTypeID))
            { %>
        <% if (Model.UniD.EntDocIdentity != null)
            { %>
        <% if (Model.UniD.EntDocIdentity.IdentityDocumentTypeEdit)
            { %>
        <tr>
            <td class="caption"><%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.IdentityDocumentTypeID)%></td>
            <td colspan="4">
                <%= Html.DropDownListExFor(m => m.UniD.EntDocIdentity.IdentityDocumentTypeID,Model.UniD.EntDocIdentity.IdentityDocumentList, new { onchange="UniD_identityDocumentTypeChanged()" })%>
            </td>
        </tr>
        <% }
            else
            { %>
        <tr>
            <td class="auto-style10"><%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.IdentityDocumentTypeID)%></td>
            <td colspan="4" class="auto-style10"><b><%= Model.UniD.EntDocIdentity.IdentityDocumentTypeName%></b></td>
        </tr>
        <% } %>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentSeries", Model.UniD.DocumentTypeID) && EntrantDocumentMatrix.isFieledInDoc("DocumentNumber", Model.UniD.DocumentTypeID))
            { %>
        <tr class="spaceUnder">
            <% if (EntrantDocumentMatrix.getFieledForDoc("DocumentNumber", Model.UniD.DocumentTypeID) == 2)
                {%>
            <%  if(Model.UniD.DocumentTypeID==9){ %>
            <td class="caption"><%= Html.TableLabelFor(m => m.UniD.DocumentSeriesNumber)%></td>
            <%}else {%>
            <td class="caption" colspan="1"><%= Html.TableLabelReqFor(m => m.UniD.DocumentSeriesNumber)%></td>
            <%}%>
            <%}
                else
                { %>
            <td class="auto-style6"><%= Html.TableLabelFor(m => m.UniD.DocumentSeriesNumber)%></td>
            <%} %>
            <td class="auto-style11">
                <%= Html.TextBoxExFor(m => m.UniD.DocumentSeries,new { @class="passSeries" ,@style="width:70px;"})%>
                <%= Html.TextBoxExFor(m => m.UniD.DocumentNumber,new { @class="passNumber", @style="width:120px;" })%>
                <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicCheck", Model.UniD.DocumentTypeID))
                    { %>
                <% if (Model.UniD.DocumentTypeID == 9 || Model.UniD.DocumentTypeID == 10)
                    {%>
                <a href="#" id="btnGetOlympicCheck" onclick="CheckAndGet()">Проверить и получить</a>
                <%}%>
                <%} %>
            </td>
        </tr>
        <% }
            else
            { %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("Number", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <% if (EntrantDocumentMatrix.getFieledForDoc("Number", Model.UniD.DocumentTypeID) == 2)
                {%>
            <td class="caption"><%= Html.TableLabelReqFor(m => m.UniD.Number)%></td>
            <%}
                else
                { %>
            <td class="caption" colspan="1">
                <% if (Model.UniD.DocumentTypeID == 18)
                    {%>
                <%= Html.TableLabelReqFor(m => m.UniD.Number)%>
                <%  }
                    else
                    {%>
                <%= Html.TableLabelFor(m => m.UniD.Number)%>
                <%}%>
            </td>
            <%} %>
            <td class="auto-style6"><%= Html.TextBoxExFor(m => m.UniD.Number)%></td>
        </tr>
        <% } %>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateNumber", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption"><%= Html.TableLabelReqFor(m => m.UniD.CertificateNumber)%></td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.CertificateNumber, new {style = "width:120px"})%>
                <a href="#" id="btnGetEge" onclick="btnGetEgeDocument()">Получить данные по номеру</a>
            </td>
        </tr>
        <% } %>

        <% if (Model.UniD.EntDocEge != null) { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("TypographicNumber", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEge.TypographicNumber) %>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEge.TypographicNumber, new {style = "width:100px"}) %>
            </td>
        </tr>
        <% } %>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentDate", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.DocumentDate)%>
            </td>
            <td colspan="4">
                <%= Html.DatePickerFor(m => m.UniD.DocumentDate, new { @class="passSeries" })%>
                <div id="txtDateError"></div>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("CountryID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.CountryID)%> 
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocEdu.CountryID, 
                        Model.UniD.EntDocEdu.Countries, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("InstitutionName", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.InstitutionName)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.InstitutionName, new {style = "width:500px"}) %>

                <div id="txtDateError"></div>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("InstitutionAddress", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.InstitutionAddress)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.InstitutionAddress, new {style = "width:500px"}) %>

                <div id="txtDateError"></div>
            </td>


        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("FacultyName", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.FacultyName)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.FacultyName, new {style = "width:500px"}) %>

                <div id="txtDateError"></div>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("BeginDate", Model.UniD.DocumentTypeID)&& EntrantDocumentMatrix.isFieledInDoc("EndDate", Model.UniD.DocumentTypeID)) { %>
        <tr>


            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.BeginDate)%>

            </td>
            <td>
                <%= Html.DatePickerFor(m => m.UniD.EntDocEdu.BeginDate)%>


                <div id="txtDateError"></div>
            </td>

            <td class="auto-style6">
                                <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.EndDate )%>

            </td>

            <td class="auto-style11" >
                                <%= Html.DatePickerFor(m => m.UniD.EntDocEdu.EndDate)%>
                                <div id="txtDateError" class="auto-style8"></div>
            </td>

        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("EducationFormID", Model.UniD.DocumentTypeID)) { %>
            <tr class="spaceUnder">
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.EducationFormID)%>
                </td>
                <td colspan="4">
                    <%= Html.DropDownListFor(m => m.UniD.EntDocEdu.EducationFormID, 
                    Model.UniD.EntDocEdu.EducationForms, 
                    new { @class = "chosen" })%>

                </td>
            </tr>
        <% } %>


        <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateYear", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.DocumentYear) %>
            </td>
            <% if (Model.UniD.DocumentTypeID == 2)
                { %>
            <td colspan="4">
                <select id="UniD_DocumentYear" style="width: 100px">
                    <option value="0" selected="selected"></option>
                    <option value="1">
                        <%= DateTime.Now.Year %>
                    </option>
                    <option value="2">
                        <%= DateTime.Now.Year-1 %></option>
                    <option value="3">
                        <%= DateTime.Now.Year-2 %></option>
                    <option value="4">
                        <%= DateTime.Now.Year-3 %></option>
                    <option value="5">
                        <%= DateTime.Now.Year-4 %></option>
                </select>
            </td>
            <% }
                else
                { %>
            <td>
                <%= Html.TextBoxExFor(m => m.UniD.DocumentYear, new {style = "width:100px"}) %>
            </td>
            <% } %>
        </tr>
        <% } %>

        <% if (Model.UniD.EntDocIdentity != null) { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("SubdivisionCode", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.SubdivisionCode)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocIdentity.SubdivisionCode,new { @class="passSeries" })%>
            </td>
        </tr>
        <% } %>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentOrganization", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentOrganization)%>
                <% if (EntrantDocumentMatrix.getFieledForDoc("DocumentOrganization", Model.UniD.DocumentTypeID) == 2)
                    {%>
                <span class="required">*</span>
                <%}%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.DocumentOrganization)%>
            </td>
        </tr>
        <% }
            else if (EntrantDocumentMatrix.isFieledInDoc("DocOrganization", Model.UniD.DocumentTypeID))
            {  %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentOrganizationW)%>
                <% if (EntrantDocumentMatrix.getFieledForDoc("DocOrganization", Model.UniD.DocumentTypeID) == 2)
                    {%>
                <span class="required">*</span>
                <%}%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.DocumentOrganization)%>
            </td>
        </tr>
        <% } %>

        <% if (Model.UniD.EntDocIdentity != null) { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("GenderTypeID", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.GenderTypeID)%>
            </td>
            <td colspan="4">
                <%= Html.DropDownListFor(m => m.UniD.EntDocIdentity.GenderTypeID,new SelectList(Model.UniD.EntDocIdentity.GenderList,"ID","Name"))%>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("NationalityTypeID", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.NationalityTypeID)%>
            </td>
            <td colspan="4">
                <%= Html.DropDownListFor(m => m.UniD.EntDocIdentity.NationalityTypeID,new SelectList(Model.UniD.EntDocIdentity.NationalityList,"ID","Name"))%>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("BirthDate", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.BirthDate)%>
            </td>
            <td colspan="4">
                <%= Html.DatePickerFor(m => m.UniD.EntDocIdentity.BirthDate)%><div id="txtbDateError"></div>
            </td>
        </tr>
        <% } %>
        
        <% if (EntrantDocumentMatrix.isFieledInDoc("BirthPlace", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelReqFor(m => m.UniD.EntDocIdentity.BirthPlace)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocIdentity.BirthPlace)%>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (Model.UniD.EntDocEdu != null)
            { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("RegistrationNumber", Model.UniD.DocumentTypeID))
            { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.RegistrationNumber)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.RegistrationNumber) %>
            </td>
        </tr>
        <% } %>

        <%--квалификация--%>
        <% if (EntrantDocumentMatrix.isFieledInDoc("QualificationName", Model.UniD.DocumentTypeID))
            { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.QualificationName)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.QualificationName,new { style="width:500px" })%>
            </td>
        </tr>
        <% } %>

        <%--направление подготовки--%>
        <% if (EntrantDocumentMatrix.isFieledInDoc("SpecialityName", Model.UniD.DocumentTypeID))
            { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.SpecialityName)%>
            </td>
            <td colspan="4">
                <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.SpecialityName,new { style="width:500px" })%>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentOU", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.DocumentOU)%>
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.DocumentOU,new { style="width:500px" })%>
                </td>
            </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("GPA", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.GPA)%>
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocEdu.GPA,new { style="width:500px" })%>
                </td>
            </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc(EntrantDocumentMatrix.Field_IsNostrificated, Model.UniD.DocumentTypeID, Model.UniD.EntrantID))
           { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.IsNostrificated)%>
                </td>
                <td colspan="4">
                    <%
                    if (!Model.UniD.EntDocEdu.IsNostrificated.HasValue)
                    {
                       Model.UniD.EntDocEdu.IsNostrificated = false;
                    }                         
                         %>
                    <%= Html.CheckBoxFor(m => m.UniD.EntDocEdu.IsNostrificated.Value)%>
                </td>
            </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("StateServicePreparation", Model.UniD.DocumentTypeID, Model.UniD.EntrantID))
           { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocEdu.StateServicePreparation)%>
                </td>
                <td colspan="4">
                    <%
                    if (!Model.UniD.EntDocEdu.StateServicePreparation.HasValue)
                    {
                       Model.UniD.EntDocEdu.StateServicePreparation = false;
                    }                         
                         %>
                    <%= Html.CheckBoxFor(m => m.UniD.EntDocEdu.StateServicePreparation.Value)%>
                </td>
            </tr>
        <% } %>
        <% } %>


        <% if (Model.UniD.EntDocOlymp != null) { %>

            <%--тип диплома--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicDiplomaTypeID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOlymp.DiplomaTypeID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOlymp.DiplomaTypeID, 
                        Model.UniD.EntDocOlymp.Diploms, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--олимпиада--%>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicID", Model.UniD.DocumentTypeID)) { %>
            <tr class="spaceUnder">
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocOlymp.OlympicID)%> <span class="required">*</span>
                </td>
                <td colspan="4">
                    <%= Html.DropDownGroupListFor(m => m.UniD.EntDocOlymp.OlympicID, Model.UniD.EntDocOlymp.Olympics, 
                    new { @class = "chosen", onchange = "ChangeOlympicType(this.value)" })%>
                    <span id="td_UniD_EntDocOlymp_OlympicYear" style="margin-left:20px;color:crimson"></span>
                </td>
            </tr>
            <% } %>

            <%--профиль --%>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicProfSubject", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicDetails.SubjectNames)%> <span class="required">*</span>
                    </td>
                    <td id="td_UniD_EntDocOlymp_SubjectNames" colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOlymp.OlympicTypeProfileID,
                        new SelectList(new[] { new { Value = "0", Text = "Не выбрано" }}, "Value", "Text"),
                        new { @class = "chosen", onchange = "ChangeOlympicTypeProfile(this.value)" })%>
                        <span id="td_UniD_EntDocOlymp_LevelName" style="margin-left:20px;color:crimson"></span>
                    </td>
                </tr>
            <% } %>

            <%-- профильная дисциплина олимпиады --%>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicSubject", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.ProfileSubjectID)%> 
                    </td>
                    <td id="td1" colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOlymp.ProfileSubjectID,
                        Model.UniD.EntDocOlymp.ProfileSubjects,
                        new { @class = "chosen", onchange = "ChangeOlympicProfileSubject(this.value)" })%>
                        <span id="Span1" style="margin-left:20px;color:crimson"></span>
                    </td>
                </tr>
            <% } %>

            <%-- соответствующий профильной дисциплине общеобразовательный предмет --%>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicEgeSubject", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.EgeSubjectID)%> 
                    </td>
                    <td id="td2" colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOlymp.EgeSubjectID,
                        Model.UniD.EntDocOlymp.EgeSubjects,
                        new { @class = "chosen" })%>
                        <span id="Span2" style="margin-left:20px;color:crimson"></span>
                    </td>
                </tr>
            <% } %>

            <%-- класс  --%>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicFormNumber", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.FormNumberID)%> <span class="required">*</span>
                    </td>
                    <td id="td_UniD_EntDocOlymp_FormNumberID" colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOlymp.FormNumberID, 
                        Model.UniD.EntDocOlymp.FormNumber,
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>
        <% } %>

        <% if (Model.UniD.EntDocDis != null) { %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("DisabilityTypeID", Model.UniD.DocumentTypeID)) { %>
                <tr>
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocDis.DisabilityTypeID)%><span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocDis.DisabilityTypeID, Model.UniD.EntDocDis.DisabilityTypes, new { @class = "chosen" })%>
                    </td>
                </tr>
            <% } %>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("AdditionalInfo", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocCustom.AdditionalInfo) %>
                </td>
                <td colspan="4">
                    <%= Html.TextBoxExFor(m => m.UniD.EntDocCustom.AdditionalInfo)%>
                </td>
            </tr>
        <% } %>
        
        <% if (Model.UniD.EntDocSubBall != null) { %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("SubjectEgeSertificate", Model.UniD.DocumentTypeID)) { %>
            <tr id="UniD_EntDocSubBall_SubjectBalls_tr">
                <td class="caption"></td>
                <td colspan="4">
                    <div id="UniD_EntDocSubBall_divErrorPlace">
                    </div>
                    <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                        <thead>
                            <tr>
                                <th class="auto-style3">
                                    <%= Html.LabelFor(x => x.UniD.EntDocSubBall.FieldInfo.SubjectID) %>
                                </th>
                                <th class="auto-style4">
                                    <%= Html.LabelFor(x => x.UniD.EntDocSubBall.FieldInfo.Value) %>
                                </th>
                                <th class="auto-style5"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr id="trAddNewSubjectBall">
                                <td colspan="4">
                                    <a href="javascript:void(0)" id="btnAddNewSubjectBall" class="add">Добавить дисциплину</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <span>
                        <input type="text" val-required="Необходимо выбрать хотя бы одну дисциплину" val-new-line="0"
                            id="tbEmptySubjectsBall" style="display: none" />
                    </span>
                </td>
            </tr>
            <% } %>
        <% } %>

        <%--=============================================================================================--%>

        <%--Прочие документы--%>
        <% if (Model.UniD.EntDocOther != null) { %>

            <%--тип диплома--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicDiplomaTypeID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.DiplomaTypeID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.DiplomaTypeID, 
                        Model.UniD.EntDocOther.Diploms, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--Член сборной команды--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("CountryID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.CountryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.CountryID, 
                        Model.UniD.EntDocOther.Countries, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--Категория CompatriotCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("CompatriotCategoryID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.CompatriotCategoryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.CompatriotCategoryID, 
                        Model.UniD.EntDocOther.CompatriotCategories, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--CompatriotStatus --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("CompatriotStatus", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.CompatriotStatus)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.TextBoxExFor(m => m.UniD.EntDocOther.CompatriotStatus, new { style="width:500px" })%>

                    </td>
                </tr>
            <% } %>

            <%--Категория OrphanCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OrphanCategoryID", Model.UniD.DocumentTypeID)) { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.OrphanCategoryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.OrphanCategoryID, 
                        Model.UniD.EntDocOther.OrphanCategories, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--Категория VeteranCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("VeteranCategoryID", Model.UniD.DocumentTypeID))
               { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.VeteranCategoryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.VeteranCategoryID, 
                        Model.UniD.EntDocOther.VeteranCategories, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--Категория ParentsLostCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("ParentsLostCategoryID", Model.UniD.DocumentTypeID))
               { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.ParentsLostCategoryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.ParentsLostCategoryID, 
                        Model.UniD.EntDocOther.ParentsLostCategories, 
                        new { @class = "chosen" })%>

                    </td>
                </tr>
            <% } %>

            <%--Категория StateEmployeeCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("StateEmployeeCategoryID", Model.UniD.DocumentTypeID))
               { %>
                <tr class="spaceUnder">
                    <td class="auto-style1">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.StateEmployeeCategoryID)%> <span class="required">*</span>
                    </td>
                    <td class="auto-style1" colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.StateEmployeeCategoryID, 
                        Model.UniD.EntDocOther.StateEmployeeCategories, 
                        new { @class = "chosen" })%>
                    </td>
                </tr>
            <% } %>

            <%--Категория RadiationWorkCategoryID --%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("RadiationWorkCategoryID", Model.UniD.DocumentTypeID))
               { %>
                <tr class="spaceUnder">
                    <td class="caption">
                        <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.RadiationWorkCategoryID)%> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.DropDownListFor(m => m.UniD.EntDocOther.RadiationWorkCategoryID, 
                        Model.UniD.EntDocOther.RadiationWorkCategories, 
                        new { @class = "chosen" })%>
                    </td>
                </tr>
            <% } %>

            <%--наименование олимпиады--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicName", Model.UniD.DocumentTypeID)) { %>
                <tr>
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.OlympicName) %> <span class="required">*</span>
                    </td>
                    <td colspan="4">
                        <%= Html.TextBoxExFor(m => m.UniD.OlympicName)%>
                    </td>
                </tr>
            <% } %>

            <%--профиль олимпиады--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicProfile", Model.UniD.DocumentTypeID)) { %>
                <tr>
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OlympicProfile) %> <% if (Model.UniD.DocumentTypeID != 28) { %> <span class="required">*</span> <% } %>
                    </td>
                    <td colspan="4">
                        <%= Html.TextBoxExFor(m => m.UniD.EntDocOther.OlympicProfile)%>
                    </td>
                </tr>
            <% } %>

            <%--дата проведения олимпиады--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicDate", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelReqFor(m => m.UniD.EntDocOther.OlympicDate)%>
                </td>
                <td colspan="4">
                    <%= Html.DatePickerFor(m => m.UniD.EntDocOther.OlympicDate, new { @class="passSeries" })%>
                    <%--<div id="txtDateError"></div>--%>
                </td>
            </tr>
            <% } %>

            <%--место проведения олимпиады--%>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicProfile", Model.UniD.DocumentTypeID)) { %>
                <tr>
                    <td class="caption">
                        <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OlympicPlace) %>
                    </td>
                    <td colspan="4">
                        <%= Html.TextBoxExFor(m => m.UniD.EntDocOther.OlympicPlace)%>
                    </td>
                </tr>
            <% } %>

        <% } %>

        <%--=============================================================================================--%>

        <% if (EntrantDocumentMatrix.isFieledInDoc("AttachmentID", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.AttachmentFileID)%>
            </td>
            <td colspan="4">
                <% if (Model.UniD.AttachmentFileID != Guid.Empty && Model.UniD.AttachmentFileID != null) { %>
                <div>
                    <a fileid="<%= Model.UniD.AttachmentID %>" class="getFileLink" href="/Entrant/GetFile1?fileID=<%=Model.UniD.AttachmentFileID %>"><%: Model.UniD.AttachmentName%></a>
                    <button class="fileDelete" onclick="UniD_detachAttachedDocument(this); return false; ">&nbsp;</button>
                </div>
                <% } %>
                <%= Html.FileForm("UniD")%>
            </td>
        </tr>
        <% } %>
    </tbody>
</table>
<script type="text/javascript">
    var prevDocSeries, prevDocNumber;
    var isClickEGE = false;

    function checkDocumentOnExisting(docSeries, docNumber, isPost) {
        //if(<%= Model.UniD.EntrantDocumentID %> > 0) return;
        return true;
        // Заглушен
        if (docSeries == prevDocSeries && prevDocNumber == docNumber && !isPost){    return;}

        prevDocNumber = docNumber;
        prevDocSeries = docSeries;
        var model = {
            EntrantID: <%= Model.UniD.EntrantID %>,
            EntrantDocumentID: <%= Model.UniD.EntrantDocumentID %>,
            DocumentTypeID: <%= Model.UniD.DocumentTypeID %>,
            ApplicationID: ApplicationId ? ApplicationId : 0,
            DocumentSeries: docSeries,
            DocumentNumber: docNumber
        };
        var func = checkDocumentSeriesNumber;
        if (isPost) func = checkDocumentSeriesNumberPost;
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.CheckDocumentOnExists(null)) %>', JSON.stringify(model), func);
    }

    function checkDocumentSeriesNumber(data) {
        // Заглушен
        if (data.Data.IsFound > 0 && <%= Model.UniD.EntrantDocumentID %> > 0) {
            $('<div>У абитуриента найден документ с теми же данными. Убедитесь, что у этого документа корректные данные.</div>').dialog({
                width: '600px',
                modal: true,
                buttons: {  "Продолжить": function() {      closeDialog($(this));   }    }
            });
            return;
        }
        if (data.Data.IsFound > 0) {
            if (!data.Data.CanBeModified) {
                $('<div>У абитуриента найден документ с теми же данными. При этом его невозможно изменить, так как он уже используется в заявлении. Продолжить добавление нового документа?</div>').dialog({
                    width: '600px',
                    modal: true,
                    buttons: {
                        "Продолжить": function() {    closeDialog($(this));  },
                        "Не добавлять": function() {  closeDialog($(this));  $('#btnCancel').click(); }
                    }
                });
            } else {
                $('<div>У абитуриента найден документ с теми же данными. Перейти к редактированию существующего?</div>').dialog({
                    width: '600px',
                    modal: true,
                    buttons: {
                        "Продолжить добавление": function() {  closeDialog($(this));      },
                        "Редактировать": function() {          closeDialog($(this));
                            if (callEditDocument) callEditDocument(data.Data.IsFound);
                        },
                        "Не добавлять": function() {    closeDialog($(this));
                            $('#btnCancel').click();
                        }
                    }
                });
            }
        }
    }

    function checkDocumentSeriesNumberPost(data) {
        if (data.Data.IsFound > 0) {
            $('<div>У абитуриента найден документ с теми же данными. Исправьте данные перед сохранением.</div>').dialog({
                width: '600px',
                modal: true,
                buttons: {   "Закрыть": function() {      closeDialog($(this));  }      }
            });
            return;
        } else {
            //doSubmitFile();
        }
    }
</script>
<script type="text/javascript">
    function UniD_detachAttachedDocument(obj) {
        $(obj).parent().remove();
    }
    function UniD_identityDocumentTypeChanged() {
        var docTypeID = $('#UniD_IdentityDocumentTypeID').val();
        //    if (russianDocs.indexOf(docTypeID) >= 0) { jQuery('#NationalityTypeID').val('1').attr('disabled', 'disabled'); } else { jQuery('#NationalityTypeID').removeAttr('disabled'); }
    }

    function UniDFormInit() {
  
        var docTypeID = $('#UniD_DocumentTypeID').val();
        $("#UniD_DocumentDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0', maxDate: new Date() });
        $("#UniD_EntDocEdu_BeginDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0', maxDate: new Date() });
        $("#UniD_EntDocEdu_EndDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0', maxDate: new Date() });
        <% if (Model.UniD.DocumentTypeID == 2)
        { %>

        // 1 - 2016 (текущий), 2 - 2015 (-1) и т.д.
        var YearEGECert = '<%=Model.UniD.DocumentYear.HasValue ? DateTime.Today.Year - Model.UniD.DocumentYear.Value + 1 : 0 %>';
        $('#UniD_DocumentYear').val(YearEGECert);

        //if (YearEGECert == '2012') {
        //    $('#UniD_DocumentYear').val(4);
        //}
        //if (YearEGECert == '2013') {
        //    $('#UniD_DocumentYear').val(3);
        //}
        //if (YearEGECert == '2014') {
        //    $('#UniD_DocumentYear').val(2);
        //}
        //if (YearEGECert == '2015') {
        //    $('#UniD_DocumentYear').val(1);
        //}
        //if (YearEGECert == '2015') {
        //    $('#UniD_DocumentYear').val(1);
        //}
        <% } %>

        // TODO;
        $('#UniD_DocumentSeries, #UniD_DocumentNumber').blur(function() {
            checkDocumentOnExisting($('#UniD_DocumentSeries').val(), $('#UniD_DocumentNumber').val());
        });

     <% if (Model.UniD.DocumentTypeID == 1){ %>
        $('#UniD_EntDocIdentity_IdentityDocumentTypeID').change(function () {
            switch ($(this).val()) {
                case '3':
                    $("#UniD_DocumentNumber").attr('maxlength', '50');
                    $("#UniD_DocumentSeries").attr('maxlength', '20');
                    break;
                case '9':
                    $("#UniD_DocumentNumber").attr('maxlength', '50');
                    $("#UniD_DocumentSeries").attr('maxlength', '20');
                    break;
                case '1':
                    $("#UniD_DocumentNumber").attr('maxlength', '6');
                    $("#UniD_DocumentNumber").val($("#UniD_DocumentNumber").val().length > 6 ? $("#UniD_DocumentNumber").val().substr(0, 6) : $("#UniD_DocumentNumber").val());
                    $("#UniD_DocumentSeries").attr('maxlength', '4');
                    $("#UniD_DocumentSeries").val($("#UniD_DocumentSeries").val().length > 4 ? $("#UniD_DocumentSeries").val().substr(0, 4) : $("#UniD_DocumentSeries").val());
                    break;
                default:
                    $("#UniD_DocumentNumber").attr('maxlength', '10');
                    $("#UniD_DocumentNumber").val($("#UniD_DocumentNumber").val().length > 10 ? $("#UniD_DocumentNumber").val().substr(0, 10) : $("#UniD_DocumentNumber").val());
                    $("#UniD_DocumentSeries").attr('maxlength', '6');
                    $("#UniD_DocumentSeries").val($("#UniD_DocumentSeries").val().length > 6 ? $("#UniD_DocumentSeries").val().substr(0, 6) : $("#UniD_DocumentSeries").val());
                    break;
            }
        });
        $('#UniD_EntDocIdentity_IdentityDocumentTypeID').change();
        <% } %>

      <% if (EntrantDocumentMatrix.getFieledForDoc("DocumentSeries", Model.UniD.DocumentTypeID) == 2) {%>    
        $('#UniD_DocumentSeries').attr('val-required','Обязательно!');
    <%}  else  { %>
        $('#UniD_DocumentSeries').attr('val-required','');
        <%} %>
      <% if (EntrantDocumentMatrix.getFieledForDoc("DocumentNumber", Model.UniD.DocumentTypeID) == 2) {%>    
        $('#UniD_DocumentNumber').attr('val-required','Обязательно!');
    <%}  else { %>
        $('#UniD_DocumentNumber').attr('val-required','');
        <%} %>
      <% if (EntrantDocumentMatrix.getFieledForDoc("DocumentOrganization", Model.UniD.DocumentTypeID) == 2) {%>    
        $('#UniD_DocumentOrganization').attr('val-required','Обязательно!');
    <%} else { %>
        $('#UniD_DocumentOrganization').attr('val-required','');
        <%} %>

        <% switch (Model.UniD.DocumentTypeID)
    { %>
        <% case 1: %> // 1	Документ, удостоверяющий личность
        // UniD_EntDocIdentity
        UniD_EntDocIdentity_Init();
        <% break; %>

    <% case 8: // 8	Академическая справка
    case 26: // 26	Диплом кандидата наук
    case 4: //	4	Диплом о высшем профессиональном образовании
    case 6: // 6	Диплом о начальном профессиональном образовании
    case 7: // 7	Диплом о неполном высшем профессиональном образовании
    case 5: // 5	Диплом о среднем профессиональном образовании
    case 25:    // 25	Диплом об окончании аспирантуры (адъюнкатуры)      

    case 3: // 3	Аттестат о среднем (полном) общем образовании    
    case 18:// 18	Справка об обучении в другом ВУЗе
    case 19: // 19	Иной документ по образованию  
      %> 
        UniD_EntDocEdu_Init();
        <% break; %>
        <% case 14: /*14	Военный билет */ %>
        <% break; %>

        // Диплом победителя/призера олимпиады школьников
        <% case 9: if (Model.UniD.EntDocOlymp != null) { %> UniD_EntDocOlymp_Init(9);  <% } break; %>
        // Диплом победителя/призера всероссийской олимпиады школьников
        <% case 10: if (Model.UniD.EntDocOlymp != null) { %> UniD_EntDocOlymp_Init(10);  <% } break; %>
        // Диплом победителя/призера IV этапа всеукраинской ученической олимпиады
        <% case 27: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(27);  <% } break; %>
        // Диплом об участии в международной олимпиаде
        <% case 28: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(28);  <% } break; %>
        // Документ, подтверждающий принадлежность к соотечественникам за рубежом
        <% case 29: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(29);  <% } break; %>
        // Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей
        <% case 30: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(30);  <% } break; %>
        // Документ, подтверждающий принадлежность к ветеранам боевых действий
        <% case 31: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(31);  <% } break; %>
        // Документ, подтверждающий наличие только одного родителя - инвалида I группы и принадлежность к числу малоимущих семей
        <% case 32: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(32);  <% } break; %>
        // Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
        <% case 33: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(33);  <% } break; %>        
        <% case 34: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(34);  <% } break; %>
        <% case 35: if (Model.UniD.EntDocOther != null) { %> UniD_EntDocOther_Init(35);  <% } break; %>

        <% case 12: /* 12	Заключение психолого-медико-педагогической комиссии*/ %>
        <% case 13: /*13	Заключение об отсутствии противопоказаний для обучения*/ %>              
        // $('#UniD_DocumentOrganization') Найти лейбил и заменить на "Кем выдана"

        <% break; %>
        <% default:	%>
        <% break; %>
        <% }%>

    <% if (Model.UniD.DocumentTypeID == 9 || Model.UniD.DocumentTypeID == 10)
    { %>
        $('#btnGetOlympicCheck').button();
        <% }%>
  
      <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateNumber", Model.UniD.DocumentTypeID))
    { %>  
        $("input#UniD_CertificateNumber").mask("99-999999999-99");
        $('#btnGetEge').button();
    <%} %>
    
        $('input#UniD_DocumentYear').mask("9999");

    <% if (Model.UniD.EntDocSubBall != null)
    { %>
        UniD_EntDocSubBall_Init();
        <% }%>

    <% if (Model.UniD.DocumentTypeID == 3 || Model.UniD.DocumentTypeID == 16)
    {
        // 3	Аттестат о среднем (полном) общем образовании
        // 16	Аттестат об основном общем образовании
    %>
        $('#tbEmptySubjectsBall').val('1');  
    <% }%>
    }

    function UniD_EntDocIdentity_Init(){

        var id = $("#UniD_EntrantDocumentID").val();

        $("#UniD_EntDocIdentity_BirthDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0', maxDate: new Date() });
        $('#UniD_EntDocIdentity_BirthDate').datepicker("option", "defaultDate", '-17y');
        $("#UniD_EntDocIdentity_SubdivisionCode").mask("999-999");

        // только для нового переносим ФИО
        if(id == 0){
            if($("#UniD_EntDocIdentity_LastName").val() == "")
                $("#UniD_EntDocIdentity_LastName").val($("#LastName").val());

            if($("#UniD_EntDocIdentity_FirstName").val() == "")
                $("#UniD_EntDocIdentity_FirstName").val($("#FirstName").val());

            if($("#UniD_EntDocIdentity_MiddleName").val() == "")
                $("#UniD_EntDocIdentity_MiddleName").val($("#MiddleName").val());
        }

    }

<% if (Model.UniD.EntDocEdu != null)
    { %>
    function UniD_EntDocEdu_Init(){    
        $('#UniD_DocumentSeries').attr('val-required','');

        $('#UniD_EntDocEdu_GPA').blur(function() {
            var el =$('input#UniD_EntDocEdu_GPA');
            var val=$('input#UniD_EntDocEdu_GPA').val();
            if (val == ''){   return false;}
            var tempvar = parseFloat(val.replace(',', '.'));

            if (tempvar > 100 || tempvar < 0) {
                infoDialog('Значение среднего балла не может быть больше 100 или меньше 0.');
                el.val('');
                return;
            }

            tempvar = (tempvar.toFixed(4)).replace('.', ',');
            if (tempvar == 'NaN') {
                infoDialog('Неправильный формат ввода. Средний балл - вещественное число о 0 до 100.');
                el.val('');
                return;
            }
            el.val(tempvar);
        });
        

        autocompleteDropdown($('#UniD_EntDocEdu_QualificationName'), {
            source: function(ui, response) {
                doPostAjax('<%= Url.Generate<DictionaryController>(x => x.GetQualifications(null)) %>', JSON.stringify({filter:ui.term}),
                    function (data) {
                        if (!data.IsError) {
                            response(data.Data);
                        } else { // Обработка ошибки.
                            infoDialog (data.Message);
                        }
                    });               
            },
            select:
                function(e){      
                },
            minLength: 3
        });

            autocompleteDropdown($('#UniD_EntDocEdu_SpecialityName'),{
                source: function(ui, response) {
                    var val=$('#UniD_EntDocEdu_QualificationName').val();
                    doPostAjax('<%= Url.Generate<DictionaryController>(x => x.GetSpecialityByQualification(null)) %>', JSON.stringify({Qualification:val}),
                    function (data) {
                        if (!data.IsError) {
                            response(data.Data);
                        } else { // Обработка ошибки.
                            infoDialog (data.Message);
                        }
                    });               
            },
            select:
                function(e){      
                },
            minLength: 3
        });


    }
<% } %>

    function UniDPrepareModel(baseModel) {
        if (UniDCheckModel()) {
            return UniDGetModel(baseModel);
        }
        return null;
    }

    function UniDCheckModel(doc) {
        var res = true;
        var $form = $('#UniDForm');
        var isError = false;
        //isClickEGE = true;

        if (doc == undefined || doc == null) {
            doc = UniDGetModel();
        }
        var flagErrorDate = false;

        $form.find('#txtDateError span').text("");
        $form.find('#UniD_DocumentDate').removeClass('input-validation-error-fixed');
        if ((doc.DocumentDate != undefined)&&(doc.DocumentDate !=null)&&(doc.DocumentDate !="")){
            if (!validateDate(doc.DocumentDate)) {
                $form.find('#UniD_DocumentDate').addClass('input-validation-error');
                $form.find('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                flagErrorDate = true;
            }
        }

      <% if (EntrantDocumentMatrix.isFieledInDoc("BirthDate", Model.UniD.DocumentTypeID)) { %>
        $form.find('#txtbDateError span').text("");
        $form.find('#UniD_EntDocIdentity_BirthDate').removeClass('input-validation-error-fixed');
        if ((doc.EntDocIdentity.BirthDate != undefined)&&(doc.EntDocIdentity.BirthDate !=null)&&(doc.EntDocIdentity.BirthDate !="")){
            if (!validateDate(doc.EntDocIdentity.BirthDate)) {
                $form.find('#UniD_EntDocIdentity_BirthDate').addClass('input-validation-error');
                $form.find('#txtbDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                flagErrorDate = true;
            }
        }
      <% } %>
        if (flagErrorDate) {
            return false;
        }

        //var typeID = parseInt(doc.IdentityDocumentTypeID);

        var type = parseInt(doc.DocumentTypeID);
        if ((type == 3) || (type == 16)) {
            // 3	Аттестат о среднем (полном) общем образовании
            // 16	Аттестат об основном общем образовании
            $('#tbEmptySubjectsBall').val('1');
        }
        if ((type == 2)) {
            //2	Свидетельство о результатах ЕГЭ
            $form.find('#UniD_DocumentYear').attr('val-required', '');
            $form.find('#UniD_DocumentYear').removeClass('input-validation-error-fixed');
            if (type == 2) {
                if ($form.find('#UniD_DocumentYear').val() == "0") {
                    if (!isClickEGE) {
                        isClickEGE = false;
                        $form.find('#UniD_DocumentYear').attr('val-required', 'Обязательно!');
                        $form.find('#UniD_DocumentYear').addClass('input-validation-error');
                        return;
                    }
                }
            }
            $('#tbEmptySubjectsBall').val('1');
        }
        //if (type == 10) {
        //    $('#tbEmptySubjects').val('1');
        //}

      <%  if (Model.UniD.DocumentTypeID == 1)
    { %>
        $form.find('#UniD_EntDocIdentity_NationalityTypeID').removeClass('input-validation-error-fixed');
        $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').removeClass('input-validation-error-fixed');
        $form.find('#UniD_DocumentSeries').removeClass('input-validation-error-fixed');
        $form.find('#UniD_DocumentSeries').attr('val-required', '');

        $('#UniD_EntDocIdentity_LastName').removeClass('validation-error');
        if($('#UniD_EntDocIdentity_LastName').val() == ""){
            $('#UniD_EntDocIdentity_LastName').addClass('validation-error');
            isError = true;
        }
        $('#UniD_EntDocIdentity_FirstName').removeClass('validation-error');
        if($('#UniD_EntDocIdentity_FirstName').val() == ""){
            $('#UniD_EntDocIdentity_FirstName').addClass('validation-error');
            isError = true;
        }

        //$('#UniD_EntDocIdentity_MiddleName').removeClass('validation-error');
        //if($('#UniD_EntDocIdentity_MiddleName').val() == ""){
        //    $('#UniD_EntDocIdentity_MiddleName').addClass('validation-error');
        //    isError = true;
        //}

        if ($form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').length > 0) {
            var listIdentityDocumentType = JSON.parse('<%= Html.Serialize(Model.ListIdentityDocumentType) %>');

          var resul = $.grep(listIdentityDocumentType, function(v) {
              return v.IdentityDocumentTypeId === parseInt($form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').val());
          });

          if ((resul[0].IdentityDocumentTypeId != 9) && (resul[0].IdentityDocumentTypeId != 10)) {
              if ((resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() != 1)) {
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').addClass('input-validation-error');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').addClass('input-validation-error');
                  $form.find('#UniD_DocumentSeries').attr('val-required', '');
              }
              if ((!resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() == 1)) {
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').addClass('input-validation-error');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').addClass('input-validation-error');
                  $form.find('#UniD_DocumentSeries').attr('val-required', '');
              }
          }

          if ((resul[0].IsRussianNationality) & ($form.find('#UniD_EntDocIdentity_NationalityTypeID').val() == 1)) {
              if ($form.find('#UniD_DocumentSeries').val() == "") {
                  $form.find('#UniD_DocumentSeries').attr('val-required', 'Обязательно!');
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').attr('val-required', '');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').attr('val-required', '');
              } else {
                  $form.find('#UniD_DocumentSeries').attr('val-required', '');
              }
          }
          if ((!resul[0].IsRussianNationality) & ($form.find('#UniD_EntDocIdentity_NationalityTypeID').val() != 1)) {
              $form.find('#UniD_DocumentSeries').attr('val-required', '');
          }
          if (resul[0].IdentityDocumentTypeId != 9) {
              if ((resul[0].IsRussianNationality) & ($form.find('#UniD_EntDocIdentity_NationalityTypeID').val() != 1)) {
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').attr('val-required', 'Ошибка!');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').attr('val-required', 'Ошибка!');
                  $form.find('#UniD_DocumentSeries').attr('val-required', '');
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').addClass('input-validation-error');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').addClass('input-validation-error');
                  isError = true;
                  return false;
              }
              if ((!resul[0].IsRussianNationality) & ($form.find('#UniD_EntDocIdentity_NationalityTypeID').val() == 1)) {
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').attr('val-required', 'Ошибка!');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').attr('val-required', 'Ошибка!');
                  $form.find('#UniD_DocumentSeries').attr('val-required', '');
                  $form.find('#UniD_EntDocIdentity_NationalityTypeID').addClass('input-validation-error');
                  $form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').addClass('input-validation-error');
                  isError = true;
                  return false;
              }
          }
      }
      <% } %>

        if (doc.EntDocSubBall !== undefined && doc.EntDocSubBall != null) {
            if (doc.EntDocSubBall.notCheck != true) {
                if (doc.EntDocSubBall.SubjectBalls !== undefined || doc.EntDocSubBall.SubjectBalls != null) {
                    if (doc.EntDocSubBall.SubjectBalls.length == 0) {
                        if (!(type == 3 || type == 16)) {
                            // 3	Аттестат о среднем (полном) общем образовании
                            // 16	Аттестат об основном общем образовании
                            infoDialog("Не заданы Дисциплины и Баллы!");
                            isError = true;
                        }
                    }
                }
            }
        }



        //валидация документа тип 27
        if (type == 27) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');

            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_DiplomaTypeID_chosen').removeClass('validation-error');
            $('#UniD_OlympicName').removeClass('validation-error');
            $('#UniD_EntDocOther_OlympicProfile').removeClass('validation-error');

            //var val = $('#UniD_DocumentSeries').val();
            //if(val == ""){
            //    $('#UniD_DocumentSeries').addClass('validation-error');
            //    isError = true;
            //}

            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_DiplomaTypeID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_DiplomaTypeID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_OlympicName').val();
            if(val == ""){
                $('#UniD_OlympicName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_OlympicProfile').val();
            if(val == ""){
                $('#UniD_EntDocOther_OlympicProfile').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 28
        if (type == 28) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');

            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_CountryID_chosen').removeClass('validation-error');
            $('#UniD_OlympicName').removeClass('validation-error');

            //var val = $('#UniD_DocumentSeries').val();
            //if(val == ""){
            //    $('#UniD_DocumentSeries').addClass('validation-error');
            //    isError = true;
            //}

            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_CountryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_CountryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_OlympicName').val();
            if(val == ""){
                $('#UniD_OlympicName').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 29
        if (type == 29) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_CompatriotCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');
            $('#UniD_EntDocOther_CompatriotStatus').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required', '');
            $('#UniD_EntDocOther_CompatriotStatus').attr('val-required', '')



            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_CompatriotCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_CompatriotCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_CompatriotStatus').val();
            if (val == 0) {
                $('#UniD_EntDocOther_CompatriotStatus').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_CompatriotStatus').val();
            if (val == "") {
                $('#UniD_EntDocOther_CompatriotStatus').addClass('validation-error');
                isError = true;
            }
        }


        //валидация документа тип 30
        if (type == 30) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_OrphanCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');



            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_OrphanCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_OrphanCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }


        //валидация документа тип 31
        if (type == 31) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_VeteranCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');
             
            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_VeteranCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_VeteranCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 32
        if (type == 32) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_VeteranCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');
             
            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 33
        if (type == 33) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_VeteranCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');
             
            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_ParentsLostCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_ParentsLostCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 34
        if (type == 34) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_VeteranCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');
             
            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_StateEmployeeCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_StateEmployeeCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }
        
        //валидация документа тип 35
        if (type == 35) {
            //$('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');
            $('#UniD_EntDocOther_RadiationWorkCategoryID_chosen').removeClass('validation-error');
            $('#UniD_DocumentDate').removeClass('validation-error');
            $('#UniD_DocumentName').removeClass('validation-error');
            $('#UniD_DocumentOrganization').removeClass('validation-error');

            $('#UniD_DocumentOrganization').attr('val-required','');
            $('#UniD_DocumentDate').attr('val-required','');
            $('#UniD_DocumentName').attr('val-required','');
             
            var val = $('#UniD_DocumentNumber').val();
            if(val == ""){
                $('#UniD_DocumentNumber').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOther_RadiationWorkCategoryID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOther_RadiationWorkCategoryID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentDate').val();
            if(val == ""){
                $('#UniD_DocumentDate').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentName').val();
            if(val == ""){
                $('#UniD_DocumentName').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_DocumentOrganization').val();
            if(val == ""){
                $('#UniD_DocumentOrganization').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 9 и 10 (ОШ и ВОШ)
        if (type == 9 || type == 10) {
            $('#UniD_EntDocOlymp_DiplomaTypeID_chosen').removeClass('validation-error');
            $('#UniD_EntDocOlymp_OlympicID_chosen').removeClass('validation-error');
            $('#UniD_EntDocOlymp_OlympicTypeProfileID_chosen').removeClass('validation-error');
            $('#UniD_EntDocOlymp_FormNumberID_chosen').removeClass('validation-error');
            $('#UniD_DocumentSeries').removeClass('validation-error');
            $('#UniD_DocumentNumber').removeClass('validation-error');

            if(type == 10)//10 - ВсОШ - проверяем номер
            {
                var val = $('#UniD_DocumentNumber').val();
                if(val == ""){
                    $('#UniD_DocumentNumber').addClass('validation-error');
                    isError = true;
                }
            }          

            var val = $('#UniD_EntDocOlymp_DiplomaTypeID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOlymp_DiplomaTypeID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOlymp_OlympicID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOlymp_OlympicID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOlymp_OlympicTypeProfileID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOlymp_OlympicTypeProfileID_chosen').addClass('validation-error');
                isError = true;
            }

            var val = $('#UniD_EntDocOlymp_FormNumberID').chosen().val();
            if(val == 0){
                $('#UniD_EntDocOlymp_FormNumberID_chosen').addClass('validation-error');
                isError = true;
            }
        }

        //валидация документа тип 11
        if (type == 11) {
            $('#UniD_EntDocDis_DisabilityTypeID_chosen').removeClass('validation-error');
            $('#UniD_EntDocDis_DisabilityTypeID').attr('val-required', '')

            var val = $('#UniD_EntDocDis_DisabilityTypeID').chosen().val();
            if (val == 0) {
                $('#UniD_EntDocDis_DisabilityTypeID_chosen').addClass('validation-error');
                isError = true;
            }

            //$('#UniD_DocumentNumber').removeClass('validation-error');
            //$('#UniD_EntDocOther_CompatriotCategoryID_chosen').removeClass('validation-error');
            //$('#UniD_DocumentDate').removeClass('validation-error');
            //$('#UniD_DocumentName').removeClass('validation-error');
            //$('#UniD_DocumentOrganization').removeClass('validation-error');

            //$('#UniD_DocumentOrganization').attr('val-required', '');
            //$('#UniD_DocumentDate').attr('val-required', '');
            //$('#UniD_DocumentName').attr('val-required', '');
            //$('#UniD_EntDocOther_CompatriotStatus').attr('val-required', '')



            //var val = $('#UniD_DocumentNumber').val();
            //if (val == "") {
            //    $('#UniD_DocumentNumber').addClass('validation-error');
            //    isError = true;
            //}

            //var val = $('#UniD_EntDocOther_CompatriotCategoryID').chosen().val();
            //if (val == 0) {
            //    $('#UniD_EntDocOther_CompatriotCategoryID_chosen').addClass('validation-error');
            //    isError = true;
            //}

            //var val = $('#UniD_DocumentDate').val();
            //if (val == "") {
            //    $('#UniD_DocumentDate').addClass('validation-error');
            //    isError = true;
            //}

            //var val = $('#UniD_DocumentName').val();
            //if (val == "") {
            //    $('#UniD_DocumentName').addClass('validation-error');
            //    isError = true;
            //}

            //var val = $('#UniD_DocumentOrganization').val();
            //if (val == "") {
            //    $('#UniD_DocumentOrganization').addClass('validation-error');
            //    isError = true;
            //}

            //var val = $('#UniD_EntDocOther_CompatriotStatus').val();
            //if (val == "") {
            //    $('#UniD_EntDocOther_CompatriotStatus').addClass('validation-error');
            //    isError = true;
            //}

        }

        if (revalidatePage($form)) {
            isError = true;
            return false;
        }

        if (isError) {
            return false;
        }

        // Проверка наличия подобного документа
        checkDocumentOnExisting(doc.DocumentSeries, doc.DocumentNumber, false);
        return res;
    }

    function UniDGetModel(baseModel) {
        var form = $('#UniDForm'); 
        var m= {
            EntrantID: form.find('#UniD_EntrantID').val(),
            EntrantDocumentID: form.find('#UniD_EntrantDocumentID').val(),
            DocumentTypeID: form.find('#UniD_DocumentTypeID').val(),
            DocumentTypeName: form.find('#UniD_DocumentTypeID option:selected').text()
        };
        try {
            if (baseModel != undefined) {
                m.EntrantID = baseModel.EntrantID;
            }else{
                if(EntrantID != undefined){
                    m.EntrantID=EntrantID;
                }      
            }
        } catch (e) {    };
        m.UID = form.find('#UniD_UID').val();
        m.ApplicationID = ApplicationId;
        m.DocumentName= form.find('#UniD_DocumentName').val();
        //для олимпиады ставим DocumentName = OlympicName
        if (m.DocumentName == null) {
            m.DocumentName= form.find('#UniD_OlympicName').val();
        }
        m.DocumentSeries= form.find('#UniD_DocumentSeries').val();
        m.DocumentNumber= form.find('#UniD_DocumentNumber').val();
        m.Series=form.find('#UniD_Series').val();
        m.Number=form.find('#UniD_Number').val();
        m.CertificateNumber=form.find('#UniD_CertificateNumber').val();    
        m.DocumentDate= form.find('#UniD_DocumentDate').val();

        if (m.DocumentTypeID == 2) {
            m.DocumentYear = form.find("#UniD_DocumentYear option:selected").text();
        } else {
            m.DocumentYear= form.find("#UniD_DocumentYear").val();
        }
        m.DocumentOrganization = form.find('#UniD_DocumentOrganization').val();
        m.AttachmentFile = form.find('#postUniDFile').val();
        m.AttachmentID = form.find('a[fileID]').attr('fileID');
        m.IdentityDocumentTypeID = form.find('#UniD_EntDocIdentity_IdentityDocumentTypeID').val();
        if(!StringIsEmpty(m.CertificateNumber)){
            if(StringIsEmpty(m.DocumentNumber)){
                m.DocumentNumber=m.CertificateNumber;
            }
        }

        var type=parseInt(m.DocumentTypeID);
        switch(type){
            case 1:   // Надо подумать как лучше делать.
                // UniD_EntDocIdentity 
                var edi = {};
                edi.GenderTypeID = $("#UniD_EntDocIdentity_GenderTypeID").val();
                edi.GenderTypeName=$("#UniD_EntDocIdentity_GenderTypeID :selected").text();
                edi.BirthDate = $("#UniD_EntDocIdentity_BirthDate").val();
                edi.IdentityDocumentTypeID = $("#UniD_EntDocIdentity_IdentityDocumentTypeID").val();
                edi.IdentityDocumentTypeName = $("#UniD_EntDocIdentity_IdentityDocumentTypeID :selected").text();

                edi.SubdivisionCode = $("#UniD_EntDocIdentity_SubdivisionCode").val();
                edi.NationalityTypeID = $("#UniD_EntDocIdentity_NationalityTypeID").val();
                edi.NationalityTypeName = $("#UniD_EntDocIdentity_NationalityTypeID :selected").text();
                edi.BirthPlace = $("#UniD_EntDocIdentity_BirthPlace").val();

                edi.LastName = $("#UniD_EntDocIdentity_LastName").val();
                edi.FirstName = $("#UniD_EntDocIdentity_FirstName").val();
                edi.MiddleName = $("#UniD_EntDocIdentity_MiddleName").val();

                m.EntDocIdentity=edi;
                break;   
                
            //case 3:	// 3	Аттестат о среднем (полном) общем образовании
            case 5:	// 5	Диплом о среднем профессиональном образовании
            case 6:	// 6	Диплом о начальном профессиональном образовании   
            //case 16:// 16	Аттестат об основном общем образовании
                var edu = {};

                edu.InstitutionName = $("#UniD_EntDocEdu_InstitutionName").val();
                edu.InstitutionAddress = $("#UniD_EntDocEdu_InstitutionAddress").val();
                edu.FacultyName = $("#UniD_EntDocEdu_FacultyName").val();
                edu.BeginDate = $("#UniD_EntDocEdu_BeginDate").val();
                edu.EndDate = $("#UniD_EntDocEdu_EndDate").val();
                edu.CountryID = $('#UniD_EntDocEdu_CountryID').chosen().val();
                edu.EducationFormID = $('#UniD_EntDocEdu_EducationFormID').chosen().val();

                //m.EntDocDis = {
                //    DisabilityTypeID: $('#UniD_EntDocDis_DisabilityTypeID').val(),
                //};

                edu.RegistrationNumber=$("#UniD_EntDocEdu_RegistrationNumber").val();
                edu.QualificationName=$("#UniD_EntDocEdu_QualificationName").val();
                edu.SpecialityName=$("#UniD_EntDocEdu_SpecialityName").val();
                edu.DocumentOU=$("#UniD_EntDocEdu_DocumentOU").val();
                edu.GPA=$("#UniD_EntDocEdu_GPA").val();
                var isNostrificatedCheckbox = $("#UniD_EntDocEdu_IsNostrificated_Value");
                if(isNostrificatedCheckbox.length == 1)
                {
                    edu.IsNostrificated = isNostrificatedCheckbox.is(":checked");
                }
                var StateServicePreparationCheckbox = $("#UniD_EntDocEdu_StateServicePreparation_Value");
                if(StateServicePreparationCheckbox.length == 1)
                {
                    edu.StateServicePreparation = StateServicePreparationCheckbox.is(":checked");
                }                
                m.EntDocEdu=edu;
                break;
            case 3:
            case 16:
                var edu = {};
                edu.GPA = $("#UniD_EntDocEdu_GPA").val();
                m.EntDocEdu = edu;
                break;
            case 4:	// 4	Диплом о высшем образовании
            case 7:	// 7	Диплом о неполном высшем образовании
            case 8:	// 8	Академическая справка
            case 18:// 18   Справка об обучении в другом ВУЗе
            case 19:// 19	Иной документ об образовании
            case 25:// 25	Диплом об окончании аспирантуры (адъюнкатуры)
            case 26:// 26	Диплом кандидата наук
                var edu = {};

                edu.InstitutionName = $("#UniD_EntDocEdu_InstitutionName").val();
                edu.InstitutionAddress = $("#UniD_EntDocEdu_InstitutionAddress").val();
                edu.FacultyName = $("#UniD_EntDocEdu_FacultyName").val();
                edu.BeginDate = $("#UniD_EntDocEdu_BeginDate").val();
                edu.EndDate = $("#UniD_EntDocEdu_EndDate").val();
                edu.CountryID = $('#UniD_EntDocEdu_CountryID').chosen().val();
                edu.EducationFormID = $('#UniD_EntDocEdu_EducationFormID').chosen().val();

                edu.RegistrationNumber=$("#UniD_EntDocEdu_RegistrationNumber").val();
                edu.QualificationName=$("#UniD_EntDocEdu_QualificationName").val();
                edu.SpecialityName=$("#UniD_EntDocEdu_SpecialityName").val();
                edu.DocumentOU=$("#UniD_EntDocEdu_DocumentOU").val();
                edu.GPA=$("#UniD_EntDocEdu_GPA").val();
                var isNostrificatedCheckbox = $("#UniD_EntDocEdu_IsNostrificated_Value");
                if(isNostrificatedCheckbox.length == 1)
                {
                    edu.IsNostrificated = isNostrificatedCheckbox.is(":checked");
                }
                m.EntDocEdu=edu;
                break;     

            case 10: // 10	Диплом победителя/призера всероссийской олимпиады школьников
            case 9: //  9	Диплом победителя/призера олимпиады школьников
                var Olymp={};        
                Olymp.DiplomaTypeID = $('#UniD_EntDocOlymp_DiplomaTypeID').chosen().val();
                Olymp.OlympicID = $('#UniD_EntDocOlymp_OlympicID').chosen().val();
                Olymp.OlympicTypeProfileID = $('#UniD_EntDocOlymp_OlympicTypeProfileID').chosen().val();
                Olymp.FormNumberID = $('#UniD_EntDocOlymp_FormNumberID').chosen().val();
                if(type == 9)
                {
                    Olymp.ProfileSubjectID = $('#UniD_EntDocOlymp_ProfileSubjectID').chosen().val();
                    Olymp.EgeSubjectID = $('#UniD_EntDocOlymp_EgeSubjectID').chosen().val();
                }
                m.EntDocOlymp=Olymp;
                break;

            case 27: //  Диплом победителя/призера IV этапа всеукраинской ученической олимпиады
                var Other = {};        
                Other.DiplomaTypeID = $('#UniD_EntDocOther_DiplomaTypeID').chosen().val();
                Other.OlympicName = $('#UniD_EntDocOther_OlympicName').val();
                Other.OlympicProfile = $('#UniD_EntDocOther_OlympicProfile').val();
                Other.OlympicDate = $('#UniD_EntDocOther_OlympicDate').val();
                Other.OlympicPlace = $('#UniD_EntDocOther_OlympicPlace').val();
                m.EntDocOther = Other;
                break;

            case 28: //  Диплом об участии в международной олимпиаде
                var Other = {};        
                Other.CountryID = $('#UniD_EntDocOther_CountryID').chosen().val();
                Other.OlympicName = $('#UniD_EntDocOther_OlympicName').val();
                Other.OlympicProfile = $('#UniD_EntDocOther_OlympicProfile').val();
                Other.OlympicDate = $('#UniD_EntDocOther_OlympicDate').val();
                Other.OlympicPlace = $('#UniD_EntDocOther_OlympicPlace').val();
                m.EntDocOther = Other;
                break;

            case 29: //  Документ, подтверждающий принадлежность к соотечественникам за рубежом
                var Other = {};        
                Other.CompatriotCategoryID = $('#UniD_EntDocOther_CompatriotCategoryID').chosen().val();
                Other.CompatriotStatus = $('#UniD_EntDocOther_CompatriotStatus').val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;

            case 30: //  Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей
                var Other = {};        
                Other.OrphanCategoryID = $('#UniD_EntDocOther_OrphanCategoryID').chosen().val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;

            case 31: //  Документ, подтверждающий принадлежность к ветеранам боевых действий
                var Other = {};        
                Other.VeteranCategoryID = $('#UniD_EntDocOther_VeteranCategoryID').chosen().val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;

            case 33: //  Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
                var Other = {};        
                Other.ParentsLostCategoryID = $('#UniD_EntDocOther_ParentsLostCategoryID').chosen().val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;

            case 34: //  Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
                var Other = {};        
                Other.StateEmployeeCategoryID = $('#UniD_EntDocOther_StateEmployeeCategoryID').chosen().val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;

            case 35: //  Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
                var Other = {};        
                Other.RadiationWorkCategoryID = $('#UniD_EntDocOther_RadiationWorkCategoryID').chosen().val();
                Other.DocumentName = $('#UniD_DocumentName').val();
                m.EntDocOther = Other;
                break;
                
            case 20: // 20	Диплом чемпиона/призера Олимпийских игр
            case 21: // 21	Диплом чемпиона/призера Паралимпийских игр
            case 22: // 22	Диплом чемпиона/призера Сурдлимпийских игр
            case 23: // 23	Диплом чемпиона мира
            case 24: // 24	Диплом чемпиона Европы
                m.EntDocCustom= { DocumentTypeNameText: $('#UniD_DocumentName').val(), AdditionalInfo: $('#UniD_EntDocCustom_AdditionalInfo').val()};
                break;
            case 19: // 19	Иной документ об образовании
                m.EntDocCustom= { DocumentTypeNameText: $('#UniD_DocumentName').val(), AdditionalInfo: $('#UniD_EntDocCustom_AdditionalInfo').val()};

                m.EntDocEdu={};
                var isNostrificatedCheckbox = $("#UniD_EntDocEdu_IsNostrificated_Value");
                if(isNostrificatedCheckbox.length == 1)
                {
                    m.EntDocEdu.IsNostrificated = isNostrificatedCheckbox.is(":checked");
                }
            case 18: // 18 Справка об обучении в другом ВУЗе
                m.EntDocEdu={};
                var isNostrificatedCheckbox = $("#UniD_EntDocEdu_IsNostrificated_Value");
                if(isNostrificatedCheckbox.length == 1)
                {
                    m.EntDocEdu.IsNostrificated = isNostrificatedCheckbox.is(":checked");
                }
            case 15: // 15	Иной документ
                m.EntDocCustom= { 
                DocumentTypeNameText: $('#UniD_DocumentName').val(),
                AdditionalInfo: $('#UniD_EntDocCustom_AdditionalInfo').val()};
                break;
            case 12: //12	Заключение психолого-медико-педагогической комиссии
            case 13: //13	Заключение об отсутствии противопоказаний для обучения
                break;
            case 11:// 11	Справка об установлении инвалидности
                var Disability = {};
                Disability.DisabilityTypeID = $('#UniD_EntDocDis_DisabilityTypeID').chosen().val();
                m.EntDocDis = Disability;
                //m.EntDocDis = {
                //    DisabilityTypeID: $('#UniD_EntDocDis_DisabilityTypeID').val(),
                //};

                break;
        }
        var $e=null;
        $e=$('#UniD_EntDocSubBall_SubjectBalls_tr');
        if($e.length>0){
            var SubBalls=[];
            $e.find('.subjectList tr:.saved').each(function(i) {
                var $el=$(this);
                SubBalls.push({
                    SubjectID: $el.find('td:first').attr('data-id'), 
                    SubjectName: unescapeHtml($el.find('td:first').html()), 
                    Value:$el.find('td:first').next().html()
                });
            });
            m.EntDocSubBall={SubjectBalls:SubBalls};
        }
    
        $e=$('#UniD_EntDocEge_TypographicNumber');
        if($e.length>0){
            m.EntDocEge={TypographicNumber:$e.val() };    
        }

        //    $e=$('#UniD_EntDocEdu_GPA');
        //    if($e.length>0){
        //    }

        return m;
    }
    
    function UniDSaveNoDblCheck(model, suсcess, error)
    { 
        if ((!model.AttachmentFile)||(model.AttachmentFile == '')) {
            UniDDataSave(model, suсcess, error);
        } else {
            $('#fileUniDForm').ajaxSubmit({
                method: 'POST',
                dataType: 'json',
                url: "/Application/ReceiveFile1",
                error: showAsyncFilePostError,
                success: function (data) {
                    unblockUI();
                    if(isFilePostError($('#postUniDFile'), data)) { return; }
                    model.AttachmentFileID = data.FileID;
                    model.AttachmentFile = model.AttachmentFile.substring(model.AttachmentFile.lastIndexOf('\\') + 1, model.AttachmentFile.length);
                    UniDDataSave(model, suсcess, error);
                }
            });
            blockUI();
        }  
    }

    function isFilePostError($control, data) {
        if (data.BigSize) {
            unblockUI();
            addFileValidationError($control);
            return true;
        }
        return false;
    }
    function addFileValidationError($control) {
        addValidationError($control, 'Размер файла превышает максимальный разрешенный размер в <%= Model.MaxFileSize %>Кб', true);
  }
  function StringIsEmpty(str){    
      if(str==undefined || str==null || str==""){ return true;}
      return false;
  }


  function UniDSave(model, success, error) {
      //    if(model.EntrantDocumentID!='0' || ( StringIsEmpty(model.DocumentNumber) && StringIsEmpty(model.DocumentSeries))){    // Не новый, на дубликат не проверяем
      //      UniDSaveNoDblCheck(model, success, error); return;
      //    }
      doPostAjax('<%= Url.Generate<EntrantController>(x => x.checkExistEntrantDocument(null)) %>', JSON.stringify(model),
          function (data) {
              if(data.IsError) 
              { 
                  infoDialog("Не удалось проверить Документ на дубликат!");  
                  return; 
              }
              if(data.Data.id>0)
              { 
                  // Найден Дубликат.
                  infoDialog("У абитуриента найден документ с теми же данными. Исправьте данные перед сохранением!");        
              }
              else
              { 
                  // Дубликата нет -> Сохраняем
                  UniDSaveNoDblCheck(model, success, error);
              }
          });
  }

    function UniDDataSave(model, success, error) 
    {
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.setEditDocument(null)) %>', JSON.stringify(model),
            function(data) {
                <% if (Model.UniD.DocumentTypeID == 1) { %>
                $('#UniDForm').find('#UniD_EntDocIdentity_BirthDate').removeClass('input-validation-error-fixed');
                if (data.Extra == "BirthDateError") {
                    $('#UniDForm').find('#UniD_EntDocIdentity_BirthDate').addClass('input-validation-error');
                    return;
                }
                $('#UniDForm').find('#UniD_DocumentDate').removeClass('input-validation-error-fixed');
                if (data.Extra == "dDateError") {
                    $('#UniDForm').find('#UniD_DocumentDate').addClass('input-validation-error');
                    return;
                }
                <% } %>
                
                if (!data.IsError) {
                    model.EntrantDocumentID = data.Data.id;
                    if (model.EntDocIdentity) {
                        model.EntDocIdentity.EntrantDocumentID = model.EntrantDocumentID;
                    }
                    $('#UniDForm').find('#UniD_EntrantDocumentID').val(model.EntrantDocumentID);
                    if (success != undefined) {
                        success(model);
                    }
                } else {
                    //infoDialog("При создании заявление произошла ошибка. " + (data.Message == null ? "" : data.Message));
                    if (error != undefined) {
                        error(data.Message);
                    } else {
                        infoDialog("При сохранении заявление произошла ошибка.<br/> " + (data.Message == null ? "" : data.Message));
                    }
              }
          });
  }

  function getUniDocument(DocId, success, error){
      doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEntrantDocument(null)) %>', JSON.stringify({EntrantDocumentID:DocId}),
      function (data) {
          if (!data.IsError) {
              if (success) { success(data.Data); }
          } else {
              infoDialog("При загрузки документа произошла ошибка. " + data.Message);
              if (error) { 
                  error(data.Message); 
              }else{
                  infoDialog("При загрузки документа произошла ошибка. " + data.Message);
              }
          }
      });        
  }

  <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateNumber", Model.UniD.DocumentTypeID) || EntrantDocumentMatrix.isFieledInDoc("OlympicCheck", Model.UniD.DocumentTypeID)) { %>  
    function btnGetEgeDocument(){

        isClickEGE = true;
        var model=UniDGetModel(); // Получаем модель
        // Проверяем  // Далее запоняем модель 
        // Если документа нет, то сохраняем его и получаем EntrantDocumentID
        // model.EntDocSubBall=null;   // Отключаем проверку на предметы и Баллы
        // model.EntDocSubBall.SubjectBalls=null;
        $('#tbEmptySubjects').attr('val-required','');
        $('#tbEmptySubjectsBall').attr('val-required','');
        if(model.EntDocSubBall){    model.EntDocSubBall.notCheck=true;}
        if(UniDCheckModel(model)){
            UniDSave(model
               , function (doc) {
                   var Method="ByIdentityDocument";
                   if(!StringIsEmpty($('#UniD_CertificateNumber').val())){
                       Method="ByCertificate";
                   }              
                   var REModel = {
                       method: Method,
                       ApplicationID: ApplicationId,
                       doc: doc.EntrantDocumentID,
                       regNum: $('#UniD_CertificateNumber').val(),
                       refr: 1,
                       currentYear: 0,
                       DocTypeID:doc.DocumentTypeID,
                       DocId:doc.EntrantDocumentID,
                       OlympicID:$('#UniD_EntDocOlymp_OlympicID').val(),
                       OlympicTypeProfileID:$("#UniD_EntDocOlymp_OlympicTypeProfileID").chosen().val()
               };
                   if(window.getEgeDocument){
                       getEgeDocument(REModel, function(d){ // success
                           isClickEGE = false;
                           // Проверка прошла успешно
                           if(d.violationId == 0){
                               if(doc.DocumentTypeID=="10" || doc.DocumentTypeID=="9"){
                                   infoDialog("Результаты участия абитуриента в олимпиаде подтверждены, ошибок не обнаружено");
                               }else{
                                   closeDialog($('#UniDDialog'));
                               }
                               /*                  getUniDocument(doc.EntrantDocumentID
                                                   , function(d){
                                                     if(d.EntDocSubBall){
                                                       if(d.EntDocSubBall.SubjectBalls){
                                                         $('#UniDForm').find('#UniD_EntrantDocumentID').val(d.EntrantDocumentID);
                                                         $('UniD_EntDocSubBall_SubjectBalls_tr').find('tbody tr[id!=trAddNewSubjectBall]').remove();
                                                         SubjectInitialFillData(d.EntDocSubBall.SubjectBalls);                        
                                                       }
                                                     }
                                                   }, function(e){
                                                      infoDialog("Не удалось загрузить документ! " + e);
                                                   }
                                                   );
                               */
                           }else{  // Не хорошо
                               //infoDialog(d.violationMessage);
                               if(doc.DocumentTypeID=="10" ||doc.DocumentTypeID=="9"){
                                   //infoDialog("По данному диплому не найдены результаты участия в олимпиаде");
                                   //infoDialog(d.violationMessage);
                               }else{                  
                               }
                           }
                       }, function(){  // error
                           //  Проверка не дала результата.
                           //infoDialog("Проверка не пройдена. Данные не найдены!");
                       }
                       );
                   }
               }, function (e) {
                   infoDialog("Не удалось сохранить документ! " + e);
               }
            ); // end UniDSave
        }
    }
    <% } %>
</script>
<script language="javascript" type="text/javascript">
    function UniDDocumentEdit(EntrantDocumentID, DocumentTypeID, baseModel, success, error, closeEditDialog) {
        var $Dialog = null;
        $Dialog = $('#UniDDialog');
        if ($Dialog.length == 0) {
            $Dialog = $('<div id="UniDDialog" style="display:none, position:fixed"></div>');
            $('body').append($Dialog);
        }
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null, null, null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: DocumentTypeID }, function (data) {
            $Dialog.html(data);
            UniDFormInit();
            $Dialog.dialog({
                modal: true,
                width: 800,
                title: "Документ",
                buttons: {
                    "Сохранить": function () {
                        // В baseModel. EntrantID должен быть ID!!!
                        var doc = UniDPrepareModel(baseModel);
                        if (doc != null || doc != undefined) {
                            // Сохранение, если соханилось нормальн то закрыть и обновить.
                            UniDSave(doc
               , function (model) {
                   if (success) { success(model); }
                   //closeDialog($('#UniDDialog'));
                   closeDialog($Dialog);
               }, function (e) {
                   if (error) {
                       error(e);
                   } else {
                       infoDialog("Не удалось сохранить документ! " + e);
                   }
               }
              ); // end UniDSave
                        }
                    },
                    "Закрыть": function () {
                        $(this).dialog('close');
                        //if (cancel) { cancel();}
                    }
                }, close: function () {
                    //  $('#UniDDialog').empty(); 
                    //  $('#UniDDialog').remove();
                    $Dialog.remove();
                    if (closeEditDialog) { closeEditDialog(); }
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }
</script>
<% if (Model.UniD.EntDocEdu != null) { %>
<script type="text/javascript">

</script>
<% } %>
<% if (Model.UniD.EntDocSubBall != null) { %>
<script type="text/javascript">
    var SubjectList = JSON.parse('<%= Html.Serialize(Model.UniD.EntDocSubBall.SubjectList) %>');

    function UniD_EntDocSubBall_Init() {
        <% if (EntrantDocumentMatrix.isFieledInDoc("SubjectEgeSertificate", Model.UniD.DocumentTypeID))
    { %>  
        var s=$('.subjectList');
        s.find('#btnAddNewSubjectBall').click(function() {
            cancelEditing();
            addEditRow(s.find('#trAddNewSubjectBall'));
            s.find('#trAddNewSubjectBall').hide();
        });
        var subjectData=<%= Html.Serialize(Model.UniD.EntDocSubBall.SubjectBalls) %>;
      if(subjectData==undefined){ subjectData=[];}
      SubjectInitialFillData(subjectData);
    <% } %>
    }
  <% if (EntrantDocumentMatrix.isFieledInDoc("SubjectEgeSertificate", Model.UniD.DocumentTypeID))
    { %>

    var $trEdited = null;
    function SubjectInitialFillData(subjectData) {
        for (var i = 0; i < subjectData.length; i++){
            createAddedRow($('#trAddNewSubjectBall'), subjectData[i]);
        }
    }
    function createAddedRow($trBefore, item) {
        var className = $trBefore.prev().hasClass('trline1') ? 'trline2' : 'trline1';
        $trBefore.before('<tr class="saved ' + className + '">'
                    +'<td data-id="'+item.SubjectID+'" >' + escapeHtml(item.SubjectName) + '</td>'
                    +'<td align="center">' +  (item.Value == null ? '' : item.Value) + '</td>'
                    +'<td align="center" nowrap="nowrap">'
                      +'<a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;' 
                      +'<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a>'
                    +'</td>'
                    +'</tr>');
        $('#tbEmptySubjectsBall').val('1');
    }

    function addEditRow($trToAdd) {
        $trToAdd.before('<tr class="trUnsaved">'
            + '<td><select class="subjectNames" /></td>'
            + '<td align="center"><input type="text" class="subjectValue numeric" maxlength="7" style="width:84px;padding-right:1px;" /></td>'
            + '<td align="center" nowrap="nowrap">'
            + '<a href="#" title="Сохранить" class="btnSaveS" onclick="saveNewRow(this);return false"></a>&nbsp;'
            + '<a href="#" title="Удалить"   class="btnDeleteUS"  onclick="cancelEditing();return false"></a>'
            + '</td>'
            + '</tr>');

        if ($('#UniD_DocumentTypeID').val() == 2) {
            for (var i = 0; i < SubjectList.length; i++) {
                if (SubjectList[i].IsEge) {
                    $('.subjectNames').append("<option value='" + SubjectList[i].SubjectID + "'>" + escapeHtml(SubjectList[i].SubjectName) + "</option>");
                }
            }
        } else {
            for (var i = 0; i < SubjectList.length; i++) {
                $('.subjectNames').append("<option value='" + SubjectList[i].SubjectID + "'>" + escapeHtml(SubjectList[i].SubjectName) + "</option>");
            }
        }
    }

    function cancelEditing() {
        $('#trAddNewSubjectBall').show();
        $('.trUnsaved').remove().detach();
        if ($trEdited != null) $trEdited.show();
        $trEdited = null;
    }
    function deleteRow(el) {
        $(el).parents('tr:first').remove().detach();
        if ($('.subjectList tr:.saved').length == 0){    $('#tbEmptySubjectsBall').val('');} else{  $('#tbEmptySubjectsBall').val('1');}
        $('#trAddNewSubjectBall').focus().blur();
    }
    function saveNewRow(el) {
        var $tr = $(el).parents('tr:first');
        var $tbl = $(el).parents('table:first');
        var val = $tr.find('.subjectValue').val().replace(',', '.');
        var valFloat = new Number(val);
        clearValidationErrors($tr);
        $('#UniD_EntDocSubBall_divErrorPlace').html('');
        if (valFloat <= 0 || valFloat > 100 || isNaN(valFloat)) {
            $('#UniD_EntDocSubBall_divErrorPlace').html('<span class="field-validation-error">Балл должен быть числом от 1 до 100</span>');
            $tr.find('.subjectValue').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            return;
        }
        var o={ SubjectID: $tr.find('.subjectNames').val(), SubjectName: $tr.find('.subjectNames :selected').text(), Value: val };

        var fnd=$tbl.find('tr:visible.saved td[data-id='+o.SubjectID+']');
        if(fnd.length>0){
            infoDialog("Дублируется название дисциплины: "+o.SubjectName);
            return;
        }

        createAddedRow($tr, o);
        $tr.remove().detach();
        $('#trAddNewSubjectBall').show();
        if ($trEdited != null){ $trEdited.remove().detach();}
        $trEdited = null;
    }
    function editRow(el) {
        cancelEditing();
        var $tr = $(el).parents('tr:first');
        var $tbl = $(el).parents('table:first');
        $trEdited = $tr;
        addEditRow($tr);
        $tbl.find('.subjectNames').val($tr.find('td:first').attr('data-id'));
        $tbl.find('.subjectValue').val(unescapeHtml($tr.find('td:first').next().html()));
        $trEdited.hide();
        return false;
    }
    <% } %>
</script>
<% } %>
<% if (Model.UniD.EntDocOlymp != null) { %>
<script type="text/javascript">
    
    function UniD_EntDocOlymp_Init(numDoc) {
        $(".chosen").chosen({ max_selected_options: 1, search_contains: true });
        $(".chosen-deselect").chosen({ allow_single_deselect: true });
        $(".chosen").chosen().change();
        $(".chosen").trigger('liszt:updated');

        $('#UniD_DocumentSeries').removeAttr('val-required');
        $('#UniD_DocumentNumber').removeAttr('val-required');
         
        DrawYear(0);
    }

    function DrawYear(val){
        if(val > 0)
            $('#td_UniD_EntDocOlymp_OlympicYear').html("Год проведения: " + val);
        else
            $('#td_UniD_EntDocOlymp_OlympicYear').html("<- Выберите олимпиаду");
    }

    // обработка выбора олимпиады
    function ChangeOlympicType(value){
        var OlympicID = value;

        doPostAjax('<%= Url.Generate<EntrantController>(x => x.GetOlympicData(null)) %>', 
            JSON.stringify({ 
                OlympicID: OlympicID 
            }),
            function (data) {
                if (!data.IsError) {
                    var d = data.Data;

                    //но модель не рефрешится и здесь могут быть битые данные !!!
                    var oldSelectedValue = '<%= Model.UniD.EntDocOlymp.OlympicTypeProfileID %>';
                    var oldValueFound = false; 

                    DrawYear(d.OlympicYear);
                    $('#td_UniD_EntDocOlymp_LevelName').html("");

                    var options = $("#UniD_EntDocOlymp_OlympicTypeProfileID");
                    options.empty();
                    options.append($("<option />").val(0).text("Не выбрано"));
                    $.each(d.OlympicTypeProfile, function() {
                        options.append($("<option />").val(this.OlympicTypeProfileID).text(this.OlympicProfile.ProfileName));
                        if(this.OlympicTypeProfileID == oldSelectedValue)
                        {
                            oldValueFound = true;
                        }
                    });

                    if ((oldValueFound)&&(oldSelectedValue != null)){
                        //устанавливаем профиль из модели, но он возможно несвежий !!!
                        $("#UniD_EntDocOlymp_OlympicTypeProfileID").chosen().val(oldSelectedValue);
                    }
                    //debugger;
                    options.trigger("chosen:updated");
                    ChangeOlympicTypeProfile($("#UniD_EntDocOlymp_OlympicTypeProfileID").chosen().val());

                } else {
                    infoDialog("При создании заявления произошла ошибка. " + data.Message);
                }
            });
    }

    function ChangeOlympicTypeProfile(value){ 
        //debugger;
        var OlympicTypeProfileID = value;
        $('#td_UniD_EntDocOlymp_LevelName').html("");
        
        if((value=='')||(value=='0'))
        {
            $('#td_UniD_EntDocOlymp_LevelName').html('');
            var options = $("#UniD_EntDocOlymp_ProfileSubjectID");
            options.empty();
            options.append($("<option />").val(0).text("Не выбрано"));

            options.trigger("chosen:updated");
        } 
        else 
        {
            doPostAjax('<%= Url.Generate<EntrantController>(x => x.GetOlympicProfileDetails(null)) %>', 
                JSON.stringify({ 
                    OlympicTypeProfileID: OlympicTypeProfileID 
                }),
                function (data) {
                    if (!data.IsError) {
                        if( data.Data.OlympicTypeProfile!=null)
                        {
                            var level = data.Data.OlympicTypeProfile.OlympicLevel;
                            if(level != null)
                            {
                                $('#td_UniD_EntDocOlymp_LevelName').html("Уровень олимпиады: " + level.Name);
                            }
                        }
                        var subjects = data.Data.OlympicTypeProfileSubjects;
                        //debugger;
                        if((subjects!=null) && (subjects.length) && (subjects.length>0))
                        {
                            var oldSelectedValue = '<%= Model.UniD.EntDocOlymp.ProfileSubjectID %>'; 
                            var oldValueFound = false; 

                            var options = $("#UniD_EntDocOlymp_ProfileSubjectID");
                            options.empty();
                            options.append($("<option />").val(0).text("Не выбрано"));
                            if(subjects.length==1)
                            {
                                options.append($("<option selected='selected' />").val(subjects[0].SubjectID).text(subjects[0].Name));
                                if(subjects[0].SubjectID == oldSelectedValue)
                                {
                                    oldValueFound = true;
                                }
                            }
                            else 
                            {
                                $.each(subjects, function() {
                                    options.append($("<option />").val(this.SubjectID).text(this.Name));
                                    if(this.SubjectID == oldSelectedValue)
                                    {
                                        oldValueFound = true;
                                    }
                                });

                                if((oldValueFound)&&(oldSelectedValue != null)){
                                    $("#UniD_EntDocOlymp_ProfileSubjectID").chosen().val(oldSelectedValue);
                                }
                            }

                            options.trigger("chosen:updated");

                            var profileSubjID = $("#UniD_EntDocOlymp_ProfileSubjectID").chosen().val();

                            var egeVal = $("#UniD_EntDocOlymp_EgeSubjectID").val();                            
                            if(egeVal==0)
                            {
                                var egeOptions = $("#UniD_EntDocOlymp_EgeSubjectID").find("option[value='"+profileSubjID+"']");                         
                                if(egeOptions.length == 1)
                                {
                                    egeOptions.parent().val(profileSubjID);
                                    egeOptions.parent().trigger("chosen:updated");
                                }
                            }
                        }
                        else
                        {
                            var options = $("#UniD_EntDocOlymp_ProfileSubjectID");
                            options.empty();
                            options.append($("<option />").val(0).text("Не выбрано"));

                            options.trigger("chosen:updated");
                        }
                    }
                });
        }
    }

    function ChangeOlympicProfileSubject(value)
    {
        if(value!=0)
        {
            var egeVal = $("#UniD_EntDocOlymp_EgeSubjectID").val();                            
            if(egeVal==0)
            {
                var egeOptions = $("#UniD_EntDocOlymp_EgeSubjectID").find("option[value='"+value+"']");                         
                if(egeOptions.length == 1)
                {
                    egeOptions.parent().val(value);
                    egeOptions.parent().trigger("chosen:updated");
                }
            }
        }
    }

    function CheckAndGet(){
        confirmDialog("Внимание, для проверки олимпиады документ будет сохранен и прикреплен к заявлению. Вы уверены, что хотите продолжить?", 
            function () {
                btnGetEgeDocument();
            })
    }
    
</script>
<% } %>
<% if (Model.UniD.EntDocOther != null) { %>
<script type="text/javascript">
    function UniD_EntDocOther_Init(numDoc) { 
        $(".chosen").chosen({ max_selected_options: 1, search_contains: true });
        $(".chosen-deselect").chosen({ allow_single_deselect: true });
        $(".chosen").chosen().change();
        $(".chosen").trigger('liszt:updated');

        $("#UniD_EntDocOther_OlympicDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0', maxDate: new Date() });

        $('#UniD_DocumentSeries').removeAttr('val-required');
        $('#UniD_DocumentNumber').removeAttr('val-required');
    }
</script>
<% } %>
