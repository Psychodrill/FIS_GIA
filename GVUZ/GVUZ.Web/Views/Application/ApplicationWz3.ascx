<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz3ViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>


<script type="text/javascript">
    function GetComposition() {
        blockUI();
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetComposition(null)) %>', 
            JSON.stringify({ ApplicationID: ApplicationId }),
            function (data) {
                unblockUI();
                if (data.IsError) {
                    infoDialog('Превышено время ожидания');
                    if (error) {
                        error();
                    }
                    if (window.ShowWz != undefined) {
                        window.ShowWz(3);
                    }
                    if (window.loadPaView != undefined) {
                        window.loadPaView(2);
                    }
                    return;
                }
                var comps = data.Data;
                var $el = $('#docComposition tbody');
                $el.empty();
                var htm = '';
                for (var i = 0; i < comps.length; i++) {
                    comps[i];
                    htm = '<tr>';
                    htm += '<td>' + comps[i].strExamDate + '</td>';
                    htm += '<td>' + comps[i].acrName + '</td>';
                    htm += '<td>' + (comps[i].acrResult ? 'Зачет' : 'Незачет') + '</td>';
                    htm += '<td>' + (comps[i].HasAppeal.HasValue && comps[i].HasAppeal.Value ? 'Да' : 'Нет') + '</td>';
                    htm += '<td>' + comps[i].AppealStatus + '</td>'; 
                    htm += '<td>' + comps[i].strDownloadDate + '</td>';
                    htm += '</tr>';
                    $el.append(htm)
                }
                if (comps.length == 0) {
                    $('#CompositionNoFound').show();
                    $('#CompositionTable').hide();
                } else {
                    $('#CompositionNoFound').hide();
                    $('#CompositionTable').show();
                }
            });
    }

    function ViewComposition() {
        window.open('/Application/ViewComposition/' + ApplicationId, '_blank');
    }

</script>

<%=Html.Hidden("IsDisabledDocumentID",Model.IsDisabledDocumentID)%>

