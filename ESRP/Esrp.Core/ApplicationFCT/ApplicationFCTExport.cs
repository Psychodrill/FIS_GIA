using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using Esrp.Core.Organizations;
using System.Drawing.Imaging;
using Esrp.Core.Properties;


namespace Esrp.Core.ApplicationFCT
{
    public class ApplicationFCTExport
    {

        static Font _ft8;
        static Font _ft10;
        static Font _ft12;
        static Font _ft10_bold;



        #region util methods

        static Paragraph CreateParagraph(Font pf, string strContent, int Align, bool isBlue)
        {
            Chunk ch;
            Paragraph p;
            int app_len = strContent.Length;

            if (isBlue)
            {
                ch = new Chunk(strContent, pf);
                ch.SetBackground(new BaseColor(0x64, 0x95, 0xDC));
                while (app_len < 150)
                {
                    ch.Append(" ");
                    app_len++;
                }
                p = new Paragraph(ch);
            }
            else
                p = new Paragraph(strContent, pf);

            p.Alignment = Align;
            return p;
        }

        static PdfPTable CreateTable(Font tf, string[] cellsContent, bool secondLong)
        {
            int[] tableWidthCols;
            int Width0 = 1;

            if (cellsContent[0].Length > 3)
                Width0 = 2;


            if (secondLong)
                tableWidthCols = new int[] { Width0, 10, 9 };
            else
                tableWidthCols = new int[] { Width0, 7, 12 };


            PdfPTable table = new PdfPTable(3);
            PdfPCell theCell;

            table.SetWidths(tableWidthCols);
            table.KeepTogether = true;

            table.HorizontalAlignment = Element.ALIGN_CENTER;
            foreach (string str in cellsContent)
            {
                theCell = new PdfPCell(new Phrase(str, tf));
                theCell.PaddingBottom = 5;
                theCell.HorizontalAlignment = Element.ALIGN_CENTER;                
                table.AddCell(theCell);
            }
            return table;
        }

        static PdfPTable CreateFinalTable(Font tf, string[] cellsContent)
        {
            int[] tableWidthCols = new int[] { 1, 1 };


            PdfPTable table = new PdfPTable(2);
            PdfPCell theCell;
            table.SetWidths(tableWidthCols);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.KeepTogether = true;

            theCell = new PdfPCell(new Phrase(cellsContent[0], tf));
            theCell.PaddingBottom = 5;
            theCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(theCell);

            theCell = new PdfPCell(new Phrase(cellsContent[1], tf));
            theCell.PaddingBottom = 5;
            theCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(theCell);

            return table;
        }

        static void InitFonts()
        {
            if (_ft8 != null)
                return;

            string fontPath = Environment.GetEnvironmentVariable("windir") + @"\Fonts\Calibri.ttf";
            BaseFont bf = BaseFont.CreateFont(fontPath, Encoding.GetEncoding(1251).BodyName, true);
            _ft8 = new Font(bf, 8);
            _ft10 = new Font(bf, 10);
            _ft12 = new Font(bf, 12, 1);
            _ft10_bold = new Font(bf, 10, 1);
        }

        #endregion

