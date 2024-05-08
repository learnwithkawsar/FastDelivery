#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/identity/Api/Api.csproj", "src/services/identity/Api/"]
COPY ["src/services/identity/Application/Application.csproj", "src/services/identity/Application/"]
COPY ["src/framework/Core/FastDelivery.Framework.Core.csproj", "src/framework/Core/"]
COPY ["src/services/identity/Domain/Domain.csproj", "src/services/identity/Domain/"]
COPY ["src/services/identity/Infrastructure/Infrastructure.csproj", "src/services/identity/Infrastructure/"]
COPY ["src/framework/Infrastructure/FastDelivery.Framework.Infrastructure.csproj", "src/framework/Infrastructure/"]
RUN dotnet restore "./src/services/identity/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/services/identity/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastDelivery.Service.Identity.Api.dll"]