﻿using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Shared.Configuration.Interfaces;

namespace IdentityServer4.Shared.Configuration.IdentityServer

{
    public class Clients
    {
        public static IEnumerable<Client> GetAdminClient(IAdminAppConfiguration adminConfiguration)
        {

            return new List<Client>
            {

	            ///////////////////////////////////////////
	            // IdentityServer4.Admin Client
	            //////////////////////////////////////////
	            new Client
                {

                    ClientId =adminConfiguration.ClientId,
                    ClientName =adminConfiguration.ClientId,
                    ClientUri = adminConfiguration.IdentityAdminBaseUrl,

                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret(adminConfiguration.ClientSecret.ToSha256())
                    },

                    RedirectUris = { $"{adminConfiguration.IdentityAdminBaseUrl}/signin-oidc"},
                    FrontChannelLogoutUri = $"{adminConfiguration.IdentityAdminBaseUrl}/signout-oidc",
                    PostLogoutRedirectUris = { $"{adminConfiguration.IdentityAdminBaseUrl}/signout-callback-oidc"},
                    AllowedCorsOrigins = { adminConfiguration.IdentityAdminBaseUrl },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles"
                    }
                },

                new Client
                {
                    ClientId = adminConfiguration.IdentityAdminApiSwaggerUIClientId,
                    ClientName = adminConfiguration.IdentityAdminApiSwaggerUIClientId,
                    
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = new List<string>
                    {
                        adminConfiguration.IdentityAdminApiSwaggerUIRedirectUrl
                    },
                    AllowedScopes =
                    {
                        adminConfiguration.IdentityAdminApiScope
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };

        }
    }
}
