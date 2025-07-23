# Multilingual Journal Web App - Phase 3

A .NET Core Razor Pages application with internationalization (i18n) support.

## 🎯 Phase 3: Add i18n Support

This phase adds:
- .NET Core localization with .resx resource files
- Multi-language UI support (English, Spanish, French, German, Italian)
- Language switcher component
- Culture-based content localization
- Persistent language preferences with cookies

## 🚀 Features

- **📝 Journal Management**: Create, read, update, and delete personal journal entries
- **💾 SQLite Database**: Lightweight, file-based database with Entity Framework Core
- **🐳 Docker Support**: Fully containerized application with persistent data
- **🌍 Multilingual UI**: Switch interface language between 5 supported languages
- **🎨 Bootstrap UI**: Responsive web interface with modern design

## 🏗️ Architecture

- **Frontend**: ASP.NET Core Razor Pages with Bootstrap 5
- **Backend**: Entity Framework Core with SQLite
- **Localization**: .resx resource files with built-in .NET localization
- **Containerization**: Docker with volume mounting for database persistence
- **Database**: Persistent SQLite with Docker volumes

## 🌍 Language Support

The application supports the following languages:
- 🇺🇸 **English** (en) - Default
- 🇪🇸 **Spanish** (es) - Español  
- 🇫🇷 **French** (fr) - Français
- 🇩🇪 **German** (de) - Deutsch
- 🇮🇹 **Italian** (it) - Italiano

Switch languages using the globe dropdown in the navigation bar.

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Docker & Docker Compose

### Option 1: Run with Docker (Recommended)
```bash
# Build and run the containerized application
docker-compose up --build

# Access the application
open http://localhost:5000
```

### Option 2: Run Locally
```bash
# Navigate to the app directory
cd MultilingualJournalApp

# Restore packages and run
dotnet restore
dotnet run

# Access the application  
open https://localhost:7139
```

## 🎨 UI Components

- **Navigation**: Bootstrap navbar with language switcher
- **Language Switcher**: Dropdown with native language names
- **Localized Labels**: All UI text translated using `@Localizer["key"]` syntax
- **Responsive Design**: Mobile-friendly interface

## 📊 Database Schema

### JournalEntry Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key (auto-increment) |
| Title | TEXT | Entry title (required) |
| Content | TEXT | Entry content (required) |
| CreatedDate | TEXT | Creation timestamp |
| Language | TEXT | Entry language code |

## 🔧 Configuration

### Database Connection
- **Local**: `Data Source=journal.db`
- **Docker**: `Data Source=/app/data/journal.db`

### Supported Cultures
Configured in `Program.cs`:
```csharp
var supportedCultures = new[] { "en", "es", "fr", "de", "it" };
```

## 🌐 Localization Architecture

### Resource Files
- `SharedResource.en.resx` - English (default)
- `SharedResource.es.resx` - Spanish
- `SharedResource.fr.resx` - French
- `SharedResource.de.resx` - German
- `SharedResource.it.resx` - Italian

### Usage in Razor Pages
```csharp
@inject IStringLocalizer<SharedResource> Localizer
<h1>@Localizer["Welcome"]</h1>
```

### Language Switching
- Cookie-based persistence
- Automatic culture detection
- Fallback to default language

## 🎓 Learning Objectives

This phase teaches:
- **.NET Core localization** setup and configuration
- **Resource file (.resx)** management and structure
- **Culture switching** with middleware
- **Localization best practices** for web applications
- **Cookie-based persistence** for user preferences

## 🐛 Troubleshooting

### Localization Issues
- Ensure resource files are embedded resources
- Check culture configuration in Program.cs
- Verify SharedResource class exists

### Docker Issues
```bash
# Clean rebuild
docker-compose down -v
docker-compose up --build
```

## 🔮 Next Phase

Phase 4 will add AI-powered translation utility for automated resource generation.

---

**Happy Journaling! 📖✨**