FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ARG GH_PKG_TOKEN_ARG
ENV GH_PKG_TOKEN=$GH_PKG_TOKEN_ARG

COPY src/PFM.MovementAggregator/. ./PFM.MovementAggregator/

RUN dotnet nuget add source --username jm89 "https://nuget.pkg.github.com/jm89/index.json" --name "JM89-GitHub-Packages" --store-password-in-clear-text --password $GH_PKG_TOKEN
RUN dotnet build ./PFM.MovementAggregator/PFM.MovementAggregator.csproj -c Release 
RUN dotnet publish ./PFM.MovementAggregator/PFM.MovementAggregator.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app/PFM.MovementAggregator
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PFM.MovementAggregator.dll"]