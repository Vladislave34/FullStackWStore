# syntax=docker/dockerfile:1
# ---------- Build stage ----------
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
# Копіюємо csproj-файли всіх трьох проєктів (зберігаючи структуру папок)
COPY ["WebApp/WebApp.csproj", "WebApp/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "WebApp/WebApp.csproj" -p:RestoreDisableParallel=true
# Копіюємо решту коду і публікуємо
COPY . .
WORKDIR /source/WebApp
RUN dotnet publish "WebApp.csproj" -c Release -o /app

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
RUN mkdir -p /app/images && chown -R app:app /app/images
USER app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApp.dll"]