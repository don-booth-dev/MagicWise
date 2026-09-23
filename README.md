# MagicWise
A Windows WPF desktop application for planning your next theme park adventure.

![License](https://img.shields.io/badge/License-PolyForm%20Noncommercial%201.0.0-blue?style=flat)
![Last Commit](https://img.shields.io/github/last-commit/don-booth-dev/MagicWise)
![Repo Size](https://img.shields.io/github/repo-size/don-booth-dev/MagicWise)
![Top Language](https://img.shields.io/github/languages/top/don-booth-dev/MagicWise)
![Issues](https://img.shields.io/github/issues/don-booth-dev/MagicWise)
[![Tests](https://github.com/don-booth-dev/MagicWise/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/don-booth-dev/MagicWise/actions/workflows/dotnet-desktop.yml)


## About
MagicWise is a Windows desktop application for planning theme park adventures. It
brings park and resort information, interactive maps, schedules, live data, and
filtering tools together in one application.

> MagicWise is currently under active development. Some features and behaviour may be incomplete.

## Features
- Browse theme park destinations, resorts, and parks
- View park details and interactive maps
- Filter map and resort information
- View park schedules and live data
- Organize plans using schedule entries
- Retrieve theme park data through an integration API

## Technology
- C#
- .NET 10
- WPF
- Entity Framework Core
- SQLite
- Mapsui
- CommunityToolkit.Mvvm

## Getting Started
### Requirements
- Windows
- .NET 10 SDK

### Run the application
```powershell
dotnet run --project MagicWise.Desktop\MagicWise.Desktop.csproj
```

### Run the tests
```powershell
dotnet test MagicWise.slnx
```

## Project Structure
- `MagicWise.Core` - Domain models and application interfaces
- `MagicWise.Data` - Data access and persistence
- `MagicWise.Desktop` - WPF desktop application and views
- `MagicWise.Integrations` - Theme park API integration and data transfer objects
- `tests` - Unit, integration, desktop, and end-to-end tests
- `docs` - Project documentation and API definitions

## License
MagicWise is licensed under the
[PolyForm Noncommercial License 1.0.0](LICENSE.md).
