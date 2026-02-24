# GPS Multi-Receiver Boundary Recorder — v1 Implementation Decisions

## Locked decisions from product review

- App model: **single-user desktop**.
- MVP receiver cap: **3 active receivers**.
- Receiver streams: **fully independent**, aligned by timestamp during export.
- Timestamp storage/export: **UTC ISO-8601**.
- Marker capture behavior: use **latest valid value per receiver** at marker time.
- Disconnect behavior while recording: **continue recording** with remaining receivers.
- Heading fallback: use **VTG first**, then calculated heading from track points.
- Low-speed offset behavior: if no previous valid heading exists, wait for first computed heading before applying directional offset.
- CSV outputs: emit **separate raw and offset CSV files** per receiver.
- Shapefile attribute naming: enforce **DBF-safe field names**.
- CRS: **EPSG:4326**.
- Receiver folder names: sanitized to filesystem-safe identifiers.
- Field import duplicate handling: **merge duplicates**.
- Field IDs: local GUIDs only.
- UX: add **Driving Mode** toggle and strong visual alert treatment for critical states.
- QA priority: parser correctness, DB integrity, export validation at **equal priority**.

## Recommended execution strategy

Use a vertical slice first, then fill in breadth:

1. Connect 2-3 serial streams and parse GGA/VTG/GST.
2. Start a session and record one track segment + one marker.
3. Persist to SQLite with autosave and recoverability hooks.
4. Export receiver-specific CSV (raw + offset) and basic shapefiles.

This yields an end-to-end prove-out early and exposes the riskiest integration points (serial variability + persistence + export compatibility).
