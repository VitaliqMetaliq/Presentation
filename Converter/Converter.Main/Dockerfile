#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Converter/Converter.Main/Converter.Main.csproj", "Converter.Main/"]
COPY ["ExchangeTypes/ExchangeTypes/ExchangeTypes.csproj", "ExchangeTypes/"]
RUN dotnet restore "Converter.Main/Converter.Main.csproj"
COPY ./Converter ./Converter
COPY ./ExchangeTypes ./ExchangeTypes
WORKDIR "/src/Converter/Converter.Main"
RUN dotnet build "Converter.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Converter.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Converter.Main.dll"]