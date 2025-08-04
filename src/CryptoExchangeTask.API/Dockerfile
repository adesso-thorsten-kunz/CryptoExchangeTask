# Use Alpine base image instead of default (Debian) because:
# - reduces image size
# - reduces the number of CVEs
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CryptoExchangeTask.API/CryptoExchangeTask.API.csproj", "src/CryptoExchangeTask.API/"]
RUN dotnet restore "./src/CryptoExchangeTask.API/CryptoExchangeTask.API.csproj"
COPY . .
WORKDIR "/src/src/CryptoExchangeTask.API"
RUN dotnet build "./CryptoExchangeTask.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoExchangeTask.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoExchangeTask.API.dll"]