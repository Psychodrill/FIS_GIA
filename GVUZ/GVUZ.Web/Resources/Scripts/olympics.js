//--------------------------------------------------------------------------------------------------------------------

var isError = false

//--------------------------------------------------------------------------------------------------------------------

var filter = {
    Page: 0, Sort: 0,
    HasValue: false,
    Year: 0, Name: 0, Profile: 0, ResultLevelID: 0, StatusID: 0, EndingDate: 0,
    FirstName: '', LastName: '', MiddleName: '', DocumentSeries: '', DocumentNumber: '', DiplomaSeries: '', DiplomaNumber: '',
};

$("#btnClear").live('click', function () {
    filter.HasValue = true;

    filter.Year = 0;
    filter.Name = 0;
    filter.Profile = 0;
    filter.FirstName = '';
    filter.LastName = '';
    filter.MiddleName = '';
    filter.DocumentSeries = '';
    filter.DocumentNumber = '';
    filter.DiplomaSeries = '';
    filter.DiplomaNumber = '';
    filter.ResultLevelID = 0;
    filter.StatusID = 0;
    filter.EndingDate = 0;

    $("#changeYear").val(0);
    $("#changeName").val(0);
    $("#changeProfile").val(0);
    $("#changeFirstName").val('');
    $("#changeLastName").val('');
    $("#changeMiddleName").val('');
    $("#changeDocumentSeries").val('');
    $("#changeDocumentNumber").val('');
    $("#changeDiplomaSeries").val('');
    $("#changeDiplomaNumber").val('');
    $("#changeResultLevelID").val(0);
    $("#changeStatusID").val(0);
    $("#changeEndingDate").val(0);

    Refresh();
});


$(document).ready(function () {
    $("#changeYear").live('change', function () {
        filter.HasValue = true;
        filter.Year = $("#changeYear").val();
        Refresh();
    });

    $("#changeName").live('change', function () {
        filter.HasValue = true;
        filter.Name = $("#changeName").val();
        Refresh();
    });
    $("#changeProfile").live('change', function () {
        filter.HasValue = true;
        filter.Profile = $("#changeProfile").val();
        Refresh();
    });
    $("#changeFirstName").live('keyup', function () {
        filter.HasValue = true;
        filter.FirstName = $("#changeFirstName").val();
        Refresh();
    });
    $("#changeLastName").live('keyup', function () {
        filter.HasValue = true;
        filter.LastName = $("#changeLastName").val();
        Refresh();
    });
    $("#changeMiddleName").live('keyup', function () {
        filter.HasValue = true;
        filter.MiddleName = $("#changeMiddleName").val();
        Refresh();
    });
    $("#changeDocumentSeries").live('keyup', function () {
        filter.HasValue = true;
        filter.DocumentSeries = $("#changeDocumentSeries").val();
        Refresh();
    });
    $("#changeDocumentNumber").live('keyup', function () {
        filter.HasValue = true;
        filter.DocumentNumber = $("#changeDocumentNumber").val();
        Refresh();
    });
    $("#changeDiplomaSeries").live('keyup', function () {
        filter.HasValue = true;
        filter.DiplomaSeries = $("#changeDiplomaSeries").val();
        Refresh();
    });
    $("#changeDiplomaNumber").live('keyup', function () {
        filter.HasValue = true;
        filter.DiplomaNumber = $("#changeDiplomaNumber").val();
        Refresh();
    });

    $("#changeResultLevelID").live('change', function () {
        filter.HasValue = true;
        filter.ResultLevelID = $("#changeResultLevelID").val();
        Refresh();
    });
    $("#changeStatusID").live('change', function () {
        filter.HasValue = true;
        filter.StatusID = $("#changeStatusID").val();
        Refresh();
    });
    $("#changeEndingDate").live('change', function () {
        filter.HasValue = true;
        filter.EndingDate = $("#changeEndingDate").val();
        Refresh();
    });

    $(".checkFind").live('click', function () {
        var $tr = $(this).parents('tr:first');
        var itemID = $tr.attr('itemID')
        ShowButtonFind();
    });
});


