/****** Object:  StoredProcedure [dbo].[aspnet_AnyDataInTables]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_AnyDataInTables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_AnyDataInTables]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Applications_CreateApplication]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Applications_CreateApplication]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_CheckSchemaVersion]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_CheckSchemaVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_CreateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_CreateUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_FindUsersByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetAllUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetPassword]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetPasswordWithFormat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_GetUserByUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_ResetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_SetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_SetPassword]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UnlockUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Membership_UpdateUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Paths_CreatePath]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Paths_CreatePath]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Paths_CreatePath]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Personalization_GetApplicationId]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Personalization_GetApplicationId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_DeleteAllState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_FindState]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_FindState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_GetCountOfState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_ResetSharedState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetUserState]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAdministration_ResetUserState]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_GetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_PersonalizationPerUser_SetPageSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteInactiveProfiles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_DeleteInactiveProfiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteProfiles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_DeleteProfiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProfiles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetProfiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProperties]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_GetProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_GetProperties]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_SetProperties]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Profile_SetProperties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Profile_SetProperties]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_RegisterSchemaVersion]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_RegisterSchemaVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_CreateRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_CreateRole]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_DeleteRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_GetAllRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_GetAllRoles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Roles_RoleExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Roles_RoleExists]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RemoveAllRoleMembers]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Setup_RemoveAllRoleMembers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RestorePermissions]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Setup_RestorePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UnRegisterSchemaVersion]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UnRegisterSchemaVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Users_CreateUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users_CreateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Users_CreateUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Users_DeleteUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_Users_DeleteUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_Users_DeleteUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_AddUsersToRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_FindUsersInRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetRolesForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_GetUsersInRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_IsUserInRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_WebEvent_LogEvent]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[aspnet_WebEvent_LogEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
GO

/****** Object:  StoredProcedure [dbo].[blk_DeleteApplicationPackage]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_DeleteApplicationPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[blk_DeleteApplicationPackage]
GO

/****** Object:  StoredProcedure [dbo].[blk_PrepareToImportApplicationPackage]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_PrepareToImportApplicationPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[blk_PrepareToImportApplicationPackage]
GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_ProcessApplicationBulkedPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[blk_ProcessApplicationBulkedPackage]
GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessConsideredApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_ProcessConsideredApplicationBulkedPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[blk_ProcessConsideredApplicationBulkedPackage]
GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessRecommendedApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_ProcessRecommendedApplicationBulkedPackage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[blk_ProcessRecommendedApplicationBulkedPackage]
GO

/****** Object:  StoredProcedure [dbo].[CopyStructureToAdmission]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CopyStructureToAdmission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CopyStructureToAdmission]
GO

/****** Object:  StoredProcedure [dbo].[CountryType_Transfer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryType_Transfer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CountryType_Transfer]
GO

/****** Object:  StoredProcedure [dbo].[dba_indexDefragStandard_sp]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dba_indexDefragStandard_sp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[dba_indexDefragStandard_sp]
GO

/****** Object:  StoredProcedure [dbo].[DeleteApplications_fromXml]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteApplications_fromXml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteApplications_fromXml]
GO

/****** Object:  StoredProcedure [dbo].[Direction_Transfer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Direction_Transfer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Direction_Transfer]
GO

/****** Object:  StoredProcedure [dbo].[FormOfLaw_Transfer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormOfLaw_Transfer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FormOfLaw_Transfer]
GO

/****** Object:  StoredProcedure [dbo].[gvuz_ValidateOtherApplicationsCount]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gvuz_ValidateOtherApplicationsCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gvuz_ValidateOtherApplicationsCount]
GO

/****** Object:  StoredProcedure [dbo].[internal_AddCreatedAndModifiedDate]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[internal_AddCreatedAndModifiedDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[internal_AddCreatedAndModifiedDate]
GO

/****** Object:  StoredProcedure [dbo].[Okato_Transfer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Okato_Transfer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Okato_Transfer]
GO

/****** Object:  StoredProcedure [dbo].[PerformSearch]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerformSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PerformSearch]
GO

/****** Object:  StoredProcedure [dbo].[ProfessionType_Transfer]    Script Date: 05/29/2014 13:42:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProfessionType_Transfer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ProfessionType_Transfer]
GO

/****** Object:  StoredProcedure [dbo].[PublishAdmissionData]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PublishAdmissionData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PublishAdmissionData]
GO

/****** Object:  StoredProcedure [dbo].[PublishAdmissionStructure]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PublishAdmissionStructure]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PublishAdmissionStructure]
GO

/****** Object:  StoredProcedure [dbo].[SendApplication]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SendApplication]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SendApplication]
GO

/****** Object:  StoredProcedure [dbo].[sys_DisableAllIndexes]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_DisableAllIndexes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_DisableAllIndexes]
GO

/****** Object:  StoredProcedure [dbo].[sys_DropColumnIndexes]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_DropColumnIndexes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_DropColumnIndexes]
GO

/****** Object:  StoredProcedure [dbo].[sys_DropColumnStatisticts]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_DropColumnStatisticts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_DropColumnStatisticts]
GO

/****** Object:  StoredProcedure [dbo].[sys_EnableAllIndexes]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_EnableAllIndexes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sys_EnableAllIndexes]
GO

/****** Object:  StoredProcedure [dbo].[test_PerformSearch]    Script Date: 05/29/2014 13:42:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[test_PerformSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[test_PerformSearch]
GO

/****** Object:  StoredProcedure [dbo].[aspnet_AnyDataInTables]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N'aspnet_Membership'
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N'aspnet_Roles'
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N'aspnet_Profile'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N'aspnet_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N'aspnet_WebEvent_Events'
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N'aspnet_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N'aspnet_Applications'
            RETURN
        END
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Applications_CreateApplication]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_CheckSchemaVersion]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        -- select user ID from aspnet_users table
        SELECT TOP 1 @UserId = u.UserId
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId

        SELECT m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut, m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  @UserId = u.UserId AND u.UserId = m.UserId 
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Paths_CreatePath]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM dbo.aspnet_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT dbo.aspnet_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM dbo.aspnet_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Personalization_GetApplicationId]    Script Date: 05/29/2014 13:42:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM aspnet_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM aspnet_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_FindState]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM dbo.aspnet_Paths Paths,
             ((SELECT Paths.PathId
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM dbo.aspnet_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM aspnet_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetUserState]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationAllUsers p WHERE p.PathId = @PathId
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteInactiveProfiles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT  0
        RETURN
    END

    DELETE
    FROM    dbo.aspnet_Profile
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    dbo.aspnet_Users u
                WHERE   ApplicationId = @ApplicationId
                        AND (LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteProfiles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
    @ApplicationName        nvarchar(256),
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N',', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC dbo.aspnet_Users_DeleteUser @ApplicationName, @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT 0
        RETURN
    END

    SELECT  COUNT(*)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
    WHERE   ApplicationId = @ApplicationId
        AND u.UserId = p.UserId
        AND (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProfiles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
        WHERE   ApplicationId = @ApplicationId
            AND u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProperties]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_GetProperties]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         dbo.aspnet_Profile
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Profile_SetProperties]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, @IsUserAnonymous, @LastActivityDate, @UserId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   dbo.aspnet_Profile
               WHERE  UserId = @UserId))
        UPDATE dbo.aspnet_Profile
        SET    PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString,
               PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO dbo.aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate)
             VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_RegisterSchemaVersion]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RemoveAllRoleMembers]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RestorePermissions]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UnRegisterSchemaVersion]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Users_CreateUser]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_Users_DeleteUser]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_UsersInRoles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO

/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
GO

/****** Object:  StoredProcedure [dbo].[aspnet_WebEvent_LogEvent]    Script Date: 05/29/2014 13:42:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        dbo.aspnet_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END
GO

/****** Object:  StoredProcedure [dbo].[blk_DeleteApplicationPackage]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[blk_DeleteApplicationPackage] @packageId INT
AS
	SET NOCOUNT ON	
	
	DELETE FROM blk_Application WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_ApplicationEntranceTestDocument WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_ApplicationSelectedCompetitiveGroup WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_ApplicationSelectedCompetitiveGroupItem WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_ApplicationSelectedCompetitiveGroupTarget WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_Entrant WHERE ImportPackageId = @packageId 	
	DELETE FROM blk_EntrantDocument WHERE ImportPackageId = @packageId 		
	DELETE FROM blk_EntrantDocumentEgeAndOlympicSubject WHERE ImportPackageId = @packageId 		
	DELETE FROM blk_ApplicationCompetitiveGroupItem WHERE ImportPackageId = @packageId 		

	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[blk_PrepareToImportApplicationPackage]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[blk_PrepareToImportApplicationPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	
	
	DECLARE @applicationIds TABLE (Id INT NOT NULL PRIMARY KEY)		
	DECLARE @entrantIds TABLE (Id INT NOT NULL)
	DECLARE @institutionId INT
		
	INSERT INTO @applicationIds
	SELECT a_db.ApplicationID
	FROM blk_Application a 
		JOIN dbo.[Application] a_db with (updlock, rowlock) ON 
			a_db.InstitutionID = a.InstitutionId AND
			a_db.ApplicationNumber = a.ApplicationNumber AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a.ImportPackageId = @packageId
			
	-- Выбираем все у которых нет связки с другими заявлениями
	DECLARE @entrantDocumentIds TABLE (Id INT NOT NULL)
	DECLARE @relatedDocumentIds TABLE (Id INT NOT NULL)
	-----------------------------------------------------------------------------
	-- все документы по заявлениям
	INSERT INTO @entrantDocumentIds	
	SELECT DISTINCT aed.EntrantDocumentID
	FROM @applicationIds a 
		JOIN ApplicationEntrantDocument aed with (updlock, rowlock) ON aed.ApplicationID = a.Id
		--JOIN EntrantDocument ed with (updlock, rowlock) on aed.EntrantDocumentID = ed.EntrantDocumentID
		--JOIN Entrant e with (updlock, rowlock) on e.IdentityDocumentID = ed.EntrantDocumentID
	UNION
	SELECT aed.EntrantDocumentID
	FROM  
		@applicationIds a 
		JOIN dbo.ApplicationEntranceTestDocument aed ON aed.ApplicationID = a.Id
	WHERE aed.EntrantDocumentID IS NOT NULL
			
	-- имеющие ссылки на другие заявления
	INSERT INTO @relatedDocumentIds	
	SELECT DISTINCT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntrantDocument aed with (updlock, rowlock) ON aed.EntrantDocumentID = A1.Id
		--JOIN Entrant e with (updlock, rowlock) on e.IdentityDocumentID = aed.EntrantDocumentID
	WHERE aed.ApplicationID NOT IN (SELECT Id from @applicationIds)				
	UNION
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntranceTestDocument aed ON aed.EntrantDocumentID = A1.Id
	WHERE 
		aed.EntrantDocumentID IS NOT NULL AND
		aed.ApplicationID NOT IN (SELECT Id from @applicationIds)	

	-- удаляем кросс документы
	DELETE ed FROM @entrantDocumentIds ed
		JOIN @relatedDocumentIds rd ON ed.Id = rd.Id
	
	-- лок
	declare @stub INT	
	SELECT @stub = COUNT(*) FROM
	EntrantDocument a with (updlock, rowlock) 
	join @entrantDocumentIds i on i.Id = a.EntrantDocumentID	
	
	SELECT @stub = COUNT(*) FROM dbo.Entrant WITH (UPDLOCK, ROWLOCK)	
	WHERE IdentityDocumentID IN (SELECT Id FROM @entrantDocumentIds)
		
	-----------------------------------------------------------------------------
	SELECT TOP 1 @institutionId = InstitutionId
	FROM blk_Application
	WHERE ImportPackageId = @packageId

	/*---------------------------------------------------------*
	 * ЛОГИРОВАНИЕ
	 *---------------------------------------------------------*/		
	--INSERT INTO [dbo].[PersonalDataAccessLog](
	--	[Method], 
	--	[OldData], 
	--	[NewData], 
	--	[ObjectType], 
	--	[AccessMethod], 
	--	[InstitutionID], 
	--	[UserLogin], 
	--	[ObjectID], 
	--	[AccessDate])
	--SELECT 
	--	'D',		
	--	'[{"ApplicationUID":"' + ISNULL(a_db.[UID], '') + 
	--	'","ApplicationNumber":"' + a_db.ApplicationNumber + 
	--	'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
	--		DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a_db.RegistrationDate)) AS VARCHAR) + 
	--	'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
	--	',"EntrantUID":"' + ISNULL(e.[UID], '') + 
	--	'","EntrantDocumentID":' + CAST(e.IdentityDocumentID AS VARCHAR) + 
	--	',"EntrantID":' + CAST(e.EntrantID AS VARCHAR) + '}]',		
	--	NULL,
	--	'Application',
	--	'ImportDeleteApplication',		
	--	a_db.InstitutionID,
	--	@userLogin,
	--	NULL,
	--	GETDATE()	
	--FROM 		
	--	dbo.[Application] a_db
	--	JOIN @applicationIds a ON a_db.ApplicationID = a.Id
	--	JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID	
	--WHERE a_db.InstitutionID = @institutionId				
	
	UPDATE dbo.Entrant with (updlock, rowlock) 
	SET IdentityDocumentID = NULL
	WHERE IdentityDocumentID IN (SELECT Id FROM @entrantDocumentIds)	
	
	DELETE a FROM [EntrantDocumentEgeAndOlympicSubject] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympicTotal] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympic] a WITH (UPDLOCK, ROWLOCK)	 
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEge] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEdu] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentDisability] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentCustom] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentIdentity] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [OrderOfAdmissionHistory] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntranceTestDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntrantDocument] a WITH (UPDLOCK, ROWLOCK)	
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [EntrantDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [ApplicationSelectedCompetitiveGroupTarget] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroupItem] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroup] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	
	delete a from dbo.ApplicationCompetitiveGroupItem a with (updlock, rowlock)
		join @applicationIds ai on ai.Id = a.ApplicationId 

	DELETE a FROM [dbo].[ApplicationConsidered] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	--INSERT INTO @entrantIds
	--SELECT e.EntrantID
	--FROM [dbo].[Application] a  
	--	JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	--	JOIN Entrant e ON a.EntrantID = e.EntrantID
	--WHERE a.InstitutionID = @institutionId AND
	--	NOT EXISTS(SELECT * FROM [EntrantDocument] ed WHERE ed.EntrantID = e.EntrantID)	
		
	DELETE a FROM [dbo].[Application] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	WHERE InstitutionID = @institutionId
		
	--/* Чистим энтрантов и персонов */
	--DELETE e FROM Entrant e WITH (UPDLOCK, ROWLOCK)
	--	JOIN @entrantIds ei ON ei.Id = e.EntrantID
	--WHERE 
	--	NOT EXISTS(SELECT * FROM [EntrantLanguage] el WHERE el.EntrantID = e.EntrantID) AND
	--	NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID)		

	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[blk_ProcessApplicationBulkedPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	
		
	declare @stub INT
	DECLARE @institutionId INT
	DECLARE @InsertedApplicationsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @InsertedEntrantsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @InsertedEntrantDocumentsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @FailedApplicationsUIDs TABLE ([UID] VARCHAR(200) NOT NULL) 	

	/*---------------------------------------------------------*
	 * Удаление старой информации по поступившим заявлениям		 
	 *---------------------------------------------------------*/
	EXEC dbo.[blk_PrepareToImportApplicationPackage] @packageId, @userLogin;
	 
	SELECT TOP 1 @institutionId = InstitutionId
	FROM blk_Application
	WHERE ImportPackageId = @packageId 
	 
	SELECT @stub = COUNT(*)
	FROM dbo.blk_Entrant e
	join Entrant e_db with (updlock, rowlock) on 
		e_db.InstitutionId = e.InstitutionId AND
		e_db.[UID] = e.[UID]
		JOIN dbo.GenderType gt with(nolock) ON e.GenderId = gt.GenderID
	WHERE e.ImportPackageId = @packageId 		
	 
	/*---------------------------------------------------------*
	 * Entrant
	 *---------------------------------------------------------*/		 
	----DISABLE TRIGGER trig_Entrant_CreatedDate ON Entrant; 
	----DISABLE TRIGGER trig_Entrant_ModifiedDate ON Entrant;	
	UPDATE dbo.Entrant
	SET 
		CustomInformation = e.CustomInformation,
		SNILS = e.Snils,
		LastName = e.LastName,
		FirstName = e.FirstName,
		MiddleName = e.MiddleName,
		GenderID = gt.GenderID,
		ModifiedDate = GETDATE()
	FROM dbo.blk_Entrant e
		JOIN dbo.GenderType gt ON e.GenderId = gt.GenderID
	WHERE 
		e.ImportPackageId = @packageId AND
		e.InstitutionId = dbo.Entrant.InstitutionID AND
		e.[UID] = dbo.Entrant.[UID]
	
	INSERT INTO dbo.Entrant (
		CustomInformation,
		SNILS,
		InstitutionID,
		[UID],
		LastName,
		FirstName,
		MiddleName,
		GenderID,
		CreatedDate,
		ModifiedDate)
	OUTPUT INSERTED.EntrantID INTO @InsertedEntrantsIds
	SELECT DISTINCT
		e.CustomInformation,
		e.Snils,
		e.InstitutionId,
		e.[UID],
		e.LastName,
		e.FirstName,
		e.MiddleName,
		gt.GenderID,
		GETDATE(),
		GETDATE()		
	FROM dbo.blk_Entrant e		
		JOIN dbo.GenderType gt ON e.GenderId = gt.GenderID		
		LEFT JOIN dbo.Entrant e_db ON 			
			e.InstitutionId = e_db.InstitutionID AND
			e.[UID] = e_db.[UID]
	WHERE 	
		e.ImportPackageId = @packageId AND
		e_db.EntrantID IS NULL;
	--ENABLE TRIGGER trig_Entrant_CreatedDate ON Entrant; 
	--ENABLE TRIGGER trig_Entrant_ModifiedDate ON Entrant;	

	---- лок
	--SELECT @stub = 1 FROM
	--Entrant a with (updlock, rowlock) 
	--join @InsertedEntrantsIds i on i.Id = a.EntrantID

	/*---------------------------------------------------------*
	 * Application
	 *---------------------------------------------------------*/	
	--DISABLE TRIGGER trig_Application_CreatedDate ON [Application]; 
	--DISABLE TRIGGER trig_Application_ModifiedDate ON [Application];	 
	INSERT INTO dbo.[Application] (
		EntrantID ,
		RegistrationDate ,
		InstitutionID ,
		NeedHostel ,
		StatusID ,
		StatusDecision ,		          		          
		SourceID ,		          
		ApplicationNumber ,
		OriginalDocumentsReceived ,
		OrderCompetitiveGroupID ,		          
		OrderOfAdmissionID ,		          
		OrderCompetitiveGroupItemID ,
		OrderCalculatedRating ,
		OrderCalculatedBenefitID ,
		OrderEducationFormID ,
		OrderEducationSourceID ,
		LastDenyDate ,
		[UID] ,
		IsRequiresBudgetO ,
		IsRequiresBudgetOZ ,
		IsRequiresBudgetZ ,
		IsRequiresPaidO ,
		IsRequiresPaidOZ ,
		IsRequiresPaidZ ,
		OriginalDocumentsReceivedDate ,
		LastEgeDocumentsCheckDate ,
		OrderCompetitiveGroupTargetID ,
		IsRequiresTargetO ,
		IsRequiresTargetOZ ,
		IsRequiresTargetZ ,
		ApplicationGUID,
		ApproveInstitutionCount,
		FirstHigherEducation,
		ApprovePersonalData,
		FamiliarWithLicenseAndRules,
		FamiliarWithAdmissionType,
		FamiliarWithOriginalDocumentDeliveryDate,
		WizardStepID,
		CreatedDate,
		ModifiedDate,
		[Priority])
	OUTPUT INSERTED.ApplicationID INTO @InsertedApplicationsIds
	SELECT 
		e.EntrantID,
		a.RegistrationDate,
		a.InstitutionId,
		a.NeedHostel,
		a.StatusId,
		a.StatusDecision,
		2, -- SourceID 
		a.ApplicationNumber,
		ISNULL(a.OriginalDocumentsReceived, 0), -- проставляется позже
		NULL ,    -- OrderCompetitiveGroupID - int ??? 
		a.OrderOfAdmissionId,
		NULL , -- OrderCompetitiveGroupItemID - int ???
		NULL , -- OrderCalculatedRating - decimal ???
        NULL , -- OrderCalculatedBenefitID - smallint ???
        NULL , -- OrderEducationFormID - smallint ???
        NULL , -- OrderEducationSourceID - smallint ???
		a.LastDenyDate,
		a.[UID],
		a.IsRequiresBudgetO,
		a.IsRequiresBudgetOZ,
		a.IsRequiresBudgetZ,
		a.IsRequiresPaidO,
		a.IsRequiresPaidOZ,
		a.IsRequiresPaidZ,
		a.OriginalDocumentsReceivedDate, -- проставляется позже
		NULL, -- LastEgeDocumentsCheckDate - datetime ???
		NULL, -- OrderCompetitiveGroupTargetID - int ???
		a.IsRequiresTargetO,
		a.IsRequiresTargetOZ,
		a.IsRequiresTargetZ,
		a.Id,
		1,1,1,1,1,1,
		2, -- WizardStepID
		GETDATE(),
		GETDATE(),
		a.[Priority]
	FROM 
		dbo.blk_Application a
		JOIN dbo.Entrant e ON
			e.[UID] = a.EntrantUID AND
			e.InstitutionId = a.InstitutionID
	WHERE a.ImportPackageId = @packageId;

	-- лок
	SELECT @stub = COUNT(*) FROM
	Application a with (updlock, rowlock) 
	join @InsertedApplicationsIds i on i.Id = a.ApplicationID	
	SELECT @stub = COUNT(*) FROM	
	Application a with (updlock, rowlock)
	JOIN ApplicationEntrantDocument aed with (updlock, rowlock) on a.ApplicationID = aed.ApplicationID
	join Entrant e with (updlock, rowlock) on e.IdentityDocumentID = aed.EntrantDocumentID  
	join @InsertedApplicationsIds i on i.Id = aed.ApplicationID

	/* Обновляем дату получения оригиналов */	
	UPDATE [dbo].[Application] 
	SET 
		[OriginalDocumentsReceived] = 1, 
		[OriginalDocumentsReceivedDate] = ed.OriginalReceivedDate,
		ModifiedDate = GETDATE()
	FROM dbo.blk_EntrantDocument ed
	WHERE 
		ed.ImportPackageId = @packageId AND
		ed.OriginalReceivedDate IS NOT NULL AND 
		[dbo].[Application].[ApplicationGUID] = ed.ParentId;
	--ENABLE TRIGGER trig_Application_CreatedDate ON [Application]; 
	--ENABLE TRIGGER trig_Application_ModifiedDate ON [Application];	 

	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroup
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroup (
		ApplicationID ,
		CompetitiveGroupID ,
		CalculatedBenefitID ,
		CalculatedRating)
	SELECT 
		a.ApplicationID,
		cg.CompetitiveGroupID,
	    ascg.CalculatedBenefitId,
	    ascg.CalculatedRating
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroup ascg
		JOIN dbo.[Application] a ON ascg.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroup cg ON ascg.[UID] = cg.[UID] 
	WHERE ascg.ImportPackageId = @packageId
		AND cg.InstitutionID = ascg.InstitutionId		
	
	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroupItem
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroupItem  (
		ApplicationID ,
		CompetitiveGroupItemID)
	SELECT 
		a.ApplicationID,
		cgi.CompetitiveGroupItemID
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroupItem ascgi
		JOIN dbo.[Application] a ON ascgi.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroupItem cgi ON ascgi.[UID] = cgi.[UID] 
		JOIN dbo.CompetitiveGroup cg ON cgi.CompetitiveGroupID = cg.CompetitiveGroupID				
	WHERE ascgi.ImportPackageId = @packageId		
		AND cg.InstitutionID = ascgi.InstitutionId	

	/*---------------------------------------------------------*
	 * ApplicationdCompetitiveGroupItem - новая структура
	 *---------------------------------------------------------*/
	insert into dbo.ApplicationCompetitiveGroupItem
	(
		ApplicationId,
		CompetitiveGroupId,
		CompetitiveGroupItemId,
		CompetitiveGroupTargetId,
		EducationFormId,
		EducationSourceId,
		[Priority]
	)
	select 
		app.ApplicationID, 
		cg.CompetitiveGroupId,
		cgi.CompetitiveGroupItemId,
		null, -- пока- null, потом заменим
		blkCGI.EducationForm,
		blkCGI.EducationSource,
		blkCGI.[Priority]
	from
		blk_ApplicationCompetitiveGroupItem blkCGI
		inner join dbo.Application app on blkCGI.ApplicationUID = app.UID
		inner join CompetitiveGroup cg on blkCGI.CompetitivegroupUID = cg.UID
		inner join CompetitiveGroupItem cgi on blkCGI.CompetitiveGroupItemUID = cgi.UID
	where blkCGI.ImportPackageId = @packageId and blkCGI.InstitutionId = @institutionId

	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroupTarget
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroupTarget (
		ApplicationID ,
		CompetitiveGroupTargetID ,
		IsForO ,
		IsForOZ ,
		IsForZ)
	SELECT 
		a.ApplicationID,
		cgt.CompetitiveGroupTargetID,
		ascgt.IsForO,
		ascgt.IsForOZ,
		ascgt.IsForZ
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroupTarget ascgt
		JOIN dbo.[Application] a ON ascgt.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroupTarget cgt ON cgt.[UID] = ascgt.[TargetOrganizationUID]
	WHERE ascgt.ImportPackageId = @packageId		
		AND cgt.InstitutionID = ascgt.InstitutionId;

	/*---------------------------------------------------------*
	 * EntrantDocument
	 *---------------------------------------------------------*/
	--DISABLE TRIGGER trig_EntrantDocument_CreatedDate ON EntrantDocument; 
	--DISABLE TRIGGER trig_EntrantDocument_ModifiedDate ON EntrantDocument;	 
	
	-- Не размножаем паспорта в БД
	UPDATE dbo.EntrantDocument with (updlock, rowlock)
	SET
		EntrantDocumentGUID = A1.Id,
		EntrantID = A1.EntrantID,
		ModifiedDate = GETDATE()		
	FROM 
		(SELECT DISTINCT
			ed1.EntrantDocumentID,
			ed.Id,
			e.EntrantID
		 FROM 
			blk_EntrantDocument ed  
			JOIN dbo.EntrantDocument ed1 ON 				
				ed1.DocumentTypeID = ed.DocumentTypeId AND
				ed1.DocumentSeries = ed.DocumentSeries AND
				ed1.DocumentNumber = ed.DocumentNumber AND
				ed1.DocumentDate = ed.DocumentDate AND				
				ed1.DocumentOrganization = ed.DocumentOrganization AND
				ed1.[UID] = ed.[UID]			
			JOIN dbo.Entrant e1 ON e1.EntrantID = ed1.EntrantID				
			JOIN dbo.blk_Application a ON a.Id = ed.ParentId
			JOIN dbo.[Application] a_db ON a_db.ApplicationGUID = a.Id		
			JOIN dbo.Entrant e ON e.EntrantID = a_db.EntrantID
		 WHERE ed.DocumentTypeId = 1 AND e1.InstitutionID = ed.InstitutionId
			AND ed.ImportPackageId = @packageId) AS A1			
	WHERE A1.EntrantDocumentID = EntrantDocument.EntrantDocumentID	
		 
	INSERT INTO dbo.EntrantDocument ( 
		EntrantID ,
		DocumentTypeID ,
		DocumentSeries ,
		DocumentNumber ,
		DocumentDate ,
		DocumentOrganization ,
		DocumentSpecificData ,
		AttachmentID ,
		[UID] ,
		EntrantDocumentGUID,
		CreatedDate,
		ModifiedDate)
	OUTPUT INSERTED.EntrantDocumentID INTO @InsertedEntrantDocumentsIds
	SELECT DISTINCT 
		e.EntrantID,
		ed.DocumentTypeId,
		ed.DocumentSeries,
		ed.DocumentNumber,
		ed.DocumentDate,
		ed.DocumentOrganization,
		ed.DocumentSpecificData,
		NULL , -- AttachmentID - int
		ed.[UID],
		ed.Id,
		GETDATE(),
		GETDATE()
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.blk_Application a ON a.Id = ed.ParentId
		JOIN dbo.[Application] a_db ON a_db.ApplicationGUID = a.Id		
		JOIN dbo.Entrant e ON e.EntrantID = a_db.EntrantID
		LEFT JOIN EntrantDocument ed1 ON ed1.EntrantDocumentGUID = ed.Id
	WHERE ed.ImportPackageId = @packageId AND ed1.EntrantDocumentID IS NULL;
	--ENABLE TRIGGER trig_EntrantDocument_CreatedDate ON EntrantDocument; 
	--ENABLE TRIGGER trig_EntrantDocument_ModifiedDate ON EntrantDocument;	 
		
	-- лок
	SELECT @stub = COUNT(*) FROM
	EntrantDocument a with (updlock, rowlock) 
	join @InsertedEntrantDocumentsIds i on i.Id = a.EntrantDocumentID
	SELECT @stub = COUNT(*) FROM	
	EntrantDocument a with (updlock, rowlock)
	--join Entrant e with (updlock, rowlock) on e.IdentityDocumentID = a.EntrantDocumentID 
	join ApplicationEntrantDocument aed with (updlock, rowlock) on a.EntrantDocumentID = aed.EntrantDocumentID
	join @InsertedEntrantDocumentsIds i on i.Id = aed.EntrantDocumentID

	/*---------------------------------------------------------*
	 * ApplicationEntrantDocument
	 *---------------------------------------------------------*/		
	--DISABLE TRIGGER trig_ApplicationEntrantDocument_CreatedDate ON ApplicationEntrantDocument; 
	--DISABLE TRIGGER trig_ApplicationEntrantDocument_ModifiedDate ON ApplicationEntrantDocument;	  
	INSERT INTO dbo.ApplicationEntrantDocument (
		ApplicationID ,
		EntrantDocumentID ,
		AttachedDocumentType ,
		[UID] ,
		OriginalReceivedDate,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		ed_db.EntrantDocumentID,
		NULL, -- AttachedDocumentType - int ???
		NULL, -- UID - varchar(200) ??? почему то всегда NULL
		ed.OriginalReceivedDate,
		GETDATE(),
		GETDATE()
	FROM 
		dbo.EntrantDocument ed_db	
		JOIN dbo.blk_EntrantDocument ed ON ed.Id = ed_db.EntrantDocumentGUID	
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID		
	WHERE ed.ImportPackageId = @packageId;
	--ENABLE TRIGGER trig_ApplicationEntrantDocument_CreatedDate ON ApplicationEntrantDocument; 
	--ENABLE TRIGGER trig_ApplicationEntrantDocument_ModifiedDate ON ApplicationEntrantDocument;	  
	
	/*---------------------------------------------------------*
	 * ApplicationEntranceTestDocument
	 *---------------------------------------------------------*/		
	--DISABLE TRIGGER trig_ApplicationEntranceTestDocument_CreatedDate ON ApplicationEntranceTestDocument; 
	--DISABLE TRIGGER trig_ApplicationEntranceTestDocument_ModifiedDate ON ApplicationEntranceTestDocument;		 
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd
		JOIN dbo.blk_EntrantDocument ed ON ed.EntranceTestResultId = aetd.Id
		JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND
			aetd.SourceId = 3 -- (OlympicDocument)	
		
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		NULL, -- InstitutionEntranceTest нет документа
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID								
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId							
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND
			aetd.SourceId = 2 -- (InstitutionEntranceTest)	
		
	-- для ЕГЭ документа может не быть
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		LEFT JOIN dbo.blk_EntrantDocument ed ON 
			ed.ParentId = aetd.ParentId AND	
			ed.[UID] = aetd.EgeDocumentId AND
			ed.ImportPackageId = @packageId					
		LEFT JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id			
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND 
		aetd.SourceId = 1 -- (EgeDocument)
		
	-- для ГИА документ обязателен
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		JOIN dbo.blk_EntrantDocument ed ON 
			ed.ParentId = aetd.ParentId AND	
			ed.[UID] = aetd.EgeDocumentId AND
			ed.ImportPackageId = @packageId					
		JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id			
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND 
		aetd.SourceId = 4 -- (GiaDocument)		
		
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		NULL,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		NULL, -- для документов Benefit
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		cg.CompetitiveGroupID,
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		LEFT JOIN dbo.blk_EntrantDocument ed ON ed.Id = aetd.BenefitEntrantDocumentId 
			AND ed.ImportPackageId = @packageId				
		LEFT JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id				
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
	WHERE aetd.ImportPackageId = @packageId AND aetd.EntranceTestTypeId IS NULL;
	--ENABLE TRIGGER trig_ApplicationEntranceTestDocument_CreatedDate ON ApplicationEntranceTestDocument; 
	--ENABLE TRIGGER trig_ApplicationEntranceTestDocument_ModifiedDate ON ApplicationEntranceTestDocument;	
		
	/*---------------------------------------------------------*
	 * EntrantDocumentIdentity
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentIdentity (
		EntrantDocumentID ,
		IdentityDocumentTypeID ,
		GenderTypeID ,
		NationalityTypeID ,
		BirthDate ,
		BirthPlace ,
		SubdivisionCode)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.IdentityDocumentTypeId,
		gt.GenderID,
		ed.NationalityTypeId,
		ed.BirthDate,
		ed.BirthPlace,
		ed.SubdivisionCode
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
		JOIN dbo.Entrant e ON e.EntrantID = ed_db.EntrantID
		JOIN dbo.GenderType gt ON gt.GenderID = gt.GenderID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 1 -- IdentityDocument
	
	-- блокировка --
	SELECT @stub = COUNT(e.EntrantID)
	FROM Entrant e with (updlock, rowlock)		
	WHERE e.InstitutionID = @institutionId
	
		--ed.ImportPackageId = @packageId	
	--	AND ed_db.EntrantDocumentID != e.IdentityDocumentID 
	--	AND ed.DocumentTypeID = 1 -- IdentityDocument	
		
	--UPDATE [dbo].[Entrant] with (updlock, rowlock)
	--SET [IdentityDocumentID] = ed_db.EntrantDocumentID
	--FROM		
	--	dbo.EntrantDocument ed_db
	--	JOIN dbo.blk_EntrantDocument ed	ON ed.Id = ed_db.EntrantDocumentGUID
	--WHERE
	--	ed.ImportPackageId = @packageId AND		
	--	ed_db.EntrantID = [dbo].[Entrant].EntrantID AND			
	--	ed.DocumentTypeID = 1 -- IdentityDocument
	
	UPDATE Entrant with (updlock, rowlock)
	SET IdentityDocumentID = ed_db.EntrantDocumentID
	FROM Entrant e
	JOIN EntrantDocument ed_db ON ed_db.EntrantID = e.EntrantID	
	WHERE 
		e.InstitutionID = @institutionId AND		
		ed_db.DocumentTypeID = 1 -- IdentityDocument
		
	/*---------------------------------------------------------*
	 * EntrantDocumentCustom		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentCustom ( 
		EntrantDocumentID ,
		DocumentTypeNameText ,
		AdditionalInfo)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DocumentTypeNameText,
		ed.AdditionalInfo
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 15 -- CustomDocument = 15

	/*---------------------------------------------------------*
	 * EntrantDocumentDisability		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentDisability (
		EntrantDocumentID ,
		DisabilityTypeID)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DisabilityTypeId
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 11 -- DisabilityDocument = 11
	
	/*---------------------------------------------------------*
	 * EntrantDocumentEdu		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEdu (
		EntrantDocumentID ,
		RegistrationNumber ,
		InstitutionName,
		GPA)			
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.RegistrationNumber,
		i.FullName,
		ed.GPA
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
		LEFT JOIN dbo.Institution i ON i.InstitutionID = ed.InstitutionId
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId IN (3, 4, 5, 6, 7, 8, 16)
		
	/*---------------------------------------------------------*
	 * EntrantDocumentEge		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEge ( 
		EntrantDocumentID ,
		DecisionNumber ,
		Decision ,
		DecisionDate ,
		TypographicNumber)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		NULL,
		NULL,
		NULL,
		NULL -- TypographicNumber ???
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 2 -- EgeDocument = 2

	/*---------------------------------------------------------*
	 * EntrantDocumentOlympic		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentOlympic (
		EntrantDocumentID ,
		DiplomaTypeID ,
		OlympicID)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DiplomaTypeId,
		ed.OlympicId
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 9 -- OlympicDocument = 9
		
	/*---------------------------------------------------------*
	 * EntrantDocumentOlympicTotal		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentOlympicTotal (
		EntrantDocumentID ,
		DiplomaTypeID ,
		OlympicPlace ,
		OlympicDate)
	SELECT DISTINCT
		ed_db.EntrantDocumentID,
		ed.DiplomaTypeId,
		ed.OlympicPlace,
		ed.OlympicDate
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 10 -- OlympicTotalDocument = 10
		
	/*---------------------------------------------------------*
	 * EntrantDocumentEgeAndOlympicSubject		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEgeAndOlympicSubject ( 
		EntrantDocumentID ,
		SubjectID ,
		Value)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		edos.SubjectId,
		edos.Value
	FROM dbo.blk_EntrantDocumentEgeAndOlympicSubject edos
		JOIN dbo.EntrantDocument ed_db ON edos.ParentId = ed_db.EntrantDocumentGUID
	WHERE 				
		edos.ImportPackageId = @packageId

	/*---------------------------------------------------------*
	 * ЛОГИРОВАНИЕ
	 *---------------------------------------------------------*/		
	--INSERT INTO [dbo].[PersonalDataAccessLog](
	--	[Method], 
	--	[OldData], 
	--	[NewData], 
	--	[ObjectType], 
	--	[AccessMethod], 
	--	[InstitutionID], 
	--	[UserLogin], 
	--	[ObjectID], 
	--	[AccessDate])
	--SELECT 
	--	'C',		
	--	'[{"ApplicationUID":"' + ISNULL(a.[UID], '') + 
	--	'","ApplicationNumber":"' + a.ApplicationNumber + 
	--	'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
	--		DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a.RegistrationDate)) AS VARCHAR) + 
	--	'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
	--	',"EntrantUID":"' + ISNULL(e.[UID], '') + 
	--	'","EntrantDocumentID":' + CAST(e.IdentityDocumentID AS VARCHAR) + 
	--	',"EntrantID":' + CAST(e.EntrantID AS VARCHAR) + '}]',		
	--	NULL,
	--	'Application',
	--	'ImportApplication',		
	--	a.InstitutionID,
	--	@userLogin,
	--	NULL,
	--	GETDATE()	
	--FROM 
	--	dbo.blk_Application a
	--	JOIN dbo.[Application] a_db ON a.Id = a_db.ApplicationGUID
	--	JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID
	--WHERE a.ImportPackageId = @packageId
	
	/*---------------------------------------------------------*
	 * Очистка BULK таблиц
	 *---------------------------------------------------------*/		
	EXEC dbo.[blk_DeleteApplicationPackage] @packageId

	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT
		(SELECT 
			Id AS '@Id' FROM @InsertedApplicationsIds
		 FOR
		 XML PATH('ImportResultItem'), TYPE) AS 'Successful',
		(SELECT 	
			[UID] AS '@UID'			
			FROM @FailedApplicationsUIDs
		 FOR
		 XML PATH('ImportResultItem'), TYPE) AS 'Failed'
	FOR XML PATH(''),
	ROOT('ImportResult')	
	
	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessConsideredApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[blk_ProcessConsideredApplicationBulkedPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	

	-- загруженные заявления
	DECLARE @SuccessfulIds TABLE (Id INT NOT NULL)		
	DECLARE @SuccessfulApplications TABLE (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL,
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[EducationFormID] [smallint] NOT NULL,
		[FinanceSourceID] [smallint] NOT NULL)
		
	-- не найденные заявления
	DECLARE @FailedApplications TABLE (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL,
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[EducationFormID] [smallint] NOT NULL,
		[FinanceSourceID] [smallint] NOT NULL,
		[IsApplicationNotFound] [bit] NULL,
		[IsApplicationNotAccepted] [bit] NULL,
		[IsFinSourceFormNotFound] [bit] NULL,
		[IsDirectionLevelNotFound] [bit] NULL,
		[IsEntrantRatingIsLessThanMinimal] [bit] NULL)
		
	/* Не найденные заявления */
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsApplicationNotFound)
	SELECT 
		a.ApplicationNumber, 
		a.RegistrationDate, 
		a.DirectionID, 
		a.EducationLevelID, 
		a.EducationFormID, 
		a.FinanceSourceID, 
		1
	FROM blk_ConsideredApplication a 
		LEFT JOIN dbo.[Application] a1 ON			
			a1.InstitutionID = a.InstitutionID AND
			a1.ApplicationNumber = a.ApplicationNumber AND
			a1.RegistrationDate = a.RegistrationDate			
	WHERE a.ImportPackageId = @packageId AND a1.ApplicationID IS NULL
	
	/* Не принятые заявления */
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsApplicationNotAccepted)
	SELECT 
		ca.ApplicationNumber, 
		ca.RegistrationDate, 
		ca.DirectionID,
		ca.EducationLevelID, 
		ca.EducationFormID, 
		ca.FinanceSourceID, 
		1
	FROM blk_ConsideredApplication ca 
		JOIN dbo.[Application] a1 ON			
			a1.InstitutionID = ca.InstitutionID AND
			a1.ApplicationNumber = ca.ApplicationNumber AND
			a1.RegistrationDate = ca.RegistrationDate			
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ca.ApplicationNumber AND
			err.RegistrationDate = ca.RegistrationDate AND
			err.EducationFormID = ca.EducationFormID AND
			err.EducationLevelID = ca.EducationLevelID AND
			err.DirectionID = ca.DirectionID AND
			err.FinanceSourceID = ca.FinanceSourceID
	WHERE ca.ImportPackageId = @packageId 
		AND a1.StatusID <> 4 AND err.ApplicationNumber IS NULL

	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsFinSourceFormNotFound)
	SELECT 
		ca.ApplicationNumber, 
		ca.RegistrationDate, 
		ca.DirectionID,
		ca.EducationLevelID, 
		ca.EducationFormID, 
		ca.FinanceSourceID, 
		1
	FROM blk_ConsideredApplication ca 
		LEFT JOIN dbo.[Application] a ON			
			a.InstitutionID = ca.InstitutionID AND
			a.ApplicationNumber = ca.ApplicationNumber AND
			a.RegistrationDate = ca.RegistrationDate AND						
			(CASE WHEN ca.IsRequiresBudgetO = 1 THEN ca.IsRequiresBudgetO ELSE a.IsRequiresBudgetO END) = a.IsRequiresBudgetO AND			
			(CASE WHEN ca.IsRequiresBudgetOZ = 1 THEN ca.IsRequiresBudgetOZ ELSE a.IsRequiresBudgetOZ END) = a.IsRequiresBudgetOZ AND			
			(CASE WHEN ca.IsRequiresBudgetZ = 1 THEN ca.IsRequiresBudgetZ ELSE a.IsRequiresBudgetZ END) = a.IsRequiresBudgetZ AND			
			(CASE WHEN ca.IsRequiresPaidO = 1 THEN ca.IsRequiresPaidO ELSE a.IsRequiresPaidO END) = a.IsRequiresPaidO AND			
			(CASE WHEN ca.IsRequiresPaidOZ = 1 THEN ca.IsRequiresPaidOZ ELSE a.IsRequiresPaidOZ END) = a.IsRequiresPaidOZ AND			
			(CASE WHEN ca.IsRequiresPaidZ = 1 THEN ca.IsRequiresPaidZ ELSE a.IsRequiresPaidZ END) = a.IsRequiresPaidZ AND			
			(CASE WHEN ca.IsRequiresTargetO = 1 THEN ca.IsRequiresTargetO ELSE a.IsRequiresTargetO END) = a.IsRequiresTargetO AND			
			(CASE WHEN ca.IsRequiresTargetOZ = 1 THEN ca.IsRequiresTargetOZ ELSE a.IsRequiresTargetOZ END) = a.IsRequiresTargetOZ AND			
			(CASE WHEN ca.IsRequiresTargetZ = 1 THEN ca.IsRequiresTargetZ ELSE a.IsRequiresTargetZ END) = a.IsRequiresTargetZ
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ca.ApplicationNumber AND
			err.RegistrationDate = ca.RegistrationDate AND
			err.EducationFormID = ca.EducationFormID AND
			err.EducationLevelID = ca.EducationLevelID AND
			err.DirectionID = ca.DirectionID AND
			err.FinanceSourceID = ca.FinanceSourceID			
	WHERE ca.ImportPackageId = @packageId 
		AND a.ApplicationID IS NULL AND err.ApplicationNumber IS NULL
	
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsDirectionLevelNotFound)
	SELECT 
		ca.ApplicationNumber, 
		ca.RegistrationDate, 
		ca.DirectionID,
		ca.EducationLevelID, 
		ca.EducationFormID, 
		ca.FinanceSourceID, 
		1
	FROM 
		blk_ConsideredApplication ca
		JOIN dbo.[Application] a_db ON 
			a_db.InstitutionID = ca.InstitutionID AND
			a_db.ApplicationNumber = ca.ApplicationNumber AND
			a_db.RegistrationDate = ca.RegistrationDate	
		LEFT JOIN 
		(
			SELECT DISTINCT 
				asg.ApplicationID,
				cgi.DirectionID,
				cgi.EducationLevelID
			FROM 
				dbo.ApplicationSelectedCompetitiveGroupItem asg
				JOIN dbo.CompetitiveGroupItem cgi ON asg.CompetitiveGroupItemID = cgi.CompetitiveGroupItemID
		) AS A1 ON 
			A1.ApplicationID = a_db.ApplicationID AND
			A1.DirectionID = ca.DirectionID AND
			A1.EducationLevelID = ca.EducationLevelID
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ca.ApplicationNumber AND
			err.RegistrationDate = ca.RegistrationDate AND
			err.EducationFormID = ca.EducationFormID AND
			err.EducationLevelID = ca.EducationLevelID AND
			err.DirectionID = ca.DirectionID AND
			err.FinanceSourceID = ca.FinanceSourceID			
	WHERE ca.ImportPackageId = @packageId 
		AND A1.ApplicationID IS NULL AND err.ApplicationNumber IS NULL	
	
	/* Балл по предметам должен быть > минимального в КГ. 
	   Если нет, то должна выдаваться ошибка «Этот абитуриент не прошел успешно вступительные испытания».
	   (ApplicationSelectedCompetitiveGroup. CalculatedRating) */
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsEntrantRatingIsLessThanMinimal)
	SELECT DISTINCT
		ca.ApplicationNumber,
		ca.RegistrationDate,		
		ca.DirectionID,
		ca.EducationLevelID,
		ca.EducationFormID, 
		ca.FinanceSourceID,	
		1											
	FROM 
		blk_ConsideredApplication ca
		JOIN dbo.[Application] a_db ON 
			a_db.InstitutionID = ca.InstitutionID AND
			a_db.ApplicationNumber = ca.ApplicationNumber AND
			a_db.RegistrationDate = ca.RegistrationDate
		JOIN dbo.ApplicationSelectedCompetitiveGroupItem ascgi ON
			ascgi.ApplicationID = a_db.ApplicationID
		JOIN dbo.CompetitiveGroupItem cgi ON 
			cgi.CompetitiveGroupItemID = ascgi.CompetitiveGroupItemID AND
			cgi.DirectionID = ca.DirectionID AND
			cgi.EducationLevelID = ca.EducationLevelID					
		JOIN dbo.EntranceTestItemC atc ON 
			atc.CompetitiveGroupID = cgi.CompetitiveGroupID			
		JOIN ApplicationEntranceTestDocument aetd ON
			aetd.ApplicationID = a_db.ApplicationID AND		
			aetd.EntranceTestItemID = atc.EntranceTestItemID AND		
			aetd.SubjectID = atc.SubjectID
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ca.ApplicationNumber AND
			err.RegistrationDate = ca.RegistrationDate AND
			err.EducationFormID = ca.EducationFormID AND
			err.EducationLevelID = ca.EducationLevelID AND
			err.DirectionID = ca.DirectionID AND
			err.FinanceSourceID = ca.FinanceSourceID				
	WHERE ca.ImportPackageId = @packageId AND err.ApplicationNumber IS NULL AND
		ISNULL(aetd.ResultValue, 0) < ISNULL(atc.MinScore, 0)

	/* 1. Удаляем существующие из ApplicationConsidered */
	DELETE FROM ApplicationConsidered
	WHERE ApplicationConsideredID IN (
		SELECT ac1.ApplicationConsideredID
		FROM blk_ConsideredApplication ca 			
			JOIN dbo.[Application] a ON 
				a.InstitutionID = ca.InstitutionId AND
				a.ApplicationNumber = ca.ApplicationNumber AND
				a.RegistrationDate = ca.RegistrationDate
			JOIN ApplicationConsidered ac1 ON 
				ac1.ApplicationID = a.ApplicationID AND
				ac1.DirectionID = ca.DirectionID AND				
				ac1.EducationLevelID = ca.EducationLevelID AND
				ac1.EducationFormID = ca.EducationFormID AND
				ac1.FinanceSourceID = ca.FinanceSourceID					
			LEFT JOIN @FailedApplications err ON 
				err.ApplicationNumber = a.ApplicationNumber AND
				err.RegistrationDate = a.RegistrationDate AND
				err.EducationFormID = ca.EducationFormID AND
				err.EducationLevelID = ca.EducationLevelID AND
				err.DirectionID = ca.DirectionID AND
				err.FinanceSourceID = ca.FinanceSourceID							
		WHERE ca.ImportPackageId = @packageId AND err.RegistrationDate IS NULL)
			
	INSERT INTO ApplicationConsidered (
		[ApplicationID],
		[DirectionID],
		[EducationLevelID],
		[EducationFormID],
		[FinanceSourceID],
		[IsRequiresBudgetO],
		[IsRequiresBudgetOZ],
		[IsRequiresBudgetZ],
		[IsRequiresPaidO],
		[IsRequiresPaidOZ],
		[IsRequiresPaidZ],
		[IsRequiresTargetO],
		[IsRequiresTargetOZ],
		[IsRequiresTargetZ],
		[Stage],
		[IsRecommended],
		[CreatedDate],
		[ModifiedDate])
	OUTPUT INSERTED.ApplicationConsideredID INTO @SuccessfulIds
	SELECT DISTINCT
		a_db.ApplicationID,
		ca.DirectionID,
		ca.EducationLevelID,
		ca.EducationFormID,
		ca.FinanceSourceID,		
		ca.IsRequiresBudgetO,
		ca.IsRequiresBudgetOZ,
		ca.IsRequiresBudgetZ,
		ca.IsRequiresPaidO,
		ca.IsRequiresPaidOZ,
		ca.IsRequiresPaidZ,
		ca.IsRequiresTargetO,		
		ca.IsRequiresTargetOZ,			
		ca.IsRequiresTargetZ,
		NULL,
		0,
		GETDATE(),
		GETDATE()				
	FROM blk_ConsideredApplication ca 
		JOIN dbo.[Application] a_db ON 
			a_db.InstitutionID = ca.InstitutionID AND
			a_db.ApplicationNumber = ca.ApplicationNumber AND
			a_db.RegistrationDate = ca.RegistrationDate	
		LEFT JOIN ApplicationConsidered ac1 ON 
			ac1.ApplicationID = a_db.ApplicationID AND
			ac1.DirectionID = ca.DirectionID AND				
			ac1.EducationLevelID = ca.EducationLevelID AND
			ac1.EducationFormID = ca.EducationFormID AND
			ac1.FinanceSourceID = ca.FinanceSourceID						
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ca.ApplicationNumber AND
			err.RegistrationDate = ca.RegistrationDate AND		
			err.EducationFormID = ca.EducationFormID AND
			err.EducationLevelID = ca.EducationLevelID AND
			err.DirectionID = ca.DirectionID AND
			err.FinanceSourceID = ca.FinanceSourceID			
	WHERE ca.ImportPackageId = @packageId AND ac1.ApplicationConsideredID IS NULL AND
		err.ApplicationNumber IS NULL
	
	INSERT INTO @SuccessfulApplications
	SELECT a.ApplicationNumber, a.RegistrationDate, ac.DirectionID, ac.EducationLevelID, ac.EducationFormID, ac.FinanceSourceID
	FROM ApplicationConsidered ac 
		JOIN dbo.Application a ON a.ApplicationID = ac.ApplicationID
	WHERE ac.ApplicationConsideredID IN (SELECT Id FROM @SuccessfulIds)
	
	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT	
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @SuccessfulApplications
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'Successful',						
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsDirectionLevelNotFound = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'DirectionLevelNotFound',						
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsFinSourceFormNotFound = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'FinSourceFormNotFound',	
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsApplicationNotAccepted = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationNotAccepted',
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsApplicationNotFound = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationNotFound',
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsEntrantRatingIsLessThanMinimal = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'EntrantRatingIsLessThanMinimal'		 		 
	FOR XML PATH(''),
	ROOT('ConsideredApplicationsResult')
	
	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[blk_ProcessRecommendedApplicationBulkedPackage]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[blk_ProcessRecommendedApplicationBulkedPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	

	-- загруженные заявления
	DECLARE @SuccessfulIds TABLE (Id INT NOT NULL)		
	DECLARE @SuccessfulApplications TABLE (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL,
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[EducationFormID] [smallint] NOT NULL,
		[FinanceSourceID] [smallint] NOT NULL)
		
	DECLARE @FailedApplications TABLE (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL,
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[EducationFormID] [smallint] NOT NULL,
		[FinanceSourceID] [smallint] NOT NULL,
		[IsApplicationNotFound] [bit] NULL,
		[IsAdmissionVolumeOverflow] [bit] NULL)	
		
	DECLARE @AdmissionVolume TABLE (				
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[NumberBudgetO] [int] NOT NULL,
		[NumberBudgetOZ] [int] NOT NULL,
		[NumberBudgetZ] [int] NOT NULL,
		[NumberPaidO] [int] NOT NULL,
		[NumberPaidOZ] [int] NOT NULL,
		[NumberPaidZ] [int] NOT NULL)			

	DECLARE @AdmissionVolumeOverflow TABLE (				
		[DirectionID] [int] NOT NULL,
		[EducationLevelID] [smallint] NOT NULL,
		[OverflowNumberBudgetO] [bit] NOT NULL,
		[OverflowNumberBudgetOZ] [bit] NOT NULL,
		[OverflowNumberBudgetZ] [bit] NOT NULL,
		[OverflowNumberPaidO] [bit] NOT NULL,
		[OverflowNumberPaidOZ] [bit] NOT NULL,
		[OverflowNumberPaidZ] [bit] NOT NULL)					
		
	-- которые не найдены в рассматриваемых
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsApplicationNotFound)
	SELECT 
		a.ApplicationNumber, 
		a.RegistrationDate, 
		a.DirectionID,
		a.EducationLevelID, 
		a.EducationFormID, 
		a.FinanceSourceID, 
		1
	FROM blk_RecommendedApplication a 
		LEFT JOIN dbo.[Application] a1 ON			
			a1.InstitutionID = a.InstitutionID AND
			a1.ApplicationNumber = a.ApplicationNumber AND
			a1.RegistrationDate = a.RegistrationDate			
		LEFT JOIN ApplicationConsidered ac ON 
			ac.ApplicationID = a1.ApplicationID AND
			ac.DirectionID = a.DirectionID AND
			ac.EducationLevelID = a.EducationLevelID AND
			ac.EducationFormID = a.EducationFormID AND
			ac.FinanceSourceID = a.FinanceSourceID
	WHERE a.ImportPackageId = @packageId AND ac.ApplicationID IS NULL

	/* Проверяем объем приема */
	INSERT INTO @AdmissionVolume
	SELECT DISTINCT
		ra.DirectionID,
		ra.EducationLevelID,
		av.NumberBudgetO,
		av.NumberBudgetOZ,
		av.NumberBudgetZ,
		av.NumberPaidO,
		av.NumberPaidOZ,
		av.NumberPaidZ
	FROM
		blk_RecommendedApplication ra
		JOIN dbo.[Application] a ON
			a.InstitutionID = ra.InstitutionId AND
			a.ApplicationNumber = ra.ApplicationNumber AND
			a.RegistrationDate = ra.RegistrationDate		
		JOIN dbo.ApplicationSelectedCompetitiveGroupItem asgi ON
			asgi.ApplicationID = a.ApplicationID						
		JOIN dbo.CompetitiveGroupItem cgi ON
			cgi.CompetitiveGroupItemID = asgi.CompetitiveGroupItemID AND
			cgi.DirectionID = ra.DirectionID AND
			cgi.EducationLevelID = ra.EducationLevelID		
		JOIN dbo.CompetitiveGroup cg ON 
			cg.CompetitiveGroupID = cgi.CompetitiveGroupID	
		JOIN dbo.AdmissionVolume av ON
			av.CampaignID = cg.CampaignID AND
			av.DirectionID = ra.DirectionID AND
			av.AdmissionItemTypeID = ra.EducationLevelID							
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ra.ApplicationNumber AND
			err.RegistrationDate = ra.RegistrationDate AND
			err.EducationLevelID = ra.EducationLevelID AND
			err.EducationFormID = ra.EducationFormID AND
			err.DirectionID = ra.DirectionID AND
			err.FinanceSourceID = ra.FinanceSourceID		
	WHERE ra.ImportPackageId = @packageId AND err.ApplicationNumber IS NULL

	/* Смотрим куда сколько народу идет и исключаем направления по которым у нас переполнение */
	INSERT INTO @AdmissionVolumeOverflow
	SELECT 
		A3.DirectionID,
		A3.EducationLevelID,
		CASE WHEN A3.ToBudgetO > av.NumberBudgetO THEN 1 ELSE 0 END AS OverflowNumberBudgetO,
		CASE WHEN A3.ToBudgetOZ > av.NumberBudgetOZ THEN 1 ELSE 0 END AS OverflowNumberBudgetOZ,
		CASE WHEN A3.ToBudgetZ > av.NumberBudgetZ THEN 1 ELSE 0 END AS OverflowNumberBudgetZ,
		CASE WHEN A3.ToPaidO > av.NumberPaidO THEN 1 ELSE 0 END AS OverflowNumberPaidO,
		CASE WHEN A3.ToPaidOZ > av.NumberPaidOZ THEN 1 ELSE 0 END AS OverflowNumberPaidOZ,
		CASE WHEN A3.ToPaidZ > av.NumberPaidZ THEN 1 ELSE 0 END AS OverflowNumberPaidZ
	FROM (
		SELECT 
			A2.DirectionID,
			A2.EducationLevelID,
			SUM(A2.ToBudgetO) AS ToBudgetO,
			SUM(A2.ToBudgetOZ) AS ToBudgetOZ,
			SUM(A2.ToBudgetZ) AS ToBudgetZ,
			SUM(A2.ToPaidO) AS ToPaidO,
			SUM(A2.ToPaidOZ) AS ToPaidOZ,
			SUM(A2.ToPaidZ) AS ToPaidZ
		FROM (		
			 SELECT 
				ra.DirectionID,
				ra.EducationLevelID,
				CASE WHEN ra.FinanceSourceID = 14 AND ra.EducationFormID = 11 THEN COUNT(*) ELSE 0 END AS ToBudgetO,
				CASE WHEN ra.FinanceSourceID = 14 AND ra.EducationFormID = 12 THEN COUNT(*) ELSE 0 END AS ToBudgetOZ,
				CASE WHEN ra.FinanceSourceID = 14 AND ra.EducationFormID = 10 THEN COUNT(*) ELSE 0 END AS ToBudgetZ,
				CASE WHEN ra.FinanceSourceID = 15 AND ra.EducationFormID = 11 THEN COUNT(*) ELSE 0 END AS ToPaidO,
				CASE WHEN ra.FinanceSourceID = 15 AND ra.EducationFormID = 12 THEN COUNT(*) ELSE 0 END AS ToPaidOZ,
				CASE WHEN ra.FinanceSourceID = 15 AND ra.EducationFormID = 10 THEN COUNT(*) ELSE 0 END AS ToPaidZ
			 FROM 
				/* Все заявления которые новые рассматриваемые */
				blk_RecommendedApplication ra
				LEFT JOIN @FailedApplications err ON 
					err.ApplicationNumber = ra.ApplicationNumber AND
					err.RegistrationDate = ra.RegistrationDate AND
					err.EducationLevelID = ra.EducationLevelID AND
					err.EducationFormID = ra.EducationFormID AND
					err.DirectionID = ra.DirectionID AND
					err.FinanceSourceID = ra.FinanceSourceID
			 WHERE ra.ImportPackageId = @packageId AND err.ApplicationNumber IS NULL
			 GROUP BY 	
				ra.DirectionID,
				ra.EducationLevelID,
				ra.EducationFormID,
				ra.FinanceSourceID) AS A2
		GROUP BY 
			A2.DirectionID, 
			A2.EducationLevelID) AS A3
		JOIN @AdmissionVolume av ON 
			A3.DirectionID = av.DirectionID AND
			A3.EducationLevelID = av.EducationLevelID

	-- заявления которые попали в переполненные направления
	INSERT INTO @FailedApplications (
		ApplicationNumber, 
		RegistrationDate, 
		DirectionID, 
		EducationLevelID, 
		EducationFormID, 
		FinanceSourceID,
		IsAdmissionVolumeOverflow)
	SELECT 
		ra.ApplicationNumber, 
		ra.RegistrationDate, 
		ra.DirectionID,
		ra.EducationLevelID, 
		ra.EducationFormID, 
		ra.FinanceSourceID, 
		1
	FROM blk_RecommendedApplication ra 
		JOIN @AdmissionVolumeOverflow o ON 
			ra.DirectionID = o.DirectionID AND
			ra.EducationLevelID = o.EducationLevelID
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ra.ApplicationNumber AND
			err.RegistrationDate = ra.RegistrationDate AND
			err.EducationLevelID = ra.EducationLevelID AND
			err.EducationFormID = ra.EducationFormID AND
			err.DirectionID = ra.DirectionID AND
			err.FinanceSourceID = ra.FinanceSourceID			
	WHERE 
		ra.ImportPackageId = @packageId AND err.ApplicationNumber IS NULL AND
		(o.OverflowNumberBudgetO = 1 AND ra.IsRequiresBudgetO = 1) OR
		(o.OverflowNumberBudgetOZ = 1 AND ra.IsRequiresBudgetOZ = 1) OR
		(o.OverflowNumberBudgetZ = 1 AND ra.IsRequiresBudgetZ = 1) OR
		(o.OverflowNumberPaidO = 1 AND ra.IsRequiresPaidO = 1) OR
		(o.OverflowNumberPaidOZ = 1 AND ra.IsRequiresPaidOZ = 1) OR
		(o.OverflowNumberPaidZ = 1 AND ra.IsRequiresPaidZ = 1)
			
	/* Сброс всех рекомендованных по направлению */
	UPDATE dbo.ApplicationConsidered	
	SET	
		IsRecommended = 0,
		Stage = NULL
	WHERE ApplicationConsideredID IN (
		SELECT ac.ApplicationConsideredID
		FROM blk_RecommendedApplication ra 
			JOIN @AdmissionVolumeOverflow o ON 
				ra.DirectionID = o.DirectionID AND
				ra.EducationLevelID = o.EducationLevelID
			JOIN dbo.[Application] a_db ON			
				a_db.InstitutionID = ra.InstitutionID AND
				a_db.ApplicationNumber = ra.ApplicationNumber AND
				a_db.RegistrationDate = ra.RegistrationDate			
			-- делаем это только по направлениям которые пришли
			JOIN ApplicationConsidered ac ON 
				ac.DirectionID = o.DirectionID AND
				ac.EducationLevelID = o.EducationLevelID AND
				ac.EducationFormID = ra.EducationFormID AND
				ac.FinanceSourceID = ra.FinanceSourceID					
			LEFT JOIN @FailedApplications err ON 
				err.ApplicationNumber = ra.ApplicationNumber AND
				err.RegistrationDate = ra.RegistrationDate AND
				err.EducationLevelID = ra.EducationLevelID AND
				err.EducationFormID = ra.EducationFormID AND
				err.DirectionID = ra.DirectionID AND
				err.FinanceSourceID = ra.FinanceSourceID		
		WHERE
			ac.IsRecommended = 1 AND
			(o.OverflowNumberBudgetO = 0 AND ac.IsRequiresBudgetO = 1) OR
			(o.OverflowNumberBudgetOZ = 0 AND ac.IsRequiresBudgetOZ = 1) OR
			(o.OverflowNumberBudgetZ = 0 AND ac.IsRequiresBudgetZ = 1) OR
			(o.OverflowNumberPaidO = 0 AND ac.IsRequiresPaidO = 1) OR
			(o.OverflowNumberPaidOZ = 0 AND ac.IsRequiresPaidOZ = 1) OR
			(o.OverflowNumberPaidZ = 0 AND ac.IsRequiresPaidZ = 1))	

	UPDATE dbo.ApplicationConsidered	
	SET
		IsRecommended = 1,
		Stage = ra.Stage,
		ModifiedDate = GETDATE()	
	OUTPUT INSERTED.ApplicationConsideredID INTO @SuccessfulIds
	FROM blk_RecommendedApplication ra
		JOIN dbo.[Application] a_db ON			
			a_db.InstitutionID = ra.InstitutionID AND
			a_db.ApplicationNumber = ra.ApplicationNumber AND
			a_db.RegistrationDate = ra.RegistrationDate			
		JOIN ApplicationConsidered ac ON 
			ac.ApplicationID = a_db.ApplicationID AND
			ac.DirectionID = ra.DirectionID AND
			ac.EducationLevelID = ra.EducationLevelID AND
			ac.EducationFormID = ra.EducationFormID AND
			ac.FinanceSourceID = ra.FinanceSourceID		
		LEFT JOIN @FailedApplications err ON 
			err.ApplicationNumber = ra.ApplicationNumber AND
			err.RegistrationDate = ra.RegistrationDate AND
			err.EducationLevelID = ra.EducationLevelID AND
			err.EducationFormID = ra.EducationFormID AND
			err.DirectionID = ra.DirectionID AND
			err.FinanceSourceID = ra.FinanceSourceID
	WHERE ra.ImportPackageId = @packageId AND 
		ac.ApplicationConsideredID = ApplicationConsideredID AND err.ApplicationNumber IS NULL

	INSERT INTO @SuccessfulApplications
	SELECT a.ApplicationNumber, a.RegistrationDate, ac.DirectionID, ac.EducationLevelID, ac.EducationFormID, ac.FinanceSourceID
	FROM ApplicationConsidered ac 
		JOIN dbo.[Application] a ON a.ApplicationID = ac.ApplicationID
	WHERE ac.ApplicationConsideredID IN (SELECT Id FROM @SuccessfulIds)
	
	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT	
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @SuccessfulApplications
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'Successful',						
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsApplicationNotFound = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationNotFound',
		(SELECT [ApplicationNumber] AS '@ApplicationNumber', [RegistrationDate] AS '@RegistrationDate', [DirectionID] AS '@DirectionID', [EducationLevelID] AS '@EducationLevelID', [EducationFormID] AS '@EducationFormID', [FinanceSourceID] AS '@FinanceSourceID'
		 FROM @FailedApplications WHERE IsAdmissionVolumeOverflow = 1
		 FOR XML PATH('ApplicationShortRefResult'), TYPE) AS 'AdmissionVolumeOverflow'		 		 
	FOR XML PATH(''),
	ROOT('ConsideredApplicationsResult')
	
	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[CopyStructureToAdmission]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[CopyStructureToAdmission]
	@EducationLevelID INT
