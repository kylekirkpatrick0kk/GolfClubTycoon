# Golf Club Tycoon

A management + life-sim game prototype built with C# and MonoGame (DesktopGL). You inherit a run‑down golf course and gradually restore, expand, and modernize it while exploring day‑to‑day club operations.

## Tech Stack
- **Engine:** MonoGame DesktopGL
- **Language:** C# (.NET 8)

## Getting Started
### Prerequisites
- .NET 8 SDK installed
- MonoGame templates/tools (optional but helpful: `dotnet new --install MonoGame.Templates.CSharp`)

### Build & Run
From the repository root:
```
dotnet build
dotnet run --project GolfClubTycoon/GolfClubTycoon.csproj
```

### Rebuilding Content (if you change assets)
MonoGame automatically builds content on project build. If you add new assets, ensure they are added to `Content.mgcb` via the MonoGame Content Pipeline Tool (mgcb-editor) or by editing the file directly.

## Roadmap (Near-Term)
- Camera that follows player (matrix transform)
- Basic tile map (CSV or simple array) & collision bounds
- Depth sorting by `Y` for overlapping objects
- Interaction system (press key near object/NPC)
- UI placeholder (money, day, weather)

## Longer-Term Vision
- Course renovation (place/upgrade tees, greens, hazards)
- Customer/NPC simulation (satisfaction, queueing, skill levels)
- Financial systems (revenue, expenses, staffing, marketing)
- Weather & season cycle influencing play
- Tournaments & reputation progression

## Contributing / Branching Strategy
Even for solo development, consider using feature branches:
```
git checkout -b feature/camera
git commit -m "Add basic camera follow"
git push origin feature/camera
```
Then merge via PR (locally or Git host) to keep history clean.

## Next Steps
Open an issue or request here what you want next (camera, tiles, collisions, UI).

---
Happy building! 🏌️‍♂️

Credit to https://alexs-assets.itch.io/ for the game art