$("#btnFind").live('click', function () {
    if ($(".checkFind:checked").length = 0)
        return;

    confirmDialog('Вы действительно хотите выполнить поиск?', function () {
        var list = [];

        $(".checkFind:checked").each(function () {
            var $tr = $(this).parents('tr:first');
            var itemID = $tr.attr('itemID')
            list.push(itemID);
        });

        doPostAjax('/OlympicDiplomant/Find', JSON.stringify(list), function (data) {
            if (data.IsError) { } else { };
            Refresh();
        }, null, "html");
    })
});


function ShowButtonFind() {
    if ($(".checkFind:checked").length > 0)
        $('#btnFind').removeAttr("disabled");
    else
        $('#btnFind').attr("disabled", "disabled");
}

function ToggleFilter() {
    if ($('#btnShowFilter').hasClass('filterDisplayed')) {
        $('#btnShowFilter').removeClass('filterDisplayed')
        $('#btnShowFilter').html('Отобразить фильтр')
        $('#btnShowFilter').parent().removeClass('nonHideTable')
        $('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed')
        $('#filters').hide()
    }
    else {
        $('#btnShowFilter').addClass('filterDisplayed')
        $('#btnShowFilter').html('Скрыть фильтр')
        $('#btnShowFilter').parent().addClass('nonHideTable')
        $('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed')
        $('#filters').show()
    }
};


function DoSort(el, sortID) {
    var isSortedUp = $(el).hasClass('sortedUp')
    if (isSortedUp) {
        $(el).removeClass('sortedUp');
        $(el).addClass('sortedDown');
        filter.Sort = -sortID;
    }
    else {
        $(el).removeClass('sortedDown');
        $(el).addClass('sortedUp');
        filter.Sort = sortID;
    }
    Refresh();
}

function DrawSort() {
    $('.sortUp,.sortDown').remove().detach();

    var c = filter.Sort;
    if (c < 0)
        c = -c;

    if (filter.Sort < 0) {
        $('#col' + c).after('<span class="sortDown"></span>');
        $('#col' + c).addClass('sortedDown');
    }

    if (filter.Sort > 0) {
        $('#col' + c).after('<span class="sortUp"></span>');
        $('#col' + c).addClass('sortedUp');
    }
}

//--------------------------------------------------------------------------------------------------------------------

function Refresh() {
    filter.Page = $('#pagenumber').text();
    $.ajax({
        data: filter,
        type: "POST",
        url: '/OlympicDiplomant/Index',
        cache: false,
        success: function (data) {
            $('#tablerows').html(data);
            DrawSort();
            ShowButtonFind();
        }
    });
}

//--------------------------------------------------------------------------------------------------------------------

$('.btnDelete').live('click', function () {
    var $tr = $(this).parents('tr:first');
    var itemID = $tr.attr('itemID')
    confirmDialog('Вы действительно хотите удалить?', function () {
        doPostAjax('/OlympicDiplomant/Delete', 'id=' + itemID, function (data) {
            if (data.IsError)
                $('<div>' + data.Message + '</div>').dialog({ buttons: { OK: function () { $(this).dialog('close'); } } })
            else {
                Refresh();
            }
            Refresh();
        }, "application/x-www-form-urlencoded")
    })
})

//--------------------------------------------------------------------------------------------------------------------

$('.btnEdit').live('click', function () {
    var $tr = $(this).parents('tr:first');
    var itemID = $tr.attr('itemID');
    AddEdit('/OlympicDiplomant/Edit', 'id=' + itemID, 'Редактирование победителя/призера ОШ',
        function () {
        });
})

//--------------------------------------------------------------------------------------------------------------------

$('.btnAdd').live('click', function () {
    AddEdit('/OlympicDiplomant/Add', '', 'Добавление победителя/призера ОШ',
        function () {
        });
})

