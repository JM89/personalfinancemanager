---
sidebar_position: 1
---

# Getting Started

import useBaseUrl from '@docusaurus/useBaseUrl';

### Pre-requisites

#### Tools

You will need the following tools installed on your development machine:
- [x] Docker
- [x] .NET 11
- [x] `jq` command line
- [x] `tilt` command line

#### Changes on hosts file

Add the following entries to your hosts file:

```sh
sudo sh -c 'echo 127.0.0.1 keycloak >> /etc/hosts'
```

#### GitHub Package Access

To run from local machine, you will need a GitHub token to access the private GitHub packages. 

In the `./PFM.Infra/configs`, you will find an .env-example file. If you copy this file and name it `.env` in the same location, the `tilt up` script will pick it up automatically. 

:warning: This file is part of gitignore. It will contains sensitive data, do not commit the .env file. 

#### Local keycloak secrets

To simplify the setup, the secrets for Keycloak have been hard-coded. In a normal setup, this would have to be stored in a secured fashion. 

| Property                 | Value                              | Description                              |
| ------------------------ | ---------------------------------- | ---------------------------------------- |
| admin                    | `SecurityMatters12456!`            | Administrator Password                   |
| jess                     | `SecurityMatters!123`              | PFM Default User                         |
| pfm                      | `RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjd` | App client for PFM Website               |
| pfm-bank-account-updater | `RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jje` | Service account for Bank Account Updater |
| pfm-mvt-aggregator       | `RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjf` | Service account for Movement Aggregator  |

#### Shared infrastructure

Some infrastructure resources are shared accross different projects (e.g. SQL server, SEQ, AWS), some are dedicated to each apps (e.g. creation DB, a SQS queue). 

The shared resources available in the PFM.Infra folder, contains the shared resources:

