FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
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

RUN apt-get update -y && apt-get install -y wget && \
    wget -O dotMemoryclt.zip https://www.nuget.org/api/v2/package/JetBrains.dotMemory.Console.linux-x64 && \
    apt-get install -y unzip && \
    unzip dotMemoryclt.zip -d ./dotMemoryclt && \
    chmod +x -R dotMemoryclt/* 

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryLeaksDemo.dll"]
