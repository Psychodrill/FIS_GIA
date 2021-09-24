<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckByPassport.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.CheckByPassport"
    MasterPageFile="~/Common/Templates/HashedCertificates.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

    <script type="text/javascript" src="/Common/Scripts/sha1.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form id="Form1" runat="server" defaultbutton="btnCheck">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div id="DValidationResult" style="display: none; color: Red;">
    </div>
    <div id="DInputs">
        <table style="width: 620px;">
            <tr>
                <td colspan="2">
                    <div class="notice" id="CheckTitleNotice">
                        <div class="top">
                            <div class="l">
                            </div>
                            <div class="r">
                            </div>
                            <div class="m">
                            </div>
                        </div>
                        <div class="cont">
                            <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                                x<span></span></div>
                            <div class="txt-in">
                                <p>
                                    Тестирование работы проверки обезличенных свидетельств</p>
                                <%--<p>Фамилию, Имя и Отчество можно вводить в произвольном регистре символов, это 
                    не влияет на результаты поиска.</p>
                
                <p>Поиск и проверка свидетельств осуществляются по строгому соответствию параметров запроса «Регистрационный номер свидетельства», «Фамилия», «Имя», «Отчество» параметрам, хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;.
                   Поля «Регистрационный номер свидетельства» и «Фамилия» обязательны для заполнения.</p>
               --%>
                            </div>
                        </div>
                        <div class="bottom">
                            <div class="l">
                            </div>
                            <div class="r">
                            </div>
                            <div class="m">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="form-l">
                        <input id="TBPassportSeria" value="" class="txt" />
                        <input id="TBPassportSeriaHash" name="TBPassportSeriaHash" value="" class="txt" style="display: none;" />
                        <input id="cPassportSeria" value="Серия" class="txt h" style="display: none" />
                        <input id="TBPassportNumber" value="" class="txt" />
                        <input id="TBPassportNumberHash" name="TBPassportNumberHash" value="" class="txt" style="display: none" />
                        <input id="cPassportNumber" value="Номер" class="txt h" style="display: none" />
                        <input id="TBLastName" value="" class="txt" />
                        <input id="TBLastNameHash" name="TBLastNameHash" value="" style="display: none;" />
                        <input id="cLastName" value="Фамилия" class="txt h" style="display: none" />
                        <input id="TBFirstName" value="" class="txt" />
                        <input id="TBFirstNameHash" name="TBFirstNameHash" value="" style="display: none;" />
                        <input id="cFirstName" value="Имя" class="txt h" style="display: none" />
                        <input id="TBPatronymicName" value="" class="txt" />
                        <input id="TBPatronymicNameHash" name="TBPatronymicNameHash" value="" style="display: none;" />
                        <input id="cPatronymicName" value="Отчество" class="txt h" style="display: none" />

                        <script type="text/javascript">
                    IntiInputWithDefaultValue("TBPassportSeria", "cPassportSeria");                        
                    IntiInputWithDefaultValue("TBPassportNumber", "cPassportNumber");
                    IntiInputWithDefaultValue("TBLastName", "cLastName");
                    IntiInputWithDefaultValue("TBFirstName", "cFirstName");
                    IntiInputWithDefaultValue("TBPatronymicName", "cPatronymicName");
                        </script>

                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="t-line">
                    <asp:Button runat="server" ID="btnReset" Text="Очистить" CssClass="bt" />
                    <asp:Button runat="server" ID="btnCheck" Text="Проверить" CssClass="bt" OnClientClick="ProcessPage();"
                        OnClick="btnCheck_Click" />
                </td>
            </tr>
        </table>

        <script type="text/javascript">
        InitNotice();
        </script>
    </div>
    <div id="DOut" style="display: none;">
        <table class="form" style="width: 100%">
            <tr>
                <td style="width: 20%;">
                    Серия
                </td>
                <td>
                    <span id="SPassportSeriaOut"></span>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Номер
                </td>
                <td>
                    <span id="SPassportNumberOut"></span>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    Фамилия
                </td>
                <td>
                    <span id="SLastNameOut"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Имя
                </td>
                <td>
                    <span id="SFirstNameOut"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Отчество
                </td>
                <td>
                    <span id="SPatronymicNameOut"></span>
                </td>
            </tr>
        </table>
        <input type="hidden" id="HPageIsValid" name="HPageIsValid" />
    </div>

    <script language="javascript" type="text/javascript">
     
    function ComputeHash(data)
    {
       return  b64_sha1(data);
    }
    
    function ProcessPage()
    {
        if(ValidatePage())
        {
            document.getElementById('HPageIsValid').value='True';
            ShowResults();
        }
    }
    
    function ValidatePassportNumber()
    {
        var number=document.getElementById('TBPassportNumber').value;
        if(number.length!=6)
        {
            return false;
        }
        for (var Index=0;Index<number.length;Index++)
        {
            if(!IsDigit(number.charAt(Index)))
            {
                return false;
            }
        }
        return true;
    }
    
    function ValidatePassportSeria()
    {
        var number=document.getElementById('TBPassportSeria').value;
        if(number.length!=4)
        {
            return false;
        }
        for (var Index=0;Index<number.length;Index++)
        {
            if(!IsDigit(number.charAt(Index)))
            {
                return false;
            }
        }
        return true;
    }
    
    function IsDigit(ch)
    {
        return ((ch=='0')||(ch=='1')||(ch=='2')||(ch=='3')||(ch=='4')||(ch=='5')||(ch=='6')||(ch=='7')||(ch=='8')||(ch=='9'))
    }
    
    function ValidatePage()
    {
        document.getElementById('DValidationResult').innerHTML = '';
        document.getElementById('DValidationResult').style['display']='none';
        var Errors='';
        if (document.getElementById('TBPassportSeria').value=='')
        {
            Errors +='<li>Не заполнено поле "Серия паспорта"</li>';
        }
        else if(!ValidatePassportSeria())
        {
            Errors +='<li>Поле "Серия паспорта" должно быть в формате XXXX</li>';
        }
        if (document.getElementById('TBPassportNumber').value=='')
        {
            Errors +='<li>Не заполнено поле "Номер паспорта"</li>';
        }
        else if(!ValidatePassportNumber())
        {
            Errors +='<li>Поле "Номер паспорта" должно быть в формате XXXXXX</li>';
        }
        if (document.getElementById('TBLastName').value=='')
        {
            Errors +='<li>Не заполнено поле "Фамилия"</li>';
        }
        if (Errors != '')
        {   
            document.getElementById('DValidationResult').innerHTML +='<p>Произошли следующие ошибки:</p><ul>'+Errors+'</ul>';
            document.getElementById('DValidationResult').style['display']='';
            window.scrollTo(0,0);
            return false;
        }
        return true;
    }
  
    function ShowResults()
    {
        var PassportNumber=document.getElementById('TBPassportNumber').value;
        document.getElementById('TBPassportNumberHash').value=ComputeHash(PassportNumber);
        
        var PassportSeria=document.getElementById('TBPassportSeria').value;
        document.getElementById('TBPassportSeriaHash').value=ComputeHash(PassportSeria);
        
        var LastName=document.getElementById('TBLastName').value;
        document.getElementById('TBLastNameHash').value=ComputeHash(LastName.toUpperCase());
        
        var FirstName=document.getElementById('TBFirstName').value;
        if(FirstName=='')
        {
            FirstName='-';
            document.getElementById('TBFirstNameHash').value='';
        }
        else 
        {
            document.getElementById('TBFirstNameHash').value=ComputeHash(FirstName.toUpperCase());
        }
        
        var PatronymicName=document.getElementById('TBPatronymicName').value;
        if (PatronymicName=='')
        {
            PatronymicName='-';
            document.getElementById('TBPatronymicNameHash').value='';
        }
        else 
        {
            document.getElementById('TBPatronymicNameHash').value=ComputeHash(PatronymicName.toUpperCase());
        }
            
        
        document.getElementById('SPassportNumberOut').innerHTML=PassportNumber;
        document.getElementById('SPassportSeriaOut').innerHTML=PassportSeria;
        document.getElementById('SLastNameOut').innerHTML=LastName;
        document.getElementById('SFirstNameOut').innerHTML=FirstName;
        document.getElementById('SPatronymicNameOut').innerHTML=PatronymicName;
        document.getElementById('DInputs').style['display']='none';
        document.getElementById('DOut').style['display']='';
        
         document.getElementById('<%=DWait.ClientID %>').style['display']='';
    }
    </script>

    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div id="DWait" runat="server" style="display: none;text-align:center;">Осуществляется обработка запроса<br />
            <img src="/Common/Images/spinner.gif" alt="Осуществляется обработка запроса" /></div>
            <asp:Panel runat="server" ID="pNotExist" Visible="false">
                <div class="notice" id="CheckResultNoticeAbsentee">
                    <div class="top">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                    <div class="cont">
                        <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                            x<span></span></div>
                        <div class="txt-in">
                            <p>
                                За дополнительной информацией о причине отсутствия свидетельства обращайтесь в Региональный
                                Центр Обработки Информации, находящийся на территории региона, в котором было выдано
                                данное свидетельство.</p>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                </div>
                <p align="center">
                    <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                    Свидетельство с заданными параметрами не найдено.</p>

                <script type="text/javascript">
                    InitNotice();
                </script>

            </asp:Panel>
           <asp:DataGrid runat="server" id="DGSearch"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th"
        >
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="15%" CssClass="left-th" />
            <HeaderTemplate>
                <div><nobr>Свидетельство</nobr></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsExist")) ?
                        String.Format("<span{2}><nobr>{0}</nobr> {1}</span>",
                                                                    String.Format("<a href=\"/Certificates/CommonNationalCertificates/HashedCheck/ResultByNumberAndLastName.aspx?Number={0}&LastName={1}\">{0}</a>", Eval("CertificateNumber"), Eval("LastName")),
                            (Convert.ToBoolean(Eval("IsDeny")) ? 
                                "<span style=\"color:Red\">(аннулировано)</span>":
                                String.Empty),  
                            (Convert.ToBoolean(Eval("IsDeny")) ?
                                String.Format(" title='Свидетельство №{0} аннулировано по следующей причине: {1}'", Eval("CertificateNumber"), Convert.ToString(Eval("DenyComment"))):
                                String.Empty)            
                        ):
                        "<span style=\"color:Red\" title='Свидетельство не найдено'>Не&nbsp;найдено</span>" %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>ТН</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("TypographicNumber") %>
            </ItemTemplate>
            </asp:TemplateColumn>
        
            <asp:TemplateColumn >
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Регион</div>
            </HeaderTemplate>
            <ItemTemplate>
               <%# Convert.ToString(Eval("RegionName")) %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn >
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Год</div>
            </HeaderTemplate>
            <ItemTemplate>
               <%# Convert.ToString(Eval("Year")) %>
            </ItemTemplate>
            </asp:TemplateColumn>            

            <asp:TemplateColumn >
            <HeaderStyle Width="10%" CssClass="right-th" />
            <HeaderTemplate>
                <div>Статус</div>
            </HeaderTemplate>
            <ItemTemplate>
               <%# Convert.ToString(Eval("Status")) %>
            </ItemTemplate>
            </asp:TemplateColumn>     
            
        </Columns>
    </asp:DataGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</asp:Content>

<script runat="server">

    private static string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.StartsWith("!") || value.StartsWith("Истек"))
            return String.Format("<font color=\"red\">{0}</font>", value);
        return value;
    }

    private static string HighlightValue(object valueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;
        return String.Format("<font color=\"red\">{0}</font>", value);
    }

    private static string HighlightValues(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        string checkValue = Convert.ToString(checkValueObj);
        if ((!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj)) ||
                string.IsNullOrEmpty(checkValue))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        checkValue = String.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
        value = String.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

        return String.Format("<span title=\"Ошибка: заявленое {0} (в базе {1})\"><span style=\"color:Red\">{0}</span> ({1})</span>",
            checkValue, value);
    }

    private static string FormLinkToNewCertificate(string comment, string newCertificate, string lastName)
    {
        if (string.IsNullOrEmpty(newCertificate))
            return comment;
        return string.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={1}&LastName={2}\">{0}</a>", comment, newCertificate, lastName);
    }    
    
</script>

