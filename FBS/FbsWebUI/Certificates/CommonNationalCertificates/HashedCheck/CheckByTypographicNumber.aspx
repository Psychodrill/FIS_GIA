<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckByTypographicNumber.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.CheckByTypographicNumber"
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
                        <input id="TBNumber" name="TBNumber" value="" class="txt" />
                        <input id="cNumber" value="Типографский номер" class="txt h" style="display: none" />
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
                    IntiInputWithDefaultValue("TBNumber", "cNumber");
                    IntiInputWithDefaultValue("TBLastName", "cLastName");
                    IntiInputWithDefaultValue("TBFirstName", "cFirstName");
                    IntiInputWithDefaultValue("TBPatronymicName", "cPatronymicName");
                        </script>

                    </div>
                </td>
                <td>
                    <div id="DSubjects">
                        <asp:Repeater runat="server" ID="rpSubjects" DataSourceID="dsSubjects">
                            <HeaderTemplate>
                                <table class="form-r">
                                    <tr>
                                        <th colspan="2">
                                            <div>
                                                <div>
                                                    Баллы</div>
                                            </div>
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="odd">
                                    <td class="left-td">
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="4" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr>
                                    <td class="left-td">
                                        <%#Eval("Name")%>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="4" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
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

        <asp:SqlDataSource runat="server" ID="dsSubjects" EnableCaching="true" CacheDuration="1"
            ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="dbo.GetSubject" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>
    </div>
    <div id="DOut" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <table style="width: 100%;" class="form">
                    <tr runat="server" id="RowNumber" visible="false">
                        <td style="width: 20%;" >
                            Номер свидетельства
                        </td>
                        <td>
                            <asp:Label runat="server" ID="LNumber"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="form" style="width: 100%">
        <tr>
                <td style="width: 20%;">
                    Типографский номер
                </td>
                <td>
                    <span id="STyphNumOut"></span>
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
    
    function ValidateNumber()
    {
        var number=document.getElementById('TBNumber').value;
        if(number.length!=7)
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
        if (document.getElementById('TBNumber').value=='')
        {
            Errors +='<li>Не заполнено поле "Типографский номер"</li>';
        }
        else if(!ValidateNumber())
        {
            Errors +='<li>Поле "Типографский номер" должно быть в формате XXXXXXX</li>';
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
            
            document.getElementById('STyphNumOut').innerHTML=document.getElementById('TBNumber').value;
        
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
            <table id="TResults" runat="server" style="width: 100%;" class="form" visible="false">
                <tr>
                    <td style="width: 20%;">
                        Регион:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="LRegion" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        Год выдачи свидетельства:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="LYear" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        Статус свидетельства:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="LStatus" EnableViewState="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:DataGrid runat="server" ID="DGSubjects" AutoGenerateColumns="false" EnableViewState="false"
                ShowHeader="True" GridLines="None" CssClass="table-th" Width="60%">
                <HeaderStyle CssClass="th" />
                <Columns>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="25%" CssClass="left-th" />
                        <HeaderTemplate>
                            <div>
                                Предмет</div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# HighlightValue(Eval("SubjectName"), 
                    Convert.ToBoolean(Eval("SubjectMarkCorrect")) || string.IsNullOrEmpty(Convert.ToString(Eval("CheckSubjectMark"))))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="15%" />
                        <HeaderTemplate>
                            <div>
                                Заявлено</div>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# HighlightValue(Eval("CheckSubjectMark"), Eval("SubjectMarkCorrect"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="15%" />
                        <HeaderTemplate>
                            <div>
                                В&nbsp;базе</div>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# HighlightValue(Eval("SubjectMark")) %>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderStyle Width="10%" CssClass="right-th" />
                        <HeaderTemplate>
                            <div>
                                Апелляция&nbsp;</div>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# (Convert.IsDBNull(Eval("HasAppeal")) ? string.Empty : (!Convert.IsDBNull(Eval("HasAppeal")) && Convert.ToBoolean(Eval("HasAppeal"))) ? "<span style=\"color:Red\">Да</span>" : "Нет") %>
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