//--------------------------------------------------------------------------------------------------------------------

function AddEdit(url, post, title, callback) {
    doPostAjax(url, post,
        function (data) {
            ShowDialog(data, title);
        }, "application/x-www-form-urlencoded", "html")
}

//--------------------------------------------------------------------------------------------------------------------

var dialog = null;

//--------------------------------------------------------------------------------------------------------------------

function ShowDialog(data, title) {
    dialog = $('#dialog').html(data).dialog({
        modal: true,
        width: 1200,
        title: title,
        buttons: { "Сохранить": function () { OnClickSubmitInDialog() }, "Отмена": function () { OnClickCancelInDialog() } }
    }).dialog('open');
}

//--------------------------------------------------------------------------------------------------------------------

function OnClickSubmitInDialog() {
    clearValidationErrors($('.container'));
    var model = FillModel();

    doPostAjax('/OlympicDiplomant/Save', JSON.stringify(model), function (data) {
        if (!addValidationErrorsFromServerResponse(data)) {
            dialog.dialog('close');
            dialog.empty();
            Refresh();
        }
        else {
            var control = "[name=" + data.Data[0].ControlID.replace(/\_/g, '.') + "]";
            $(control).addClass("input-validation-error");
            $('#errorMessage').text(data.Data[0].ErrorMessage);
        };
        unblockUI();
    }, null, null, false);
}

//--------------------------------------------------------------------------------------------------------------------

function OnClickCancelInDialog() {
    dialog.dialog('close');
    dialog.empty();
}

//--------------------------------------------------------------------------------------------------------------------

function FillModel() {
    var model = {
        OlympicTypeID: $('#editOlympicTypeID').val(),
        OlympicYearID: $('#editOlympicYearID').val(),
        Data: {
            OlympicDiplomantID: $('#editID').val(),
            OlympicDiplomantIdentityDocumentID: $('#editOlympicDiplomantIdentityDocumentID').val(),
            SchoolRegionID: $('#editSchoolRegionID').val(),
            FormNumber: $('#editFormNumber').val(),
            EndingDate: $('#editEndingDate').val(),
            SchoolEgeCode: $('#editSchoolEgeCode').val(),
            SchoolEgeName: $('#editSchoolEgeName').val(),
            ResultLevelID: $('#editResultLevelID').val(),
            DiplomaSeries: $('#editDiplomaSeries').val(),
            DiplomaNumber: $('#editDiplomaNumber').val(),
            DiplomaDateIssue: $('#editDiplomaDateIssue').val(),
            OlympicTypeProfileID: $('#editOlympicTypeProfileID').val(),

            StatusID: $('#editStatusID').val(),
            PersonId: $('#editPersonID').val(),
            PersonLinkDate: $('#editPersonLinkDate').val(),
            Comment: $('#editComment').val(),

            AdoptionUnfoundedComment: $('#editAdoptionUnfoundedComment').val(),
            CreateDate: AutoCalc,

            OlympicDiplomantDocument: {
                FirstName: $('#editFirstName').val(),
                LastName: $('#editLastName').val(),
                MiddleName: $('#editMiddleName').val(),
                BirthDate: $('#editBirthDate').val(),
                IdentityDocumentTypeID: $('#editIdentityDocumentTypeID').val(),
                DocumentSeries: $('#editDocumentSeries').val(),
                DocumentNumber: $('#editDocumentNumber').val(),
                OrganizationIssue: $('#editOrganizationIssue').val(),
                DateIssue: $('#editDateIssue').val(),
            },
            OlympicDiplomantDocumentCanceled: []
        }
    }

    $('#canceledTable > tbody  > tr').each(function () {
        model.Data.OlympicDiplomantDocumentCanceled.push(
            {
                OlympicDiplomantDocumentID: $(this).attr('itemID'),
                OlympicDiplomantID: $('#editID').val(),
                FirstName: $(this).find("span:[itemtype='FirstName']").text(),
                LastName: $(this).find("span:[itemtype='LastName']").text(),
                MiddleName: $(this).find("span:[itemtype='MiddleName']").text(),
                IdentityDocumentTypeID: $(this).find("span:[itemtype='IdentityDocumentTypeID']").text(),
                DocumentSeries: $(this).find("span:[itemtype='DocumentSeries']").text(),
                DocumentNumber: $(this).find("span:[itemtype='DocumentNumber']").text(),
                OrganizationIssue: $(this).find("span:[itemtype='OrganizationIssue']").text(),
                DateIssue: $(this).find("span:[itemtype='DateIssue']").text(),
                BirthDate: $(this).find("span:[itemtype='BirthDate']").text(),
            })
    });

    return model;
}

