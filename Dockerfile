# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY PeminjamanRuangan.Api/PeminjamanRuangan.Api.csproj PeminjamanRuangan.Api/
RUN dotnet restore PeminjamanRuangan.Api/PeminjamanRuangan.Api.csproj

# Copy everything else and build
COPY PeminjamanRuangan.Api/ PeminjamanRuangan.Api/
WORKDIR /src/PeminjamanRuangan.Api
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 80

# Copy published files
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PeminjamanRuangan.Api.dll"]
