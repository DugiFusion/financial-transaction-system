# Use the official ASP.NET runtime image as the base for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8087

# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj files and restore dependencies
COPY ["Services/Reporting.API/Reporting.API.csproj", "Services/Reporting.API/"]
COPY ["Services/Transaction.API/Transaction.API.csproj", "Services/Transaction.API/"]
RUN dotnet restore "Services/Reporting.API/Reporting.API.csproj"

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src/Services/Reporting.API"
RUN dotnet build "Reporting.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the Reporting.API service to a separate folder
FROM build AS publish_reporting
RUN dotnet publish "Services/Reporting.API/Reporting.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish_reporting /p:UseAppHost=false

# Publish the Transaction.API service to a separate folder 
FROM build AS publish_transaction
RUN dotnet publish "Services/Transaction.API/Transaction.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish_transaction /p:UseAppHost=false


# Final image with the runtime-only .NET environment
FROM base AS final
WORKDIR /app

# Copy the published Reporting.API from the build stage
COPY --from=publish_reporting /app/publish_reporting ./ReportingAPI

# Copy the published Transaction.API from the build stage
COPY --from=publish_transaction /app/publish_transaction ./TransactionAPI

# Set the ASP.NET Core environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Change the default port to 8087
ENV ASPNETCORE_URLS=http://+:8087

# Set the entry point for the Reporting.API container
ENTRYPOINT ["dotnet", "ReportingAPI/Reporting.API.dll"]
