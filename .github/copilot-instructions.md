# Copilot Instructions

## Project Guidelines
- Prefer DTOs as classic C# classes with public get/set properties instead of record types when generating code for this workspace.
- Prefer DTOs split into one-class-per-file under `MagicWise.Integrations.V1.Models` for this workspace.
- Do not use C# expression-bodied members (the '=>') in generated code or responses; use explicit method bodies instead.