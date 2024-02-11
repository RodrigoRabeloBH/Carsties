#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuctionSearchService.Api/AuctionSearchService.Api.csproj", "AuctionSearchService.Api/"]
COPY ["AuctionSearchService.Application/AuctionSearchService.Application.csproj", "AuctionSearchService.Application/"]
COPY ["AuctionSearchService.Domain/AuctionSearchService.Domain.csproj", "AuctionSearchService.Domain/"]
COPY ["AuctionSearchService.CrossCutting/AuctionSearchService.CrossCutting.csproj", "AuctionSearchService.CrossCutting/"]
COPY ["AuctionSearchService.Infrastructure/AuctionSearchService.Infrastructure.csproj", "AuctionSearchService.Infrastructure/"]
RUN dotnet restore "AuctionSearchService.Api/AuctionSearchService.Api.csproj"
COPY . .
WORKDIR "/src/AuctionSearchService.Api"
RUN dotnet build "AuctionSearchService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuctionSearchService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionSearchService.Api.dll"]