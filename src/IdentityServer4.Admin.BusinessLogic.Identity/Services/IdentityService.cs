﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using IdentityServer4.Admin.BusinessLogic.Identity.Resources;
using IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using IdentityServer4.Admin.BusinessLogic.Shared.ExceptionHandling;
using IdentityServer4.Admin.EntityFramework.Extensions.Common;
using IdentityServer4.Admin.EntityFramework.Identity.Repositories.Interfaces;

namespace IdentityServer4.Admin.BusinessLogic.Identity.Services
{
    public class IdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, 
        TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>: IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole,
        TUserLogin, TRoleClaim, TUserToken,
        TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
        TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>
        where TUserDto : UserDto<TUserDtoKey>
        where TRoleDto : RoleDto<TRoleDtoKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
        where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
        where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
        where TUserClaimsDto : UserClaimsDto<TUserDtoKey>
        where TUserProviderDto : UserProviderDto<TUserDtoKey>
        where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
        where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>
    {
        protected readonly IIdentityRepository<TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> IdentityRepository;
        protected readonly IIdentityServiceResources IdentityServiceResources;
        protected readonly IMapper Mapper;

        public IdentityService(IIdentityRepository<TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> identityRepository,
            IIdentityServiceResources identityServiceResources,
            IMapper mapper)
        {
            IdentityRepository = identityRepository;
            IdentityServiceResources = identityServiceResources;
            Mapper = mapper;
        }

        public virtual async Task<bool> ExistsUserAsync(string userId)
        {
            var exists = await IdentityRepository.ExistsUserAsync(userId);
            if (!exists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            return true;
        }

        public virtual async Task<bool> ExistsRoleAsync(string roleId)
        {
            var exists = await IdentityRepository.ExistsRoleAsync(roleId);
            if (!exists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            return true;
        }

        public virtual async Task<TUsersDto> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await IdentityRepository.GetUsersAsync(search, page, pageSize);
            var usersDto = Mapper.Map<TUsersDto>(pagedList);

            return usersDto;
        }

        public virtual async Task<TUsersDto> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10)
        {
            var roleKey = ConvertToKeyFromString(roleId);

            var userIdentityRole = await IdentityRepository.GetRoleAsync(roleKey);
            if (userIdentityRole == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var pagedList = await IdentityRepository.GetRoleUsersAsync(roleId, search, page, pageSize);
            var usersDto = Mapper.Map<TUsersDto>(pagedList);

            return usersDto;
        }

        public virtual async Task<TRolesDto> GetRolesAsync(string search, int page = 1, int pageSize = 10)
        {
            PagedList<TRole> pagedList = await IdentityRepository.GetRolesAsync(search, page, pageSize);
            var rolesDto = Mapper.Map<TRolesDto>(pagedList);

            return rolesDto;
        }

        public virtual async Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(TRoleDto role)
        {
            var roleEntity = Mapper.Map<TRole>(role);
            var (identityResult, roleId) = await IdentityRepository.CreateRoleAsync(roleEntity);
            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.RoleCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);

            return (handleIdentityError, roleId);
        }

        private IdentityResult HandleIdentityError(IdentityResult identityResult, string errorMessage, string errorKey, object model)
        {
            if (!identityResult.Errors.Any()) return identityResult;
            var viewErrorMessages = Mapper.Map<List<ViewErrorMessage>>(identityResult.Errors);

            throw new UserFriendlyViewException(errorMessage, errorKey, viewErrorMessages, model);
        }

        public virtual async Task<TRoleDto> GetRoleAsync(string roleId)
        {
            var roleKey = ConvertToKeyFromString(roleId);

            var userIdentityRole = await IdentityRepository.GetRoleAsync(roleKey);
            if (userIdentityRole == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var roleDto = Mapper.Map<TRoleDto>(userIdentityRole);

            return roleDto;
        }

        public virtual async Task<List<TRoleDto>> GetRolesAsync()
        {
            var roles = await IdentityRepository.GetRolesAsync();
            var roleDtos = Mapper.Map<List<TRoleDto>>(roles);

            return roleDtos;
        }

        public virtual async Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(TRoleDto role)
        {
            var userIdentityRole = Mapper.Map<TRole>(role);
            var (identityResult, roleId) = await IdentityRepository.UpdateRoleAsync(userIdentityRole);

            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.RoleUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);

            return (handleIdentityError, roleId);
        }

        public virtual async Task<TUserDto> GetUserAsync(string userId)
        {
            var identity = await IdentityRepository.GetUserAsync(userId);
            if (identity == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userDto = Mapper.Map<TUserDto>(identity);

            return userDto;
        }

        public virtual async Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(TUserDto user)
        {
            var userIdentity = Mapper.Map<TUser>(user);
            var (identityResult, userId) = await IdentityRepository.CreateUserAsync(userIdentity);
            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.UserCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);

            return (handleIdentityError, userId);
        }

        /// <summary>
        /// Updates the specified user, but without updating the password hash value
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(TUserDto user)
        {
            var userIdentity = Mapper.Map<TUser>(user);
            await MapOriginalPasswordHashAsync(userIdentity);

            var (identityResult, userId) = await IdentityRepository.UpdateUserAsync(userIdentity);
            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.UserUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);

            return (handleIdentityError, userId);
        }

