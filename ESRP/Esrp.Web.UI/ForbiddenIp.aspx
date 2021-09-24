<%@ Page Language="C#" AutoEventWireup="true" 
    MasterPageFile="~/Common/Templates/Regular.master" %>
<%@ Import Namespace="Esrp.Core.Systems" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Вы не можете зайти в систему с этого компьютера: текущий IP-адрес не зарегистрирован в списке 
        разрешенных адресов. <br/>
        Если у вас нет постоянных IP-адресов, то доступ разрешен только через VPN соединение. 
    </p>
    <p>Подробности вы можете найти в документе <a href="/VpnDoc">Инструкция по резервному подключению к <%= GeneralSystemManager.GetSystemName(2)%></a></p> 
</asp:Content>