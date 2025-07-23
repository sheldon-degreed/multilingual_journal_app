# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MultilingualJournalApp.csproj", "./"]
RUN dotnet restore "MultilingualJournalApp.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "MultilingualJournalApp.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MultilingualJournalApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: copy the published app to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create the database directory and set permissions
RUN mkdir -p /app/data && chmod 755 /app/data

# Set the connection string to use the persistent volume
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/journal.db"

ENTRYPOINT ["dotnet", "MultilingualJournalApp.dll"]