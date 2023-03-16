# Set the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj file and restore NuGet packages
COPY ./AppSettingsManagerApi/AppSettingsManagerApi.csproj ./
RUN dotnet restore

# Copy the rest of the application source code and build the project
COPY . .
RUN dotnet build --configuration Release --no-restore -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish --configuration Release --no-build -o /app/publish

# Set the base image for the final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the published files from the publish stage
COPY --from=publish /app/publish .

# Expose the application port
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "AppSettingsManagerApi.dll"]