AS
SET NOCOUNT ON
SET XACT_ABORT ON 

DECLARE @result TABLE ([Depth] SMALLINT NOT NULL,
	InstitutionItemID INT NOT NULL,
	[InstitutionID] [int] NOT NULL,
	[ItemTypeID] [smallint] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[BriefName] [varchar](30) NULL,
	[DirectionID] [int] NULL,
	[ParentID] [int] NULL,
	AdmissionItemID INT NULL
)

DECLARE @instID INT, @lineageFilter VARCHAR(126), @parentDepth SMALLINT,
	@depth SMALLINT, @DirectionAdmissionItemTypeID SMALLINT

SELECT @instID = ai.InstitutionID, @lineageFilter = s.Lineage + '%', @parentDepth = s.Depth,
	@DirectionAdmissionItemTypeID = ai.ItemTypeID
FROM AdmissionItem ai JOIN AdmissionStructure s ON s.AdmissionItemID = ai.AdmissionItemID
WHERE ai.AdmissionItemID = @EducationLevelID

IF @instID IS NULL 
BEGIN
	RAISERROR('Уровень образования не найден', 18, 1)
	RETURN -1
END

IF EXISTS(SELECT * FROM AdmissionItem ai WHERE ai.ParentID = @EducationLevelID)
BEGIN
	RAISERROR('Можно копировать структуру только в пустой уровень образования', 18, 1)
	RETURN -2
