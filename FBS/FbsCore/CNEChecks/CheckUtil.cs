namespace Fbs.Core.CNEChecks
{
    using Fbs.Utility;

    /// <summary>
    /// различные вспомогательные методы для проверок
    /// </summary>
    public class CheckUtil
    {
        #region Static Fields

        private static string CHECK_TOKEN = "23FJASVOJWCQBECQ9ENWADCAWE9JCABWECAWEI9URWEBDCVAS";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The verify check hash.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="certNumber">
        /// The cert number.
        /// </param>
        /// <param name="hash">
        /// The hash.
        /// </param>
        /// <returns>
        /// The verify check hash.
        /// </returns>
        public static bool VerifyCheckHash(string userLogin, string certNumber, string hash)
        {
            return Hasher.VerifyMd5Hash(string.Format("{0}{1}{2}", userLogin, certNumber, CHECK_TOKEN), hash);
        }

        public static string GetCheckHash(string userLogin, string certNumber)
        {
            return Hasher.GetMd5Hash(string.Format("{0}{1}{2}", userLogin, certNumber, CHECK_TOKEN));
        }

        #endregion
    }
}