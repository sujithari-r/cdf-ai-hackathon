FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/RenewableDashboard.Api/RenewableDashboard.Api.csproj", "src/RenewableDashboard.Api/"]
COPY ["src/RenewableDashboard.Core/RenewableDashboard.Core.csproj", "src/RenewableDashboard.Core/"]
COPY ["src/RenewableDashboard.Infrastructure/RenewableDashboard.Infrastructure.csproj", "src/RenewableDashboard.Infrastructure/"]
RUN dotnet restore "src/RenewableDashboard.Api/RenewableDashboard.Api.csproj"
COPY . .
RUN dotnet build "src/RenewableDashboard.Api/RenewableDashboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/RenewableDashboard.Api/RenewableDashboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RenewableDashboard.Api.dll"]
