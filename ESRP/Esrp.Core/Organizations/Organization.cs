namespace Esrp.Core.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Esrp.Core.CatalogElements;

    /// <summary>
    /// The organization type.
    /// </summary>
    public enum OrganizationType
    {
        /// <summary>
        /// ВУЗ
        /// </summary>
        VUZ = 1,

        /// <summary>
        /// ССУЗ
        /// </summary>
        SSUZ = 2,

        /// <summary>
        /// РЦОИ - регистрационный центр обработки информации
        /// </summary>
        InfoProcessing = 3,

        /// <summary>
        /// ОУО - Орган управления образованием
        /// </summary>
        Direction = 4,

        /// <summary>
        /// Другое
        /// </summary>
        Other = 5,

        /// <summary>
        /// Учредитель
        /// </summary>
        Founder = 6
    }

    /// <summary>
    /// Организация из справочника организаций
    /// </summary>
    public class Organization
    {
        #region Constants and Fields

        private CatalogElement Kind_ = new CatalogElement();

        private CatalogElement OrgType_ = new CatalogElement();

        private CatalogElement Region_ = new CatalogElement();

        private CatalogElement IS_ = new CatalogElement();
        /// <summary>
        /// Заполняется, если выбрана "Другая модель приемной кампании"
        /// </summary>
        private string _rcDescription;

        private CatalogElement status = new CatalogElement();

        public IEnumerable<int> FounderIds
        {
            get
            {
                if (!Founders.Any())
                    return new int[0];
                return Founders.Where(x => x.Id.HasValue).Select(x => x.Id.Value).Distinct();
            }
        }

        public List<CatalogElement> Founders { get { return founders_; } }
        private List<CatalogElement> founders_ = new List<CatalogElement>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Organization"/> class.
        /// </summary>
        /// <param name="fullName">
        /// The full name.
        /// </param>
        /// <param name="regionId">
        /// The region id.
        /// </param>
        /// <param name="typeId">
        /// The type id.
        /// </param>
        /// <param name="kindId">
        /// The kind id.
        /// </param>
        /// <param name="ISId">
        /// The IS id.
        /// </param>
        /// <param name="isPrivate">
        /// The is private.
        /// </param>
        /// <param name="INN_In">
        /// The in n_ in.
        /// </param>
        /// <param name="OGRN_In">
        /// The ogr n_ in.
        /// </param>
        /// <param name="statusId">
        /// The status id.
        /// </param>
        /// <param name="dateChangeStatus">
        /// The date change status.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// <param name="OrganizationISId">
        /// </param>
        public Organization(
            string fullName,
            int regionId,
            int typeId,
            int kindId,
            bool isPrivate,
            string INN_In,
            string OGRN_In,
            int statusId,
            DateTime? dateChangeStatus,
            string reason,        
            int ISId,
            string AnotherName)
        {
            this.FullName = fullName;
            this.Region = new CatalogElement(regionId);
            this.OrgType = new CatalogElement(typeId);
            this.Kind = new CatalogElement(kindId);
            this.IsPrivate = isPrivate;
            this.INN = INN_In;
            this.OGRN = OGRN_In;
            this.Status = new CatalogElement(statusId);
            this.DateChangeStatus = dateChangeStatus;
            this.Reason = reason;
            this.IS = new CatalogElement(ISId);
            this.AnotherName = AnotherName;
        }

        internal Organization()
        {
        }

        internal Organization(IDataReader reader)
        {
            this.ReceptionOnResultsCNE = reader[OrganizationDataAccessor.TableColumns.ReceptionOnResultsCNE] == DBNull.Value
                                     ? (int?)null
                                     : Convert.ToInt32(reader[OrganizationDataAccessor.TableColumns.ReceptionOnResultsCNE]);
            this.Id = Convert.ToInt32(reader[OrganizationDataAccessor.TableColumns.Id]);
            this.FullName = reader[OrganizationDataAccessor.TableColumns.FullName].ToString();
            this.ShortName = reader[OrganizationDataAccessor.TableColumns.ShortName].ToString();
            this.IsPrivate = Convert.ToBoolean(reader[OrganizationDataAccessor.TableColumns.IsPrivate]);
            this.IsFilial = Convert.ToBoolean(reader[OrganizationDataAccessor.TableColumns.IsFilial]);
            this.INN = reader[OrganizationDataAccessor.TableColumns.INN].ToString();
            this.OGRN = reader[OrganizationDataAccessor.TableColumns.OGRN].ToString();
            this.KPP = reader[OrganizationDataAccessor.TableColumns.KPP].ToString();
            this.OwnerDepartment = reader[OrganizationDataAccessor.TableColumns.OwnerDepartment].ToString();
            this.DirectorFullName = reader[OrganizationDataAccessor.TableColumns.DirectorFullName].ToString();
            this.DirectorFullNameInGenetive = reader[OrganizationDataAccessor.TableColumns.DirectorFullNameInGenetive].ToString();
            this.DirectorFirstName = reader[OrganizationDataAccessor.TableColumns.DirectorFirstName].ToString();
            this.DirectorLastName = reader[OrganizationDataAccessor.TableColumns.DirectorLastName].ToString();
            this.DirectorPatronymicName = reader[OrganizationDataAccessor.TableColumns.DirectorPatronymicName].ToString();
            this.DirectorPosition = reader[OrganizationDataAccessor.TableColumns.DirectorPosition].ToString();
            this.DirectorPositionInGenetive = reader[OrganizationDataAccessor.TableColumns.DirectorPositionInGenetive].ToString();
            this.OUConfirmation = Convert.ToBoolean(reader[OrganizationDataAccessor.TableColumns.OUConfirmation]);
            this.AccreditationSertificate = reader[OrganizationDataAccessor.TableColumns.AccreditationSertificate].ToString();
            this.LawAddress = reader[OrganizationDataAccessor.TableColumns.LawAddress].ToString();
            this.TownName = reader[OrganizationDataAccessor.TableColumns.TownName].ToString();
            this.FactAddress = reader[OrganizationDataAccessor.TableColumns.FactAddress].ToString();
            this.PhoneCityCode = reader[OrganizationDataAccessor.TableColumns.PhoneCityCode].ToString();
            this.Phone = reader[OrganizationDataAccessor.TableColumns.Phone].ToString();
            this.Fax = reader[OrganizationDataAccessor.TableColumns.Fax].ToString();
            this.EMail = reader[OrganizationDataAccessor.TableColumns.EMail].ToString();
            this.Site = reader[OrganizationDataAccessor.TableColumns.Site].ToString();
            this.Region = new CatalogElement(
                reader, OrganizationDataAccessor.TableColumns.RegionId, OrganizationDataAccessor.TableColumns.RegionName)
                {
                    Code = reader[OrganizationDataAccessor.TableColumns.RegionCode].ToString()
                };
            this.OrgType = new CatalogElement(
                reader, OrganizationDataAccessor.TableColumns.TypeId, OrganizationDataAccessor.TableColumns.TypeName);
            this.Kind = new CatalogElement(
                reader, OrganizationDataAccessor.TableColumns.KindId, OrganizationDataAccessor.TableColumns.KindName);
            this.IS = new CatalogElement(
                reader, OrganizationDataAccessor.TableColumns.ISId, OrganizationDataAccessor.TableColumns.ISName);
            this.ConnectionScheme = new CatalogElement(reader, OrganizationDataAccessor.TableColumns.ConnectionSchemeId, OrganizationDataAccessor.TableColumns.ConnectionSchemeName);
            this.ConnectionStatus = new CatalogElement(reader, OrganizationDataAccessor.TableColumns.ConnectionStatusId, OrganizationDataAccessor.TableColumns.ConnectionStatusName);
            this.TimeConnectionToSecureNetwork = reader[OrganizationDataAccessor.TableColumns.TimeConnectionToSecureNetwork] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader[OrganizationDataAccessor.TableColumns.TimeConnectionToSecureNetwork]);
            this.TimeEnterInformationInFIS = reader[OrganizationDataAccessor.TableColumns.TimeEnterInformationInFIS] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader[OrganizationDataAccessor.TableColumns.TimeEnterInformationInFIS]);
            this.IsAgreedTimeConnection = reader[OrganizationDataAccessor.TableColumns.IsAgreedTimeConnection] == DBNull.Value
                        ? (bool?)null : Convert.ToBoolean(reader[OrganizationDataAccessor.TableColumns.IsAgreedTimeConnection]);

            this.IsAgreedTimeEnterInformation = reader[OrganizationDataAccessor.TableColumns.IsAgreedTimeEnterInformation] == DBNull.Value
                 ? (bool?)null : Convert.ToBoolean(reader[OrganizationDataAccessor.TableColumns.IsAgreedTimeEnterInformation]);

            this.LetterToReschedule = (reader[OrganizationDataAccessor.TableColumns.LetterToReschedule] == DBNull.Value
                                           ? null
                                           : reader[OrganizationDataAccessor.TableColumns.LetterToReschedule]) as byte[];
            this.LetterToRescheduleName = reader[OrganizationDataAccessor.TableColumns.LetterToRescheduleName] == DBNull.Value
                                     ? string.Empty
                                     : reader[OrganizationDataAccessor.TableColumns.LetterToRescheduleName].ToString();

            this.LetterToRescheduleContentType = reader[OrganizationDataAccessor.TableColumns.LetterToRescheduleContentType] == DBNull.Value
                         ? string.Empty
                         : reader[OrganizationDataAccessor.TableColumns.LetterToRescheduleContentType].ToString();

            // Модель приемной кампании
            this.RCModelId = reader[OrganizationDataAccessor.TableColumns.RCModelId] as int?;
            this.RCModelName = reader[OrganizationDataAccessor.TableColumns.RCModelName].ToString();
            this.RCDescription = reader[OrganizationDataAccessor.TableColumns.RCDescription] == DBNull.Value
                                     ? string.Empty
                                     : reader[OrganizationDataAccessor.TableColumns.RCDescription].ToString();

            // Сведения об объеме и структуре приема
            this.CNFederalBudget = (int)reader[OrganizationDataAccessor.TableColumns.CNFederalBudget];
            this.CNTargeted = (int)reader[OrganizationDataAccessor.TableColumns.CNTargeted];
            this.CNLocalBudget = (int)reader[OrganizationDataAccessor.TableColumns.CNLocalBudget];
            this.CNPaying = (int)reader[OrganizationDataAccessor.TableColumns.CNPaying];
            this.CNFullTime = (int)reader[OrganizationDataAccessor.TableColumns.CNFullTime];
            this.CNEvening = (int)reader[OrganizationDataAccessor.TableColumns.CNEvening];
            this.CNPostal = (int)reader[OrganizationDataAccessor.TableColumns.CNPostal];

            this.MainId = reader[OrganizationDataAccessor.TableColumns.MainId] == DBNull.Value
                              ? (int?)null
                              : (int)reader[OrganizationDataAccessor.TableColumns.MainId];

            this.MainFullName = reader[OrganizationDataAccessor.TableColumns.MainFullName].ToString();
            this.MainShortName = reader[OrganizationDataAccessor.TableColumns.MainShortName].ToString();

            // Сведения об учредителе организации
            this.DepartmentId = reader[OrganizationDataAccessor.TableColumns.DepartmentId] == DBNull.Value
                                    ? (int?)null
                                    : (int)reader[OrganizationDataAccessor.TableColumns.DepartmentId];
            this.DepartmentFullName = reader[OrganizationDataAccessor.TableColumns.DepartmentFullName].ToString();
            this.DepartmentShortName = reader[OrganizationDataAccessor.TableColumns.DepartmentShortName].ToString();

            this.CreateDate = Convert.ToDateTime(reader[OrganizationDataAccessor.TableColumns.CreateDate]);
            this.UpdateDate = Convert.ToDateTime(reader[OrganizationDataAccessor.TableColumns.UpdateDate]);

            this.Status = new CatalogElement(
                reader, OrganizationDataAccessor.TableColumns.StatusId, OrganizationDataAccessor.TableColumns.StatusName);

            this.NewOrgId = reader[OrganizationDataAccessor.TableColumns.NewOrgId] == DBNull.Value
                                ? (int?)null
                                : (int)reader[OrganizationDataAccessor.TableColumns.NewOrgId];

            this.NewOrgFullName = reader[OrganizationDataAccessor.TableColumns.NewOrgFullName].ToString();
            this.NewOrgShortName = reader[OrganizationDataAccessor.TableColumns.NewOrgShortName].ToString();
            this.Version = Convert.ToInt32(reader[OrganizationDataAccessor.TableColumns.Version]);
            this.DateChangeStatus = reader[OrganizationDataAccessor.TableColumns.DateChangeStatus] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader[OrganizationDataAccessor.TableColumns.DateChangeStatus]);
            this.Reason = reader[OrganizationDataAccessor.TableColumns.Reason].ToString();
            try
            {
                this.AnotherName = reader[OrganizationDataAccessor.TableColumns.IsAnotherName].ToString();
                this.ISLOD_GUID = reader[OrganizationDataAccessor.TableColumns.ISLOD_GUID].ToString();
            }
            catch { } // Ничего не делаем просто потому, что это не ошибка, а просто такого поля может и не быть.

            this.LicenseRegNumber = reader[OrganizationDataAccessor.TableColumns.LicenseRegNumber].ToString();
            this.LicenseOrderDocumentDate = reader[OrganizationDataAccessor.TableColumns.LicenseOrderDocumentDate] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        reader[OrganizationDataAccessor.TableColumns.LicenseOrderDocumentDate]);
            this.LicenseStatusName = reader[OrganizationDataAccessor.TableColumns.LicenseStatusName].ToString();

            // FIS-1777 - added by akopylov 30.10.2017
            this.SupplementNumber = reader[OrganizationDataAccessor.TableColumns.SupplementNumber].ToString();
            this.SupplementOrderDocumentDate = reader[OrganizationDataAccessor.TableColumns.SupplementOrderDocumentDate] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(
                                        reader[OrganizationDataAccessor.TableColumns.SupplementOrderDocumentDate]);
            this.SupplementStatusName = reader[OrganizationDataAccessor.TableColumns.SupplementStatusName].ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Срок подключения к защищенной сети
        /// </summary>
        public DateTime? TimeConnectionToSecureNetwork { get; set; }

        /// <summary>
        /// Срок внесения сведений в ФИС ЕГЭ и приема
        /// </summary>
        public DateTime? TimeEnterInformationInFIS { get; set; }

        /// <summary>
        /// Письмо о переносе сроков (в виде байтов)
        /// </summary>
        public byte[] LetterToReschedule { get; set; }

        /// <summary>
        /// Письмо о переносе сроков (имя)
        /// </summary>
        public string LetterToRescheduleName { get; set; }

        /// <summary>
        /// Письмо о переносе сроков (тип файла)
        /// </summary>
        public string LetterToRescheduleContentType { get; set; }

        /// <summary>
        /// Схема подключения
        /// </summary>
        public CatalogElement ConnectionScheme { get; set; }

        /// <summary>
        /// Статус подключения
        /// </summary>
        public CatalogElement ConnectionStatus { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public string KPP { get; set; }

        /// <summary>
        /// Свидетельство об аккредитации
        /// </summary>
        public string AccreditationSertificate { get; set; }

        /// <summary>
        /// Количество мест, выделенных для приема на очно-заочную форму обучения
        /// </summary>
        public int CNEvening { get; set; }

        /// <summary>
        /// Контрольные цифры приема граждан, обучающихся за счет средств федерального бюджета
        /// </summary>
        public int CNFederalBudget { get; set; }

        /// <summary>
        /// Количество мест, выделенных для приема на очную форму обучения
        /// </summary>
        public int CNFullTime { get; set; }

        /// <summary>
        /// Объем и структура приема обучающихся за счет средств бюджета субъектов РФ
        /// </summary>
        public int CNLocalBudget { get; set; }

        /// <summary>
        /// Количество мест для обучения на основе договоров с оплатой стоимости обучения
        /// </summary>
        public int CNPaying { get; set; }

        /// <summary>
        /// Количество мест, выделенных для приема на заочную форму обучения
        /// </summary>
        public int CNPostal { get; set; }

        /// <summary>
        /// Квоты по целевому приему
        /// </summary>
        public int CNTargeted { get; set; }

        /// <summary>
        /// Gets or sets CreateDate.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Фактическая дата изменения статуса
        /// </summary>
        public DateTime? DateChangeStatus { get; set; }

        /// <summary>
        /// Полное название учредителя
        /// </summary>
        public string DepartmentFullName { get; set; }

        /// <summary>
        /// Код учредителя
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Сокращенное название учредителя
        /// </summary>
        public string DepartmentShortName { get; set; }

        /// <summary>
        /// ФИО руководителя
        /// </summary>
        public string DirectorFullName { get; set; }

        /// <summary>
        /// ФИО руководителя в род. падеже
        /// </summary>
        public string DirectorFullNameInGenetive { get; set; }

        /// <summary>
        /// Имя руководителя
        /// </summary>
        public string DirectorFirstName { get; set; }

        /// <summary>
        /// Фамилия руководителя
        /// </summary>
        public string DirectorLastName { get; set; }

        /// <summary>
        /// Отчество руководителя
        /// </summary>
        public string DirectorPatronymicName { get; set; }

        /// <summary>
        /// Должность руководителя
        /// </summary>
        public string DirectorPosition { get; set; }

        /// <summary>
        /// Должность руководителя в род. падеже
        /// </summary>
        public string DirectorPositionInGenetive { get; set; }

        /// <summary>
        /// Подтверждение
        /// </summary>
        public bool OUConfirmation { get; set; }

        /// <summary>
        /// Gets or sets EMail.
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Фактический адрес 
        /// </summary>
        public string FactAddress { get; set; }

        /// <summary>
        /// Gets or sets Fax.
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets INN.
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; } 
        
        /// <summary>
        /// Является филиалом
        /// </summary>
        public bool IsFilial { get; set; }

        /// <summary>
        /// Организационно-правовая форма (государственный, негосударственный)
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Вид (институт, университет, академия...)
        /// </summary>
        /// 
        public string AnotherName { get; set; }
        /// <summary>
        /// Другая ИС
        /// </summary>
        /// 

        public CatalogElement Kind
        {
            get
            {
                return this.Kind_;
            }

            set
            {
                this.Kind_ = value;
            }
        }

        /// <summary>
        /// ИС ОО (Галактика, 1С, ...)
        /// </summary>
        public CatalogElement IS
        {
            get
            {
                return this.IS_;
            }

            set
            {
                this.IS_ = value;
            }
        }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LawAddress { get; set; }

        /// <summary>
        /// Город\населеднный пункт
        /// </summary>
        public string TownName { get; set; }

        /// <summary>
        /// Полное название головной организации
        /// </summary>
        public string MainFullName { get; set; }

        /// <summary>
        /// Код головной организации
        /// </summary>
        public int? MainId { get; set; }

        /// <summary>
        /// Сокращенное название головной организации
        /// </summary>
        public string MainShortName { get; set; }

        /// <summary>
        /// Полное название новой организации
        /// </summary>
        public string NewOrgFullName { get; set; }

        /// <summary>
        /// Id организации, в которую была реорганизована текущаю организация
        /// </summary>
        public int? NewOrgId { get; set; }

        /// <summary>
        /// Сокращенное название новой организации
        /// </summary>
        public string NewOrgShortName { get; set; }

        /// <summary>
        /// Gets or sets OGRN.
        /// </summary>
        public string OGRN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAgreedTimeConnection { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool? IsAgreedTimeEnterInformation { get; set; }

        /// <summary>
        /// Тип (ВУЗ, ССУЗ, РЦОИ, ОУО...)
        /// </summary>
        public CatalogElement OrgType
        {
            get
            {
                return this.OrgType_;
            }

            set
            {
                this.OrgType_ = value;
            }
        }

        /// <summary>
        /// Ведомственная принадлежность
        /// </summary>
        public string OwnerDepartment { get; set; }

        /// <summary>
        /// Gets or sets Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Телефонный код города
        /// </summary>
        public string PhoneCityCode { get; set; }

        /// <summary>
        /// Gets or sets RCDescription.
        /// </summary>
        public string RCDescription
        {
            get
            {
                return this._rcDescription;
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    this._rcDescription = null;
                }
                else
                {
                    this._rcDescription = value;
                }
            }
        }

        /// <summary>
        /// Код модели приемной кампании выбирается из справочника
        /// </summary>
        public int? RCModelId { get; set; }

        /// <summary>
        /// Название модели приемной кампании
        /// </summary>
        public string RCModelName { get; set; }

        /// <summary>
        /// Обоснование для изменённого статуса
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets Region.
        /// </summary>
        public CatalogElement Region
        {
            get
            {
                return this.Region_;
            }

            set
            {
                this.Region_ = value;
            }
        }

        /// <summary>
        /// Gets or sets ShortName.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Сайт организации
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Прием по результатам ЕГЭ
        /// </summary>
        public int? ReceptionOnResultsCNE { get; set; }

        /// <summary>
        /// статус
        /// </summary>
        public CatalogElement Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
            }
        }

        /// <summary>
        /// Gets or sets UpdateDate.
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// версия организации
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Код организации в системе ИСЛОД
        /// </summary>
        public string ISLOD_GUID { get; set; }

        public string LicenseRegNumber { get; set; }
        public DateTime? LicenseOrderDocumentDate { get; set; }
        public string LicenseStatusName { get; set; }

        /// <summary>
        /// FIS-1777 - added by akopylov 30.10.2017
        /// </summary>
        public string SupplementNumber { get; set; }
        public DateTime? SupplementOrderDocumentDate { get; set; }
        public string SupplementStatusName { get; set; }

        #endregion
    }
}