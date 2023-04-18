using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using PFM.Services.ExternalServices.AuthApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFM.Api.Filters
{
    public class DelegateToAuthApiAuthorizeFilter : AuthorizeFilter
    {
        private readonly IAuthApi _authApi;
        private readonly Serilog.ILogger _logger;
        private IList<string> _ignoreList = new List<string>() {
            "/api/Account/Login",
            "/api/Account/Register"
        };

        public DelegateToAuthApiAuthorizeFilter(AuthorizationPolicy policy, Serilog.ILogger logger, IAuthApi authApi) : base(policy)
        {
            _logger = logger;
            _authApi = authApi;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (_ignoreList.Any(x => x == context.HttpContext.Request.Path))
                return;

            bool result = false;
            try
            {
                string token = null;
                if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
                    if (authorizationHeader.ToString().StartsWith("Bearer"))
                    {
                        token = authorizationHeader.ToString();
                    }
                }

                if (token != null)
                {
                    result = await _authApi.ValidateToken(token);
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Auth API thrown an exception");
            }

            if (!result)
            {
                context.Result = new ChallengeResult();
            }
        }
    }

}
