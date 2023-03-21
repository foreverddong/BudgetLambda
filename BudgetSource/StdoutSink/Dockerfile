#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM ghcr.io/openfaas/of-watchdog:0.9.11 as watchdog
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StdoutSink/StdoutSink.csproj", "StdoutSink/"]
RUN dotnet restore "StdoutSink/StdoutSink.csproj"
COPY . .
WORKDIR "/src/StdoutSink"
RUN dotnet build "StdoutSink.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StdoutSink.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

RUN echo $PATH
ENV mode="http"
ENV upstream_url="http://127.0.0.1:80"
ENV fprocess="dotnet StdoutSink.dll"
CMD ["fwatchdog"]