#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BiddingService.Api/BiddingService.Api.csproj", "BiddingService.Api/"]
COPY ["BiddingService.Application/BiddingService.Application.csproj", "BiddingService.Application/"]
COPY ["BiddingService.Domain/BiddingService.Domain.csproj", "BiddingService.Domain/"]
COPY ["BiddingService.Infrastructure/BiddingService.Infrastructure.csproj", "BiddingService.Infrastructure/"]
COPY ["BiddingService.CrossCutting/BiddingService.CrossCutting.csproj", "BiddingService.CrossCutting/"]
RUN dotnet restore "BiddingService.Api/BiddingService.Api.csproj"
COPY . .
WORKDIR "/src/BiddingService.Api"
RUN dotnet build "BiddingService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BiddingService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiddingService.Api.dll"]