<div id="actual">
    <% if (Model.ShowDenyMessage) { %>
    <div>Невозможно редактировать данное заявление</div>
    <script type="text/javascript">
        function Wz3Init() { return false; }
        function Wz3Save(success, error) { if (success) { success({}); } }
        function Wz3Cancel() { }
    </script>
    <%} else { %>
    <%  if (true) { %>
    <div id="compositionResults">
        <div class="statementtitle">
            Результаты итогового сочинения
        </div>
        <div id='CompositionNoFound' style='display: none'>
            Результаты сочинения не найдены
        </div>
        <div id="CompositionTable" class="statementborder">
            <table class="gvuzDataGrid" cellpadding="3" id="docComposition">
                <thead>
                    <tr>
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.ExamDate)%>
                        </th>
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.acrName)%>
                        </th>
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.acrResult)%>
                        </th>
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.HasAppeal)%>
                        </th>
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.AppealStatus)%>
                        </th> 
                        <th>
                            <%= Html.LabelFor(x => x.BaseComposition.DownloadDate)%>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%foreach (var cr in Model.CompositionResult)
                        {%>
                    <tr>
                        <td>
                            <%= cr.strExamDate %>
                        </td>
                        <td>
                            <%= cr.acrName %>
                        </td>
                        <td>
                            <%if (cr.acrResult)
                                {%>Зачет<%}
                                        else
                                        {%>Незачет<%}%>
                        </td>
                        <td>
                            <%if (cr.HasAppeal.HasValue && cr.HasAppeal.Value)
                                {%>Да<%}
                                         else
                                         {%>Нет<%}%>
                        </td>
                        <td>
                            <%= cr.AppealStatus %>
                        </td> 
                        <td>
                            <%= cr.strDownloadDate %>
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        <br />

        <div id="compositionResultsContent" style="margin-top: 8px; margin-bottom: 8px">
        </div>

        <table>
            <thead style="display: none;">
                <tr>
                    <td style="width: 20%"></td>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                    <td style="width: 30%"></td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <input type="button" id="getComposition" value="Получить результаты сочинения" class="button3"
                            style="white-space: nowrap; width: auto" onclick="GetComposition()" />
                    </td>
                    <td>
                        <input type="button" id="viewComposition" value="Просмотреть бланки сочинений" class="button3"
                            style="white-space: nowrap; width: auto" onclick="ViewComposition()" />

                    </td>
                    <td>
                        <%= Html.CheckBoxFor(x => x.IsDisabled, new { onclick = "ChektIsDisabled();" })%>
                        <label for="IsDisabled">ВИ с созданием специальных условий</label>
                        <div id="linkAddSpecialDoc">
                            <a href="#" onclick="addSpecialDoc(1); return false;">Подтверждающий документ</a>
                            <span id="krymdocnumber"></span>
                        </div>
                    </td>
                    <td>
                        <%= Html.CheckBoxFor(x => x.IsDistant, new { onclick = "ChektIsDistant();" })%>
                        <label for="IsDistant">ВИ с использованием дистанционных технологий</label>
                        <div id="divDistantPlace">
                            <label for="DistantPlace" class="caption">Место сдачи ВИ:</label>
                            <%= Html.TextBoxFor(x => x.DistantPlace, new { style = "width:180px;" })%>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
    <% } %>
    <div id="content">
        <% 
            foreach (var G in Model.AppComGroups)
            { %>
        <div class="statementtitle">Конкурс: <span class="statementsubtitle"><%=G.GroupName %></span></div>
        <div class="divGeneralBenefits" style="padding-bottom: 10px" groupid="<%=G.GroupID %>"
            hasbenefits="<%=G.HasBenefits %>">
            <span style="font-size: 12pt; font-weight: 500;" class="commonBenefitsB">Общие льготы:</span>&nbsp;&nbsp;&nbsp;
        <% foreach (var D in G.BenefitDocuments)
            {// Проверка общих документов %>
            <p benefit="<%=D.BenefitID %>" doc-id="<%=D.ID %>">
                <span><span style="font-size: 12pt">
                    <%=D.BenefitName %></span><span class="statementsubtitle"> (<% =D.Description%></span>
                </span><a href="#" class="btnDelete action-edit" onclick="detachETCGDocument(this, <%= D.ID %>);return false;" title="Удалить результаты">&nbsp;</a>
            </p>
            <% }%>
            <% if (G.Course == 1)
                { %><br />
            <a href="#" onclick="addNoExETValue(this, <%=G.GroupID  %>); return false;">Зачисление  без вступительных испытаний</a>&nbsp;&nbsp;&nbsp;
        <% } %>
            <% if (G.IsQuotaBenefitEnabled)
                { %>
            <a id="quotaRef" href="#" onclick="addDisETValue(this, <%=G.GroupID %>); return false;">По квоте приёма лиц, имеющих особое право</a> &nbsp;&nbsp;&nbsp;
        <% } %>
            <a href="#" onclick="addPrefETValue(this, <%=G.GroupID %>); return false;">Преимущественное
            право на поступление</a>
        </div>
        <div class="statementborder">
            <table id="CG<%=G.GroupID %>" class="gvuzDataGrid docGrid1" cellpadding="3" groupid="<%=G.GroupID %>" hasbenefits="<%=G.HasBenefits %>">
                <thead>
                    <th style="width: 20%"><%= Html.LabelFor(x => G.EntranceTestItemDescr.SubjectNameView)%></th>
                    <th><%= Html.LabelFor(x => G.EntranceTestItemDescr.Priority)%></th>
                    <th style="width: 80px" nowrap="nowrap" id="thResultValue"><%= Html.LabelFor(x => G.EntranceTestItemDescr.ResultValue)%></th>
                    <th><%= Html.LabelFor(x => G.EntranceTestItemDescr.EgeResultValue)%></th>
                    <th><%= Html.LabelFor(x => G.EntranceTestItemDescr.SourceID)%></th>
                </thead>
                <tbody>
                    <% if (false)
                        { // Model.EntranceTests.Length==0 %>
                    <tr>
                        <td colspan="3" align="center">Документы не требуются   </td>
                    </tr>
                    <% } %>
                    <% var idx = 0;
                        var prevETType = EntranceTestType.MainType;
                        foreach (var I in G.TestItems)
                        { %>
                    <% if (I.EntranceTestTypeID != prevETType)
                        {
                            idx++;
                            prevETType = I.EntranceTestTypeID.Value;
                    %>
                    <tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>">
                        <td colspan="3">
                            <b>
                                <%= I.EntranceTestTypeID == EntranceTestType.CreativeType ? "Дополнительные вступительные испытания творческой и (или) профессиональной направленности" : ""%>
                                <%= I.EntranceTestTypeID == EntranceTestType.ProfileType ? "Дополнительные вступительные испытания профильной направленности" : ""%>
                                <%= I.EntranceTestTypeID == 1 ? "Аттестационные испытания" : ""%>
                            </b>
                        </td>
                    </tr>
                    <% }
                        idx++;
                    %>
                    <tr id="eti<%=I.EntranceTestItemID %>" class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>" etiid="<%= I.EntranceTestItemID %>">
                        <td><%= I.SubjectNameView %><% if (I.IsProfileSubject.Value)
                                                        { %><br />
                            <span style="font-size: 7pt">(профильный предмет)</span><%} %></td>
                        <td><%= I.Priority == null ? "Без приоритета" : I.Priority.ToString() %> </td>
                        <td nowrap="nowrap" data-field="ResultValue">
                            <input type="text" data-field="ResultValue" class="numeric <%= (I.SubjectID != null && I.SubjectName == null) && (I.Doc == null || (I.Doc.SourceID == 1 && I.Doc.EntrantDocumentID == null)) ? "" : "view" %>"
                                <%= (I.SubjectID != null && I.SubjectName == null) && (I.Doc == null || (I.Doc.SourceID == 1 && I.Doc.EntrantDocumentID == null)) ? "" : "readonly=\"readonly\"" %>
                                style="width: 70px" data-id="<%=I.Doc == null ? "" : I.Doc.ID.ToString() %>" value="<%  =I.Doc == null ? "" : (I.Doc.ResultValue == null ? "" : I.Doc.ResultValue.Value.ToString("0.####")) %>"
                                maxlength="3" isege="<%=(I.EntranceTestTypeID == EntranceTestType.MainType) ? "1" : "0" %>"
                                onfocus="doManualInputStart(this);" onblur="doManualInputEnd(this);" onchange="doManualInput(this);"
                                onkeyup="return proverka(this);" />
                        </td>
                        <td nowrap="nowrap" data-field="EgeResultValue">
                            <% if (I.Doc != null)
                                {
                                    if (I.Doc.BenefitID == 3 && I.EntranceTestItemID != null)
                                    { %>
                            <input type="text" data-field="EgeResultValue" class="numeric" style="width: 70px"
                                value="<%= I.Doc.EgeResultValue == null ? "" : I.Doc.EgeResultValue.Value.ToString("0.####") %>"
                                maxlength="3" isege="<%=I.EntranceTestTypeID == EntranceTestType.MainType ? "1" : "0" %>"
                                onfocus="doManualInputStart(this);" onblur="doManualInputEnd(this);" onchange="doSaveEgeValueForBenefit(this, '<%=I.Doc.ID %>', '<%= I.Doc.EntrantDocumentID%>','<%= I.Doc.SubjectID %>');" />
                            <% }
                                } %>
                        </td>
                        <td class="source">
                            <% if (I.Doc != null)
                                { %>
                            <span docdescr="1"><%= I.Doc.Description %></span><a href="#" class="btnDelete action-edit" onclick="detachETDocument(this, <%= I.Doc.ID %>); return false;" title="Удалить результаты" docdelid="<%= I.Doc.ID %>">&nbsp;</a>
                            <% }
                                else
                                {%>
                            <% if (I.EntranceTestTypeID == EntranceTestType.MainType && (I.SubjectIsEge == true))
                                { %>
                            <a class="action-edit" href="#" style="padding-right: 15px;" onclick="addEGEETValue(this); return false;">Свидетельство ЕГЭ</a>
                            <% }
                                else
                                { %>
                            <span style="color: #606060; display: none;">Свидетельство ЕГЭ</span>
                            <% } %>
                            <% if (I.EntranceTestTypeID == EntranceTestType.MainType && I.SubjectID > 0)
                                { %>
                            <a class="action-edit" href="#" style="padding-right: 15px;" onclick="addGIAETValue(this); return false;">Справка ГИА</a>
                            <% }
                                else
                                { %>
                            <span style="color: #606060; display: none;">Справка ГИА</span>
                            <%} %>
                            <% if (I.EntranceTestTypeID != EntranceTestType.MainType || true)
                                { %>
                            <a class="action-edit" href="#" id="ET_Ref" style="padding-right: 15px;" onclick="addManualETValue(this);return false;">Вступительное испытание ОО</a>
                            <% }
                                else
                                { %>
                            <span style="color: #606060; display: block;">Вступительное испытание ОО</span>
                            <%} %>
                            <% if (true)
                                { %>
                            <a class="action-edit" href="#" style="padding-right: 15px;" onclick="addDiplomaETValue(this); return false;">Право на 100 баллов</a>
                            <% }
                                else
                                { %>
                            <span style="color: #606060; display: none;" title="Для данной дисцпилины отсутствуют льготы">Право на 100 баллов</span>
                            <%} %>
                            <%} %>
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
        <% } %>
    </div>
    
    <% if (Model.Course < 2) { %>
    <div id="divEgeCheck" style="margin-top: 10px">
        <input type="button" class="button3" style="width: auto" id="btnGetResultEge" value="Получить/проверить результаты ЕГЭ" onclick="btnGetResultEge(true, false, null, null, null)" />
    </div>
    <%} %>

    <div style="display: none">
        <div id="divAddETDocumentFromManual">
            <div style="padding-bottom: 10px">
                <b>У данного абитуриента есть сертификат ЕГЭ текущего года, содержащий оценку по дисциплине<span id="spDocumentTypeSubjectForManual"></span></b>
            </div>
            <div>
                <b>Выбрать существующий документ:</b>
                <div id="divExistingDocumentTypesForManual" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
                </div>
            </div>
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
                <span class="linkSumulator" id="spAddManualETFromEG">Использовать результат внутреннего вступительного испытания</span>
            </div>
        </div>
    </div>
    <% } %>
</div>

<script language="javascript" type="text/javascript">
    ////////////////////ДИАЛОГ////////////////////////////
    function dManualETValue() {
        var $dManualETValue = null;
        $dManualETValue = $('#dManualETValue');
        if ($dManualETValue.length == 0) {
            $dManualETValue = $('<div id="divManualETValue" style="display:none, position:fixed">' +
                '<table class="gvuzData">' +
                '<tr><td class="caption">' + '<%= Html.TableLabelFor(x => x.InsDocDescr.InstitutionDocumentTypeID)%>' + '</td><td>' + '<select id="InsDocDescr_DocumentTypeID" />' + '</td></tr>' +
                '<tr><td class="caption">' + '<%= Html.TableLabelFor(x => x.InsDocDescr.InstitutionDocumentNumber)%>' + '</td><td>' + '<%= Html.TextBoxExFor(x => x.InsDocDescr.DocumentNumber)%>' + '</td></tr>' +
                '<tr><td class="caption">' + '<%= Html.TableLabelFor(x => x.InsDocDescr.InstitutionDocumentDate)%>' + '</td><td>' + '<%= Html.DatePickerFor(x => x.InsDocDescr.DocumentDate)%>' + '<br/><div id="txtDateError"></div>' + '</td></tr>' +
                '<tr><td class="caption">' + '<%= Html.TableLabelFor(x => x.InsDocDescr.ResultValue)%>' + '</td><td>' + '<%= Html.TextBoxExFor(x => x.InsDocDescr.ResultValue, new { maxLength = "8" })%>' + '</td></tr>' +
                '</table>' +
                '</div>');
            //$('body').append($dManualETValue);
        }
        for (var i = 0; i < Wz3.ManualETValueList.length; i++) {
            $dManualETValue.find('#InsDocDescr_DocumentTypeID').append("<option value='" + Wz3.ManualETValueList[i].ID + "'>" + escapeHtml(Wz3.ManualETValueList[i].Name) + "</option>");
        }
        $dManualETValue.find("#InsDocDescr_DocumentDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date() });
        return $dManualETValue;
    }

    function DialogWz3() { 
        var $Dialog = null;
        $Dialog = $('#divAddETDocumentWz3');
        if ($Dialog.length == 0) {
            $Dialog = $('<div id="divAddETDocumentWz3" style="display:none, position:fixed">' +
                '<div id="divCurrentDocumentPart" style="display: none">' +
                '<b>Выбранный документ:</b>' +
                '<div id="divCurrentDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '<div id="divAddDocumentPart" style="display: none">' +
                '<b>Добавить документ:</b>' +
                '<div id="divAddDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '<div id="divExistingDocumentPart" style="display: none">' +
                '<b>Выбрать существующий документ:</b>' +
                '<div id="divExistingDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '</div>');
            $('body').append($Dialog);
        }
        return $Dialog;
    }

</script>
<script language="javascript" type="text/javascript">
    var Wz3 = {};
    Wz3.ManualETValueList = JSON.parse('<%= Html.Serialize(Model.InstitutionDocumentTypes) %>');
     
    var EntrantID=<%=Model.EntrantID %>;
    var clickButton = false;

    function proverka(input) {
        var n = parseInt($(input).val().replace(/\D/g, ''), 10);
        $(input).val(n.toLocaleString());
        if (n >= 0) {
        } else {
            $(input).val("");
        }
    }

    function CheckBenefit(EntranceTestItemID, GroupID, doc, entrantDocumentID, documentTypeId)
    {
        var checkResult = false;
         
        var checkBenefitModel = 
            {
                CompetitiveGroupId :GroupID,
                EntranceTestItemId :EntranceTestItemID,
                EntrantDocumentId:entrantDocumentID,
                DocumentTypeId :documentTypeId,                
            };
        
        if((documentTypeId==9 || documentTypeId==10) && (doc))
        {
            checkBenefitModel.CheckBenefitOlympicModel =
                 {
                     OlympicTypeProfileId : doc.OlympicTypeProfileID,
                     DiplomaTypeId  :doc.DiplomaTypeID,
                     OlympicId :doc.OlympicID,
                     ClassNumber :doc.FormNumberID,
                 }
        }
       
        doPostAjaxSync("<%= Url.Generate<EntrantController>(x => x.CheckBenefit(null)) %>", JSON.stringify(checkBenefitModel),
            function(data)
            {
                if((data.Data.errorMessage) && (data.Data.errorMessage!=''))
                {
                    infoDialog(data.Data.errorMessage);
                    checkResult = false;
                }
                else if(data.Data.violationId > 0)
                {
                    infoDialog(data.Data.violationMessage);
                    checkResult = false;
                }
                else
                {
                    checkResult = true;
                }
            }); 

        return checkResult;
    } 
     
    function EditDocument(EntrantDocumentID, DocumentTypeID, EntranceTestItemID, GroupID, procEntDocID) {
        var $Dialog = null;
        $Dialog = $('#UniDDialog');
        if ($Dialog.length == 0) {
            $Dialog = $('<div id="UniDDialog" style="display:none, position:fixed"></div>');
            $('body').append($Dialog);
        }
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null, null, null)) %>', { DocTypeID: DocumentTypeID, EntrantID:EntrantID }, function(data) {
            $Dialog.html(data);
            UniDFormInit();
            $Dialog.dialog({
                modal: true,
                width: 800,
                title: "Документ",
                buttons: {
                    "Сохранить": function() {
                        var baseModel = { EntrantID: <%=Model.EntrantID %> };
                        var doc = UniDPrepareModel(baseModel);
                        if (doc != null || doc != undefined) 
                        { 
                            // Сохранение, если соханилось нормальн то закрыть и обновить.
                            UniDSave(doc, function(model) {
                                // Получить обновленную модель документа
                                var docid = model.EntrantDocumentID;
                                
                                var saveEntranceTestDocument=true; 
                                if(!CheckBenefit(EntranceTestItemID, GroupID, doc.EntDocOlymp, docid, DocumentTypeID))
                                {  
                                    //Документ сохранится, но с ВИ не свяжется
                                    saveEntranceTestDocument = false;
                                } 

                                // Вызываем процедуру обработки нового документа
                                if ((procEntDocID)&&(saveEntranceTestDocument)) {
                                    procEntDocID(docid);
                                }
                                closeDialog($Dialog);
                            }, function(e) {
                                infoDialog("Не удалось сохранить документ! " + e);
                            });
                        }
                    },
                    "Закрыть": function() {
                        $(this).dialog('close');
                    }
                },
                close: function() {
                    $Dialog.remove();
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }
</script>
<script language="javascript" type="text/javascript">

    function doETDocumentAdd(DocumentTypeID, EntranceTestItemID, GroupID, SourceID) {
        closeDialog($('#divAddETDocumentWz3'));
        EditDocument(0, DocumentTypeID, EntranceTestItemID, GroupID, function (EntrantDocumentID) {
            doETDocumentSelect(EntrantDocumentID, DocumentTypeID, EntranceTestItemID, GroupID, SourceID);
        });
    }

    function doETDocumentSelect(EntrantDocumentID, DocumentTypeID, EntranceTestItemID, GroupID, SourceID, rq, success) {
        closeDialog($('#divAddETDocumentWz3'));

        $('#txtDateError span').text("");
        if (rq == undefined) {
            rq = {};
        }
        rq.ApplicationID = ApplicationId;
        rq.EntrantDocumentID = EntrantDocumentID;
        rq.EntranceTestItemID = EntranceTestItemID;
        rq.CompetitiveGroupID = GroupID;
        rq.SourceID = SourceID;   

        if(specialDoc)
        {
            $('#IsDisabledDocumentID').val(EntrantDocumentID);
            specialDoc = false;
            GetDocInfo(EntrantDocumentID);
            return;
        }
        else
        {
            if(!CheckBenefit(EntranceTestItemID, GroupID, null, EntrantDocumentID,DocumentTypeID))
                return;
        }

        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocSave(null)) %>", JSON.stringify(rq), function (data) {

            $("#InsDocDescr_DocumentDate").removeClass('input-validation-error-fixed');
            if (data.Extra == "DateError") {
                $("#InsDocDescr_DocumentDate").addClass('input-validation-error');
                $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                return;
            }
            if (data.IsError) {
                infoDialog(data.Message.replace(/\n/g, '<br/>'));
                return;
            }
            if (success) {
                success(data.Data);
            }
            //if(data.Data.Doc!=null){  data.Data.Doc.DocumentDate=parseMSJsonDate(data.Data.Doc.DocumentDate); }
            if (data.Data.EntranceTestItemID != null) {
                ETIRowUpd(data.Data);
            } else if (data.Data.Doc != null) {
                if (data.Data.Doc.BenefitID > 0) {
                    ETCGUpd(data.Data);
                }
            }
            // Или в группу
        });
    }

    function SelectDocsList(EntranceTestItemID, SourceID, GroupID, selecedETID) {
        var url = '<%= Url.Generate<ApplicationController>(x => x.SelectEntDocsList(null, null, null, null)) %>';
        doPostAjax(url, JSON.stringify({ ApplicationID: ApplicationId, EntranceTestItemID: EntranceTestItemID, SourceID: SourceID, GroupID: GroupID }),
            function (data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                var ds = data.Data;
                if (ds.EntrantDocuments.length == 0 && ds.DocumentTypes.length == 1) { // Если существующих подходящих документов нет, то 
                    // Если тип только один, то сразу предлагаем новый документ
                    doETDocumentAdd(ds.DocumentTypes[0].ID, EntranceTestItemID, GroupID, SourceID);
                } else {
                    // Составить список документов
                    var $dts;
                    var htm = '';
                    $dts = $('#divAddETDocumentWz3').find('#divAddDocumentTypes');
                    $dts.empty();
                    for (var i = 0; i < ds.DocumentTypes.length; i++) {
                        htm += '<span class="linkSumulator" doctypeid="' + ds.DocumentTypes[i].ID + '" onclick="doETDocumentAdd(' + ds.DocumentTypes[i].ID + ', ' + EntranceTestItemID + ',' + GroupID + ',' + SourceID + ')" >' + ds.DocumentTypes[i].Name + '</span><br/>';
                    }
                    $dts.html(htm);
                    $('#divAddDocumentPart').show();

                    $dts = $('#divAddETDocumentWz3').find('#divExistingDocumentTypes');
                    $dts.empty();
                    htm = '';
                    for (var i = 0; i < ds.EntrantDocuments.length; i++) {
                        htm += '<span class="linkSumulator" docid="' + ds.EntrantDocuments[i].EntrantDocumentID + '" onclick="doETDocumentSelect(' + ds.EntrantDocuments[i].EntrantDocumentID + ', ' +ds.EntrantDocuments[i].EntrantDocumentTypeID+ ', ' + EntranceTestItemID + ',' + GroupID + ',' + SourceID + ')" >' + ds.EntrantDocuments[i].Description + '</span><br/>';
                    }
                    $dts.html(htm);
                    if (ds.EntrantDocuments.length > 0) {
                        $('#divExistingDocumentPart').show();
                    } else {
                        $('#divExistingDocumentPart').hide();
                    }
                    $('#divAddETDocumentWz3').dialog({ modal: true, width: 600, title: 'Выбор документа' }).dialog('open');
                }
            }
        );
    }
</script>
<script language="javascript" type="text/javascript">

    function addNoExETValue(el, GroupID) {
        DialogWz3();
        SelectDocsList(null, 101, GroupID, null);
    }

    function addDisETValue(el, GroupID) {
        DialogWz3();
        SelectDocsList(null, 104, GroupID, null);
    }

    function addPrefETValue(el, GroupID) {
        DialogWz3();
        SelectDocsList(null, 105, GroupID, null);
    }

    function addSpecialDoc(GroupID) {
        DialogWz3();
        specialDoc = true;
        SelectDocsList(null, 107, GroupID, null);
    }

    var ManualInputRun=false;
    var ManualInputValueStart;
    var ManualInputValueEnd;

    function doManualInputStart(el) {
        ManualInputRun = true;
        ManualInputValueStart = $(el).val();
        return true;
    }

    function doManualInputEnd(el) {
        if (ManualInputValueStart == $(el).val()) {
            ManualInputRun = false;
        } else {
            if ($(el).parents('tr:first').find("input[data-field=EgeResultValue]").val() == undefined) {
                doManualInput(el);
            }
        }
        return true;
    }

    function doManualInput(el) {
        var $el = $(el);
        clearValidationErrors($el.parent());
        var etiID = $el.parents('tr:first').attr('etiID');
        $el.removeClass('input-validation-error').removeClass('input-validation-error-fixed');
        if ($el.attr('readonly')) {
            return;
        }

        if ($el.val() == '') {
            var $btnDelete = $el.parents('tr:first').find('.btnDelete');
            if ($btnDelete.length > 0) {
                detachETDocument($btnDelete[0], $btnDelete.attr('docDelID'));
            }
            return;
        }
        var isEGE = $el.attr('isege');
        var bValue = new Number($el.val());
        if (isNaN(bValue) || bValue < 0 || bValue > 999 || bValue != Math.floor(bValue)) {
            addValidationError($el, 'Балл должен быть целым числом от 1 до 999', true);
            setTimeout(function() { $el.focus() }, 0);
            return;
        }

        var rq = { ApplicationID: ApplicationId, EntranceTestItemID: etiID, ResultValueString: bValue.toFixed(4) };
        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocValueSave(null)) %>", JSON.stringify(rq), function(data) {

            if (data.IsError) {
                infoDialog(data.Message.replace(/\n/g, '<br/>'));
                return;
            }
            var htm = RenderSourceControl(data.Data); // Перотображаем 
            $el.parents('tr:first').find('td.source').html(htm);
            if (ManualInputRun) {
                ManualInputRun = false;
                if (clickButton) {
                    btnGetResultEge();
                    clickButton = false;
                }
            }
        });
        return true;
    }

    function doSaveEgeValueForBenefit(el, id, EDID, sID) {
        var $el = $(el);

        var bValue = new Number($el.val());
        if (isNaN(bValue) || bValue < 1 || bValue > 100 || bValue != Math.floor(bValue)) {
            addValidationError($el, 'Балл должен быть целым числом от 1 до 100', true);
            setTimeout(function() { $el.focus() }, 0);
            return;
        }
        var rq = {};
        rq.ID = id;
        rq.ResultValueString = bValue.toFixed(4);
        rq.ApplicationID = ApplicationId;
        rq.EntrantDocumentID = EDID;
        rq.SubjectID = sID;

        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocEgeValueSave(null)) %>", JSON.stringify(rq), function(data) { 
            ManualInputRun = false;
            if (data.IsError) {
                infoDialog(data.Message.replace(/\n/g, '<br/>'));
                return;
            }
            $el.removeClass('input-validation-error');
            $el.parents('tr:first').find("input[data-field=ResultValue]").removeClass('input-validation-error');
            $el.parents('tr:first').find("td.source").find('span.field-validation-error').remove();
            $el.parents('tr:first').find("td.source").find('br').remove();
            if (data.Data[0] == 1) {
                //addValidationError( $el, 'Для использования льготы минимальный балл по предмету должен быть не менее X баллов', true);
                $el.parents('tr:first').find("input[data-field=ResultValue]").addClass('input-validation-error');
                $el.addClass('input-validation-error');
                $el.parents('tr:first').find("td.source").append('<br/><span class="field-validation-error">' + 'Для использования льготы минимальный балл по предмету должен быть не менее ' + data.Data[1] + ' баллов' + '</span>');
                return;
            } else {
                $el.removeClass('input-validation-error');
                $el.parents('tr:first').find("input[data-field=ResultValue]").removeClass('input-validation-error');
                $el.parents('tr:first').find("td.source").find('span.field-validation-error').remove();
            }
        });
    }

    function detachETDocument(el, id) {
        var $el = $(el);
        if (id == undefined) {
            id = $el.parents('tr:first').find("td.source").find('a.btnDelete').attr('docdelid');
        }
        // Сохранить Value
        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocDel(null)) %>", JSON.stringify({ aetdID: id }),
            function(data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                ETIRowUpd(data.Data);
            });
    }

    function detachETCGDocument(el, id) {
        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocCommonDel(null)) %>", JSON.stringify({ aetdID: id }),
            function(data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                var $el = $(el).parents('p[doc-id]:first').remove();
            });
    }

    function addEGEETValue(el) {
        DialogWz3();
        var $el = $(el);
        // Открыть документ и получить новый документ
        // Или создается документ или выбирается, получаем ID и уже далее используем.
        var EntranceTestItemID = $el.parents('tr:first').attr('etiID');
        var GroupID = $el.parents('table[GroupID]:first').attr('GroupID');
        var SourceID = 1;
        SelectDocsList(EntranceTestItemID, SourceID, GroupID, function(id) {});
    }

    function addGIAETValue(el) {
        var $el = $(el);
        DialogWz3();
        // Открыть документ и получить новый документ
        // Или создается документ или выбирается, получаем ID и уже далее используем.
        var EntranceTestItemID = $el.parents('tr:first').attr('etiID');
        var GroupID = $el.parents('table[GroupID]:first').attr('GroupID');
        var SourceID = 4;
        SelectDocsList(EntranceTestItemID, SourceID, GroupID, function(id) {});
    }

    function addDiplomaETValue(el) {
        DialogWz3();
        var $el = $(el);
        // Открыть документ и получить новый документ
        // Или создается документ или выбирается, получаем ID и уже далее используем.
        var EntranceTestItemID = $el.parents('tr:first').attr('etiID');
        var GroupID = $el.parents('table[GroupID]:first').attr('GroupID');
        var SourceID = 3;
        SelectDocsList(EntranceTestItemID, SourceID, GroupID, function(id) {});
    }

    function addManualETValue(el) {
        var course = <%= Model.Course %>;
        var etID = jQuery(el).parents('tr:first').attr('etiid');
        var IsFromKrym = <%= Model.IsFromKrym %>;
        var GetChekcEGE = <%=Model.GetChekcEGE %>;
        if ((IsFromKrym == 1)||(GetChekcEGE == 0)) {
            btnEgeCheck(el, etID);
        } else {
            confirmDialog('Перед вводом результатов вступительного испытания ОО необходимо проверить наличие свидетельств ЕГЭ у абитуриента. Проверить сейчас?',
                function() {
                    btnGetResultEge(true, false, true, el, etID);
                },
                function() {
                    btnEgeCheck(el, etID);
                }
            );
        }
    }

    function btnEgeCheck(el, etID) {
        addManualETValueDialogInternal(el);
        return;
    }

    ////КНОПКА === Получить/проверить результаты ЕГЭ
    function btnGetResultEge(success, error, res, el, etID) {        
        if (ManualInputRun === true) {
            clickButton = true;
            return;
        }
        eModel = {
            method: "ByIdentityDocument",
            ApplicationID: ApplicationId,
            doc: null,
            regNum: null,
            refr: 1,
            currentYear: 0,
            res: res,
            EtiId: etID
        }
        eModel.ApplicationID = ApplicationId;
        blockUI();
        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.CheckEGEResults(null)) %>", JSON.stringify({ model: eModel }), function(data) {
            unblockUI();
            
            if (res) {
                
                res = false;
                if (data.IsError) {
                    btnEgeCheck(el, etID);
                    infoDialog(data.Message);
                    return;
                }
                if (data.Data.violationId == 0) {
                    if (data.Extra == 1) {
                        btnEgeCheck(el, etID);
                    } else {
                        if (window.ShowWz != undefined) {
                            window.ShowWz(3);

                        }
                        if (window.loadPaView != undefined) {
                            window.loadPaView(2);
                        }
                    }
                } else {
                    btnEgeCheck(el, etID);
                    infoDialog(data.Data.violationMessage);
                }
                return;
            }

            if (data.IsError) {
                infoDialog(data.Message);
                if (error) {
                    error();
                }
                if (window.ShowWz != undefined) {
                    window.ShowWz(3);
                }
                if (window.loadPaView != undefined) {
                    window.loadPaView(2);
                }
                return;
            }
            if (data.Data.violationId == 0) {
                if (window.ShowWz != undefined) {
                    window.ShowWz(3);
                }
                if (window.loadPaView != undefined) {
                    window.loadPaView(2);
                }
            } else {
                infoDialog(data.Data.violationMessage,
                    function() {
                        if (window.ShowWz != undefined) {
                            window.ShowWz(3);
                        }
                        if (window.loadPaView != undefined) {
                            window.loadPaView(2);
                        }
                    }
                );

            }
        });
    }

    ////КОНЕЦ === КНОПКА === Получить/проверить результаты ЕГЭ

    function getEgeDocument(REModel, success, error) {
        btnCheckEge(REModel, success, error);
    }

    function btnCheckEge(REModel, success, error) {
        if (REModel == undefined) {
            REModel = {
                Step: WizStep,
                method: "ByCertificate",
                ApplicationID: ApplicationId,
                doc: null,
                regNum: null,
                refr: 1,
                currentYear: 0,
                Typ: 1,
                DocTypeID: 0,
                DocId: null,
                OlympicID: null
            }
        }
        REModel.ApplicationID = ApplicationId;
        //	Признак типа олимпиады (далее – @Typ, 0 – Всероссийская олимпиада школьников / 1 – олимпиада школьников)    
        REModel.Typ = 1;
        //	Номер олимпиады в перечне (OlympicType.OlympicNumber) (далее - @OlympNumber, может быть NULL, если @Typ = 0)
        // REModel.OlympNumber=1;
        var type = parseInt(REModel.DocTypeID);
        var apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckEGEResults(null)) %>";
        switch (type) {
            case 2: // 2	Свидетельство о результатах ЕГЭ
                apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckEGEResults(null)) %>";
                break;
            case 9: // 9	Диплом победителя/призера олимпиады школьников
                apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckOlympicResults(null)) %>";
                REModel.Typ = 1;
                break;
            case 10: // 10	Диплом победителя/призера всероссийской олимпиады школьников
                apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckOlympicResults(null)) %>";
                REModel.Typ = 0;
                break;
            default:
                REModel.Typ = 0;
                break;
        }
        blockUI();
        doPostAjax(apiurl, JSON.stringify({ model: REModel }), function(data) {
            unblockUI();
            if (data.IsError) {
                infoDialog(data.Message);
                if (error) {
                    error();
                }
                if (window.ShowWz != undefined) {
                    window.ShowWz(3);
                }
                if (window.loadPaView != undefined) {
                    window.loadPaView(2);
                }
                return;
            }

            if (data.Data) {
                if (data.Data.violationId == 0) { //ПЕРЕЗАГРУЗКА СТРАНИЦЫ -- СООБЩЕНИЙ НЕ НАДО!
                    closeDialog($('#divAddETDocumentWz3'));
                    if (success) {
                        success(data.Data);
                    }

                    if (window.ShowWz != undefined) {
                        window.ShowWz(3);
                    }
                    if (window.loadPaView != undefined) {
                        window.loadPaView(2);
                    }
                } else {
                    if (success) {
                        success(data.Data);
                    }
                    infoDialog(data.Data.violationMessage);
                    //closeDialog($('#divAddETDocument'));
                    if (window.ShowWz != undefined) {
                        window.ShowWz(3);
                    }
                    if (window.loadPaView != undefined) {
                        window.loadPaView(2);
                    }
                }
            }
        });
    }

    function addManualETValueDialogInternal(el) {

        var $dManualETValue = dManualETValue();
        $dManualETValue.find('#InsDocDescr_DocumentNumber').val('');
        $dManualETValue.find('#InsDocDescr_DocumentDate').val('');
        $dManualETValue.find('#InsDocDescr_ResultValue').val('');

        $dManualETValue.find("#InsDocDescr_DocumentTypeID").change(function() {
            jQuery('#InsDocDescr_DocumentNumber').val('');
            jQuery('#InsDocDescr_DocumentDate').val('');
            jQuery('#InsDocDescr_ResultValue').val('');
        });

        var $el = $(el);
        var EntranceTestItemID = $el.parents('tr:first').attr('etiID');
        var GroupID = $el.parents('table[GroupID]:first').attr('GroupID');
        var SourceID = 2;
        var EntrantDocumentID = null;
        var currentValue = $el.parents('tr:first').find('input[type="text"]').val();

        $dManualETValue.find('#InsDocDescr_ResultValue').val(currentValue);
        $dManualETValue.find('#InsDocDescr_ResultValue, #InsDocDescr_DocumentDate, #InsDocDescr_DocumentNumber').removeClass('input-validation-error').removeClass('input-validation-error-fixed');
        $dManualETValue.dialog({
            modal: true,
            width: 600,
            title: 'Ввод результатов вступительных испытаний ОО',
            buttons: {
                "Сохранить": function() {
                    clearValidationErrors($dManualETValue.find('#InsDocDescr_ResultValue').parents('table:first'));
                    var unparsedValue = $dManualETValue.find('#InsDocDescr_ResultValue').val().replace(',', '.');
                    var bValue = parseFloat(unparsedValue);
                    var isError = false;
                    if (isNaN(bValue) || bValue < 0 || bValue > 1000) {
                        addValidationError($dManualETValue.find('#InsDocDescr_ResultValue'), 'Балл должен быть числом от 0 до 999', true);
                        isError = true;
                    }
                    if (!$dManualETValue.find('#InsDocDescr_DocumentDate').val()) {
                        addValidationError($dManualETValue.find('#InsDocDescr_DocumentDate'), 'Дата документа обязательна', true);
                        isError = true;
                    }
                    if (!$dManualETValue.find('#InsDocDescr_DocumentNumber').val()) {
                        addValidationError($dManualETValue.find('#InsDocDescr_DocumentNumber'), 'Номер документа обязателен', true);
                        isError = true;
                    }
                    if (isError) {
                        return;
                    }
                    var d = {};
                    d.InstitutionDocumentTypeID = $dManualETValue.find('#InsDocDescr_DocumentTypeID').val();
                    d.InstitutionDocumentNumber = $dManualETValue.find('#InsDocDescr_DocumentNumber').val();
                    d.InstitutionDocumentDate = $dManualETValue.find('#InsDocDescr_DocumentDate').val();
                    d.ResultValueString = bValue.toFixed(4);

                    doETDocumentSelect(EntrantDocumentID, 0, EntranceTestItemID, GroupID, SourceID, d, function() {
                        closeDialog($dManualETValue);
                    });
                    // $dManualETValue.remove();
                },
                "Отмена": function() {
                    //$dManualETValue.dialog('close');
                    //$dManualETValue.remove();
                    closeDialog($dManualETValue);
                }
            },
            close: function() {
                $dManualETValue.remove();
            }
        }).dialog('open');
    }

    function appEntTestDocGet(EntTestItemId, success) {
        var d = { EntTestItemId: EntTestItemId, ApplicationId: ApplicationId };

        doPostAjax("<%= Url.Generate<ApplicationController>(x => x.AppEntTestDocGet(null, null)) %>", JSON.stringify(d),
            function(data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                if (success) {
                    success(data.Data);
                }
            });
}

