# Используйте базовый образ от Microsoft
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
# Копируйте csproj и восстанавливайте зависимости
COPY *.sln ./
COPY SocialService.WebApi/*.csproj ./SocialService.WebApi/
COPY SocialService.Infrastructure/*.csproj ./SocialService.Infrastructure/
COPY SocialService.Core/*.csproj ./SocialService.Core/
COPY SocialService.Application/*.csproj ./SocialService.Application/
COPY SocialService.DataAccess/*.csproj ./SocialService.DataAccess/
COPY SocialService.Shared/*.csproj ./SocialService.Shared/
RUN dotnet restore



# Копируйте остальной проект и собирайте
COPY . ./
RUN dotnet publish SocialService.WebApi/*.csproj -c Release -o out

# Генерируйте runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
# Копирование сертификатов в образ
COPY ["/certs/SocialService.WebApi.pfx", "/https/"]
ENV ASPNETCORE_ENVIRONMENT=InDocker
ENTRYPOINT ["dotnet", "SocialService.WebApi.dll"]
