namespace Fbs.Core.Organizations
{
    using System;
    using System.Data;

    using Fbs.Core.CatalogElements;

    /// <summary>
    /// Организация из справочника организаций
    /// </summary>
    public class Organization
    {
        #region Constants and Fields

        private string factAddress;

        private CatalogElement kind = new CatalogElement();

        private CatalogElement orgType = new CatalogElement();

        private CatalogElement region = new CatalogElement();

        private CatalogElement status = new CatalogElement();

        /// <summary>
        /// Заполняется, если выбрана "Другая модель приемной кампании"
        /// </summary>
        private string rcDescription;

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
        /// <param name="isPrivate">
        /// The is private.
        /// </param>
        /// <param name="INN_In">
        /// The in n_ in.
        /// </param>
        /// <param name="OGRN_In">
        /// The ogr n_ in.
        /// </param>
        public Organization(
            string fullName, int regionId, int typeId, int kindId, bool isPrivate, string INN_In, string OGRN_In, int statusId)
        {
            this.FullName = fullName;
            this.Region = new CatalogElement(regionId);
            this.OrgType = new CatalogElement(typeId);
            this.Kind = new CatalogElement(kindId);
            this.Status = new CatalogElement(statusId);
            this.IsPrivate = isPrivate;
            this.INN = INN_In;
            this.OGRN = OGRN_In;
        }

        internal Organization()
        {
        }

