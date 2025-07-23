# 🚀 Developer Setup Guide - MacBook Pro

A comprehensive setup guide for novice developers to get the Multilingual Journal Web App running on MacBook Pro.

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Phase-by-Phase Setup](#phase-by-phase-setup)  
- [Architecture Overview](#architecture-overview)
- [Troubleshooting](#troubleshooting)
- [Development Workflow](#development-workflow)
- [Useful Commands](#useful-commands)

## 🔧 Prerequisites

### Required Tools

| Tool | Version | Purpose | Installation Method |
|------|---------|---------|-------------------|
| **Git** | 2.40+ | Version control | Pre-installed on macOS |
| **.NET SDK** | 8.0+ | .NET development | Download from Microsoft |
| **Docker Desktop** | 4.20+ | Containerization | Download from Docker |
| **VS Code** | Latest | Code editor (recommended) | Download from Microsoft |
| **SQL Server** | 2022+ | Database (via Docker) | Included in docker-compose |

### System Requirements

- **macOS**: 12.0 (Monterey) or later
- **RAM**: 8GB minimum, 16GB recommended  
- **Storage**: 5GB free space
- **Internet**: Required for package downloads

## 🏗️ Architecture Overview

This project contains two applications demonstrating different .NET Core approaches:

### 1. MultilingualJournal (Web API + SPA)
- **Backend**: ASP.NET Core Web API with Entity Framework Core
- **Database**: SQL Server (dockerized)
- **Frontend**: HTML/CSS/JavaScript SPA with Bootstrap
- **Features**: Full CRUD API, translation service integration, milestone Git tagging

### 2. MultilingualJournalApp (Razor Pages)  
- **Backend**: ASP.NET Core Razor Pages with Entity Framework Core
- **Database**: SQLite (file-based)
- **Frontend**: Server-rendered Razor Pages with Bootstrap
- **Features**: Internationalization (i18n), localization, educational progression

The main docker-compose.yml runs the Web API version, while individual phases demonstrate the Razor Pages progression.

## 🛠️ Step-by-Step Installation

### 1. Install Homebrew (Package Manager)

```bash
# Open Terminal and run:
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Add to PATH (follow the instructions shown after installation)
echo 'eval "$(/opt/homebrew/bin/brew shellenv)"' >> ~/.zprofile
source ~/.zprofile

# Verify installation
brew --version
```

### 2. Install Git (if not already installed)

```bash
# Check if Git is installed
git --version

# If not installed, install via Homebrew
brew install git

# Configure Git (replace with your info)
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### 3. Install .NET 8.0 SDK

**Option A: Download from Microsoft (Recommended)**
1. Visit [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. Download ".NET 8.0 SDK" for macOS
3. Run the installer package
4. Follow the installation wizard

**Option B: Install via Homebrew**
```bash
brew install dotnet
```

**Verify Installation:**
```bash
dotnet --version
# Should show 8.0.x
```

### 4. Install Docker Desktop

1. Visit [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
2. Download "Docker Desktop for Mac"
3. Open the `.dmg` file and drag Docker to Applications
4. Launch Docker Desktop from Applications
5. Follow the setup wizard and create a Docker account if needed

**Verify Installation:**
```bash
docker --version
docker-compose --version
```

### 5. Install Visual Studio Code (Recommended)

1. Visit [https://code.visualstudio.com/](https://code.visualstudio.com/)
2. Download for macOS
3. Open the `.zip` file and drag to Applications
4. Launch VS Code

**Install Recommended Extensions:**
```bash
# Open VS Code and install these extensions:
# - C# (Microsoft)
# - Docker (Microsoft)  
# - GitLens (GitKraken)
# - REST Client (Huachao Mao)
```

## 📂 Project Setup

### Clone the Repository

```bash
# Create a development directory
mkdir -p ~/Development
cd ~/Development

# Clone the project
git clone <repository-url> multilingual-journal
cd multilingual-journal

# View available phases
git tag --list
```

## 🔄 Quick Start (Docker - Recommended)

### Run the Complete Application

```bash
# Start all services with Docker Compose
docker-compose up --build

# Access the application
open http://localhost:5000        # Main Web API + SPA
open http://localhost:5000/swagger # API Documentation
open http://localhost:5001        # Translation Service
```

**Expected Outcome:**
- ✅ Web API with full CRUD operations
- ✅ SQL Server database with automatic setup
- ✅ Translation service integration
- ✅ Bootstrap SPA frontend

## 🔄 Phase-by-Phase Setup (Educational)

### Phase 1: Basic Razor Pages App

**Checkout Phase 1:**
```bash
git checkout phase-1-complete
```

**Run the Razor Pages Application:**
```bash
# Navigate to the Razor Pages app directory
cd MultilingualJournalApp

# Restore packages
dotnet restore

# Run the application
dotnet run

# Open in browser
open https://localhost:7139
```

**Expected Outcome:**
- ✅ Basic journal app with CRUD operations
- ✅ SQLite database created automatically
- ✅ Bootstrap UI with responsive design

---

### Phase 2: Docker Support

**Checkout Phase 2:**
```bash
git checkout phase-2-complete
```

**Run Razor Pages App with Docker:**
```bash
# Navigate to Razor Pages app
cd MultilingualJournalApp

# Build and run the individual Razor Pages container
docker build -t multilingual-journal-app .
docker run -p 5000:8080 -v $(pwd)/Data:/app/Data multilingual-journal-app

# Open in browser
open http://localhost:5000
```

**Alternative - Local Development:**
```bash
cd MultilingualJournalApp
dotnet restore
dotnet run
open https://localhost:7139
```

**Expected Outcome:**
- ✅ Razor Pages application runs in Docker container
- ✅ SQLite database persists with volume mapping
- ✅ Port 5000 mapped to container

---

### Phase 3: Internationalization

**Checkout Phase 3:**
```bash
git checkout phase-3-complete
```

**Run the Application:**
```bash
# Option 1: Docker (Recommended)
docker-compose up --build
open http://localhost:5000

# Option 2: Local
cd MultilingualJournalApp
dotnet restore
dotnet run
open https://localhost:7139
```

**Test Language Switching:**
1. Look for the globe 🌍 dropdown in the navigation
2. Switch between languages (English, Spanish, French, German, Italian)
3. Verify UI labels change language
4. Language preference should persist

**Expected Outcome:**
- ✅ Language switcher in navigation
- ✅ UI labels translated to 5 languages
- ✅ Language preference saved in cookies

---

### Phase 4: AI Translation Utility

**Checkout Phase 4 (Complete Educational Project):**
```bash
git checkout phase-4-complete
```

**Run the Razor Pages Application:**
```bash
cd MultilingualJournalApp
dotnet restore
dotnet run
open https://localhost:7139
```

**Test the Translation Utility:**
```bash
# In a new terminal window
cd TranslationUtility
dotnet run
```

**Expected Outcome:**
- ✅ Complete multilingual Razor Pages application
- ✅ AI translation utility generates .resx files
- ✅ All educational phases integrated and working

---

## 🚀 Production Web API Application

**For the full-featured production application:**

```bash
# Run the complete Web API + SPA + Translation Service
docker-compose up --build

# Access different parts of the application
open http://localhost:5000         # Main SPA Frontend
open http://localhost:5000/swagger  # API Documentation
open http://localhost:5001         # Translation Service API
```

**Features Available:**
- ✅ RESTful Web API with full CRUD operations
- ✅ SQL Server database with Entity Framework
- ✅ Single Page Application (SPA) frontend
- ✅ Microservice architecture with translation service
- ✅ Git tagging integration for milestone tracking
- ✅ Swagger API documentation

## 🚨 Troubleshooting

### Common Issues

#### 1. ".NET SDK not found"
```bash
# Check if .NET is in PATH
echo $PATH | grep dotnet

# If not found, add to PATH
echo 'export PATH="/usr/local/share/dotnet:$PATH"' >> ~/.zprofile
source ~/.zprofile
```

#### 2. "Docker daemon not running"
```bash
# Start Docker Desktop application
open -a Docker

# Wait for Docker to start (whale icon in menu bar)
# Then retry your docker commands
```

#### 3. "Port 5000 already in use"
```bash
# Find what's using the port
lsof -i :5000

# Kill the process (replace PID with actual process ID)
kill -9 <PID>

# Or change port in docker-compose.yml
```

#### 4. "SQLite database locked" (Razor Pages App)
```bash
# Stop the application
pkill -f MultilingualJournalApp

# Remove the locked database file
cd MultilingualJournalApp
rm journal.db*

# Restart the application
dotnet run
```

#### 5. "SQL Server connection failed" (Web API App)
```bash
# Stop all running containers
docker-compose down

# Remove volumes (this will delete data!)
docker-compose down -v

# Restart with fresh database
docker-compose up --build

# Check SQL Server container logs
docker-compose logs sqlserver
```

#### 6. "Permission denied" errors
```bash
# Fix Docker permissions
sudo chown -R $(whoami) ~/.docker

# Restart Docker Desktop
```

#### 7. "Translation service not responding"
```bash
# Check if translation service container is running
docker-compose ps

# Restart just the translation service
docker-compose restart translation-service

# Check translation service logs
docker-compose logs translation-service
```

### Entity Framework Issues

```bash
# Install EF tools globally
dotnet tool install --global dotnet-ef

# Update EF tools
dotnet tool update --global dotnet-ef

# Reset database
cd MultilingualJournalApp
rm journal.db*
dotnet ef database update
```

### VS Code Issues

**If C# extension not working:**
1. Open Command Palette: `Cmd+Shift+P`
2. Type: "Developer: Reload Window"
3. Wait for OmniSharp to load

## 💡 Development Workflow

### Daily Development

```bash
# 1. Start your development session
cd ~/Development/multilingual-journal
git status
git pull origin master

# 2. Choose your phase
git checkout phase-X-complete

# 3. Start the application
docker-compose up --build

# 4. Open in VS Code
code .

# 5. Make changes and test
# 6. Commit your work when done
git add .
git commit -m "Your changes"
```

### Testing Different Phases

```bash
# Quick phase switching
git checkout phase-1-complete  # Basic journal
git checkout phase-2-complete  # + Docker  
git checkout phase-3-complete  # + i18n
git checkout phase-4-complete  # Complete project
```

## 📚 Useful Commands

### Git Commands
```bash
git tag --list                    # List all phase tags
git log --oneline                 # View commit history
git status                        # Check current status
git branch -a                     # List all branches
```

### .NET Commands
```bash
dotnet --info                     # System information
dotnet restore                    # Restore packages
dotnet build                      # Build project
dotnet run                        # Run project
dotnet ef database update         # Update database (EF migrations)
dotnet tool install --global dotnet-ef  # Install EF tools globally
```

### Docker Commands
```bash
# Web API Application (docker-compose)
docker-compose up --build         # Build and run all services
docker-compose down               # Stop all containers
docker-compose down -v            # Stop and remove volumes
docker-compose ps                 # List running services
docker-compose logs <service>     # View service logs

# Individual Container Commands
docker ps                         # List running containers
docker images                     # List images
docker system prune -a            # Clean up everything
docker build -t <name> .          # Build individual container
```

### Application-Specific Commands

#### Razor Pages App (MultilingualJournalApp)
```bash
cd MultilingualJournalApp
dotnet restore && dotnet run      # Quick start
rm journal.db*                    # Reset SQLite database
```

#### Web API App (MultilingualJournal)
```bash
cd MultilingualJournal
dotnet ef database update         # Apply migrations
dotnet run --urls="https://localhost:7139"  # Run on specific port
```

#### Translation Service
```bash
cd TranslationService
dotnet run --urls="http://localhost:5001"   # Run translation service
```

## 🎯 Learning Path

### For Complete Beginners

1. **Week 1**: Setup tools, run Phase 1
2. **Week 2**: Understand Razor Pages, try Phase 2
3. **Week 3**: Learn Docker basics, explore Phase 3
4. **Week 4**: Study i18n concepts, complete Phase 4

### Key Concepts to Learn

- **Razor Pages**: ASP.NET Core page-based programming model
- **Entity Framework**: .NET Object-Relational Mapping (ORM)
- **Docker**: Containerization platform
- **i18n**: Internationalization and localization
- **Git**: Version control system

## 🔗 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [Docker Documentation](https://docs.docker.com/)
- [Git Tutorial](https://git-scm.com/docs/gittutorial)
- [VS Code Tips](https://code.visualstudio.com/docs)

## 🆘 Getting Help

1. **Check the logs**: Look at terminal output for error details
2. **Google the error**: Copy-paste error messages into Google
3. **Stack Overflow**: Search for similar issues
4. **GitHub Issues**: Check the project's issue tracker
5. **Documentation**: Refer to official docs for each technology

---

**Happy Coding! 🚀**

*Last Updated: July 2025*