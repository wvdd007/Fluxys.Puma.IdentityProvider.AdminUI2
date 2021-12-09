FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/fluxysroot.crt", "/etc/ssl/certs/fluxysroot.crt"]
COPY ["src/Fluxys.Puma.IdentityProvider.STS.Identity/Fluxys.Puma.IdentityProvider.STS.Identity.csproj", "src/Fluxys.Puma.IdentityProvider.STS.Identity/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared.csproj", "src/Fluxys.Puma.IdentityProvider.Admin.EntityFramework.Shared/"]
COPY ["src/Fluxys.Puma.IdentityProvider.Shared/Fluxys.Puma.IdentityProvider.Shared.csproj", "src/Fluxys.Puma.IdentityProvider.Shared/"]
RUN dotnet restore "src/Fluxys.Puma.IdentityProvider.STS.Identity/Fluxys.Puma.IdentityProvider.STS.Identity.csproj"
COPY . .
WORKDIR "/src/src/Fluxys.Puma.IdentityProvider.STS.Identity"
RUN dotnet build "Fluxys.Puma.IdentityProvider.STS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fluxys.Puma.IdentityProvider.STS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "Fluxys.Puma.IdentityProvider.STS.Identity.dll"]