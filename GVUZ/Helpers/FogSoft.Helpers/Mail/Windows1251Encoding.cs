using System.Text;

namespace FogSoft.Helpers.Mail
{
    /// <summary>
    ///     Fixes microsoft bug with <see cref="BodyName" />.
    /// </summary>
    public class Windows1251Encoding : EncodingWrapper
    {
        public static readonly Encoding Current = new Windows1251Encoding();

        private Windows1251Encoding() : base(GetEncoding(1251))
        {
        }

        public override string BodyName
        {
            get { return "windows-1251"; }
        }
    }
}