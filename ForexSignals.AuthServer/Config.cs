using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace ForexSignals.AuthServer
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                    Username = "Gary",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Gary"),
                        new Claim("family_name", "Hollingshead")
                    }
                },
                new TestUser
                {
                    SubjectId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
                    Username = "Marina",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Marina"),
                        new Claim("family_name", "Gulakova")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityReferences()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Forex Signals",
                    ClientId = "forexsignalsclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>
                    {
                        "/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}
