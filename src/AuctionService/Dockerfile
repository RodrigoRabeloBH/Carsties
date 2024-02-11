#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuctionService.Api/AuctionService.Api.csproj", "AuctionService.Api/"]
COPY ["AuctionService.CrossCutting/AuctionService.CrossCutting.csproj", "AuctionService.CrossCutting/"]
COPY ["AuctionService.Application/AuctionService.Application.csproj", "AuctionService.Application/"]
COPY ["AuctionService.Domain/AuctionService.Domain.csproj", "AuctionService.Domain/"]
COPY ["AuctionService.Infrastructure/AuctionService.Infrastructure.csproj", "AuctionService.Infrastructure/"]
RUN dotnet restore "AuctionService.Api/AuctionService.Api.csproj"
COPY . .
WORKDIR "/src/AuctionService.Api"
RUN dotnet build "AuctionService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuctionService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionService.Api.dll"]