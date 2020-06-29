#!/bin/bash
PROJECT_NAME=Hangman
CSPROJ_PATH=./$PROJECT_NAME/$PROJECT_NAME.csproj

echo "[MIGRATION]: Running migrations..."
dotnet ef database update --project $CSPROJ_PATH  # reads ASPNETCORE_ENVIRONMENT