| Resources        | Description                                                                                                      | Docker                                                                                       | Debug Mode                                                        | Internal Docker                     |
| ---------------- | ---------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------- |
| SEQ              | Logging                                                                                                          | [Log Ingest](http://localhost:5341)<br/>[UI](http://localhost:80)                            | [Log Ingest](http://localhost:5341)<br/>[UI](http://localhost:80) | [Log Ingest](http://seq:5341)       |
| SQL Server       | a single container is used for several isolated DB, to reduce the setup time, space and memory in local machine. | localhost,1433                                                                               | localhost,1433                                                    | db-server,1433                      |
| Localstack       | Localstack for mocking AWS resources.                                                                            | [Endpoints](http://localhost:4566)                                                           | [Endpoints](http://localhost:4566)                                | [Endpoints](http://localstack:4566) |
| EventStore       | Publishing/consuming events and auditing.                                                                        | [Endpoints](http://localhost:2113)                                                           | [Endpoints](http://localhost:2113/)                               | [Endpoints](http://eventstore:2113) |
| Keycloak         | Authentication & Authorisation server. The keycloak-init folder will setup the minimum user & clients.           | [Endpoints](http://localhost:8080)                                                           | [Endpoints](http://localhost:8080)                                | [Endpoints](http://keycloak:8080)   |
| MySql Server     | a single container is used for several isolated DB, to reduce the setup time, space and memory in local machine. | localhost,3306                                                                               | localhost,3306                                                    | mysql-db-server,3306                |
| Aspire Dashboard | Monitoring dashboard for logging, metrics and tracing.                                                           | [Dashboard](http://localhost:18888/structuredlogs) <br/> Telemetry Ingestion: localhost:4317 |                                                                   |                                     |

### Starting the backend services & dependencies

You can start all the backend services and dependencies using the command: 

```shell
tilt up
```

From there, you can start the main website from Visual Studio. 

For frontend development, the website can use in-memory storage instead of
calling PFM.API. The setting is
`ApplicationSettings:ExternalServiceSettings:PfmApiOptions:Enabled`.

#### Frontend-only development

The React frontend-only profile uses the website's in-memory .NET services and
does not start the backend APIs, Keycloak, or infrastructure. Install Node.js
and npm, then from the repository root run:

```shell
npm ci --prefix PFM.Website.Reboot/src/PFM.Website.Client
dotnet run --project PFM.Website.Reboot/src/PFM.Website/PFM.Website.csproj --launch-profile frontend-only
```

Open http://localhost:5110. The ASP.NET Core SPA proxy starts Vite on port
5174 and redirects the browser there. Select **Enter your workspace** to sign in as a local
developer without Keycloak. Data is lost when the website stops. Offline
authentication is restricted to the Development environment and must not be
used for a publicly accessible deployment. Other profiles continue to use the
existing Blazor website and Keycloak.

The existing in-memory implementations have limitations: some stores use
separate seeded copies of related data, so newly created expense types may not
be usable when creating expenses. Closing an ATM
withdrawal is not implemented, and editing a budget does not update its
expected income or savings. These are existing offline-service limitations;
the React migration does not change the backend to address them.

Alternatively, after `npm ci`, use the existing Tilt configuration:

```shell
tilt up -- --offline
```

Select the `pfm-frontend-offline` resource and open its link. This mode does not
start the other Tilt resources. Port 5110 is the .NET host and port 5174 is
the React development server; both must be available.

To run the client tests, use `npm test --prefix PFM.Website.Reboot/src/PFM.Website.Client`.
With the offline website running, set `PFM_OFFLINE_URL=http://localhost:5174`
to include the in-memory integration test. The integration test creates and
modifies temporary in-memory data and uploads then removes a test bank icon.

#### React workspace appearance

The React workspace retains PFM's original green piggy-bank identity as a
redrawn SVG with a coin detail. The shared `public/pfm-mark.svg` asset is used
in the sidebar, welcome screen and browser favicon; the original Blazor
assets are unchanged.

The offline React workspace has two built-in themes: **Pearl** (ivory and teal)
and **Midnight** (navy and mint). Open **Appearance** in the sidebar (at the top
on small screens) to preview and select one. The choice applies immediately,
including chart labels and gridlines, and is saved under `pfm-appearance` in
this browser's local storage. If browser storage is unavailable, the UI
reports that the preference could not be saved.

The dashboard retains the existing account selector, category drill-downs,
data tables and movement routes. Configuration uses labelled forms with
required-field indicators and contextual hints; expense colors use a color
picker while still sending six-digit hexadecimal values without `#`.
Deletion uses a keyboard-accessible confirmation dialog, with the cancel
action initially focused and service errors displayed without closing it.
Escape dismisses dialogs when no deletion is in progress.
Configuration tables use emphasized headers and left-aligned edit, favorite
and delete icons. Each operation has an item-specific accessible label and
hover tooltip; delete still requires confirmation.

This is a presentation-only redesign of the offline React client. The Blazor
profiles, authentication, API payloads, and service limitations are unchanged.
There are no new packages, environment settings, infrastructure or CI steps:
the existing client test and Vite build commands cover the affected delivery
surface.

#### Workspace walkthrough

The following 32-second video demonstrates the offline React UI using local
demo data. It is silent, with English captions, and does not autoplay.

<video
  controls
  playsInline
  preload="none"
  poster={useBaseUrl('/img/pfm/react-expense-types.png')}
  style={{width: '100%', borderRadius: '12px'}}
  aria-label="PFM workspace walkthrough"
>
  <source src={useBaseUrl('/video/pfm-workspace.webm')} type="video/webm" />
  <track default kind="captions" src={useBaseUrl('/video/pfm-workspace.en.vtt')} srcLang="en" label="English" />
  Your browser does not support embedded video.
</video>

<a href={useBaseUrl('/video/pfm-workspace.webm')}>Download the video</a>.

Text alternative:

1. **Overview** shows your selected account, balance, monthly summaries and
   spending charts. Select a chart category to explore its details.
2. Open **Appearance** and choose **Pearl** or **Midnight**. The choice is saved
   in this browser and applies to chart labels as well as the surrounding UI.
3. **Bank accounts**, **Banks** and **Expense types** share compact tables
   with strong headers. Hover an operation icon to see its action and item name.
4. Open **Add expense type** to name a category, choose a chart color, and
   decide whether it appears on the dashboard. **Cancel** leaves data unchanged.
5. A bin icon opens a confirmation dialog. **Keep item** is focused initially;
   cancel, Escape or the close button dismisses it without deleting anything.

##### Configuration at a glance

![Expense types with emphasized headers and left-aligned operation icons](../../static/img/pfm/react-expense-types.png)

![Category form with required-field guidance, a color picker and dashboard visibility option](../../static/img/pfm/react-category-form.png)

##### On smaller screens

The same workspace adapts to a phone-sized viewport:

<img src={useBaseUrl('/img/pfm/react-mobile-pearl.png')} alt="Pearl dashboard at a 390-pixel mobile viewport with top navigation" width="390" style={{maxWidth: '100%', height: 'auto'}} />

##### Refreshing these captures

These are captures of the offline React client, not replacements for the
historical Blazor images embedded in the business specifications. Refresh the
`react-*.png` assets and `video/pfm-workspace.webm` when the visible UI changes;
keep the matching English WebVTT captions and the text alternative in sync.
The current video was recorded locally using temporary
[Playwright](https://playwright.dev/docs/videos) tooling
([Apache-2.0 license](https://github.com/microsoft/playwright/blob/main/LICENSE))
and installed Chrome. The recording uses only the loopback offline host, not
connected accounts or real credentials. Playwright is not an application or
documentation build dependency, and recording is not a CI requirement.

#### Configurations

##### External Settings Configs

Configuration Sections: `ApplicationSettings:AuthOptions`

| Property    | Default Value                    | Configration      |
| ----------- | -------------------------------- | ----------------- |
| `Authority` | http://localhost:8080/realms/pfm | SSO Authority     |
| `ClientId`  | pfm                              | PFM Client ID     |
| `ClientId`  | RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjd | PFM Client Secret |

##### External Settings Configs

Configuration Sections: `ApplicationSettings:ExternalServiceSettings`

| Property                            | Default Value                  | Configration                                                                                                   |
| ----------------------------------- | ------------------------------ | -------------------------------------------------------------------------------------------------------------- |
| `PfmApiOptions:Enabled`             | false                          | If disabled, use in-memory storage                                                                             |
| `PfmApiOptions:EndpointUrl`         | https://localhost:4431/        | PFM API Docker Endpoint.                                                                                       |
| `BankIconSettings:UseRemoteStorage` | false                          | If disabled, use local system. We need to put metrics to measure latency and optimise before re-enabling this. |
| `BankIconSettings:Location`         | ./wwwroot/Resources/bank_icons | If UseRemoteStorage is enabled, use the s3 bucket name for the location: pfm-website-bank-icons                |
| `AwsRegion`                         | eu-west-2                      | If UseRemoteStorage is enabled, s3 bucket AWS region (localstack init setup)                                   |
| `AwsEndpointUrl`                    | http://localhost:4566          | If UseRemoteStorage is enabled, s3 bucket AWS endpoint url (localstack init setup)                             |

The API URLs can be found here:

| Resources          | Docker                                                                                               | Debug Mode                                                                                             | Internal Docker                                |
| ------------------ | ---------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | ---------------------------------------------- |
| App - PFM.Api      | [Api Endpoints](https://localhost:4431/api)<br/>[Swagger](https://localhost:4431/swagger/index.html) | [Api Endpoints](https://localhost:7098/api)<br/>[Swagger](https://localhost:4431/swagger/index.html)   | [Api Endpoints](https://pfm-api:4431/api)      |
| App - PFM.Bank.Api | [Api Endpoints](https://localhost:7099/api)<br/>[Swagger](https://localhost:7099/swagger/index.html) | [Api Endpoints](https://localhost:50001/api)<br/>[Swagger](https://localhost:50001/swagger/index.html) | [Api Endpoints](http://pfm-bank-api:50001/api) |
| App - PFM.TNP.Api  | [Api Endpoints](https://localhost:7111/api)<br/>[Swagger](https://localhost:7111/swagger/index.html) | [Api Endpoints](https://localhost:50002/api)<br/>[Swagger](https://localhost:50002/swagger/index.html) | [Api Endpoints](http://pfm-bank-api:50002/api) |

### Debug the APIs

All APIs uses Swagger. You can generate a token using the user created during the initialisation script: 

```shell
curl -v -L -X POST "http://localhost:8080/realms/pfm/protocol/openid-connect/token" \
-H 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=pfm' \
--data-urlencode 'grant_type=password' \
--data-urlencode 'client_secret=RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjd' \
--data-urlencode 'scope=openid' \
--data-urlencode 'username=jess' \
--data-urlencode 'password=SecurityMatters!123' | jq .access_token
```

The bearer token generated can then be used in Swagger.

Click on the "Authorize" button at the top of the page, enter "Bearer ", copy the token and Authorize:

![](../../static/img/pfm/API-BearerToken.PNG)

You can now access the actions.

### Generate some data

Some features of the system are only demo-able if there is some data. To generate some data, run the following script:

```shell
sh ./generate-data.sh
```
