#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MockAcquiringBank.API/MockAcquiringBank.API.csproj", "MockAcquiringBank.API/"]
RUN dotnet restore "MockAcquiringBank.API/MockAcquiringBank.API.csproj"
COPY . .
WORKDIR "/src/MockAcquiringBank.API"
RUN dotnet build "MockAcquiringBank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockAcquiringBank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockAcquiringBank.API.dll"]