function ETCGUpd(I) { // Добавление документа в общие документы группы
    if (I.Doc == null) {
        return;
    }
    var htm = '<p benefit="' + I.Doc.BenefitID + '" doc-id="' + I.Doc.ID + '">';
    htm += '<span><span style="font-size: 12pt">' + I.Doc.BenefitName + '</span><span class="statementsubtitle"> (' + I.Doc.Description + ')</span> </span>';
    htm += '<a href="#" class="btnDelete action-edit" onclick="detachETCGDocument(this, ' + I.Doc.ID + '); return false;" title="Удалить результаты">&nbsp;</a>';
    htm += '</p>';

    var $cg = $('div.divGeneralBenefits[GroupID=' + I.Doc.CompetitiveGroupID + ']');
    var $el = $cg.find('p[benefit]:last');
    if ($el.length == 0) {
        $el = $cg.find('b.commonBenefitsB');
    }
    $el.after(htm);
}

function ETIRowUpd(I) { // Обновление строки по предмету 
    var $el = $('#eti' + I.EntranceTestItemID);
    var $v = $el.find("input[data-field=ResultValue]");
    var $egetd = $el.find("td[data-field=EgeResultValue]");
    $v.removeClass('input-validation-error').removeClass('input-validation-error-fixed');
    $egetd.removeClass('input-validation-error').removeClass('input-validation-error-fixed');
    if ((I.SubjectID != null && I.SubjectName == null) && (I.Doc == null || (I.Doc.SourceID == 1 && I.Doc.EntrantDocumentID == null))) {
        $v.removeClass('view');
        $v.attr('readonly', '');
    } else {
        $v.addClass('view');
        $v.attr('readonly', 'readonly');
    }
    $egetd.empty();
    if (I.Doc == null) {
        $v.val('');
    } else {
        $v.val(I.Doc.ResultValue);
        //var $egev =$el.find("input[data-field=EgeResultValue]");
        if (I.Doc.BenefitID == 3 && I.EntranceTestItemID != null) {
            // Рендерим окно ввода для ЕГЕ
            var htm = '';
            htm += '<input type="text" data-field="EgeResultValue" class="numeric" style="width: 70px" value="' + (I.Doc == null ? '' : (I.Doc.EgeResultValue == null ? '' : I.Doc.EgeResultValue)) + '"';
            htm += ' maxlength="3" isege="' + I.EntranceTestTypeID == 1 ? '1' : '0' + '"';
            htm += ' onfocus="doManualInputStart(this);" onblur="doManualInputEnd(this);" onchange="doSaveEgeValueForBenefit(this, ' + I.Doc.ID + ',' + I.Doc.EntrantDocumentID + ',' + I.Doc.SubjectID + ');"  />';
            $egetd.html(htm);
        }
    }
    $el.find('td.source').html(RenderSourceControl(I));
}

