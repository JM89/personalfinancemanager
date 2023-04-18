FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

EXPOSE 5000

COPY src/PFM.Authentication.Api/. ./PFM.Authentication.Api/
COPY src/PFM.Authentication.Api.Contracts/. ./PFM.Authentication.Api.Contracts/

RUN dotnet build ./PFM.Authentication.Api/PFM.Authentication.Api.csproj -c Release 
RUN dotnet publish ./PFM.Authentication.Api/PFM.Authentication.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/PFM.Authentication.Api
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PFM.Authentication.Api.dll"]