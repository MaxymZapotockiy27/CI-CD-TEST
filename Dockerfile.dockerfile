# Використовуємо офіційний образ .NET SDK для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Встановлюємо робочу директорію в контейнері
WORKDIR /app

# Копіюємо файли проєкту в робочу директорію
COPY *.csproj ./
RUN dotnet restore

# Копіюємо інші файли проєкту та збираємо його
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Використовуємо менший runtime-образ для запуску застосунку
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Вказуємо команду запуску
ENTRYPOINT ["dotnet", "Convertator.dll"]
