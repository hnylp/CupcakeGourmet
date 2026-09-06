FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/CupcakeGourmet.Web/CupcakeGourmet.Web.csproj src/CupcakeGourmet.Web/
RUN dotnet restore src/CupcakeGourmet.Web/CupcakeGourmet.Web.csproj

COPY src/CupcakeGourmet.Web/ src/CupcakeGourmet.Web/
RUN dotnet publish src/CupcakeGourmet.Web/CupcakeGourmet.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "CupcakeGourmet.Web.dll"]