//====================================================================================================================
//
//====================================================================================================================

var row = null;

$('.btnEditS').live('click', function () {
    row = $(this).parents('tr:first');
    var itemID = row.attr('itemID');

    var model = {
        OlympicDiplomantDocumentID: row.find("span:[itemtype='OlympicDiplomantDocumentID']").text(),
        LastName: row.find("span:[itemtype='LastName']").text(),
        FirstName: row.find("span:[itemtype='FirstName']").text(),
        MiddleName: row.find("span:[itemtype='MiddleName']").text(),
        IdentityDocumentTypeID: row.find("span:[itemtype='IdentityDocumentTypeID']").text(),
        DocumentSeries: row.find("span:[itemtype='DocumentSeries']").text(),
        DocumentNumber: row.find("span:[itemtype='DocumentNumber']").text(),
        OrganizationIssue: row.find("span:[itemtype='OrganizationIssue']").text(),
        DateIssue: row.find("span:[itemtype='DateIssue']").text(),
        BirthDate: row.find("span:[itemtype='BirthDate']").text(),
    }

    AddEditCanceled('/OlympicDiplomant/EditCanceled', model, '"Редактирование сведений о недействующем документе, удостоверяющем личность',
        function () {
        });
})

$('.btnAddS').live('click', function () {
    row = null;

    var model = {
        LastName: $('#editLastName').val(),
        FirstName: $('#editFirstName').val(),
        MiddleName: $('#editMiddleName').val(),
        BirthDate: $('#editBirthDate').val(),
    }

    AddEditCanceled('/OlympicDiplomant/AddCanceled', model, 'Добавление сведений о недействующем документе, удостоверяющем личность',
        function () {
        });
})

$('.btnDeleteS').live('click', function () {
    var $tr = $(this).parents('tr:first');
    var itemID = $tr.attr('itemID')
    confirmDialog('Вы действительно хотите удалить документ?', function () {
        $tr.remove();
    });
})


function AddEditCanceled(url, post, title, callback) {
    doPostAjax(url, post,
        function (data) {
            ShowDialogCanceled(data, title);
        }, "application/x-www-form-urlencoded", "html")
}

var dialogCanceled = null;

function ShowDialogCanceled(data, title) {
    dialogCanceled = $('#dialogCanceled').html(data).dialog({
        modal: true,
        width: 1000,
        title: title,
        buttons: { "Сохранить": function () { OnClickSubmitInDialogCanceled() }, "Отмена": function () { OnClickCancelInDialogCanceled() } }
    }).keypress(function (e) { if (e.keyCode == $.ui.keyCode.ENTER) { OnClickSubmitInDialogCanceled() } }).dialog('open');
}

function OnClickCancelInDialogCanceled() {
    dialogCanceled.dialog('close');
    dialogCanceled.empty();
}

