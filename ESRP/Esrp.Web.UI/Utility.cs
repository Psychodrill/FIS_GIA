namespace Esrp.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;
    using Esrp.Web.Administration.Organizations;

    /// <summary>
    /// The utility.
    /// </summary>
    public static class Utility
    {
        // Словарь для генерации пароля
        #region Constants and Fields

        private const string PasswordChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890abcdefghijklmnopqrstuvwxyz";

        private const string PasswordDefaultString = "**********";

        // ключи сессии
        private const string SessionPasswordKey = "password";

        private const string UsersAndPasswords = "usersandpasswords";

        private static readonly byte[] TripleIV = new byte[] { 12, 67, 145, 98, 73, 201, 32, 1 };

        private static readonly byte[] TripleKey = new byte[]
            {
               77, 34, 90, 190, 234, 155, 89, 91, 0, 1, 76, 73, 55, 23, 76, 71, 40, 167, 211, 194, 86, 1, 30, 61 
            };

        #endregion

        #region Enums

        [Flags]
        private enum MimeFlags
        {
            /// <summary>
            /// No flags specified
            /// </summary>
            None = 0
        }

        #endregion

        #region Properties

        private static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        private static HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        private static IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        private static string UserLogin
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add user registration document.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="ContentType">
        /// The content type.
        /// </param>
        /// <param name="Extension">
        /// The extension.
        /// </param>
        /// <param name="Data">
        /// The data.
        /// </param>
        /// <param name="forceSend">
        /// The force send.
        /// </param>
        public static void AddUserRegistrationDocument(
            string userLogin, string ContentType, string Extension, byte[] Data, bool forceSend)
        {
            UserAccount user = UserAccount.GetUserAccount(userLogin);
            if (user != null)
            {
                user.RegistrationDocument.ContentType = ContentType;
                user.RegistrationDocument.Extension = Extension;
                user.RegistrationDocument.Document = Data;

                // Сохраню статус
                UserAccount.UserAccountStatusEnum prevStatus = user.Status;

                user.RegistrationDocument.Update();

                // Если статус изменился на "на согласовании" - отправлю уведомление
                if (user.Status == UserAccount.UserAccountStatusEnum.Consideration
                    && (forceSend || user.Status != prevStatus))
                {
                    SendNotification(user, EmailTemplateTypeEnum.Consideration);
                }
            }
        }

        /// <summary>
        /// Формирование метапеременных для замены в email сообщении. Замена производится в полях 
        /// subject и body.
        /// </summary>
        /// <param name="user">
        /// Объект UserAccount
        /// </param>
        /// <param name="password">
        /// Пароль
        /// </param>
        /// <param name="serverUrl">
        /// Url server-а
        /// </param>
        public static Pair[] CollectEmailMetaVariables(UserAccount user, string password, string serverUrl)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##ADMINCOMMENT##", user.AdminComment));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// Формирование метапеременных для замены в email сообщении. Замена производится в полях 
        /// subject и body.
        /// </summary>
        /// <param name="user">
        /// Объект UserAccount
        /// </param>
        /// <param name="password">
        /// Пароль
        /// </param>
        /// <param name="serverUrl">
        /// Url server-а
        /// </param>
        public static Pair[] CollectEmailMetaVariables(UserAccount user, string password, string serverUrl, string resetPasswordLink)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            @params.Add(new Pair("##RESETLINK##", resetPasswordLink));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##ADMINCOMMENT##", user.AdminComment));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// Формирование метапеременных для замены в email сообщении. Замена производится в полях 
        /// subject и body.
        /// </summary>
        /// <param name="user">
        /// Объект OrgUser
        /// </param>
        /// <param name="password">
        /// Пароль
        /// </param>
        /// <param name="serverUrl">
        /// Url server-а
        /// </param>
        public static Pair[] CollectEmailMetaVariables(OrgUser user, string password, string serverUrl)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            //no need
            @params.Add(new Pair("##LOGIN##", user.login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.lastName));
            @params.Add(new Pair("##FIRSTNAME##", user.firstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.patronymicName));
            @params.Add(new Pair("##EMAIL##", user.email));
            @params.Add(new Pair("##ADMINCOMMENT##", string.Empty));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        
        /// <summary>
        /// Формирование метапеременных для замены в email сообщении. Замена производится в полях 
        /// subject и body.
        /// </summary>
        /// <param name="user">
        /// Объект UserAccount
        /// </param>
        public static Pair[] CollectEmailMetaVariables(UserAccount user)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##ADMINCOMMENT##", user.AdminComment));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables(OrgUser user)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##LOGIN##", user.login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.lastName));
            @params.Add(new Pair("##FIRSTNAME##", user.firstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.patronymicName));
            @params.Add(new Pair("##EMAIL##", user.email));
            @params.Add(new Pair("##ADMINCOMMENT##", string.Empty));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables(OrgUserBrief user)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.MiddleName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##ADMINCOMMENT##", user.AdminComment));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="serverUrl">
        /// The server url.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables(IntrantAccount user, string password, string serverUrl)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="serverUrl">
        /// The server url.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables(IntrantAccount user, string password, string serverUrl, string resetPasswordLink)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            @params.Add(new Pair("##RESETLINK##", resetPasswordLink));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }
        /// <summary>
        /// The collect email meta variables.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables(IntrantAccount user)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.LastName));
            @params.Add(new Pair("##FIRSTNAME##", user.FirstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.PatronymicName));
            @params.Add(new Pair("##EMAIL##", user.Email));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables_ admin.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="serverUrl">
        /// The server url.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables_Admin(UserAccount user, string serverUrl)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##LOGIN##", user.Login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##USERPHONE##", user.Phone));
            @params.Add(new Pair("##EMAIL##", user.Email));

            @params.Add(new Pair("##ORGNAME##", user.OrganizationName));
            if (user.EducationInstitutionTypeId.HasValue)
            {
                @params.Add(new Pair("##ORGTYPE##", "Не определён"));
            }
            else
            {
                @params.Add(new Pair("##ORGTYPE##", "Не определён"));
            }

            if (user.OrganizationRegionId.HasValue && ((int)user.OrganizationRegionId > 0))
            {
                @params.Add(new Pair("##ORGREGION##", Region.Get((int)user.OrganizationRegionId).Name));
            }
            else
            {
                @params.Add(new Pair("##ORGREGION##", "Не определён"));
            }

            @params.Add(new Pair("##ORGFOUNDER##", user.OrganizationFounderName));
            @params.Add(new Pair("##ORGADDRESS##", user.OrganizationAddress));
            @params.Add(new Pair("##ORGCHIEFNAME##", user.OrganizationChiefName));
            @params.Add(new Pair("##ORGFAX##", user.OrganizationFax));
            @params.Add(new Pair("##ORGPHONE##", user.OrganizationPhone));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// The collect email meta variables_ admin.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="serverUrl">
        /// The server url.
        /// </param>
        /// <returns>
        /// </returns>
        public static Pair[] CollectEmailMetaVariables_Admin(OrgUser user, string serverUrl)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##LOGIN##", user.login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##USERPHONE##", user.phone));
            @params.Add(new Pair("##EMAIL##", user.email));

            @params.Add(new Pair("##ORGNAME##", user.RequestedOrganization.FullName));
            if (user.RequestedOrganization.OrgType.Id >= 0)
            {
                @params.Add(new Pair("##ORGTYPE##", user.RequestedOrganization.OrgType.Name));
            }
            else
            {
                @params.Add(new Pair("##ORGTYPE##", "Не определён"));
            }

            if (user.RequestedOrganization.Region.Id >= 0)
            {
                @params.Add(new Pair("##ORGREGION##", user.RequestedOrganization.Region.Name));
            }
            else
            {
                @params.Add(new Pair("##ORGREGION##", "Не определён"));
            }

            @params.Add(new Pair("##ORGFOUNDER##", user.RequestedOrganization.OwnerDepartment));
            @params.Add(new Pair("##ORGADDRESS##", user.RequestedOrganization.FactAddress));
            @params.Add(new Pair("##ORGCHIEFNAME##", user.RequestedOrganization.DirectorFullName));
            @params.Add(new Pair("##ORGFAX##", user.RequestedOrganization.Fax));
            @params.Add(new Pair("##ORGPHONE##", user.RequestedOrganization.Phone));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }

        /// <summary>
        /// Создание, готового для применения, CustomValidator-а
        /// </summary>
        /// <param name="errorMessage">
        /// Сообщение об ошибка, отображаемое валидатором
        /// </param>
        public static CustomValidator CreateErrorValidator(string errorMessage)
        {
            var validator = new CustomValidator();
            validator.EnableClientScript = false;
            validator.Display = ValidatorDisplay.None;
            validator.IsValid = false;
            validator.ErrorMessage = errorMessage;

            return validator;
        }

        /// <summary>
        /// Determines the Multipurpose Internet Mail Extensions (MIME) type from the data provided.
        /// </summary>
        /// <param name="url">
        /// String value that contains the URL of the data. This can be set to null if buffer contains the data to be sniffed.
        /// </param>
        /// <param name="buffer">
        /// Buffer containing the data to be sniffed. This can be set to null if url contains a valid URL.
        /// </param>
        /// <param name="mimeProposed">
        /// String value containing the proposed MIME type. This can be set to null.
        /// </param>
        /// <returns>
        /// String value containing the suggested MIME type
        /// </returns>
        public static string FindMimeFromData(string url, byte[] buffer, string mimeProposed)
        {
            const int maxLen = 255;
            string result;
            int hr;
            int bufLen = 0;

            if (buffer != null)
            {
                bufLen = buffer.Length < maxLen ? buffer.Length : maxLen;
            }

            hr = FindMimeFromData(new IntPtr(0), url, buffer, bufLen, mimeProposed, MimeFlags.None, out result, 0);

            if (result == null)
            {
                return "application/octet-stream";
            }

            return result;
        }

        /// <summary>
        /// The find site map node from resource key.
        /// </summary>
        /// <param name="resourceKey">
        /// The resource key.
        /// </param>
        /// <returns>
        /// </returns>
        public static SiteMapNode FindSiteMapNodeFromResourceKey(string resourceKey)
        {
            return FindSiteMapNodeFromResourceKey(resourceKey, SiteMap.RootNode);
        }

        /// <summary>
        /// The find site map node from resource key.
        /// </summary>
        /// <param name="resourceKey">
        /// The resource key.
        /// </param>
        /// <param name="rootNode">
        /// The root node.
        /// </param>
        /// <returns>
        /// </returns>
        public static SiteMapNode FindSiteMapNodeFromResourceKey(string resourceKey, SiteMapNode rootNode)
        {
            foreach (SiteMapNode node in rootNode.ChildNodes)
            {
                if (node.ResourceKey == resourceKey)
                {
                    return node;
                }
                else
                {
                    SiteMapNode temp = FindSiteMapNodeFromResourceKey(resourceKey, node);
                    if (temp != null)
                    {
                        return temp;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Преобразование килобайт в мегабайты с постледющим форматирование 
        /// </summary>
        /// <param name="value">
        /// Значение в килобайтах
        /// </param>
        /// <param name="format">
        /// Формат результата
        /// </param>
        /// <returns>
        /// The format kb to mb.
        /// </returns>
        public static string FormatKbToMb(int value, string format)
        {
            if (value == 0)
            {
                return "0";
            }

            return (value / 1024).ToString(format);
        }

        /// <summary>
        /// Генерация пароля заданной длины
        /// </summary>
        /// <remarks>
        /// Длина пароля определяется значением параметра MinRequiredPasswordLength 
        /// секции membership в web.config
        /// </remarks>
        /// <returns>
        /// The generate password.
        /// </returns>
        public static string GeneratePassword()
        {
            var password = new StringBuilder();
            var rand = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
            for (int i = 1; i < Membership.MinRequiredPasswordLength; i++)
            {
                password.Append(PasswordChars[rand.Next(PasswordChars.Length)]);
            }

            return password.ToString();
        }

        /// <summary>
        /// The get node link address.
        /// </summary>
        /// <param name="currentNode">
        /// The current node.
        /// </param>
        /// <returns>
        /// The get node link address.
        /// </returns>
        public static string GetNodeLinkAddress(SiteMapNode currentNode)
        {
            // Если у узла задан url, то вернем его
            if (!string.IsNullOrEmpty(currentNode.GetActualUrl()))
            {
                return currentNode.GetActualUrl();
            }

            // Построю коллекцию разрешенных детей
            var allowedNodes = new SiteMapNodeCollection();
            foreach (SiteMapNode node in currentNode.ChildNodes)
            {
                if (ShowNodebyUserRoles(node.Roles))
                {
                    allowedNodes.Add(node);
                }
            }

            // Если разрешенных детей нет, то верну корень сайта
            if (allowedNodes.Count == 0)
            {
                return "/";
            }

            // Верну url первого разрешенного узла
            return GetNodeLinkAddress(allowedNodes[0]);
        }

        /// <summary>
        /// Получение пароля пользователя из сессии
        /// </summary>
        /// <returns>
        /// The get password from session.
        /// </returns>
        public static string GetPasswordFromSession()
        {
            if (Session[SessionPasswordKey] != null)
            {
                return TripleDesDecrypt((string)Session[SessionPasswordKey]);
            }

            return PasswordDefaultString;
        }

        /// <summary>
        /// The get sever path.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The get sever path.
        /// </returns>
        public static string GetSeverPath(HttpRequest request)
        {
            return request.Url.AbsoluteUri.Substring(
                0, request.Url.AbsoluteUri.Length - request.Url.PathAndQuery.Length);
        }

        /// <summary>
        /// Сохранение зашифрованного пароля пользователя в сессию
        /// </summary>
        /// <param name="password">
        /// Пароль
        /// </param>
        public static void SavePasswordToSession(string password)
        {
            Session[SessionPasswordKey] = TripleDesEncrypt(password);
        }

        /// <summary>
        /// Сохранения словаря: пользователь, пароль в сессию
        /// </summary>
        /// <param name="users">Словарь: пользователь, пароль</param>
        public static void SaveUsersAndPasswordsToSession(Dictionary<string, string> users)
        {
            var result = users.ToDictionary(user => user.Key, user => TripleDesEncrypt(user.Value));
            Session[UsersAndPasswords] = result;
        }

        /// <summary>
        /// Получение словаря: пользователь, пароль из сессии
        /// </summary>
        /// <returns>словарь: пользователь, пароль</returns>
        public static Dictionary<string, string> GetUsersAndPasswords()
        {
            var result = new Dictionary<string, string>();
            if (Session[UsersAndPasswords] != null)
            {
                foreach (var user in (Dictionary<string, string>)Session[UsersAndPasswords])
                {
                    result.Add(user.Key, TripleDesDecrypt(user.Value));
                }
            }

            return result;
        }

        /// <summary>
        /// The send notification.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="emailTemplateType">
        /// The email template type.
        /// </param>
        public static void SendNotification(UserAccount user, EmailTemplateTypeEnum emailTemplateType)
        {
            // Подготовлю email сообщение 
            var template = new EmailTemplate(emailTemplateType);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.Email;
            message.Params = CollectEmailMetaVariables(user);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }

        /// <summary>
        /// The show nodeby user roles.
        /// </summary>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <returns>
        /// The show nodeby user roles.
        /// </returns>
        public static bool ShowNodebyUserRoles(IList roles)
        {
            if (roles.Count == 0)
            {
                return true;
            }

            foreach (string role in roles)
            {
                if (CheckCustomAppRule(role) || Account.CheckRole(UserLogin, role))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The show nodeby user roles.
        /// </summary>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <returns>
        /// The show nodeby user roles.
        /// </returns>
        public static bool ShowNodebyUserRoles(IList roles, IList notroles)
        {
            if (roles.Count == 0)
            {
                return true;
            }

            var isShow = false;
            foreach (string role in roles)
            {
                if (CheckCustomAppRule(role) || Account.CheckRole(UserLogin, role))
                {
                    isShow = true;
                }
            }
            if (notroles != null)
            {
                foreach (string role in notroles)
                {
                    if (CheckCustomAppRule(role) || Account.CheckRole(UserLogin, role))
                    {
                        isShow = false;
                    }
                }
            }

            return isShow;
        }

        private static bool CheckCustomAppRule(string role)
        {
            if (string.IsNullOrEmpty(role) || !role.StartsWith("CustomAppRule_"))
            {
                return false;
            }

            if (role == "CustomAppRule_IsUserFromMainOrg")
            {
                return GeneralSystemManager.CanViewSubordinateOrganizations(User.Identity.Name);
            }

            return false;
        }

        /// <summary>
        /// Декодирование пароля, взятого из сессии 
        /// </summary>
        /// <param name="passwordHash">
        /// Хэшированный пароль
        /// </param>
        /// <returns>
        /// Необходимо для отображения пароля пользователю во время первой сессии после регистрации 
        /// или изменеия пароля
        /// </returns>
        public static string TripleDesDecrypt(string passwordHash)
        {
            var des = new TripleDESCryptoServiceProvider();
            var outputStream = new MemoryStream();

            var cryptoStream = new CryptoStream(
                outputStream, des.CreateDecryptor(TripleKey, TripleIV), CryptoStreamMode.Write);

            byte[] dataToDecrypt = HttpUtility.UrlDecodeToBytes(Convert.FromBase64String(passwordHash));
            cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
            cryptoStream.Close();

            byte[] decryptedData = outputStream.ToArray();
            outputStream.Close();

            return BytesToString(decryptedData);
        }

        /// <summary>
        /// Кодирование пароля для передачи в сессию 
        /// </summary>
        /// <param name="password">
        /// </param>
        /// <returns>
        /// Необходимо для отображения пароля пользователю во время первой сессии после регистрации 
        /// или изменеия пароля
        /// </returns>
        public static string TripleDesEncrypt(string password)
        {
            var des = new TripleDESCryptoServiceProvider();
            var outputStream = new MemoryStream();

            var cryptoStream = new CryptoStream(
                outputStream, des.CreateEncryptor(TripleKey, TripleIV), CryptoStreamMode.Write);

            byte[] bytes = StringToBytes(password);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.Close();

            byte[] encryptedData = outputStream.ToArray();
            outputStream.Close();

            return Convert.ToBase64String(HttpUtility.UrlEncodeToBytes(encryptedData));
        }

        #endregion

        // Преобразование строки в массив байтов

        // Преобразования массива байтов в строку
        #region Methods

        private static string BytesToString(byte[] bytes)
        {
            var enc = new UnicodeEncoding();
            return enc.GetString(bytes);
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Auto)]
        private static extern int FindMimeFromData(
            IntPtr pBC,
            string url,
            byte[] buffer,
            int bufferSize,
            string pwzMimeProposed,
            MimeFlags mimeFlags,
            out string ppwzMimeOut,
            int dwReserved);

        private static byte[] StringToBytes(string str)
        {
            var enc = new UnicodeEncoding();
            return enc.GetBytes(str);
        }

        #endregion

        public static Pair[] CollectEmailMetaVariables(OrgUser user, string password, string serverUrl, string resetPasswordLink)
        {
            var @params = new List<Pair>();
            @params.Add(new Pair("##DATE##", DateTime.Now.ToString("yyyy.MM.dd HH:mm")));
            @params.Add(new Pair("##PASSWORD##", password));
            @params.Add(new Pair("##RESETLINK##", resetPasswordLink));
            @params.Add(new Pair("##LOGIN##", user.login));
            @params.Add(new Pair("##FULLNAME##", user.GetFullName()));
            @params.Add(new Pair("##LASTNAME##", user.lastName));
            @params.Add(new Pair("##FIRSTNAME##", user.firstName));
            @params.Add(new Pair("##PATRONYMICNAME##", user.patronymicName));
            @params.Add(new Pair("##EMAIL##", user.email));
            @params.Add(new Pair("##ADMINCOMMENT##", user.AdminComment));
            @params.Add(new Pair("##SERVERURL##", serverUrl));
            @params.Add(new Pair("##FBDNAME##", GeneralSystemManager.GetSystemName(3)));
            @params.Add(new Pair("##FBSNAME##", GeneralSystemManager.GetSystemName(2)));
            @params.Add(new Pair("##ESRPNAME##", GeneralSystemManager.GetSystemName(1)));

            return @params.ToArray();
        }
    }
}