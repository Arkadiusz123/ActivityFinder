# Używamy oficjalnego obrazu .NET jako obraz bazowy
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Używamy oficjalnego obrazu .NET SDK jako obraz budowy
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instalacja Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs

# Kopiowanie plików projektu i przygotowanie do restore
COPY ["activityfinder.client/nuget.config", "activityfinder.client/"]
COPY ["ActivityFinder.Server/ActivityFinder.Server.csproj", "ActivityFinder.Server/"]
RUN dotnet restore "./ActivityFinder.Server/ActivityFinder.Server.csproj"

# Kopiowanie pozostałych plików projektu
COPY . .

# Budowanie aplikacji
WORKDIR "/src/ActivityFinder.Server"
RUN dotnet build "./ActivityFinder.Server.csproj" -c Release -o /app/build

# Publikowanie aplikacji
FROM build AS publish
RUN dotnet publish "./ActivityFinder.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Finalny etap, używamy obrazu bazowego
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivityFinder.Server.dll"]
