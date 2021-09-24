<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckByMarkSum.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.CheckByMarkSum" 
 MasterPageFile="~/Common/Templates/HashedCertificates.Master"%>
 <%@ Register Src="~/Controls/OrgSelector.ascx" TagName="OrgSelector" TagPrefix="uc"  %>
 
 <%@ Register Src="~/Controls/Captcha.ascx" TagName="Captcha" TagPrefix="uc"  %>
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
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click" />
                        
                    </Triggers>
                    <ContentTemplate>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <div id="DValidationResult" style="display: none; color: Red;">
    </div>

    <asp:Panel id="DInputs" runat="server">
        <div id="DWait" runat="server" style="display: none;text-align:center;">Осуществляется обработка запроса<br />
         <img src="/Common/Images/spinner.gif" alt="Осуществляется обработка запроса" /></div>
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
                                <p>Проверка свидетельств осуществляются по строгому соответствию параметров запроса Фамилия, Имя, Отчество и суммы баллов как минимум по трем и максимум четырем предметам ЕГЭ хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;. Все поля обязательны для заполнения.</p>
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
                    <uc:OrgSelector id="orgSelector" runat="server" />
                    <div class="form-l">
                        
                        
                        <input id="TBLastName" value="" class="txt" />
                        <input id="TBLastNameHash" name="TBLastNameHash" value="" style="display: none;" />
                        <input id="cLastName" value="Фамилия" class="txt h" style="display: none" />
                        <input id="TBFirstName" value="" class="txt" />
                        <input id="TBFirstNameHash" name="TBFirstNameHash" value="" style="display: none;" />
                        <input id="cFirstName" value="Имя" class="txt h" style="display: none" />
                        <input id="TBPatronymicName" value="" class="txt" />
                        <input id="TBPatronymicNameHash" name="TBPatronymicNameHash" value="" style="display: none;" />
                        <input id="cPatronymicName" value="Отчество" class="txt h" style="display: none" />
                        <input id="TBMarkSum" maxlength="5" name="TBMarkSum" value="" class="txt" />
                        <input id="cMarkSum" value="Сумма баллов" class="txt h" style="display: none" />
                        <script type="text/javascript">
                            $(function () {
                                InitWaterMarks();
                            });
                           
                        </script>

                    </div>
                </td>
                <td>
                    <div id="DSubjects">
                        <div style="height: 29px;background: url(/Common/Images/table-h.gif) 0 0 repeat #E7E7E7;padding: 0;text-align: left;">
                            <div style="background: url(/Common/Images/table-h-l.gif) 0 0 no-repeat;height: 29px;" >
                                <div style="background: url(/Common/Images/table-h-r.gif) right 0 no-repeat;padding: 6px 0 0 10px;height: 23px;">
                                    Предметы
                                </div>
                            </div>
                        </div>
                        <asp:CheckBoxList  runat="server" ID="cblSubjects" DataSourceID="dsSubjects" DataTextField="Name" DataValueField="Id"  CssClass="form-r cblist"  RepeatDirection="Vertical" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc:Captcha id="captcha" runat="server" ErrorMessage="Неправильный код подтверждения"></uc:Captcha>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="t-line">
                    <asp:Button runat="server" ID="btnReset" OnClientClick="window.location.href='CheckByMarkSum.aspx';return false;" Text="Очистить" CssClass="bt" />
                    <asp:Button runat="server" ID="btnCheck" Text="Проверить" CssClass="bt" OnClientClick="return ProcessPage();"
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
        <input type="hidden" id="HPageIsValid" name="HPageIsValid" />
       
    </asp:Panel>
    <asp:Panel id="DOut" runat="server" Visible="false">
                        <div style="height: 29px;background: url(/Common/Images/table-h.gif) 0 0 repeat #E7E7E7;padding: 0;text-align: left;">
                            <div style="background: url(/Common/Images/table-h-l.gif) 0 0 no-repeat;height: 29px;" >
                                <div style="background: url(/Common/Images/table-h-r.gif) right 0 no-repeat;padding: 6px 0 0 10px;height: 23px;">
                                    Результат проверки
                                </div>
                            </div>
                        </div>
                    <table class="form" style="width: 60%">
                    <tr>
                        <td style="width: 40%;">
                            Номер проверки
                        </td>
                        <td>
                            <asp:Label ID="checkNumber" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="">
                            Результат проверки
                        </td>
                        <td>
                            <asp:Label style="font-weight:bold" ID="checkResult" runat="server" />
                        </td>
                    </tr>
                </table>
                
                     <div style="height: 29px;background: url(/Common/Images/table-h.gif) 0 0 repeat #E7E7E7;padding: 0;text-align: left;">
                            <div style="background: url(/Common/Images/table-h-l.gif) 0 0 no-repeat;height: 29px;" >
                                <div style="background: url(/Common/Images/table-h-r.gif) right 0 no-repeat;padding: 6px 5px 0 10px;height: 23px;">
                                    Информация об участнике ЕГЭ
                                </div>
                            </div>
                        </div>
                    <table class="form" style="width: 60%">
                        <tr>
                            <td style="width: 40%;">
                                Фамилия
                            </td>
                            <td>
                                <span id="SLastNameOut" ></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Имя
                            </td>
                            <td>
                                <span id="SFirstNameOut" ></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Отчество
                            </td>
                            <td>
                                <span id="SPatronymicNameOut" ></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Сумма баллов
                            </td>
                            <td>
                                <span id="SMarkSum"></span>
                            </td>
                        </tr>
                    </table>
        <a href="javascript:void(0)" onclick="window.location.reload();return false;" class="button">Вернуться назад</a>
        <asp:Button runat="server" ID="btnAbuse" OnClientClick="$('#complaintForm').submit();return false;" Text="Сообщить о нарушении" Visible="false" />
        
    </asp:Panel>
   
    </ContentTemplate>
    </asp:UpdatePanel>
    
    <script language="javascript" type="text/javascript">

        $(function () {
            if ($("input[id$='selectedOrg']").val() == '' || $("input[id$='selectedOrg']").val() == 0) {
                $('input:not(.immutable)').attr('disabled','disabled');
            }

            if ($("#DSubjects input:checked").length == 4) {
                $("#DSubjects input:not(:checked)").attr('disabled', 'disabled');
            };
            $("#DSubjects input[type='checkbox']").each(function () {
                $(this).click(function () {
                    if ($("#DSubjects input:checked").length == 4) {
                        $("#DSubjects input:not(:checked)").attr('disabled', 'disabled');
                    }
                    else
                        $("#DSubjects input:not(:checked)").removeAttr('disabled');
                })
            });
        });
        function ComputeHash(data) {
            return b64_sha1(data);
        }

        function ProcessPage() {
            if (ValidatePage()) {
                document.getElementById('HPageIsValid').value = 'True';
                ShowResults();
                return true;
            }
            return false;
        }

        

        function IsDigit(ch) {
            return ((ch == '0') || (ch == '1') || (ch == '2') || (ch == '3') || (ch == '4') || (ch == '5') || (ch == '6') || (ch == '7') || (ch == '8') || (ch == '9'))
        }

        function ValidatePage() {
            document.getElementById('DValidationResult').innerHTML = '';
            document.getElementById('DValidationResult').style['display'] = 'none';
            var Errors = '';
            
            if (document.getElementById('TBLastName').value == '') {
                Errors += '<li>Не заполнено поле "Фамилия"</li>';
            }
            if (document.getElementById('TBFirstName').value == '') {
                Errors += '<li>Не заполнено поле "Имя"</li>';
            }
            if (document.getElementById('TBMarkSum').value == '') {
                Errors += '<li>Не заполнено поле "Сумма баллов"</li>';
            }
            else if (!/^[\d]+$/.test(document.getElementById('TBMarkSum').value)) {
                Errors += '<li> поле "Сумма балов" должно содержать действительное целое число</li>';
            }
            if (!/^[\d]+$/.test($("input[id$='selectedOrg']").val())) {
                Errors += '<li> Выберите образовательное учреждение</li>';
            }
            if ($("#DSubjects input:checked").length < 3) {
                Errors += '<li> Необходимо выбрать не менее 3 предметов</li>';
            }
            if (Errors != '') {
                document.getElementById('DValidationResult').innerHTML += '<ul>' + Errors + '</ul>';
                document.getElementById('DValidationResult').style['display'] = '';
                window.scrollTo(0, 0);
                return false;
            }
            return Page_ClientValidate() && true;
        }
        function RestoreResults() {
            if ($("#DSubjects input:checked").length == 4) {
                $("#DSubjects input:not(:checked)").attr('disabled', 'disabled');
            };
            $('#TBFirstName').val($('#hFirstName').val());

            $('#TBLastName').val($('#hLastName').val());
            $('#TBPatronymicName').val($('#hGivenName').val());
            $("#TBMarkSum").val($('#hMarkSum').val());
            $("#SLastNameOut").html($('#hLastName').val());
            $("#SFirstNameOut").html($('#hFirstName').val());
            $("#SPatronymicNameOut").html($('#hGivenName').val());
            $("#SMarkSum").html($('#hMarkSum').val());
            $("#hCheckNumber").val($("span[id$='checkNumber']").html());
            if ($("#DOut").length > 0) {
                $(".content-in h1").first().html("Результат проверки по сумме баллов");
            }

            $('#<%=DWait.ClientID %>').hide();
        }
        function ShowResults() {
            var LastName = document.getElementById('TBLastName').value;
            document.getElementById('TBLastNameHash').value = ComputeHash(LastName.toUpperCase());

            var FirstName = document.getElementById('TBFirstName').value;
            if (FirstName == '') {
                FirstName = '-';
                document.getElementById('TBFirstNameHash').value = '';
            }
            else {
                document.getElementById('TBFirstNameHash').value = ComputeHash(FirstName.toUpperCase());
            }
           
            var PatronymicName = document.getElementById('TBPatronymicName').value;
            if (PatronymicName == '') {
                PatronymicName = '-';
                document.getElementById('TBPatronymicNameHash').value = '';
            }
            else {
                document.getElementById('TBPatronymicNameHash').value = ComputeHash(PatronymicName.toUpperCase());
            }

           $('#hLastName').val(LastName);
            
            $('#hFirstName').val(FirstName);
            $('#hGivenName').val(PatronymicName);
            $("#hOrgId").val($("input[id$='selectedOrg']").val());
            $("#hMarkSum").val($("#TBMarkSum").val());
            $('#<%=DWait.ClientID %>').show();
            $('#<%=DWait.ClientID %>').next().hide();
        }
        InitWaterMarks = function () {
            IntiInputWithDefaultValue("TBMarkSum", "cMarkSum");
            IntiInputWithDefaultValue("TBLastName", "cLastName");
            IntiInputWithDefaultValue("TBFirstName", "cFirstName");
            IntiInputWithDefaultValue("TBPatronymicName", "cPatronymicName");
        };
        ResetFields = function () {
            $("#DInputs").find("input").each(function () {
                $(this).val("");
            });
            $("#DSubjects input:checked").removeAttr("checked")
            $("#DSubjects input:not(:checked)").removeAttr('disabled'); ;
            InitWaterMarks();
        };
        ErrorHandler = function (sender, args) {
            if (args.get_error() != undefined) {
                var errorMessage;
                if (args.get_response().get_statusCode() == '200') {
                    errorMessage = args.get_error().message;
                }
                else {
                    // Error occurred somewhere other than the server page.
                    errorMessage = 'An unspecified error occurred. ';
                }
                args.set_errorHandled(true);
                alert(errorMessage);
            }

        };
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RestoreResults);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitWaterMarks);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ErrorHandler);
    </script>

   
    </form>
    <form action="Conmplaint.aspx" method="post" id="complaintForm">
        <input type="hidden" name="hFirstName" id="hFirstName" />
        <input type="hidden" name="hLastName" id="hLastName" />
        <input type="hidden" name="hGivenName" id="hGivenName" />
        <input type="hidden" name="hOrgId" id="hOrgId" />
        <input type="hidden" name="hMarkSum" id="hMarkSum" />
        <input type="hidden" name="hCheckNumber" id="hCheckNumber" />
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

