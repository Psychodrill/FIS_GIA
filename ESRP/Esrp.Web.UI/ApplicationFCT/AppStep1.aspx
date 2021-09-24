<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppStep1.aspx.cs" Inherits="Esrp.Web.ApplicationFCT.AppStep1" 
MasterPageFile="~/Common/Templates/Main.master" ValidateRequest="False" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/cusel-min-2.5.js"></script>
    <script type="text/javascript" src="/Common/Scripts/js.js"></script>
    <script language="javascript" type="text/javascript">

    </script>
    <form id="form1" runat="server">    
				<h1>Шаг 1 из 2</h1><br/>  
				<div class="statement edit">               
					<p class="back" style="margin-bottom:20px;">
						<a href="AppEnter.aspx"><span class="un">Вернуться</span></a>
					</p>
				</div>
				<h2>ФОРМА №1. КАРТОЧКА ПОДКЛЮЧАЕМОЙ ОРГАНИЗАЦИИ</h2>
				<div class="statement_table">
					<table width="100%">
						<tbody>
						<tr>
							<th>
								E-mail учетной записи в ФИС ЕГЭ и Приема
							</th>
							<td width="1">
								<asp:label runat="server"  id="EMail" value="~~~"/>
							</td>
						</tr>
						<tr>
							<th>
								 Приложите скан-копию приказа о назначении ответственного лица, отвечающего за вопросы реализации и поддержки работоспособности и корректности эксплуатации схемы подключения ФГБУ "ФЦТ"<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>                            
							<td>                                
                                <asp:label runat="server" ID="ScanOrderText" class="small text" />
                                <br />
                                <br />
                                <asp:LinkButton ID="LoadScan" runat="server" OnClick="ShowOrder">Показать скан-копию</asp:LinkButton><br />							
                                <br />
                                <asp:FileUpload ID="Order4Person" runat="server" class="long" Width="532" size="61"/>								
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ControlToValidate="Order4Person" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Скан-копия приказа о назначении ответственного лица" обязательно для заполнения' />								

							</td>
						</tr>						
						<tr>
							<th rowspan="2">
								Полное и краткое наименование организации
							</th>
							<td width="1">
								<asp:label runat="server"  id="FullName" />								
							</td>
						</tr>
						<tr>
							<td width="1">
								<asp:label runat="server"  id="BriefName" />								
							</td>
						</tr>
						<tr>
							<th>
								Фактический адрес организации
							</th>
							<td width="1">
								<asp:label runat="server"  id="RealAddress" />								
							</td>
						</tr>
						<tr>
							<th>
								Телефон организации
							</th>
							<td width="1">
								<asp:label runat="server" ID="OrgPhone" />
							</td>
						</tr>
						<tr>
							<th>
								Фамилия, имя, отчество ответственного за схемы подключения к ФЦТ лица<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>
							<td width="1">
								<asp:textbox runat="server" CssClass="txt long" MaxLength="512" id="FIO" />								
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="FIO" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "ФИО ответственного лица" обязательно для заполнения' />								
                                <asp:RegularExpressionValidator ID="revFIO" runat="server" ControlToValidate="FIO"
                                EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "ФИО ответственного лица" заполнено неверно'
                                ValidationGroup="common_group" ValidationExpression="[\sа-яА-Я]{5,512}"></asp:RegularExpressionValidator>
							</td>
						</tr>
						<tr>
							<th>
								Должность ответственного  за схемы подключения к ФЦТ лица<span class="required" style="color:rgb(215, 5, 5)">(*)</span> 
							</th>
							<td width="1">
								<asp:textbox runat="server" CssClass="txt long" MaxLength="64"   id="Position" />								
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Position" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Должность ответственного лица" обязательно для заполнения' />								
                                <asp:RegularExpressionValidator ID="revPostion" runat="server" ControlToValidate="Position"
                                EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Должность ответственного лица" заполнено неверно'
                                ValidationGroup="common_group" ValidationExpression="[\-\sа-яА-Я0-9]{2,64}"></asp:RegularExpressionValidator>

							</td>
						</tr>
						<tr>
							<th>
								Рабочий телефон ответственного за схемы подключения ФЦТ лица: +7&nbsp;(код)&nbsp;номер<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>
							<td width="1">
								<asp:textbox runat="server" CssClass="txt small" MaxLength="14"  id="WorkPhone" />							
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="WorkPhone" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Рабочий телефон ответственного лица" обязательно для заполнения' />								
                               <asp:RegularExpressionValidator ID="revWorkPhone" runat="server" ControlToValidate="WorkPhone"
                                EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Рабочий телефон ответственного лица" заполнено неверно'
                                ValidationGroup="common_group" ValidationExpression="^\+7\([0-9]{3,5}\)[0-9]{5,7}$"></asp:RegularExpressionValidator>

							</td>
						</tr>
						<tr>
							<th>
								Мобильный телефон ответственного лица: +7&nbsp;(код&nbsp;ОСС)&nbsp;номер<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>
							<td width="1">
								<asp:textbox runat="server" CssClass="txt small" MaxLength="14"  id="MPhone" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="MPhone" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Мобильный телефон ответственного лица" обязательно для заполнения' />								
                                <asp:RegularExpressionValidator ID="revMPhone" runat="server" ControlToValidate="MPhone"
                                EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Мобильный телефон ответственного лица" заполнено неверно'
                                ValidationGroup="common_group" ValidationExpression="^\+7\([0-9]{3,5}\)[0-9]{5,7}$"></asp:RegularExpressionValidator>
							</td>							
						</tr>	
						<tr>
							<th>
								Е-mail ответственного  за схемы подключения ФЦТ лица<span class="required" style="color:rgb(215, 5, 5)">(*)</span> 
							</th>
							<td width="1">
								<asp:textbox runat="server" CssClass="txt long" MaxLength="64"  id="EMail4Person" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EMail4Person" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Е-mail ответственного лица" обязательно для заполнения' />								
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidatorEMail" ControlToValidate="EMail4Person"
                                    ErrorMessage='Поле "Е-mail ответственного лица" заполнено неверно' EnableClientScript="false" Display="None" 
                                    Text="*" ValidationGroup="common_group" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" />
							</td>
						</tr>	
						<tr>
							<th>
								Есть ли аттестат на ИСПДн не ниже К1?
							</th>
							<td width="1">
                                <asp:RadioButtonList ID="rblIsThereAttestat" runat="server" CssClass="radio-button-list">
                                    <asp:ListItem Text="Да" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Нет" Value="0"></asp:ListItem>
                                 </asp:RadioButtonList>
							</td>
						</tr>	
						<tr>
							<th>
								Количество подключаемых АРМ<span class="required" style="color:rgb(215, 5, 5)">(*)</span> 
							</th>
							<td width="1">
								<asp:textbox MaxLength="1" runat="server" CssClass="txt small"  id="NumARMs" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="NumARMs" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Количество подключаемых АРМ" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="regValNumARMs" runat="server" ControlToValidate="NumARMs"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Количество подключаемых АРМ" заполнено неверно' 
                                    ValidationExpression="^[1-9]{1}$" ValidationGroup="common_group" />

							</td>
						</tr>	
						<tr>
							<th>
								Количество субъектов  ПДн за год<span class="required" style="color:rgb(215, 5, 5)">(*)</span> 
							</th>
							<td width="1">
								<asp:textbox MaxLength="5" runat="server" CssClass="txt small"  id="NumPD" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="NumPD" ValidationGroup="common_group"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Количество субъектов ПДн за год" обязательно для заполнения' />								
                                  <asp:RegularExpressionValidator ID="regNumPD" runat="server" ControlToValidate="NumPD"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Количество субъектов ПДн за год" заполнено неверно' 
                                    ValidationExpression="^([1-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]|[1-9][0-9][0-9][0-9][0-9])$" ValidationGroup="common_group" />
							</td>
						</tr>													
						<tr>
							<td colspan="2" class="box-submit" style="border-bottom-color: #fff;">								
                                <asp:Button runat="server" ID="btnGotoStep2" Text="Далее >>" CssClass="bt" OnClick="ValidateStep1" Width="100px" />
							</td>
						</tr>
					</tbody>
                    </table>
                    <asp:ValidationSummary CssClass="error_block" runat="server" ID="vsErrors" DisplayMode="BulletList"  ValidationGroup="common_group" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />                    
				</div>
                <div class="clear"></div>
  </form>
</asp:Content>

