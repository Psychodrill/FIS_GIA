<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/Templates/Main.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphContent">
            <!-- content in -->
            <div class="content-in">
                <p style="font-weight: bold;">
                    Подсистема ФИС ГИА и Приема &laquo;Результаты ЕГЭ&raquo;
                    предназначена для хранения и проверки информации о всех выданных 
                    свидетельствах, о результатах единого государственного экзамена (ЕГЭ) с 2011 года.<br>
                </p>
                <p>
                    Информация о результатах ЕГЭ текущего года начнет поступать в Федеральную базу свидетельств по мере их формирования в регионах, ориентировочно с 25 июня.</b><br>
                </p>
                <p>
                    <span style='font-weight: bolder'>ВНИМАНИЮ ЧАСТНЫХ ЛИЦ!</span><br />
                    Служба поддержки ФИС ГИА и Приема <b>не предоставляет частным лицам информацию</b> о результатах
                    сдачи ЕГЭ,<br />
                    обращайтесь на <a href="http://www1.ege.edu.ru/ege-in-rf">горячую линию
                        по вопросам ЕГЭ в вашем регионе</a>.</p>
                   <span style='font-weight: bolder'>ВНИМАНИЕ!</span> Промышленная версия ФИС ГИА и приема доступна по адресу: <b><a href="http://10.0.3.1:8080">http://10.0.3.1:8080</a></b></p>
                </p>
                <div class="notice" id="urgentMessage">
                    <div class="top">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                    <div class="cont">
                        <div class="txt-in">
                            <p>

                                Перед прохождением процедуры регистрации рекомендуется ознакомиться со следующими
                                нормативными документами:
                                <ul>
            <li><a href="./Docs/Registration_Letter.png">Письмо Рособрнадзора о необходимости регистрации в ФБС</a></li>
                  <li><a href="./Docs/UserGuide.doc">Инструкция по работе пользователей с Федеральной базой свидетельств о результатах единого государственного экзамена в 2011 году</a></li>
            <li><a href="./Docs/Prikaz133.pdf">Порядок формирования и ведения федеральный баз данных и баз данных субъектов Российской Федерации об участниках ЕГЭ и о результатах ЕГЭ, обеспечения их взаимодействия и доступа</a></li>
                    <li><a href="./Docs/ExamsList2011.pdf">Перечень вступительных испытаний в ВУЗы-2011</a></li>
                                    <li><a href="./Docs/ApprVuz2011.doc">Порядок приема в ВУЗы</a></li>
            <li><a href="./Docs/ApprSsuz2011.doc">Порядок приема в ССУЗы</a></li>
                                    <li><a href="./Docs/EgeCalendar2011.pdf">Расписание ЕГЭ-2011</a></li>
                                </ul>
                            </p>
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
            </div>
            <!-- /content in -->
</asp:Content>