END

DECLARE @needSqlTran BIT, @RC INT
IF @@TRANCOUNT = 0 SET @needSqlTran = 1
ELSE SET @needSqlTran = 0

IF @needSqlTran = 1 BEGIN TRAN	


INSERT @result
SELECT s.Depth,
ii.InstitutionItemID, @instID,
	CASE ii.ItemTypeID WHEN 1 THEN 6 WHEN 2 THEN 7 WHEN 3 THEN 9 END,
	ii.[Name], ii.BriefName, ii.DirectionID, 
	CASE WHEN Depth > 2 THEN ii.ParentID ELSE @EducationLevelID END, NULL
FROM InstitutionStructure s
JOIN InstitutionItem ii ON ii.InstitutionItemID = s.InstitutionItemID
LEFT JOIN AllowedDirections ad ON ad.InstitutionID = @instID 
	AND ad.AdmissionItemTypeID = @DirectionAdmissionItemTypeID
	AND ii.DirectionID = ad.DirectionID
WHERE ii.InstitutionID = @instID AND s.Depth > 1
	AND (ii.DirectionID IS NULL OR ad.AdmissionItemTypeID IS NOT NULL)
ORDER BY s.Depth, ii.[Name]

SET @depth = 2
-- начинаем со следующего за самим ОУ уровнем
WHILE @depth <= 4
BEGIN
	INSERT INTO AdmissionItem (InstitutionID, ItemTypeID, [Name], BriefName, DirectionID, ParentID)
	SELECT @instID, ItemTypeID, [Name], BriefName, DirectionID, ParentID
	FROM @result r WHERE r.Depth = @depth
	IF @@ROWCOUNT = 0 BREAK
	
	UPDATE r SET AdmissionItemID = ai.AdmissionItemID 
	--OUTPUT DELETED.AdmissionItemID, INSERTED.AdmissionItemID
	FROM @result r
	JOIN AdmissionItem ai ON ai.InstitutionID = r.InstitutionID AND ai.ItemTypeID = r.ItemTypeID
		AND ai.[Name] = r.[Name] AND ai.ParentID = r.ParentID
	WHERE r.Depth = @depth
	
	--SELECT @depth
	
	IF @depth != 4 -- проверка на максимальную глубину, чтобы не делать лишний раз
	BEGIN
		UPDATE r SET ParentID = p.AdmissionItemID
		--OUTPUT DELETED.ParentID, INSERTED.ParentID
		FROM @result r JOIN @result p ON r.ParentID = p.InstitutionItemID
		WHERE p.Depth = @depth AND r.Depth = @depth+1
	END
	
	INSERT INTO AdmissionStructure (AdmissionItemID, ParentID)
	SELECT r.AdmissionItemID, ps.AdmissionStructureID
	FROM @result r
		JOIN AdmissionItem p ON p.AdmissionItemID = r.ParentID
		JOIN AdmissionStructure ps ON ps.AdmissionItemID = p.AdmissionItemID
	WHERE r.Depth = @depth AND ps.Lineage LIKE @lineageFilter
	
	SET @depth = @depth + 1
