#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CSharpFunction/CSharpFunction.csproj", "CSharpFunction/"]
RUN dotnet restore "CSharpFunction/CSharpFunction.csproj"
COPY . .
