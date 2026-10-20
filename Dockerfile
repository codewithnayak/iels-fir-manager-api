
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for restore caching
COPY src/IELS.FIR.Domain/*.csproj src/IELS.FIR.Domain/
COPY src/IELS.FIR.Application/*.csproj src/IELS.FIR.Application/
COPY src/IELS.FIR.Infrastructure/*.csproj src/IELS.FIR.Infrastructure/
COPY src/IELS.FIR.Api/*.csproj src/IELS.FIR.Api/

RUN dotnet restore src/IELS.FIR.Api/IELS.FIR.Api.csproj

# Copy source and publish
COPY src/ src/

RUN dotnet publish src/IELS.FIR.Api/IELS.FIR.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

USER $APP_UID

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "IELS.FIR.Api.dll"]
