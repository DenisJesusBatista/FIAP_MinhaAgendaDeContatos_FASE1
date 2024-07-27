# Define a imagem base de SDK do .NET para construção
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura as dependências
COPY ["src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj", "src/Backend/MinhaAgendaDeContatos.Api/"]
COPY ["src/Shared/MinhaAgendaDeContatos.Comunicacao/MinhaAgendaDeContatos.Comunicacao.csproj", "src/Shared/MinhaAgendaDeContatos.Comunicacao/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Domain/MinhaAgendaDeContatos.Domain.csproj", "src/Backend/MinhaAgendaDeContatos.Domain/"]
COPY ["src/Shared/MinhaAgendaDeContatos.Exceptions/MinhaAgendaDeContatos.Exceptions.csproj", "src/Shared/MinhaAgendaDeContatos.Exceptions/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Application/MinhaAgendaDeContatos.Application.csproj", "src/Backend/MinhaAgendaDeContatos.Application/"]
COPY ["src/Backend/MinhaAgendaDeContatos.Infraestrutura/MinhaAgendaDeContatos.Infraestrutura.csproj", "src/Backend/MinhaAgendaDeContatos.Infraestrutura/"]

# Executa o dotnet restore para restaurar as dependências
RUN dotnet restore "src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj"

# Copia o resto do código fonte
COPY . .

# Executa o build do projeto
RUN dotnet build "src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj" -c Release -o /app/build

# Define a etapa de publicação
FROM build AS publish
RUN dotnet publish "src/Backend/MinhaAgendaDeContatos.Api/MinhaAgendaDeContatos.Api.csproj" -c Release -o /app/publish

# Define a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define o comando de entrada para a aplicação
ENTRYPOINT ["dotnet", "MinhaAgendaDeContatos.Api.dll"]
