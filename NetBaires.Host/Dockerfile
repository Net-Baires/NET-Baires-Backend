#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["NetBaires.Host/NetBaires.Host.csproj", "NetBaires.Host/"]
COPY ["NetBaires.Api/NetBaires.Api.csproj", "NetBaires.Api/"]
COPY ["NetBaires.Data/NetBaires.Data.csproj", "NetBaires.Data/"]
RUN dotnet restore "NetBaires.Host/NetBaires.Host.csproj"
COPY . .
WORKDIR "/src/NetBaires.Host"
RUN dotnet build "NetBaires.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetBaires.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetBaires.Host.dll"]