<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupList.ascx.cs" Inherits="Esrp.Web.Controls.GroupList" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.HtmlControls" Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>    
 <script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"> </script>
           <script language="javascript" type="text/javascript">
               $(document).ready(function () {                   
                   removeFirstLineFtomTable("#<% =this.dgGroupList.ClientID %>");
               });
               function removeFirstLineFtomTable(idtable) {
                   $(idtable + ' tr:first th').css('border-top-color', '#fff');                   
               }
    </script>
    <asp:DataGrid runat="server" ID="dgGroupList" DataSourceID="dsGroupList" AutoGenerateColumns="false" UseAccessibleHeader="true"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th"  >
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                    Наименование группы
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="groupId" Value='<%# Eval("Id") %>'/>
                    <a href="#" id="editGroup" runat="server" onclick="showUpdateGroupDlg(); return false;" ><%#Eval("Name")%></a>
                </ItemTemplate>
                <ItemStyle Width="200px"/>
            </asp:TemplateColumn>            
            <asp:TemplateColumn>
                <HeaderStyle />
                <HeaderTemplate>
                    Обозначение группы
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lCode"><%#Eval("Code")%></asp:Label>                    
                </ItemTemplate>
                <ItemStyle Width="170px"/>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle />
                <HeaderTemplate>
                    По умолчанию 
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:RadioButton ID="rdoId" Checked='<%# Bind("Default")%>' runat="server" OnClick="javascript:selectOne(this, this.id,'dgGroupList');" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="130px" />
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" />
                <HeaderTemplate>
                    <div></div>
                </HeaderTemplate>
                <ItemTemplate> 
                    <asp:LinkButton ID="lbDeleteGroup" runat="server" CommandArgument='<%# Eval("Id") %>' OnCommand="DeleteGroup">X</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="dgGroupList">
        <Message>
            <p class="notfound">
                Групп нет</p>
        </Message>
    </web:NoRecordsText>
    
    <asp:ObjectDataSource runat="server" ID="dsGroupList" 
        TypeName="Esrp.Services.GroupService" DataObjectTypeName="Esrp.Web.ViewModel.Group.GroupView"
        SelectMethod="SelectGroupList" OnSelecting="DsGroupListOnSelecting" ></asp:ObjectDataSource>
                
    <asp:HiddenField runat="server" ID="hfDefaultGroupId" Value=""/>
    <script language="javascript" type="text/javascript">
        function selectOne(a, rdoId, gridName) {
            var rdo = document.getElementById(rdoId);
            /* Getting an array of all the "INPUT" controls on the form.*/
            var all = document.getElementsByTagName("input");
            for (i = 0; i < all.length; i++) {
                /*Checking if it is a radio button, and also checking if the
                id of that radio button is different than "rdoId" */
                if (all[i].type == "radio" && all[i].id != rdo.id) {
                    var count = all[i].id.indexOf(gridName);
                    if (count != -1) {
                        all[i].checked = false;
                    }
                }
            }
            rdo.checked = true; /* Finally making the clicked radio button CHECKED */
            var parent = $(a).closest("tr");
            var groupId = parent.find($('input[id*="groupId"]')).val();
            $('#<%=hfDefaultGroupId.ClientID %>').val(groupId);
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('a[id*="editGroup"]').click(function () {

                var parent = $(this).closest("tr");
                $('#fgroupName').val($(this).text());
                $('#fgroupCode').val(parent.find($('SPAN[id*="lCode"]')).text());
                var groupId = parent.find($('input[id*="groupId"]')).val();
                showUpdateGroupDlg(groupId);
            });
        });
</script>
    