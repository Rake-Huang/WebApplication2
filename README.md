# WebApplication2

A simple ASP.NET Core Web API for managing football matches.

## Features

- Create new matches
- Get match details
- Update match results
- Comprehensive unit tests

## API Endpoints

- `GET /api/matches/{id}` - Get match by ID
- `POST /api/matches` - Create a new match
- `PUT /api/matches/{id}/result` - Update match result

## Technologies Used

- ASP.NET Core 9.0
- In-memory data storage
- NUnit for testing
- Swagger/OpenAPI for documentation

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/Rake-Huang/WebApplication2.git
   cd WebApplication2
   ```

2. Run the application:
   ```bash
   dotnet run --project WebApplication2
   ```

3. Access the API documentation at: `https://localhost:7051/swagger`

## Running Tests

```bash
dotnet test
```

## Project Structure

- `WebApplication2/` - Main web API project
- `WebApplication2.Tests/` - Unit tests
- `Controllers/` - API controllers
- `Models/` - Data models
- `Services/` - Business logic
- `Repositories/` - Data access layer 