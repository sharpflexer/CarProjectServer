# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV DOTNET_RUNNING_IN_CONTAINER=true
WORKDIR /src
COPY ["CarProjectServer.API/CarProjectServer.API.csproj", "CarProjectServer.API/"]
COPY ["CarProjectServer.BL/CarProjectServer.BL.csproj", "CarProjectServer.BL/"]
COPY ["CarProjectServer.DAL/CarProjectServer.DAL.csproj", "CarProjectServer.DAL/"]
RUN dotnet restore "./CarProjectServer.API/CarProjectServer.API.csproj"
COPY . .
WORKDIR "/src/CarProjectServer.API"
RUN dotnet build "./CarProjectServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarProjectServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarProjectServer.API.dll"]