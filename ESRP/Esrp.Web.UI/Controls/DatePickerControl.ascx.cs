using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.ComponentModel;

namespace Esrp.Web.Controls
{
    [ControlValueProperty("Value")]

    [DefaultProperty("Value")]
    [SupportsEventValidation]
    [ValidationProperty("Value")]
    public partial class DatePickerControl : System.Web.UI.UserControl
    {

        public string ValidationGroup
        {
            get { return this.RegularExpressionValidator1.ValidationGroup; }
            set
            { this.RegularExpressionValidator1.ValidationGroup = value; this.cv1.ValidationGroup = value; }

        }
        public string _format = "dd.MM.yyyy";

        /// <summary>
        /// Получает или задает формат времени для отображения
        /// </summary>
        public string Format
        {
            get
            {
                return this._format;
            }
            set
            {
                this._format = value;
            }
        }

        /// <summary>
        /// Минимальная дата
        /// </summary>
        public DateTime MinDate
        {
            get;
            set;
        }

        /// <summary>
        /// Максимальная дата
        /// </summary>
        public DateTime MaxDate
        {
            get;
            set;
        }

        /// <summary>
        /// Получает или задает значение
        /// </summary>
        public DateTime? Value
        {
            get
            {
                DateTime value = DateTime.MinValue;
                if (String.IsNullOrEmpty(this.datePickerBox.Text))
                    return null;
                DateTime.TryParseExact(this.datePickerBox.Text, this.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
                return value;
            }
            set
            {
                this.datePickerBox.Text = !value.HasValue ? "" :value.Value.ToString(this.Format);
            }
        }

        public void ValidateRange(object sender, ServerValidateEventArgs e)
        {
            CustomValidator cv = (CustomValidator)sender;

            if (!this.Value.HasValue)
            {
                e.IsValid = true;
                return;
            }

            if (this.MaxDate != null && this.MaxDate != DateTime.MinValue && this.Value > this.MaxDate)
            {
                cv.ErrorMessage = "Дата должна быть меньше чем " + this.MaxDate.ToShortDateString();
                e.IsValid = false;
            }
            if (this.MinDate != null && this.MinDate != DateTime.MinValue && this.Value < this.MinDate)
            {
                cv.ErrorMessage = "Дата должна быть больше чем " + this.MinDate.ToShortDateString();
                e.IsValid = false;
            }
        }
    }
}
