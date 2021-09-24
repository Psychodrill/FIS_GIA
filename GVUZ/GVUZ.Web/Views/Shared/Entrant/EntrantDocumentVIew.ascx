<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.UniDocuments.UniDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Helper"%>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Model.Entrants.UniDocuments"  %>
<table id="UniDForm" class="data">
    <thead>
        <tr>
            <th class="caption">
            </th>
            <th>
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentTypeID)%>
            </td>
            <td>
                <b>
                    <%= Model.UniD.DocumentTypeName%></b>
                <%=Html.Hidden("UniD_DocumentTypeID",   Model.UniD.DocumentTypeID)%>
                <%=Html.Hidden("UniD_EntrantID",        Model.UniD.EntrantID)%>
                <%=Html.Hidden("UniD_EntrantDocumentID",Model.UniD.EntrantDocumentID)%>
            </td>
        </tr>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentName", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentName)%>
            </td>
            <td>
                <%= Model.UniD.DocumentName%>
            </td>
        </tr>
        <% } %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.UID)%>
            </td>
            <td>
                <%= Model.UniD.UID %>
            </td>
        </tr>
        <% if (Model.UniD.EntDocIdentity != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("IdentityDocumentTypeID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.IdentityDocumentTypeID)%>
            </td>
            <td>
                <b>
                    <%= Model.UniD.EntDocIdentity.IdentityDocumentTypeName%></b>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentSeries", Model.UniD.DocumentTypeID) && EntrantDocumentMatrix.isFieledInDoc("DocumentNumber", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentSeriesNumber)%>
            </td>
            <td>
                <%= Model.UniD.DocumentSeries %>
                <%= Model.UniD.DocumentNumber %>
            </td>
        </tr>
        <% } %>
        <%
           else
           {%>
        <% if (EntrantDocumentMatrix.isFieledInDoc("Number", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.Number)%>
            </td>
            <td>
                <%= Model.UniD.Number %>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateNumber", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.CertificateNumber)%>
            </td>
            <td>
                <%= Model.UniD.CertificateNumber%>
            </td>
        </tr>
        <% } %>
        <% if (Model.UniD.EntDocEge != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("TypographicNumber", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEge.TypographicNumber) %>
            </td>
            <td>
                <%= Model.UniD.EntDocEge.TypographicNumber %>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentDate", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentDate)%>
            </td>
            <td>
                <%= Model.UniD.DocumentDate.HasValue ? Model.UniD.DocumentDate.Value.ToString("d") : ""%>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("CertificateYear", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentYear)%>
            </td>
            <td>
                <%= Model.UniD.DocumentYear %>
            </td>
        </tr>
        <% } %>
        <% if (Model.UniD.EntDocIdentity != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("SubdivisionCode", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.SubdivisionCode)%>
            </td>
            <td>
                <%= Model.UniD.EntDocIdentity.SubdivisionCode %>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentOrganization", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.DocumentOrganization)%>
            </td>
            <td>
                <%= Model.UniD.DocumentOrganization %>
            </td>
        </tr>
        <% } %>
        <% if (Model.UniD.EntDocIdentity != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("GenderTypeID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.GenderTypeID)%>
            </td>
            <td>
                <%= Model.UniD.EntDocIdentity.GenderTypeName %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("NationalityTypeID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.NationalityTypeID)%>
            </td>
            <td>
                <%= Model.UniD.EntDocIdentity.NationalityTypeName %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("BirthDate", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.BirthDate)%>
            </td>
            <td>
                <%= Model.UniD.EntDocIdentity.BirthDate.ToString("dd.MM.yyyy")%>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("BirthPlace", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocIdentity.BirthPlace)%>
            </td>
            <td>
                <%= Model.UniD.EntDocIdentity.BirthPlace %>
            </td>
        </tr>
        <% } %>
        <% } %>
        
        <% if (Model.UniD.EntDocEdu != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("CountryID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.CountryID)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.Country %>
            </td>
        </tr>
             <% } %>
            

        <% if (EntrantDocumentMatrix.isFieledInDoc("InstitutionName", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.InstitutionName)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.InstitutionName %>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("InstitutionAddress", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.InstitutionAddress)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.InstitutionAddress %>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("FacultyName", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.FacultyName)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.FacultyName %>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("BeginDate", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.BeginDate)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.BeginDate %>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("EndDate", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.EndDate)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.EndDate %>
            </td>
        </tr>
        <% } %>

        <% if (EntrantDocumentMatrix.isFieledInDoc("EducationFormID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.EducationFormID)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.EducationForm %>
            </td>
        </tr>
        <% } %>


        <% if (EntrantDocumentMatrix.isFieledInDoc("RegistrationNumber", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.RegistrationNumber)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.RegistrationNumber %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("QualificationName", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.QualificationName)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.QualificationName%>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("SpecialityName", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.SpecialityName)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.SpecialityName %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DocumentOU", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.DocumentOU)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.DocumentOU %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("GPA", Model.UniD.DocumentTypeID)) { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.GPA)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.GPA %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc(EntrantDocumentMatrix.Field_IsNostrificated, Model.UniD.DocumentTypeID, Model.UniD.EntrantID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.IsNostrificated)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.IsNostrificatedStr %>
            </td>
        </tr>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("StateServicePreparation", Model.UniD.DocumentTypeID, Model.UniD.EntrantID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocEdu.StateServicePreparation)%>
            </td>
            <td>
                <%= Model.UniD.EntDocEdu.StateServicePreparationStr %>
            </td>
        </tr>
        <% } %>
        <% } %>

        <%-- 27,28,29,30 --%>
        <% if (Model.UniD.EntDocOther != null) { %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicName", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.OlympicName)%> </td>
                <td> <%= Model.UniD.OlympicName%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicProfile", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OlympicProfile)%> </td>
                <td> <%= Model.UniD.EntDocOther.OlympicProfile%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicDate", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OlympicDate)%> </td>
                <td> <%= Model.UniD.EntDocOther.OlympicDate != null ? Model.UniD.EntDocOther.OlympicDate.Value.ToShortDateString() : ""%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicPlace", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OlympicPlace)%> </td>
                <td> <%= Model.UniD.EntDocOther.OlympicPlace%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("CountryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.CountryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.Country%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("CompatriotCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.CompatriotCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.CompatriotCategory%> </td>

            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("CompatriotStatus", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.CompatriotStatus)%> </td>
                <td> <%= Model.UniD.EntDocOther.CompatriotStatus%> </td>

            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("OrphanCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.OrphanCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.OrphanCategory%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("VeteranCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.VeteranCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.VeteranCategory%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("ParentsLostCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.ParentsLostCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.ParentsLostCategory%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("StateEmployeeCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.StateEmployeeCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.StateEmployeeCategory%> </td>
            </tr>
            <% } %>

            <% if (EntrantDocumentMatrix.isFieledInDoc("RadiationWorkCategoryID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption"> <%= Html.TableLabelFor(m => m.UniD.EntDocOther.RadiationWorkCategoryID)%> </td>
                <td> <%= Model.UniD.EntDocOther.RadiationWorkCategory%> </td>
            </tr>
            <% } %>

        <% } %>


        <%-- олимпиада ОШ/ВОШ --%>
        <% if (Model.UniD.EntDocOlymp != null) { %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicDiplomaTypeID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.DiplomaTypeID)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.DiplomaTypeName%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicID)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.OlympicDetails.OlympicName%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicProfSubject", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicTypeProfileID)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.ProfileText%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicSubject", Model.UniD.DocumentTypeID))
               { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.ProfileSubjectID)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.ProfileSubjectName%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicEgeSubject", Model.UniD.DocumentTypeID))
               { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.EgeSubjectID)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.EgeSubjectName%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicLevelID", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicDetails.LevelName)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.Level%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OlympicYear", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicDetails.OlympicYear)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.Year%>
                </td>
            </tr>
            <% } %>
            <% if (EntrantDocumentMatrix.isFieledInDoc("OrganizerName", Model.UniD.DocumentTypeID)) { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.OlympicDetails.OrganizerName)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.Organizer%>
                </td>
            </tr>
            <% } %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => m.UniD.EntDocOlymp.ApprovedText)%>
                </td>
                <td>
                    <%= Model.UniD.EntDocOlymp.ApprovedText%>
                </td>
            </tr>
        <% } %>

        <% if (Model.UniD.EntDocDis != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("DisabilityTypeID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocDis.DisabilityTypeID)%>
            </td>
            <td>
                <%= Model.UniD.EntDocDis.DisabilityType%>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (Model.UniD.EntDocCustom != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("AdditionalInfo", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.EntDocCustom.AdditionalInfo) %>
            </td>
            <td>
                <%= Model.UniD.EntDocCustom.AdditionalInfo%>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (Model.UniD.EntDocSubBall != null)
           { %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("SubjectEgeSertificate", Model.UniD.DocumentTypeID))
           { %>
        <tr id="UniD_EntDocSubBall_SubjectBalls_tr">
            <td class="caption">
            </td>
            <td>
                <div id="UniD_EntDocSubBall_divErrorPlace">
                </div>
                <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                    <thead>
                        <tr>
                            <th style="width: 60%">
                                <%= Html.LabelFor(x => x.UniD.EntDocSubBall.FieldInfo.SubjectID) %>
                            </th>
                            <th style="width: 25%">
                                <%= Html.LabelFor(x => x.UniD.EntDocSubBall.FieldInfo.Value) %>
                            </th>
                            <th style="width: 40px">
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <%foreach (var item in Model.UniD.EntDocSubBall.SubjectBalls)
                          {
                        %>
                        <tr>
                            <td>
                                <%=item.SubjectName %>
                            </td>
                            <td>
                                <%=item.Value %>
                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </td>
        </tr>
        <% } %>
        <% } %>
        <% if (EntrantDocumentMatrix.isFieledInDoc("AttachmentID", Model.UniD.DocumentTypeID))
           { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.UniD.AttachmentFileID)%>
            </td>
            <td>
                <% if (Model.UniD.AttachmentFileID != Guid.Empty && Model.UniD.AttachmentFileID != null)
                   { %>
                <div>
                    <a fileid="<%= Model.UniD.AttachmentID %>" href="/Entrant/GetFile1?fileID=<%=Model.UniD.AttachmentFileID %>"
                        class="getFileLink">
                        <%: Model.UniD.AttachmentName%></a></div>
                <% } %>
            </td>
        </tr>
        <% } %>
    </tbody>
</table>
