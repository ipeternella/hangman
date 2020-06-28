#!/bin/bash
echo "[PRE-RUN]: Creating migrations..."
dotnet ef migrations add initial
dotnet ef database update

echo "[PRE-RUN]: Building project..."
dotnet build

echo "[PRE-RUN]: Watching and running project..."
dotnet watch run