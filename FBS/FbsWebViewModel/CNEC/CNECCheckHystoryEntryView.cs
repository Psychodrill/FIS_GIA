namespace FbsWebViewModel.CNEC
{
    using System;

    /// <summary>
    /// проверка сертификата организацией 
    /// </summary>
    public class CNECCheckHystoryEntryView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CNECCheckHystoryEntryView"/> class.
        /// </summary>
        /// <param name="checkHistoryView">
        /// история всех проверок организацией
        /// </param>
        public CNECCheckHystoryEntryView(CNECCheckHistoryView checkHistoryView)
        {
            this.CheckHistoryView = checkHistoryView;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// тип проверки
        /// </summary>
        public CNECCheckType CheckType { get; set; }

        /// <summary>
        /// тип проверки для вывода в UI
        /// </summary>
        public string CheckTypeView
        {
            get
            {
                switch (this.CheckType)
                {
                    case CNECCheckType.Batch:
                        return "Пакетная проверка";
                    case CNECCheckType.Interactive:
                        return "Интерактивная проверка";
                    default:
                        throw new NotSupportedException(
                            string.Format("Тип проверки '{0}' не может быть обработан", this.CheckType.ToString()));
                }
            }
        }

        /// <summary>
        /// история всех проверок организацией
        /// </summary>
        public CNECCheckHistoryView CheckHistoryView { get; set; }

        /// <summary>
        /// дата проверки
        /// </summary>
        public DateTime? Date { get; set; }

        #endregion
    }
}