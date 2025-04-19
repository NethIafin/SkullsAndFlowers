FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkullsAndFlowersGame/SkullsAndFlowersGame.csproj", "SkullsAndFlowersGame/"]
RUN dotnet restore "SkullsAndFlowersGame/SkullsAndFlowersGame.csproj"
COPY . .
WORKDIR "/src/SkullsAndFlowersGame"
RUN dotnet build "SkullsAndFlowersGame.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkullsAndFlowersGame.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkullsAndFlowersGame.dll"]
