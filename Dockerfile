FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
ARG PROJECT_NAME=Hangman

# Copy csproj and restore as distinct layers
COPY ./$PROJECT_NAME/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./$PROJECT_NAME ./
# WORKDIR "/app/${PROJECT_NAME}"
# app/out
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime-env
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "$PROJECT_NAME.dll"]