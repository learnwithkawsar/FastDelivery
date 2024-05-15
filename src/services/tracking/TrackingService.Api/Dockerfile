#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/tracking/TrackingService.Api/TrackingService.Api.csproj", "src/services/tracking/TrackingService.Api/"]
COPY ["src/services/orders/Application/OrderService.Application.csproj", "src/services/orders/Application/"]
COPY ["src/buildingBlocks/EventBus/EventBus.csproj", "src/buildingBlocks/EventBus/"]
COPY ["src/framework/Core/FastDelivery.Framework.Core.csproj", "src/framework/Core/"]
COPY ["src/services/orders/Domain/OrderService.Domain.csproj", "src/services/orders/Domain/"]
COPY ["src/services/tracking/TrackingService.Infrastructure/TrackingService.Infrastructure.csproj", "src/services/tracking/TrackingService.Infrastructure/"]
COPY ["src/framework/Infrastructure/FastDelivery.Framework.Infrastructure.csproj", "src/framework/Infrastructure/"]
COPY ["src/services/tracking/TrackingService.Application/TrackingService.Application.csproj", "src/services/tracking/TrackingService.Application/"]
RUN dotnet restore "./src/services/tracking/TrackingService.Api/TrackingService.Api.csproj"
COPY . .
WORKDIR "/src/src/services/tracking/TrackingService.Api"
RUN dotnet build "./TrackingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrackingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackingService.Api.dll"]