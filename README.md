# Tournament Tracker Web API

## Introduction

Welcome to the Tournament Tracker Web API documentation! This API is a comprehensive solution for managing tournaments efficiently. Developed in C# using ASP.NET Web API with a robust class project handling models and data context, this application caters to three distinct user roles: players, coaches, and organizers.

## Features

1. **User Signup and Authentication**: Enable users to sign up with different roles, including players, coaches, and tournament organizers.

2. **Player-Team Interaction**: Players can find and join teams, fostering collaboration and team building.

3. **Coach Booking System**: Coaches have the ability to book tournaments for their teams, streamlining the process of team participation.

4. **Tournament Organizer Management**: Tournament organizers can create, update, and manage tournaments efficiently.

## Technologies Used

- **.NET 8.0**
  - Used for building robust and high-performance web APIs.
  - Provides a powerful framework for developing modern, scalable, and cross-platform applications.

- **Entity Framework (ORM)**
  - Enables seamless integration with the database, ensuring efficient data management.
  - Utilizes object-relational mapping (ORM) to simplify and streamline database interactions.

- **Microsoft SQL Server**
  - Chosen as the Database Management System for its simplicity and efficiency.
  - Offers robust data storage capabilities, ensuring reliability and optimal performance.

These technologies collectively contribute to the development of a secure, scalable, and feature-rich Tournament Tracker Web API. The use of .NET 8.0 provides a solid foundation for building modern web APIs, while Entity Framework simplifies database interactions, and Microsoft SQL Server ensures efficient data management.

## User Hierarchy

All users inherit from the application 'User Model,' which inherits from the `IdentityUser` class, creating a table-per-hierarchy structure for the user entities. This hierarchy includes the following user roles:

1. **Player**
   - Properties: player status indicating if they are looking for a team or in one, UserId (Inherited), Teams, etc.

2. **Coach**
   - Properties: UserId (Inherited), Teams, Experience, Specialization, etc.

3. **Tournament Organizer**
   - Properties: UserId (Inherited), CreatedTournaments, Organization or Office they work with, etc.

## Running the Application

### Prerequisites

1. Install [.NET SDK](https://dotnet.microsoft.com/download) on your machine.
2. Install Microsoft SQL Server.

### Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/WinnieNgina/Tournaments.git
   ```

2. **Navigate to the Project Directory:**
   ```bash
   cd Tournaments
   ```

3. **Restore Packages:**
   ```bash
   dotnet restore
   ```

4. Update the database connection string in the `appsettings.json` file:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=DESKTOP-MKT43G1;Initial Catalog=Tournament;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
    }
    ```

5. **Apply Migrations:**
   ```bash
   dotnet ef database update
   ```

6. **Run the Application:**
   ```bash
   dotnet run
   ```

7. **Access the Application:**
   Open your browser and navigate to [https://localhost:5001](https://localhost:5001) to explore the Tournament Tracker Web API.

## Models

### Tournament
- Properties: TournamentId, Name, Date, Location, etc.

### Team
- Properties: TeamId, Name, Players, Captain, etc.

### MatchUp
- Properties: MatchId, TournamentId, Teams, Winner, Loser, Date, etc.

## Detailed Resources

- **SQL Optimization**: Refer to [SQL Optimization](docs/sql-optimization.md) for detailed insights into the employed techniques.
- **Sending and Receiving Emails in C#**: Learn with the step-by-step guide provided in [Sending and Receiving emails in C#](docs/sending-receiving-emails.md).
- **Entity Framework Best Practices**: Explore the best practices in [Entity Framework Best Practices](docs/entity-framework-best-practices.md).
- **In-Memory Caching in .NET Core**: Understand in-memory caching in .NET Core through [In-Memory Caching in .NET Core](docs/in-memory-caching.md) and [ASP.NET Core In-Memory Caching](docs/aspnet-core-caching.md).
- **Boosting API Performance and Scalability**: Enhance your API's performance and scalability with best practices outlined in [Boosting API Performance and Scalability](docs/api-performance-scalability.md).
- **Two-Factor Authentication**: Implement two-factor authentication using email and SMS services by following the guide [here](https://learn.microsoft.com/en-us/aspnet/identity/overview/features-api/two-factor-authentication-using-sms-and-email-with-aspnet-identity).
- **Africa's Talking README for Implementing SMS Service**: For integrating Africa's Talking SMS service, please refer to the provided [Africa's Talking README](docs/africas-talking-sms-service.md).

Feel free to contribute to the Tournament Tracker Web API by opening issues or creating pull requests.

If you have any questions or encounter issues, please feel free to reach out. Happy coding!