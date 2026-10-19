
# ==========================================
# Stage 1: Build and publish
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution and projects
COPY IELS.FIR.slnx ./

COPY IELS.FIR.Api/*.csproj IELS.FIR.Api/
COPY IELS.FIR.Application/*.csproj IELS.FIR.Application/
COPY IELS.FIR.Domain/*.csproj IELS.FIR.Domain/
COPY IELS.FIR.Infrastructure/*.csproj IELS.FIR.Infrastructure/

# Restore dependencies
RUN dotnet restore IELS.FIR.Api/IELS.FIR.Api.csproj

# Copy remaining source code
COPY . .

# Publish release build
RUN dotnet publish IELS.FIR.Api/IELS.FIR.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ==========================================
# Stage 2: Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Run as the built-in non-root .NET user
USER $APP_UID

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "IELS.FIR.Api.dll"]
