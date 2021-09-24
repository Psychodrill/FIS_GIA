<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestForm.aspx.cs" 
 Inherits="Esrp.Web.Administration.Requests.RequestForm"	MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ OutputCache Location="None"  %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Users" %>
<%@ Import Namespace="Esrp.Web" %>

<%@ Register Src="~/Controls/OperatorPanelForRequest.ascx" TagName="CommentPanel" TagPrefix="uc1" %>
<%@ Register TagPrefix="esrp" TagName="UsersInfo" Src="~/Controls/UsersInfo.ascx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphActions">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
				<div class="right_col">
					<div class="col_in">
                        <uc1:CommentPanel ID="CommentPanel" runat="server" />						
					</div>
				</div>
<div class="left_col">
    <asp:ValidationSummary  CssClass="error_block" ID="ValidationSummary2" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />

                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Заявление на регистрацию № <%= RequestID%></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
                                <a href="#" class="gray" runat="server" ID="liActivate" title="Активировать">Активировать</a>
				                <a href="#" class="gray" runat="server" ID="liRevision" title="Отправить на доработку">Отправить на доработку</a>
				                <a href="#" class="gray" runat="server" ID="liDeactivate" title="Отключить">Отключить</a>
							</p>
							<div class="clear"></div>
							<div class="statement_table statement1">
                                    <esrp:UsersInfo runat="server" ID="usersInfo" />
                            </div>
                            <div class="statement_table statement1" style="margin-top: -30px;">
				<table width="100%">					
					<tr>
						<th style="border-top-color:#fff;">
							<span>Полное наименование организации<br />
								(без организационно-правовой формы)</span>
						</th>
						<td style="border-top-color:#fff;">
							<%= OrgRequest.Organization.OrgFullName %>
							<br/> <asp:Label runat="server" ID="lblEtalonOrgName" Style="white-space:normal;" />
						</td>
					</tr>
					<tr>
						<th>
							<span>Краткое наименование организации</span>
						</th>
						<td>
							<%= OrgRequest.Organization.OrgShortName %>
						</td>
					</tr>
					<tr>
						<th>
							Субъект Российской Федерации, на территории которого находится организация
						</th>
						<td>
							<%= OrgRequest.Organization.RegionName %>
							<BR/> <asp:Label runat="server" ID="lblEtalonRegion" Style="white-space:normal;" />
						</td>
					</tr>
					<tr>
                    <th>Тип ОУ</th>
                    <td><%= String.IsNullOrEmpty(OrgRequest.Organization.OrgTypeName) ? "Не задан" : OrgRequest.Organization.OrgTypeName%>
                    <BR/> <asp:Label runat="server" ID="lblEtalonOrgType" Style="white-space:normal;" /></td>
                </tr>
                    <tr>
                    <th>Вид ОУ</th>
                    <td><%= String.IsNullOrEmpty(OrgRequest.Organization.OrgKindName) ? "Не задан" : OrgRequest.Organization.OrgKindName%>
                    </td>
                </tr>
				<tr>
                    <th>Организационно-правовая форма ОУ</th>
                    <td><%= OrgRequest.Organization.IsPrivate ? "Негосударственный" : "Государственный"%>
                    </td>
                </tr>
				<tr>
                    <th>Является филиалом</th>
                    <td><%= OrgRequest.Organization.IsFilial ? "Да" : "Нет"%>
                    </td>
                </tr>
                <tr>
                    <th>ОГРН</th>
                    <td><%= OrgRequest.Organization.OGRN%>
                    <BR/> <asp:Label runat="server" ID="lblEtalonOGRN" Style="white-space:normal;" />
                    </td>
                </tr>

                <tr>
                    <th>ИНН</th>
                    <td><%= OrgRequest.Organization.INN%>
                    <BR/> <asp:Label runat="server" ID="lblEtalonINN" Style="white-space:normal;" />
                    </td>
                </tr>

                <tr>
                    <th>КПП</th>
                    <td>
                        <%=OrgRequest.Organization.KPP%>
                        <BR/> <asp:Label runat="server" ID="lblEtalonKPP" Style="white-space:normal;" />
                    </td>
                </tr>

				<tr>
					<th>
						Учредитель (для ССУЗов, ВУЗов и РЦОИ)
					</th>
					<td>
						<%= OrgRequest.Organization.FounderName %>
						<BR/> <asp:Label runat="server" ID="lblEtalonFounder" Style="white-space:normal;" />
					</td>
				</tr>
				<tr>
                    <th>Должность руководителя организации</th>
                    <td><%= OrgRequest.Organization.DirectorPosition %>
                    </td>
                </tr>
					<tr>
						<th>
							Ф. И. О. руководителя организации
						</th>
						<td>
							<%= OrgRequest.Organization.DirectorFullName %>
							<BR/> <asp:Label runat="server" ID="lblEtalonChief" Style="white-space:normal;" />
						</td>
					</tr>
				<tr>
                    <th>Фактический адрес</th>
                    <td><%= OrgRequest.Organization.FactAddress%>
                    <BR/> <asp:Label runat="server" ID="Label8" Style="white-space:normal;" /></td>
                </tr>
				<tr>
					<th>
						Юридический адрес
					</th>
					<td>
						<%= OrgRequest.Organization.LawAddress %>
						<BR/> <asp:Label runat="server" ID="lblEtalonAddress" Style="white-space:normal;" />
					</td>
				</tr>
                <tr id="trReceptionOnResultsCNE" runat="server" Visible="false">
					<th>
						Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
					</th>
					<td class="text">
                        <asp:Label runat="server" ID="lblReceptionOnResultsCNE" />
                        <BR/>
                        <asp:Label runat="server" ID="lblEtalonReceptionOnResultsCNE" Style="white-space:normal;" />
					</td>
				</tr>
				<tr>
                    <th>Свидетельсво об аккредитации</th>
                    <td class="text"><%= OrgRequest.Organization.AccreditationSertificate%>
                    </td>
                </tr>
                    <tr>
                    <th>Код города</th>
                    <td><%= OrgRequest.Organization.PhoneCityCode%>
                    </td>
                </tr>
					<tr>
						<th>
							Телефон руководителя организации
						</th>
						<td>
							<%= OrgRequest.Organization.Phone %>
							<BR/> <asp:Label runat="server" ID="lblEtalonOrgPhone" Style="white-space:normal;" />
						</td>
					</tr>
					<tr>
						<th>
							Факс
						</th>
						<td>
							<%= OrgRequest.Organization.Fax %>
						</td>
					</tr>
					<tr>
                    <th>EMail</th>
                    <td><%= OrgRequest.Organization.EMail  %></td>
                </tr>
                    <tr>
                    <th>Сайт</th>
                    <td><%= OrgRequest.Organization.Site%></td>
                </tr>
					<%  foreach (OrgUserBrief user in OrgRequest.Organization.ActivatedUsers)
						{ %>
					<tr>
						<th>
							Ф. И. О. лица, ответственного за работу с <%= user.GetSystemsName() %>
						</th>
						<td>
							<%= user.GetFullName()%>
						</td>
					</tr>
					<tr>
						<th>
							Телефон лица, ответственного за работу с <%= user.GetSystemsName() %>
						</th>
						<td>
							<%= user.Phone%>
						</td>
					</tr>
					<% } %>
				</table>
                            </div>
                        </div>
                    </div>

                </div>
</form>
</asp:Content>

