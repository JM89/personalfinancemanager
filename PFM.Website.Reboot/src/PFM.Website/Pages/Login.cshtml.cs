using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Website.Configurations;
using System.Security.Claims;

namespace PFM.Website.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthOptions authOptions;

        public LoginModel(AuthOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public async Task<IActionResult> OnGet(string? redirectUri)
        {
            var returnUrl = Url.IsLocalUrl(redirectUri) ? redirectUri! : "/";
            if (!authOptions.DevOffline)
            {
                return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Local developer"),
                new Claim("preferred_username", "Local developer")
            };
            var identity = new ClaimsIdentity(claims, AuthOptions.OfflineCookieScheme);
            await HttpContext.SignInAsync(AuthOptions.OfflineCookieScheme, new ClaimsPrincipal(identity));
            return LocalRedirect(returnUrl);
        }
    }
}