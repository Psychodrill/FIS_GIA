using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Fbs.Core.Shared;

namespace Fbs.Core.WebServiceCheck
{
    using System.Text.RegularExpressions;

    public class QueryItem
    {
        #region Fields
        QueryParameter _patronymicName, _firstName, _lastName;

        /// <summary>
        /// Имя
        /// </summary>
        public QueryParameter QueryFirstName
        {
            get;
            set;
        }          

        
        /// /// <summary>
        /// Фамилия
        /// </summary> 
        public QueryParameter QueryLastName
        {
            get;
            set;
        }          
        

        /// <summary>
        /// Отчество
        /// </summary>
        public QueryParameter QueryPatronymicName
        {
            get;
            set;
        }     
        
        public QueryParameter QueryPassportSeria { get; set; }      // Серия паспорта
        public QueryParameter QueryPassportNumber { get; set; }     // Номер паспорта
        public QueryParameter QueryCertificateNumber { get; set; }  // Номер свидетельства
        public QueryParameter QueryTypographicNumber { get; set; }  // Типографский номер свидетельства
        public QueryParameter Marks { get; set; }  //Оценки
        private int nodeIndex;  // Порядковый номер элемента <query>

        #endregion

        #region Constructors

        public QueryItem(XmlNode xmlNode, int itemIndex)
        {
            nodeIndex = itemIndex;

            QueryFirstName = ExtractNodeValue(xmlNode, "firstName", "Имя");
            QueryLastName = ExtractNodeValue(xmlNode, "lastName", "Фамилия");
            QueryPatronymicName = ExtractNodeValue(xmlNode, "patronymicName", "Отчество");
            QueryPassportSeria = ExtractNodeValue(xmlNode, "passportSeria", "Серия паспорта");
            QueryPassportNumber = ExtractNodeValue(xmlNode, "passportNumber", "Номер паспорта");
            QueryCertificateNumber = ExtractNodeValue(xmlNode, "certificateNumber", "Номер свидетельства");
            QueryTypographicNumber = ExtractNodeValue(xmlNode, "typographicNumber", "Типографский номер");
            this.Marks = this.GetMarks(xmlNode, "marks", "Баллы по предметам");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Определяется тип поискового алгоритма по наполненности полей элемента 'query'
        /// </summary>
        /// <returns>Тип поискового запроса</returns>
        public WSSearchType GetSearchType()
        {
            if (!QueryLastName.ValueExists)
            {
                throw new WSQueryElementException(string.Format("Невозможно определить тип поискового алгоритма. Отсутствует одно из обязательных полей lastName в элементе 'query' с индексом {0}", nodeIndex));
            }

            if (QueryCertificateNumber.ValueExists)
            {
                return WSSearchType.wsCertNumber;
            }

            if (QueryTypographicNumber.ValueExists)
            {
                return WSSearchType.wsTypoNumber;
            }

            if (QueryPassportNumber.ValueExists)
            {
                return WSSearchType.wsPassport;
            }

            throw new WSQueryElementException(string.Format("Невозможно определить тип поискового алгоритма. Для элемента 'query' с индексом {0} должны быть заданы значения для элементов certificateNumber, либо passportNumber, либо typographicNumber", nodeIndex));
        }

        public void CheckStructure()
        {
            List<string> errorMessages = new List<string>();

            AddStructureError(QueryFirstName, errorMessages);
            AddStructureError(QueryLastName, errorMessages);
            AddStructureError(QueryPatronymicName, errorMessages);
            AddStructureError(QueryPassportSeria, errorMessages);
            AddStructureError(QueryPassportNumber, errorMessages);
            AddStructureError(QueryCertificateNumber, errorMessages);
            AddStructureError(QueryTypographicNumber, errorMessages);

            if (errorMessages.Count > 0)
            {
                throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Отсутствуют элементы: {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
            }
        }

        public void CheckStructurePassportNumber()
        {
            var errorMessages = new List<string>();

            this.AddStructureError(this.QueryPassportNumber, errorMessages);

            if (errorMessages.Count > 0)
            {
                throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Отсутствуют элементы: {1}", this.nodeIndex, string.Join(", ", errorMessages.ToArray())));
            }
        }

        public void CheckStructureCertNumber()
        {
            var errorMessages = new List<string>();
            var markMessages = new List<string>();

            this.AddStructureError(this.QueryCertificateNumber, errorMessages);
            if (errorMessages.Count > 0)
            {
                throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Отсутствуют элементы: {1}", this.nodeIndex, string.Join(", ", errorMessages.ToArray())));
            }
            
            this.AddStructureError(this.Marks, markMessages);
            if (markMessages.Count > 0)
            {
                throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Нужно указать баллы не меньше чем по двум предметам.", this.nodeIndex));
            }
        }

        public void CheckDataNN(WSSearchType searchType)
        {
            List<string> errorMessages = new List<string>();
            if (searchType == WSSearchType.wsCertNumber)
            {
                AddDataError(QueryCertificateNumber, errorMessages);
                if (errorMessages.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по номеру сертификата. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
                }
            }

            // тип поиска: по серии и номеру паспорта
            if (searchType == WSSearchType.wsPassport)
            {
                AddDataError(QueryPassportNumber, errorMessages);
                if (errorMessages.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по серии и номеру паспорта. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
                }

                List<string> nErrors = DocumentCheck.DocNumberCheck(QueryPassportNumber.Value);
                if (nErrors.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по серии и номеру паспорта. Ошибка в элементе 'Номер документа': {1}", nodeIndex, string.Join(", ", nErrors.ToArray())));
                }
            }
        }

        public void CheckData(WSSearchType searchType)
        {
            List<string> errorMessages = new List<string>();

            AddDataError(QueryLastName, errorMessages);

            if (errorMessages.Count > 0)
            {
                throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
            }

            // тип поиска: по номеру сертификата
            if (searchType == WSSearchType.wsCertNumber)
            {
                AddDataError(QueryCertificateNumber, errorMessages);
                if (errorMessages.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по номеру сертификата. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
                }
            }

            // тип поиска: по серии и номеру паспорта
            if (searchType == WSSearchType.wsPassport)
            {
                AddDataError(QueryPassportNumber, errorMessages);
                if (errorMessages.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по серии и номеру паспорта. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
                }

                List<string> sErrors = DocumentCheck.DocSeriesCheck(QueryPassportSeria.Value);
                if (sErrors.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по серии и номеру паспорта. Ошибка в элементе 'Серия документа': {1}", nodeIndex, string.Join(", ", sErrors.ToArray())));
                }

                List<string> nErrors = DocumentCheck.DocNumberCheck(QueryPassportNumber.Value);
                if (nErrors.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по серии и номеру паспорта. Ошибка в элементе 'Номер документа': {1}", nodeIndex, string.Join(", ", nErrors.ToArray())));
                }
            }

            // тип поиска: по типографскому номеру сертификата
            if (searchType == WSSearchType.wsTypoNumber)
            {
                AddDataError(QueryTypographicNumber, errorMessages);
                if (errorMessages.Count > 0)
                {
                    throw new WSQueryElementException(string.Format("Ошибка в элементе 'query' с индексом {0}. Тип поиска: по типографскому номеру сертификата. Отсутствуют значения для элементов {1}", nodeIndex, string.Join(", ", errorMessages.ToArray())));
                }
            }
        }

        #endregion

        #region Private Methods


        private QueryParameter GetMarks(XmlNode queryNode, string nodeName, string parameterName)
        {
            QueryParameter result = new QueryParameter(parameterName, nodeName);
            var marksNode = queryNode.SelectSingleNode("./" + nodeName);
            result.NodeExists = false;
            if (marksNode != null)
            {
                var markNodes = marksNode.SelectNodes("./mark");
                var value = new StringBuilder();
                int cnt = 0;
                if (markNodes != null && markNodes.Count >= 2)
                {
                        for (int i = 0; i < markNodes.Count; i++)
                        {
                            var subjectName = markNodes[i].SelectSingleNode("./subjectName");
                            var subjectMark = markNodes[i].SelectSingleNode("./subjectMark");
                            if (subjectName != null && subjectMark != null)
                            {
                                if (!Regex.IsMatch(value.ToString(), string.Format(@"\D{0}=", subjectName.InnerText)))
                                {
                                    value.AppendFormat("{0}{1}={2}", value.Length == 0 ? string.Empty : ",", subjectName.InnerText, subjectMark.InnerText);
                                    cnt++;
                                }
                            }
                        }

                        if (cnt >= 2)
                        {
                            result.NodeExists = true;
                            result.Value = value.ToString();
                        }
                }
             }

            return result;
        }


        /// <summary>
        /// Извлекает из элемента 'query' параметр с именем nodeName
        /// </summary>
        /// <param name="queryNode">XML-фрагмент. Тэг 'query'</param>
        /// <param name="nodeName">Наименование тэга параметра. Используется для поиска в элементе 'query'</param>
        /// <param name="parameterName">Наименование (на русском) узла</param>
        /// <returns>Возвращает объект, в котором содержится информация о параметре (наименование тэга, имя, значение)</returns>
        private QueryParameter ExtractNodeValue(XmlNode queryNode, string nodeName, string parameterName)
        {
            QueryParameter result = new QueryParameter(parameterName, nodeName);
            if (queryNode.SelectSingleNode("./" + nodeName) == null)
            {
                result.NodeExists = false;
            }
            else
            {
                result.NodeExists = true;
                result.Value = queryNode.SelectSingleNode("./" + nodeName).InnerText;
            }
            return result;
        }

        private void AddStructureError(QueryParameter prm, List<string> errorList)
        {
            if (!prm.NodeExists) errorList.Add(string.Format("'{0}'", prm.NodeName));
        }

        private void AddDataError(QueryParameter prm, List<string> errorList)
        {
            if (!prm.ValueExists) errorList.Add(string.Format("'{0}'", prm.NodeName));
        }

        #endregion

    }
}
