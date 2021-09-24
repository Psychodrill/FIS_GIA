<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckTypeNumExportCsv.aspx.cs" 
    Inherits="Esrp.Web.Certificates.CommonNationalCertificates.BatchCheckTypeNumExportCsv" 
%><%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Parameters" Assembly="Esrp.Web.UI"
%><%@ Import Namespace="Esrp.Utility" 
%><% 
    
      // Установлю хидеры страницы
      ResponseWriter.PrepareHeaders("BatchCheckResult.csv", "application/text", 
          Encoding.GetEncoding(1251));                                                                                      
                                                                                      
%>
<%--<asp:Repeater runat="server"
    DataSourceID="dsResultsList"
     >--%>
     <asp:Repeater runat="server" id="Repeater1">
<HeaderTemplate>Номер свидетельства%Типографский номер%Фамилия%Имя%Отчество%Серия паспорта%Номер паспорта%Регион%Год%Статус%Русский язык%Апелляция по русскому языку%Математика%Апелляция по математике%Физика%Апелляция по физике%Химия%Апелляция по химии%Биология%Апелляция по биологии%История России%Апелляция по истории России%География%Апелляция по географии%Английский язык%Апелляция по английскому языку%Немецкий язык%Апелляция по немецкому языку%Французский язык%Апелляция по французскому языку%Обществознание%Апелляция по обществознанию%Литература%Апелляция по литературе%Испанский язык%Апелляция по испанскому языку%Информатика%Апелляция по информатике%Комментарий<%= "\r\n" %></HeaderTemplate>
<ItemTemplate><%# 
    string.Format("{0}%{1}%{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{0}%{11}%{12}%{13}%{14}%{15}%{16}%{17}%{18}%{19}%{20}%{21}%{22}%{23}%{24}%{25}%{26}%{27}%{28}%{29}%{30}%{31}%{32}%{33}%{34}%{35}%{36}%{37}%{38}",
  	   "",
       Eval("Типографский номер"),
       Eval("Фамилия"),
       Eval("Имя"),
       Eval("Отчество"),
                  "",
                  "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
                         "",
       "",
       Eval("Комментарий")
    )
    %><%= "\r\n" %></ItemTemplate>
</asp:Repeater><%= "\r\n" %>
<script runat="server">
    public string HighlightValue(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        string checkValue = Convert.ToString(checkValueObj);
        checkValue = String.IsNullOrEmpty(checkValue) ? string.Empty : checkValue;
        value = String.IsNullOrEmpty(value) ? string.Empty : value;

        return String.Format("Ошибка {0} ({1})",
            checkValue, value);
    }
</script>
