<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Esrp.Web.News"
    MasterPageFile="~/Common/Templates/Main.master" %>
    
<asp:Content ContentPlaceHolderID="cphContent" runat="server">

					<div class="col_in">
						<div class="statement">
   							<p class="title"><%=CurrentNews.Name %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Назад</span></a></p>
							<div class="clear"></div>
   							<div class="statement_in">
<p>
    <%=CurrentNews.Text %>
</p>                          
                            </div>
                        </div>
                    </div>
</asp:Content>