        internal Organization(IDataReader reader)
        {
            this.Id = Convert.ToInt32(reader[OrganizationTableColumns.Id]);
            this.FullName = reader[OrganizationTableColumns.FullName].ToString();
            this.ShortName = reader[OrganizationTableColumns.ShortName].ToString();
            this.IsPrivate = Convert.ToBoolean(reader[OrganizationTableColumns.IsPrivate]);
            this.IsFilial = Convert.ToBoolean(reader[OrganizationTableColumns.IsFilial]);
            this.INN = reader[OrganizationTableColumns.INN].ToString();
            this.OGRN = reader[OrganizationTableColumns.OGRN].ToString();
            this.OwnerDepartment = reader[OrganizationTableColumns.OwnerDepartment].ToString();
            this.DirectorFullName = reader[OrganizationTableColumns.DirectorFullName].ToString();
            this.DirectorPosition = reader[OrganizationTableColumns.DirectorPosition].ToString();
            this.AccreditationSertificate =
                reader[OrganizationTableColumns.AccreditationSertificate].ToString();
            this.LawAddress = reader[OrganizationTableColumns.LawAddress].ToString();
            this.FactAddress = reader[OrganizationTableColumns.FactAddress].ToString();
            this.PhoneCityCode = reader[OrganizationTableColumns.PhoneCityCode].ToString();
            this.Phone = reader[OrganizationTableColumns.Phone].ToString();
            this.Fax = reader[OrganizationTableColumns.Fax].ToString();
            this.EMail = reader[OrganizationTableColumns.EMail].ToString();
            this.Site = reader[OrganizationTableColumns.Site].ToString();
            this.Region = new CatalogElement(
                reader, OrganizationTableColumns.RegionId, OrganizationTableColumns.RegionName);
            this.OrgType = new CatalogElement(
                reader, OrganizationTableColumns.TypeId, OrganizationTableColumns.TypeName);
            this.Kind = new CatalogElement(
                reader, OrganizationTableColumns.KindId, OrganizationTableColumns.KindName);

            // Модель приемной кампании
            this.RCModelId = (int)reader[OrganizationTableColumns.RCModelId];
            this.RCModelName = reader[OrganizationTableColumns.RCModelName].ToString();
            this.RCDescription = reader[OrganizationTableColumns.RCDescription] == DBNull.Value
                                     ? string.Empty
                                     : reader[OrganizationTableColumns.RCDescription].ToString();

            // Сведения об объеме и структуре приема
            this.CNFBFullTime = (int)reader[OrganizationTableColumns.CNFBFullTime];
            this.CNFBEvening = (int)reader[OrganizationTableColumns.CNFBEvening];
            this.CNFBPostal = (int)reader[OrganizationTableColumns.CNFBPostal];
            this.CNPayFullTime = (int)reader[OrganizationTableColumns.CNPayFullTime];
            this.CNPayEvening = (int)reader[OrganizationTableColumns.CNPayEvening];
            this.CNPayPostal = (int)reader[OrganizationTableColumns.CNPayPostal];

            this.MainId = reader[OrganizationTableColumns.MainId] == DBNull.Value
                              ? (int?)null
                              : (int)reader[OrganizationTableColumns.MainId];

            this.MainFullName = reader[OrganizationTableColumns.MainFullName].ToString();
            this.MainShortName = reader[OrganizationTableColumns.MainShortName].ToString();

            // Сведения об учредителе организации
            this.DepartmentId = reader[OrganizationTableColumns.DepartmentId] == DBNull.Value
                                    ? (int?)null
                                    : (int)reader[OrganizationTableColumns.DepartmentId];
            this.DepartmentFullName = reader[OrganizationTableColumns.DepartmentFullName].ToString();
            this.DepartmentShortName = reader[OrganizationTableColumns.DepartmentShortName].ToString();

            this.UpdateDate = (DateTime)reader[OrganizationTableColumns.UpdateDate];

            this.NewOrgId = reader[OrganizationTableColumns.NewOrgId] == DBNull.Value
                    ? (int?)null
                    : (int)reader[OrganizationTableColumns.NewOrgId];

            this.NewOrgFullName = reader[OrganizationTableColumns.NewOrgFullName].ToString();
            this.NewOrgShortName = reader[OrganizationTableColumns.NewOrgShortName].ToString();
            this.Status = new CatalogElement(reader, OrganizationTableColumns.StatusId, OrganizationTableColumns.StatusName);
            this.Version = (int)reader[OrganizationTableColumns.Version];

            this.DisableLog = reader[OrganizationTableColumns.DisableLog] != DBNull.Value && (bool)reader[OrganizationTableColumns.DisableLog];
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Свидетельство об аккредитации
        /// </summary>
        public string AccreditationSertificate { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся за счет бюджета по очно-заочной форме обучения
        /// </summary>
        public int CNFBEvening { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся за счет бюджета по очной форме обучения
        /// </summary>
        public int CNFBFullTime { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся за счет бюджета по заочной форме обучения
        /// </summary>
        public int CNFBPostal { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
        /// установленное учредителем по очно-заочной форме обучения
        /// </summary>
        public int CNPayEvening { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
        /// установленное учредителем по очной форме обучения
        /// </summary>
        public int CNPayFullTime { get; set; }

        /// <summary>
        /// Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
        /// установленное учредителем по заочной форме обучения
        /// </summary>
        public int CNPayPostal { get; set; }

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
        /// Должность руководителя
        /// </summary>
        public string DirectorPosition { get; set; }

        /// <summary>
        /// Gets or sets EMail.
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Фактический адрес 
        /// </summary>
        public string FactAddress
        {
            get
            {
                if (this.factAddress == string.Empty)
                {
                    return null;
                }

                return this.factAddress;
            }

            set
            {
                this.factAddress = value;
            }
        }

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
        /// Отключить журналирование
        /// = true, отключено
        /// = false, включено
        /// </summary>
        public bool DisableLog { get; set; }

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
        public CatalogElement Kind
        {
            get
            {
                return this.kind;
            }

            set
            {
                this.kind = value;
            }
        }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LawAddress { get; set; }

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
        /// Gets or sets OGRN.
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Тип (ВУЗ, ССУЗ, РЦОИ, ОУО...)
        /// </summary>
        public CatalogElement OrgType
        {
            get
            {
                return this.orgType;
            }

            set
            {
                this.orgType = value;
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
                return this.rcDescription;
            }

            set
            {
                if (value.Trim().Length == 0)
                {
                    this.rcDescription = null;
                }
                else
                {
                    this.rcDescription = value;
                }
            }
        }

        /// <summary>
        /// Код модели приемной кампании выбирается из справочника
        /// </summary>
        public int RCModelId { get; set; }

        /// <summary>
        /// Название модели приемной кампании
        /// </summary>
        public string RCModelName { get; set; }

        /// <summary>
        /// Gets or sets Region.
        /// </summary>
        public CatalogElement Region
        {
            get
            {
                return this.region;
            }

            set
            {
                this.region = value;
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
        /// Дата обновления записи об организации
        /// </summary>
        public DateTime UpdateDate { get; set; }

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
        /// версия организации
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}