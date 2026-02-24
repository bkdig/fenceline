# Fenceline

Foundation code for the GPS multi-receiver boundary recording application.

## Repository layout

- `src/Fenceline.Core`: core parsing and geometry helpers.
- `tests/Fenceline.Core.Tests`: xUnit tests for parser/geometry behavior.
- `docs/v1-implementation-decisions.md`: locked product decisions for v1.
- `.github/workflows/ci.yml`: GitHub Actions build/test workflow.

## Run locally

Prerequisite: install **.NET 8 SDK**.

```bash
dotnet restore Fenceline.sln
dotnet build Fenceline.sln -c Release
dotnet test Fenceline.sln -c Release
```

## What is currently implemented

- NMEA checksum validation (`NmeaParser.IsChecksumValid`).
- GGA parsing for lat/lon/fix/HDOP/satellite/altitude data.
- Heading-based offset coordinate transform (`OffsetCalculator.Apply`).
- Unit tests for checksum and basic GGA parsing.



## Getting new files from GitHub

Whenever I share updates, run this in your local clone to get the new files:

```powershell
git fetch origin
git checkout main
git pull --rebase origin main
```

If Git says untracked files would be overwritten, clean local scaffold leftovers first:

```powershell
Remove-Item .\src\Fenceline.Core\Class1.cs -ErrorAction SilentlyContinue
Remove-Item .\tests\Fenceline.Core.Tests\UnitTest1.cs -ErrorAction SilentlyContinue
Remove-Item .\backup-local -Recurse -Force -ErrorAction SilentlyContinue
git clean -fd
```

Then run the pull commands again.

## Troubleshooting (Windows)

If build fails with `FactAttribute` not found and mentions `tests/Fenceline.Core.Tests/UnitTest1.cs`, you likely have leftover local files from `dotnet new`.

Delete local scaffold leftovers and try again:

```powershell
Remove-Item .\src\Fenceline.Core\Class1.cs -ErrorAction SilentlyContinue
Remove-Item .\tests\Fenceline.Core.Tests\UnitTest1.cs -ErrorAction SilentlyContinue
Remove-Item .\backup-local -Recurse -Force -ErrorAction SilentlyContinue
dotnet clean .\Fenceline.sln
dotnet restore .\Fenceline.sln
dotnet build .\Fenceline.sln -c Release
dotnet test .\Fenceline.sln -c Release
```
