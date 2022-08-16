FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
 WORKDIR /app
EXPOSE 80
EXPOSE 443
 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinanceApp/FinanceApp.Api.csproj", "FinanceApp/"]
RUN dotnet restore "FinanceApp/FinanceApp.Api.csproj"
COPY . .
WORKDIR "/src/FinanceApp"
RUN dotnet build "FinanceApp.Api.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "FinanceApp.Api.csproj" -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceApp.Api.dll"]