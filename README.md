# MyBookmarks

MyBookmarks is a personal bookmark manager built for the ASP.NET Fundamentals course. Users can save useful links, organize them into categories and collections, and find them through search, filters and sorting.

## Features

- Account registration, login, logout and persistent sign-in through ASP.NET Core Identity.
- Create, list, edit and delete bookmarks, categories and collections.
- Optional bookmark descriptions and categories.
- Mark bookmarks as favorites using a heart button.
- Search bookmarks by title and filter by category or favorites.
- Sort bookmarks by title in either direction, newest first or oldest first.
- Paginated lists, with bookmark search, filters and sorting preserved across page changes and favorite updates.
- Add a bookmark to multiple collections and remove it without deleting the bookmark.
- User-specific data: operations check ownership before accessing another user's resources.
- Server-side and client-side form validation, with service checks for duplicate names, titles and URLs.
- Responsive Bootstrap interface and sample data for local evaluation.

A category classifies a bookmark; each bookmark can have one category or no category. Collections group bookmarks for a purpose, and the same bookmark can belong to multiple collections.

## Technologies

| Technology | Version | Purpose |
| --- | --- | --- |
| .NET | 10.0 (`net10.0`) | Target framework |
| ASP.NET Core MVC and Razor | 10.0 | Web framework and server-rendered pages |
| Entity Framework Core | 10.0.12 | Database access and migrations |
| ASP.NET Core Identity EF stores and UI | 10.0.12 | Accounts, password hashing and cookie authentication |
| Microsoft SQL Server | Not pinned by the project | Application and Identity data |
| SB Admin | 7.0.7 | Application layout and theme |
| | Bootstrap | CSS 5.2.3 / JS 5.3.3 | Responsive interface and interactive components |
| jQuery | 3.7.1 | Client-side JavaScript |
| jQuery Validation / Unobtrusive Validation | 1.21.0 / 4.0.0 | Client-side form validation | |

SQL Server is installed separately; its server version is not defined by the EF Core package version.

## Project structure

```text
MyBookmarks.slnx
MyBookmarks/              MVC controllers, Razor views, ViewModels, Identity pages and static assets
MyBookmarks.Data/         Entities, DbContext, EF configurations, migrations, repositories and demo seeding
MyBookmarks.Services/     Business operations, service contracts, DTOs, result enums and URL normalization
MyBookmarks.Common/       Shared validation limits, pagination settings and sorting options
```

Controllers handle HTTP requests, call services and map service DTOs to ViewModels. Services implement application rules and ownership checks, while repositories handle database queries and persistence through Entity Framework Core.

Entity models represent the stored data and relationships. DTOs carry data into and out of the service layer. ViewModels provide the data and input validation rules needed by each view, while Razor Views render the interface.

The DbContext and entity configurations define database mappings and constraints, and migrations track schema changes. The Common project holds shared constants and sorting options. ASP.NET Core Identity manages accounts and authentication, while the demo seeder provides sample data for local testing.

XML documentation describes the contracts and important behavior.

## Prerequisites

- **.NET 10 SDK** to build and run the application.
- A running **SQL Server instance** and an account with permission to create the database and apply migrations.
- **Git**, if cloning the repository instead of downloading it as a ZIP archive.

Visual Studio or another IDE is optional. The setup instructions use the .NET CLI.

## Run locally

Run the following commands from the repository root, which contains `MyBookmarks.slnx`.

### 1. Clone the repository

```powershell
git clone https://github.com/bogdan-s-stoyanov/my-bookmarks.git
cd my-bookmarks
```

### 2. Check the database configuration

The committed connection string in `MyBookmarks/appsettings.json` is:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyBookmarks;Trusted_Connection=True;Encrypt=False;"
  }
}
```

- `Server=.` selects the local SQL Server default instance.
- `Database=MyBookmarks` is the database created by the migrations if it does not exist.
- `Trusted_Connection=True` uses the Windows account running the application.
- `Encrypt=False` is the setting provided for local development.

If that instance is installed and accessible, no configuration change is needed. For a different instance, change `DefaultConnection` in that file or use one of the configuration overrides described below. For example, the server name for SQL Server Express is usually `.\SQLEXPRESS`; LocalDB commonly uses `(localdb)\MSSQLLocalDB`. Backslashes must be escaped as `\\` inside JSON strings.

### 3. Restore and build

```powershell
dotnet restore MyBookmarks.slnx
dotnet build MyBookmarks.slnx --no-restore
```

### 4. Apply the existing migrations

Install the EF Core CLI tool if it is not already available:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
```

