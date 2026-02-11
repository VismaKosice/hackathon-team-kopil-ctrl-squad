FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PensionCalculationEngine.Api/PensionCalculationEngine.Api.csproj", "PensionCalculationEngine.Api/"]
COPY ["PensionCalculationEngine.Domain/PensionCalculationEngine.Domain.csproj", "PensionCalculationEngine.Domain/"]
COPY ["PensionCalculationEngine.Shared/PensionCalculationEngine.Shared.csproj", "PensionCalculationEngine.Shared/"]
RUN dotnet restore "PensionCalculationEngine.Api/PensionCalculationEngine.Api.csproj"
COPY . .
WORKDIR "/src/PensionCalculationEngine.Api"
RUN dotnet build "./PensionCalculationEngine.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PensionCalculationEngine.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PensionCalculationEngine.Api.dll"]
