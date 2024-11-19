# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY ["NhaSachMetMoi.sln", "./"]
COPY ["NhaSachMetMoi/NhaSachMetMoi.csproj", "NhaSachMetMoi/"]

# Restore dependencies
RUN dotnet restore "NhaSachMetMoi/NhaSachMetMoi.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build "NhaSachMetMoi/NhaSachMetMoi.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "NhaSachMetMoi/NhaSachMetMoi.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NhaSachMetMoi.dll"]
