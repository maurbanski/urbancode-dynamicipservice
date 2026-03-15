FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
RUN useradd -u 1656 dynamic-dns
USER dynamic-dns
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Urbancode.DynamicIPService/Urbancode.DynamicIPService.Service.csproj", "Urbancode.DynamicIPService/"]
COPY ["Urbancode.DynamicIPService.Models/Urbancode.DynamicIPService.Models.csproj", "Urbancode.DynamicIPService.Models/"]
RUN dotnet restore "Urbancode.DynamicIPService/Urbancode.DynamicIPService.Service.csproj"
COPY . .
WORKDIR "/src/Urbancode.DynamicIPService"
RUN dotnet build "./Urbancode.DynamicIPService.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Urbancode.DynamicIPService.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Urbancode.DynamicIPService.Service.dll"]