END

IF @needSqlTran = 1 COMMIT


GO

/****** Object:  StoredProcedure [dbo].[CountryType_Transfer]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[CountryType_Transfer]
	WITH EXECUTE AS OWNER
AS
SET NOCOUNT ON
SET XACT_ABORT ON

-- delete regions before removing countries
DELETE FROM RegionType 
FROM RegionType r JOIN CountryType ct ON r.CountryID = ct.CountryID
 JOIN #CountryType t ON t.DigitCode = ct.DigitCode
WHERE t.Status = 1

DELETE CountryType FROM #CountryType t
WHERE t.DigitCode = CountryType.DigitCode AND t.Status = 1

-- first update existing rows in CountryType for avoiding key constraints
UPDATE CountryType SET Name = CASE WHEN CHARINDEX('^', t.[Name]) > 0 THEN LEFT(t.[Name], CHARINDEX('^', t.[Name]) - 1) ELSE t.[Name] END, 
Alfa2Code = t.Alfa2Code, Alfa3Code = t.Alfa3Code 
FROM #CountryType t 
WHERE CountryType.DigitCode = t.DigitCode AND t.Status = 2

INSERT INTO CountryType (DigitCode, Name, Alfa2Code, Alfa3Code)
SELECT DISTINCT t.DigitCode, CASE WHEN CHARINDEX('^', t.[Name]) > 0 THEN LEFT(t.[Name], CHARINDEX('^', t.[Name]) - 1) ELSE t.[Name] END,
	t.Alfa2Code, t.Alfa3Code 
FROM #CountryType t LEFT JOIN CountryType f ON f.DigitCode = t.DigitCode
WHERE f.Name IS NULL AND t.DigitCode IS NOT NULL AND t.Status IN (0,3)

-- 


GO

/****** Object:  StoredProcedure [dbo].[dba_indexDefragStandard_sp]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 
CREATE PROCEDURE [dbo].[dba_indexDefragStandard_sp]
 
    /* Declare Parameters */
      @minFragmentation     FLOAT           = 10.0
        /* in percent, will not defrag if fragmentation
           less than specified */
    , @rebuildThreshold     FLOAT           = 30.0
        /* in percent, greater than @rebuildThreshold
           will result in rebuild instead of reorg */
    , @executeSQL           BIT             = 1
        /* 1 = execute; 0 = print command only */
    , @tableName            VARCHAR(4000)   = Null 
        /* Option to specify a table name */
    , @printCommands        BIT             = 0
        /* 1 = print commands; 0 = do not print commands */
    , @printFragmentation   BIT             = 0
        /* 1 = print fragmentation prior to defrag; 
           0 = do not print */
    , @defragDelay          CHAR(8)         = '00:00:05'
        /* time to wait between defrag commands */
