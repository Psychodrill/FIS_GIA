<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>
<script language="javascript" type="text/javascript">
  function EditDocument(EntrantDocumentID, DocTypeID) {
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null, null, null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: DocTypeID }, function (data) {
      
      $('#UniDDialog').html(data);
      UniDFormInit();
      $('#UniDDialog').dialog({
        modal: true,
        width: 800,
        title: "Документ",
        buttons: { 
          "Сохранить": function () { 
            var baseModel={
              EntrantID: $('#EntrantID').val()
            };
            var doc=UniDPrepareModel(baseModel);
            if(doc!=null || doc!=undefined){
              // Сохранение, если соханилось нормальн то закрыть и обновить.
              UniDSave(doc, function(model){
                var d={};
                d.EntrantDocumentID=doc.EntrantDocumentID;
                d.DocumentTypeName=doc.EntDocIdentity.IdentityDocumentTypeName;
                d.DocumentSeriesNumber=(doc.DocumentSeries+' '+doc.DocumentNumber);
                d.DocumentDate=doc.DocumentDate;
                d.DocumentOrganization=doc.DocumentOrganization;
                renderDocument($('#trEntityDocsAddNew'), d);
                // Обновляем данные по model
                $('#UniDDialog').dialog('close');
              }, function(e){
                  infoDialog("Не удалось сохранить документ! "+e);
              });
            }else{
            
            }
          },
          "Закрыть": function () { $(this).dialog('close'); },
        },
        close: function () { }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");
  }

  //-- Открываем диалог просмотра документа --
  function ViewDocument(EntrantDocumentID) {
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
      $('#UniDDialog').html(data);
      $('#UniDDialog').dialog({
        modal: true,
        width: 800,
        title: "Документ",
        buttons: {  "Закрыть": function () { $(this).dialog('close'); }}
        ,  close: function () { }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");

  }
</script>
