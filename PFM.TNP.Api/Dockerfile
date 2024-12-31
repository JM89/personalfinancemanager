FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

ARG GH_PKG_TOKEN_ARG
ENV GH_PKG_TOKEN=$GH_PKG_TOKEN_ARG

EXPOSE 4431

COPY src/Api/. ./Api/
COPY src/Api.Contracts/. ./Api.Contracts/
COPY src/Event.Contracts/. ./Event.Contracts/
COPY src/DataAccessLayer/. ./DataAccessLayer/
COPY src/Services/. ./Services/

RUN dotnet nuget add source --username jm89 "https://nuget.pkg.github.com/jm89/index.json" --name "JM89-GitHub-Packages" --store-password-in-clear-text --password $GH_PKG_TOKEN
RUN dotnet build ./Api/Api.csproj -c Release 
RUN dotnet publish ./Api/Api.csproj -c Release -o out
RUN dotnet dev-certs https --trust

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app/Api
COPY --from=build-env /app/out .
COPY --from=build-env /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/

ENTRYPOINT ["dotnet", "Api.dll"]