AS
/********************************************************************
    Name:       dba_indexDefragStandard_sp
 
    Author:     Michelle F. Ufford
 
    Purpose:    Defrags all indexes for the current database
 
    Notes:      This script was designed for SQL Server 2005
                Standard edition.
 
    CAUTION: Monitor transaction log if executing for the first time!
 
      @minFragmentation     defaulted to 10%, will not defrag if
                            fragmentation if less than specified.
 
      @rebuildThreshold     defaulted to 30% as recommended by
                            Microsoft in BOL;
                            > than 30% will result in rebuild instead
 
      @executeSQL           1 = execute the SQL generated by this proc;
                            0 = print command only
 
      @tableName            Specify if you only want to defrag indexes
                            for a specific table
 
      @printCommands        1 = print commands to screen;
                            0 = do not print commands
 
      @printFragmentation   1 = print fragmentation to screen;
                            0 = do not print fragmentation
 
      @defragDelay          time to wait between defrag commands;
                            gives the server some time to catch up
 
    Called by:  SQL Agent Job or DBA
 
    Date        Initials  Description
    ----------------------------------------------------------------
    2008-10-27  MFU       Initial Release
    2008-11-17  MFU       Added page_count to log table
                          , added @printFragmentation option
********************************************************************
    Exec dbo.dba_indexDefragStandard_sp
          @executeSQL         = 1
        , @printCommands      = 1
        , @minFragmentation   = 0
        , @printFragmentation = 1;
********************************************************************/
 
SET NOCOUNT ON;
SET XACT_Abort ON;
 
BEGIN
 
    /* Declare our variables */
    DECLARE   @objectID         INT
            , @indexID          INT
            , @schemaName       NVARCHAR(130)
            , @objectName       NVARCHAR(130)
            , @indexName        NVARCHAR(130)
            , @fragmentation    FLOAT
            , @pageCount        INT
            , @sqlCommand       NVARCHAR(4000)
            , @rebuildCommand   NVARCHAR(200)
            , @dateTimeStart    DATETIME
            , @dateTimeEnd      DATETIME
            , @containsLOB      BIT;
 
    /* Just a little validation... */
    IF @minFragmentation Not Between 0.00 And 100.0
        SET @minFragmentation = 10.0;
 
    IF @rebuildThreshold Not Between 0.00 And 100.0
        SET @rebuildThreshold = 30.0;
 
    IF @defragDelay Not Like '00:[0-5][0-9]:[0-5][0-9]'
        SET @defragDelay = '00:00:05';
 
    /* Determine which indexes to defrag using our
       user-defined parameters */
    SELECT
          OBJECT_ID AS objectID
        , index_id AS indexID
        , avg_fragmentation_in_percent AS fragmentation
        , page_count 
        , 0 AS 'defragStatus'
            /* 0 = unprocessed, 1 = processed */
    INTO #indexDefragList
    FROM sys.dm_db_index_physical_stats
        (DB_ID(), OBJECT_ID(@tableName), NULL , NULL, N'Limited')
    WHERE avg_fragmentation_in_percent > @minFragmentation
        And index_id > 0
    OPTION (MaxDop 1);
 
    /* Create a clustered index to boost performance a little */
    CREATE CLUSTERED INDEX CIX_temp_indexDefragList
        ON #indexDefragList(objectID, indexID);
 
    /* Begin our loop for defragging */
    WHILE (SELECT COUNT(*) FROM #indexDefragList
            WHERE defragStatus = 0) > 0
    BEGIN
 
        /* Grab the most fragmented index first to defrag */
        SELECT TOP 1
              @objectID         = objectID
            , @fragmentation    = fragmentation
            , @indexID          = indexID
            , @pageCount        = page_count
        FROM #indexDefragList
        WHERE defragStatus = 0
        ORDER BY fragmentation DESC;
 
        /* Look up index information */
        SELECT @objectName = QUOTENAME(o.name)
             , @schemaName = QUOTENAME(s.name)
        FROM sys.objects AS o
        Inner Join sys.schemas AS s
            ON s.schema_id = o.schema_id
        WHERE o.OBJECT_ID = @objectID;
 
        SELECT @indexName = QUOTENAME(name)
        FROM sys.indexes
        WHERE OBJECT_ID = @objectID
            And index_id = @indexID
            And type > 0;
 
        /* Look for LOBs */
        SELECT TOP 1
            @containsLOB = column_id
        FROM sys.columns WITH (NOLOCK)
        WHERE 
            [OBJECT_ID] = @objectID
            And (system_type_id In (34, 35, 99)
            -- 34 = image, 35 = text, 99 = ntext
                    Or max_length = -1);
            -- varbinary(max), varchar(max), nvarchar(max), xml
 
        /* See if we should rebuild or reorganize; handle thusly */
        IF @fragmentation < @rebuildThreshold 
            Or IsNull(@containsLOB, 0) > 0 
            -- Cannot rebuild if the table has one or more LOB
            SET @sqlCommand = N'Alter Index ' + @indexName + N' On '
                + @schemaName + N'.' + @objectName + N' ReOrganize;'
        ELSE
            SET @sqlCommand = N'Alter Index ' + @indexName + N' On '
                + @schemaName + N'.' + @objectName +  ' Rebuild '
                + 'With (MaxDop = 1)'; -- minimize impact on server
 
        /* Are we executing the SQL?  If so, do it */
        IF @executeSQL = 1
        BEGIN
 
            /* Grab the time for logging purposes */
            SET @dateTimeStart  = GETDATE();
            EXECUTE (@sqlCommand);
            SET @dateTimeEnd  = GETDATE();
 
            /* Log our actions */
            INSERT INTO dbo.dba_indexDefragLog
            (
                  objectID
                , objectName
                , indexID
                , indexName
                , fragmentation
                , page_count
                , dateTimeStart
                , durationSeconds
            )
            SELECT
                  @objectID
                , @objectName
                , @indexID
                , @indexName
                , @fragmentation
                , @pageCount
                , @dateTimeStart
                , DATEDIFF(SECOND, @dateTimeStart, @dateTimeEnd);
 
            /* Just a little breather for the server */
            WAITFOR Delay @defragDelay;
 
            /* Print if specified to do so */
            IF @printCommands = 1
                PRINT N'Executed: ' + @sqlCommand;
        END
        ELSE
        /* Looks like we're not executing, just print
            the commands */
        BEGIN
            IF @printCommands = 1
                PRINT @sqlCommand;
        END
 
        /* Update our index defrag list when we've
            finished with that index */
        UPDATE #indexDefragList
        SET defragStatus = 1
        WHERE objectID  = @objectID
          And indexID   = @indexID;
 
    END
 
    /* Do we want to output our fragmentation results? */
    IF @printFragmentation = 1
        SELECT idl.objectID
            , o.name As 'tableName'
            , idl.indexID
            , i.name As 'indexName'
            , idl.fragmentation
            , idl.page_count
        FROM #indexDefragList AS idl
        JOIN sys.objects AS o
            ON idl.objectID = o.object_id
        JOIN sys.indexes As i
            ON idl.objectID = i.object_id
            AND idl.indexID = i.index_id;
 
    /* When everything is done, make sure to get rid of
        our temp table */
    DROP TABLE #indexDefragList;
 
    SET NOCOUNT OFF;
    RETURN 0
END

GO

