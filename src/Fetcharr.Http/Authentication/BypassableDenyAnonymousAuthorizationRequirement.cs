using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Fetcharr.Http.Authentication
{
    public class BypassableDenyAnonymousAuthorizationRequirement : DenyAnonymousAuthorizationRequirement
    {
    }
}
