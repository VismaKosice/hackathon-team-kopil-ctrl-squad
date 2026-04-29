# syntax=docker/dockerfile:1.6
#
# Two-stage native AOT build. Final image is a chiseled (distroless-style)
# Ubuntu base with nothing but glibc + tls — the AOT-compiled engine runs as
# a single native ELF binary.

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Native AOT toolchain on Linux requires clang + zlib headers.
RUN apt-get update \
 && apt-get install -y --no-install-recommends clang zlib1g-dev \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Restore in its own layer so subsequent code-only changes hit the cache.
COPY PensionCalculationEngine/PensionCalculationEngine.Shared/PensionCalculationEngine.Shared.csproj PensionCalculationEngine.Shared/
COPY PensionCalculationEngine/PensionCalculationEngine.Domain/PensionCalculationEngine.Domain.csproj PensionCalculationEngine.Domain/
COPY PensionCalculationEngine/PensionCalculationEngine.Api/PensionCalculationEngine.Api.csproj PensionCalculationEngine.Api/

RUN dotnet restore PensionCalculationEngine.Api/PensionCalculationEngine.Api.csproj \
    -r linux-x64

COPY PensionCalculationEngine/PensionCalculationEngine.Shared/ PensionCalculationEngine.Shared/
COPY PensionCalculationEngine/PensionCalculationEngine.Domain/ PensionCalculationEngine.Domain/
COPY PensionCalculationEngine/PensionCalculationEngine.Api/ PensionCalculationEngine.Api/

RUN dotnet publish PensionCalculationEngine.Api/PensionCalculationEngine.Api.csproj \
    -c Release \
    -r linux-x64 \
    -o /app/publish \
    /p:PublishAot=true \
    /p:StripSymbols=true

FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-noble-chiseled AS runtime
WORKDIR /app
COPY --from=build /app/publish/PensionCalculationEngine.Api ./engine

ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    DOTNET_TieredPGO=1

EXPOSE 8080
ENTRYPOINT ["./engine"]
