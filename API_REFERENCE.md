# Backend API Reference - Quick Guide

## Base URL
```
http://localhost:5000
```

## Endpoints Summary

### Health & Status
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/health` | Check backend status |

### Member Management
| Method | Endpoint | Purpose |
|--------|----------|---------|
| POST | `/api/join` | Register new member |
| GET | `/api/members` | List all members |
| GET | `/api/members/search?q=term` | Search members |
| GET | `/api/members/stats` | Get member statistics |

### Contact
| Method | Endpoint | Purpose |
|--------|----------|---------|
| POST | `/api/contact` | Send contact message |

### Admin
| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/submissions` | Get all submissions (join + contact) |

---

## Detailed Endpoints

### POST /api/join
**Register as a member**

```bash
curl -X POST http://localhost:5000/api/join \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Alice Johnson",
    "email": "alice@university.edu",
    "studyProgram": "CSE",
    "interests": "Software development, Data science",
    "message": "I want to enhance my professional skills and network with industry experts."
  }'
```

**Response (200):**
```json
{
  "message": "Your membership interest has been recorded.",
  "submissionId": "c47c5ba5-3b9e-4d8b-a1c2-5e9b7f8d3a2c"
}
```

**Validation:**
- Name: 2+ chars (required)
- Email: Valid format (required)
- Message: 10+ chars (required)
- StudyProgram: Optional
- Interests: Optional

---

### GET /api/members
**List all members**

```bash
curl http://localhost:5000/api/members
```

**Response (200):**
```json
{
  "count": 3,
  "members": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "type": "join",
      "submittedAt": "2026-05-09T10:30:00Z",
      "data": {
        "name": "Alice Johnson",
        "email": "alice@university.edu",
        "studyProgram": "CSE",
        "interests": "Software development",
        "message": "I want to enhance..."
      }
    }
  ]
}
```

---

### GET /api/members/search
**Search members**

```bash
# Search by name
curl "http://localhost:5000/api/members/search?q=Alice"

# Search by email
curl "http://localhost:5000/api/members/search?q=alice@university.edu"

# Search by program
curl "http://localhost:5000/api/members/search?q=CSE"
```

**Response (200):**
```json
{
  "count": 1,
  "searchTerm": "Alice",
  "members": [
    { /* matching member records */ }
  ]
}
```

---

### GET /api/members/stats
**Get member statistics**

```bash
curl http://localhost:5000/api/members/stats
```

**Response (200):**
```json
{
  "totalMembers": 15,
  "joinedThisMonth": 5,
  "programDistribution": {
    "CSE": 8,
    "EEE": 4,
    "ME": 2,
    "BTE": 1
  }
}
```

---

### POST /api/contact
**Send contact message**

```bash
curl -X POST http://localhost:5000/api/contact \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Bob Smith",
    "email": "bob@example.com",
    "message": "I have a question about the workshop schedule."
  }'
```

**Response (200):**
```json
{
  "message": "Thanks! Your message has been received.",
  "submissionId": "a1b2c3d4-e5f6-47g8-h9i0-j1k2l3m4n5o6"
}
```

---

### GET /api/submissions
**Get all submissions (admin only - no auth yet)**

```bash
curl http://localhost:5000/api/submissions
```

**Response (200):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "type": "join",
    "submittedAt": "2026-05-09T10:30:00Z",
    "data": { /* member data */ }
  },
  {
    "id": "660e8400-e29b-41d4-a716-446655440001",
    "type": "contact",
    "submittedAt": "2026-05-09T11:15:00Z",
    "data": { /* contact data */ }
  }
]
```

---

## Error Responses

### Bad Request (400)
```json
{
  "message": "Please enter a valid email address."
}
```

### Server Error (500)
```json
{
  "message": "Internal server error"
}
```

---

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 400 | Bad Request (validation error) |
| 404 | Not Found |
| 500 | Server Error |

---

## Data Storage

**File Location:** `data/submissions.json`

All submissions are automatically:
- Saved to JSON file on each POST
- Loaded from JSON file on application startup
- Persisted across application restarts

---

## Frontend Pages

| Page | File | Purpose |
|------|------|---------|
| Home | `home.html` | Landing page with "Become a Member" CTA |
| Join | `join.html` | Member registration form |
| Members | `members.html` | Member directory & dashboard |
| About | `about.html` | Club information |
| Events | `events.html` | Event listings |
| Contact | `contact.html` | Contact form |

---

## Integration Examples

### JavaScript - Register Member
```javascript
async function registerMember(formData) {
  const response = await fetch('/api/join', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      name: formData.name,
      email: formData.email,
      studyProgram: formData.studyProgram,
      interests: formData.interests,
      message: formData.message
    })
  });
  
  const result = await response.json();
  if (!response.ok) {
    console.error('Error:', result.message);
  } else {
    console.log('Success:', result.submissionId);
  }
}
```

### JavaScript - Load Member Directory
```javascript
async function loadMembers() {
  const response = await fetch('/api/members');
  const data = await response.json();
  console.log(`Total members: ${data.count}`);
  data.members.forEach(member => {
    console.log(`- ${member.data.name} (${member.data.email})`);
  });
}
```

### Python - Fetch Members
```python
import requests

# Get all members
response = requests.get('http://localhost:5000/api/members')
data = response.json()
print(f"Total members: {data['count']}")

# Search members
search = requests.get('http://localhost:5000/api/members/search?q=CSE')
results = search.json()
print(f"Found {results['count']} members in CSE")
```

---

## Development Notes

### Testing Endpoints
Use Postman, curl, or VS Code REST Client:

```rest
### Get health status
GET http://localhost:5000/api/health

### Register member
POST http://localhost:5000/api/join
Content-Type: application/json

{
  "name": "Test User",
  "email": "test@example.com",
  "studyProgram": "CSE",
  "interests": "Learning",
  "message": "This is a test message to join the club."
}

### Get all members
GET http://localhost:5000/api/members

### Search members
GET http://localhost:5000/api/members/search?q=Test

### Get stats
GET http://localhost:5000/api/members/stats
```

---

**Last Updated:** May 9, 2026  
**Version:** 1.0
