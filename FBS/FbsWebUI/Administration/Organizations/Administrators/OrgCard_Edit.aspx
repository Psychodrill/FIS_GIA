<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
	AutoEventWireup="true" CodeBehind="OrgCard_Edit.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.OrgCard" %>

<%@ Register src="~/Controls/OrgFilialsList.ascx" TagName="FilialsList" TagPrefix="fl" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="asp" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<script type="text/javascript" src="/Common/Scripts/jquery.min.js"></script>
	<script type="text/javascript">

		$(document).ready(function () {

			//Скрываем строки таблиц для Учредителей
			if ($("#<%=hiddenKindIdEdit.ClientID %>").val() == 6) {
				$(".displayNone").css("display", "none");
			}
			else {
				$(".displayNone").css("display", "table-row");
			}

		});
	</script>
	<style type="text/css">
		#filname span
		{
			white-space: normal;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
<div class="h10">
	</div>
	<div class="border-block">
		<div class="tr">
			<div class="tt">
				<div>
				</div>
			</div>
		</div>
		<div class="content">
			<ul>
				<li><a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%= GetParam_Int("OrgID") %>"
					title="История изменений" class="gray">История изменений</a></li>
                <li>
                    <a href="/Administration/Organizations/CertificateListCommon.aspx?OrgId=<%= GetParam_Int("OrgID") %>"
					title="История проверок свидетельств организацией" class="gray">История проверок свидетельств организацией</a>
                </li>
			</ul>
		</div>
		<div class="br">
			<div class="tt">
				<div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
	<form id="Form1" runat="server">
	<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
		EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
	<asp:Label ID="lblError" Visible="False" runat="server" ForeColor="Red" />
	<asp:HiddenField ID="hiddenKindIdEdit" runat="server" />
	<table class="form" style="width: 573px">
		<tr>
			<td class="left" width="300px">
				Регион
			</td>
			<td class="text" colspan="2">
				<asp:Literal runat="server" ID="LRegion"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Тип
			</td>
			<td colspan="2">
				<asp:Literal runat="server" ID="LOrgLevel"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Вид
			</td>
			<td colspan="2">
				<asp:Literal runat="server" ID="LOrgKind"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Организационно-правовая форма
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LOPF"></asp:Literal>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left">
				Является филиалом
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LIsFilial"></asp:Literal>
			</td>
		</tr>
		<asp:PlaceHolder ID="phMainOrgName" runat="server">
			<tr>
				<td class="left">
					Головная организация
				</td>
				<td colspan="2" style="vertical-align: middle;">
					<asp:Literal runat="server" ID="lblMainOrgName"></asp:Literal>
				</td>
			</tr>
		</asp:PlaceHolder>
        <tr>
            <td class="left">
                Статус
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="lblOrgStatus"></asp:Literal>
                <br />
                <asp:Label ID="lblNewOrgName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                Модель приемной кампании
            </td>
            <td colspan="2" style="vertical-align: top;" class="text">
                <asp:Literal runat="server" ID="lblModelName"></asp:Literal>
            </td>
        </tr>
		<tr class="displayNone">
			<td colspan="3" style="padding-bottom: 0;">
				<table id="Table1" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
					<tr>
						<td>
							<strong>Филиалы организации:</strong>
						</td>
					</tr>
					<tr>
						<td style="text-align: center;">
							<fl:FilialsList runat="server"/>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left" colspan="3" style="text-align: center; padding-top: 0;">
				<asp:CustomValidator ID="vlStruct" Visible="false" runat="server" ErrorMessage="Ошибки при заполнении сведений об объеме и структуре приема"
					EnableClientScript="true"></asp:CustomValidator>
			</td>
		</tr>

        <%if (User.IsInRole("ViewAdministrationSection"))
        { %>
        <tr>
            <td>   
                <asp:CheckBox runat="server" ID="cbIsLogCheckEvent" Text="Отключить журналирование" CssClass="checkbox-box" />
            </td>
        </tr>
        <% } %>
		<tr class="displayNone">
			<td class="left" colspan="3" style="text-align: center; padding-top: 0;">
				<strong>Сведения об объеме и структуре приема<sup>(1)</sup></strong>
			</td>
		</tr>
		<!-- 
		Чтобы не перекраивать всю верстку на странице делаем ячейку объединяющую все столбцы
		и в нее добавляем таблицу с нужным размером столбцов 
		
		Чтобы не перекраивать все стили, CSS пишется в самих тэгах
		-->
		<tr class="displayNone">
			<td colspan="3">
				<table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
					<asp:PlaceHolder ID="phCommonError" runat="server" Visible="false">
						<tr>
							<td colspan="2" style="vertical-align: middle; width: 100%; padding-left: 0;">
								<asp:Label ID="lblCommonError" runat="server" Text="" ForeColor="Red"></asp:Label>
							</td>
						</tr>
					</asp:PlaceHolder>
					<tr>
						<td class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
							Общее количество мест обучающихся за счет бюджета по различным формам обучения:
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td align="right" style="vertical-align: middle; width: 100%; padding-left: 0;">
							<asp:Label ID="lblFederalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Очная
						</td>
						<td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxFBFullTime" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle; width: 100%; padding-left: 0;" align="right">
							<asp:Label ID="lblTargetedError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Очно-заочная
						</td>
						<td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxFBEvening" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle; width: 100%; padding-left: 0;" align="right">
							<asp:Label ID="lblLocalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Заочная
						</td>
						<td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxFBPostal" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
					<tr>
						<td class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
							Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
							установленное учредителем по различным формам обучения:
						</td>
						<td>
						</td>
					</tr>
					<tr>
						<td style="vertical-align: middle; width: 100%; padding-left: 0;" align="right">
							<asp:Label ID="lblPayingError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Очная
						</td>
						<td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxPayFullTime" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
					<tr>
						<td style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0;
							text-align: right;" align="right">
							<asp:Label ID="lblFullTimeError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Очно-заочная
						</td>
						<td class="text" style="padding-top: 5px; vertical-align: middle; padding-right: 0;
							padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxPayEvening" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
					<tr>
						<td style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0;
							text-align: right" align="right">
							<asp:Label ID="lblEveningError" runat="server" Text="" ForeColor="Red"></asp:Label>
							Заочная
						</td>
						<td class="text" style="padding-top: 5px; vertical-align: middle; padding-right: 0;
							padding-left: 20px;">
							<asp:TextBox runat="server" ID="txtBxPayPostal" CssClass="txt" Width="50px" MaxLength="5" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left" colspan="3" style="text-align: center;">
				<strong>Детальная информация об ОУ</strong>
			</td>
		</tr>
		<!-- -->
		<tr>
			<td class="left">
				Полное наименование
			</td>
			<td class="text" colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LFullName"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Краткое наименование
			</td>
			<td class="text" colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LShortName"></asp:Literal>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left">
				ИНН
			</td>
			<td class="text" colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LINN"></asp:Literal>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left">
				ОГРН
			</td>
			<td class="text" colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LOGRN"></asp:Literal>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left">
				Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LOwnerDepartment"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Должность&nbsp;руководителя
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LDirectorPosition"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				ФИО&nbsp;руководителя
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LDirectorName"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Фактический адрес
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LFactAddress"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Юридический адрес
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LJurAddress"></asp:Literal>
			</td>
		</tr>
		<tr class="displayNone">
			<td class="left">
				Свидетельство об аккредитации
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LAccredCert"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Код города
			</td>
			<td style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LCityCode"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Телефон
			</td>
			<td style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LPhone"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Факс
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LFax"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				E-mail
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LEMail"></asp:Literal>
			</td>
		</tr>
		<tr>
			<td class="left">
				Сайт
			</td>
			<td colspan="2" style="vertical-align: middle;">
				<asp:Literal runat="server" ID="LSite"></asp:Literal>
			</td>
		</tr>        
		<asp:PlaceHolder ID="phUpdate" runat="server">
			<tr>
				<td colspan="2" id="noteTd" class="box-submit" style="text-align: left; font-size: .8em;">
					(1) Сведения об объеме и структуре приема заполняются для ОУ в целом
				</td>
				<td class="box-submit" colspan="3">
					<asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click"
						OnClientClick="btnUpdateClick()" />
				</td>
			</tr>
		</asp:PlaceHolder>
	</table>
	</form>
</asp:Content>
