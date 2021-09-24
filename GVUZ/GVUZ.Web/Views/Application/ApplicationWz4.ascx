<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz4ViewModel>" %>
<%@ Register TagName="EntrantDocumentAddPart" TagPrefix="gv" Src="~/Views/Shared/Controls/EntrantDocumentAddPart.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>

<div id="actual">

<div class="subHeader">Сведения об индивидуальных достижениях поступающего</div>
<p style="margin: 10px;">
    Внимание! Поле "Дополнительный балл" необходимо заполнять только в том случае, если
    правилами приема образовательной организации предусмотрено начисление баллов за
    индивидуальные достижения.</p>
<div id="txtUidError"></div>
<table class="gvuzDataGrid" cellpadding="3" id="indAchGrid">
    <thead>
        <tr><th><%= Html.TableLabelFor(x => x.FakedAchivement.UID, required: false)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IAName, required: true)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IAMark, required: false)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IADocumentDisplay, required: false)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.isAdvantageRight, required: false)%></th>
            <th>Действия</th>
        </tr>
    </thead>
    <tbody>
        <tr id="trAddNew1" style="display: none;"> </tr>
        <tr id="addNewRow">
            <td colspan="5">
                <a href="#" class="add" id="btAddAchievement" onclick="addAchievement(this);return false;">Добавить</a>
            </td>
        </tr>
    </tbody>
</table>
    <table class="tableApp2">
        <tbody>
            <tr><td class="caption" style="width:200px"><label>Суммарное количество баллов:</label></td><td><input type="text" style="width:60px" disabled="disabled" id="iachSum" value="<%=Model.IndividualAchivementsMarkStr%>" /></td></tr>
        </tbody>
    </table>
<div id="documentAddDialog">    
    <div id="divAddETDocument">
        <div id="divCurrentDocumentPart" style="display: none">
            <b>Выбранный документ:</b>
            <div id="divCurrentDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
            </div>
        </div>
        <div id="divAddDocumentPart" style="display: none">
            <b>Добавить документ:</b>
            <div id="divAddDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
            </div>
        </div>
        <div id="divExistingDocumentPart" style="display: none">
            <b>Выбрать существующий документ:</b>
            <div id="divExistingDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
            </div>
        </div>
    </div>
</div>

</div>

