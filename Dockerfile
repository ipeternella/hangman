# Main variables required for the Docker layers
ARG MAIN_PROJECT_NAME=Hangman
ARG DOTNETCORE_VERSION=3.1

# Starting layer point using build image with dotnet SDK (very heavy image ~ 2GB)
FROM mcr.microsoft.com/dotnet/core/sdk:$DOTNETCORE_VERSION AS build-env
ARG MAIN_PROJECT_NAME
ARG DOTNETCORE_VERSION

WORKDIR /app

# Restores (downloads) all NuGet packages from all projects of the solution (Test is ignored)
COPY . ./
RUN dotnet restore ./$MAIN_PROJECT_NAME/$MAIN_PROJECT_NAME.csproj

# CD to the main project as dotnet 2.x publish requires this to send compiled files to the out folder
# PublishSingleFile and PublishTrimmed are ignored by the 2.x compiler (only available on 3.x)
WORKDIR ./$MAIN_PROJECT_NAME
RUN dotnet publish --runtime alpine-x64 --configuration Release --output out \
    -p:PublishSingleFile=true -p:PublishTrimmed=true

# Final layer based on Alpine Linux (ultra light-weight ~ 5MB)
FROM alpine:3.9.4 AS runtime-env
ARG MAIN_PROJECT_NAME
ARG DOTNETCORE_VERSION

# Installing some libraries required by .NET Core on Alpine Linux
RUN apk add --no-cache libstdc++ libintl icu

# Copies from the build environment the compiled files of the out folder
WORKDIR /app
COPY --from=build-env /app/$MAIN_PROJECT_NAME/out .