﻿using IdentityServer4.Admin.BusinessLogic.Identity.Helpers;

namespace IdentityServer4.Admin.BusinessLogic.Identity.Resources
{
    public interface IIdentityServiceResources
    {
        ResourceMessage IdentityErrorKey();
        ResourceMessage RoleClaimDoesNotExist();
        ResourceMessage RoleClaimsCreateFailed();
        ResourceMessage RoleClaimsDeleteFailed();
        ResourceMessage RoleCreateFailed();
        ResourceMessage RoleDeleteFailed();
        ResourceMessage RoleDoesNotExist();
        ResourceMessage RoleUpdateFailed();
        ResourceMessage UserClaimDoesNotExist();
        ResourceMessage UserClaimsCreateFailed();
        ResourceMessage UserClaimsDeleteFailed();
        ResourceMessage UserCreateFailed();
        ResourceMessage UserDeleteFailed();
        ResourceMessage UserDoesNotExist(); 
        ResourceMessage UserChangePasswordFailed();
        ResourceMessage UserProviderDeleteFailed();
        ResourceMessage UserProviderDoesNotExist();
        ResourceMessage UserRoleCreateFailed();
        ResourceMessage UserRoleDeleteFailed();
        ResourceMessage UserUpdateFailed();
    }
}