/****** Object:  StoredProcedure [dbo].[DeleteApplications_fromXml]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[DeleteApplications_fromXml] @xml NTEXT, @institutionId INT, @userLogin VARCHAR(100), @logEnabled bit = true
AS
	SET NOCOUNT ON	
		
    DECLARE @idoc INT
    EXEC sp_xml_preparedocument @idoc output, @xml, '<ArrayOfApplicationShortRef xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"/>' ;

	-- заявления со статусами = 8
	CREATE TABLE #InOrderApplications (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #InOrderApplications (ApplicationNumber, RegistrationDate)

	-- не найденные заявления
	CREATE TABLE #NotFoundApplications (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #NotFoundApplications (ApplicationNumber, RegistrationDate)
   
    CREATE TABLE #Applications (
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)		
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #Applications (ApplicationNumber, RegistrationDate)
	
	INSERT INTO #Applications  (
		[ApplicationNumber],
		[RegistrationDate])
	SELECT 	
		[ApplicationNumber],
		[RegistrationDate]
	FROM openXml(@idoc, '//ApplicationShortRef', 1) 
	WITH 
	( 		
		[ApplicationNumber] [varchar](50) 'ApplicationNumber[@xsi:nil!="true" or not(@xsi:nil)]',
		[RegistrationDate] [datetime] 'RegistrationDate'	
	)		
	EXEC sp_xml_removedocument @idoc;

	CREATE TABLE #processedEntrantsIds ([Id] INT NOT NULL) 
	CREATE TABLE #applicationIds (
		[Id] INT NOT NULL PRIMARY KEY, 
		[ApplicationNumber] [varchar](50) NULL, 
		[RegistrationDate] [datetime] NOT NULL)	
	CREATE NONCLUSTERED INDEX [idx_NumberDate] ON #applicationIds (ApplicationNumber, RegistrationDate)		
	
	-- заявления которые точно есть в БД	
	INSERT INTO #applicationIds
	SELECT a_db.ApplicationID, a_db.ApplicationNumber, a_db.RegistrationDate
	FROM #Applications a 
		JOIN dbo.[Application] a_db ON 
			LTRIM (RTRIM (a_db.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a_db.InstitutionID = @institutionId AND a_db.StatusID != 8 -- InOrder

	-- Энтраты к которым привязаны удаляемые заявления
	INSERT INTO #processedEntrantsIds
	SELECT a.EntrantID
	FROM
		dbo.[Application] a 
		JOIN #applicationIds a1 ON a.ApplicationID = a1.Id
	WHERE a.InstitutionID = @institutionId

	-- которые со статусами 8
	INSERT INTO #InOrderApplications
	SELECT a_db.ApplicationNumber, a_db.RegistrationDate
	FROM #Applications a 
		JOIN dbo.[Application] a_db ON 			
			LTRIM (RTRIM (a_db.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a_db.InstitutionID = @institutionId AND a_db.StatusID = 8 -- InOrder

	-- которые не найдены
	INSERT INTO #NotFoundApplications
	SELECT a.ApplicationNumber, a.RegistrationDate
	FROM #Applications a 
		LEFT JOIN #applicationIds a1 ON
			LTRIM (RTRIM (a1.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a1.RegistrationDate = a.RegistrationDate			
		LEFT JOIN #InOrderApplications a2 ON
			LTRIM (RTRIM (a2.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a2.RegistrationDate = a.RegistrationDate			
	WHERE a1.Id IS NULL AND a2.ApplicationNumber IS NULL
	
	-- Выбираем все у которых нет связки с другими заявлениями
	DECLARE @entrantDocumentIds TABLE (Id INT NOT NULL)
	DECLARE @relatedDocumentIds TABLE (Id INT NOT NULL)
	-----------------------------------------------------------------------------
	-- все документы по заявлениям
	INSERT INTO @entrantDocumentIds	
	SELECT aed.EntrantDocumentID
	FROM #applicationIds a 
		JOIN ApplicationEntrantDocument aed ON aed.ApplicationID = a.Id
	UNION
	SELECT aed.EntrantDocumentID
	FROM  
		#applicationIds a 
		JOIN dbo.ApplicationEntranceTestDocument aed ON aed.ApplicationID = a.Id
	WHERE aed.EntrantDocumentID IS NOT NULL
			
	-- имеющие ссылки на другие заявления
	INSERT INTO @relatedDocumentIds	
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntrantDocument aed ON	aed.EntrantDocumentID = A1.Id
	WHERE aed.ApplicationID NOT IN (SELECT Id from #applicationIds)				
	UNION
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntranceTestDocument aed ON aed.EntrantDocumentID = A1.Id
	WHERE 
		aed.EntrantDocumentID IS NOT NULL AND
		aed.ApplicationID NOT IN (SELECT Id from #applicationIds)	

	-- удаляем кросс документы
	DELETE ed FROM @entrantDocumentIds ed
		JOIN @relatedDocumentIds rd ON ed.Id = rd.Id
	-----------------------------------------------------------------------------

	/*---------------------------------------------------------*
	 * ЛОГИРОВАНИЕ
	 *---------------------------------------------------------*/
	if @logEnabled = 1
	begin
		INSERT INTO [dbo].[PersonalDataAccessLog](
			[Method], 
			[OldData], 
			[NewData], 
			[ObjectType], 
			[AccessMethod], 
			[InstitutionID], 
			[UserLogin], 
			[ObjectID], 
			[AccessDate])
		SELECT 
			'D',		
			'[{"ApplicationUID":"' + ISNULL(a_db.[UID], '') + 
			'","ApplicationNumber":"' + a_db.ApplicationNumber + 
			'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
				DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a_db.RegistrationDate)) AS VARCHAR) + 
			'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
			',"EntrantUID":"' + ISNULL(e.[UID], '') + 
			'","EntrantDocumentID":' + CAST(ISNULL(e.IdentityDocumentID, 0) AS VARCHAR) + 
			',"EntrantID":' + CAST(a_db.EntrantID AS VARCHAR) + '}]',		
			NULL,
			'Application',
			'ImportDeleteApplication',		
			a_db.InstitutionID,
			@userLogin,
			NULL,
			GETDATE()	
		FROM 		
			dbo.[Application] a_db
			JOIN #applicationIds a ON a_db.ApplicationID = a.Id
			JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID
		WHERE a_db.InstitutionID = @institutionId
	end

	UPDATE dbo.Entrant WITH (UPDLOCK, ROWLOCK)
	SET IdentityDocumentID = NULL
	WHERE IdentityDocumentID IN (SELECT Id FROM @entrantDocumentIds)	
	
	DELETE a FROM [EntrantDocumentEgeAndOlympicSubject] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympicTotal] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympic] a WITH (UPDLOCK, ROWLOCK)	 
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEge] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEdu] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentDisability] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentCustom] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentIdentity] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [OrderOfAdmissionHistory] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntranceTestDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntrantDocument] a WITH (UPDLOCK, ROWLOCK)	
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [EntrantDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [ApplicationSelectedCompetitiveGroupTarget] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroupItem] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroup] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationConsidered] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID 		
	
	DELETE a FROM [dbo].[ApplicationCompetitiveGroupItem] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationId
	
	DECLARE @entrantIds TABLE (Id INT NOT NULL)
	INSERT INTO @entrantIds
	SELECT e.EntrantID
	FROM [dbo].[Application] a 
		JOIN Entrant e ON a.EntrantID = e.EntrantID
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	WHERE a.InstitutionID = @institutionId AND
		NOT EXISTS(SELECT * FROM [EntrantDocument] ed WHERE ed.EntrantID = e.EntrantID)

	DELETE a FROM [dbo].[Application] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	WHERE InstitutionID = @institutionId
	
	/* Чистим энтрантов и персонов */
	DELETE e FROM EntrantLanguage e WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantIds ei ON ei.Id = e.EntrantID
	WHERE 
		NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID)	
	
	DELETE e FROM Entrant e WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantIds ei ON ei.Id = e.EntrantID
	WHERE 
		NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID) AND 		
		NOT EXISTS(SELECT * FROM [EntrantLanguage] el WHERE el.EntrantID = e.EntrantID)

	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT
		(SELECT 
			[ApplicationNumber] AS '@ApplicationNumber',
			[RegistrationDate] AS '@RegistrationDate'
			FROM #InOrderApplications
		 FOR
		 XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationIsInOrder',
		(SELECT 	
			[ApplicationNumber] AS '@ApplicationNumber',
			[RegistrationDate] AS '@RegistrationDate'		
			FROM #NotFoundApplications
		 FOR
		 XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationIsNotFound'
	FOR XML PATH(''),
	ROOT('DeleteApplicationsResult')		

	SET NOCOUNT OFF

GO

/****** Object:  StoredProcedure [dbo].[Direction_Transfer]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Direction_Transfer](@firstLoad BIT = 0)
WITH EXECUTE AS OWNER
AS
SET NOCOUNT ON
SET XACT_ABORT ON

DROP INDEX trsDirection.IX_trsDirection_ParentCode
TRUNCATE TABLE trsDirection

INSERT trsDirection (Code, [Name], IsLeaf, ParentCode)
SELECT Code, [Name], 1,
CASE WHEN Code LIKE '____[1-9]_' OR Code LIKE '_____[1-9]' THEN LEFT(Code, 4) + '00'
	WHEN Code LIKE '__[1-9]_00' OR Code LIKE '___[1-9]00' THEN LEFT(Code, 2) + '0000' END AS ParentCode
FROM #Direction

CREATE INDEX IX_trsDirection_ParentCode ON trsDirection(ParentCode)

-- проставляем "не-листья"
UPDATE trsDirection SET IsLeaf = 0 WHERE EXISTS(SELECT * FROM trsDirection td WHERE td.ParentCode = trsDirection.Code)

-- оставляем только два уровня (листья и их верхние родители)
DELETE trsDirection WHERE IsLeaf = 0 AND Code NOT LIKE '__0000'

-- задаем DirectionID для старых
UPDATE trsDirection SET DirectionID = d.DirectionID FROM Direction d WHERE trsDirection.Code = d.Code
UPDATE trsDirection SET ParentDirectionID = pd.ParentDirectionID FROM ParentDirection pd WHERE trsDirection.Code = pd.Code

-- обновляем названия
UPDATE Direction SET [Name] = td.[Name] FROM trsDirection td 
	WHERE Direction.DirectionID = td.DirectionID
UPDATE ParentDirection SET [Name] = td.[Name] FROM trsDirection td 
	WHERE ParentDirection.ParentDirectionID = td.ParentDirectionID

-- вставляем не-листья
INSERT INTO ParentDirection (Code, [Name])
	SELECT DISTINCT td.Code, td.[Name]
	  FROM trsDirection td WHERE td.IsLeaf = 0 AND td.ParentDirectionID IS NULL
	  
-- вставляем листья
INSERT INTO Direction (Code, [Name])
	SELECT DISTINCT td.Code, td.[Name]
	  FROM trsDirection td WHERE td.IsLeaf = 1 AND td.DirectionID IS NULL

-- обновляем ParentID
UPDATE d SET ParentID = pd.ParentDirectionID FROM Direction d 
	JOIN ParentDirection pd ON d.Code LIKE LEFT(pd.Code,2) + '%'
WHERE d.ParentID IS NULL

-- UPDATE Direction SET ParentID = NULL 
-- DELETE Direction WHERE Code LIKE '__0000'

-- COMMIT
TRUNCATE TABLE trsDirection

GO

/****** Object:  StoredProcedure [dbo].[FormOfLaw_Transfer]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[FormOfLaw_Transfer](@firstLoad BIT = 0)
	WITH EXECUTE AS OWNER
AS
SET NOCOUNT ON
SET XACT_ABORT ON

INSERT INTO FormOfLaw (Name, Code)
SELECT DISTINCT t.Name, t.Code FROM #FormOfLaw t LEFT JOIN FormOfLaw f ON f.Code = t.Code
WHERE f.Code IS NULL

UPDATE FormOfLaw SET Name = t.Name FROM #FormOfLaw t WHERE FormOfLaw.Code = t.Code


GO

/****** Object:  StoredProcedure [dbo].[gvuz_ValidateOtherApplicationsCount]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[gvuz_ValidateOtherApplicationsCount] 
 @appilationId INT, 
 @dateFrom DATETIME, 
 @dateTO DATETIME
 as
 
 SELECT 
 a.InstitutionID
FROM ApplicationEntrantDocument ad
JOIN EntrantDocument ed ON ad.EntrantDocumentID = ed.EntrantDocumentID
JOIN Application AS a ON a.ApplicationID = ad.ApplicationID
JOIN ApplicationStatusType s ON s.StatusID = a.StatusID
JOIN ApplicationSelectedCompetitiveGroup ag ON ag.ApplicationID = a.ApplicationID
JOIN ApplicationSelectedCompetitiveGroupItem item ON item.ApplicationID = a.ApplicationID
JOIN CompetitiveGroupItem AS gitem ON gitem.CompetitiveGroupItemID = item.ItemID AND gitem.EducationLevelID IN (2, 5)
JOIN CompetitiveGroup g ON g.CompetitiveGroupID = ag.CompetitiveGroupID
JOIN Entrant AS e ON e.EntrantID = a.EntrantID
JOIN
(
 SELECT 
 ed.DocumentTypeID,
 ISNULL(ed.DocumentSeries, '') AS DocumentSeries,
 ed.DocumentNumber,
 e.FirstName,
    e.LastName,
    ISNULL(e.MiddleName, '') AS MiddleName 
  FROM Entrant e
  JOIN EntrantDocument ed ON e.EntrantID = ed.EntrantID
) Doc ON Doc.FirstName = e.FirstName AND Doc.LastName = e.LastName
   AND Doc.DocumentTypeID = ed.DocumentTypeID
   AND Doc.DocumentNumber = ed.DocumentNumber
WHERE ed.DocumentTypeID IN (1, 2) 
 AND a.ApplicationID = @appilationId
 AND Doc.MiddleName = CASE WHEN LEN(ISNULL(e.MiddleName, '')) > 0 THEN e.MiddleName ELSE Doc.MiddleName END
 AND Doc.DocumentSeries = CASE WHEN LEN(ISNULL(ed.DocumentSeries, '')) > 0 AND ed.DocumentTypeID =1  
         THEN ed.DocumentSeries 
         ELSE Doc.DocumentSeries 
        END
 AND s.IsActiveApp = 1
 AND g.Course = 1
 AND  a.RegistrationDate >= @dateFrom AND a.RegistrationDate <= @dateTO
 GROUP BY a.ApplicationID,
 a.RegistrationDate,
 a.InstitutionID
GO

/****** Object:  StoredProcedure [dbo].[internal_AddCreatedAndModifiedDate]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[internal_AddCreatedAndModifiedDate](@name sysname)
AS
declare @trigInsert sysname, @trigUpdate sysname, @pk sysname
select @trigInsert = '[trig_' + @name + '_CreatedDate]', @trigUpdate = '[trig_' + @name + '_ModifiedDate]'

SELECT @pk = c.name from sys.objects o join sys.indexes i on i.object_id = o.object_id
	join sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
	join sys.columns c ON c.object_id = o.object_id AND ic.column_id = c.column_id
where o.type = 'U' and o.name = @name and i.is_primary_key = 1

IF OBJECT_ID(@trigInsert) IS NOT NULL EXEC('DROP TRIGGER ' + @trigInsert);
IF OBJECT_ID(@trigUpdate) IS NOT NULL EXEC('DROP TRIGGER ' + @trigUpdate);

IF COL_LENGTH(@name, 'CreatedDate') IS NULL
	EXEC('ALTER TABLE [' + @name + '] ADD CreatedDate datetime NULL')
IF COL_LENGTH(@name, 'ModifiedDate') IS NULL
	EXEC('ALTER TABLE [' + @name + '] ADD ModifiedDate datetime NULL')

EXEC('CREATE TRIGGER ' + @trigInsert + ' ON [' + @name + ']
AFTER INSERT AS
UPDATE [' + @name + '] SET CreatedDate = getdate() FROM inserted 
	WHERE [' + @name + '].[' + @pk + '] = inserted.[' + @pk + ']')
	
EXEC('CREATE TRIGGER ' + @trigUpdate + ' ON [' + @name + ']
AFTER UPDATE AS
UPDATE [' + @name + '] SET ModifiedDate = getdate() FROM inserted 
	WHERE [' + @name + '].[' + @pk + '] = inserted.[' + @pk + ']')


GO

/****** Object:  StoredProcedure [dbo].[Okato_Transfer]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Okato_Transfer](@firstLoad BIT = 0)
WITH EXECUTE AS OWNER
AS
SET NOCOUNT ON
SET XACT_ABORT ON

/*DECLARE @centrums TABLE(Code VARCHAR(16) PRIMARY KEY)

INSERT INTO @centrums
SELECT DISTINCT LEFT(Code, LEN(Code) - CHARINDEX('/', REVERSE(Code))) 
FROM #Okato WHERE [Name] LIKE 'Районы%/'
*/
DROP INDEX trsOkato.IX_trsOkato_Depth
DROP INDEX trsOkato.IX_trsOkato_ParentCode
TRUNCATE TABLE trsOkato

-- удаляем районы
/*DELETE FROM #Okato WHERE Code LIKE '%/%'
	AND EXISTS (SELECT * FROM #Okato t WHERE t.Code LIKE #Okato.Code + '/%')
	AND NOT EXISTS(SELECT * FROM #Okato t WHERE t.Code LIKE #Okato.Code + '/%' AND t.[Name] LIKE 'Районы%/')
*/

DECLARE @errorPK VARCHAR(16)

SELECT top 1 @errorPK = code FROM #Okato GROUP BY code HAVING COUNT(*) > 1
IF @errorPK IS NOT NULL
BEGIN
	RAISERROR(@errorPK, 18, 1)
	RETURN -1
END
	

INSERT trsOkato (Code, Razdel, [Name], Centrum, [Description], [Status], ModifiedDate, Depth, ParentCode)
SELECT Code, Razdel, [Name], Centrum, [Description], [Status], ModifiedDate,
LEN(Code)-LEN(REPLACE(Code, '/', '')) + 1 AS Depth,
CASE WHEN Code NOT LIKE '%/%' THEN NULL ELSE LEFT(Code, LEN(Code) - CHARINDEX('/', REVERSE(Code))) END AS ParentCode
FROM #Okato

CREATE INDEX IX_trsOkato_Depth ON trsOkato(Depth)
CREATE INDEX IX_trsOkato_ParentCode ON trsOkato(ParentCode)

-- удаляем районы
-- DELETE FROM trsOkato WHERE ParentCode IN (SELECT Code FROM @centrums)

IF @firstLoad = 0
BEGIN
	DELETE RegionType FROM trsOkato t WHERE t.Code = OkatoCode AND t.[Status] = 1
	
	UPDATE RegionType SET [Name] = t.[Name], OkatoModified = t.ModifiedDate
	FROM trsOkato t
	WHERE t.Depth = 1 AND t.[Status] = 2 AND CountryID = 1 
		AND t.Code = OkatoCode AND t.ModifiedDate > OkatoModified
	
	INSERT INTO RegionType (CountryID, [Name], OkatoCode, OkatoModified)
	SELECT DISTINCT 1, t.[Name], t.Code, t.ModifiedDate 
	FROM trsOkato t LEFT JOIN RegionType rt ON rt.OkatoCode = t.Code
		WHERE t.Depth = 1 AND t.[Status] IN (0,3) AND rt.RegionID IS NULL
			-- AND t.Code NOT IN('40', '45') -- исключаем Москву и Питер
	
	DELETE CityType FROM trsOkato t WHERE t.Code = OkatoCode AND t.[Status] = 1
	
	UPDATE CityType SET [Name] = t.[Name], [RegionID] = rt.RegionID,
		OkatoModified = t.ModifiedDate
	FROM trsOkato t JOIN trsOkato p ON t.Code LIKE p.Code + '/%'
		JOIN RegionType rt ON rt.OkatoCode = p.Code AND CountryID = 1 
	WHERE t.Depth > 1 AND t.[Status] = 2 AND CountryID = 1 
		AND t.Code = CityType.OkatoCode AND t.ModifiedDate > CityType.OkatoModified
		
	INSERT INTO CityType (RegionID, [Name], OkatoCode, OkatoModified)
	SELECT DISTINCT rt.RegionID, t.[Name], t.Code, t.ModifiedDate 
	FROM trsOkato t JOIN trsOkato p ON t.Code LIKE p.Code + '/%'
		JOIN RegionType rt ON rt.OkatoCode = p.Code AND CountryID = 1 
		LEFT JOIN CityType ct ON ct.OkatoCode = t.Code
	WHERE t.Depth > 1 AND t.[Status] IN (0,3) AND ct.CityID IS NULL 
		-- AND NOT EXISTS (SELECT * FROM trsOkato to1 WHERE t.Code = to1.ParentCode)
END
ELSE
BEGIN
	INSERT INTO RegionType (CountryID, [Name], OkatoCode, OkatoModified)
	SELECT DISTINCT 1, t.[Name], t.Code, t.ModifiedDate FROM trsOkato t
		WHERE t.Depth = 1 AND t.[Status] != 1 
			-- AND t.Code NOT IN('40', '45') -- исключаем Москву и Питер
	
	INSERT INTO CityType (RegionID, [Name], OkatoCode, OkatoModified)
	SELECT DISTINCT rt.RegionID, t.[Name], t.Code, t.ModifiedDate 
	FROM trsOkato t JOIN trsOkato p ON t.Code LIKE p.Code + '/%'
		JOIN RegionType rt ON rt.OkatoCode = p.Code AND CountryID = 1 
	WHERE t.Depth > 1 AND t.[Status] != 1 
		-- AND NOT EXISTS (SELECT * FROM trsOkato to1 WHERE t.Code = to1.ParentCode)
END

SELECT COUNT(*) AS TotalRecords FROM trsOkato

TRUNCATE TABLE trsOkato


GO

/****** Object:  StoredProcedure [dbo].[PerformSearch]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[PerformSearch]
@IsVUZ BIT,
@IsSSUZ BIT,
@NamePart VARCHAR(500) = NULL,
@DirectionName VARCHAR(100) = NULL,
@DirectionCode VARCHAR(6) = NULL,
@RegionName VARCHAR(500) = NULL,
@FormOfLawID INT = NULL,
@EducationLevelID SMALLINT = NULL,
@StudyID SMALLINT = NULL,
@AdmissionTypeID SMALLINT = NULL,
@HasMilitaryDepartment BIT = NULL,
@HasPreparatoryCourses BIT = NULL,
@HasOlympics BIT = NULL,
@DepthLimit SMALLINT = NULL,
@ParentStructureID INT = NULL,
@PageSize INT = NULL,
@PageNumber INT = NULL,
@Snils VARCHAR(50),
@TotalPageCount INT = NULL OUT
AS
SET NOCOUNT ON

DECLARE @institutions TABLE(ID INT IDENTITY PRIMARY KEY, InstitutionID INT UNIQUE,
	FullName VARCHAR(500), 	ReceivingApplicationDate DATETIME)

SELECT @DepthLimit = COALESCE(@DepthLimit, 10), @PageSize = COALESCE(@PageSize, 50),
	@PageNumber = COALESCE(@PageNumber, 1)

SELECT @FormOfLawID = CASE WHEN @FormOfLawID != 0 THEN @FormOfLawID END,
	@EducationLevelID = CASE WHEN @EducationLevelID != 0 THEN @EducationLevelID END,
	@StudyID = CASE WHEN @StudyID != 0 THEN @StudyID END,
	@AdmissionTypeID = CASE WHEN @AdmissionTypeID != 0 THEN @AdmissionTypeID END,
	@NamePart = CASE WHEN LEN(@NamePart) != 0 THEN @NamePart END,
	@DirectionName = CASE WHEN LEN(@DirectionName) != 0 THEN @DirectionName END,
	@DirectionCode = CASE WHEN LEN(@DirectionCode) != 0 THEN @DirectionCode END,
	@RegionName = CASE WHEN LEN(@RegionName) != 0 THEN @RegionName END
	
DECLARE @maxItemLevel SMALLINT, @startPagerIndex INT, @endPagerIndex INT, @entrantID INT
SET @startPagerIndex = ((@PageNumber-1) * @PageSize) + 1
SET @endPagerIndex = @startPagerIndex + @PageSize - 1

IF @EducationLevelID IS NOT NULL SET @maxItemLevel = 2

IF COALESCE(@DirectionName,@DirectionCode) IS NOT NULL 
BEGIN
	SET @maxItemLevel = 6
END

SELECT @entrantID = e.EntrantID FROM Entrant e WHERE e.SNILS = @Snils --AND e.IsHistoric = 0

IF @StudyID IS NOT NULL SET @maxItemLevel = 7
IF @AdmissionTypeID IS NOT NULL SET @maxItemLevel = 8

IF @ParentStructureID IS NULL
BEGIN 
	INSERT @institutions (InstitutionID, FullName, ReceivingApplicationDate)
	SELECT DISTINCT i.InstitutionID, ad.[Name], i.ReceivingApplicationDate
	FROM Institution i
	-- получаем денормализованные данные для поиска
	JOIN AdmissionData ad ON ad.InstitutionID = i.InstitutionID AND ad.ItemLevel = 0
	LEFT JOIN AdmissionData sub ON sub.InstitutionID = i.InstitutionID AND sub.ItemLevel > 0
	LEFT JOIN Direction d ON sub.DirectionID = d.DirectionID
	-- подключаем фильтр по регионам
	LEFT JOIN RegionType reg ON reg.RegionID = i.RegionID
	WHERE 
		-- проверяем тип ОУ
		((@IsVUZ = 1 AND @IsSSUZ = 1) OR 
		(@IsVUZ = 1 AND @IsSSUZ = 0 AND i.InstitutionTypeID = 1) OR
		(@IsVUZ = 0 AND @IsSSUZ = 1 AND i.InstitutionTypeID = 2)
		) 
		-- ищем по части названия ОУ
		AND (@NamePart IS NULL OR ad.[Name] LIKE @NamePart)
		-- ищем по части названия Региона
		AND (@RegionName IS NULL OR reg.[Name] LIKE @RegionName)
		-- ищем по правовой форме
		AND (@FormOfLawID IS NULL OR i.FormOfLawID = @FormOfLawID)
		-- ищем по флажкам
		AND (@HasMilitaryDepartment IS NULL OR ad.HasMilitaryDepartment = @HasMilitaryDepartment)
		AND (@HasPreparatoryCourses IS NULL OR ad.HasPreparatoryCourses = @HasPreparatoryCourses)
		AND (@HasOlympics IS NULL OR ad.HasOlympics = @HasOlympics)
		AND (@EducationLevelID IS NULL OR sub.EducationLevelID = @EducationLevelID)
		AND (@DirectionName IS NULL OR d.[Name] LIKE @DirectionName)
		AND (@DirectionCode IS NULL OR d.[Code] LIKE @DirectionCode)
		AND (@StudyID IS NULL OR sub.StudyID = @StudyID)
		AND (@AdmissionTypeID IS NULL OR sub.AdmissionTypeID = @AdmissionTypeID)
	ORDER BY ad.[Name]
		
	SET @TotalPageCount = CEILING(CAST(@@ROWCOUNT AS MONEY) / @PageSize)
	
	SELECT i.InstitutionID, ad.AdmissionStructureID, ad.AdmissionItemID, 
		ad.ItemLevel, ad.ParentID, ad.[Name],
		ad.PlaceCount, 
		(SELECT CASE WHEN COUNT(*) > 0 THEN COUNT(*) END FROM [Application] a WHERE a.InstitutionID = i.InstitutionID
			AND a.StatusID > 1 AND a.StatusID != 5) AS ApplicationCount, 
		ad.HasOlympics,	ad.HasPreparatoryCourses, ad.HasMilitaryDepartment,
		CAST(CASE WHEN ad.ItemLevel > 0 AND i.ReceivingApplicationDate < GETDATE() THEN 1 END AS BIT) AS Applicable,
		CAST(CASE WHEN ad.ItemLevel > 0 AND i.ReceivingApplicationDate < GETDATE() THEN 1 END AS BIT) AS CanBeChecked,
		ast.StatusID AS ApplicationStatusID, ast.[Name] AS ApplicationStatus, a.ApplicationID,
		a.EntrantApplicationCount, ad.IsLeaf
	FROM @institutions i
		JOIN AdmissionData ad ON ad.InstitutionID = i.InstitutionID
		JOIN AdmissionItemPublished aip ON aip.AdmissionItemID = ad.AdmissionItemID
		LEFT JOIN (SELECT ai.AdmissionItemID, MAX(a.ApplicationID) AS ApplicationID, 
			COUNT(*) AS EntrantApplicationCount FROM [Application] a
			JOIN ApplicationItem ai ON ai.ApplicationID = a.ApplicationID
			WHERE a.EntrantID = @entrantID GROUP BY ai.AdmissionItemID
			) a ON a.AdmissionItemID = aip.AdmissionItemID
		LEFT JOIN [Application] sa ON sa.ApplicationID = a.ApplicationID
		LEFT JOIN ApplicationStatusType ast ON ast.StatusID = sa.StatusID
	WHERE ad.Depth < @DepthLimit + 1
		AND i.ID BETWEEN @startPagerIndex AND @endPagerIndex
		-- "режем" ветки (чтобы не отображать лишнее)
		AND (@maxItemLevel IS NULL OR 
		EXISTS(SELECT 1 FROM AdmissionData mad 
			LEFT JOIN Direction d ON mad.DirectionID = d.DirectionID
		    WHERE mad.InstitutionID = i.InstitutionID AND mad.ItemLevel = @maxItemLevel
			AND ((ad.ItemLevel <= @maxItemLevel AND mad.Lineage LIKE ad.Lineage + '%')
			OR (ad.ItemLevel > @maxItemLevel AND ad.Lineage LIKE mad.Lineage + '%'))
			AND (@EducationLevelID IS NULL OR mad.EducationLevelID = @EducationLevelID)
			AND (@DirectionName IS NULL OR d.[Name] LIKE @DirectionName)
			AND (@DirectionCode IS NULL OR d.[Code] LIKE @DirectionCode)
			AND (@StudyID IS NULL OR mad.StudyID = @StudyID)
			AND (@AdmissionTypeID IS NULL OR mad.AdmissionTypeID = @AdmissionTypeID)
			))
		
	-- порядок сортировки важен для создания иерархии на C# (поэтому нельзя по названию)
	ORDER BY ad.Depth, ad.Lineage
	-- TODO: попробовать поменять сортировку (если подерживать только json - тогда в коде можно убрать лишние классы и параметры)
END
ELSE
BEGIN
	SELECT i.InstitutionID, ad.AdmissionStructureID, ad.AdmissionItemID, 
		ad.ItemLevel, ad.ParentID, ad.[Name],
		ad.PlaceCount, 
		(SELECT CASE WHEN COUNT(*) > 0 THEN COUNT(*) END FROM [Application] a WHERE a.InstitutionID = i.InstitutionID
			AND a.StatusID > 1 AND a.StatusID != 5) AS ApplicationCount, 
		ad.HasOlympics,	ad.HasPreparatoryCourses, ad.HasMilitaryDepartment,
		CAST(CASE WHEN ad.ItemLevel > 0 AND i.ReceivingApplicationDate < GETDATE() THEN 1 END AS BIT) AS Applicable,
		CAST(CASE WHEN ad.ItemLevel > 0 AND i.ReceivingApplicationDate < GETDATE() THEN 1 END AS BIT) AS CanBeChecked,
		ast.StatusID AS ApplicationStatusID, ast.[Name] AS ApplicationStatus, a.ApplicationID,
		a.EntrantApplicationCount, ad.IsLeaf
	FROM Institution i
	-- получаем рутовый айтем
	JOIN AdmissionItemPublished r ON r.InstitutionID = i.InstitutionID 
	JOIN AdmissionStructurePublished rs ON rs.AdmissionItemID = r.AdmissionItemID
	JOIN AdmissionItemType art ON art.ItemTypeID = r.ItemTypeID
	-- получаем дерево (по рутовому айтему)
	JOIN AdmissionData ad ON ad.InstitutionID = i.InstitutionID AND ad.ItemLevel >= art.ItemLevel
		AND ad.Lineage LIKE rs.Lineage + '%'
	JOIN AdmissionItemPublished aip ON aip.AdmissionItemID = ad.AdmissionItemID
	LEFT JOIN (SELECT ai.AdmissionItemID, MAX(a.ApplicationID) AS ApplicationID, 
			COUNT(*) AS EntrantApplicationCount FROM [Application] a
			JOIN ApplicationItem ai ON ai.ApplicationID = a.ApplicationID
			WHERE a.EntrantID = @entrantID GROUP BY ai.AdmissionItemID
			) a ON a.AdmissionItemID = aip.AdmissionItemID
	LEFT JOIN [Application] sa ON sa.ApplicationID = a.ApplicationID
	LEFT JOIN ApplicationStatusType ast ON ast.StatusID = sa.StatusID
	WHERE rs.AdmissionStructureID = @ParentStructureID AND ad.Depth - rs.Depth < @DepthLimit
		AND (@maxItemLevel IS NULL OR 
		EXISTS(SELECT 1 FROM AdmissionData mad 
			LEFT JOIN Direction d ON mad.DirectionID = d.DirectionID
		    WHERE mad.InstitutionID = i.InstitutionID AND mad.ItemLevel = @maxItemLevel
			AND ((ad.ItemLevel <= @maxItemLevel AND mad.Lineage LIKE ad.Lineage + '%')
			OR (ad.ItemLevel > @maxItemLevel AND ad.Lineage LIKE mad.Lineage + '%'))
			AND (@EducationLevelID IS NULL OR mad.EducationLevelID = @EducationLevelID)
			AND (@DirectionName IS NULL OR d.[Name] LIKE @DirectionName)
			AND (@DirectionCode IS NULL OR d.[Code] LIKE @DirectionCode)
			AND (@StudyID IS NULL OR mad.StudyID = @StudyID)
			AND (@AdmissionTypeID IS NULL OR mad.AdmissionTypeID = @AdmissionTypeID)
			))
	-- порядок сортировки важен для создания иерархии на C# (поэтому нельзя по названию)
	ORDER BY ad.Depth, ad.Lineage
END


GO

/****** Object:  StoredProcedure [dbo].[ProfessionType_Transfer]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[ProfessionType_Transfer]
	WITH EXECUTE AS OWNER
AS
SET NOCOUNT ON
SET XACT_ABORT ON

-- должны импортироваться записи со значением поля «Код» длиной 7 знаков и не содержащим нули одновременно в 5-ом и 6-ом разряде.
DELETE FROM #ProfessionType WHERE Code IS NULL OR LEN(Code) <> 7 OR SUBSTRING(Code, 5, 2) = '00'

DELETE ProfessionType FROM #ProfessionType t
WHERE t.Code = ProfessionType.Code AND t.Status = 1

-- first update existing rows in ProfessionType for avoiding key constraints
UPDATE ProfessionType SET Name = t.[Name], Code = t.Code
FROM #ProfessionType t 
WHERE ProfessionType.Code = t.Code AND t.Status = 2

INSERT INTO ProfessionType (Code, Name)
SELECT DISTINCT t.Code, t.[Name]
FROM #ProfessionType t LEFT JOIN ProfessionType f ON f.Code = t.Code
WHERE f.Name IS NULL AND t.Status IN (0,3)


GO

/****** Object:  StoredProcedure [dbo].[PublishAdmissionData]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROC [dbo].[PublishAdmissionData]
	@institutionID INT
AS
SET XACT_ABORT ON
SET NOCOUNT ON

DECLARE @needSqlTran BIT, @RC INT
IF @@TRANCOUNT = 0 SET @needSqlTran = 1
ELSE SET @needSqlTran = 0

IF @needSqlTran = 1 BEGIN TRAN	

DELETE FROM AdmissionData WHERE InstitutionID = @institutionID

INSERT INTO AdmissionData
(AdmissionStructureID, AdmissionItemID, InstitutionID, ParentID, Depth,	Lineage,
	ItemTypeID, ItemLevel, PlaceCount, TotalDirectionPlaceCount,
	Name, HasMilitaryDepartment, HasPreparatoryCourses,	HasOlympics,
	EducationLevelID, DirectionID, StudyID,	AdmissionTypeID)
SELECT asp.AdmissionStructureID, asp.AdmissionItemID, aip.InstitutionID, 
	asp.ParentID, asp.Depth, asp.Lineage,
	aip.ItemTypeID, ait.ItemLevel, aip.PlaceCount, aip.TotalDirectionPlaceCount,
	CASE WHEN ait.ItemTypeID = 0 THEN i.FullName ELSE aip.[Name] END AS [Name],
	CAST(CASE WHEN ait.ItemTypeID = 0 THEN i.HasMilitaryDepartment END AS BIT) AS HasMilitaryDepartment,
	CAST(CASE WHEN ait.ItemTypeID = 0 AND pc.InstitutionID IS NULL THEN 0 
		WHEN ait.ItemTypeID = 0 AND pc.InstitutionID IS NOT NULL THEN 1 END AS BIT) AS HasPreparatoryCourses,
	CAST(CASE WHEN ait.ItemTypeID = 0 AND bi.InstitutionID IS NULL THEN 0 
		WHEN ait.ItemTypeID = 0 AND bi.InstitutionID IS NOT NULL THEN 1 END AS BIT)AS HasOlympics,
	CASE WHEN ait.ItemLevel = 2 THEN ait.ItemTypeID END AS EducationLevelID, 
	CASE WHEN ait.ItemLevel = 6 THEN aip.DirectionID END AS DirectionID,
	CASE WHEN ait.ItemLevel = 7 THEN ait.ItemTypeID END AS StudyID,
	CASE WHEN ait.ItemLevel = 8 THEN ait.ItemTypeID END AS AdmissionTypeID
FROM AdmissionItemPublished aip 
JOIN AdmissionStructurePublished asp ON aip.AdmissionItemID = asp.AdmissionItemID
JOIN Institution i ON i.InstitutionID = @institutionID
JOIN AdmissionItemType ait ON ait.ItemTypeID = aip.ItemTypeID
-- TODO: РІРёРґРёРјРѕ РЅСѓР¶РЅРѕ Р±СѓРґРµС‚ Р·Р°РјРµРЅРёС‚СЊ РЅР° BenefitItemPublished Рё PreparatoryCoursePublished
LEFT JOIN (SELECT bi.InstitutionID, MIN(bi.BenefitItemID) AS BenefitItemID
	FROM BenefitItem bi WHERE bi.InstitutionID = @institutionID GROUP BY bi.InstitutionID
	) bi ON bi.InstitutionID = @institutionID
LEFT JOIN (SELECT pc.InstitutionID, MIN(pc.PreparatoryCourseID) AS PreparatoryCourseID
	FROM PreparatoryCourse pc WHERE pc.InstitutionID = @institutionID GROUP BY pc.InstitutionID
	) pc ON pc.InstitutionID = @institutionID
WHERE aip.InstitutionID = @institutionID

UPDATE ad SET EducationLevelID = p.EducationLevelID
FROM AdmissionData ad  JOIN AdmissionData p ON ad.Lineage LIKE p.Lineage + '%' AND p.ItemLevel < ad.ItemLevel
WHERE ad.InstitutionID = @institutionID AND p.InstitutionID = @institutionID AND p.ItemLevel = 2

UPDATE ad SET DirectionID = p.DirectionID
FROM AdmissionData ad JOIN AdmissionData p ON ad.Lineage LIKE p.Lineage + '%' AND p.ItemLevel < ad.ItemLevel
WHERE ad.InstitutionID = @institutionID AND p.InstitutionID = @institutionID AND p.ItemLevel = 6

UPDATE ad SET StudyID = p.StudyID
FROM AdmissionData ad JOIN AdmissionData p ON ad.Lineage LIKE p.Lineage + '%' AND p.ItemLevel < ad.ItemLevel
WHERE ad.InstitutionID = @institutionID AND p.InstitutionID = @institutionID AND p.ItemLevel = 7

UPDATE AdmissionData SET IsLeaf = 1
WHERE AdmissionData.InstitutionID = @institutionID
	AND NOT EXISTS(
		SELECT * FROM AdmissionData ad 
	    WHERE ad.ParentID = AdmissionData.AdmissionStructureID
	    AND ad.InstitutionID = @institutionID
	)

IF @needSqlTran = 1 COMMIT

GO

/****** Object:  StoredProcedure [dbo].[PublishAdmissionStructure]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROC [dbo].[PublishAdmissionStructure]
	@institutionID INT
AS
SET XACT_ABORT ON
SET NOCOUNT ON

DECLARE @instID INT

SELECT @instID = InstitutionID
FROM Institution i 
WHERE i.InstitutionID = @institutionID

IF @instID IS NULL 
BEGIN
	RAISERROR('РЈСЂРѕРІРµРЅСЊ РѕР±СЂР°Р·РѕРІР°РЅРёСЏ РЅРµ РЅР°Р№РґРµРЅ', 18, 1)
	RETURN -1
END

DECLARE @needSqlTran BIT, @RC INT
IF @@TRANCOUNT = 0 SET @needSqlTran = 1
ELSE SET @needSqlTran = 0

IF @needSqlTran = 1 BEGIN TRAN	

DECLARE @parentStructureID INT
SELECT @parentStructureID = as1.AdmissionStructureID FROM AdmissionItem ai JOIN AdmissionStructure as1 ON as1.AdmissionItemID = ai.AdmissionItemID
WHERE ai.ItemTypeID=0 AND ai.InstitutionID=@instID

DECLARE @parentStructureIDPublished INT
SELECT @parentStructureIDPublished = as1.AdmissionStructureID FROM AdmissionItem ai JOIN AdmissionStructurePublished as1 ON as1.AdmissionItemID = ai.AdmissionItemID
WHERE ai.ItemTypeID=0 AND ai.InstitutionID=@instID


DELETE FROM EntranceTestItemPublished WHERE AdmissionItemID IN (SELECT AdmissionItemID FROM AdmissionItemPublished)
DELETE FROM AdmissionStructurePublished WHERE Lineage LIKE ('/' + CAST(@parentStructureIDPublished AS VARCHAR) + '/%')
DELETE FROM AdmissionItemPublished WHERE InstitutionID=@instID

INSERT INTO AdmissionItemPublished
SELECT * FROM AdmissionItem WHERE InstitutionID=@instID

INSERT INTO AdmissionStructurePublished
SELECT * FROM AdmissionStructure WHERE Lineage LIKE ('/' + CAST(@parentStructureID AS VARCHAR) + '/%')

INSERT INTO EntranceTestItemPublished
SELECT * FROM EntranceTestItem WHERE AdmissionItemID IN (SELECT AdmissionItemID FROM AdmissionItemPublished)

EXEC @RC = PublishAdmissionData @institutionID = @instID
IF @@ERROR != 0 OR @RC != 0
BEGIN
	IF @needSqlTran = 1 AND @@TRANCOUNT > 0 ROLLBACK
	RETURN -2
END

UPDATE Institution SET AdmissionStructurePublishDate=GETDATE() WHERE institutionID=@instID

IF @needSqlTran = 1 COMMIT

GO

/****** Object:  StoredProcedure [dbo].[SendApplication]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[SendApplication] @applicationID INT
AS
	SET NOCOUNT ON
	SET XACT_ABORT ON 

	DECLARE @needSqlTran BIT, @RC INT
	IF @@TRANCOUNT = 0 SET @needSqlTran = 1
	ELSE SET @needSqlTran = 0

	IF(SELECT StatusID FROM [Application] WHERE ApplicationID=@applicationID) IS NULL
	BEGIN
		RAISERROR('Заявление не найдено', 18, 1)
		RETURN -1
	END

	IF(SELECT StatusID FROM [Application] WHERE ApplicationID=@applicationID) != 1
	BEGIN
		RAISERROR('Заявление уже подано', 18, 1)
		RETURN -1
	END

	IF @needSqlTran = 1 BEGIN TRAN	

	DECLARE @entrantID INT
	DECLARE @newEntrantPerson INT
	DECLARE @newRegAddressID INT
	DECLARE @newFactAddressID INT

	SELECT @entrantID = EntrantID 
	FROM [Application] a WHERE a.ApplicationID = @applicationID

	IF(SELECT TOP 1 RegistrationAddressID FROM Entrant WHERE EntrantID=@entrantID) IS NOT NULL
	BEGIN
	INSERT INTO [Address] (	CountryID,	RegionID,	CityName,	PostalCode,	Street,	Building, BuildingPart, Room,	Phone, RegionName, CountryName)
	SELECT a.CountryID, a.RegionID, a.CityName, a.PostalCode, a.Street, a.Building, a.BuildingPart, a.Room, a.Phone, a.RegionName, a.CountryName
	  FROM [Address] a WHERE a.AddressID = (SELECT TOP 1 RegistrationAddressID FROM Entrant WHERE EntrantID=@entrantID)
	SELECT @newRegAddressID=SCOPE_IDENTITY()
	END

	IF(SELECT TOP 1 FactAddressID FROM Entrant WHERE EntrantID=@entrantID) IS NOT NULL
	BEGIN
	INSERT INTO [Address] (	CountryID,	RegionID,	CityName,	PostalCode,	Street,	Building, BuildingPart,	Room,	Phone, RegionName, CountryName)
	SELECT a.CountryID, a.RegionID, a.CityName, a.PostalCode, a.Street, a.Building, a.BuildingPart, a.Room, a.Phone, a.RegionName, a.CountryName
	  FROM [Address] a WHERE a.AddressID = (SELECT TOP 1 FactAddressID FROM Entrant WHERE EntrantID=@entrantID)
	SELECT @newFactAddressID=SCOPE_IDENTITY()
	END

	INSERT INTO EntrantLanguage (EntrantID,	LanguageID)
	SELECT @entrantID, LanguageID FROM EntrantLanguage el WHERE el.EntrantID=@entrantID

	UPDATE [Application]
	SET StatusID = (CASE WHEN SourceID=1 THEN 2 ELSE 4 END)
	WHERE @applicationID = ApplicationID

	IF @needSqlTran = 1 COMMIT

GO

/****** Object:  StoredProcedure [dbo].[sys_DisableAllIndexes]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[sys_DisableAllIndexes](@tableName VARCHAR(100))
AS
	DECLARE @sql AS VARCHAR(MAX)='';
	SELECT @sql = @sql + 
		'ALTER INDEX ' + sys.indexes.name  + ' ON ' + sys.objects.name  + ' DISABLE;' +CHAR(13)+CHAR(10)
	FROM    sys.indexes
			JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
	WHERE   sys.indexes.type_desc = 'NONCLUSTERED'
			AND sys.objects.name = @tableName
	EXEC(@sql);

GO

/****** Object:  StoredProcedure [dbo].[sys_DropColumnIndexes]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sys_DropColumnIndexes](@tableName VARCHAR(100), @columnName VARCHAR(100))
AS
	-- drop all indexes from column
	declare @sqlDropIndex NVARCHAR(1000)
	WHILE 1=1
	BEGIN	
		select @sqlDropIndex = 'DROP INDEX ' + i.name + ' ON ' + t.name
		from sys.tables t
		inner join sys.schemas s on t.schema_id = s.schema_id
		inner join sys.indexes i on i.object_id = t.object_id
		inner join sys.index_columns ic on ic.object_id = t.object_id
			inner join sys.columns c on c.object_id = t.object_id and
				ic.column_id = c.column_id
		where i.index_id > 0    
		and i.type in (1, 2) -- clustered & nonclustered only
		and i.is_primary_key = 0 -- do not include PK indexes
		and i.is_unique_constraint = 0 -- do not include UQ
		--and i.is_disabled = 0
		and i.is_hypothetical = 0
		--and ic.key_ordinal > 0
		and t.NAME = @tableName AND c.name = @columnName
		order by ic.key_ordinal

		IF (@@ROWCOUNT = 0)
			BREAK;

		print @sqlDropIndex
		exec sp_executeSql @sqlDropIndex
	END

GO

/****** Object:  StoredProcedure [dbo].[sys_DropColumnStatisticts]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sys_DropColumnStatisticts](@tableName VARCHAR(100), @columnName VARCHAR(100))
AS
	declare @sqlDropStats NVARCHAR(1000)
	WHILE 1=1
	BEGIN	
		select @sqlDropStats = 'DROP STATISTICS ' + t.name + '.' + s.name
		  FROM sys.stats     s 
		  JOIN sys.tables    t
			ON s.object_id = t.object_id
		INNER JOIN sys.stats_columns sc
		ON sc.stats_id = s.stats_id AND sc.object_id = s.object_id
		INNER JOIN sys.columns c 
		ON c.column_id = sc.column_id AND c.object_id = sc.object_id
		 WHERE s.object_id > 100 AND s.user_created = 1
		   AND s.name NOT IN 
				 (SELECT name FROM sys.indexes WHERE object_id = s.object_id)
		   AND t.name = @tableName
		   AND c.Name = @columnName

		IF (@@ROWCOUNT = 0)
			BREAK;

		print @sqlDropStats
		exec sp_executeSql @sqlDropStats
	END 

GO

/****** Object:  StoredProcedure [dbo].[sys_EnableAllIndexes]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[sys_EnableAllIndexes](@tableName VARCHAR(100))
AS
	DECLARE @sql AS VARCHAR(MAX)='';
	SELECT @sql = @sql + 
		'ALTER INDEX ' + sys.indexes.name  + ' ON ' + sys.objects.name  + ' REBUILD;' +CHAR(13)+CHAR(10)
	FROM    sys.indexes
			JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
	WHERE   sys.indexes.type_desc = 'NONCLUSTERED'
			AND sys.objects.name = @tableName
	EXEC(@sql);

GO

/****** Object:  StoredProcedure [dbo].[test_PerformSearch]    Script Date: 05/29/2014 13:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[test_PerformSearch]
@IsVUZ BIT,
@IsSSUZ BIT,
@NamePart VARCHAR(500) = NULL,
@DirectionName VARCHAR(100) = NULL,
@DirectionCode VARCHAR(6) = NULL,
@RegionName VARCHAR(500) = NULL,
@FormOfLawID INT = NULL,
@EducationLevelID SMALLINT = NULL,
@StudyID SMALLINT = NULL,
@AdmissionTypeID SMALLINT = NULL,
@HasMilitaryDepartment BIT = NULL,
@HasPreparatoryCourses BIT = NULL,
@HasOlympics BIT = NULL,
@DepthLimit SMALLINT = NULL,
@ParentStructureID INT = NULL,
@PageSize INT = NULL,
@PageNumber INT = NULL,
@TotalPageCount INT = NULL OUT
AS
SET NOCOUNT ON

DECLARE @institutions TABLE(ID INT IDENTITY PRIMARY KEY, InstitutionID INT UNIQUE,
	FullName VARCHAR(500), HasMilitaryDepartment BIT, HasPreparatoryCourses BIT, HasOlympics BIT)

SELECT @DepthLimit = COALESCE(@DepthLimit, 10), @PageSize = COALESCE(@PageSize, 50),
	@PageNumber = COALESCE(@PageNumber, 1)

SELECT @FormOfLawID = CASE WHEN @FormOfLawID != 0 THEN @FormOfLawID END,
	@EducationLevelID = CASE WHEN @EducationLevelID != 0 THEN @EducationLevelID END,
	@StudyID = CASE WHEN @StudyID != 0 THEN @StudyID END,
	@AdmissionTypeID = CASE WHEN @AdmissionTypeID != 0 THEN @AdmissionTypeID END,
	@NamePart = CASE WHEN LEN(@NamePart) != 0 THEN @NamePart END,
	@DirectionName = CASE WHEN LEN(@DirectionName) != 0 THEN @DirectionName END,
	@DirectionCode = CASE WHEN LEN(@DirectionCode) != 0 THEN @DirectionCode END,
	@RegionName = CASE WHEN LEN(@RegionName) != 0 THEN @RegionName END
	
DECLARE @filterWithSubLevels SMALLINT, @startPagerIndex INT, @endPagerIndex INT 
SET @startPagerIndex = ((@PageNumber-1) * @PageSize) + 1
SET @endPagerIndex = @startPagerIndex + @PageSize - 1

SET @filterWithSubLevels = 0

IF @EducationLevelID IS NOT NULL SET @filterWithSubLevels = @filterWithSubLevels + 1

IF COALESCE(@DirectionName,@DirectionCode) IS NOT NULL 
	SET @filterWithSubLevels = @filterWithSubLevels + 2

IF @StudyID IS NOT NULL SET @filterWithSubLevels = @filterWithSubLevels + 4
IF @AdmissionTypeID IS NOT NULL SET @filterWithSubLevels = @filterWithSubLevels + 8

IF @ParentStructureID IS NULL
BEGIN 
	INSERT @institutions (InstitutionID, FullName, HasMilitaryDepartment,
		HasPreparatoryCourses, HasOlympics)
	SELECT DISTINCT i.InstitutionID, i.FullName, i.HasMilitaryDepartment,
		CAST(CASE WHEN pc.InstitutionID IS NULL THEN 0 ELSE 1 END AS BIT),
		CAST(CASE WHEN bi.InstitutionID IS NULL THEN 0 ELSE 1 END AS BIT)
	FROM Institution i
	-- подключаем фильтр по регионам
	JOIN Region reg ON reg.RegionID = i.RegionID
	-- получаем рутовые айтемы
	JOIN AdmissionItemPublished r ON r.InstitutionID = i.InstitutionID AND r.ItemTypeID = 0
	JOIN AdmissionStructurePublished rs ON rs.AdmissionItemID = r.AdmissionItemID
	-- подключаем фильтр по флажкам
	LEFT JOIN (SELECT bi.InstitutionID, MIN(bi.BenefitItemID) AS BenefitItemID
	        FROM BenefitItem bi GROUP BY bi.InstitutionID) bi ON bi.InstitutionID = i.InstitutionID
	LEFT JOIN (SELECT pc.InstitutionID, MIN(pc.PreparatoryCourseID) AS PreparatoryCourseID
	        FROM PreparatoryCourse pc GROUP BY pc.InstitutionID) pc ON pc.InstitutionID = i.InstitutionID
	WHERE 
	-- проверяем тип ОУ
	((@IsVUZ = 1 AND @IsSSUZ = 1) OR 
	(@IsVUZ = 1 AND @IsSSUZ = 0 AND i.InstitutionTypeID = 1) OR
	(@IsVUZ = 0 AND @IsSSUZ = 1 AND i.InstitutionTypeID = 2)
	) 
	-- ищем по части названия ОУ
	AND (@NamePart IS NULL OR i.FullName LIKE @NamePart)
	-- ищем по части названия Региона
	AND (@RegionName IS NULL OR reg.[Name] LIKE @RegionName)
	-- ищем по флажкам
	AND (@HasMilitaryDepartment IS NULL OR i.HasMilitaryDepartment = @HasMilitaryDepartment)
	AND (@HasPreparatoryCourses IS NULL OR 
		(@HasPreparatoryCourses = 0 AND pc.InstitutionID IS NULL) OR
		(@HasPreparatoryCourses = 1 AND pc.InstitutionID IS NOT NULL))
	AND (@HasOlympics IS NULL OR (@HasOlympics = 0 AND bi.InstitutionID IS NULL) OR
		(@HasOlympics = 1 AND bi.InstitutionID IS NOT NULL))
	-- ищем по подуровням
	AND (@filterWithSubLevels = 0 
	OR  ((EXISTS(SELECT * FROM AdmissionItemPublished sub WHERE i.InstitutionID = sub.InstitutionID
				AND (@EducationLevelID IS NULL OR sub.ItemTypeID = @EducationLevelID)))
		AND (EXISTS(SELECT * FROM AdmissionItemPublished sub 
			LEFT JOIN Direction d ON sub.DirectionID = d.DirectionID
			   WHERE i.InstitutionID = sub.InstitutionID
			AND ( (@DirectionName IS NULL AND @DirectionCode IS NULL)
					OR ((@DirectionName IS NOT NULL AND d.[Name] LIKE @DirectionName)
					OR (@DirectionCode IS NOT NULL AND d.[Code] LIKE @DirectionCode)) 
			)))
		AND (EXISTS(SELECT * FROM AdmissionItemPublished sub WHERE i.InstitutionID = sub.InstitutionID
				AND (@StudyID IS NULL OR sub.ItemTypeID = @StudyID)))
		AND (EXISTS(SELECT * FROM AdmissionItemPublished sub WHERE i.InstitutionID = sub.InstitutionID
				AND (@AdmissionTypeID IS NULL OR sub.ItemTypeID = @AdmissionTypeID)))
	))
	ORDER BY i.FullName
	
	SET @TotalPageCount = CEILING(CAST(@@ROWCOUNT AS MONEY) / @PageSize)
	
	--SELECT * FROM @institutions i WHERE i.ID BETWEEN @startPagerIndex AND @endPagerIndex
		--AND i.InstitutionID NOT IN (SELECT ai.InstitutionID FROM AdmissionItemPublished ai WHERE ai.ItemTypeID = 0)
		
	SELECT s.Depth, s.Lineage,
		i.InstitutionID, s.AdmissionStructureID, ai.AdmissionItemID, 
		CAST(COALESCE(ait.ItemLevel, 0) AS SMALLINT) AS ItemLevel, s.ParentID,
		CASE WHEN ait.ItemTypeID = 0 THEN i.FullName ELSE ai.[Name] END AS Name,
		ai.PlaceCount, ai.ApplicationCount,
		CAST((CASE WHEN ait.ItemTypeID = 0 AND i.HasOlympics = 1 THEN 1
			ELSE CASE WHEN ait.ItemTypeID = 0 THEN 0 END END) AS BIT)
			AS HasOlympics,
		CAST((CASE WHEN ait.ItemTypeID = 0 AND i.HasPreparatoryCourses = 1 THEN 1
			ELSE CASE WHEN ait.ItemTypeID = 0 THEN 0 END END) AS BIT)
			AS HasPreparatoryCourses,
		CASE WHEN ait.ItemTypeID = 0 THEN i.HasMilitaryDepartment ELSE CAST(NULL AS BIT) END AS HasMilitaryDepartment
	FROM @institutions i
	-- получаем рутовые айтемы
	JOIN AdmissionItemPublished r ON r.InstitutionID = i.InstitutionID AND r.ItemTypeID = 0
	JOIN AdmissionStructurePublished rs ON rs.AdmissionItemID = r.AdmissionItemID
	-- получаем дерево (по рутовым айтемам)
	LEFT JOIN AdmissionStructurePublished s ON s.Lineage LIKE rs.Lineage + '%' 
	LEFT JOIN AdmissionItemPublished ai ON ai.AdmissionItemID = s.AdmissionItemID
	LEFT JOIN AdmissionItemType ait ON (ai.ItemTypeID IS NULL AND ait.ItemTypeID = 0)
		OR (ai.ItemTypeID IS NOT NULL AND ait.ItemTypeID = ai.ItemTypeID)
	WHERE (s.Depth IS NULL OR s.Depth - rs.Depth < @DepthLimit)
		AND i.ID BETWEEN @startPagerIndex AND @endPagerIndex
		AND s.Lineage LIKE '/17008/17009/17023/%'
	-- порядок сортировки важен для создания иерархии на C# (поэтому нельзя по названию)
	ORDER BY s.Depth, s.Lineage
END
ELSE
BEGIN
	SELECT --s.Lineage,
		i.InstitutionID, s.AdmissionStructureID, ai.AdmissionItemID, ait.ItemLevel, s.ParentID,
		CASE WHEN ait.ItemTypeID = 0 THEN i.FullName ELSE ai.[Name] END AS Name,
		ai.PlaceCount, ai.ApplicationCount,
		CAST(NULL AS BIT) AS HasOlympics, CAST(NULL AS BIT) AS HasPreparatoryCourses,
		CAST(NULL AS BIT) AS HasMilitaryDepartment
	FROM Institution i
	-- получаем рутовый айтем
	JOIN AdmissionItemPublished r ON r.InstitutionID = i.InstitutionID 
	JOIN AdmissionStructurePublished rs ON rs.AdmissionItemID = r.AdmissionItemID 
		AND rs.AdmissionStructureID = @ParentStructureID
	-- получаем дерево (по рутовому айтему)
	JOIN AdmissionStructurePublished s ON s.Lineage LIKE rs.Lineage + '%' 
	JOIN AdmissionItemPublished ai ON ai.AdmissionItemID = s.AdmissionItemID
	JOIN AdmissionItemType ait ON ait.ItemTypeID = ai.ItemTypeID
	WHERE s.Depth - rs.Depth < @DepthLimit
	-- порядок сортировки важен для создания иерархии на C# (поэтому нельзя по названию)
	ORDER BY s.Depth, s.Lineage
END

GO


