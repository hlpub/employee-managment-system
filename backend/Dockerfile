FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /work
COPY src/ .

FROM build AS publish
WORKDIR /work/Employees.Api
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Employees.Api.dll"]