workspace "Personal Finance Manager" "Current live and offline architecture" {
    model {
        user = person "PFM User" "Manages accounts, spending, savings and budgets."
        admin = person "PFM Administrator" "Maintains shared reference data."
        keycloak = softwareSystem "Keycloak" "Identity provider for live sign-in and service accounts." {
            tags "External"
        }
        iconStorage = softwareSystem "Object storage" "Optional remote storage for bank icons." {
            tags "External"
        }

        pfm = softwareSystem "Personal Finance Manager" "Tracks and visualizes personal finances." {
            react = container "React website" "Browser UI for dashboards, accounts, movements and budgets." "React / Vite"
            host = container "Website host" "Serves React and its same-origin API. Handles session cookies, record access and downstream tokens." "ASP.NET Core"
            api = container "PFM API" "Serves finance and reporting APIs and forwards bank and TNP requests." "ASP.NET Core"
            bank = container "Bank API" "Manages bank and account data." "ASP.NET Core"
            tnp = container "Tax & Pension API" "Stores and retrieves tax reports and pensions; no website integration yet." "ASP.NET Core"
            updater = container "Bank Account Updater" "Applies balance changes from account events." ".NET Worker"
            aggregator = container "Movement Aggregator" "Builds reporting summaries from account events." ".NET Worker"
            events = container "Event Store" "Stores account events for asynchronous processing." "EventStoreDB" {
                tags "Database"
            }
            mainDb = container "Main database" "Stores finance records and reference data." "SQL Server" {
                tags "Database"
            }
            bankDb = container "Bank database" "Stores bank and account records." "SQL Server" {
                tags "Database"
            }
            tnpDb = container "TNP database" "Stores pension and tax records." "MySQL" {
                tags "Database"
            }
            reportingDb = container "Reporting database" "Stores movement aggregates." "SQL Server" {
                tags "Database"
            }
        }

        user -> react "Uses the website"
        admin -> api "Maintains shared defaults" "HTTPS / JSON"
        react -> host "Loads the UI and calls /api/frontend" "Same origin; session cookie"
        host -> keycloak "Signs users in" "OIDC"
        host -> api "Calls existing finance APIs" "HTTPS / JSON; bearer token"
        host -> iconStorage "Reads and writes bank icons when configured" "S3; local files by default"
        api -> keycloak "Validates access tokens" "JWT / OIDC"
        bank -> keycloak "Validates access tokens" "JWT / OIDC"
        tnp -> keycloak "Validates access tokens" "JWT / OIDC"
        api -> bank "Forwards banking requests" "HTTPS / JSON"
        api -> tnp "Forwards TNP requests from API clients" "HTTPS / JSON"
        api -> mainDb "Reads and writes" "SQL"
        api -> reportingDb "Reads summaries" "SQL"
        bank -> bankDb "Reads and writes" "SQL"
        tnp -> tnpDb "Reads and writes" "SQL"
        api -> events "Publishes account events" "gRPC"
        updater -> events "Subscribes to account events" "gRPC"
        updater -> bank "Updates balances" "HTTPS / JSON"
        updater -> keycloak "Obtains a service token" "OIDC"
        aggregator -> events "Subscribes to account events" "gRPC"
        aggregator -> reportingDb "Writes summaries" "SQL"
    }

    views {
        systemContext pfm "pfm-system-context" "PFM users, administration and live identity provider" {
            include user admin pfm keycloak iconStorage
            autoLayout lr
        }

        container pfm "pfm-live-containers" "Live website, finance APIs, and asynchronous processing" {
            include user admin react host api bank tnp updater aggregator events mainDb bankDb tnpDb reportingDb keycloak iconStorage
            autoLayout lr
        }

        container pfm "pfm-offline-containers" "Development only: local sign-in and in-memory demo data in the website host; no downstream APIs or infrastructure" {
            include user react host
            autoLayout lr
        }

        styles {
            element "Person" {
                shape person
                background #0B2E59
                color #FFFFFF
            }
            element "Software System" {
                background #1565C0
                color #FFFFFF
            }
            element "Container" {
                background #1565C0
                color #FFFFFF
            }
            element "External" {
                background #D9D9D9
                color #222222
            }
            element "Database" {
                shape cylinder
                background #DCECFF
                color #222222
            }
            relationship "Relationship" {
                color #43556B
            }
        }
    }
}
