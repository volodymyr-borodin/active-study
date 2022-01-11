using System.Collections.Generic;
using IdentityServer4.Models;

namespace ActiveStudy.Web;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiScope> ApiScopes => new[]
    {
        new ApiScope("api", "Full Access to API")
    };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("api", "Main API")
        {
            Scopes = {"api"}
        }
    };

    public static IEnumerable<Client> Clients => new[]
    {
        new Client
        {
            ClientId = "ios_app",
            ClientName = "IOS Client",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes = new List<string> {"openid", "profile", "api"},
            ClientSecrets = new List<Secret>
            {
                new Secret("temp_secret".Sha256())
            }
        }
    };
}