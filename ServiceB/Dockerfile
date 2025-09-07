FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ServiceB/ServiceB.csproj", "ServiceB/"]
RUN dotnet restore "ServiceB/ServiceB.csproj"
COPY . .
WORKDIR "/src/ServiceB"
RUN dotnet build "ServiceB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceB.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceB.dll"]
