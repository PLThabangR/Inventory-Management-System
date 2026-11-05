# LogiTrack – Order Management System

LogiTrack is an ASP.NET Core Web API application designed to streamline inventory management and order processing across multiple fulfillment centers.

The project demonstrates the development of a secure, database-driven logistics management system using **C#, ASP.NET Core, Entity Framework Core, and ASP.NET Core Identity**.

It forms part of the LogiTrack capstone project series, focusing on building RESTful APIs, implementing authentication and authorization, managing relational data, and improving application performance.

## Features

- **Inventory Management:** Create, retrieve, and manage inventory items across warehouse locations.
- **Order Management:** Create and retrieve customer orders, manage associated inventory items, and generate order summaries.
- **Relational Database Integration:** Persistent data storage using Entity Framework Core and database migrations.
- **Authentication:** User registration and login using ASP.NET Core Identity.
- **JWT Authentication:** Secure API access using JSON Web Tokens.
- **Role-Based Authorization:** Restrict protected operations based on assigned user roles, including the `Manager` role.
- **API Security:** Protect endpoints using ASP.NET Core authorization middleware.
- **Performance Optimization:** Apply query optimization and caching techniques to improve API responsiveness.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Primary programming language |
| ASP.NET Core Web API | REST API development |
| Entity Framework Core | ORM and database access |
| ASP.NET Core Identity | User management |
| JWT | Token-based authentication |
| SQL Database | Relational data persistence |
| Swagger / OpenAPI | API documentation and testing |
| Microsoft Copilot | AI-assisted code review and optimization |

## Project Structure

```text
LogiTrack/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── InventoryController.cs
│   └── OrderController.cs
│
├── Data/
│   └── LogiTrackContext.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── InventoryItem.cs
│   └── Order.cs
│
├── Migrations/
│
├── Program.cs
├── appsettings.json
└── LogiTrack.csproj
```

## Implementation Overview

The project was developed through a series of activities covering the core components of an enterprise-style Web API.

### 1. Project Initialization

- Created an ASP.NET Core Web API project.
- Established the application structure.
- Configured application services and middleware.

### 2. Domain Modeling

Implemented the core application models.

**InventoryItem**
- `ItemId`
- `Name`
- `Quantity`
- `Location`
- `DisplayInfo()` helper method.

**Order**
- `OrderId`
- `CustomerName`
- `DatePlaced`
- Collection of associated inventory items.
- `AddItem()` method.
- `RemoveItem()` method.
- `GetOrderSummary()` method.

### 3. Database Integration

- Configured `LogiTrackContext`.
- Integrated Entity Framework Core.
- Extended `IdentityDbContext<ApplicationUser>`.
- Configured entity relationships and database mappings.
- Created and applied database migrations.

### 4. Authentication and Authorization

Integrated ASP.NET Core Identity and JWT authentication.

Implemented:
- User registration.
- User login.
- JWT token generation.
- Role-based authorization.
- Protected inventory and order endpoints.
- Manager-specific access restrictions.

### 5. Performance Optimization

Improved API performance through:
- In-memory caching for frequently accessed data.
- Database query optimization.
- Efficient data retrieval.
- Review and refinement of controller logic.

### 6. AI-Assisted Development

Used Microsoft Copilot to support:
- Code reviews.
- Refactoring suggestions.
- Performance analysis.
- Security configuration reviews.
- Improving maintainability and code quality.

## Getting Started

### Prerequisites

Ensure the following tools are installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- Entity Framework Core CLI tools.
- A configured relational database.

Install the EF Core CLI tools if they are not already installed:

```bash
dotnet tool install --global dotnet-ef
```

### Installation

**1. Clone the repository**

```bash
git clone <repository-url>
```

**2. Navigate to the project directory**

```bash
cd LogiTrack
```

**3. Restore dependencies**

```bash
dotnet restore
```

**4. Configure the database connection**

Update the connection string in `appsettings.json` or configure it through your development environment's user secrets.

**5. Apply database migrations**

```bash
dotnet ef database update
```

**6. Run the application**

```bash
dotnet run
```

Once running, access the Swagger interface if Swagger is enabled in the current environment.

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate a user and receive a JWT |

### Inventory Management

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/inventory` | Retrieve inventory items |
| POST | `/api/inventory` | Create an inventory item |
| GET | `/api/inventory/{id}` | Retrieve an inventory item by ID |
| DELETE | `/api/inventory/{id}` | Delete an inventory item |

### Order Management

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/orders` | Retrieve customer orders |
| POST | `/api/orders` | Create a new order |

## Authentication Workflow

LogiTrack uses JWT Bearer authentication to secure protected API endpoints.

1. **Register:** Send a `POST` request to `/api/auth/register` to create an account.
2. **Login:** Send a `POST` request to `/api/auth/login` with valid credentials.
3. **Receive Token:** The API generates and returns a JWT upon successful authentication.
4. **Authorize Requests:** Include the token in the HTTP Authorization header when accessing protected endpoints.

```http
Authorization: Bearer <your-jwt-token>
```

Endpoints protected by `[Authorize]` require a valid authentication token. Endpoints with role restrictions additionally require the appropriate assigned role.

## Learning Outcomes

Through this project, I have gained practical experience with:

- Building RESTful APIs using ASP.NET Core.
- Designing application domain models.
- Implementing relational database persistence with EF Core.
- Managing database schemas through migrations.
- Integrating ASP.NET Core Identity.
- Implementing JWT authentication and role-based authorization.
- Optimizing database queries and API performance.
- Applying caching strategies.
- Using AI-assisted development tools to review and improve code.

## Project Status

**Development:** Capstone project – ongoing.

LogiTrack is being developed incrementally, with each activity introducing additional backend development concepts and improvements.

---

**Author:** Thabang Rakgoropo

**Focus:** Backend Development | C# | ASP.NET Core | Entity Framework Core | REST APIs

**Project:** LogiTrack – Order Management System
