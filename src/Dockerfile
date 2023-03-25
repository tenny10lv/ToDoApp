FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy everything else and build app
COPY . ./app
WORKDIR /source/app

RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "ToDoAppApi.dll"]
