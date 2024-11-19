# Use the official .NET Framework SDK image to build the app (Windows base image)
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build

# Set working directory
WORKDIR /src

# Copy the solution and projects files
COPY ["NhaSachMetMoi.sln", "./"]
COPY ["NhaSachMetMoi/NhaSachMetMoi.csproj", "NhaSachMetMoi/"]

# Restore dependencies
RUN nuget restore "NhaSachMetMoi.sln"

# Copy the rest of the application code
COPY . .

# Build the application using MSBuild
WORKDIR "/src/NhaSachMetMoi"
RUN msbuild "NhaSachMetMoi.csproj" /p:Configuration=Release /p:Platform="Any CPU"

# Use IIS base image to host ASP.NET MVC
FROM mcr.microsoft.com/windows/servercore/iis:windowsservercore-ltsc2019 AS runtime

# Install ASP.NET 4.8 (for ASP.NET MVC support)
RUN dism /online /enable-feature /featurename:IIS-ASPNET45 /all

# Set working directory
WORKDIR /inetpub/wwwroot

# Copy the build output to IIS directory
COPY --from=build /src/NhaSachMetMoi/. ./

# Expose port 80
EXPOSE 80
