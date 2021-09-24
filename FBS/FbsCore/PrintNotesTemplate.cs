namespace Fbs.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    using DW.RtfWriter;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.Users;
    using System;
    using System.Linq;
    using Fbs.Core.Organizations;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PrintNotesTemplate
    {
        /// <summary>
        /// Формирование файла со справками
        /// </summary>
        /// <param name="notesList">Список объектов типа CNEInfo</param>
        /// <param name="virPath">Путь до папки куда будет сохранен файл</param>
        /// <param name="uniqueStr">уникальный идентификатор</param>
        /// <param name="orgId">fsdaf</param>
        /// <returns>Путь до файла</returns>
        public static string GetDocument(List<CNEInfo> notesList, string virPath, string uniqueStr, int? orgId = null)
        {
            var doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait, Lcid.TraditionalChinese);
            foreach (var cneInfo in notesList)
            {

                var par = doc.addParagraph();
                par.StartNewPage = true;
                par.Alignment = Align.Center;
                par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Bold);
                par.Text = "СПРАВКА";
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Bold);
                par.Text = "о результатах единого государственного экзамена";
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = "В соответствии со свидетельством";
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = string.Format("№ {0} статус: {1}", cneInfo.CertificateNumber, cneInfo.Status);
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;

                if (!string.IsNullOrEmpty(cneInfo.LastName))
                {
                    par.Text = string.Format("{0} {1} {2}", cneInfo.LastName, cneInfo.FirstName, cneInfo.PatronymicName);
                    par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Underline);
                }
                else
                {
                    par.Text = "__________________________________________________________";
                }

                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.addCharFormat().FontSize = 9;
                par.Text = "(Фамилия Имя Отчество)";

                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = string.Format("Паспорт{0} номер {1}", string.IsNullOrEmpty(cneInfo.PassportSeria) ? string.Empty : " серия " + cneInfo.PassportSeria, cneInfo.PassportNumber);

                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = string.Format("по результатам сдачи единого государственного экзамена в {0} году обнаружил(а) следующие знания по общеобразовательным предметам", cneInfo.Year);

                doc.addParagraph();

                var countMarks = cneInfo.Marks.Count;
                if (countMarks > 0)
                {
                    var table = doc.addTable(countMarks + 1, 2);
                    table.Margins[Direction.Bottom] = 20;

                    var headerCell = table.cell(0, 0).addParagraph();
                    headerCell.Text = "Наименование общеобразовательных предметов";
                    headerCell.Alignment = Align.Center;
                    headerCell = table.cell(0, 1).addParagraph();
                    headerCell.Text = "Баллы";
                    headerCell.Alignment = Align.Center;

                    for (var i = 0; i < cneInfo.Marks.Count; i++)
                    {
                        table.setInnerBorder(BorderStyle.Single, 1f);
                        table.setOuterBorder(BorderStyle.Single, 1f);
                        table.cell(i + 1, 0).addParagraph().Text = cneInfo.Marks[i].SubjectName;

                        var cell = table.cell(i + 1, 1).addParagraph();
                        cell.Text = cneInfo.Marks[i].SubjectMark;
                        cell.Alignment = Align.Center;
                    }
                }

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                Organization org;
                if (orgId.HasValue)
                    org = OrganizationDataAccessor.Get(orgId.Value);
                else
                    org=OrgUserDataAccessor.Get(HttpContext.Current.User.Identity.Name).RequestedOrganization;
                var organizationName = string.Empty;
                if (org != null)
                {
                    organizationName = org.FullName;
                }

                par.Text =
                    string.Format(
                        "Справка для личного дела абитуриента сформирована из Подсистемы ФИС \"Результаты ЕГЭ\" "
                        +
                        "(Свидетельство о государственной регистрации базы данных № 2010620233), "
                        +
                        "действующей на основании Приказа Минобрнауки России от 15 апреля 2009 года №133 для образовательного учреждения:\n"
                        + "\n{0}\n", string.IsNullOrEmpty(organizationName) ? "<название не задано>" : organizationName);

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Лицо, сформировавшее справку:";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "_________________   _________________   ______________________________";
                par = doc.addParagraph();
                par.Text = "        (должность)             (подпись)                       (Фамилия И. О.)";
                var fmt = par.addCharFormat();
                fmt.FontSize = 9;

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Ответственное лицо приемной комиссии:";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "_________________   _________________   ______________________________";
                par = doc.addParagraph();
                par.Text = "        (должность)             (подпись)                       (Фамилия И. О.)";
                fmt = par.addCharFormat();
                fmt.FontSize = 9;

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = "М. П.";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Дата выдачи «____»  ____________  ______ г.";
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "регистрационный № ______________";
            }

            // Получаем файл в виде строки и записываем его на диск
            var rtfDocument = doc.render();
            File.WriteAllText(Path.Combine(virPath, string.Format("test{0}.rtf", uniqueStr)), rtfDocument);
            return Path.Combine(virPath, string.Format("test{0}.rtf", uniqueStr));
        }

        public static string AddNotFoundNotes(string virPath, IEnumerable<CNEInfo> notesList, string uniqueStr, int? orgId = null)
        {
            RtfDocument doc = new RtfDocument(PaperSize.A4, PaperOrientation.Portrait, Lcid.TraditionalChinese);
            foreach (CNEInfo info in notesList)
            {
                var par = doc.addParagraph();
                par.StartNewPage = true;
                par.Alignment = Align.Center;
                par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Bold);
                par.Text = "ИНФОРМАЦИЯ";
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.addCharFormat().FontStyle.addStyle(FontStyleFlag.Bold);
                par.Text = "об отсутствии результатов единого государственного экзамена";
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;

                par.Text = "По запросу с параметрами:";
                if (!String.IsNullOrEmpty(info.CertificateNumber))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Номер свидетельства о результатах ЕГЭ: {0}", info.CertificateNumber);
                }
                if (!String.IsNullOrEmpty(info.PassportSeria))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Серия документа удостоверяющего личность: {0}", info.PassportSeria);
                }
                if (!String.IsNullOrEmpty(info.PassportNumber))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Номер документа удостоверяющего личность: {0}", info.PassportNumber);
                }
                if (!String.IsNullOrEmpty(info.LastName))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Фамилия участника ЕГЭ: {0}", info.LastName);
                }
                if (!String.IsNullOrEmpty(info.FirstName))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Имя участника ЕГЭ: {0}", info.FirstName);
                }
                if (!String.IsNullOrEmpty(info.PatronymicName))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Отчество участника ЕГЭ: {0}", info.PatronymicName);
                }

                if (!String.IsNullOrEmpty(info.Year))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Год: {0}", info.Year);
                }
                if (!String.IsNullOrEmpty(info.TypographicNumber))
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Типографский номер: {0}", info.TypographicNumber);
                }

                foreach (MarkItem mi in info.Marks)
                {
                    par = doc.addParagraph();
                    par.Alignment = Align.Center;
                    par.Text = String.Format("Балл по {0}: {1}", mi.SubjectName,mi.SubjectMark);
                }
                doc.addParagraph();
                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = String.Format("В Подсистеме ФИС «Результаты ЕГЭ» " +
                                        "на {0} сведения о результатах ЕГЭ " +
                                        "за период 2010 – {1}  годы не найдены:\n", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), DateTime.Now.ToString("yyyy"));
                Organization org;
                if (orgId.HasValue)
                    org = OrganizationDataAccessor.Get(orgId.Value);
                else
                    org = OrgUserDataAccessor.Get(HttpContext.Current.User.Identity.Name).RequestedOrganization;
                var organizationName = string.Empty;
                if (org != null)
                {
                    organizationName =  org.FullName;
                }
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = String.Format("Информация получена из Подсистемы ФИС \"Результаты ЕГЭ \" (Свидетельство о государственной регистрации базы данных № 2010620233), образовательным учреждением: " +
                               "\n\n{0}\n", string.IsNullOrEmpty(organizationName) ? "<название не задано>" : organizationName);
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Лицо, получившее информацию:";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "_________________   _________________   ______________________________";
                par = doc.addParagraph();
                par.Text = "        (должность)             (подпись)                       (Фамилия И. О.)";
                var fmt = par.addCharFormat();
                fmt.FontSize = 9;

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Ответственное лицо приемной комиссии:";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "_________________   _________________   ______________________________";
                par = doc.addParagraph();
                par.Text = "        (должность)             (подпись)                       (Фамилия И. О.)";
                fmt = par.addCharFormat();
                fmt.FontSize = 9;

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Center;
                par.Text = "М. П.";

                doc.addParagraph();

                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "Дата выдачи «____»  ____________  ______ г.";
                par = doc.addParagraph();
                par.Alignment = Align.Left;
                par.Text = "регистрационный № ______________";
            }
            var rtfDocument = doc.render();
            File.WriteAllText(
                Path.Combine(virPath, string.Format("test{0}.rtf", uniqueStr)),
                RTFUtils.MergeRTFs(
                    File.ReadAllText(Path.Combine(virPath, string.Format("test{0}.rtf", uniqueStr))),
                    rtfDocument,
                    string.Empty));
            return Path.Combine(virPath, string.Format("test{0}.rtf", uniqueStr));
        }
    }
    class RTFUtils
    {
        public static string getRTFBlock(string blockName, string rtf)
        {

            int i = rtf.IndexOf(@"{\" + blockName);
            int startOfBlock = i;
            //Next find the end of style sheet element tag
            Stack<char> braceHolder = new Stack<char>();
            braceHolder.Push('{');

            string stylesheetBlock = string.Empty;

            while (braceHolder.Count != 0 && i < rtf.Length)
            {
                i++;
                if (rtf[i] == '{')
                {
                    braceHolder.Push('{');
                    continue;
                }
                if (rtf[i] == '}')
                {
                    braceHolder.Pop();
                }
            }
            if (braceHolder.Count == 0)
            {
                //encountered the ending tag for stylesheet
                stylesheetBlock = rtf.Substring(startOfBlock, i - startOfBlock + 1);
                return stylesheetBlock;
            }
            else
            {
                //Error in doc format
                throw (new Exception("Error in doc format"));
            }


        }



        public static string MergeRTFs(string rtf1, string rtf2, string mergingBreak)
        {
            //mergingBreak is the type of break that will be sandwiched between the docs
            //get the fonttbl blocks for both the documents
            string fontTableOfDoc1 = getRTFBlock("fonttbl", rtf1);
            string fontTableOfDoc2 = getRTFBlock("fonttbl", rtf2);

            //get font lists
            List<string> fontList1 = ExtractRTFFonts(fontTableOfDoc1);
            List<string> fontList2 = ExtractRTFFonts(fontTableOfDoc2);

            //Union the font list
            IEnumerable<string> mergedfonts = fontList1.Union(fontList2);
            List<string> fontList3 = new List<string>(mergedfonts);
            string mergedFontListBlock = @"{\fonttbl";
            foreach (string font in fontList3)
            {
                mergedFontListBlock += font;
            }
            mergedFontListBlock += "}";

            //Find location of the fonttable in doc 1 and doc 2
            int indexOfFontTable1 = rtf1.IndexOf(@"{\fonttbl");
            int indexOfFontTable2 = rtf2.IndexOf(@"{\fonttbl");

            string rtfMerged = string.Empty;
            //Get rtf content before and after fonttable
            string headerRTF1 = rtf1.Substring(0, indexOfFontTable1);
            int endOfFontTableIndex = indexOfFontTable1 + (fontTableOfDoc1.Length - 1);
            string trailerRTF1 = rtf1.Substring(endOfFontTableIndex + 1, rtf1.LastIndexOf('}') - (endOfFontTableIndex + 1)); //-2 to remove ending } of 1st doc
            //create the first rtf with merged fontlist
            rtfMerged = headerRTF1 + mergedFontListBlock + trailerRTF1;
            //next identify trailer part after font table in rtf 2
            string trailerRTF2 = rtf2.Substring(indexOfFontTable2 + fontTableOfDoc2.Length);
            rtfMerged += mergingBreak + trailerRTF2;

            return rtfMerged;
        }

        private static List<string> ExtractRTFFonts(string fontTableBlock)
        {
            Stack<char> braces = new Stack<char>();
            List<string> fonts = new List<string>();
            int fontDefStart = 0, fontDefLength;
            braces.Push('{');
            int i = 0;
            while (braces.Count > 0 && i < fontTableBlock.Length)
            {
                i++;
                if (fontTableBlock[i] == '{')
                {
                    braces.Push('{');
                    if (braces.Count == 2)
                    {
                        //means font definition brace started store the position
                        fontDefStart = i;
                    }
                    continue;
                }
                if (fontTableBlock[i] == '}')
                {
                    braces.Pop();
                    if (braces.Count == 1)
                    {
                        //means only root level brace left identifying one font definition ended
                        fontDefLength = i - fontDefStart + 1;
                        fonts.Add(fontTableBlock.Substring(fontDefStart, fontDefLength));
                    }
                }
            }

            if (braces.Count == 0)
            {
                //everything is fine then
                return fonts;
            }
            else
            {
                //malformed font table passed
                throw (new Exception("Malformed font table passed"));
            }
        }


    }
}
