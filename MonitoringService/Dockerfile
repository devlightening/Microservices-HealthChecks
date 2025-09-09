FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MonitoringService/MonitoringService.csproj", "MonitoringService/"]
RUN dotnet restore "MonitoringService/MonitoringService.csproj"
COPY . .
WORKDIR "/src/MonitoringService"
RUN dotnet build "MonitoringService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonitoringService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitoringService.dll"]