---
sidebar_position: 1
---

# Overview

## C4 system context

![PFM users and administrator interact with Personal Finance Manager, which uses Keycloak for live identity and can use object storage for bank icons.](../../static/img/pfm/Architecture/current/pfm-system-context.svg)

## Live containers

![React calls the ASP.NET Core website host, which signs users in through Keycloak and calls the existing PFM API. The APIs and workers use their existing data stores and account event stream.](../../static/img/pfm/Architecture/current/pfm-live-containers.svg)

The website host keeps upstream tokens server-side. The Tax & Pension API
remains part of the backend. The React UI provides Tax & Pension list, create,
edit and delete screens. Live uses the existing authenticated PFM API
boundary; offline uses temporary in-memory demo behavior. Offline acceptance
is verified; live wiring is implemented but runtime behavior against external
services was not exercised here.

## Offline containers

![The user opens the same React UI, backed by the local website host and temporary in-memory demo data.](../../static/img/pfm/Architecture/current/pfm-offline-containers.svg)

The offline profile signs users in locally without Keycloak, backend APIs, or
databases. The in-memory data is reset when the host stops.

The [React UI decision](./0002-react-offline-ui-adr.md) documents the contract
boundary. See [System Architecture Evolution](./system-evolution) for earlier
designs and the [regeneration instructions](../0003-Development/overview.md#regenerate-the-c4-diagrams)
to update these diagrams.