﻿using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Shared.Configuration.Interfaces;

namespace IdentityServer4.Shared.Configuration.IdentityServer
{
    public class ClientResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Roles", new[] {"role"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IAdminAppConfiguration adminConfiguration)
        {
            return new[]
            {
                new ApiResource
                {
                    Name = adminConfiguration.IdentityAdminApiScope,
                    Scopes = new List<Scope>
                    {
                        new Scope
                        {
                            Name = adminConfiguration.IdentityAdminApiScope,
                            DisplayName = adminConfiguration.IdentityAdminApiScope,
                            UserClaims = new List<string>
                            {
                                "role"
                            },
                            Required = true
                        }
                    }
                }
            };
        }
    }
}