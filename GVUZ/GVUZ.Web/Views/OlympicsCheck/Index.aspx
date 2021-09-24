<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OlympicsCheckViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Олимпиады школьников
</asp:Content>

<asp:Content runat="server" ID="PageSubtitle" ContentPlaceHolderID="PageSubtitle">
    Проверка результатов участия в олимпиадах
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <% using (Html.BeginForm("Submit", "OlympicsCheck", FormMethod.Post)) { %>
   <table style="border: none;border-collapse: collapse;">
       <tr>
           <td style="white-space: nowrap;padding-right: 8px;"><%:Html.LabelFor(m => m.ParticipantName) %><sup style="color: red">*</sup></td>
           <td><%:Html.TextBoxFor(m => m.ParticipantName, new { maxlength=64 }) %></td>
       </tr>
       <tr>
           <td colspan="2" style="text-align: right">
               <%:Html.ValidationMessageFor(m => m.ParticipantName) %>
           </td>
       </tr>
       <tr>
           <td style="white-space: nowrap;padding-right: 8px;"><%:Html.LabelFor(m => m.DocumentNumber) %><sup style="color: red">*</sup></td>
           <td><%:Html.TextBoxFor(m => m.DocumentNumber, new { maxlength = 32 }) %></td>
       </tr>
       <tr>
           <td colspan="2" style="text-align: right">
               <%:Html.ValidationMessageFor(m => m.DocumentNumber) %>
           </td>
       </tr>
       <tr>
           <td colspan="2" style="text-align: right">
               <input type="submit" value="Проверить"/>
           </td>
       </tr>
    </table>
   <% } %>

</asp:Content>