function RenderSourceControl(I) {
    var htm = '';
    var HasBenefits;

    HasBenefits = $('div[GroupID=' + I.GroupID + ']').attr('HasBenefits');
    if (I.Doc != undefined) {
        htm += ' <span docdescr="1">' + I.Doc.Description + '</span> <a href="#" class="btnDelete action-edit" onclick="detachETDocument(this, ' + I.Doc.ID + '); return false;" title="Удалить результаты" docdelid="' + I.Doc.ID + '">&nbsp;</a>';
    } else {
        if (I.EntranceTestTypeID == 1 && I.SubjectIsEge == true) {
            htm += ' <a href="#" style="padding-right: 15px;" onclick="addEGEETValue(this); return false;">Свидетельство ЕГЭ</a>';
        } else {
            htm += ' <span style="color: #606060; display: none;">Свидетельство ЕГЭ</span>';
        }
        if (I.EntranceTestTypeID == 1 && I.SubjectID > 0) {
            htm += ' <a href="#" style="padding-right: 15px;" onclick="addGIAETValue(this); return false;">Справка ГИА</a>';
        } else {
            htm += ' <span style="color: #606060; display: none;">Справка ГИА</span>';
        }
        if (I.EntranceTestTypeID != 1 || true) {
            htm += ' <a href="#" id="ET_Ref" style="padding-right: 15px;" onclick="addManualETValue(this);return false;">Вступительное испытание ОО</a>';
        } else {
            htm += ' <span style="color: #606060; display: none;">Вступительное испытание ОО</span>';
        }
        if ((HasBenefits || I.EntranceTestTypeID != 2 || I.SubjectID > 0)) {
            htm += ' <a href="#" style="padding-right: 15px;" onclick="addDiplomaETValue(this); return false;">Право на 100 баллов</a>'
        } else {
            htm += ' <span style="color: #606060; display: none;" title="Для данной дисцпилины отсутствуют льготы">Право на 100 баллов</span>';
        }
    }
    return htm;
}
</script>

