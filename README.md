# AVMLabs Technical Assessment

Full Stack Developer Technical Assessment using .NET, SQL Server and
ASP.NET Core MVC.

## Tech Stack

-   .NET 10
-   ASP.NET Core Web API
-   ASP.NET Core MVC
-   Razor / CSHTML
-   Entity Framework Core
-   SQL Server
-   Bootstrap 5
-   JavaScript / Fetch API
-   xUnit
-   Moq
-   EF Core InMemory

## Solution Structure

``` text
AVMLabs
│
├── AVMLabs.Api
│   ├── Common
│   ├── Controllers
│   ├── Data
│   ├── DTOs
│   ├── Exceptions
│   ├── Logging
│   ├── Middleware
│   ├── Migrations
│   ├── Models
│   └── Services
│
├── AVMLabs.Api.Tests
│   └── NblServiceTests.cs
│
├── AVMLabs.Mvc
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Views
│   └── wwwroot
│
├── AVMLabsDB.sql
├── AVMLabs.slnx
├── README.md
└── .gitignore
```

### How the Structure Works

The application follows MVC → Web API → Database flow:

``` text
Browser
   │
   │ HTTP / Fetch
   ▼
AVMLabs.Mvc
   │
   │ API Requests
   ▼
AVMLabs.Api
   │
   ├── Controllers
   │       │
   │       ▼
   │    Services
   │       │
   │       ▼
   │    EF Core / AppDbContext
   │       │
   │       ▼
   │    SQL Server
   │
   └── Middleware
           │
           ▼
      Centralized Error Handling
```

### Folder Responsibilities

#### AVMLabs.Mvc

The presentation layer.

``` text
Controllers
    ↓
Receives browser requests and prepares MVC views

Models
    ↓
Contains MVC ViewModels / request models used by the UI

Services
    ↓
Communicates with AVMLabs.Api through HttpClient

Views
    ↓
Razor / CSHTML pages rendered in the browser

wwwroot
    ↓
CSS, JavaScript and client-side libraries
```

#### AVMLabs.Api

The backend API and business logic layer.

``` text
Controllers
    ↓
Receives API requests and returns API responses

DTOs
    ↓
Request / response models exchanged through the API

Services
    ↓
Contains application business logic

Data
    ↓
AppDbContext and database configuration

Models
    ↓
Entity Framework Core database entities

Migrations
    ↓
Database schema changes

Middleware
    ↓
Centralized exception handling

Logging
    ↓
Application error logging
```

#### AVMLabs.Api.Tests

Contains automated unit tests for API business logic.

``` text
NblServiceTests
    ↓
NBL business-rule tests
    ↓
xUnit + Moq + EF Core InMemory
```

## Setup

### Prerequisites

-   Visual Studio 2022 or later
-   .NET 10 SDK
-   SQL Server
-   SQL Server Management Studio (optional)

### Steps

1.  Clone the repository.
2.  Open `AVMLabs.slnx` in Visual Studio.
3.  Verify the SQL Server connection string in
    `AVMLabs.Api/appsettings.json`.
4.  Open Visual Studio Package Manager Console.
5.  Select `AVMLabs.Api` as the Default Project.
6.  Apply the existing EF Core migrations:

``` powershell
Update-Database
```

The migrations are available under:

``` text
AVMLabs.Api/Migrations
```

## Run

Run the projects locally from Visual Studio.

The API automatically:

1.  Applies pending EF Core migrations.
2.  Runs the seed process.
3.  Inserts the required sample data.

No manual seed script execution is required.

Run:

``` text
AVMLabs.Api
    ↓
AVMLabs.Mvc
```

The MVC application communicates with the API using the configured API
base URL in:

``` text
AVMLabs.Mvc/appsettings.json
```

## SQL File

The repository contains:

``` text
AVMLabsDB.sql
```

The file contains:

-   Database schema
-   Table definitions
-   Relationships
-   Seed/sample data
-   SQL Assessment Task 1
-   SQL Assessment Task 2
-   SQL Assessment Task 3
-   SQL Assessment Task 4
-   SQL Assessment Task 5

**Note:** Do not execute the entire `AVMLabsDB.sql` file as a single
script. It contains the schema, data and individual SQL assessment task
queries for reference/submission.

## Testing

The test project is:

``` text
AVMLabs.Api.Tests
```

Run from Visual Studio Test Explorer or using:

``` powershell
dotnet test
```

Current NBL unit test coverage includes:

-   No outstanding amount → NBL
-   Pending invoice outstanding → Non-NBL
-   InTransit outstanding → Non-NBL

## Notes

-   SQL Server is required to run the application.
-   EF Core migrations are included in the repository.
-   Seed data is automatically created when the API starts.
-   The MVC project communicates with the Web API rather than accessing
    the database directly.
-   API business logic is handled in the service layer.
-   API errors are handled through centralized exception middleware.
-   DTOs/ViewModels are used between application layers instead of
    exposing database entities directly.
