FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "DrinkVendingMachine.sln"
RUN dotnet publish "src/DrinkVendingMachine.Api/DrinkVendingMachine.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DrinkVendingMachine.Api.dll"]
