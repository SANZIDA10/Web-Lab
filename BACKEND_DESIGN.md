# KUET Career Club - Backend Members Management System

## Overview
This document describes the backend architecture and API endpoints for managing club members through the "Become a Member" feature.

## Architecture

### Technology Stack
- **Framework**: ASP.NET Core 7+
- **Language**: C#
- **Data Storage**: JSON file-based persistence
- **Architecture**: Minimal APIs (lightweight and performant)

### Project Structure
```
backend/
├── Program.cs          # Main application file with all endpoints
└── WebLab.Api.csproj  # Project configuration
data/
└── submissions.json   # Persistent storage for member data
```

## Data Models

### SubmissionRecord
Core record type for all submissions (members, contacts, etc.)

```csharp
public sealed record SubmissionRecord(
    Guid Id,                                        // Unique identifier
    string Type,                                    // "join" for members, "contact" for messages
    DateTimeOffset SubmittedAt,                    // Submission timestamp
    IReadOnlyDictionary<string, string> Data      // Flexible data dictionary
);
```

### JoinSubmission (Member Registration)
Request model for becoming a member:

```csharp
public sealed record JoinSubmission(
    string? Name,              // Full name
    string? Email,             // Email address
    string? StudyProgram,      // e.g., "CSE", "EEE", "ME"
    string? Interests,         // e.g., "Workshops, mentoring"
    string? Message           // Why they want to join (min 10 chars)
);
```

### ContactSubmission
Request model for contact form:

```csharp
public sealed record ContactSubmission(
    string? Name,
    string? Email,
    string? Message
);
```

## API Endpoints

### 1. Health Check
**GET** `/api/health`

Check if the backend service is running.

**Response:**
```json
{
  "status": "ok",
  "service": "KUET Career Club backend"
}
```

### 2. Submit Member Registration
**POST** `/api/join`

Register as a member to join the club.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "studyProgram": "CSE",
  "interests": "Workshops, mentoring, internships",
  "message": "I'm interested in developing my career skills and networking with professionals."
}
```

**Validation Rules:**
- `name`: Required, minimum 2 characters
- `email`: Required, must be valid email format
- `message`: Required, minimum 10 characters
- `studyProgram`: Optional
- `interests`: Optional

**Success Response (200 OK):**
```json
{
  "message": "Your membership interest has been recorded.",
  "submissionId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Please enter a valid email address."
}
```

### 3. Submit Contact Message
**POST** `/api/contact`

Send a general contact message.

**Request Body:**
```json
{
  "name": "Jane Smith",
  "email": "jane@example.com",
  "message": "I have a question about the upcoming workshop."
}
```

**Success Response (200 OK):**
```json
{
  "message": "Thanks! Your message has been received.",
  "submissionId": "550e8400-e29b-41d4-a716-446655440001"
}
```

### 4. Get All Members
**GET** `/api/members`

Retrieve all registered members (filtered from "join" type submissions).

**Response:**
```json
{
  "count": 5,
  "members": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "type": "join",
      "submittedAt": "2026-05-09T10:30:00Z",
      "data": {
        "name": "John Doe",
        "email": "john@example.com",
        "studyProgram": "CSE",
        "interests": "Workshops, mentoring",
        "message": "I'm interested..."
      }
    }
  ]
}
```

### 5. Search Members
**GET** `/api/members/search?q=<search_term>`

Search members by name, email, or program.

**Query Parameters:**
- `q`: Search term (minimum recommended 2 characters)

**Example:** `/api/members/search?q=CSE`

**Response:**
```json
{
  "count": 2,
  "searchTerm": "CSE",
  "members": [
    { /* member records matching "CSE" */ }
  ]
}
```

### 6. Get Member Statistics
**GET** `/api/members/stats`

Get aggregated member statistics and program distribution.

**Response:**
```json
{
  "totalMembers": 15,
  "joinedThisMonth": 5,
  "programDistribution": {
    "CSE": 8,
    "EEE": 4,
    "ME": 2,
    "Other": 1
  }
}
```

### 7. Get All Submissions
**GET** `/api/submissions`

Retrieve all submissions (members and contacts combined).

**Response:**
```json
[
  {
    "id": "...",
    "type": "join",
    "submittedAt": "...",
    "data": { /* ... */ }
  },
  {
    "id": "...",
    "type": "contact",
    "submittedAt": "...",
    "data": { /* ... */ }
  }
]
```

## Data Persistence

### File-Based Storage
- **Location**: `data/submissions.json` (relative to the web root)
- **Format**: JSON array of submission records
- **Auto-save**: Every submission is automatically saved
- **Auto-load**: Submissions are loaded on application startup

### File Structure
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "type": "join",
    "submittedAt": "2026-05-09T10:30:00+00:00",
    "data": {
      "name": "John Doe",
      "email": "john@example.com",
      "studyProgram": "CSE",
      "interests": "Workshops",
      "message": "I want to join..."
    }
  }
]
```