        /// <summary>
        /// Get original password hash and map password hash to user
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <returns></returns>
        private async Task MapOriginalPasswordHashAsync(TUser userIdentity)
        {
            var identity = await IdentityRepository.GetUserAsync(userIdentity.Id.ToString());
            userIdentity.PasswordHash = identity.PasswordHash;
        }

        public virtual async Task<IdentityResult> DeleteUserAsync(string userId, TUserDto user)
        {
            var identityResult = await IdentityRepository.DeleteUserAsync(userId);

            return HandleIdentityError(identityResult, IdentityServiceResources.UserDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);
        }

        public virtual async Task<IdentityResult> CreateUserRoleAsync(TUserRolesDto role)
        {
            var identityResult = await IdentityRepository.CreateUserRoleAsync(role.UserId.ToString(), role.RoleId.ToString());

            if (!identityResult.Errors.Any()) return identityResult;

            var userRolesDto = await BuildUserRolesViewModel(role.UserId, 1);
            return HandleIdentityError(identityResult, IdentityServiceResources.UserRoleCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, userRolesDto);
        }

        public virtual async Task<TUserRolesDto> BuildUserRolesViewModel(TUserDtoKey id, int? page)
        {
            var roles = await GetRolesAsync();
            var userRoles = await GetUserRolesAsync(id.ToString(), page ?? 1);
            userRoles.UserId = id;
            userRoles.RolesList = roles.Select(x => new SelectItemDto(x.Id.ToString(), x.Name)).ToList();

            return userRoles;
        }

        public virtual async Task<TUserRolesDto> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userIdentityRoles = await IdentityRepository.GetUserRolesAsync(userId, page, pageSize);
            var roleDtos = Mapper.Map<TUserRolesDto>(userIdentityRoles);

            var user = await IdentityRepository.GetUserAsync(userId);
            roleDtos.UserName = user.UserName;

