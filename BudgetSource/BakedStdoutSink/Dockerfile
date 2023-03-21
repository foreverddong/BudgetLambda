#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM ghcr.io/openfaas/of-watchdog:0.9.11 as watchdog

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BakedStdoutSink/BakedStdoutSink.csproj", "BakedStdoutSink/"]
RUN dotnet restore "BakedStdoutSink/BakedStdoutSink.csproj"
COPY . .
WORKDIR "/src/BakedStdoutSink"
RUN dotnet build "BakedStdoutSink.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BakedStdoutSink.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BakedStdoutSink.dll"]

ENV mode="http"
ENV upstream_url="http://127.0.0.1:80"
ENV fprocess="dotnet BakedStdoutSink.dll"
CMD ["fwatchdog"]