<script language="javascript" type="text/javascript">
    var WZ4={};
    WZ4.achivements = JSON.parse('<%= Html.Serialize(Model.Items) %>');
    WZ4.applicationID = <%= Model.ApplicationID %>;
    WZ4.institutionID = <%= Model.InstitutionID %>;
    WZ4.entrantID = <%= Model.EntrantID %>;
    WZ4.SubjectList = JSON.parse('<%= Html.Serialize(Model.IAchievements) %>');
    WZ4.DocsList = {};
    WZ4.statSave = false;
    WZ4.maxIASum = <%= (Model.MaxIAValues) %>;
    WZ4.CheckWorks = <%= Convert.ToInt32(Model.CheckWorks as int?) %>;
    WZ4.EducationLevelID = <%= Model.EducationLevelID %>;

    function proverka(input) { 
        //var kvk = WZ4.EducationLevelID == <%= GVUZ.Model.Institutions.EDLevelConst.HighQualification %>;

        //var mark = 0;
        //if (kvk)
        //    mark = parseFloat($(input).val()).toFixed(2);
        //else
        //    var mark = parseInt($(input).val().replace(/\D/g, ''), 10);

        var mark = parseFloat($(input).val().replace(',','.')).toFixed(2);

        $(input).val(mark.toLocaleString());
        if (mark >= 0) {
            $('#markInput').removeClass('input-validation-error');
            $('#divErrorBlockOl').find('span.field-validation-error').remove();
            if (mark > 999) {
                $('#markInput').addClass('input-validation-error');
                $('#divErrorBlockOl').html('<span class="field-validation-error">Превышен допустимый максимальный балл за индивидуальное достижение </span>');
            }
        } else {
            $(input).val("");
        }
    }

    function addAchievement(el) {
        
        $('#nameInput').removeClass('input-validation-error-fixed');
        WZ4.statSave = true;
        $("#addNewRow").hide();
        var newRowHtml = '<tr id="rowAdded"  data-id="0"><td><input type="text" id="uidInput" /></td>';        
        newRowHtml += '<td><select class="subjectNames" id="nameInput" /></td>';
        newRowHtml += '<td><input type="text" class="numeric" id="markInput" onblur="return proverka(this);" /><div id="divErrorBlockOl"></div></td>';  
        //onkeyup="return proverka(this);" 
        newRowHtml += '<td><a class="AchievementDocument" href="#" id="addDocumentLink" onclick="addAchievementDocument()">Добавить подтверждающий документ</a></td>';
        newRowHtml +='<td class="nameCell"><input type="checkbox" id="isAdvantageRight"/></td>';
        newRowHtml += '<td><a href="#" class="btnSave bthSaveAchievement" onclick="saveAchievement(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="cancelAddAchievement()">&nbsp;</a></td></tr>';
        $("#addNewRow").before(newRowHtml);

        //debugger;
        for (var i = 0; i < WZ4.SubjectList.length; i++){
            var toAdd = true;
            //for (var j=0; j < WZ4.achivements.length; j++){
            //    if (WZ4.achivements[j].IAName == WZ4.SubjectList[i].Name){
            //        toAdd = false;
            //        break;
            //    }
            //}

            if (toAdd)
                $('.subjectNames').append("<option value='"+WZ4.SubjectList[i].IdAchievement +"'>" + escapeHtml(WZ4.SubjectList[i].Name) + "</option>");
        }
        if (WZ4.SubjectList.length == 0) {
            $('#nameInput').addClass('input-validation-error');
            return;
        }
    }

    function doAddRow($rowBefore, item){
        //debugger;
      var className = $rowBefore.prev().attr('class');
      if(className == 'trline1'){ className = 'trline2'; }else{ className = 'trline1';}

      var htmlRow = '<tr data-id="'+item.IAID+'">';
      htmlRow+='<td class="tdUID">' + (item.UID == null ? '' : item.UID) + '</td>';
      htmlRow+='<td class="nameCell">' + item.IAName + '</td>';
      htmlRow+='<td>' + (item.IAMark == null ? '' : item.IAMark.toString()) + '</td>';
      htmlRow+='<td><a class="AchievementDocument" href="#" documentID="' + item.IADocument.EntrantDocumentID + '" onclick="showDocument(this);return false">' + item.IADocument.Description + '</a></td>';
      htmlRow+='<td class="nameCell">' + (item.isAdvantageRight ? 'Да' : '') + '</td>';
      htmlRow+='<td><a href="#" class="btnDelete Achievement" onclick="deleteAchievement(this)" achievementID="' + item.IAID + '" documentID="' + item.IADocument.EntrantDocumentID + '">&nbsp;</a></td>';
      $rowBefore.before(htmlRow);
    }
    function cancelAddAchievement() {
        WZ4.statSave = false;
        jQuery('#rowAdded').remove().detach();
        jQuery("#addNewRow").show();
        // Документ пока не удаляем. Понадобится - вставлять код сюда!
    }

    function addAchievementDocument() {
        var url = '<%= Url.Generate<ApplicationController>(x => x.SelectEntDocsList(null,null,null,null)) %>';
        doPostAjax(url, JSON.stringify({ ApplicationID: WZ4.applicationID, EntranceTestItemID: 0, SourceID: 15, GroupID: 0 }),
            function(data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                var ds = data.Data;
                if (ds.EntrantDocuments.length == 0 && ds.DocumentTypes.length == 1) { // Если существующих подходящих документов нет, то 
                    // Если тип только один, то сразу предлагаем новый документ
                    doAddDocument(15, null, null);
                } else {
                    // Составить список документов
                    var $dts;
                    var htm = '';
                    $dts = $('#divAddETDocument').find('#divAddDocumentTypes');
                    $dts.empty();
                    for (var i = 0; i < ds.DocumentTypes.length; i++) {
                        htm += '<span class="linkSumulator" doctypeid="' + ds.DocumentTypes[i].ID + '" onclick="doAddDocument(' + ds.DocumentTypes[i].ID + ', ' + null + ',' + null + ')" >' + ds.DocumentTypes[i].Name + '</span><br/>';
                    }
                    $dts.html(htm);
                    $('#divAddDocumentPart').show();

                    $dts = $('#divAddETDocument').find('#divExistingDocumentTypes');
                    $dts.empty();
                    htm = '';
                    WZ4.DocsList = ds.EntrantDocuments;
                    for (var i = 0; i < ds.EntrantDocuments.length; i++) {
                        htm += '<span class="linkSumulator" docid="' + ds.EntrantDocuments[i].EntrantDocumentID + '" onclick="addDoc(' + ds.EntrantDocuments[i].EntrantDocumentID + ')" >' + ds.EntrantDocuments[i].Description + '</span><br/>';
                    }
                    $dts.html(htm);
                    if (ds.EntrantDocuments.length > 0) {
                        $('#divExistingDocumentPart').show();
                    } else {
                        $('#divExistingDocumentPart').hide();
                    }
                    $('#documentAddDialog').dialog({ modal: true, width: 600, title: 'Выбор документа' }).dialog('open');
                }
            }
        );
    }

    function addDoc(EntrantDocumentID) {
        closeDialog($('#documentAddDialog'));
        var Description = "";
        for (var i = 0; i < WZ4.DocsList.length; i++) {
            if (WZ4.DocsList[i].EntrantDocumentID == EntrantDocumentID) {
                if (WZ4.DocsList[i].DocumentSeries == null) { WZ4.DocsList[i].DocumentSeries = ""}
                if (WZ4.DocsList[i].DocumentNumber == null) { WZ4.DocsList[i].DocumentNumber = ""}

                Description = WZ4.DocsList[i].DocumentTypeName + ' №' + WZ4.DocsList[i].DocumentSeries + ' ' + WZ4.DocsList[i].DocumentNumber + ' от ' + WZ4.DocsList[0].DocumentDateString;
            }
        }
        var newLinkHtml = '<a class="AchievementDocument" href="#" id="addDocumentLink" onclick="showDocument(this);return false">' + Description + '</a>';
        $("#addDocumentLink").after(newLinkHtml).remove();
        $("#addDocumentLink").attr('documentID', EntrantDocumentID);
        closeDialog($('#documentAddDialog'));
    }

    function addTRDocument(doc){
      //if(doc.EntDocCustom!=null && doc.EntDocCustom!=undefined){
      //  doc.Description=doc.EntDocCustom.DocumentTypeNameText + ' №' + doc.DocumentSeries +' '+ doc.DocumentNumber + ' от ' + doc.DocumentDate;
      //}else{
        doc.Description=doc.DocumentName + ' №' + doc.DocumentSeries +' '+ doc.DocumentNumber + ' от ' + doc.DocumentDate;
      //}
      var newLinkHtml = '<a class="AchievementDocument" href="#" id="addDocumentLink" onclick="showDocument(this);return false">' + doc.Description + '</a>';
      $("#addDocumentLink").after(newLinkHtml).remove();
      $("#addDocumentLink").attr('documentID', doc.EntrantDocumentID);
    }
    function updTRDocument(doc, $tr){
      //if(doc.EntDocCustom!=null && doc.EntDocCustom!=undefined){
      //  doc.Description=doc.EntDocCustom.DocumentTypeNameText + ' №' + doc.DocumentSeries +' '+ doc.DocumentNumber + ' от ' + doc.DocumentDate;
      //}else{
        doc.Description=doc.DocumentName + ' №' + doc.DocumentSeries +' '+ doc.DocumentNumber + ' от ' + doc.DocumentDate;
      //}      
      $tr.find('a.AchievementDocument').text(doc.Description);
    }
    function showDocument(el){
      var documentId = $(el).attr('documentID');
      var $tr = $(el).parents('tr:first');

        EditUniDDocument(documentId, 0, function(doc) {
            updTRDocument(doc, $tr);
        });
    }

    function deleteAchievement(el) {
        var achievementID = $(el).attr('achievementID');
        var entrantDocumentID = $(el).attr('documentID');
        confirmDialog('Удалить индивидуальное достижение?', function() {
            doPostAjax('<%= Url.Generate<ApplicationController>(x => x.DeleteIndividualAchievement(null, null)) %>',
                JSON.stringify({ achievementID: achievementID, entrantDocumentID: entrantDocumentID }),
                function(data) {
                    if (data.Message != null) {
                        alert(data.Message);
                        return false;
                    }
                    $(el).parents('tr[data-id=' + achievementID + ']:first').remove();
                    
                    //var $tr = jQuery(el).parents('tr');
                    //var row = $tr[0].rowIndex;
                    //debugger;
                    var row = 0;
                    for(var k = 0; k<WZ4.achivements.length; k++ ){
                        if (WZ4.achivements[k].IAID.toString() == achievementID.toString() ){
                            row = k;
                            break;
                        }
                    }

                    WZ4.achivements.splice(row, 1);
                    RefreshMarksSum();
                });
        });
    }

    function saveAchievement(el, success) {
        if ($('#markInput').hasClass('input-validation-error')) {
            return;
        }

        var $tr = $(el).parents('tr:first');
        clearValidationErrors($tr);
        var flagError = false;
        $('#uidInput').removeClass('input-validation-error');
        $('#txtUidError span').text("");

        $('#uidInput').removeClass('input-validation-error-fixed');
        if ($('#nameInput').val() == '' || $('#nameInput').val() == null) {
            $('#nameInput').addClass('input-validation-error');
            flagError = true;
        } else {
            $('#nameInput').removeClass('input-validation-error');
            flagError = false;
        }

        //proverka($tr);
        var markVal = $('#markInput').val().replace(',','.');
        //if (markVal.split('.').length == 1) {
        //    markVal += '.0';
        //}

        if (Number(markVal).toString() == "NaN") {
            $('#markInput').addClass('input-validation-error');
            flagError = true;
        } else {
            $('#markInput').removeClass('input-validation-error');
            flagError = flagError ? flagError : false;
        }

        var sAch=null;
        var sNames = $('.subjectNames').val();
        for (var i = 0; i < WZ4.SubjectList.length; i++) {
            if (WZ4.SubjectList[i].IdAchievement == sNames) {
                sAch=WZ4.SubjectList[i];
                if (WZ4.SubjectList[i].MaxValue < markVal) {
                    $('#markInput').addClass('input-validation-error');
                    $('#divErrorBlockOl').html('<span class="field-validation-error">Превышен допустимый максимальный балл за индивидуальное достижение </span>');
                    flagError = true;
                } else {
                    $('#markInput').removeClass('input-validation-error');
                    flagError = flagError ? flagError : false;
                }
            }
        }
        var documentID = $("#addDocumentLink").attr('documentID');

        if(sAch!=null)
        if (sAch.IdCategory!= 12) {
            if (documentID == '' || documentID == undefined) {
                $("td>a#addDocumentLink").addClass('input-validation-error');
                flagError = true;
            } else {
                $("td>a#addDocumentLink").removeClass('input-validation-error');
                flagError = flagError ? flagError : false;
            }
        } else {
            $("td>a#addDocumentLink").removeClass('input-validation-error');
            flagError = flagError ? flagError : false;
        } 
        
        if (flagError) return false;

        var model = {
            UID: $('#uidInput').val(),
            IAName: $("#nameInput option:selected").text(),
            IAMarkString: markVal,
            IADocumentID: documentID,
            isAdvantageRight: $('#isAdvantageRight').is(':checked'),
            ApplicationID: WZ4.applicationID,
            InstitutionID: WZ4.institutionID,
            IdAchievement: sAch.IdAchievement,
            IAID: sAch.IdAchievement
        };

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SaveIndividualAchievement(null))%>',
            JSON.stringify(model, WZ4.applicationID),
            function(data) {
                if (data.Message != null) {
                    alert(data.Message);
                    return false;
                }

                if (data.Data != null) {
                    doAddRow($('#trAddNew1'), data.Data);
                    WZ4.achivements.push(data.Data);
                    cancelAddAchievement();
                    RefreshMarksSum();
                } else {
                    if (data.Extra != null) {
                        $('#uidInput').addClass('input-validation-error');
                        $('#txtUidError').html('<span class="field-validation-error">'+data.Extra+'</span>');
                        return;
                    }
                }

                if (WZ4.statSave) {
                    WZ4.statSave = false;
                    if (success) {
                        success();
                    }
                }
            });
    }

    function RefreshMarksSum(){ 
        var achSum = 0;
        var maxIASum = WZ4.maxIASum;
        for (var i = 0; i < WZ4.achivements.length; i++)
        {
            achSum += WZ4.achivements[i].IAMark
        }

        <% if(Model.CheckAchievementsSum) { %>
        if(achSum > maxIASum)
        {
            achSum = maxIASum;
        }
        <% } %>
       
        $("#iachSum").val(achSum);
    }
