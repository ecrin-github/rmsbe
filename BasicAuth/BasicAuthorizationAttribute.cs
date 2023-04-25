using Microsoft.AspNetCore.Authorization;

namespace rmsbe.BasicAuth;

public class BasicAuthorizationAttribute : AuthorizeAttribute
{
    public BasicAuthorizationAttribute()
    {
        AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme;
    }
}