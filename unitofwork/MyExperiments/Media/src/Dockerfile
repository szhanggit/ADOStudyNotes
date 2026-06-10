#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN apt-get update && \
    apt-get install -y -q curl gnupg2
RUN curl http://nginx.org/keys/nginx_signing.key | apt-key add -

RUN apt-get install -y -q nginx
RUN apt-get install -y -q dnsutils

WORKDIR /app
EXPOSE 80 8001 9001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TXC.Proto.Media/TXC.Proto.Media.csproj", "TXC.Proto.Media/"]
COPY ["ServiceMedia.Api/ServiceMedia.Api.csproj", "ServiceMedia.Api/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["nuget.config", "TXC.Proto.Media/"]
COPY ["nuget.config", "Domain/"]
COPY ["nuget.config", "Infrastructure/"]
COPY ["nuget.config", "Services/"]
COPY ["nuget.config", "ServiceMedia.Api/"]


RUN dotnet restore "ServiceMedia.Api/ServiceMedia.Api.csproj"
COPY . .
WORKDIR "/src/ServiceMedia.Api"
RUN dotnet build "ServiceMedia.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceMedia.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:8001  

ENTRYPOINT ["dotnet", "ServiceMedia.Api.dll"]