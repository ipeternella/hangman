# Hangman

A simplistic Hangman gaming API written in `C#` with `dotnet` framework.

## Disclaimer

This project is a WIP. Therefore, even this `README.md` is far from being finished.

## Project construction

```bash
# solution creation
dotnet new sln -n Hangman 
dotnet sln list  # shows no projects in the solution

# projects creation
dotnet new webapi -n Hangman
dotnet new xunit -n Tests

 # projects wrapping inside the solution
dotnet sln add ./Hangman/Hangman.csproj  # adds project to the solution
dotnet sln add ./Tests/Tests.csproj  # adds project to the solution
```

## First Migrations

```bash
dotnet ef migrations add FirstMigration --project ./Hangman/Hangman.csproj --context HangmanDbContext
```