#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Crawler/Crawler.Main/Crawler.Main.csproj", "Crawler.Main/"]
COPY ["ExchangeTypes/ExchangeTypes/ExchangeTypes.csproj", "ExchangeTypes/"]
RUN dotnet restore "Crawler.Main/Crawler.Main.csproj"
COPY ./Crawler ./Crawler
COPY ./ExchangeTypes ./ExchangeTypes
WORKDIR "/src/Crawler/Crawler.Main"
RUN dotnet build "Crawler.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crawler.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawler.Main.dll"]