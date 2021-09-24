using WebControls;

namespace Esrp.Web.Controls
{
    public class SortRef_Prefix : SortRef
    {
        private string m_Prefix = "";
        public string Prefix
        {
            get { return m_Prefix; }
            set
            {
                m_Prefix = value;
                DescImageUrl = m_Prefix + DescImageUrl;
                AscImageUrl = m_Prefix + AscImageUrl;
            }
        }
    }
}