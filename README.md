# SkillSnap

SkillSnap is a full-stack web application for managing user portfolios, projects, and skills. Built with ASP.NET Core (API) and Blazor WebAssembly (client), it demonstrates robust authentication, efficient data management, and a modern, responsive user experience.

---

## Table of Contents

- [Features](#features)
- [Architecture Overview](#architecture-overview)
- [Development Challenges](#development-challenges)
- [Business Logic, Data Persistence, and State Management](#business-logic-data-persistence-and-state-management)
- [Security](#security)
- [Performance Optimizations](#performance-optimizations)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- User registration and login with JWT authentication
- Role-based access control (User/Admin)
- Portfolio management: create, update, and delete projects and skills
- Responsive Blazor WebAssembly client
- Secure API endpoints with ASP.NET Identity
- Entity Framework Core with SQLite (dev) and Azure SQL (prod-ready)
- In-memory caching and efficient state management

---

## Architecture Overview

SkillSnap is structured into three main layers:

### 1. Business Logic

- **Location:** API Controllers and Services in `SkillSnap.Api`
- **Design:** Controllers handle routing; services enforce validation, business rules, and entity orchestration using dependency injection.

### 2. Data Persistence

- **Technology:** Entity Framework Core (`SkillSnapContext`)
- **Models:** `PortfolioUser`, `Project`, and `Skill` with explicit [Key] and [ForeignKey] attributes for relationships.
- **Seeding:** Initial data seeded in `OnModelCreating` for development.
- **Storage:** SQLite for local development; Azure SQL for production.

### 3. State Management

- **Client Side:** Managed in `SkillSnap.Client` using Blazor’s dependency injection and scoped services.
- **Data Services:** Typed C# services handle API calls and cache data in memory.
- **Consistency:** State is refreshed after mutations; tokens are stored securely.

---

## Development Challenges

- **Authentication Complexity:** Integrating ASP.NET Identity with JWT and synchronizing with Blazor’s HttpClient required careful token handling and role checks.
- **EF Core Relationships:** Managing foreign keys, cascade behaviors, and reliable seeding for `PortfolioUser`, `Project`, and `Skill`.
- **State Management:** Ensuring data consistency in Blazor after CRUD operations, especially with caching.
- **Copilot Guidance Limits:** Required staged, context-rich prompts for meaningful code generation.
- **Deployment Nuances:** Adapting local SQLite setup for Azure, securing secrets, and configuring environment variables.

---

## Business Logic, Data Persistence, and State Management

- **Business Logic:** Encapsulated in API controllers and services, with clear separation of concerns and dependency injection.
- **Data Persistence:** EF Core models define relationships; migrations and seeding handled in `SkillSnap.Api/Data`.
- **State Management:** Blazor client uses scoped services for API calls and caching; state is refreshed after mutations; authentication state is managed via `ProtectedLocalStorage`.

---

## Security

SkillSnap employs multiple security layers:

1. **Authentication:** ASP.NET Identity with JWT; custom login/register endpoints issue tokens.
2. **Token Handling:** JWT stored in `ProtectedLocalStorage` and attached to API requests.
3. **Authorization:** Role-based access with `[Authorize]` attributes; backend enforces ownership and role checks.
4. **API Protection:** All data-modifying endpoints require authentication; public endpoints are read-only.
5. **Token Validation:** Configured `JwtBearerOptions` for issuer, audience, and signing key validation.
6. **UI Enforcement:** Blazor components check auth state and restrict UI actions.
7. **Secret Management:** Secrets stored in environment variables or `appsettings.Development.json` (excluded from version control).

---

## Performance Optimizations

- **Caching:** In-memory caching for frequently accessed endpoints (e.g., projects, skills).
- **Efficient Queries:** EF Core queries project only necessary fields and use `.AsNoTracking()` for reads.
- **Lazy Loading Avoidance:** Explicit loading to prevent hidden performance costs.
- **Client State Caching:** Blazor services cache data to reduce redundant HTTP requests.
- **Minimal API Responses:** Lean payloads for faster serialization/deserialization.
- **Error Logging:** API and client errors are logged; frontend fallback mechanisms prevent unnecessary reloads.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (for Blazor WASM if needed)
- SQLite (for local development)
- Azure account (for deployment)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd SkillSnap-App-New
   ```

2. **Set the JWT secret:**
   - In `SkillSnap.Api/appsettings.json`:
     ```json
     "Jwt": {
       "Secret": "your-very-strong-secret-key"
     }
     ```
   - Or as an environment variable:
     ```bash
     export Jwt__Secret=your-very-strong-secret-key
     ```

3. **Apply EF Core migrations (if needed):**
   ```bash
   dotnet ef database update --project SkillSnap.Api/SkillSnap.Api.csproj
   ```

---

## Running the Application

- **Run the API:**
  ```bash
  dotnet run --project SkillSnap.Api/SkillSnap.Api.csproj
  ```

- **Run the Blazor client:**
  ```bash
  dotnet run --project SkillSnap.Client/SkillSnap.Client.csproj
  ```

- The API will be available at `https://localhost:5001` (default).
- The Blazor client will be available at `https://localhost:5002` (default).

---

## Testing

- **API Integration Tests:** Located in `SkillSnap.Api/IntegrationTests`
- **Client Unit Tests:** Located in `SkillSnap.Client.Tests/Services`
- **Run all tests:**
  ```bash
  dotnet test
  ```

---

## Deployment

- **Azure Deployment:**
  - Set environment variables for secrets and connection strings.
  - Use Azure SQL for production database.
  - Update `appsettings.Production.json` as needed.
  - Ensure all secrets are secured and not checked into version control.

---

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

---

## License

This project is licensed under the MIT License.

---