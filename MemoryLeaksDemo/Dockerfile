FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /tools
RUN dotnet tool install --tool-path /tools dotnet-trace \
 && dotnet tool install --tool-path /tools dotnet-counters \
 && dotnet tool install --tool-path /tools dotnet-dump \
 && dotnet tool install --tool-path /tools dotnet-gcdump

WORKDIR /src
COPY ["MemoryLeaksDemo/MemoryLeaksDemo.csproj", "MemoryLeaksDemo/"]
RUN dotnet restore "MemoryLeaksDemo/MemoryLeaksDemo.csproj"
COPY . .
WORKDIR "/src/MemoryLeaksDemo"
RUN dotnet build "MemoryLeaksDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MemoryLeaksDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /tools .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryLeaksDemo.dll"]
