FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY api/api.csproj api/
RUN dotnet restore "api/api.csproj"
COPY . .
RUN dotnet build "api/api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api/api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "api.dll"]
