#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BudgetLambda.Server/BudgetLambda.Server.csproj", "BudgetLambda.Server/"]
RUN dotnet restore "BudgetLambda.Server/BudgetLambda.Server.csproj"
COPY . .
WORKDIR "/src/BudgetLambda.Server"
RUN dotnet build "BudgetLambda.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetLambda.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetLambda.Server.dll"]