Contributing to RazorPlus

Principles
- Server-first Razor Pages ergonomics. Tag Helpers are the primary API.
- Accessibility and performance are non-negotiable. Keep JS tiny and optional.
- Small, elegant surface area. Prefer clear constraints over feature sprawl.

Project Setup
- Requirements: .NET 7 SDK (local), Node 18+ for Playwright tests (later stage).
- Solution: `RazorPlus.sln`
  - Library: `src/RazorPlus`
  - Docs app: `samples/RazorPlus.Docs`
  - Tests: `tests/RazorPlus.Tests`

Common Commands
- Build: `dotnet build`
- Run docs: `dotnet run --project samples/RazorPlus.Docs`
- Tests: `dotnet test`

Coding Standards
- C#: nullable enabled, implicit usings on. Favor explicit, readable names.
- Tag Helpers: expose intuitive attributes; mirror HTML semantics where possible.
- Accessibility: define ARIA roles and keyboard flows in component docs; ensure focus management.
- CSS: use CSS variables only; avoid frameworks. Keep selectors flat, component-scoped with `rp-` prefix.
- JS: framework-free ES modules; progressive enhancement via `data-rp-*`.

Commit Conventions
- Conventional Commits
  - feat(rp-button): add loading state
  - fix(rp-input): correct aria-invalid
  - docs(docs): add button examples
  - test(taghelpers): add table header tests
  - chore(ci): bump action versions

Review Checklist
- API consistency across components (names, variants, sizes).
- a11y keyboard behavior meets spec; roles/labels present.
- Server-first rendering works without JS; enhancement is optional.
- Tests updated or added (unit + e2e when applicable).

Release
- SemVer. Document changes in CHANGELOG.md.
- Library packs static web assets under `wwwroot`.

