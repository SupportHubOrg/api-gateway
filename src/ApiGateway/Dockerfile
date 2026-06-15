#Stage 1: Build and Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

#Restore dependencies
COPY ["ApiGateway.csproj", "."]
RUN dotnet restore "./ApiGateway.csproj"

#Copy the remaining source code and build
COPY . .
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

#Stage 2: Final Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

#Standard port for .NET applications running in containers
EXPOSE 8080 

#Switch to the restricted user for security
USER $APP_UID

ENTRYPOINT ["dotnet", "ApiGateway.dll"]