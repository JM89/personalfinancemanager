# PFM Website Reboot

React is the website UI in both modes. The ASP.NET Core host handles
authentication and calls the existing PFM APIs; the browser does not receive
the upstream bearer token.

## Live development

1. Start the PFM infrastructure and allow Keycloak initialization to finish.
   The website client in the `pfm` realm is named `pfm`. Read the current
   client secret from its Credentials tab and store it with .NET user secrets
   (`AuthOptions:ClientSecret` on `src/PFM.Website/PFM.Website.csproj`).
2. From `src/PFM.Website.Client`, run `npm ci` and `npm run build`.
3. From this directory, run `dotnet run --project src/PFM.Website --launch-profile dev`
   and open <https://localhost:7142>. Sign in with a Keycloak development user.
   For live Tilt/Compose, supply the matching `WEBSITE_OIDC_CLIENT_SECRET` and
   open <https://localhost:7143>. The registered Keycloak callback is `/signin-oidc`.

## Offline development

From the repository root, run `tilt up -- --offline`, or launch the website
with `--launch-profile frontend-only` and open <http://localhost:5110>.
The offline profile uses a development-only login and temporary in-memory data.

## Build and test

Run `npm test` and `npm run build` in the React project. Build the solution
with `dotnet build`. Build React before `dotnet publish`; publish includes
the React files automatically. To run the offline HTTP smoke test, set
`PFM_OFFLINE_URL` to the running offline website before running `npm test`.