function FillModelCanceled() {
    var model = {
        OlympicDiplomantDocumentID: $('#editOlympicDiplomantDocumentID').val(),
        LastName: $('#editCanceledLastName').val(),
        FirstName: $('#editCanceledFirstName').val(),
        MiddleName: $('#editCanceledMiddleName').val(),
        IdentityDocumentTypeID: $('#editCanceledIdentityDocumentTypeID').val(),
        DocumentSeries: $('#editCanceledDocumentSeries').val(),
        DocumentNumber: $('#editCanceledDocumentNumber').val(),
        OrganizationIssue: $('#editCanceledOrganizationIssue').val(),
        DateIssue: $('#editCanceledDateIssue').val(),
        BirthDate: $('#editCanceledBirthDate').val(),
    }
    return model;
}

function OnClickSubmitInDialogCanceled() {
    clearValidationErrors($('.containerCanceled'));
    var model = FillModelCanceled();

    $.ajax({
        data: model,
        type: "POST",
        url: '/OlympicDiplomant/PresentCanceledRow',
        cache: false,
        success: function (data) {
            if (!addValidationErrorsFromServerResponse(data)) {
                if (row == null)
                    $('#canceledTable tbody').append(data);
                else
                    row.replaceWith(data);

                dialogCanceled.dialog('close');
                dialogCanceled.empty();
            } else {
                var control = "[name=Data." + data.Data[0].ControlID.replace(/\_/g, '.') + "]";
                $(control).addClass("input-validation-error");
                $('#errorMessageCanceled').text(data.Data[0].ErrorMessage);

            }
        }
    });


}

//--------------------------------------------------------------------------------------------------------------------

function SetDatepicker() {
    $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '/Resources/Images/calendar.jpg', buttonImageOnly: true, yearRange: '-40:+0' });
}

//--------------------------------------------------------------------------------------------------------------------

