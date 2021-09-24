<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>  
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    —труктура образовательной организации
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">—ведени€ об образовательной организации</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="divstatement" id="structureList">
        <gv:TabControl runat="server" ID="tabControl" />
        <div id="divContent" style="background-color: white;">
            <table width="99%" cellpadding="1" id="table4Controls">
                <tbody id="TFCBody">
                    <tr v-for="(filial, index) in filials">
                        <td>
                            <input type="radio" :value="filial" v-model="selected" @click="onRadioClick(filial.InstitutionId)" />
                            <label style="font-size: 14px">{{filial.FullName}}</label>
                        </td>
                     </tr>
                </tbody>
            </table>
        </div>
        <!-- div id="treeControl"> </div -->
    </div>
    <script type="text/javascript">
            var app = new Vue({
            el: '#structureList',
            data: {
                filials: [],
                mainId: null, //убрать
                currentId: null,
                selected: []
            },
            created: function () {
                this.loadData()
            },
            methods: {
                loadData: function () {
                    doPostAjax('<%= Url.Generate<StructureController>(x => x.GetFilials()) %>', null, function (data)
                    {
                        <%--app.mainId = <%= InstitutionHelper.MainInstitutionID %>;--%>
                        app.currentId = <%= InstitutionHelper.GetInstitutionID(false) %>;
                        
                        if (!addValidationErrorsFromServerResponse(data, false))
                        {
                            app.filials = data.Data;
                            app.selected = _findByKey(app.filials, 'InstitutionId', app.currentId); 
                        }
                    })
                },
                onRadioClick: function (institutionId) {
                    $.ajax({ async: false, type: 'POST', url: '<%= Url.Generate<StructureController>(x => x.SetInstitution(null)) %>?institutionId=' + institutionId });
                    document.location.reload();
                }
            },
            computed: {
                
            }
        })
        menuItems[1].selected = true;

        var UserReadonly = <%= UrlUtils.IsReadOnly(FBDUserSubroles.InstitutionDataDirection) ? "true" : "false" %>;
        
    </script>
</asp:Content>