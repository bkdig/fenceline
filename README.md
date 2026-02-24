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
