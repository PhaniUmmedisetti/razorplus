RazorPlus API Reference (outline)

Tag Helpers
- rp-button
  - Attributes: variant(primary|secondary|ghost|danger), size(sm|md|lg), icon, block, loading, as(button|a), href, disabled
  - ARIA: role=button when rendered as <a>; aria-disabled on links
- rp-input
  - Attributes: asp-for, label, hint, required, prefix, suffix
  - Behavior: integrates MVC validation; renders label/input/hint/error
- rp-select
  - Attributes: asp-for, label, items(IEnumerable<SelectListItem>), multiple, clearable, filterable
  - Behavior: server-rendered select; enhances via data-rp-select (future)
- rp-tabs / rp-tab
  - Attributes: rp-tabs(id); rp-tab(id, header, active)
  - Behavior: roving tabindex; synthesized tablist buttons
- rp-table / rp-column
  - Attributes: rp-table(items, sortable, pageable, page-size); rp-column(for, header, width)
  - Behavior: server-only rendering for MVP
- rp-chart
  - Attributes: id, type, data, options, height, theme(auto|light|dark), defer, export
  - Behavior: outputs container + <script type="application/json"> payload

Static Assets
- CSS: _content/RazorPlus/css/razorplus.css
- JS:  _content/RazorPlus/js/razorplus.core.js

Initialization
- Auto-init core on DOM ready. Optional: import and call `init()` for partial updates.

Accessibility Notes
- Buttons: visible focus, aria rules for link-as-button.
- Inputs: label association, aria-invalid via MVC validation.
- Tabs: tablist + roving tabindex; Enter/Space activates.

