<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OlympicFiles.OlympicFileUploadViewModel>" %>

<script type="text/javascript">
    
  function uploadDialogSubmit(dialogCallback) {

      $('#uploadForm').ajaxSubmit({
          method: 'POST',
          dataType: 'json',
          url: $('#uploadForm').attr('action'),
          error: showAsyncFilePostError,
          success: function (res) {
              if (!res.IsError) {
                  dialogCallback.call(window);
              }
              else if (res.Message) {
                  alert(res.Message);
              }
          }
      });
  }

</script>
<form id="uploadForm" method="POST" action="<%= Url.Action("Submit") %>" enctype="multipart/form-data">
    <table style="width: 100%">
        <colgroup>
            <col style="width: 10%;padding: 8px"/>
            <col style="width: 90%;padding: 8px"/>
        </colgroup>
        <tr>
            <td valign="top"><%= Html.LabelFor(x => x.Comments) %>:</td>
            <td><%= Html.TextAreaFor(x => x.Comments, new { style = "resize: none;width: 100%", rows="6"}) %></td>
        </tr>
        <tr>
            <td style="font-weight: bold" valign="top"><%= Html.LabelFor(x => x.UploadFile) %>:</td>
            <td><input type="file" id="UploadFile" name="UploadFile" style="width: 100%"></td>
        </tr>
    </table>
</form>
<iframe id="submitTarget" name="submitTarget" style="display: none;width: 1px;height: 1px;"></iframe>


