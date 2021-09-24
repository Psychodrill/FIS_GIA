namespace Esrp.Core.RegistrationTemplates
{
    using System;
    using System.Collections.Generic;

    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using FogSoft.Helpers;

    /// <summary>
    /// The registration template factory.
    /// </summary>
    public class RegistrationTemplateFactory
    {
        #region Public Methods

        /// <summary>
        /// Создание документа с заявкой
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Документ с заявкой</returns>
        public static string GetWordDocument(string login, string virPath)
        {
            var registrationSystemsTemplate = new RegistrationSystemsTemplate();

            // получить текущего пользователя
            OrgUser user = OrgUserDataAccessor.Get(login);

            if (!user.RequestedOrganization.OrganizationId.HasValue)
            {
                throw new Exception(
                    "При формировании шаблона возникла ошибка. Заявка подается без указания организации.");
            }

            // документы выдаем в зависимости от данных заявки: регистрация на системы, на одного или нескольких пользователей
            OrgRequest lastOrgRequest = OrgRequestManager.GetLastRequest(
                login, user.RequestedOrganization.OrganizationId.Value);

            return registrationSystemsTemplate.GetDocument(lastOrgRequest, virPath);
        }

        /// <summary>
        /// The get document.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public static CustomTeplateBaseWordDocument GetDocument(string login)
        {
            // получить текущего пользователя
            OrgUser user = OrgUserDataAccessor.Get(login);

            if (!user.RequestedOrganization.OrganizationId.HasValue)
            {
                throw new Exception(
                    "При формировании шаблона возникла ошибка. Заявка подается без указания организации.");
            }

            // документы выдаем в зависимости от данных заявки: регистрация на системы, на одного или нескольких пользователей
            OrgRequest lastOrgRequest = OrgRequestManager.GetLastRequest(
                login, user.RequestedOrganization.OrganizationId.Value);

            // заявка на одного пользователя
            if (lastOrgRequest.LinkedUsers.Count == 1)
            {
                OrgUserBrief linkedUser = lastOrgRequest.LinkedUsers[0];
                if (linkedUser.HasAccessToFbd && !linkedUser.HasAccessToFbs
                    || !linkedUser.HasAccessToFbd && linkedUser.HasAccessToFbs)
                {
                    // ФБС
                    if (linkedUser.HasAccessToFbs)
                    {
                        return new SystemRegistrationTemplate(
                            user, 
                            SystemKind.Fbs, 
                            "Прошу зарегистрировать пользователя для работы с автоматизированной информационной системой «Федеральная база свидетельств о результатах единого государственного экзамена» ("
                            + GeneralSystemManager.GetSystemName(2) + ").");
                    }

                    // ФБД
                    switch (linkedUser.Status)
                    {
                        case UserAccount.UserAccountStatusEnum.Registration:
                        case UserAccount.UserAccountStatusEnum.Consideration:
                        case UserAccount.UserAccountStatusEnum.Revision:

                            // активация
                            if (lastOrgRequest.IsForActivation)
                            {
                                return new SystemRegistrationTemplate(
                                    user, 
                                    SystemKind.Fbd, 
                                    "Прошу активировать учетную запись уполномоченного сотрудника по работе с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                                    + GeneralSystemManager.GetSystemName(3) + "»)");
                            }

                            // смена
                            OrgUser currActiveStaff =
                                OrgUserDataAccessor.GetActivatedAuthorizedStaffForOrg(
                                    user.RequestedOrganization.OrganizationId.Value, SystemKind.Fbd);
                            if (currActiveStaff != null)
                            {
                                return new FbdChangeUserTemplate(user, currActiveStaff);
                            }

                            // все остальное
                            return new SystemRegistrationTemplate(
                                user, 
                                SystemKind.Fbd, 
                                "Прошу зарегистрировать пользователя для работы с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                                + GeneralSystemManager.GetSystemName(3) + "»)");
                    }
                }
                    
                    // заявка от одного пользователя на две системы 
                else if (linkedUser.HasAccessToFbd && linkedUser.HasAccessToFbs)
                {
                    var systemKinds = new List<SystemKind>(new[] { SystemKind.Fbs, SystemKind.Fbd });
                    switch (linkedUser.Status)
                    {
                        case UserAccount.UserAccountStatusEnum.Registration:
                        case UserAccount.UserAccountStatusEnum.Consideration:
                        case UserAccount.UserAccountStatusEnum.Revision:

                            // активация
                            if (lastOrgRequest.IsForActivation)
                            {
                                return new SystemRegistrationTemplate(
                                    user, 
                                    systemKinds, 
                                    "Прошу зарегистрировать пользователя для работы с автоматизированной информационной системой «Федеральная база свидетельств о результатах единого государственного экзамена» ("
                                    + GeneralSystemManager.GetSystemName(2)
                                    +
                                    "), а также активировать учетную запись уполномоченного сотрудника по работе с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                                    + GeneralSystemManager.GetSystemName(3) + "»)");
                            }

                            // смена
                            OrgUser currActiveStaff =
                                OrgUserDataAccessor.GetActivatedAuthorizedStaffForOrg(
                                    user.RequestedOrganization.OrganizationId.Value, SystemKind.Fbd);
                            if (currActiveStaff != null)
                            {
                                return new FbsUserFbdChangeUserTemplate(user, user, currActiveStaff);
                            }

                            // все остальное
                            return new SystemRegistrationTemplate(
                                user, 
                                systemKinds, 
                                "Прошу зарегистрировать пользователя для работы с автоматизированной информационной системой «Федеральная база свидетельств о результатах единого государственного экзамена» ("
                                + GeneralSystemManager.GetSystemName(2)
                                +
                                "), а также с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                                + GeneralSystemManager.GetSystemName(3) + "»)");
                    }
                }
            }
                
                // заявка на двух разных пользователей - один регистрируется на доступ к ФБС, другой - к ФБД
            else if (lastOrgRequest.LinkedUsers.Count == 2)
            {
                OrgUser fbsUser, fbdUser;
                if (lastOrgRequest.LinkedUsers[0].HasAccessToFbs)
                {
                    fbsUser = OrgUserDataAccessor.Get(lastOrgRequest.LinkedUsers[0].Login);
                    fbdUser = OrgUserDataAccessor.Get(lastOrgRequest.LinkedUsers[1].Login);
                }
                else
                {
                    fbsUser = OrgUserDataAccessor.Get(lastOrgRequest.LinkedUsers[1].Login);
                    fbdUser = OrgUserDataAccessor.Get(lastOrgRequest.LinkedUsers[0].Login);
                }

                switch (user.status)
                {
                    case UserAccount.UserAccountStatusEnum.Registration:
                    case UserAccount.UserAccountStatusEnum.Consideration:
                    case UserAccount.UserAccountStatusEnum.Revision:

                        // активация
                        if (lastOrgRequest.IsForActivation)
                        {
                            return new TwoUsersForSystemsTemplate(
                                fbsUser, 
                                fbdUser, 
                                "Прошу зарегистрировать пользователя для работы с автоматизированной информационной системой «Федеральная база свидетельств о результатах единого государственного экзамена» ("
                                + GeneralSystemManager.GetSystemName(2)
                                +
                                "), а также активировать учетную запись уполномоченного сотрудника по работе с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                                + GeneralSystemManager.GetSystemName(3) + "»)");
                        }

                        // смена
                        OrgUser currActiveStaff =
                            OrgUserDataAccessor.GetActivatedAuthorizedStaffForOrg(
                                user.RequestedOrganization.OrganizationId.Value, SystemKind.Fbd);
                        if (currActiveStaff != null)
                        {
                            return new FbsUserFbdChangeUserTemplate(fbsUser, fbdUser, currActiveStaff);
                        }

                        // все остальное
                        return new TwoUsersForSystemsTemplate(
                            fbsUser, 
                            fbdUser, 
                            "Прошу зарегистрировать пользователей для работы с автоматизированной информационной системой «Федеральная база свидетельств о результатах единого государственного экзамена» ("
                            + GeneralSystemManager.GetSystemName(2)
                            +
                            "), а также с федеральной информационной системой обеспечения проведения единого государственного экзамена и приема граждан в образовательные учреждения СПО и образовательные учреждения ВПО («"
                            + GeneralSystemManager.GetSystemName(3) + "»)");
                }
            }

            throw new Exception("Не найден шаблон заявки для пользователя с логином: {0}".FormatWith(login));
        }

        #endregion
    }
}