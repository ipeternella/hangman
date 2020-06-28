#!/bin/bash
CSPROJ_PATH=./Hangman/Hangman.csproj

echo "[PRE-RUN]: Creating migrations..."
dotnet ef migrations add initial --project $CSPROJ_PATH

echo "[PRE-RUN]: Running migrations..."
dotnet ef database update --project $CSPROJ_PATH

echo "[PRE-RUN]: Building project..."
dotnet build $CSPROJ_PATH

echo "[PRE-RUN]: Watching and running project..."
dotnet watch --project $CSPROJ_PATH run