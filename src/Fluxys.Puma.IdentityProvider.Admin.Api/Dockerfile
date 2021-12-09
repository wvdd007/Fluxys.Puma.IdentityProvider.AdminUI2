FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/fluxysroot.crt", "/etc/ssl/certs/fluxysroot.crt"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.Api/Fluxys.Puma.IdentityProvider.Admin.Api.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.Api/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.SqlServer/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.SqlServer.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.SqlServer/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Shared/Fluxys.Puma.IdentityProvider.Shared.csproj", "src/Fluxys.Puma.IdentityProvider.Shared/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.PostgreSQL/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.PostgreSQL.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.MySql/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.MySql.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/Fluxys.Puma.IdentityProvider.Admin.Api/Fluxys.Puma.IdentityProvider.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/Fluxys.Puma.IdentityProvider.Admin.Api"
RUN dotnet build "Fluxys.Puma.IdentityProvider.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fluxys.Puma.IdentityProvider.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "Fluxys.Puma.IdentityProvider.Admin.Api.dll"]