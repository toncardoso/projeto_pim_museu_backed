# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos do projeto
COPY MarsMuseumAPI/*.csproj ./MarsMuseumAPI/
RUN dotnet restore ./MarsMuseumAPI/MarsMuseumAPI.csproj

# Copiar todo o código e compilar
COPY MarsMuseumAPI/. ./MarsMuseumAPI/
WORKDIR /app/MarsMuseumAPI
RUN dotnet publish -c Release -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Porta que o Render vai usar
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "MarsMuseumAPI.dll"]
