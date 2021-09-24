namespace FbsUnitTestServices
{
    using System.Text.RegularExpressions;
    using System.Threading;

    using Fbs.Core.WebServiceCheck;

    using NUnit.Framework;

    [TestFixture]
    public class WSSingleCheckTest
    {
        [Test]
        public void CheckByCertificateNumberNNTest()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            var result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result.Contains("<checkResults><certificate>"));
        }

        [Test]
        public void CheckByCertificateNumberNNTestFailWithWrongMarks()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>61</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            var result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result == string.Empty);
        }

        [Test]
        public void CheckByCertificateNumberNNTestFailWithOneMark()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            var result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result.Contains("errors") && result.Contains("Нужно указать баллы не меньше чем по двум предметам"));
        }

        [Test]
        public void CheckByCertificateNumberNNTestFailWithSameMarks()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
           var result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
           Assert.IsTrue(result.Contains("errors") && result.Contains("Нужно указать баллы не меньше чем по двум предметам"));
        }

        [Test]
        public void CheckByCertificateNumberNNTestFail()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result.Contains("errors") && result.Contains("Нужно указать баллы не меньше чем по двум предметам"));
        }

        [Test]
        public void CheckByPassportNumberNNTest()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<passportSeria>7905</passportSeria>" +
                       "<passportNumber>444062</passportNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result.Contains("<passportSeria>7905</passportSeria><passportNumber>444062</passportNumber><certificateNumber>01-000043478-08</certificateNumber>"));
        }

        [Test]
        public void CheckByPassportNumberNNTestFailWithWrongMarks()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<passportSeria>7905</passportSeria>" +
                       "<passportNumber>444062</passportNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>61</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result == string.Empty);
        }

        [Test]
        public void CheckByPassportNumberNNTestNoFailWithoutMarks()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<passportSeria>7905</passportSeria>" +
                       "<passportNumber>444062</passportNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(result.Contains("<checkResults><certificate>"));
        }

        [Test]
        public void CheckByPassportNumberNNTestMultipleResultForOnePassport()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<passportSeria></passportSeria>" +
                       "<passportNumber>341564</passportNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            Assert.IsTrue(Regex.Matches(result, @"\<certificate\>").Count == 3);
        }

        [Test]
        public void ChooseProperCheckPassport()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<passportSeria>123</passportSeria>" +
                       "<passportNumber>341564</passportNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            
            // поиск по паспорту ничего не дает, т.к. серия не верна
            Assert.IsTrue(result == string.Empty);
        }

        [Test]
        public void ChooseProperCheckCertNumber()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<certificateNumber>01-000043478-08</certificateNumber>" +
                       "<passportSeria></passportSeria>" +
                       "<passportNumber></passportNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSSingleCheck("super").SingleCheckNN("super", "127.0.0.1", queryXML);
            
            // поиск по номеру ничего не дает, т.к. оценок нет
            Assert.IsTrue(result.Contains("errors") && result.Contains("Нужно указать баллы не меньше чем по двум предметам"));
        }

        [Test]
        public void BatchCheckByCertificateNumberNNTest()
        {
            var queryXML = "<items>" +
                  "<query>" +
                       "<certificateNumber>77-000057923-11</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>6</subjectName>" +
                       "	<subjectMark>38</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>11</subjectName>" +
                       "	<subjectMark>56</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
                    "<query>" +
                       "<certificateNumber>01-100043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string batchIdxml = new WSBatchCheck("super").BeginBatchCheckNN(queryXML);
            var batchMatch = Regex.Match(batchIdxml, @"\<batchId\>.+\</batchId\>");
            Assert.NotNull(batchMatch);

            string batchResultRequest = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><items>{0}</items>", batchMatch.Value);

            string result = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                result = new WSBatchCheck("super").GetResultNN("1234", batchResultRequest);
                if (result.Contains("<statusMessage>Обработан</statusMessage>"))
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            Assert.IsTrue(Regex.Matches(result, @"\<certificate\>").Count == 2);

            // серия паспорта
            Assert.IsTrue(result.Contains("341564"));

            Assert.IsFalse(result.Contains("lastName") || result.Contains("firstName") || result.Contains("patronymicName"));
        }

        [Test]
        public void BatchCheckByCertificateNumberNNTestFailWithWrongMarks()
        {
            var queryXML = "<items>" +
                  "<query>" +
                       "<certificateNumber>77-000057923-11</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>6</subjectName>" +
                       "	<subjectMark>38</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>11</subjectName>" +
                       "	<subjectMark>57</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
                    "<query>" +
                       "<certificateNumber>01-100043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>61</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string batchIdxml = new WSBatchCheck("super").BeginBatchCheckNN(queryXML);
            var batchMatch = Regex.Match(batchIdxml, @"\<batchId\>.+\</batchId\>");
            Assert.NotNull(batchMatch);

            string batchResultRequest = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><items>{0}</items>", batchMatch.Value);

            string result = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                result = new WSBatchCheck("super").GetResultNN("1234", batchResultRequest);
                if (result.Contains("<statusMessage>Обработан</statusMessage>"))
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            Assert.IsTrue(Regex.Matches(result, @"\<status\>Не найдено\</status\>").Count == 2);
        }

        [Test]
        public void BatchCheckByCertificateNumberNNTestFailWithOneMark()
        {
            var queryXML = "<items>" +
                  "<query>" +
                       "<certificateNumber>77-000057923-11</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>6</subjectName>" +
                       "	<subjectMark>38</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
                    "<query>" +
                       "<certificateNumber>01-100043478-08</certificateNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
               "</items>";
            string result = new WSBatchCheck("super").BeginBatchCheckNN(queryXML);
            Assert.IsTrue(result.Contains("Нужно указать баллы не меньше чем по двум предметам"));
        }

        [Test]
        public void BatchCheckByPassportNumberNNTest()
        {
            var queryXML = "<items>" +
                   "<query>" +
                       "<passportSeria>7905</passportSeria>" +
                       "<passportNumber>444062</passportNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
                     "<query>" +
                       "<passportNumber>444062</passportNumber>" +
                       "<marks>" +
                       "<mark>" +
                       "	<subjectName>1</subjectName>" +
                       "	<subjectMark>83</subjectMark>" +
                       "</mark>" +
                       "<mark>" +
                       "	<subjectName>2</subjectName>" +
                       "	<subjectMark>60</subjectMark>" +
                       "</mark>" +
                       "</marks>" +
                    "</query>" +
                    "<query>" +
                       "<passportSeria></passportSeria>" +
                       "<passportNumber>341564</passportNumber>" +
                       "<marks>" +
                       "</marks>" +
                    "</query>" +
               "</items>";

            string batchIdxml = new WSBatchCheck("super").BeginBatchCheckNN(queryXML);
            var batchMatch = Regex.Match(batchIdxml, @"\<batchId\>.+\</batchId\>");
            Assert.IsFalse(string.IsNullOrEmpty(batchMatch.Value));

            string batchResultRequest = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><items>{0}</items>", batchMatch.Value);

            string result = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                result = new WSBatchCheck("super").GetResultNN("1234", batchResultRequest);
                if (result.Contains("<statusMessage>Обработан</statusMessage>"))
                {
                    break;
                }

                Thread.Sleep(5000);
            }

            Assert.IsTrue(Regex.Matches(result, @"\<certificate\>").Count == 5);
            Assert.IsTrue(Regex.Matches(result, @"\<passportNumber\>341564\</passportNumber\>").Count == 3);
            Assert.IsFalse(result.Contains("lastName") || result.Contains("firstName") || result.Contains("patronymicName"));
        }
    }
}