        public static void SaveApplicationFCTInPDF(string FullFilePath, Organization org, ApplicationFCT app)
        {
                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

                PdfWriter.GetInstance(document, new FileStream(FullFilePath, FileMode.Create, FileAccess.Write));

                InitFonts();

                document.AddAuthor("ESRP");
                document.AddSubject("Заявка на подключение к ЗКСПД");

                document.Open();

                MemoryStream stream = new MemoryStream();
                Resources.logo.Save(stream, ImageFormat.Jpeg);
                byte[] content = stream.ToArray();
                Image img = Image.GetInstance(content);
                img.Alignment = Element.ALIGN_CENTER;
                document.Add(img);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                

                Paragraph p = CreateParagraph(_ft12, "ФОРМА #1. КАРТОЧКА ПОДКЛЮЧАЕМОЙ ОРГАНИЗАЦИИ.", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                p = CreateParagraph(_ft12, "1   ОРГАНИЗАЦИЯ", Element.ALIGN_LEFT, true);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                PdfPTable tbl = CreateTable(_ft10, new string[] { "1.1", "Идентификатор", "" }, false);
                document.Add(tbl);
                p = CreateParagraph(_ft8, "Присваивается и заполняется сотрудниками ФЦТ", Element.ALIGN_RIGHT, false);
                p.IndentationRight = 50;
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "1.2", "Наименование организации", org.ShortName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "1.3", "Фактический адрес", org.FactAddress }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "1.4", "Телефон приемной", org.Phone }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft12, "2   ОТВЕТСТВЕННОЕ ЛИЦО", Element.ALIGN_LEFT, true);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.1", "ФИО", app.PersonFullName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.2", "Должность", app.PersonPosition }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.3", "Рабочий телефон", app.PersonWorkPhone }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.4", "Мобильный телефон", app.PersonMobPhone }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.5", "E-Mail", app.PersonEmail }, false);
                document.Add(tbl);

                document.NewPage();

                p = CreateParagraph(_ft12, "3   ХАРАКТЕРИСТИКА ОБЪЕКТА", Element.ALIGN_LEFT, true);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "3.1", "Есть ли аттестат на ИСПДн не ниже К1? (да/нет)", app.IsThereAttestatK1More ? "да" : "нет" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "3.2", "Количество подключаемых АРМ", app.NumARMs.ToString() }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "3.3", "Количество ПДн за год", app.NumPDNs.ToString() }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "3.4", "Осуществляется ли автоматизированная обработка ПДн  в АС организации? (да/нет)", "нет" }, true);
                document.Add(tbl);
                document.NewPage();

                p = CreateParagraph(_ft12, "ФОРМА №2. СХЕМА ПОДКЛЮЧЕНИЯ.", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                p = CreateParagraph(_ft12, "1   ОБЩАЯ СХЕМА", Element.ALIGN_LEFT, true);
                document.Add(p);

                stream = new MemoryStream();
                Resources.scheme.Save(stream, ImageFormat.Png);
                content = stream.ToArray();
                img = Image.GetInstance(content);

                img.Alignment = Element.ALIGN_LEFT;
                document.Add(img);

                stream = new MemoryStream();
                Resources.sch_table.Save(stream, ImageFormat.Png);
                content = stream.ToArray();
                img = Image.GetInstance(content);
                img.Alignment = Element.ALIGN_LEFT;
                document.Add(img);
                document.NewPage();


                p = CreateParagraph(_ft12, "2   ОПИСАНИЕ СХЕМЫ", Element.ALIGN_LEFT, true);
                document.Add(p);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.1", "Схема подключения", "Схема №1" }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft10_bold, "Блок №1 - выделенные АРМ", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);


                tbl = CreateTable(_ft10, new string[] { "2.2", "Операционная система", app.DictOperationSystemName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                if (((bool)app.IsIPStatic) && (app.IPAddress != null))
                {
                    string[] ipaddresses = app.IPAddress.Split('|');
                    int nIndexNum = 0;
                    do
                    {
                        nIndexNum++;
                        tbl = CreateTable(_ft10, new string[] { "2.3." + nIndexNum, "АРМ " + nIndexNum, ipaddresses[nIndexNum - 1] }, false);
                        document.Add(tbl);
                        document.Add(Chunk.NEWLINE);

                    } while (nIndexNum < app.NumARMs);
                }

                if (!(bool)app.IsIPStatic)
                {
                    tbl = CreateTable(_ft10, new string[] { "2.3","IP адрес для АРМ", "Используется динамический IP-адрес, просим выделить виртуальный адрес ViPNet"}, false);
                    document.Add(tbl);
                    document.Add(Chunk.NEWLINE);                    
                }

                tbl = CreateTable(_ft10, new string[] { "2.4", "Учетная запись в ФИС ЕГЭ и приема", app.FISLogin }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.5", "Антивирусная защита", app.DictAntivirusName == null ? "Не требуется, так как используется VipNet Terminal" : app.DictAntivirusName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.6", "Средства защиты информации от НСД", app.DictUnauthAccessProtectName == null ? "Не требуется, так как используется VipNet Terminal" : app.DictUnauthAccessProtectName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.7", "Электронный замок", app.DictElectronicLockName == null ? "Не требуется, так как используется VipNet Terminal" : app.DictElectronicLockName }, false);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft10_bold, "Блок №2 - Специализированные СЗИ", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);


                tbl = CreateTable(_ft10, new string[] { "2.8", "СЗИ идентификации отправителей и получателей запросов на ПДн", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.9", "СЗИ регистрации факта совершения запросов на передачу ПДн, сведений об ответах на такие запросы, а также сведений о результатах обработки таких ответов", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.10", "СЗИ ограничения передачи заданной группы сведений в составе передаваемых ПДн", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.11", "СЗИ ограничения передачи ПДн по количеству заданных субъектов", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.12", "СЗИ ограничения возможности передачи ПДн рамками заданного периода времени", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.13", "СЗИ ограничения передачи ПДн в зависимости от отправителя и получателя запроса ПДн", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);
                tbl = CreateTable(_ft10, new string[] { "2.14", "Прочие СЗИ", "" }, true);
                document.Add(tbl);

                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);
                document.Add(Chunk.NEWLINE);


                p = CreateParagraph(_ft10_bold, "Блок №3 - Межсетевой экран", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);


                tbl = CreateTable(_ft10, new string[] { "2.15", "Наименование МЭ", app.DictTNScreenName == null ? "Используется межсетевой экран в составе  VipNet Terminal" : app.DictTNScreenName }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.16", "IP адрес МЭ", app.IP4TNS }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.17", "Правила фильтрации МЭ", "Запрещены все соединения, разрешены входящие и исходящие соединения по протоколам TCP, UDP для IP 85.143.100.24" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft10_bold, "Блок №4 - Криптографическая защита информации", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.18", "Средство криптографической защиты информации", app.DictVipNetCryptoName == null ? "Используются СКЗИ в составе VipNet Terminal" : app.DictVipNetCryptoName }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft10_bold, "Блок №5 - Обработка и передача персональных данных", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.19", "Планируется ли использовать  АРМ, находящиеся в ЛВС для подключения к ЗКСПД  и обработки и передачи ПДН?", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);


                tbl = CreateTable(_ft10, new string[] { "2.20", "Планируется ли осуществление автоматизированной обработки и передачи ПДн?", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);



                p = CreateParagraph(_ft10_bold, "Блок №6 - Аттестованная ИСПДн класса К1", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);


                tbl = CreateTable(_ft10, new string[] { "2.21", "Наименование", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.22", "Срок действия аттестата", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.23", "Количество АРМ в ИСПДн", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                tbl = CreateTable(_ft10, new string[] { "2.24", "Дата последней проверки защищенности объекта аттестации", "" }, true);
                document.Add(tbl);
                document.Add(Chunk.NEWLINE);

                p = CreateParagraph(_ft10, "Достоверность предоставленных данных гарантируем. Обязуемся не нарушать согласованную схему подключения к ЗКСПД и производить изменения только по согласованию с ФГБУ «ФЦТ».", Element.ALIGN_CENTER, false);
                document.Add(p);
                document.Add(Chunk.NEWLINE);

                Paragraph pt = new Paragraph();
                pt.KeepTogether = true;
                pt.Alignment = Element.ALIGN_RIGHT;

                tbl = CreateFinalTable(_ft10, new string[] { "''____''________________201_г.", "" });
                pt.Add(tbl);
                p = CreateParagraph(_ft8, "Подпись ответственного лица и печать организации", Element.ALIGN_RIGHT, false);
                p.IndentationRight = 50;
                pt.Add(p);                
                document.Add(pt);

                document.CloseDocument();

        }

    }
}
