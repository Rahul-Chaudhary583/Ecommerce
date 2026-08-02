# Base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER root

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libgssapi-krb5-2 \
        krb5-user && \
    rm -rf /var/lib/apt/lists/*

USER $APP_UID

WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Services/Discount/Discount.API/Discount.API.csproj", "Services/Discount/Discount.API/"]
COPY ["Services/Discount/Discount.Application/Discount.Application.csproj", "Services/Discount/Discount.Application/"]
COPY ["Services/Discount/Discount.Core/Discount.Core.csproj", "Services/Discount/Discount.Core/"]
COPY ["Services/Discount/Discount.Infrastructure/Discount.Infrastructure.csproj", "Services/Discount/Discount.Infrastructure/"]

RUN dotnet restore "./Services/Discount/Discount.API/Discount.API.csproj"

COPY . .

WORKDIR "/src/Services/Discount/Discount.API"

RUN dotnet build "Discount.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Publish
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Discount.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# Final
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Discount.API.dll"]