# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj", "src/Backend/MinhaAgendaDeContatos.Api/"]
COPY ["src/Shared/MinhaAgendaDeContatos.Comunicacao/MinhaAgendaDeContatos.Comunicacao.csproj", "src/Shared/MinhaAgendaDeContatos.Comunicacao/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Domain/MinhaAgendaDeContatos.Domain.csproj", "src/Backend/MinhaAgendaDeContatos.Domain/"]
COPY ["src/Shared/MinhaAgendaDeContatos.Exceptions/MinhaAgendaDeContatos.Exceptions.csproj", "src/Shared/MinhaAgendaDeContatos.Exceptions/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Application/MinhaAgendaDeContatos.Application.csproj", "src/Backend/MinhaAgendaDeContatos.Application/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Infraestrutura/MinhaAgendaDeContatos.Infraestrutura.csproj", "src/Backend/MinhaAgendaDeContatos.Infraestrutura/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Produtor/MinhaAgendaDeContatos.Produtor.csproj", "src/Backend/MinhaAgendaDeContatos.Produtor/"]
RUN dotnet restore "./src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj"
COPY . .
WORKDIR "/src/src/Backend/MinhaAgendaDeContatos.Api"
RUN dotnet build "./MinhaAgendaDeContatos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MinhaAgendaDeContatos.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinhaAgendaDeContatos.Api.dll"]