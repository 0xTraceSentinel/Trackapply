# Trackapply
`Trackapply` is a job application tracker. Ever applied to 10+ jobs concurrently, forgetting about where you applied, what you applied for and awaiting feedback? This job tracker would keep track of these for you.

## Status
Still under active development...

## Tech stack
- **Backend:** ASP.NET Core (.NET 10), EF Core, PostgreSQL, structured as Clean Architecture (`Api` → `Application` → `Domain`, with `Infrastructure` implementing persistence).
- **Frontend:** Angular 22 (standalone components, signals, zoneless), Vitest.

## Prerequisites
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js 24+](https://nodejs.org/) and npm
- PostgreSQL 14+ (installed locally, hosted, or via the included `docker-compose.yml`)

## Getting started

### 1. Database
Create an empty database (the migrations create the tables):
```sql
CREATE DATABASE trackapply;
```
Or, if you'd rather use Docker, copy `.env.example` to `.env`, set `POSTGRES_PASSWORD`, then run `docker compose up -d`. This starts Postgres 17 on `localhost:5432` with the `trackapply` database already created. `.env` is git-ignored.

### 2. Connection string
The connection string isn't stored in the repo with a password. Set it once per machine with [user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):
```bash
cd src/Backend
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=trackapply;Username=<user>;Password=<password>" --project Trackapply.Api
```
In other environments, provide it as the `ConnectionStrings__DefaultConnection` environment variable.

### 3. Backend
```bash
cd src/Backend
dotnet tool restore
dotnet ef database update --project Trackapply.Infrastructure --startup-project Trackapply.Api
dotnet run --project Trackapply.Api --launch-profile http
```
The API listens on `http://localhost:5099`. `GET /health` reports whether the API can reach Postgres. The OpenAPI document is at `/openapi/v1.json` in Development, and `Trackapply.Api/Trackapply.Api.http` has sample requests.

#### Database conventions
- Tables and columns use `snake_case` (e.g. `job_applications.company_url`), so you never need to quote identifiers in `psql`.
- Primary keys are time-ordered UUIDv7 values in `uuid` columns. Timestamps are UTC `timestamptz`.
- `status` is stored as text and enforced by a `CHECK` constraint that's generated from the C# enum.

#### Migrations
```bash
# Add a migration after changing the model
dotnet ef migrations add <Name> --project Trackapply.Infrastructure --startup-project Trackapply.Api --output-dir Persistence/Migrations

# Generate an idempotent SQL script (for reviewing or applying in production)
dotnet ef migrations script --idempotent --project Trackapply.Infrastructure --startup-project Trackapply.Api -o migrate.sql
```

### 4. Frontend
```bash
cd src/Frontend
npm install
npm start
```
Open `http://localhost:4200`. The dev build points at the API through `src/environments/environment.development.ts`.

| Command | Description |
| --- | --- |
| `npm start` | Dev server with live reload |
| `npm run build` | Production build |
| `npm test` | Unit tests (Vitest) |
| `npm run format` | Format sources with Prettier |

## API
| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/job-applications` | List all applications (newest first) |
| `GET` | `/api/job-applications/{id}` | Get one application |
| `POST` | `/api/job-applications` | Create an application |
| `PUT` | `/api/job-applications/{id}` | Update an application's details |
| `PATCH` | `/api/job-applications/{id}/status` | Change an application's status |
| `DELETE` | `/api/job-applications/{id}` | Delete an application |

Statuses: `Bookmarked`, `Applied`, `Interviewing`, `Offered`, `Rejected`. Errors are returned as RFC 7807 `ProblemDetails`.
