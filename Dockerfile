# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["DocumentFlowAPI/DocumentFlowAPI.csproj", "DocumentFlowAPI/"]
RUN dotnet restore "DocumentFlowAPI/DocumentFlowAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DocumentFlowAPI"
RUN dotnet build "DocumentFlowAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DocumentFlowAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for health checks (optional)
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

# Expose port
EXPOSE 80

# Copy published application
COPY --from=publish /app/publish .
# Set entry point
ENTRYPOINT ["dotnet", "DocumentFlowAPI.dll"]