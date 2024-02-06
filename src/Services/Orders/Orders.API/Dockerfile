#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/Orders/Orders.API/Orders.API.csproj", "Services/Orders/Orders.API/"]
COPY ["Services/Infrastructure/Orders.Infrastructure/Orders.Infrastructure.csproj", "Services/Infrastructure/Orders.Infrastructure/"]
COPY ["Services/Orders/Orders.Application/Orders.Application.csproj", "Services/Orders/Orders.Application/"]
COPY ["Services/Orders/Orders.Domain/Orders.Domain.csproj", "Services/Orders/Orders.Domain/"]
RUN dotnet restore "Services/Orders/Orders.API/Orders.API.csproj"
COPY . .
WORKDIR "/src/Services/Orders/Orders.API"
RUN dotnet publish "Orders.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orders.API.dll"]
