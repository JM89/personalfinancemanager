using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using PFM.Bank.Api.Services.ExternalServices.AuthApi;

namespace Api.Filters
{
    public class DelegateToAuthApiAuthorizeFilter : AuthorizeFilter
    {
        private readonly IAuthApi _authApi;
        private readonly Serilog.ILogger _logger;

        public DelegateToAuthApiAuthorizeFilter(AuthorizationPolicy policy, Serilog.ILogger logger, IAuthApi authApi) : base(policy)
        {
            _logger = logger;
            _authApi = authApi;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool result = false;
            try
            {
                string token = string.Empty;
                if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
                    if (authorizationHeader.ToString().StartsWith("Bearer"))
                    {
                        token = authorizationHeader.ToString();
                    }
                }

                if (!token.IsNullOrEmpty())
                {
                    result = await _authApi.ValidateToken(token);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Authentication API thrown an exception");
            }

            if (!result)
            {
                context.Result = new ChallengeResult();
            }
        }
    }

}
