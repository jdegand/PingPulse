# Stage 1: Build the application using the full .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["PingPulse.csproj", "./"]
RUN dotnet restore "PingPulse.csproj"

# Copy the remaining source files and build
COPY . .
RUN dotnet publish "PingPulse.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime image using the lean ASP.NET Core 10 runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PingPulse.dll"]
