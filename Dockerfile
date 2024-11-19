# Use the official image as a base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NhaSachMetMoi/NhaSachMetMoi.csproj", "NhaSachMetMoi/"]
RUN dotnet restore "NhaSachMetMoi/NhaSachMetMoi.csproj"
COPY . .
WORKDIR "/src/NhaSachMetMoi"
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NhaSachMetMoi.dll"]