$("#editOlympicYearID").live('change', function () {
    var year = 'year=' + $("#editOlympicYearID").val();
    doPostAjax('/OlympicDiplomant/GetOlympicsForYear', year, function (data) {

        var ddl = $('#editOlympicTypeID').empty();
        $.each(data, function (index, item) {
            ddl.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        $("#editOlympicTypeID").val(OlympicTypeID);

        $("#editOlympicTypeID").change();

    }, "application/x-www-form-urlencoded")
})

////--------------------------------------------------------------------------------------------------------------------

$("#editOlympicTypeID").live('change', function () {
    var olympic = 'olympic=' + $("#editOlympicTypeID").val();
    doPostAjax('/OlympicDiplomant/GetProfilesForOlympic', olympic, function (data) {

        var ddl = $('#editOlympicTypeProfileID').empty();
        $.each(data, function (index, item) {
            ddl.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
        });
        $("#editOlympicTypeProfileID").val(OlympicTypeProfileID);

    }, "application/x-www-form-urlencoded")
})

//--------------------------------------------------------------------------------------------------------------------

$('#calcButton').live('click', function () {
    var model = FillModel();
    doPostAjax('/OlympicDiplomant/SyncOlympicDiplomant', JSON.stringify(model), function (data) {
        ShowDialogInfo(data, 'Важная информация о результатах поиска');
    }, null, "html");
})

//--------------------------------------------------------------------------------------------------------------------

function FillModelFilter() {
    var model = {}

    if ($('#filCheckLastName').is(':checked'))
        model.LastName = $('#filLastName').val();

    if ($('#filCheckFirstName').is(':checked'))
        model.FirstName = $('#filFirstName').val();

    if ($('#filCheckMiddleName').is(':checked'))
        model.Patronymic = $('#filMiddleName').val();

    if ($('#filCheckDocumentSeries').is(':checked'))
        model.DocumentSeries = $('#filDocumentSeries').val();

    if ($('#filCheckDocumentNumber').is(':checked'))
        model.DocumentNumber = $('#filDocumentNumber').val();

    if ($('#filCheckIdentityDocumentTypeID').is(':checked'))
        model.PersonIdentDocID = $('#filIdentityDocumentTypeID').val();

    if ($('#filCheckBirthDate').is(':checked'))
        model.BirthDay = $('#filBirthDate').val();

    return model;
}

//--------------------------------------------------------------------------------------------------------------------

$('#filFind').live('click', function () {
    var model = FillModelFilter();
    doPostAjax('/OlympicDiplomant/FilterPersons', JSON.stringify(model), function (data) {
        $('#findresultsrows').html(data);
        $('#filNotFind').attr('checked', true);
    }, null, "html");
})

//--------------------------------------------------------------------------------------------------------------------

var dialogInfo = null;
var StatusId = 0;
//var PersonId = 0;
var AutoCalc = '01.01.2001';

//--------------------------------------------------------------------------------------------------------------------

function ShowDialogInfo(data, title) {
    dialogInfo = $('#dialogInfo').html(data).dialog({
        modal: true,
        width: 800,
        title: title,
        buttons: {
            "Сохранить": {
                id: "saveButton",
                text: "Сохранить",
                click: function () {

                    var s = $('input[name=filsel]:checked').val();
                    if (s == null)
                        alert('Необходимо выбрать участника ЕГЭ для выполнения процедуры связывания');
                    else {
                        var stat = StatusId;

                        var pid = $('input[name=filsel]:checked').attr('itemId');
                        if (pid == 0)
                            pid = null;
                        $('#editPersonID').val(pid);

                        if (pid != null) {
                            AutoCalc = '01.01.2001';
                            $('#editAdoptionUnfoundedComment').val("");

                            if (stat == 1)
                                stat = 1;
                            else
                                stat = 2;
                        }
                        else
                            stat = 4;

                        $('#editStatusID').val(stat);

                        $('#editComment').val($('#filComment').val());

                        dialogInfo.dialog('close');
                        dialogInfo.empty();

                        var model = { Status: stat, Person: pid, AdoptionUnfoundedComment: $('#editAdoptionUnfoundedComment').val() };
                        doPostAjax('/OlympicDiplomant/UpdateInfo', JSON.stringify(model), function (data) {
                            $('#calcArea').html(data);
                        }, null, "html");


                    }
                }
            },
            "Закрыть": function () {
                dialogInfo.dialog('close');
                dialogInfo.empty();
            }
        }
    }).dialog('open');

    $('#filComment').val($('#editComment').val());
}

//--------------------------------------------------------------------------------------------------------------------

$('.classChange').live('change', function () {
    AutoCalc = '02.02.2002';
})

//--------------------------------------------------------------------------------------------------------------------

var dialogImport = null;

function ShowDialogImport(data, title) {
    dialogImport = $('#dialogImport').html(data).dialog({
        modal: true,
        width: 850,
        height: 600,
        title: title,
        buttons: {
            "Закрыть": function () {
                dialogImport.dialog('close');
                dialogImport.empty();
            }
        }
    }).dialog('open');
}

$('.btnLoad').live('click', function () {
    //Opera
    if ($.browser.opera) {
        $('#csvInput').css({ "position": "absolute", "top": "0", "left": "-9999px" });
        $('#csvInput').show();
    }
    //End: Opera 

    $('#csvInput').click(); 
})

$('#csvInput').live('change', function () {
    UploadOlympicsCSV();
});

$('#csvInput').live('input', function () {
    UploadOlympicsCSV();
});

function UploadOlympicsCSV() {
     
    $('#csvInput').focusout();
    var file = $('#csvInput').get()[0].files[0];
    if (file != null) {
        blockUI();
        var form = new FormData();
        form.append("file", file);

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/OlympicDiplomant/UploadFiles");

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4) {
                if (xhr.status == 200) {
                    var json = JSON.parse(xhr.responseText);
                    ShowDialogImport(json.Data, 'Результат импорта');

                    $('#csvInput').replaceWith($('#csvInput').clone(true));

                    unblockUI();
                    Refresh();
                }
            }
        };
    }
    xhr.send(form);
}
 

//--------------------------------------------------------------------------------------------------------------------



