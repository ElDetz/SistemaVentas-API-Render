﻿# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Render usa la variable PORT
ENV ASPNETCORE_URLS=http://+:$PORT
ENTRYPOINT ["dotnet", "ventas_examen_final.dll"]
