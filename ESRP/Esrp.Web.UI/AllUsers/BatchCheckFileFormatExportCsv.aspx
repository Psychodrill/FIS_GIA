<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckFileFormatExportCsv.aspx.cs" 
    Inherits="Esrp.Web.Certificates.CommonNationalCertificates.BatchCheckFileFormatExportCsv" 
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
     <asp:Repeater runat="server" id="Repeater1"
    
     >
<HeaderTemplate>Номер свидетельства%Типографский номер%Фамилия%Имя%Отчество%Серия паспорта%Номер паспорта%Регион%Год%Статус%Русский язык%Апелляция по русскому языку%Математика%Апелляция по математике%Физика%Апелляция по физике%Химия%Апелляция по химии%Биология%Апелляция по биологии%История России%Апелляция по истории России%География%Апелляция по географии%Английский язык%Апелляция по английскому языку%Немецкий язык%Апелляция по немецкому языку%Французский язык%Апелляция по французскому языку%Обществознание%Апелляция по обществознанию%Литература%Апелляция по литературе%Испанский язык%Апелляция по испанскому языку%Информатика%Апелляция по информатике%Комментарий<%= "\r\n" %></HeaderTemplate>
<ItemTemplate><%# 
    string.Format("{0}%{1}%{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{0}%{11}%{12}%{13}%{14}%{15}%{16}%{17}%{18}%{19}%{20}%{21}%{22}%{23}%{24}%{25}%{26}%{27}%{28}%{29}%{30}%{31}%{32}%{33}%{34}%{35}%{36}%{37}%{38}",
  	   Eval("Номер свидетельства"),
       Eval("Типографский номер"),
       Eval("Фамилия"),
       Eval("Имя"),
       Eval("Отчество"),
      "",
       "",
       "",
       "",
       "",
       Eval("Русский язык"),
       Eval("Апелляция по русскому языку"),         
       Eval("Математика"),
       Eval("Апелляция по математике"),
       Eval("Физика"),
       Eval("Апелляция по физике"),
       Eval("Химия"),
       Eval("Апелляция по химии"),
       Eval("Биология"),
       Eval("Апелляция по биологии"), 
       Eval("История России"),
       Eval("Апелляция по истории России"),
       Eval("География"),
       Eval("Апелляция по географии"),
       Eval("Английский язык"),
       Eval("Апелляция по английскому языку"),
       Eval("Немецкий язык"),
       Eval("Апелляция по немецкому языку"),
       Eval("Французский язык"),
       Eval("Апелляция по французскому языку"), 
       Eval("Обществознание"),
       Eval("Апелляция по обществознанию"),
       Eval("Литература"),
       Eval("Апелляция по литературе"),
       Eval("Испанский язык"),
       Eval("Апелляция по испанскому языку"),
       Eval("Информатика"),
       Eval("Апелляция по информатике"),
       Eval("Комментарий")
    )
    %><%= "\r\n" %></ItemTemplate>
</asp:Repeater><%= "\r\n" %><asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateCheck"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <esrp:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters></asp:SqlDataSource><script runat="server">
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
