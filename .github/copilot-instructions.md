# Copilot Instructions

## Project Guidelines
- Prefer DTOs as classic C# classes with public get/set properties instead of record types when generating code for this workspace.
- Prefer DTOs split into one-class-per-file under `MagicWise.Integrations.V1.Models` for this workspace.
- Do not use C# expression-bodied members (the '=>') in generated code or responses; use explicit method bodies instead.

## Commit Message Guidelines
- Always generate a short imperative subject line.
- Always include a body with bullet points.
- Use `- ` as the bullet marker.
- Include at least 2 bullet points summarizing the main code changes.
- Do not use paragraphs in the commit body unless explicitly requested.
