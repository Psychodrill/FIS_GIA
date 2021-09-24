namespace Esrp.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Web;
    using System.Web.Management;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using FbsUtility;

    /// <summary>
    /// The message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Простое сообщения для пользователя.
        /// </summary>
        Message, 

        /// <summary>
        /// Предупреждени для пользователя.
        /// </summary>
        Warning, 

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        Error, 

        /// <summary>
        /// Сообщение о системной ошибке.
        /// </summary>
        SystemError
    }

    /// <summary>
    /// Проедоставляет простой механизм работы с сообщениями, которые необходимо показывать пользователю.
    /// </summary>
    public class MessageControl : WebControl
    {
        #region Constants and Fields

        /// <summary>
        /// The content.
        /// </summary>
        private readonly Literal Content = new Literal();

        #endregion

        #region Delegates

        /// <summary>
        /// The activate handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        public delegate void ActivateHandler(MessageControl sender);

        #endregion

        #region Public Events

        /// <summary>
        /// Событие, происходящее в случае добавления нового сообщения.
        /// </summary>
        public event ActivateHandler Activate;

        #endregion

        #region Public Properties

        /// <summary>
        /// Адрес картинки, которой помечаются сообщения об ошибках.
        /// </summary>
        public string ErrorUrl
        {
            get
            {
                return this.ViewState["ErrorUrl"] as string;
            }

            set
            {
                this.ViewState["ErrorUrl"] = value;
            }
        }

        /// <summary>
        /// Адрес картинки, которой помечаются простые сообщения.
        /// </summary>
        public string MessageUrl
        {
            get
            {
                return this.ViewState["MessageUrl"] as string;
            }

            set
            {
                this.ViewState["MessageUrl"] = value;
            }
        }

        /// <summary>
        /// Сообщение, которое будет показано пользователю в случае обнаружения системной ошибки.
        /// </summary>
        public string SystemErrorText
        {
            get
            {
                return this.ViewState["SystemErrorText"] as string;
            }

            set
            {
                this.ViewState["SystemErrorText"] = value;
            }
        }

        /// <summary>
        /// Адрес картинки, которой помечаются системные ошибки.
        /// </summary>
        public string SystemErrorUrl
        {
            get
            {
                return this.ViewState["SystemErrorUrl"] as string;
            }

            set
            {
                this.ViewState["SystemErrorUrl"] = value;
            }
        }

        /// <summary>
        /// Адрес картинки, которой помечаются предупреждения.
        /// </summary>
        public string WarningUrl
        {
            get
            {
                return this.ViewState["WarningUrl"] as string;
            }

            set
            {
                this.ViewState["WarningUrl"] = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Messages.
        /// </summary>
        private List<Pair<MessageType, string>> Messages
        {
            get
            {
                object objMsg = this.Context.Items[this.UniqueID + "Messages"];
                if (objMsg == null)
                {
                    this.Context.Items[this.UniqueID + "Messages"] = new List<Pair<MessageType, string>>();
                }

                return this.Context.Items[this.UniqueID + "Messages"] as List<Pair<MessageType, string>>;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Добавляет сообщение об ошибке.
        /// </summary>
        /// <param name="type">
        /// Тип ошибки.
        /// </param>
        /// <param name="message">
        /// Текст сообщения.
        /// </param>
        public void Add(MessageType type, string message)
        {
            this.Messages.Add(new Pair<MessageType, string>(type, message));
            this.Visible = true;
            this.OnActivate();
        }

        /// <summary>
        /// Добавляет сообщение об ошибки.
        /// </summary>
        /// <param name="type">
        /// Тип ошибки.
        /// </param>
        /// <param name="ex">
        /// Объект типа <see cref="Exception"/> описывающий ошибку.
        /// </param>
        public void Add(MessageType type, Exception ex)
        {
            this.Add(type, Informer.GetExceptionInfo(ex));

            WebBaseEvent.Raise(new HandeledWebErrorEvent("Handeled Exception", this, 100001, ex));
        }

        /// <summary>
        /// Удаляет все накопленные ранее сообщения всех типов.
        /// </summary>
        public void Clear()
        {
            this.Messages.Clear();
        }

        /// <summary>
        /// Показывает, имеются или нет сообщения об ошибках определенного типа.
        /// </summary>
        /// <param name="type">
        /// Типа сообщения.
        /// </param>
        /// <returns>
        /// The contain.
        /// </returns>
        public bool Contain(MessageType type)
        {
            List<Pair<MessageType, string>> matches =
                this.Messages.FindAll(delegate(Pair<MessageType, string> value) { return value.First == type; });
            return matches != null && matches.Count > 0;
        }

        /// <summary>
        /// Принудительный вызов события <see cref="Activate"/>.
        /// </summary>
        public void Refresh()
        {
            this.OnActivate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var pnl = new Panel();
            pnl.Style["padding"] = "5px";
            pnl.Style["padding-left"] = "10px";

            pnl.Controls.Add(this.Content);
            this.Controls.Add(pnl);
            //this.Controls.Add(CreateFooter());
        }

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.Visible = this.Messages.Count != 0;
        }

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            this.Visible = this.Messages.Count != 0;

            var sb = new StringBuilder();
            bool showSystemErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowSystemError"]);

            List<Pair<MessageType, string>> systemErrors = this.GetMessages(MessageType.SystemError);
            if (systemErrors != null && systemErrors.Count > 0)
            {
                if (showSystemErrors)
                {
                    AddBeginTags(sb, this.SystemErrorUrl);
                    sb.AppendFormat("{0}:</br>", this.SystemErrorText);
                    AddContent(sb, systemErrors);
                    AddEndTags(sb);
                }
                else
                {
                    sb.Append(this.SystemErrorText);
                }
            }

            this.RenderMesages(sb, MessageType.Error, this.ErrorUrl);
            this.RenderMesages(sb, MessageType.Warning, this.WarningUrl);
            this.RenderMesages(sb, MessageType.Message, this.MessageUrl);

            this.Content.Text = sb.ToString();

            this.Clear();

            base.OnPreRender(e);
        }

        /// <summary>
        /// The add begin tags.
        /// </summary>
        /// <param name="sb">
        /// The sb.
        /// </param>
        /// <param name="imgUrl">
        /// The img url.
        /// </param>
        private static void AddBeginTags(StringBuilder sb, string imgUrl)
        {
            sb.Append("<table width='100%' border='0'>");
            sb.Append("<tr>");
/*            sb.Append("<td valign='middle'>");
            sb.AppendFormat("<img src='{0}' />", VirtualPathUtility.ToAbsolute(imgUrl));
            sb.Append("</td>");*/
            sb.Append("<td valign='middle' width='100%' style='text-align:left;'>");
        }

        /// <summary>
        /// The add content.
        /// </summary>
        /// <param name="sb">
        /// The sb.
        /// </param>
        /// <param name="array">
        /// The array.
        /// </param>
        private static void AddContent(StringBuilder sb, IList<Pair<MessageType, string>> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                sb.Append("&nbsp;&nbsp;");
                sb.Append("•&nbsp;");
                sb.Append(array[i].Second);
                sb.Append("<br>");
            }
        }

        /// <summary>
        /// The add end tags.
        /// </summary>
        /// <param name="sb">
        /// The sb.
        /// </param>
        private static void AddEndTags(StringBuilder sb)
        {
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
        }

        /// <summary>
        /// The create footer.
        /// </summary>
        /// <returns>
        /// </returns>
        private static Control CreateFooter()
        {
            var tab = new Table();
            tab.CellPadding = 0;
            tab.CellSpacing = 0;
            var trow = new TableRow();
            var c1 = new TableCell();
            var img1 = new Image();
            img1.ImageUrl = "~/Images/MessageControl/footer_left.gif";
            c1.Controls.Add(img1);
            trow.Cells.Add(c1);
            var c2 = new TableCell();
            c2.Width = Unit.Percentage(100);
            c2.Attributes.Add("background", VirtualPathUtility.ToAbsolute("~/Images/MessageControl/footer_bg.gif"));
            trow.Cells.Add(c2);
            var c3 = new TableCell();
            var img2 = new Image();
            img2.ImageUrl = "~/Images/MessageControl/footer_right.gif";
            c3.Controls.Add(img2);
            trow.Cells.Add(c3);
            tab.Rows.Add(trow);
            return tab;
        }

        /// <summary>
        /// The get messages.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private List<Pair<MessageType, string>> GetMessages(MessageType type)
        {
            return this.Messages.FindAll(delegate(Pair<MessageType, string> value) { return value.First == type; });
        }

        /// <summary>
        /// The on activate.
        /// </summary>
        private void OnActivate()
        {
            if (this.Activate != null)
            {
                this.Activate(this);
            }
        }

        /// <summary>
        /// The render mesages.
        /// </summary>
        /// <param name="sb">
        /// The sb.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="imgUrl">
        /// The img url.
        /// </param>
        private void RenderMesages(StringBuilder sb, MessageType type, string imgUrl)
        {
            List<Pair<MessageType, string>> messages = this.GetMessages(type);
            if (messages != null && messages.Count > 0)
            {
                AddBeginTags(sb, imgUrl);
                AddContent(sb, messages);
                AddEndTags(sb);
            }
        }

        #endregion
    }
}