﻿using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Admin.Api.Configuration.Constants;
using IdentityServer4.Admin.Api.Dtos.IdentityResources;
using IdentityServer4.Admin.Api.ExceptionHandling;
using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.BusinessLogic.Dtos.Configuration;
using IdentityServer4.Admin.BusinessLogic.Services.Interfaces;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme, Policy = AuthorizationConsts.AdministrationPolicy)]
    public class IdentityResourcesController : ControllerBase
    {
        private readonly IIdentityResourceService _identityResourceService;

        public IdentityResourcesController(IIdentityResourceService identityResourceService)
        {
            _identityResourceService = identityResourceService;
        }

        [HttpGet]
        public async Task<ActionResult<IdentityResourcesApiDto>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var identityResourcesDto = await _identityResourceService.GetIdentityResourcesAsync(searchText, page, pageSize);
            var identityResourcesApiDto = identityResourcesDto.ToIdentityResourceApiModel<IdentityResourcesApiDto>();

            return Ok(identityResourcesApiDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityResourceApiDto>> Get(int id)
        {
            var identityResourceDto = await _identityResourceService.GetIdentityResourceAsync(id);
            var identityResourceApiModel = identityResourceDto.ToIdentityResourceApiModel<IdentityResourceApiDto>();

            return Ok(identityResourceApiModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]IdentityResourceApiDto identityResourceApi)
        {
            var identityResourceDto = identityResourceApi.ToIdentityResourceApiModel<IdentityResourceDto>();
            await _identityResourceService.AddIdentityResourceAsync(identityResourceDto);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]IdentityResourceApiDto identityResourceApi)
        {
            var identityResource = identityResourceApi.ToIdentityResourceApiModel<IdentityResourceDto>();

            await _identityResourceService.GetIdentityResourceAsync(identityResource.Id);
            await _identityResourceService.UpdateIdentityResourceAsync(identityResource);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var identityResource = new IdentityResourceDto { Id = id };

            await _identityResourceService.GetIdentityResourceAsync(identityResource.Id);
            await _identityResourceService.DeleteIdentityResourceAsync(identityResource);

            return Ok();
        }

        [HttpGet("{id}/Properties")]
        public async Task<ActionResult<IdentityResourcePropertiesApiDto>> GetProperties(int id, int page = 1, int pageSize = 10)
        {
            var identityResourcePropertiesDto = await _identityResourceService.GetIdentityResourcePropertiesAsync(id, page, pageSize);
            var identityResourcePropertiesApiDto = identityResourcePropertiesDto.ToIdentityResourceApiModel<IdentityResourcePropertiesApiDto>();

            return Ok(identityResourcePropertiesApiDto);
        }

        [HttpGet("Properties/{propertyId}")]
        public async Task<ActionResult<IdentityResourcePropertyApiDto>> GetProperty(int propertyId)
        {
            var identityResourcePropertiesDto = await _identityResourceService.GetIdentityResourcePropertyAsync(propertyId);
            var identityResourcePropertyApiDto = identityResourcePropertiesDto.ToIdentityResourceApiModel<IdentityResourcePropertyApiDto>();

            return Ok(identityResourcePropertyApiDto);
        }

        [HttpPost("{id}/Properties")]
        public async Task<IActionResult> PostProperty(int id, [FromBody]IdentityResourcePropertyApiDto identityResourcePropertyApi)
        {
            var identityResourcePropertiesDto = identityResourcePropertyApi.ToIdentityResourceApiModel<IdentityResourcePropertiesDto>();
            identityResourcePropertiesDto.IdentityResourceId = id;

            await _identityResourceService.AddIdentityResourcePropertyAsync(identityResourcePropertiesDto);

            return Ok();
        }

        [HttpDelete("Properties/{propertyId}")]
        public async Task<IActionResult> DeleteProperty(int propertyId)
        {
            var identityResourceProperty = new IdentityResourcePropertiesDto { IdentityResourcePropertyId = propertyId };

            await _identityResourceService.GetIdentityResourcePropertyAsync(identityResourceProperty.IdentityResourcePropertyId);
            await _identityResourceService.DeleteIdentityResourcePropertyAsync(identityResourceProperty);

            return Ok();
        }
    }
}