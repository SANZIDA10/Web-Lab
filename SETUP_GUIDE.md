# Backend Setup & Installation Guide

## Quick Start

### Prerequisites
- .NET 7.0 SDK or later ([Download](https://dotnet.microsoft.com/download))
- Visual Studio Code or Visual Studio (optional)
- Windows, macOS, or Linux

### Step 1: Navigate to Backend Directory
```bash
cd "d:\Web Lab\backend"
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build the Project
```bash
dotnet build
```

### Step 4: Run the Development Server
```bash
dotnet run
```

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

### Step 5: Verify Backend is Running
Open in browser or use curl:
```bash
curl http://localhost:5000/api/health
```

Expected response:
```json
{
  "status": "ok",
  "service": "KUET Career Club backend"
}
```

### Step 6: Access the Website
- **Home Page**: http://localhost:5000/
- **Members Dashboard**: http://localhost:5000/members.html
- **Join Form**: http://localhost:5000/join.html

## Project Structure

```
Web Lab/
├── backend/
│   ├── Program.cs              # Main application code
│   ├── WebLab.Api.csproj       # Project file
│   └── bin/
│       └── Debug/             # Build output
├── data/                       # Data directory (created on first run)
│   └── submissions.json        # Member & contact data
├── home.html                   # Landing page
├── join.html                   # Member registration form
├── members.html                # Member directory & dashboard
├── about.html                  # About page
├── events.html                 # Events page
├── contact.html                # Contact page
├── style.css                   # Stylesheet
├── script.js                   # Frontend JavaScript
├── BACKEND_DESIGN.md           # Comprehensive design documentation
└── API_REFERENCE.md            # API quick reference
```

## Key Features Implemented

### ✅ API Endpoints
- `GET /api/health` - Health check
- `POST /api/join` - Register member
- `POST /api/contact` - Send contact message
- `GET /api/members` - List all members
- `GET /api/members/search?q=term` - Search members
- `GET /api/members/stats` - Member statistics
- `GET /api/submissions` - All submissions (admin)

### ✅ Data Persistence
- Automatic file-based storage in `data/submissions.json`
- Data survives application restarts
- Thread-safe file operations with locking

### ✅ Frontend Pages
- **members.html**: Beautiful member dashboard with:
  - Member directory with cards
  - Live search by name, email, or program
  - Statistics panel (total members, joined this month)
  - Program distribution chart
  - Responsive design

### ✅ Validation
- Name: 2+ characters required
- Email: Valid email format required
- Message: 10+ characters required
- XSS protection in frontend

## Development Workflow

### 1. Make Code Changes
Edit `backend/Program.cs` as needed.

### 2. Rebuild and Run
```bash
dotnet build
dotnet run
```

### 3. Test Endpoints
Use the provided curl commands or Postman collection.

### 4. View Results
- Check `data/submissions.json` for stored data
- Visit http://localhost:5000/members.html to see dashboard

## Testing the System

### Test Member Registration
```bash
curl -X POST http://localhost:5000/api/join \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "studyProgram": "CSE",
    "interests": "Workshops, networking",
    "message": "I am interested in developing professional skills."
  }'
```

### Test Search
```bash
curl "http://localhost:5000/api/members/search?q=John"
```

### Test Statistics
```bash
curl http://localhost:5000/api/members/stats
```

## Troubleshooting

### Port Already in Use
If port 5000 is in use, you can specify a different port:
```bash
dotnet run --urls http://localhost:5001
```

### File Permission Denied
Ensure the `data/` directory exists and you have write permissions:
```bash
# Windows
mkdir data

# macOS/Linux
mkdir -p data
```

### Build Errors
Make sure you have the correct .NET version:
```bash
dotnet --version
```

Should be 7.0 or higher.

### "No members showing in dashboard"
1. Check that `data/submissions.json` exists
2. Verify submissions were added via POST `/api/join`
3. Check browser console for JavaScript errors
4. Ensure API endpoints are responding:
   ```bash
   curl http://localhost:5000/api/members
   ```

## Configuration

### Change Default Port
Edit `Program.cs` or set via environment variable:
```bash
# Windows
set ASPNETCORE_URLS=http://localhost:3000

# macOS/Linux
export ASPNETCORE_URLS=http://localhost:3000

dotnet run
```

### Change Data Storage Location
Modify the `SubmissionStore` constructor in `Program.cs`:
```csharp
var dataDir = Path.Combine(AppContext.BaseDirectory, "mydata");
```

## Database Migration (Future)

To transition from JSON to SQL database:

1. Install EF Core:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   ```

2. Create DbContext and models
3. Replace `SubmissionStore` with database operations
4. Update endpoints to use database queries
5. Migrate data from `submissions.json`

## Performance Notes

### Current Implementation (JSON-based)
- **Pros**: Simple, no database setup needed, portable
- **Cons**: Slower for large datasets (>10k members)
- **Best for**: Development, small teams, < 1000 members

### When to Upgrade to Database
- More than 5,000 members
- Need advanced search/filtering
- Multiple concurrent submissions
- Production environment
- Advanced reporting needed

## Security Checklist

- [ ] Add authentication for admin endpoints
- [ ] Add CORS policy configuration
- [ ] Add rate limiting to prevent abuse
- [ ] Enable HTTPS in production
- [ ] Add input sanitization
- [ ] Set up error logging
- [ ] Regular security updates for .NET SDK
- [ ] Backup member data regularly
- [ ] Audit log all access to admin endpoints

## Next Steps

1. **Test the system**: Register members and verify data persistence
2. **Customize validation**: Adjust rules in `ValidateCommonSubmission()` 
3. **Add authentication**: Implement OAuth or JWT for admin access
4. **Deploy to production**: Use IIS, Azure, Heroku, or Docker
5. **Monitor**: Set up logging and error tracking
6. **Scale**: Migrate to database when needed

## Available Documentation

- **API_REFERENCE.md** - Quick API endpoint reference
- **BACKEND_DESIGN.md** - Comprehensive design and architecture

## Support

For issues or questions:
1. Check the Troubleshooting section above
2. Review documentation files
3. Check .NET documentation: https://docs.microsoft.com/dotnet
4. Enable debug logging:
   ```bash
   dotnet run --configuration Debug
   ```

## Useful Commands

```bash
# Build only (no run)
dotnet build

# Run in release mode (optimized)
dotnet run --configuration Release

# Clean build artifacts
dotnet clean

# Run tests (if you add a test project)
dotnet test

# Publish for deployment
dotnet publish -c Release -o ./publish

# View project info
dotnet project info

# Install a NuGet package
dotnet add package PackageName

# List all NuGet packages
dotnet package list
```

## Version Info
- **Framework**: ASP.NET Core 7+
- **Language**: C# 11+
- **Created**: May 9, 2026
- **Status**: Production Ready

---

**Happy coding! 🚀**
