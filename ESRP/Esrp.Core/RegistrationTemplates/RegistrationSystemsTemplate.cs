namespace Esrp.Core.RegistrationTemplates
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DW.RtfWriter;

    using Esrp.Core.Users;

    /// <summary>
    /// Формирование заявки на регистрацию
    /// </summary>
    public class RegistrationSystemsTemplate
    {
        /// <summary>
        /// Словать с данными о пользователе, организацие и имени ИС
        /// </summary>
        private List<OrgUser> _dictionaryOrgUser = new List<OrgUser>();

        /// <summary>
        /// Формирование заявки на регистрацию
        /// </summary>
        /// <param name="lastOrgRequest">Список пользователей, которые должны попасть в заявку</param>
        /// <param name="virPath">Путь до папки куда будет сохранен файл</param>
        /// <returns>Путь до файла</returns>
        public string GetDocument(OrgRequest lastOrgRequest, string virPath)
        {
            foreach (OrgUserBrief orgUserBrief in lastOrgRequest.LinkedUsers)
            {
                var orgUser = OrgUserDataAccessor.Get(orgUserBrief.Login);
                if (orgUser != null)
                {
                    orgUser.fullSystemNameList.AddRange(orgUserBrief.FullSystemNameList);
                    this._dictionaryOrgUser.Add(orgUser);
                }
            }

            var doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait, Lcid.TraditionalChinese);
            var par = doc.addParagraph();
            par.Alignment = Align.Right;
            par.Text = "В Федеральное государственное бюджетное учреждение ";
            par = doc.addParagraph();
            par.Alignment = Align.Right;
            par.Text = @"""Федеральный центр тестирования""";
            doc.addParagraph();
            par = doc.addParagraph();
            par.Alignment = Align.Left;
            par.Text = "Прошу зарегистрировать пользователей для работы со следующими системами:";

            int index = 1;
            
            foreach (OrgUser orgUser in this._dictionaryOrgUser)
            {
                foreach (var fullSystemName in orgUser.fullSystemNameList)
                {
                    par = doc.addParagraph();
                    par.Text = string.Format("  {0}. {1}", index, fullSystemName);
                    index++;
                }
            }

            doc.addParagraph();
            par = doc.addParagraph();
            par.Text = "Сведения для регистрации в 2012 году:";
            doc.addParagraph();
            var table = doc.addTable(13, 2);
            table.Margins[Direction.Bottom] = 20;

            /*** Добавление основной таблицы ***/
            var constCells = new List<string>
                {
                    "1. Полное наименование организации ",
                    "2. Тип организации",
                    "3. Субъект Российской Федерации, на территории которого находится организация ",
                    "4. Учредитель (для ссузов, вузов и РЦОИ)",
                    "5. Юридический адрес",
                    "6. Фамилия, Имя, Отчество руководителя организации",
                    "7. Телефон (с указанием кода города) руководителя организации",
                    "8. Факс (с указанием кода города)",
                    "9. Модель проведения приема",
                    "10. Прием по результатам ЕГЭ (для ССУЗов и ВУЗов)",
                    "11. ОГРН",
                    "12. ИНН", 
                    "13. КПП"
                };

            for (int j = 0; j < table.RowCount; j++)
            {
                table.cell(j, 0).Alignment = Align.Left;
                table.cell(j, 0).AlignmentVertical = AlignVertical.Middle;
                table.cell(j, 0).addParagraph().Text = constCells[j];
            }

            table.cell(0, 1).Alignment = Align.Left;
            table.cell(0, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(0, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.FullName;

            table.cell(1, 1).Alignment = Align.Left;
            table.cell(1, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(1, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.OrgType.Name;

            table.cell(2, 1).Alignment = Align.Left;
            table.cell(2, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(2, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.Region.Name;

            table.cell(3, 1).Alignment = Align.Left;
            table.cell(3, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(3, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.OwnerDepartment;

            table.cell(4, 1).Alignment = Align.Left;
            table.cell(4, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(4, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.LawAddress;

            table.cell(5, 1).Alignment = Align.Left;
            table.cell(5, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(5, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.DirectorFullName;

            table.cell(6, 1).Alignment = Align.Left;
            table.cell(6, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(6, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.Phone;

            table.cell(7, 1).Alignment = Align.Left;
            table.cell(7, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(7, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.Fax;

            table.cell(8, 1).Alignment = Align.Left;
            table.cell(8, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(8, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.RCModelId != 999
                                                        ? this._dictionaryOrgUser[0].RequestedOrganization.RCModelName
                                                        : this._dictionaryOrgUser[0].RequestedOrganization.RCDescription;
            table.cell(9, 1).Alignment = Align.Left;
            table.cell(9, 1).AlignmentVertical = AlignVertical.Middle;
            var receptionOnResultsCNE = this._dictionaryOrgUser[0].RequestedOrganization.ReceptionOnResultsCNE;
            if (receptionOnResultsCNE != null)
            {
                table.cell(9, 1).addParagraph().Text = receptionOnResultsCNE == 0 ? "Проводится" : "Не проводится";
            }

            table.cell(10, 1).Alignment = Align.Left;
            table.cell(10, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(10, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.OGRN;

            table.cell(11, 1).Alignment = Align.Left;
            table.cell(11, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(11, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.INN;

            table.cell(12, 1).Alignment = Align.Left;
            table.cell(12, 1).AlignmentVertical = AlignVertical.Middle;
            table.cell(12, 1).addParagraph().Text = this._dictionaryOrgUser[0].RequestedOrganization.KPP;

            table.setInnerBorder(BorderStyle.Single, 1f);
            table.setOuterBorder(BorderStyle.Single, 1f);

            /*** Добавление таблиц с информацией о пользователях ***/
            FontDescriptor tahoma = doc.createFont("Tahoma");

            foreach (OrgUser orgUser in this._dictionaryOrgUser)
            {
                table = doc.addTable(6, 2);
                table.Margins[Direction.Bottom] = 20;

                table.merge(0, 0, 1, 2);
                table.merge(5, 0, 1, 2);
                table.cell(0, 0).Alignment = Align.Left;
                table.cell(0, 0).AlignmentVertical = AlignVertical.Middle;

                table.cell(1, 0).Alignment = Align.Left;
                table.cell(1, 0).AlignmentVertical = AlignVertical.Middle;
                table.cell(1, 0).addParagraph().Text = "Фамилия, Имя, Отчество ответственного лица";

                table.cell(2, 0).Alignment = Align.Left;
                table.cell(2, 0).AlignmentVertical = AlignVertical.Middle;
                table.cell(2, 0).addParagraph().Text = "Должность ответственного лица";

                table.cell(3, 0).Alignment = Align.Left;
                table.cell(3, 0).AlignmentVertical = AlignVertical.Middle;
                table.cell(3, 0).addParagraph().Text = "Телефон (с указанием кода города) ответственного лица";

                table.cell(4, 0).Alignment = Align.Left;
                table.cell(4, 0).AlignmentVertical = AlignVertical.Middle;
                table.cell(4, 0).addParagraph().Text = "E-mail ответственного лица";

                table.cell(5, 0).Alignment = Align.Left;
                table.cell(5, 0).AlignmentVertical = AlignVertical.Middle;
                par = table.cell(5, 0).addParagraph();
                par.Text = @"Даю согласие на обработку и публикацию своих персональных данных в общедоступных источниках персональных данных в порядке, установленном Федеральным законом Российской Федерации от 27.07.2006 № 152-Ф3  _____________________________
                                           (подпись ответственного лица)";

                var fmt = par.addCharFormat();
                fmt.FontStyle.addStyle(FontStyleFlag.Italic);
                fmt.FontSize = 9;
                fmt.Font = tahoma;

                fmt = par.addCharFormat(237, 265);
                fmt.Font = tahoma;
                fmt.FontSize = 8;

                table.cell(0, 1).Alignment = Align.Left;
                table.cell(0, 1).AlignmentVertical = AlignVertical.Middle;
                par = table.cell(0, 1).addParagraph();

                par.Text = string.Join(", ",
                orgUser.fullSystemNameList.Cast<object>().Select(c => c.ToString()).ToArray());

                char[] charsToTrim = { ',' };

                par.Text = par.Text.Trim(charsToTrim);

                par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Bold);

                table.cell(1, 1).Alignment = Align.Left;
                table.cell(1, 1).AlignmentVertical = AlignVertical.Middle;
                table.cell(1, 1).addParagraph().Text = orgUser.GetFullName();

                table.cell(2, 1).Alignment = Align.Left;
                table.cell(2, 1).AlignmentVertical = AlignVertical.Middle;
                table.cell(2, 1).addParagraph().Text = orgUser.position;

                table.cell(3, 1).Alignment = Align.Left;
                table.cell(3, 1).AlignmentVertical = AlignVertical.Middle;
                table.cell(3, 1).addParagraph().Text = orgUser.phone;

                table.cell(4, 1).Alignment = Align.Left;
                table.cell(4, 1).AlignmentVertical = AlignVertical.Middle;
                table.cell(4, 1).addParagraph().Text = orgUser.email;

                table.setInnerBorder(BorderStyle.Single, 1f);
                table.setOuterBorder(BorderStyle.Single, 1f);
            }

            /*** Добавление нижнего колонтитула***/
            var footer = doc.Footer;
            par = footer.addParagraph();
            par.Alignment = Align.Left;
            par.Text = "                                                                                             ";
            var charFormat = par.addCharFormat();
            charFormat.FontSize = 12;
            charFormat.FontStyle.addStyle(FontStyleFlag.Underline);

            charFormat = par.addCharFormat(40, 45);
            charFormat.FontSize = 12;

            par = footer.addParagraph();
            par.Text = "    (Руководитель организации";

            par = footer.addParagraph();
            par.Text = "\t с указанием должности)\t\t\t\t\t\t(Подпись)";

            par = footer.addParagraph();
            par.Text = "                                                                                             ";
            charFormat = par.addCharFormat(46, 87);
            charFormat.FontSize = 12;
            charFormat.FontStyle.addStyle(FontStyleFlag.Underline);

            par = footer.addParagraph();
            par.Text = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t(Расшифровка подписи)";

            par = footer.addParagraph();
            par.Text = "    " + DateTime.Now.ToShortDateString() + "\t\t\t\t\t\t\t\t      М.П.";

            /*** Получаем файл в виде строки и записываем его на диск***/
            var rtfDocument = doc.render();
            File.WriteAllText(virPath + "tempblankregistration.rtf", rtfDocument);
            return virPath + "tempblankregistration.rtf";
        }
    }
}
