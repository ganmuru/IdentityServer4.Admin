﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Admin.Api.Configuration.Constants;
using IdentityServer4.Admin.Api.Dtos.Roles;
using IdentityServer4.Admin.Api.ExceptionHandling;
using IdentityServer4.Admin.Api.Helpers.Localization;
using IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme, Policy = AuthorizationConsts.AdministrationPolicy)]
    public class RolesController<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> : ControllerBase
        where TUserDto : UserDto<TUserDtoKey>, new()
        where TRoleDto : RoleDto<TRoleDtoKey>, new()
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TRoleDtoKey : IEquatable<TRoleDtoKey>
        where TUserDtoKey : IEquatable<TUserDtoKey>
        where TUsersDto : UsersDto<TUserDto, TUserDtoKey>
        where TRolesDto : RolesDto<TRoleDto, TRoleDtoKey>
        where TUserRolesDto : UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>
        where TUserClaimsDto : UserClaimsDto<TUserDtoKey>, new()
        where TUserProviderDto : UserProviderDto<TUserDtoKey>
        where TUserProvidersDto : UserProvidersDto<TUserDtoKey>
        where TUserChangePasswordDto : UserChangePasswordDto<TUserDtoKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleDtoKey>, new()
    {
        private readonly IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> _identityService;
        private readonly IGenericControllerLocalizer<UsersController<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> _localizer;

        private readonly IMapper _mapper;

        public RolesController(IIdentityService<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto> identityService,
            IGenericControllerLocalizer<UsersController<TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TUserKey, TRoleKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto>> localizer, IMapper mapper)
        {
            _identityService = identityService;
            _localizer = localizer;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TRoleDto>> Get(TUserDtoKey id)
        {
            var role = await _identityService.GetRoleAsync(id.ToString());

            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult<TRolesDto>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var rolesDto = await _identityService.GetRolesAsync(searchText, page, pageSize);

            return Ok(rolesDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TRoleDto role)
        {
            await _identityService.CreateRoleAsync(role);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TRoleDto role)
        {
            await _identityService.GetRoleAsync(role.Id.ToString());
            await _identityService.UpdateRoleAsync(role);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TRoleDtoKey id)
        {
            var roleDto = new TRoleDto { Id = id };

            await _identityService.GetRoleAsync(id.ToString());
            await _identityService.DeleteRoleAsync(roleDto);

            return Ok();
        }

        [HttpGet("{id}/Users")]
        public async Task<ActionResult<TRolesDto>> GetRoleUsers(string id, string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetRoleUsersAsync(id, searchText, page, pageSize);

            return Ok(usersDto);
        }

        [HttpGet("{id}/Claims")]
        public async Task<ActionResult<RoleClaimsApiDto<TRoleDtoKey>>> GetRoleClaims(string id, int page = 1, int pageSize = 10)
        {
            var roleClaimsDto = await _identityService.GetRoleClaimsAsync(id, page, pageSize);
            var roleClaimsApiDto = _mapper.Map<RoleClaimsApiDto<TRoleDtoKey>>(roleClaimsDto);

            return Ok(roleClaimsApiDto);
        }

        [HttpPost("Claims")]
        public async Task<IActionResult> PostRoleClaims([FromBody]RoleClaimApiDto<TRoleDtoKey> roleClaims)
        {
            var roleClaimsDto = _mapper.Map<TRoleClaimsDto>(roleClaims);
            await _identityService.CreateRoleClaimsAsync(roleClaimsDto);

            return Ok();
        }

        [HttpDelete("{id}/Claims")]
        public async Task<IActionResult> DeleteRoleClaims(TRoleDtoKey id, int claimId)
        {
            var roleDto = new TRoleClaimsDto { ClaimId = claimId, RoleId = id };

            await _identityService.GetRoleClaimAsync(roleDto.RoleId.ToString(), roleDto.ClaimId);
            await _identityService.DeleteRoleClaimsAsync(roleDto);

            return Ok();
        }
    }
}