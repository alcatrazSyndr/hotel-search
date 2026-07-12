# Hotel Search API

A JSON REST API for hotel search, built for a take-home assignment.

The service exposes two interfaces:
1. A **CRUD interface** for managing hotel data
2. A **search interface** that extracts a location and budget from a free-text prompt, and returns matching hotels ranked by price and distance

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- In-memory storage (no database required, but designed to allow one to be added easily)
- Swagger / OpenAPI for interactive API documentation

## Architecture & Design Principles

- Clean Architecture (strict inward-pointing dependencies across four projects)
- Domain-Driven Design concepts (entities, value objects)
- Dependency Inversion (repository and parser behind interfaces, swappable without touching business logic)

```
HotelSearch.Api            → Controllers, DI wiring, middleware
HotelSearch.Application    → Use cases, interfaces, DTOs, value objects
HotelSearch.Infrastructure → Concrete implementations (in-memory repository, regex parser)
HotelSearch.Domain         → Core entities and value objects, no external dependencies
```

- **Domain** contains `Hotel` (entity) and `GeoLocation` (value object, with Haversine distance calculation).
- **Application** contains the use-case logic (`HotelSearchService`), interfaces (`IHotelRepository`, `IHotelSearchService`, `IHotelSearchPromptParser`), DTOs, and the `HotelSearchParameters` value object.
- **Infrastructure** contains `InMemoryHotelRepository` (thread-safe, in-memory storage) and `RegexHotelSearchPromptParser` (extracts location/budget from free text using regex).
- **Api** wires everything together via dependency injection and exposes the REST endpoints.

This separation means the in-memory repository and regex parser can each be swapped for a different implementation (e.g. a real database, or an AI-based parser) by writing a new class that implements the same interface and changing one line in `Program.cs` — no other code needs to change.

## Running the project

1. Clone the repository
2. Open `HotelSearch.sln` in Visual Studio
3. Set `HotelSearch.Api` as the startup project
4. Press F5 (or Ctrl+F5)
5. Swagger UI will open automatically at `https://localhost:<port>/swagger`

## API Endpoints

### CRUD
- `GET /api/Hotel` — get all hotels
- `GET /api/Hotel/{id}` — get a hotel by id
- `POST /api/Hotel` — create a hotel
- `PUT /api/Hotel/{id}` — update a hotel
- `DELETE /api/Hotel/{id}` — delete a hotel

### Search
- `GET /api/Hotel/search?prompt={text}&page={n}&pageSize={n}`

Example: `GET /api/Hotel/search?prompt=cheap hotels near Zagreb under 100&page=1&pageSize=10`

The prompt can mention a known city and/or a budget (e.g. "under 100", "budget 150", "between 50 and 100", "over 75"). Both are optional — an empty prompt returns all hotels. Paging is supported (`page`, `pageSize`, capped at 100 per page).

## How search ranking works

Each matching hotel is scored using min-max normalized price and distance (both scaled to a 0–1 range relative to the current result set), combined into a single weighted score:

```
score = (normalizedPrice * 1.0) + (normalizedDistance * 3.0)
```

Distance is weighted more heavily than price, so nearby hotels rank above cheaper-but-farther ones. Results are sorted ascending by score (lower = better).

## Known limitations / possible future improvements

- **Location matching** is a hardcoded city dictionary with exact (diacritic-insensitive) string matching. It does not handle Croatian grammatical case declension (e.g. "Zagrebu", "Zagreba") or transliterated spellings beyond a few manually added aliases. A real geocoding API, or an AI/NLP-based parser, would handle this far more robustly.
- **Prompt parsing** is regex-based, which is fully explainable and dependency-free but has an inherent ceiling — it can't understand arbitrary natural-language phrasing the way an LLM-based parser could. Since the parser is behind an interface (`IHotelSearchPromptParser`), an AI-based implementation could be added without changing any other part of the application.
- **Scoring weights** (price vs. distance) are currently hardcoded constants. These could be made configurable per-request or via app settings.
- **No persistent storage** — hotels are stored in memory and reset on restart, per the assignment's requirements. The repository is behind an interface (`IHotelRepository`), so a database-backed implementation could be added without changing the Application or API layers.
- **No authentication/authorization** — out of scope for this PoC.
- A client with access to native geolocation (e.g. a browser or mobile app) could be supported by accepting latitude/longitude directly as optional parameters, bypassing prompt-based location extraction entirely.

## AI Usage

See [AI_USAGE.md](./AI_USAGE.md) for a detailed account of how AI tools were used during development.