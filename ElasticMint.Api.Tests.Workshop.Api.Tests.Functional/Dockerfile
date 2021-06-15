#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ElasticMint.Api.Tests.Workshop.Api.Tests.Functional/ElasticMint.Api.Tests.Workshop.Api.Tests.Functional.csproj", "ElasticMint.Api.Tests.Workshop.Api.Tests.Functional/"]
COPY ["ElasticMint.Api.Test.Workshop.Api/ElasticMint.Api.Test.Workshop.Api.csproj", "ElasticMint.Api.Test.Workshop.Api/"]
RUN dotnet restore "ElasticMint.Api.Tests.Workshop.Api.Tests.Functional/ElasticMint.Api.Tests.Workshop.Api.Tests.Functional.csproj"
COPY . .
WORKDIR "/src/ElasticMint.Api.Tests.Workshop.Api.Tests.Functional"
RUN dotnet build "ElasticMint.Api.Tests.Workshop.Api.Tests.Functional.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElasticMint.Api.Tests.Workshop.Api.Tests.Functional.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElasticMint.Api.Tests.Workshop.Api.Tests.Functional.dll"]