## Frontend Integration

### Members Dashboard (members.html)
A comprehensive dashboard for viewing all club members with:

#### Features
- **Member Directory**: Display all registered members in card format
- **Search Functionality**: Search by name, email, or program
- **Statistics Panel**: 
  - Total member count
  - Members joined this month
  - Distribution by program
- **Member Details**: View each member's:
  - Full name and email
  - Study program
  - Interests
  - Membership motivation
  - Join date

#### API Usage in Dashboard
```javascript
// Fetch all members
GET /api/members

// Search members
GET /api/members/search?q=search_term

// Get statistics
GET /api/members/stats
```

## Security Considerations

### Current Implementation
- Basic email validation
- Input sanitization (XSS prevention in frontend)
- No authentication/authorization (add for production)

### Production Recommendations
1. **Add Authentication**: Implement OAuth or JWT for admin access
2. **Add Authorization**: Restrict membership viewing to authenticated users
3. **Add Rate Limiting**: Prevent abuse of submission endpoints
4. **Add Data Validation**: Server-side sanitization of all inputs
5. **Add HTTPS**: Encrypt data in transit
6. **Add CORS**: Configure appropriate CORS policies
7. **Add Database**: Replace JSON file storage with SQL/NoSQL database
8. **Add Logging**: Implement comprehensive audit logging
9. **Add Backup**: Implement automatic data backups

## Validation Rules

### Common Validations (Applied to all submissions)

```
Name:
  - Required: Yes
  - Min Length: 2 characters
  - Max Length: No limit (UI prevents spam)

Email:
  - Required: Yes
  - Format: Valid email address
  - Example: user@example.com

Message:
  - Required: Yes (for join/contact)
  - Min Length: 10 characters
  - Max Length: No limit
```

## Error Handling

### Common Error Responses

| Status | Error | Cause |
|--------|-------|-------|
| 400 | "Please enter your full name." | Name missing or < 2 chars |
| 400 | "Please enter a valid email address." | Invalid email format |
| 400 | "Your message should be at least 10 characters long." | Message too short |
| 500 | Internal Server Error | Unexpected server error |

## Performance Considerations

### Scalability
- **Current**: In-memory queue + file storage (suitable for < 10k members)
- **Future**: Migrate to database (SQL Server, PostgreSQL, MongoDB)
- **Search**: Currently O(n) linear search; add indexing with database

### Optimization Tips
1. Implement pagination for member listings
2. Add caching for frequently accessed statistics
3. Use database indexing on name, email, program
4. Implement async/await patterns for I/O operations
5. Add compression for large datasets

## Deployment Guide

### Prerequisites
- .NET 7+ SDK
- Operating System: Windows, Linux, or macOS

### Build
```bash
cd backend
dotnet build
```

### Run Development Server
```bash
dotnet run
```

### Production Deployment
```bash
dotnet publish -c Release
# Host the published output on IIS, Linux, Docker, etc.
```

### Environment Configuration
- Web root: Configured to serve parent directory (frontend files)
- Static files: All HTML, CSS, JS from web root
- API: Available at `/api/*` endpoints

## Example Integration Workflow

1. **User registers** via `join.html` form
2. **Frontend submits** to `POST /api/join`
3. **Backend validates** and saves to `submissions.json`
4. **Success response** with submission ID
5. **Admin/member views** all members in `members.html`
6. **Dashboard fetches** data from `GET /api/members` and `GET /api/members/stats`
7. **Member directory** displays live member list with search capability

## Future Enhancements

### Phase 2
- [ ] Member profile pages
- [ ] Member-to-member messaging
- [ ] Event registration tracking
- [ ] Attendance management

### Phase 3
- [ ] Member roles and permissions
- [ ] Admin dashboard with full CRUD operations
- [ ] Email notifications
- [ ] Member verification
- [ ] Export member data to Excel/CSV

### Phase 4
- [ ] Mobile app integration
- [ ] Advanced analytics
- [ ] Member renewal/subscription system
- [ ] Payment integration
- [ ] Third-party integrations (Slack, Teams, etc.)

## Support & Maintenance

### Regular Tasks
- Monitor `data/submissions.json` file size
- Back up member data regularly
- Review validation rules quarterly
- Update security protocols

### Troubleshooting
- **No members showing**: Check `data/submissions.json` exists and is readable
- **Search not working**: Verify API endpoint is accessible
- **File permission errors**: Ensure app has write permissions to `data/` directory

## Contact
For questions or issues with the backend system, please contact the development team.
