using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.AspNet.Identity;
using System.Configuration;

[assembly: OwinStartup(typeof(B_M.Startup))]

namespace B_M
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable Cookie Authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Enable External Sign In (Google OAuth)
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Google OAuth Configuration
            var googleClientId = ConfigurationManager.AppSettings["GoogleClientId"];
            var googleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];

            if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = googleClientId,
                    ClientSecret = googleClientSecret
                });
            }
        }
    }
}

