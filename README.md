# OMNE Project

## Overview

OMNE is a full-stack .NET application with a Blazor WebAssembly frontend, FastEndpoints API backend, and PostgreSQL database. The project uses Aspire for service orchestration and follows a monorepo structure managed with pnpm workspaces.

## Prerequisites

Before you can run the OMNE project, ensure you have the following installed:

1. **.NET SDK >=9.0.304** as defined in `global.json`
2. **Node.js >=20+** - as defined in `packages.json`
3. **pnpm >=10** - for managing the monorepo structure
4. **Docker** - must be running before starting the application

### Installing Node.js

For Windows users, the fastest way to install Node.js and pnpm is using winget (Windows Package Manager):

1. Open PowerShell or Command Prompt
2. Install Node.js:
   ```bash
   winget install OpenJS.NodeJS
   ```
3. Install pnpm:

   ```bash
   winget install pnpm
   ```

4. Verify the installations:
   ```bash
   node --version
   pnpm --version
   ```

## Running the Project

To run the OMNE project, follow these steps:

1. **Ensure Docker is running**, Aspire requires Docker to be running to orchestrate services but does not start it automatically

2. **Restore .NET tools:**

   ```bash
   dotnet tool restore
   ```

3. **Build the project** , this will automatically install pnpm dependencies:

   ```bash
   dotnet build
   ```

4. **Run the application:**
   ```bash
   dotnet aspire run
   ```

This will start the Aspire host which orchestrates:

- PostgreSQL database container
- API service (FastEndpoints)
- WebAssembly frontend (Blazor)

## Key Technologies

The OMNE project leverages several modern technologies:

### Backend

- **.NET 9** - core framework
- **FastEndpoints** - API framework for building REST APIs
- **Entity Framework Core** - SQL schema management
- **Dapper** - Micro-ORM for high-performance data access
- **PostgreSQL** - Primary database
- **Aspire** - Distributed application hosting and service orchestration
- **Npgsql** - PostgreSQL database provider for .NET

### Frontend

- **Blazor WebAssembly** - client-side web UI framework
- **MudBlazor** - UI component library for Blazor
- **RestSharp** - HTTP client for API communication

### Development & Tooling

- **pnpm** - fast, disk space efficient package manager
- **Prettier** - code formatter for multiple languages
- **Analyzers** - C# code style enforcement
- **ESLint** - JS/TS linting\
- **Husky** - Git hooks for code quality enforcement
- **Central Package Management (CPM)**

## Project Structure

```
├── apps
│   ├── OMNE.Api             # FastEndpoints API backend
│   ├── OMNE.Host            # Aspire host for service orchestration
│   └── OMNE.Web             # Blazor WebAssembly frontend
├── src
│   ├── OMNE.Data            # Data models and database context
│   ├── OMNE.EFCore          # Entity Framework Core extensions
│   ├── OMNE.Model           # Shared models
│   ├── OMNE.Postgres        # PostgreSQL-specific implementations
│   ├── OMNE.ServiceDefaults # Common service configurations
│   └── OMNE.Workspace       # Automatic dependency installation and git hooks
└── test
    └── OMNE.Api.Tests       # API integration tests
```

## Workspace Project

The `OMNE.Workspace` project is a special project that handles automatic dependency installations:

1. when any project is built, it ensures that all Node.js dependencies are installed by running `pnpm install` if needed
2. automatically installs and configures Husky git hooks for code quality enforcement

This means you don't need to manually run `pnpm install` in most cases, as it will be handled during the build process.

## Development Scripts

The project includes several npm scripts for development tasks:

- `pnpm format` - format all code files
- `pnpm format:dotnet` - format C# code files only
- `pnpm format:prettier` - format JS/TS/XML/etc files only
