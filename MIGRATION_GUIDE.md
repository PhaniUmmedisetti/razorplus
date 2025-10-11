RazorPlus Migration Guide (stub)

Audience
- Teams migrating duplicated Razor markup (tables, forms, selects) to RazorPlus components.

Strategy
- Start with low-risk replacements: rp-button, rp-input, rp-select.
- Replace custom tables with <rp-table> + <rp-column> in server mode.
- Introduce <rp-tabs> and <rp-accordion> to unify interactions.

Mappings (initial)
- Buttons: map existing variants to RazorPlus variants
  - primary -> variant="primary"
  - secondary -> variant="secondary"
  - outline/ghost -> variant="ghost"
  - danger -> variant="danger"
- Inputs: wrap fields with rp-input to standardize labels/hints/validation.
- Selects: map `IEnumerable<SelectListItem>` to `items`.

Bootstrap Compatibility (optional)
- If adopting incrementally, keep BS utilities for layout. RazorPlus components are class-name independent.

Known Considerations
- Ensure unobtrusive validation is enabled for consistent error rendering.
- For tables, start with server paging; defer client sorting to a later phase.

Next
- Analyzer package will suggest replacements automatically (future).

