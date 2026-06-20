FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY src/Server/Server.csproj src/Server/
COPY src/Core/Domain/CisApi.Src.Core.Domain.csproj src/Core/Domain/
COPY src/Core/Application/CisApi.Src.Core.Application.csproj src/Core/Application/
COPY src/Infrastructure/MySQL/CisApi.Src.Infrastructure.MySQL.csproj src/Infrastructure/MySQL/
COPY src/Presentation/RestApi/CisApi.Src.Presentation.RestApi.csproj src/Presentation/RestApi/
COPY src/Infrastructure/Auth/CisApi.Src.Infrastructure.Auth.csproj src/Infrastructure/Auth/
COPY src/Infrastructure/Http/CisApi.Src.Infrastructure.Http.csproj src/Infrastructure/Http/
COPY src/Infrastructure/IoC/CisApi.Src.Infrastructure.IoC.csproj src/Infrastructure/IoC/
RUN dotnet restore src/Server/Server.csproj

COPY . .
WORKDIR /app/src/Server
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 8005
ENTRYPOINT ["dotnet", "Server.dll"]