</script>
<script type="text/javascript">
  //<%-- вызываем диалог редактирования (находим строчку с документом если есть и открываем диалог)  --%>
    function callEditDocument(docID) {
        var $el = $('.gvuzDataGrid tr[itemID="' + docID + '"] a').length;
        if ($el.length > 0) {
            doEditDocument($el[0]);
        }
    }

//<%--  редактирование документа. Данные из строки с ним --%>
  function doEditDocument(el) {
      var $tr = jQuery(el).parents('tr:first');
      var edID = $tr.attr('itemID');
      edID = parseInt(edID);
      EditUniDDocument(edID, typeID, function(model) {
          updTRDocument(model, $tr);
      });
      return false;
  }

//<%-- добавление документа  --%>
  function doAddDocument(typeID, cgID, subjectId) {
        closeDialog($('#documentAddDialog'));
        EditUniDDocument(0, typeID, function(model) {
            addTRDocument(model);
        });
        return false;
    }
</script>
<script language="javascript" type="text/javascript">
  function EditUniDDocument(EntrantDocumentID, DocumentTypeID, success) {
    var $Dialog = null;
    $Dialog = $('#UniDDialog');
    if ($Dialog.length == 0) {
      $Dialog = $('<div id="UniDDialog" style="display:none, position:fixed"></div>');
      $('body').append($Dialog);
    }
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null,null,null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: DocumentTypeID, EntrantID: WZ4.entrantID }, function (data) {      
      $Dialog.html(data);
      UniDFormInit();
      $Dialog.dialog({
        modal: true,
        width: 800,
        title: "Документ",
        buttons: { 
          "Сохранить": function () { 
            var baseModel={ EntrantID:<%=Model.EntrantID %>};
            var doc=UniDPrepareModel(baseModel);
            if(doc!=null || doc!=undefined){
              // Сохранение, если соханилось нормальн то закрыть и обновить.
              UniDSave(doc
               ,function(model){ 
                if(success){ success(model);}
                closeDialog($Dialog);
              },function(e){
                  infoDialog("Не удалось сохранить документ! "+e);
              });
            }
          },"Закрыть": function () { $(this).dialog('close'); }          
        },close: function () {  $Dialog.remove();  }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");
  }
</script>
<script language="javascript" type="text/javascript">
    function Wz4Init() {
        for (var i = 0; i < WZ4.achivements.length; i++) {
            doAddRow($('#trAddNew1'), WZ4.achivements[i]);
        }

        var StatusId=<%=Model.StatusID %>;
        if (StatusId == 4 || StatusId == 6) {
            $('#btAddAchievement').hide();
            $('#btnAppSaveETop').setDisabled();
            $('#btnAppSaveE').setDisabled();
            $('#btnAppCancelETop').setDisabled();
            $('#btnAppCancelE').setDisabled();
            $("#actual").find("a").removeClass("btnDelete").removeAttr("href").attr("disabled", "disabled").live("click", function() {
                return false; 
            });

        }
    }

    function W4zSave(success, error, step) {
      if (WZ4.statSave) {
          confirmDialog("Есть несохраненные изменения. Желаете сохранить?", function() {
              saveAchievement($("#addNewRow"));
          }, function() {
              if (success) {
                  success();
              }
          });
      } else {
          Wz4Save(success, error, step);
      }
  }

  function Wz4Save(success, error, step) {
      var model = {};
      if (step == undefined) {
          step = 4;
      }
      doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.SetWzStep(null,null)) %>', JSON.stringify({ ApplicationID: ApplicationId, Step: step }),
          function(data) {
              if (!data.IsError) {
                  if (success) {
                      success(model);
                  }
              } else {
                  if (error) {
                      error("При сохранении произошла ошибка.");
                  }
              }
          });
  }

  function Wz4Cancel() {    }
</script>