#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ApiGateways/OcelotApiGW/OcelotApiGW.csproj", "ApiGateways/OcelotApiGW/"]
RUN dotnet restore "ApiGateways/OcelotApiGW/OcelotApiGW.csproj"
COPY . .
WORKDIR "/src/ApiGateways/OcelotApiGW"
RUN dotnet publish "OcelotApiGW.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OcelotApiGW.dll"]
