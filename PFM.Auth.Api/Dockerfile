FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

EXPOSE 5000

# Copy everything else and build
COPY src/PFM.Authentication.Api/. ./
RUN dotnet build ./PFM.Authentication.Api.csproj -c Release 
RUN dotnet publish ./PFM.Authentication.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PFM.Authentication.Api.dll"]