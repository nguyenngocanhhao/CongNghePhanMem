# Use the official .NET image as a base for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ["NhaSachMetMoi.sln", "./"]
COPY ["NhaSachMetMoi/NhaSachMetMoi.csproj", "NhaSachMetMoi/"]
RUN dotnet restore

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src/NhaSachMetMoi"
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Use the base image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NhaSachMetMoi.dll"]
