FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/fluxysroot.crt", "/etc/ssl/certs/fluxysroot.crt"]
COPY ["src/TestDatabase/TestDatabase.csproj", "src/TestDatabase/"]
RUN dotnet restore "src/TestDatabase/TestDatabase.csproj"
COPY . .
WORKDIR "/src/src/TestDatabase"
RUN dotnet build "TestDatabase.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestDatabase.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "TestDatabase.dll"]