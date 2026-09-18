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

1. Clone the repository.
2. Open `AVMLabs.slnx` in Visual Studio.
3. Verify the SQL Server connection string in
   `AVMLabs.Api/appsettings.json`.
4. Build the solution.

EF Core migrations are included in the repository and pending migrations
are automatically applied when the API starts.

The migrations are available under:

``` text
AVMLabs.Api/Migrations
```

## Run

Run both `AVMLabs.Api` and `AVMLabs.Mvc` projects together.

In Visual Studio:

1.  click Debug the solution and select **Startup Project/Profile**.
2.  Select **Multiple startup projects**.
3.  Set both projects to **Start**:
    - `AVMLabs.Api`
    - `AVMLabs.Mvc`
4.  Apply the settings and run the solution.

The API automatically:

1.  Applies pending EF Core migrations.
2.  Runs the seed process.
3.  Inserts the required sample data.

No manual seed script execution is required.

The application flow is:

```text
AVMLabs.Mvc
    ↓
AVMLabs.Api
    ↓
SQL Server
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

### Assumptions

-   Payment Gateway Fee: The assessment requires the gateway fee to be
    recorded as a separate debit entry in the client ledger, but does not
    define whether the fee should be deducted from the payment amount.
    In this implementation, the full payment Amount is applied to the
    invoice, while GatewayFee is recorded separately as a debit.
    NetAmount represents Amount minus GatewayFee.

-   Work Order and Invoice: Creating a Work Order does not immediately
    create an invoice. An invoice is created when the Work Order reaches
    Billed status. Work Orders/items that remain InTransit are still
    included in the applicable outstanding/NBL calculation until an
    invoice exists for that Work Order. This avoids double-counting a
    Work Order after it has been invoiced.