#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Api/"]
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Application/MinhaAgendaDeContatos.Application.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Application/"]
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Domain/MinhaAgendaDeContatos.Domain.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Domain/"]
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Infraestrutura/MinhaAgendaDeContatos.Infraestrutura.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Infraestrutura/"]
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Produtor/MinhaAgendaDeContatos.Produtor.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Produtor/"]
COPY ["MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Consumidor/MinhaAgendaDeContatos.Consumidor.csproj", "MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Consumidor/"]
RUN dotnet restore "./MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj"
COPY . .
WORKDIR "/src/MinhaAgendaDeContatos/Backend/MinhaAgendaDeContatos.Api"
RUN dotnet build "./MinhaAgendaDeContatos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MinhaAgendaDeContatos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinhaAgendaDeContatos.Api.dll"]