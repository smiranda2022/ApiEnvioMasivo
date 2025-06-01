# Imagen base con SDK para compilar
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Imagen con SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiEnvioMasivo.csproj", "./"]
RUN dotnet restore "./ApiEnvioMasivo.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet publish "ApiEnvioMasivo.csproj" -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiEnvioMasivo.dll"]
