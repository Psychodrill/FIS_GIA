namespace Esrp.Web.Controls
{
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Web.ViewModel.Users;

    /// <summary>
    /// The users info.
    /// </summary>
    public partial class UsersInfo : UserControl
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets RequestID.
        /// </summary>
        public int RequestID { get; set; }

        #endregion

        protected void UsersByRequestOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["requestID"] = this.RequestID;
        }

        protected void UsersByRequestOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            foreach (var userView in (List<UserViewForRequest>)e.ReturnValue)
            {
                userView.StatusText = UserAccountExtentions.GetUserAccountNewStatusName(userView.Status);
            }
        }
    }
}