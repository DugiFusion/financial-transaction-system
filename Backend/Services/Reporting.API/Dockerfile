# Use the official ASP.NET runtime image as the base for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8087

# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj and restore dependencies
COPY ["Reporting.API.csproj", "."]
RUN dotnet restore "Reporting.API.csproj"

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src"
RUN dotnet build "Reporting.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Reporting.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image with the runtime-only .NET environment
FROM base AS final
WORKDIR /app

# Copy the published app from the build stage
COPY --from=publish /app/publish .

# Set the ASP.NET Core environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Change the default port to 8087
ENV ASPNETCORE_URLS=http://+:8087

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Reporting.API.dll"]
