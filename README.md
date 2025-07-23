# Multilingual Journal Web App - Phase 2

A .NET Core Razor Pages application with Docker containerization support.

## 🎯 Phase 2: Dockerize It

This phase adds:
- Docker containerization with multi-stage build
- Docker Compose orchestration
- Persistent SQLite database with volume mounting
- Production-ready container configuration

## 🚀 Features

- **📝 Journal Management**: Create, read, update, and delete personal journal entries
- **💾 SQLite Database**: Lightweight, file-based database with Entity Framework Core
- **🐳 Docker Support**: Fully containerized application with persistent data
- **🎨 Bootstrap UI**: Responsive web interface with modern design

## 🏗️ Architecture

- **Frontend**: ASP.NET Core Razor Pages with Bootstrap 5
- **Backend**: Entity Framework Core with SQLite
- **Containerization**: Docker with volume mounting for database persistence
- **Database**: Persistent SQLite with Docker volumes

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

## 🐳 Docker Configuration

### Dockerfile Features
- Multi-stage build for optimized image size
- ASP.NET Core 8.0 runtime
- Persistent data directory at `/app/data`
- Environment variable configuration

### Docker Compose Features
- Service orchestration
- Volume mounting for database persistence
- Port mapping (5000:80)
- Development environment configuration
- Automatic restart policy

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

## 🎓 Learning Objectives

This phase teaches:
- **Docker containerization** concepts and best practices
- **Multi-stage builds** for production optimization
- **Volume mounting** for data persistence
- **Docker Compose** orchestration
- **Environment configuration** for different deployment scenarios

## 🐛 Troubleshooting

### Docker Issues
```bash
# Clean rebuild
docker-compose down -v
docker-compose up --build
```

## 🔮 Next Phase

Phase 3 will add internationalization (i18n) support with multiple languages.

---

**Happy Journaling! 📖✨**