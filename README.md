# Multilingual Journal Web App

A .NET Core web application for creating and managing multilingual journal entries with AI-powered translation capabilities.

## Features

- **Journal Management**: Create, read, update, and delete journal entries
- **Multilingual Support**: Write entries in multiple languages (English, Spanish, French, German, Italian, Portuguese)
- **AI Translation**: Automatic translation of entries to different languages via microservice
- **Tag System**: Organize entries with regular tags and milestone tags
- **Search Functionality**: Search through journal entries
- **Modern UI**: Responsive web interface built with Bootstrap

## Architecture

- **Main Application**: ASP.NET Core Web API with Entity Framework Core
- **Translation Service**: Separate microservice for AI translation (mock implementation included)
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: HTML/CSS/JavaScript with Bootstrap
- **Containerization**: Docker containers for all services

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 8.0 SDK (for local development)

### Running with Docker

1. Clone the repository
2. Run the application:
   ```bash
   docker-compose up --build
   ```

3. Access the application:
   - Main App: http://localhost:5000
   - Translation Service: http://localhost:5001
   - Swagger UI: http://localhost:5000/swagger

### Database Migrations

The application will automatically create the database on first run. For manual migrations:

```bash
cd MultilingualJournal
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## API Endpoints

### Journal Entries
- `GET /api/journalentry` - Get all entries
- `GET /api/journalentry/{id}` - Get specific entry
- `POST /api/journalentry` - Create new entry
- `PUT /api/journalentry/{id}` - Update entry
- `DELETE /api/journalentry/{id}` - Delete entry
- `GET /api/journalentry/search` - Search entries

### Tags
- `GET /api/tag` - Get all tags
- `GET /api/tag/milestones` - Get milestone tags only
- `POST /api/tag` - Create new tag

### Translations
- `POST /api/translation/translate-entry/{id}` - Translate journal entry
- `GET /api/translation/entry/{id}/translations` - Get entry translations

## Data Models

### JournalEntry
- `Id`: Unique identifier
- `Title`: Entry title (max 200 chars)
- `Content`: Entry content
- `Language`: Language code (max 10 chars)
- `CreatedAt`: Creation timestamp
- `UpdatedAt`: Last update timestamp
- `Tags`: Associated tags
- `Translations`: Available translations

### Tag
- `Id`: Unique identifier
- `Name`: Tag name (max 50 chars, unique)
- `Color`: Hex color code
- `IsMilestone`: Milestone tag flag
- `CreatedAt`: Creation timestamp

### Translation
- `Id`: Unique identifier
- `JournalEntryId`: Reference to journal entry
- `TargetLanguage`: Target language code
- `TranslatedTitle`: Translated title
- `TranslatedContent`: Translated content
- `CreatedAt`: Translation timestamp

## Translation Service

The translation service is a separate microservice that provides:
- Text translation between supported languages
- Batch translation support
- Language detection (planned)

Currently implements a mock translator for demonstration. Can be extended with real AI translation services.

## Development

### Local Development

1. Start SQL Server:
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
   ```

2. Run the main application:
   ```bash
   cd MultilingualJournal
   dotnet run
   ```

3. Run the translation service:
   ```bash
   cd TranslationService
   dotnet run --urls="http://localhost:5001"
   ```

### Adding New Languages

1. Update the language dropdown in `wwwroot/index.html`
2. Add language mappings in `TranslationService/Services/MockTranslationService.cs`
3. Update the frontend language indicator styling if needed

## Docker Configuration

- **SQL Server**: Standard Microsoft SQL Server 2022 container
- **Main App**: Multi-stage build with ASP.NET Core runtime
- **Translation Service**: Multi-stage build with ASP.NET Core runtime
- **Networking**: Services communicate via Docker internal network

## License

This project is licensed under the MIT License.