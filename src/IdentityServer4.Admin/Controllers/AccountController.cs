﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer4.Shared.Configuration.Constants;
using IdentityServer4.Shared.Configuration.Interfaces;

namespace IdentityServer4.Admin.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IRootConfiguration _rootConfiguration;

        public AccountController(ILogger<ConfigurationController> logger, IRootConfiguration rootConfiguration) : base(logger)
        {
            _rootConfiguration = rootConfiguration;
        }

        public IActionResult AccessDenied()
        {
            ViewData["IdentityServerBaseUrl"] = _rootConfiguration.AdminAppConfiguration.IdentityServerBaseUrl;

            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new List<string> { AuthenticationConsts.SignInScheme, AuthenticationConsts.OidcAuthenticationScheme },
                new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}
