﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrderPickingSystem/OrderPickingSystem.csproj", "OrderPickingSystem/"]
RUN dotnet restore "OrderPickingSystem/OrderPickingSystem.csproj"
COPY . .
WORKDIR "/src/OrderPickingSystem"
RUN dotnet build "OrderPickingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderPickingSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderPickingSystem.dll"]