            return roleDtos;
        }

        public virtual async Task<IdentityResult> DeleteUserRoleAsync(TUserRolesDto role)
        {
            var identityResult = await IdentityRepository.DeleteUserRoleAsync(role.UserId.ToString(), role.RoleId.ToString());

            return HandleIdentityError(identityResult, IdentityServiceResources.UserRoleDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);
        }

        public virtual async Task<TUserClaimsDto> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityUserClaims = await IdentityRepository.GetUserClaimsAsync(userId, page, pageSize);
            var claimDtos = Mapper.Map<TUserClaimsDto>(identityUserClaims);

            var user = await IdentityRepository.GetUserAsync(userId);
            claimDtos.UserName = user.UserName;

            return claimDtos;
        }

        public virtual async Task<TUserClaimsDto> GetUserClaimAsync(string userId, int claimId)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityUserClaim = await IdentityRepository.GetUserClaimAsync(userId, claimId);
            if (identityUserClaim == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserClaimDoesNotExist().Description, userId), IdentityServiceResources.UserClaimDoesNotExist().Description);

            var userClaimsDto = Mapper.Map<TUserClaimsDto>(identityUserClaim);

            return userClaimsDto;
        }

        public virtual async Task<IdentityResult> CreateUserClaimsAsync(TUserClaimsDto claimsDto)
        {
            var userIdentityUserClaim = Mapper.Map<TUserClaim>(claimsDto);
            var identityResult = await IdentityRepository.CreateUserClaimsAsync(userIdentityUserClaim);

            return HandleIdentityError(identityResult, IdentityServiceResources.UserClaimsCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<int> DeleteUserClaimsAsync(TUserClaimsDto claim)
        {
            return await IdentityRepository.DeleteUserClaimsAsync(claim.UserId.ToString(), claim.ClaimId);
        }

        public virtual TUserDtoKey ConvertUserDtoKeyFromString(string id)
        {
            if (id == null)
            {
                return default(TUserDtoKey);
            }
            return (TUserDtoKey)TypeDescriptor.GetConverter(typeof(TUserDtoKey)).ConvertFromInvariantString(id);
        }

        public virtual TKey ConvertToKeyFromString(string id)
        {
            if (id == null)
            {
                return default(TKey);
            }
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        public virtual async Task<TUserProvidersDto> GetUserProvidersAsync(string userId)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userLoginInfos = await IdentityRepository.GetUserProvidersAsync(userId);
            var providersDto = Mapper.Map<TUserProvidersDto>(userLoginInfos);
            providersDto.UserId = ConvertUserDtoKeyFromString(userId);

            var user = await IdentityRepository.GetUserAsync(userId);
            providersDto.UserName = user.UserName;

            return providersDto;
        }

        public virtual async Task<IdentityResult> DeleteUserProvidersAsync(TUserProviderDto provider)
        {
            var identityResult = await IdentityRepository.DeleteUserProvidersAsync(provider.UserId.ToString(), provider.ProviderKey, provider.LoginProvider);

            return HandleIdentityError(identityResult, IdentityServiceResources.UserProviderDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, provider);
        }

        public virtual async Task<TUserProviderDto> GetUserProviderAsync(string userId, string providerKey)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityUserLogin = await IdentityRepository.GetUserProviderAsync(userId, providerKey);
            if (identityUserLogin == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserProviderDoesNotExist().Description, providerKey), IdentityServiceResources.UserProviderDoesNotExist().Description);

            var userProviderDto = Mapper.Map<TUserProviderDto>(identityUserLogin);
            var user = await GetUserAsync(userId);
            userProviderDto.UserName = user.UserName;

            return userProviderDto;
        }

        public virtual async Task<IdentityResult> UserChangePasswordAsync(TUserChangePasswordDto userPassword)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userPassword.UserId.ToString());
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userPassword.UserId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityResult = await IdentityRepository.UserChangePasswordAsync(userPassword.UserId.ToString(), userPassword.Password);

            return HandleIdentityError(identityResult, IdentityServiceResources.UserChangePasswordFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, userPassword);
        }

        public virtual async Task<IdentityResult> CreateRoleClaimsAsync(TRoleClaimsDto claimsDto)
        {
            var identityRoleClaim = Mapper.Map<TRoleClaim>(claimsDto);
            var identityResult = await IdentityRepository.CreateRoleClaimsAsync(identityRoleClaim);

            return HandleIdentityError(identityResult, IdentityServiceResources.RoleClaimsCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<TRoleClaimsDto> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10)
        {
            var roleExists = await IdentityRepository.ExistsRoleAsync(roleId);
            if (!roleExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var identityRoleClaims = await IdentityRepository.GetRoleClaimsAsync(roleId, page, pageSize);
            var roleClaimDtos = Mapper.Map<TRoleClaimsDto>(identityRoleClaims);
            var roleDto = await GetRoleAsync(roleId);
            roleClaimDtos.RoleName = roleDto.Name;

            return roleClaimDtos;
        }

        public virtual async Task<TRoleClaimsDto> GetRoleClaimAsync(string roleId, int claimId)
        {
            var roleExists = await IdentityRepository.ExistsRoleAsync(roleId);
            if (!roleExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var identityRoleClaim = await IdentityRepository.GetRoleClaimAsync(roleId, claimId);
            if (identityRoleClaim == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleClaimDoesNotExist().Description, claimId), IdentityServiceResources.RoleClaimDoesNotExist().Description);
            var roleClaimsDto = Mapper.Map<TRoleClaimsDto>(identityRoleClaim);
            var roleDto = await GetRoleAsync(roleId);
            roleClaimsDto.RoleName = roleDto.Name;

            return roleClaimsDto;
        }

        public virtual async Task<int> DeleteRoleClaimsAsync(TRoleClaimsDto role)
        {
            return await IdentityRepository.DeleteRoleClaimsAsync(role.RoleId.ToString(), role.ClaimId);
        }

        public virtual async Task<IdentityResult> DeleteRoleAsync(TRoleDto role)
        {
            var userIdentityRole = Mapper.Map<TRole>(role);
            var identityResult = await IdentityRepository.DeleteRoleAsync(userIdentityRole);

            return HandleIdentityError(identityResult, IdentityServiceResources.RoleDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);
        }
    }
}