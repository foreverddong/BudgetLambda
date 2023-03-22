FROM ghcr.io/openfaas/of-watchdog:0.9.11 as watchdog
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM registry.donglinxu.com/budget/csharplambdamap:latest AS build
WORKDIR "/src/CSharpFunction"
COPY "InputModel.cs" .
COPY "OutputModel.cs" .
COPY "Handler.cs" .
RUN dotnet build "CSharpFunction.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSharpFunction.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

RUN echo $PATH
ENV mode="http"
ENV upstream_url="http://127.0.0.1:80"
ENV fprocess="dotnet CSharpFunction.dll"
CMD ["fwatchdog"]