Check its version with `dotnet ef --version`. If an older global version is installed, update it:

```powershell
dotnet tool update --global dotnet-ef --version 10.0.12
```

Apply the migrations:

```powershell
dotnet ef database update --project MyBookmarks.Data/MyBookmarks.Data.csproj --startup-project MyBookmarks/MyBookmarks.csproj --context MyBookmarksDbContext -- --environment Development
```

The data project contains the migrations, and the MVC project supplies the startup configuration. Use the migrations already included in the repository; creating an `InitialCreate` migration is unnecessary.

Migrations must be applied before starting the application. Startup does not automatically apply them.

### 5. Start the application

Trust the local HTTPS development certificate if necessary:

```powershell
dotnet dev-certs https --trust
```

Run the HTTPS launch profile:

```powershell
dotnet run --project MyBookmarks/MyBookmarks.csproj --launch-profile https
```

Open **https://localhost:7289**. The profile also binds **http://localhost:5014**, and the application redirects HTTP requests to HTTPS.

The launch profile sets `ASPNETCORE_ENVIRONMENT=Development`, which enables demo seeding. Check the console output for the active environment and listening addresses if you use custom launch settings.

## Demo account and sample data

After migrations are applied, the first start in **Development** creates:

| Field | Value |
| --- | --- |
| Email | `demo@mybookmarks.test` |
| Password | `Demo123!` |

The demo account receives:

- **3 categories:** Development, Learning and Tools.
- **15 bookmarks:** a mix of favorites, categorized and uncategorized links, optional descriptions and different creation dates.
- **2 collections:** Web development and Learning essentials. One bookmark is included in both.

The 15 bookmarks allow pagination to be exercised immediately.

A completion marker on the demo account prevents repeated seeding. Restarting the application does not duplicate the initial data or restore sample records that the evaluator edits or deletes. An existing demo account's password is not reset.

These are published local demonstration credentials. Demo seeding is not run outside Development.

## Configuration overrides

No private API keys or email service credentials are required for the local demo. Account sign-in does not require email confirmation; an email delivery service is not configured by this project.

User Secrets belong to the local developer account and are not included in the repository. In Development, a connection string in User Secrets overrides the value in `appsettings.json`. Environment variables can override both.

To store an optional local connection string in User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLEXPRESS;Database=MyBookmarks;Trusted_Connection=True;Encrypt=False;" --project MyBookmarks/MyBookmarks.csproj
```

Alternatively, set it for the current PowerShell session before running the migration and startup commands:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=.\SQLEXPRESS;Database=MyBookmarks;Trusted_Connection=True;Encrypt=False;'
```

Use your actual SQL Server instance name. Do not commit private database credentials into application settings.

If changing `appsettings.json` appears to have no effect, check for a User Secrets or environment-variable override.

## Using the application

1. Sign in with the demo account or register a separate account.
2. Open **Bookmarks**, **Categories** or **Collections** from the sidebar.
3. Use **Add bookmark** to save an absolute HTTP or HTTPS URL, such as `https://example.com/`. A scheme is required; the application does not add one automatically.
4. Use the bookmark search and filter form, choose a sort order and press **Apply**. **Clear** resets the selection.
5. Click the heart to add or remove a favorite.
6. Open a collection and choose **Add bookmarks**. Already included bookmarks are marked accordingly.
7. **Remove from collection** removes only the membership; **Delete** on a bookmark deletes the saved bookmark itself.

Newly registered accounts start without sample categories, bookmarks or collections.

## Troubleshooting

- **SQL Server connection failure:** confirm that the instance is running, the server name is correct and Windows Authentication permits access.
- **Missing database tables:** apply the included migrations against the same connection string used when running the app.
- **Demo account is absent:** confirm that startup uses Development and that the migrations have been applied.
- **HTTPS certificate warning:** run `dotnet dev-certs https --trust`, then restart the application and browser if needed.
- **Port already in use:** stop the other running instance or update the local launch profile; use the address reported by the console.

## UI attribution

The interface uses [SB Admin by Start Bootstrap](https://startbootstrap.com/template/sb-admin), adapted for MyBookmarks, together with Bootstrap and jQuery validation libraries. Third-party license notices are retained with their assets.
