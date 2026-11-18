FROM node:24-alpine AS frontend
WORKDIR /app
COPY src/PFM.Website.Client/package*.json ./
RUN npm ci
COPY src/PFM.Website.Client/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:11.0-preview AS build-env
WORKDIR /app

ARG GH_PKG_TOKEN_ARG
COPY src/PFM.Models/ ./PFM.Models/
COPY src/PFM.Utils/ ./PFM.Utils/
COPY src/PFM.Services/ ./PFM.Services/
COPY src/PFM.Website/ ./PFM.Website/
COPY --from=frontend /app/dist/ ./PFM.Website.Client/dist/

RUN dotnet nuget add source --username jm89 "https://nuget.pkg.github.com/jm89/index.json" --name "JM89-GitHub-Packages" --store-password-in-clear-text --password "$GH_PKG_TOKEN_ARG" \
    && dotnet publish ./PFM.Website/PFM.Website.csproj -c Release -o out \
    && dotnet nuget remove source "JM89-GitHub-Packages"
RUN dotnet dev-certs https

FROM mcr.microsoft.com/dotnet/aspnet:11.0-preview
WORKDIR /app/PFM.Website
EXPOSE 7143
COPY --from=build-env /app/out .
COPY --from=build-env /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/

ENTRYPOINT ["dotnet", "PFM.Website.dll"]