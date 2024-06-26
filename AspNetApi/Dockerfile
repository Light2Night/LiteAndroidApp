# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# copy csproj and restore as distinct layers
WORKDIR /src
COPY AspNetApi.sln ./
COPY Api/*.csproj ./Api/
COPY Model/*.csproj ./Model/
RUN dotnet restore

# copy everything else and build app
COPY . .

WORKDIR /src/Model
RUN dotnet publish -o /app

WORKDIR /src/Api
RUN dotnet publish -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# environments
ENV ASPNETCORE_ENVIRONMENT=Docker

# information
EXPOSE 8080

# run
ENTRYPOINT ["dotnet", "Api.dll"]