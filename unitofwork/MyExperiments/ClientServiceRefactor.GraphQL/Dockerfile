#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN apt-get update && \
    apt-get install -y -q curl gnupg2
	
RUN curl http://nginx.org/keys/nginx_signing.key | apt-key add -

RUN apt-get install -y -q nginx
RUN apt-get install -y -q dnsutils

WORKDIR /app
EXPOSE 80 8005 9005

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Client.Api/ServiceClient.Api.csproj", "Client.Api/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["nuget.config", "Domain/"]
COPY ["nuget.config", "Infrastructure/"]
COPY ["nuget.config", "Services/"]
COPY ["nuget.config", "Client.Api/"]


RUN dotnet restore "Client.Api/ServiceClient.Api.csproj"
COPY . .
WORKDIR "/src/Client.Api"
RUN dotnet build "ServiceClient.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceClient.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:8005

WORKDIR /app
ENTRYPOINT ["dotnet", "ServiceClient.Api.dll"]