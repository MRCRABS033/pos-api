FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Pos.Api/Pos.Api.csproj", "src/Pos.Api/"]
COPY ["src/Pos.Application/Pos.Application.csproj", "src/Pos.Application/"]
COPY ["src/Pos.Domain/Pos.Domain.csproj", "src/Pos.Domain/"]
COPY ["src/Pos.Infrastructure/Pos.Infrastructure.csproj", "src/Pos.Infrastructure/"]
RUN dotnet restore "src/Pos.Api/Pos.Api.csproj"
COPY . .
WORKDIR "/src/src/Pos.Api"
RUN dotnet build "./Pos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pos.Api.dll"]
