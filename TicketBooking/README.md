# Airport Ticket Booking System

## Overview
This is a .NET console application for managing airport ticket bookings, designed using Domain-Driven Design (DDD) principles. The system allows passengers to search and book flights, and managers to manage bookings and import flight data via CSV files. Data is stored in the file system for simplicity.

## Features

### Passenger
- **Book a Flight:**
  - Search flights by price, departure/destination country, date, airport, and class (Economy, Business, First Class).
  - Select flight class with dynamic pricing.
- **Manage Bookings:**
  - Cancel, modify, and view personal bookings.

### Manager
- **Filter Bookings:**
  - Filter by flight, price, country, date, airport, passenger, and class.
- **Batch Flight Upload:**
  - Import flights from CSV files.
- **Validate Imported Data:**
  - Model-level validation with detailed error reporting.
- **Dynamic Validation Details:**
  - View constraints for each flight data field.

## Project Structure

```
TicketBooking/
├── Domain/
│   ├── Models/
│   ├── Enums/
│   ├── Repositories/
│   ├── Services/
│   └── Validations/
├── Infrastructure/
│   ├── Dtos/
│   ├── Mappers/
│   ├── Repositories/
│   └── Utils/
├── Presentation/
│   ├── DTOs/
│   ├── Mappers/
│   ├── AdminMenu.cs
│   ├── Menu.cs
│   ├── Test.cs
│   └── UserMenu.cs
├── csv/
│   └── flight_data.csv
├── Program.cs
├── TicketBooking.csproj
└── README.md
```

## Getting Started

1. **Clone the repository**
2. **Build the project**
   - Use your preferred IDE (Rider, Visual Studio, VSCode) or run `dotnet build` in the terminal.
3. **Run the application**
   - Use `dotnet run` or execute the built `.exe` file.
   - Ensure the CSV file path is correct for your environment:
     - Console: `./csv/flight_data.csv`
     - Rider: `../../../csv/flight_data.csv`

## Usage
- Follow the console menus to:
  - Search and book flights
  - Manage bookings
  - Import flights (manager)
  - View and filter bookings

## Design Principles
- **Domain-Driven Design (DDD):**
  - Clear separation of domain, infrastructure, and presentation layers.
- **SOLID Principles:**
  - Decoupled via interfaces, dependency injection used for services and repositories.
- **Validation:**
  - Validators for domain models (Flight, User) ensure data integrity.
- **Mapping:**
  - Mappers convert between DTOs and domain models.

## Dependencies
- [CsvHelper](https://joshclose.github.io/CsvHelper/) for CSV file operations.
