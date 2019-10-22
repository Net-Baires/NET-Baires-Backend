FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["NetBaires.Api/NetBaires.Api.csproj", "NetBaires.Api/"]
RUN dotnet restore "NetBaires.Api/NetBaires.Api.csproj"
COPY . .
WORKDIR "/src/NetBaires.Api"
RUN dotnet build "NetBaires.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetBaires.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetBaires.Api.dll"]