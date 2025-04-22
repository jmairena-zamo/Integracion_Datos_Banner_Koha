using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiBase.Auth.Authorization.Attributes
{
    public class ApiKeyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _policy;

        public ApiKeyAttribute(params string[] Roles)
        {
            _policy = Roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeyAuthorizationFilter = new ApiKeyFilter(configuration, _policy);
            await apiKeyAuthorizationFilter.OnAuthorizationAsync(context);
        }
    }
}
