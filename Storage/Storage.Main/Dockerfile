#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Storage/Storage.Main/Storage.Main.csproj", "Storage.Main/"]
COPY ["ExchangeTypes/ExchangeTypes/ExchangeTypes.csproj", "ExchangeTypes/"]
RUN dotnet restore "Storage.Main/Storage.Main.csproj"
COPY ./Storage ./Storage
COPY ./ExchangeTypes ./ExchangeTypes
WORKDIR "/src/Storage/Storage.Main"
RUN dotnet build "Storage.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Storage.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Storage.Main.dll"]