# Multilingual Journal Web App - Phase 1

A basic .NET Core Razor Pages application for creating and managing personal journal entries.

## 🎯 Phase 1: Journal App Basics

This phase demonstrates:
- ASP.NET Core Razor Pages architecture
- Entity Framework Core with SQLite
- Basic CRUD operations
- Bootstrap UI components

## 🚀 Features

- **📝 Journal Management**: Create, read, update, and delete personal journal entries
- **💾 SQLite Database**: Lightweight, file-based database with Entity Framework Core
- **🎨 Bootstrap UI**: Responsive web interface with modern design

## 🏗️ Architecture

- **Frontend**: ASP.NET Core Razor Pages with Bootstrap 5
- **Backend**: Entity Framework Core with SQLite
- **Database**: Local SQLite file

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK

### Run Locally
```bash
# Navigate to the app directory
cd MultilingualJournalApp

# Restore packages and run
dotnet restore
dotnet run

# Access the application  
open https://localhost:7139
```

## 📊 Database Schema

### JournalEntry Table
| Column | Type | Description |
|--------|------|-------------|
| Id | INTEGER | Primary key (auto-increment) |
| Title | TEXT | Entry title (required) |
| Content | TEXT | Entry content (required) |
| CreatedDate | TEXT | Creation timestamp |
| Language | TEXT | Entry language code |

## 🎓 Learning Objectives

This phase teaches:
- **Razor Pages routing** and page structure
- **Entity Framework Core** setup and migrations
- **Model binding** and validation
- **Bootstrap integration** for responsive UI
- **CRUD operations** with proper error handling

## 🔮 Next Phase

Phase 2 will add Docker containerization support.

---

**Happy Journaling! 📖✨**