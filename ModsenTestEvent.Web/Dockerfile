FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ModsenTestEvent.Web/ModsenTestEvent.Web.csproj", "ModsenTestEvent.Web/"]
COPY ["ModsenTestEvent.Domain/ModsenTestEvent.Domain.csproj", "ModsenTestEvent.Domain/"]
COPY ["ModsenTestEvent.Infrastructure/ModsenTestEvent.Infrastructure.csproj", "ModsenTestEvent.Infrastructure/"]
COPY ["ModsenTestEvent.Application/ModsenTestEvent.Application.csproj", "ModsenTestEvent.Application/"]
RUN dotnet restore "ModsenTestEvent.Web/ModsenTestEvent.Web.csproj"
COPY . .
WORKDIR "/src/ModsenTestEvent.Web"
RUN dotnet build "ModsenTestEvent.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ModsenTestEvent.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModsenTestEvent.Web.dll"]
