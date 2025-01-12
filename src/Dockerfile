# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copiar os arquivos do projeto para o container
COPY . .

# Restaurar dependências e compilar
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os binários compilados para o runtime
COPY --from=build /app .

EXPOSE 8081

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "Stocks.dll"]
