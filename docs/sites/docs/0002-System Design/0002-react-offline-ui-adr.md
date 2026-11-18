---
sidebar_position: 2
---

# ADR: Move the website UI to React without changing backend contracts

## Status

Accepted for both live and offline use. React replaces the Blazor presentation;
the .NET website host and downstream API contracts remain.

## Context

The previous website was a server-side Blazor application. It contained presentation
logic alongside .NET services that adapt the PFM API contracts to website models.
The existing in-memory implementations allow frontend development without the
backend APIs, while the development-only offline authentication mode avoids
requiring Keycloak.

The project is also exploring shared development patterns from the platform
toolkit. Its gateway uses React, Vite and ASP.NET Core SPA proxy, making it a
useful reference for a consistent frontend development experience.

## Decision

Move the website presentation to React while keeping the existing .NET host,
service layer and backend API contracts. In development, the host will start
the React development server through ASP.NET Core SPA proxy, following the
gateway example. The React application will call a local website API; that
API delegates to the existing .NET services. Live uses Keycloak and the real
PFM API; offline uses the existing in-memory implementations. Tokens stay on
the server and downstream calls use the signed-in request's access token.
The published website serves the React build, including direct links to views.

The initial migration targets offline development only: no backend APIs,
Keycloak, database or other infrastructure services should be required.
Existing local-development authentication must remain restricted to the
Development environment. The existing Tilt setup should provide an offline
workflow without starting the backend stack.

## Consequences

- React and .NET can evolve independently, with the .NET website host retaining
  the boundary to the existing contracts and service behavior.
- The website needs a browser-facing API and explicit error and validation
  handling; it must not expose development-only authentication in production.
- The frontend requires Node.js and npm in addition to the .NET SDK.
- In-memory data is temporary and is lost when the host stops.
- The React dashboard uses Chart.js rather than the Blazor chart components.
  Its in-memory movement summaries remain synthetic and do not
  automatically recalculate after individual transactions change.
- Existing in-memory implementations keep independent seeded copies of some
  related data; creating an expense type does not necessarily make it available
  when creating expenses. ATM withdrawal closing is not
  implemented and budget edits do not update expected income or savings.
- Website endpoints adapt the existing .NET website models; they are not a
  replacement for, or a change to, the backend API contracts.
- Both modes use the same React pages. Live shows the authenticated user's
  data and does not display offline demo messaging. Tax & Pension is deferred.

## Alternatives considered

- **Keep Blazor:** avoids migration work but does not align the frontend with the
  React/Vite pattern being explored in the platform toolkit.
- **Call backend APIs directly from React:** would require backend services for
  local development and introduce browser-facing authentication and contract
  concerns into this offline-first phase.
- **Implement browser-only mock data:** removes the .NET host from development
  but duplicates the existing .NET service and in-memory behavior, increasing
  the risk of divergence from the contracts.
