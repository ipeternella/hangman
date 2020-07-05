#!/bin/bash
PROJECT_NAME=Hangman
CSPROJ_PATH=./$PROJECT_NAME/$PROJECT_NAME.csproj

echo "[PRE-RUN]: Building project..."
dotnet build $CSPROJ_PATH

echo "[PRE-RUN]: Watching and running project..."
dotnet watch --project $CSPROJ_PATH run --urls https://$ASPNETCORE_SOCKET_BIND_ADDRESS:$ASPNETCORE_SOCKET_BIND_PORT --launch-profile=DockerCompose