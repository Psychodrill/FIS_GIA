namespace Esrp.Web.ViewModel.InformationSystems
{
    using System.Drawing;

    /// <summary>
    /// Вьюшка для регистрации пользователя в ИС
    /// </summary>
    public class InformationSystemsRegistrationView
    {
        #region Public Properties

        /// <summary>
        /// E-mail (будет являться логином)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Короткое наименование ИС
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Идентификатор ИС
        /// </summary>
        public int SystemId { get; set; }

        private bool _accessToSystem = true;

        /// <summary>
        /// Доступ к ФБС
        /// </summary>
        public bool AccessToSystem
        {
            get
            {
                return this._accessToSystem;
            }

            set
            {
                this._accessToSystem = value;
            }
        }

        /// <summary>
        /// Доступ к ФИС ЕГЭ и приема
        /// </summary>
        public bool AccessToFbd { get; set; }

        /// <summary>
        /// Показывать или не показывать checkBox "Совпадает с ФБС" для ИС 
        /// </summary>
        public bool SameDataAsFbsVisible { get; set; }

        private bool _emailEnable = true;

        /// <summary>
        /// E-mail (Изменяяемое или нет)
        /// </summary>
        public bool EmailEnable
        {
            get
            {
                return this._emailEnable;
            }

            set
            {
                this._emailEnable = value;
            }
        }

        private bool _fullNameEnable = true;

        /// <summary>
        /// ФИО (Изменяяемое или нет)
        /// </summary>
        public bool FullNameEnable
        {
            get
            {
                return this._fullNameEnable;
            }

            set
            {
                this._fullNameEnable = value;
            }
        }

        private bool _phoneEnable = true;

        /// <summary>
        /// Телефон (Изменяяемое или нет)
        /// </summary>
        public bool PhoneEnable
        {
            get
            {
                return this._phoneEnable;
            }

            set
            {
                this._phoneEnable = value;
            }
        }

        private bool _positionEnable = true;

        /// <summary>
        /// Должность (Изменяяемое или нет)
        /// </summary>
        public bool PositionEnable
        {
            get
            {
                return this._positionEnable;
            }

            set
            {
                this._positionEnable = value;
            }
        }

        private bool _accessToSystemEnable = true;

        /// <summary>
        /// Доступ к ИС (Изменяяемое или нет)
        /// </summary>
        public bool AccessToSystemEnable
        {
            get
            {
                return this._accessToSystemEnable;
            }

            set
            {
                this._accessToSystemEnable = value;
            }
        }

        private bool _sameDataAsFbsEnable = true;

        /// <summary>
        /// checkBox "Совпадает с ФБС" для ИС (Включено или нет)
        /// </summary>
        public bool SameDataAsFbsEnable
        {
            get
            {
                return this._sameDataAsFbsEnable;
            }

            set
            {
                this._sameDataAsFbsEnable = value;
            }
        }

        private bool _sameDataAsFbs = true;

        /// <summary>
        /// checkBox "Совпадает с ФБС"
        /// </summary>
        public bool SameDataAsFbs
        {
            get
            {
                return this._sameDataAsFbs;
            }

            set
            {
                this._sameDataAsFbs = value;
            }
        }

        /// <summary>
        /// RequiredFieldValidator для ФИО (Включено или нет)
        /// </summary>
        public bool RequiredFieldFullNameEnable { get; set; }

        /// <summary>
        /// RequiredFieldValidator для E-mail (Включено или нет)
        /// </summary>
        public bool RequiredFieldEmailEnable { get; set; }

        /// <summary>
        /// RegularExpressionValidator для Телефон (Включено или нет)
        /// </summary>
        public bool RegularExpressionPhone { get; set; }

        /// <summary>
        /// RegularExpressionValidator для E-mail (Включено или нет)
        /// </summary>
        public bool RegularExpressionEmailEnable { get; set; }

        /// <summary>
        /// свойство ReadOnly для ФИО
        /// </summary>
        public bool ReadOnlyFullName { get; set; }

        /// <summary>
        /// свойство ReadOnly для E-mail
        /// </summary>
        public bool ReadOnlyEmail { get; set; }

        /// <summary>
        /// свойство ReadOnly для Должность  
        /// </summary>
        public bool ReadOnlyPosition { get; set; }

        /// <summary>
        /// свойство ReadOnly для Телефон
        /// </summary>
        public bool ReadOnlyPhone { get; set; }

        /// <summary>
        /// свойство BackColor для ФИО
        /// </summary>
        public Color BackColorFullName
        {
            get
            {
                return this.ReadOnlyFullName ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        /// <summary>
        /// свойство BackColor для E-mail
        /// </summary>
        public Color BackColorEmail
        {
            get
            {
                return this.ReadOnlyEmail ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        /// <summary>
        /// свойство BackColor для Должность
        /// </summary>
        public Color BackColorPosition
        {
            get
            {
                return this.ReadOnlyPosition ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        /// <summary>
        /// свойство BackColor для Телефон
        /// </summary>
        public Color BackColorPhone
        {
            get
            {
                return this.ReadOnlyPhone ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        public bool UserPhoneVisible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive1
        /// </summary>
        public bool trStatActive1Visible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive2
        /// </summary>
        public bool trStatActive2Visible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive3
        /// </summary>
        public bool trStatActive3Visible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive4
        /// </summary>
        public bool trStatActive4Visible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive5
        /// </summary>
        public bool trStatActive5Visible { get; set; }

        /// <summary>
        /// Visible поля с id trStatActive5ForCb
        /// </summary>
        public bool trStatActive5ForCbVisible { get; set; }

        /// <summary>
        /// Visible поля с id trFbdChangeReadonlyAuthorizedStaff
        /// </summary>
        public bool trFbdChangeReadonlyAuthorizedStaffVisible { get; set; }

        private bool _sameDataAsFbsForChangeEnable = true;

        /// <summary>
        /// "Совпадает с" для нового пользователя  ФБС (Enable/Disable)
        /// </summary>
        public bool SameDataAsFbsForChangeEnable
        {
            get
            {
                return this._sameDataAsFbsForChangeEnable;
            }

            set
            {
                this._sameDataAsFbsForChangeEnable = value;
            }
        }

        private bool _sameDataAsForChangeFbs = true;

        /// <summary>
        /// "Совпадает с" (Checked)
        /// </summary>
        public bool SameDataAsForChangeFbs
        {
            get
            {
                return this._sameDataAsForChangeFbs;
            }

            set
            {
                this._sameDataAsForChangeFbs = value;
            }
        }

        /// <summary>
        /// Надпись для ФБС
        /// </summary>
        public string lFbdInfo { get; set; }

        /// <summary>
        /// ФИО (для ФИС ЕГЭ и приема, данные о текущем сотруднике)
        /// </summary>
        public string CurFullName { get; set; }

        /// <summary>
        /// Должность (для ФИС ЕГЭ и приема, данные о текущем сотруднике)
        /// </summary>
        public string CurPosition { get; set; }

        /// <summary>
        /// E-mail (для ФИС ЕГЭ и приема, данные о текущем сотруднике)
        /// </summary>
        public string CurEmail { get; set; }

        /// <summary>
        /// свойство ReadOnly ФИО (для ФИС ЕГЭ и приема, данные о текущем сотруднике)
        /// </summary>
        public bool ReadOnlyCurFullName { get; set; }

        /// <summary>
        /// свойство ReadOnly E-mail (для ФИС ЕГЭ и приема, данные о текущем сотруднике)
        /// </summary>
        public bool ReadOnlyCurEmail { get; set; }

        /// <summary>
        /// свойство ReadOnly Должность (для ФИС ЕГЭ и приема, данные о текущем сотруднике) 
        /// </summary>
        public bool ReadOnlyCurPosition { get; set; }

        /// <summary>
        /// свойство BackColor для ФИО
        /// </summary>
        public Color BackColorCurFullName
        {
            get
            {
                return this.ReadOnlyCurFullName ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        /// <summary>
        /// свойство BackColor для E-mail
        /// </summary>
        public Color BackColorCurEmail
        {
            get
            {
                return this.ReadOnlyCurEmail ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        /// <summary>
        /// свойство BackColor для Должность
        /// </summary>
        public Color BackColorCurPosition
        {
            get
            {
                return this.ReadOnlyCurPosition ? Color.LightGray : Color.FromArgb(235, 243, 246);
            }
        }

        #endregion
    }
}