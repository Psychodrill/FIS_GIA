<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppStep2.aspx.cs" Inherits="Esrp.Web.ApplicationFCT.AppStep2"MasterPageFile="~/Common/Templates/Main.master" ValidateRequest="False" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/cusel-min-2.5.js"></script>
    <script type="text/javascript" src="/Common/Scripts/js.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(
         function () {
             if (OnSystemsChange())
                VipNetOff()
             else
                 VipNetOn()             
                 

             $("#<%=ddlOperationSystem.ClientID %>").change(function () {
                 if (OnSystemsChange())
                     VipNetOff()
                 else
                     VipNetOn()
             })


             $("#<%=ddlUnauthAccessProtect.ClientID %>").change(function () {
                   OnUAProtectChange()
             })


             OnIPTypeChange()


             $("#<%=ddlIPType.ClientID %>").change(function () {
                 OnIPTypeChange()   
             })

         })

         function OnIPTypeChange() {
             var IPType = document.getElementById('<%=ddlIPType.ClientID %>')

             if (IPType.options[IPType.selectedIndex].text == 'Динамический') {
                 $("#bdStaticIPs").hide()
                 $("#trDynamicText").show()
             }
             else {
                 $("#trDynamicText").hide()
                 $("#bdStaticIPs").show()
             }
         }

         function VipNetOn() {
            $("#<%=ddlAntivirus.ClientID %>").hide()
            $("#<%=ddlUnauthAccessProtect.ClientID %>").hide()
            $("#<%=ddlElectronicLock.ClientID %>").hide()
            $("#<%=ddlTNScreen.ClientID%>").hide()            
            $("#<%=ddlVipNetCrypto.ClientID%>").hide()

            $("#txtAntivirus").show()
            $("#txtUnauthAccessProtect").show()
            $("#txtElectronicLock").show()
            $("#txtTNScreen").show()
            $("#txtVipNetCrypto").show()            
         }


         function VipNetOff() {             
            $("#txtAntivirus").hide()
            $("#txtUnauthAccessProtect").hide()
            $("#txtElectronicLock").hide()
            $("#txtTNScreen").hide()
            $("#txtVipNetCrypto").hide()            

            $("#<%=ddlAntivirus.ClientID %>").show()
            $("#<%=ddlUnauthAccessProtect.ClientID %>").show()
            $("#<%=ddlElectronicLock.ClientID %>").show()
            $("#<%=ddlTNScreen.ClientID%>").show()
            $("#<%=ddlVipNetCrypto.ClientID%>").show()
        }

        function ClearDisabledOptions(select) {               
            var i
            for (i = 0; i < select.options.length;i++ ) {
                select.options[i].disabled = false;
            }
        }


        function SelectNewIfDisabled(select) {
            if (!select.options[select.selectedIndex].disabled)
             return;

            var i
            for (i = 0; i < select.options.length; i++) 
            {
                if (select.options[i].disabled == false) {
                    select.selectedIndex = i;
                    return;
                }
            }
            
        }


        function OnSystemsChange() {
            var result = true
            var systems = document.getElementById('<%=ddlOperationSystem.ClientID %>')
            var antiviruses = document.getElementById('<%=ddlAntivirus.ClientID%>')
            var ua_protect = document.getElementById('<%=ddlUnauthAccessProtect.ClientID%>')
            var tnscreens = document.getElementById('<%=ddlTNScreen.ClientID%>')
            var vipnet = document.getElementById('<%=ddlVipNetCrypto.ClientID%>')

            ClearDisabledOptions(antiviruses)
            ClearDisabledOptions(ua_protect)
            ClearDisabledOptions(tnscreens)


          switch (systems.options[systems.selectedIndex].text)
          {
              case "Microsoft Windows XP 32 разрядная":
              case "Microsoft Windows XP 64 разрядная":
                  ua_protect.options[0].disabled = true;
                  ua_protect.options[4].disabled = true;
                  if (systems.options[systems.selectedIndex].text == "Microsoft Windows XP 32 разрядная")
                      ua_protect.options[2].disabled = true;
                  else
                  {
                      ua_protect.options[1].disabled = true;
                      //vipnet.options[0].disabled = true;
                   }

                  antiviruses.options[1].disabled = true;
                  antiviruses.options[3].disabled = true;
                  antiviruses.options[4].disabled = true;
                  antiviruses.options[5].disabled = true;
                  antiviruses.options[9].disabled = true;

                  tnscreens.options[2].disabled = true;
                  tnscreens.options[3].disabled = true;
                  tnscreens.options[4].disabled = true;
                  break;
            case "Microsoft Windows XP 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Windows XP 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[0].disabled = true;
                ua_protect.options[4].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows XP 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                {
                    ua_protect.options[1].disabled = true;
                     //vipnet.options[0].disabled = true;
                }

                antiviruses.options[1].disabled = true;
                antiviruses.options[3].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                antiviruses.options[9].disabled = true;

                tnscreens.options[2].disabled = true;
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
               break;
            case "Microsoft Windows XP (SP2) 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Windows XP (SP2) 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[0].disabled = true;
                ua_protect.options[4].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows XP (SP2) 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                {
                    ua_protect.options[1].disabled = true;
                    //vipnet.options[0].disabled = true;
                }
                antiviruses.options[1].disabled = true;                
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                antiviruses.options[9].disabled = true;

                tnscreens.options[2].disabled = true;
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Vista 32 разрядная":
            case "Microsoft Vista 64 разрядная":
                ua_protect.options[4].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Vista 32 разрядная")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[3].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                antiviruses.options[9].disabled = true;


                tnscreens.options[2].disabled = true;
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Vista 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Vista 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[0].disabled = true;
                ua_protect.options[3].disabled = true;
                ua_protect.options[4].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Vista 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[3].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                antiviruses.options[9].disabled = true;


                tnscreens.options[2].disabled = true;
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Vista (SP1) 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Vista (SP1) 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[0].disabled = true;
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Vista (SP1) 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[3].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Vista (SP2) 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Vista (SP2) 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[3].disabled = true;
                ua_protect.options[4].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Vista (SP2) 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;                
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                antiviruses.options[9].disabled = true;

                tnscreens.options[2].disabled = true;
                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Windows 7 32 разрядная":
            case "Microsoft Windows 7 64 разрядная":
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 7 32 разрядная")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                break;
            case "Microsoft Windows 7 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Windows 7 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 7 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                break;
            case "Microsoft Windows 7 (SP1) 32 разрядная (сертифицированная ФСТЭК)":
            case "Microsoft Windows 7 (SP1) 64 разрядная (сертифицированная ФСТЭК)":
                ua_protect.options[0].disabled = true;
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 7 (SP1) 32 разрядная (сертифицированная ФСТЭК)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[1].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[4].disabled = true;
                antiviruses.options[5].disabled = true;
                break;
            case "Microsoft Windows 2003 Server SE 32 разрядная (SP2)":
            case "Microsoft Windows 2003 Server SE 64 разрядная (SP2)":
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 2003 Server SE 32 разрядная (SP2)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;

                antiviruses.options[0].disabled = true;                
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;

                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Windows 2003 Server SE R2 32 разрядная (SP2)":
            case "Microsoft Windows 2003 Server SE R2 64 разрядная (SP2)":
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 2003 Server SE R2 32 разрядная (SP2)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;

                antiviruses.options[0].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;

                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Windows 2003 Server EE 32 разрядная (SP2)":
            case "Microsoft Windows 2003 Server EE 64 разрядная (SP2)":
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows 2003 Server EE 32 разрядная (SP2)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;

                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Windows 2003 Server EE R2 32 разрядная (SP2)":
                ua_protect.options[2].disabled = true;

                antiviruses.options[0].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;

                tnscreens.options[3].disabled = true;
                tnscreens.options[4].disabled = true;
                break;
            case "Microsoft Windows Server 2008 Standard Edition 32 разрядная (SP2)":
            case "Microsoft Windows Server 2008 Standard Edition 64 разрядная (SP2)":
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows Server 2008 Standard Edition 32 разрядная (SP2)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;
                break;
            case "Microsoft Windows Server 2008 Enterprise Edition 32 разрядная (SP2)":
            case "Microsoft Windows Server 2008 Enterprise Edition 64 разрядная (SP2)":
                ua_protect.options[3].disabled = true;
                if (systems.options[systems.selectedIndex].text == "Microsoft Windows Server 2008 Enterprise Edition 32 разрядная (SP2)")
                    ua_protect.options[2].disabled = true;
                else
                    ua_protect.options[1].disabled = true;


                antiviruses.options[0].disabled = true;
                antiviruses.options[2].disabled = true;
                antiviruses.options[3].disabled = true;
                break;
            case "Linux в составе VipNet Terminal":
                result = false;
                break;
        }

        SelectNewIfDisabled(antiviruses)
        SelectNewIfDisabled(ua_protect)
        SelectNewIfDisabled(tnscreens)

        OnUAProtectChange() 

        return result;
       }


       function OnUAProtectChange() {
           var ua_protect = document.getElementById('<%=ddlUnauthAccessProtect.ClientID%>')
           var el_locks = document.getElementById('<%=ddlElectronicLock.ClientID%>')

           ClearDisabledOptions(el_locks)

           switch (ua_protect.options[ua_protect.selectedIndex].text) {
            case "Secret Net 6":
            case "Security studio endpoint protection":
              el_locks.options[2].disabled = true;
              break;

            case "Аккорд-Win32":
            case "Аккорд-Win64":
            case "Аккорд-NT/2000 v.3.0":
              el_locks.options[0].disabled = true;
              el_locks.options[1].disabled = true;
              break;            
           }

           SelectNewIfDisabled(el_locks);
      }
    </script>

    <form id="form1" runat="server">
    <p class="l8" style="padding-bottom: 10px">
        &nbsp;&nbsp;Ввиду возможных изменений в составе сертифицированных средств защиты и данных об их совместимости ФГБУ «ФЦТ» не несет ответственности в случае несовместимости указанных технических решений, а также в случае отсутствия у них действующих сертификатов. По вопросам выбора, использования и совместимости средств защиты информации рекомендуем обратиться к организациям, оказывающими услуги по технической защите информации и имеющим соответствующие лицензии ФСТЭК и ФСБ России.
        В случае, если используемое вами техническое решение отсутствует в списке, необходимо заполнить <a href="http://priem.edu.ru/Document.aspx?id=292192004863">заявку</a> вручную и прислать на адрес <a href="mailto:connect@rustest.ru">connect@rustest.ru</a>
    </p>
    <br />
				<h1>Шаг 2 из 2</h1><br/>  
				<div class="statement edit">               
					<p class="back" style="margin-bottom:20px;">
						<a href="AppStep1.aspx"><span class="un">Вернуться</span></a>
					</p>
				</div>
				<h2>ФОРМА №2. СХЕМА ПОДКЛЮЧЕНИЯ</h2>
				<div class="statement_table">
					<table width="100%">						
						<tr>
							<th>
								Операционная система
							</th>
							<td width="1">
                            <asp:DropDownList runat="server" ID="ddlOperationSystem" CssClass="sel long" DataValueField="Id" 
                                DataTextField="Name" AppendDataBoundItems="true">                                
                            </asp:DropDownList>
							</td>
						</tr>																								
						<tr>
							<th>
								Тип IP адреса
							</th>
							<td width="1">
                                <asp:DropDownList ID="ddlIPType" runat="server" CssClass="sel short">
                                    <asp:ListItem Text="Динамический" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Статический" Value="1"></asp:ListItem>
                                </asp:DropDownList>
							</td>
						</tr>
						<tr id = "trDynamicText">
							<th>
								IP адрес</th>

							<td width="1">
                               <asp:label  runat="server" ID="labDynamicAddressRequest" Text="Используется динамический IP-адрес, просим выделить виртуальный адрес ViPNet"/>                                                              
							</td>						
                         </tr>	
                        <tbody  id="bdStaticIPs">
						<tr id = "trIP1" runat="server">
							<th>
                            	IP адрес для АРМ №1<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP1" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP1" runat="server" ControlToValidate="IP1" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №1" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP1" runat="server" ControlToValidate="IP1"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №1" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />

                            </td>
                          </tr>
						<tr id = "trIP2" runat="server">
							<th>
                            	IP адрес для АРМ №2<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP2" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP2" runat="server" ControlToValidate="IP2" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №2" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP2" runat="server" ControlToValidate="IP2"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №2" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />

                            </td>
                          </tr> 
						<tr id = "trIP3" runat="server">
							<th>
                            	IP адрес для АРМ №3<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP3" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP3" runat="server" ControlToValidate="IP3" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №3" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP3" runat="server" ControlToValidate="IP3"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №3" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
						<tr id = "trIP4" runat="server">
							<th>
                            	IP адрес для АРМ №4<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP4" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP4" runat="server" ControlToValidate="IP4" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №4" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP4" runat="server" ControlToValidate="IP4"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №4" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
						<tr id = "trIP5" runat="server">
							<th>
                            	IP адрес для АРМ №5<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP5" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP5" runat="server" ControlToValidate="IP5" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №5" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP5" runat="server" ControlToValidate="IP5"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №5" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />

                            </td>
                          </tr> 
						<tr id = "trIP6" runat="server">
							<th>
                            	IP адрес для АРМ №6<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                            </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP6" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP6" runat="server" ControlToValidate="IP6" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №6" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP6" runat="server" ControlToValidate="IP6"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №6" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
						<tr id = "trIP7" runat="server">
							<th>
                            	IP адрес для АРМ №7<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP7" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP7" runat="server" ControlToValidate="IP7" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №7" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP7" runat="server" ControlToValidate="IP7"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №7" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
						<tr id = "trIP8" runat="server">
							<th>
                            	IP адрес для АРМ №8<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                            </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP8" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP8" runat="server" ControlToValidate="IP8" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №8" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP8" runat="server" ControlToValidate="IP8"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №8" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
						<tr id = "trIP9" runat="server">
							<th>
                            	IP адрес для АРМ №9<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
                             </th>
							<td width="1" >                        
                               <asp:TextBox ID="IP9" runat="server" MaxLength="15" style="width:240px;" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIP9" runat="server" ControlToValidate="IP9" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №9" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidatorIP9" runat="server" ControlToValidate="IP9"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес для АРМ №9" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />
                            </td>
                          </tr> 
                         </tbody> 					
						<tr>
							<th>
								Учётная запись в ФИС ЕГЭ и Приема<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>
							<td width="1">
								<asp:textbox ID="FISLogin" runat="server" />								
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FISLogin" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Учётная запись в ФИС ЕГЭ и Приема" обязательно для заполнения' />								                                                                
							</td>
						</tr>
						<tr>
							<th>
								Антивирусная защита
							</th>
							<td width="1">
                                <asp:DropDownList runat="server" ID="ddlAntivirus" CssClass="sel long" DataValueField="Id"
                                    DataTextField="Name" AppendDataBoundItems="true">                                
                                </asp:DropDownList>                              
                                <input type="text" id="txtAntivirus" value="Не требуется, так как используется VipNet Terminal"  disabled="disabled" /> 
							</td>                            
						</tr>
						<tr>
							<th>
								Защита информации от НСД
							</th>
							<td width="1" id="Protect_NSD">
                                <asp:DropDownList runat="server" ID="ddlUnauthAccessProtect" CssClass="sel long" DataValueField="Id"
                                    DataTextField="Name" AppendDataBoundItems="true">                                
                                </asp:DropDownList>                                
                                <input type="text" id="txtUnauthAccessProtect" value="Не требуется, так как используется VipNet Terminal" disabled="disabled" />
							</td>
						</tr>
						<tr>
							<th>
								Электронный замок
							</th>
							<td width="1">
                                <asp:DropDownList runat="server" ID="ddlElectronicLock" CssClass="sel long" DataValueField="Id"
                                    DataTextField="Name" AppendDataBoundItems="true">                                
                                </asp:DropDownList>
                                <input type="text" id="txtElectronicLock" value="Не требуется, так как используется VipNet Terminal" disabled="disabled" />
							</td>
						</tr>						<tr>
							<th>
								Наименование межсетевого экрана
							</th>
							<td width="1">
                                <asp:DropDownList runat="server" ID="ddlTNScreen" CssClass="sel long" DataValueField="Id"
                                    DataTextField="Name" AppendDataBoundItems="true">                                
                                </asp:DropDownList>                                
                                <input type="text" id="txtTNScreen" value="Используется межсетевой экран в составе  VipNet Terminal" disabled="disabled" />
							</td>
						</tr>
						<tr id="trTNScreenIP" runat="server">
							<th>
								IP адрес  межсетевого экрана<span class="required" style="color:rgb(215, 5, 5)">(*)</span>
							</th>
							<td width="1">
                                <asp:textbox ID="IP4TNS" runat="server" MaxLength="15" style="width:240px;" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTNSIP" runat="server" ControlToValidate="IP4TNS" 
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес  межсетевого экрана" обязательно для заполнения' />								                                                                
                                  <asp:RegularExpressionValidator ID="regValTNSIP" runat="server" ControlToValidate="IP4TNS"
                                    EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "IP адрес  межсетевого экрана" заполнено неверно' 
                                    ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" />

							</td>
						</tr>
						<tr>
							<th>
								Системы криптографической зашиты информации
							</th>
							<td width="1">
                                <asp:DropDownList runat="server" ID="ddlVipNetCrypto" CssClass="sel long" DataValueField="Id"
                                    DataTextField="Name" AppendDataBoundItems="true">                                
                                </asp:DropDownList>                                
                                <input type="text" id="txtVipNetCrypto" value="Используются СКЗИ в составе VipNet Terminal" disabled="disabled" />
							</td>
						</tr>
						<tr>
							<td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
								<asp:Button runat="server" ID="btnSave" Text="Далее >>" CssClass="bt" OnClick="ValidateStep2" Width="100px" />
							</td>
						</tr>                        
					</table>
                    <asp:ValidationSummary CssClass="error_block" runat="server" ID="vsErrors" DisplayMode="BulletList"  EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />                    
				</div>
    </form>
</asp:Content>