<script type="text/javascript">
    
    var specialDoc = false;

    function GetDocInfo(docId) {
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetDocInfo(null)) %>',
            JSON.stringify({ docId: docId }),
            function (data) {
                if (data.IsError) {
                    return;
                }
                $('#krymdocnumber').html(data.Data);
            }
        );
    }


    function Wz3Init() {

        $("#InsDocDescr_DocumentDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date() });

        <% if (Model.CompositionResult.Count == 0)
    { %>
        $('#StatementNoFound').show();
        $('#CompositionTable').hide();
        <% }
    else
    { %>
        $('#StatementNoFound').hide();
        $('#CompositionTable').show();
        <% } %>


        var StatusId=<%=Model.StatusID %>;
        if (StatusId == 4 || StatusId == 6) {
            $("#actual").find("a").removeAttr("href").attr("disabled", "disabled").live("click", function() {
                return false; 
            });
            $("#actual").find(".action-edit").hide();

            $("#actual").find("input, button").not("#btnGetResultEge").not("#viewComposition").not("#getComposition").attr("disabled", "disabled");
        }

        ChektIsDisabled();
        ChektIsDistant();

        GetDocInfo($('#IsDisabledDocumentID').val());
    }

    

    function Wz3Save(success, error, step) {

        if (step == undefined) {
            step = 2;
        }
        
        var error = false;
        error = $('input[data-field=ResultValue]').hasClass('input-validation-error');
        if (error) {
            return false;
        }

        if($('#IsDisabled').is(":checked") && !$('#IsDisabledDocumentID').val()){
            infoDialog("Необходимо выбрать подтверждающий документ для ВИ с созданием специальных условий");
            return;
        }

        if($('#IsDistant').is(":checked") && !$('#DistantPlace').val()){
            infoDialog("Место сдачи ВИ является обязательным в случае выбора чекбокса ВИ с использованием дистанционных технологий");
            return;
        }


        var model = {
            Step: step,
            ApplicationID: ApplicationId,
            IsDisabled: $('#IsDisabled').is(":checked"),
            IsDistant: $('#IsDistant').is(":checked"),
            IsDisabledDocumentID: $('#IsDisabledDocumentID').val(),
            DistantPlace: $('#DistantPlace').val(),
        }



        if (!ManualInputRun) {
            doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.Wz3Save(null)) %>', 
            JSON.stringify(model),
            function(data) {
                if (!data.IsError) { if (success) { success(model); }} 
                else { if (error) { error("При сохранении произошла ошибка."); }}
            });
        }
    }

    function Wz3Cancel() {
        var model = WzObjs[2];
        if (model == undefined)
            return;
    }

    function ChektIsDisabled() {
        var checked = $('#IsDisabled').is(":checked");
        if(checked)
            $('#linkAddSpecialDoc').show();
        else
            $('#linkAddSpecialDoc').hide();
    }

    function ChektIsDistant() {
        var checked = $('#IsDistant').is(":checked");
        if(checked)
            $('#divDistantPlace').show();
        else
            $('#divDistantPlace').hide();
    }
    

</script>



