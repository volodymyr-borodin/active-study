FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ActiveStudy.Web/ActiveStudy.Web.csproj
WORKDIR /src/ActiveStudy.Web
RUN dotnet build ActiveStudy.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ActiveStudy.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ActiveStudy.Web.dll"]
