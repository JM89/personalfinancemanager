FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

ARG GH_PKG_TOKEN_ARG
ENV GH_PKG_TOKEN=$GH_PKG_TOKEN_ARG

EXPOSE 4431

COPY src/PFM.Website/. ./PFM.Website/

RUN dotnet nuget add source --username jm89 "https://nuget.pkg.github.com/jm89/index.json" --name "JM89-GitHub-Packages" --store-password-in-clear-text --password $GH_PKG_TOKEN
RUN dotnet build ./PFM.Website/PFM.Website.csproj -c Release 
RUN dotnet publish ./PFM.Website/PFM.Website.csproj -c Release -o out
RUN dotnet dev-certs https --trust

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app/PFM.Website
COPY --from=build-env /app/out .
COPY --from=build-env /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/

ENTRYPOINT ["dotnet", "PFM.Website.dll"]