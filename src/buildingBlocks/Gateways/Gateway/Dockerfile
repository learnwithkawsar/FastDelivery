#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/buildingBlocks/Gateways/Gateway/Gateway.csproj", "src/buildingBlocks/Gateways/Gateway/"]
COPY ["src/framework/Core/FastDelivery.Framework.Core.csproj", "src/framework/Core/"]
COPY ["src/framework/Infrastructure/FastDelivery.Framework.Infrastructure.csproj", "src/framework/Infrastructure/"]
COPY ["src/buildingBlocks/EventBus/EventBus.csproj", "src/buildingBlocks/EventBus/"]
COPY ["src/buildingBlocks/Healthchecks/Healthchecks.csproj", "src/buildingBlocks/Healthchecks/"]
RUN dotnet restore "./src/buildingBlocks/Gateways/Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/src/buildingBlocks/Gateways/Gateway"
RUN dotnet build "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]