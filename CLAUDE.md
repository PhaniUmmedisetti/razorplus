# CLAUDE.md

This file provides complete knowledge about the RazorPlus project for AI assistants and developers. Reading this file gives you full context about the library's architecture, components, patterns, and implementation details.

---

## Project Overview

**RazorPlus** is a production-ready Razor Class Library (RCL) for ASP.NET Core that provides accessible, server-first UI components through TagHelpers. Built for Razor Pages and MVC applications, it emphasizes minimal JavaScript, progressive enhancement, and accessibility.

### Key Characteristics
- **Server-First Rendering**: HTML generated on the server, enhanced progressively
- **Accessibility Built-In**: ARIA attributes, keyboard navigation, semantic HTML
- **Minimal JavaScript**: ~4KB core + lazy-loaded modules for advanced features
- **Token-Based Theming**: CSS custom properties for light/dark modes
- **Native ASP.NET Integration**: Works seamlessly with model binding and validation
- **Multi-Framework**: Targets .NET 6.0 and .NET 7.0

### Project Structure

```
razorplus/
├── src/RazorPlus/               # Main library
│   ├── TagHelpers/              # All TagHelper implementations
│   │   ├── ButtonTagHelper.cs
│   │   ├── InputTagHelper.cs
│   │   ├── SelectTagHelper.cs
│   │   ├── TextAreaTagHelper.cs
│   │   ├── SwitchTagHelper.cs
│   │   ├── RadioGroupTagHelper.cs
│   │   ├── ValidationMessageTagHelper.cs
│   │   ├── TabsTagHelpers.cs
│   │   ├── AccordionTagHelper.cs
│   │   ├── ModalTagHelper.cs
│   │   ├── TableTagHelpers.cs
│   │   ├── PaginationTagHelper.cs
│   │   └── ChartTagHelper.cs
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── razorplus.css         # Complete stylesheet
│   │   └── js/
│   │       ├── razorplus.core.js      # Core initialization
│   │       ├── razorplus.select.js    # Select enhancements
│   │       ├── razorplus.table.js     # Client table features
│   │       └── razorplus.chart.js     # ECharts integration
│   └── RazorPlus.csproj
├── samples/RazorPlus.Docs/      # Documentation & demo site
│   ├── Pages/
│   │   ├── Index.cshtml
│   │   ├── Index.cshtml.cs
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   └── Program.cs
├── tests/RazorPlus.Tests/       # Unit tests
│   ├── ButtonTagHelperTests.cs
│   ├── PaginationTagHelperTests.cs
│   └── StructureTagHelperTests.cs
├── RAZORPLUS_DOCUMENTATION.md   # User documentation
└── RazorPlus.sln
```

### Dependencies
- **ASP.NET Core 6.0/7.0**: Framework reference only
- **ECharts 5.x**: Loaded dynamically via CDN for charts (optional)
- **No npm dependencies**: Pure server-side library

---

## Complete Component Reference

### Form Components

#### 1. rp-input (InputTagHelper.cs)

**Purpose**: Single-line text input with label, hint, prefix/suffix, and validation.

**Properties**:
- `asp-for` (ModelExpression): Model binding
- `label` (string): Label text (defaults to DisplayName)
- `hint` (string): Help text below input
- `required` (bool): Adds required attribute
- `prefix` (string): Text before input
- `suffix` (string): Text after input

**Implementation Details**:
```csharp
// File: src/RazorPlus/TagHelpers/InputTagHelper.cs
[HtmlTargetElement("rp-input", TagStructure = TagStructure.NormalOrSelfClosing)]
public class InputTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public InputTagHelper(IHtmlGenerator generator) => _generator = generator;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field");

        // Generate label using IHtmlGenerator
        var labelTag = _generator.GenerateLabel(ViewContext, For?.ModelExplorer, For?.Name, labelText, new { @class = "rp-label" });
        output.Content.AppendHtml(labelTag);

        // Wrapper with prefix/suffix support
        output.Content.AppendHtml("<div class=\"rp-input\">");
        if (!string.IsNullOrEmpty(Prefix))
        {
            output.Content.AppendHtml($"<span class=\"rp-input__affix rp-input__prefix\">{enc.Encode(Prefix)}</span>");
        }

        // Generate input
        var inputTag = _generator.GenerateTextBox(ViewContext, For?.ModelExplorer, For?.Name, For?.Model, null, new { @class = "rp-input__control", required = Required ? "required" : null });
        output.Content.AppendHtml(inputTag);

        // Validation message
        var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, message: null, tag: null, htmlAttributes: new { @class = "rp-error" });
        output.Content.AppendHtml(validation);
    }
}
```

**Generated HTML**:
```html
<div class="rp-field">
  <label class="rp-label" for="Email">Email</label>
  <div class="rp-input">
    <span class="rp-input__prefix">@</span>
    <input class="rp-input__control" type="text" id="Email" name="Email" value="" />
    <span class="rp-input__suffix">.com</span>
  </div>
  <div class="rp-hint">Your email address</div>
  <span class="rp-error" data-valmsg-for="Email"></span>
</div>
```

**CSS Classes**:
- `.rp-field`: Outer container (grid layout, 6px gap)
- `.rp-label`: Label element (font-weight: 600)
- `.rp-input`: Input wrapper (grid, auto-flow column)
- `.rp-input__control`: The actual input (min-height: 36px)
- `.rp-input__affix`: Prefix/suffix wrapper
- `.rp-hint`: Help text (color: muted, font-size: .9rem)
- `.rp-error`: Validation message (color: danger)

---

#### 2. rp-textarea (TextAreaTagHelper.cs)

**Purpose**: Multi-line text input with same field layout as rp-input.

**Properties**:
- `asp-for` (ModelExpression)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `rows` (int): Default 4
- `cols` (int?): Optional
- `placeholder` (string)

**Implementation Pattern**: Nearly identical to rp-input but uses `_generator.GenerateTextArea()`.

**CSS Classes**:
- `.rp-textarea__control`: min-height 120px, resize: vertical

---

#### 3. rp-select (SelectTagHelper.cs)

**Purpose**: Dropdown with async search, client filtering, and clearable options.

**Properties**:
- `asp-for` (ModelExpression)
- `items` (IEnumerable<SelectListItem>)
- `label` (string)
- `multiple` (bool)
- `clearable` (bool): Show clear button
- `filterable` (bool): Client-side filtering
- `placeholder` (string)
- `fetch-url` (string): URL for async search
- `search-param` (string): Query parameter (default: "q")
- `search-min` (int): Min chars to search (default: 2)
- `debounce-ms` (int): Debounce delay (default: 250)

**Implementation Highlights**:
```csharp
// Adds data attributes for JavaScript enhancement
var attrs = new Dictionary<string, object?>
{
    ["class"] = "rp-select__control",
    ["data-rp-select"] = "",
    ["aria-haspopup"] = "listbox"
};
if (Multiple) attrs["multiple"] = "multiple";
if (Clearable) attrs["data-rp-select-clearable"] = "true";
if (Filterable) attrs["data-rp-select-filterable"] = "true";
if (!string.IsNullOrWhiteSpace(FetchUrl))
{
    attrs["data-rp-select-fetch"] = FetchUrl;
    attrs["data-rp-select-search-param"] = SearchParam;
    attrs["data-rp-select-search-min"] = SearchMin.ToString();
    attrs["data-rp-select-debounce"] = DebounceMs.ToString();
}
if (Clearable || Filterable || !string.IsNullOrWhiteSpace(FetchUrl))
{
    attrs["data-rp-select-enhance"] = "true";
}
```

**JavaScript Enhancement** (razorplus.select.js):
```javascript
export function enhanceSelect(select) {
  const container = select.closest('[data-rp-select-container]');
  const filterable = select.dataset.rpSelectFilterable === 'true';
  const clearable = select.dataset.rpSelectClearable === 'true';
  const fetchUrl = select.dataset.rpSelectFetch;

  // Client-side filtering
  if (filterable) {
    input.addEventListener('input', () => applyFilter(select, input.value, placeholder));
  }

  // Async search with debounce
  if (fetchUrl) {
    const runSearch = async (value) => {
      const url = new URL(fetchUrl, window.location.origin);
      url.searchParams.set(searchParam, value);
      const response = await fetch(url.toString());
      const payload = await response.json();
      updateOptions(select, payload, { placeholder, clearable });
    };
    input.addEventListener('input', debounce(runSearch, debounceMs));
  }
}
```

**Server-Side Search Example**:
```csharp
// In PageModel
public JsonResult OnGetSearchCountries(string q)
{
    var results = _countries
        .Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
        .Take(10)
        .Select(c => new { value = c.Id, text = c.Name });
    return new JsonResult(results);
}
```

**CSS Classes**:
- `.rp-select`: Container
- `.rp-select__control`: Native select element
- `.rp-select__filter`: Filter input row
- `.rp-select__clear`: Clear button
- `.rp-select__status`: Search status text

---

#### 4. rp-switch (SwitchTagHelper.cs)

**Purpose**: Toggle switch for boolean properties with visual on/off states.

**Properties**:
- `asp-for` (ModelExpression)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `on-text` (string): Text when checked
- `off-text` (string): Text when unchecked

**Implementation**:
```csharp
var checkedValue = For?.Model as bool?;
var attributes = new Dictionary<string, object?>
{
    ["class"] = "rp-switch__control",
    ["role"] = "switch",
    ["aria-checked"] = checkedValue == true ? "true" : "false"
};
var checkbox = _generator.GenerateCheckBox(ViewContext, For?.ModelExplorer, For?.Name, isChecked: checkedValue, htmlAttributes: attributes);
```

**HTML Structure**:
```html
<div class="rp-field rp-switch">
  <div class="rp-label">Enable Notifications</div>
  <div class="rp-switch__track" data-rp-switch>
    <input type="checkbox" class="rp-switch__control" role="switch" aria-checked="false" />
    <span class="rp-switch__state" data-on="On" data-off="Off"></span>
    <span class="rp-switch__thumb"></span>
  </div>
</div>
```

**JavaScript (in core.js)**:
```javascript
function enhanceSwitches(root) {
  const tracks = root.querySelectorAll('[data-rp-switch]');
  tracks.forEach(track => {
    const input = track.querySelector('.rp-switch__control');
    const state = track.querySelector('.rp-switch__state');
    const update = () => {
      input.setAttribute('aria-checked', input.checked ? 'true' : 'false');
      track.classList.toggle('rp-switch__track--checked', input.checked);
    };
    input.addEventListener('change', update);
    update();
  });
}
```

**CSS**:
- `.rp-switch__track`: 48px × 26px, rounded pill
- `.rp-switch__thumb`: 20px circle, transitions left on check
- `.rp-switch__control`: Invisible checkbox overlay
- Transitions on background and transform (0.2s ease)

---

#### 5. rp-radio-group (RadioGroupTagHelper.cs)

**Purpose**: Accessible radio button group with fieldset/legend.

**Properties**:
- `asp-for` (ModelExpression)
- `items` (IEnumerable<SelectListItem>)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `layout` (string): "vertical" (default) or "horizontal"

**Implementation**:
```csharp
output.TagName = "fieldset";
output.Attributes.SetAttribute("class", $"rp-radio-group rp-radio-group--{Layout}");

// Generate each radio
foreach (var item in items)
{
    var id = $"{For?.Name ?? "rp-radio"}_{index++}";
    var radio = _generator.GenerateRadioButton(ViewContext, For?.ModelExplorer, For?.Name, item.Value, item.Selected, new { id, @class = "rp-radio__input" });

    output.Content.AppendHtml($"<label class=\"rp-radio\" for=\"{id}\">{inputHtml}<span class=\"rp-radio__label\">{enc.Encode(item.Text)}</span></label>");
}
```

**CSS**:
- `.rp-radio-group`: Grid layout with 8px gap
- `.rp-radio-group--horizontal`: grid-auto-flow: column
- `.rp-radio__input`: Native radio input (margin: 0)
- `.rp-radio__label`: Text label

---

#### 6. rp-validation-message (ValidationMessageTagHelper.cs)

**Purpose**: Standalone validation message display.

**Properties**:
- `asp-for` (ModelExpression)

**Simple Implementation**:
```csharp
public override void Process(TagHelperContext context, TagHelperOutput output)
{
    output.TagName = null; // render child only
    var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, null, null, new { @class = "rp-error" });
    output.Content.SetHtmlContent(validation);
}
```

---

### Structural Components

#### 7. rp-button (ButtonTagHelper.cs)

**Purpose**: Consistent button/link styling with variants, sizes, icons, and states.

**Properties**:
- `variant` (string): "primary", "secondary", "ghost", "danger" (default: "primary")
- `size` (string): "sm", "md", "lg" (default: "md")
- `icon` (string): Icon identifier (rendered as data attribute)
- `block` (bool): Full-width
- `loading` (bool): Loading state
- `as` (string): "button" or "a"
- `href` (string): Link URL when as="a"
- `disabled` (bool)
- `type` (string): "button", "submit", "reset"

**Implementation**:
```csharp
var tag = (As?.ToLowerInvariant() == "a") ? "a" : "button";
output.TagName = tag;

// Build classes
var cls = $"rp-btn rp-btn--{Variant} rp-btn--{Size}" + (Block ? " rp-btn--block" : "");

// Tag-specific attributes
if (tag == "a")
{
    if (!string.IsNullOrWhiteSpace(Href))
        output.Attributes.SetAttribute("href", Href);
    if (Disabled)
        output.Attributes.SetAttribute("aria-disabled", "true");
    output.Attributes.SetAttribute("role", "button");
}
else
{
    if (Disabled)
        output.Attributes.SetAttribute("disabled", "disabled");
    output.Attributes.SetAttribute("type", ButtonType);
}

// Icon + label structure
if (!string.IsNullOrWhiteSpace(Icon))
{
    output.Content.AppendHtml($"<span class=\"rp-btn__icon\" aria-hidden=\"true\" data-icon=\"{iconName}\"></span>");
}
output.Content.AppendHtml($"<span class=\"rp-btn__label\">{content.GetContent()}</span>");
```

**CSS Variants**:
- `.rp-btn--primary`: Blue background, white text
- `.rp-btn--secondary`: Gray background
- `.rp-btn--ghost`: Transparent with border
- `.rp-btn--danger`: Red background

**Sizes**:
- `--sm`: 28px min-height, .9rem font
- `--md`: 36px min-height
- `--lg`: 44px min-height, 1.05rem font

**Responsive** (mobile adds 8px height):
```css
@media (max-width: 640px) {
  .rp-btn { min-height: var(--rp-control-lg); }
}
```

---

#### 8. rp-tabs (TabsTagHelpers.cs)

**Purpose**: Tab navigation with ARIA roles and keyboard controls.

**Parent: rp-tabs**
- `id` (string): Container ID

**Child: rp-tab**
- `id` (string): Panel ID
- `header` (string): Tab button text
- `active` (bool): Initially active

**Implementation**:
```csharp
// rp-tab renders a section with template for header
output.TagName = "section";
output.Attributes.SetAttribute("class", Active ? "rp-tab rp-tab--active" : "rp-tab");
output.Attributes.SetAttribute("role", "tabpanel");
output.Attributes.SetAttribute("id", id);
output.Attributes.SetAttribute("aria-labelledby", $"tab-{id}");
output.PreElement.SetHtmlContent($"<template data-rp-tab-header id=\"tab-{id}\">{enc.Encode(header)}</template>");
```

**JavaScript Enhancement (core.js)**:
```javascript
function enhanceTabs(root) {
  const containers = root.querySelectorAll('[data-rp-tabs]');
  containers.forEach(c => {
    const panels = Array.from(c.querySelectorAll('.rp-tab'));

    // Build tablist from templates
    const list = document.createElement('div');
    list.setAttribute('role', 'tablist');
    list.className = 'rp-tablist';

    panels.forEach((p, i) => {
      const btn = document.createElement('button');
      btn.className = 'rp-btn rp-btn--ghost rp-btn--sm';
      btn.setAttribute('role', 'tab');
      btn.setAttribute('aria-controls', p.id);
      btn.setAttribute('tabindex', i === 0 ? '0' : '-1');
      btn.setAttribute('aria-selected', i === 0 ? 'true' : 'false');
      list.appendChild(btn);
    });

    c.prepend(list);

    // Keyboard navigation
    list.addEventListener('keydown', (e) => {
      if (e.key === 'ArrowRight') { /* move focus right */ }
      if (e.key === 'ArrowLeft') { /* move focus left */ }
      if (e.key === 'Home') { tabs[0].focus(); }
      if (e.key === 'End') { tabs[tabs.length-1].focus(); }
    });

    // Click activation
    list.addEventListener('click', (e) => {
      const btn = e.target.closest('[role=tab]');
      if (!btn) return;
      // Update aria-selected, show/hide panels
    });
  });
}
```

**Features**:
- Roving tabindex
- Arrow key navigation
- Home/End keys
- Auto-generated tablist from server markup

---

#### 9. rp-accordion (AccordionTagHelper.cs)

**Purpose**: Collapsible content sections.

**Parent: rp-accordion**
- `id` (string)

**Child: rp-accordion-item**
- `id` (string)
- `header` (string): Trigger button text
- `expanded` (bool): Initially open

**Implementation**:
```csharp
// rp-accordion-item
var buttonId = $"{baseId}-trigger";
var panelId = $"{baseId}-panel";

var trigger = $"<button type=\"button\" class=\"rp-accordion__trigger\" id=\"{buttonId}\" aria-expanded=\"{expandedAttr}\" aria-controls=\"{panelId}\" data-rp-accordion-trigger>{enc.Encode(headerText)}</button>";

var panel = $"<div class=\"rp-accordion__panel\" id=\"{panelId}\" role=\"region\" aria-labelledby=\"{buttonId}\"{hiddenAttr} data-rp-accordion-panel>{content.GetContent()}</div>";
```

**JavaScript (core.js)**:
```javascript
function enhanceAccordion(root) {
  const accordions = root.querySelectorAll('[data-rp-accordion]');
  accordions.forEach(acc => {
    acc.addEventListener('click', (event) => {
      const trigger = event.target.closest('[data-rp-accordion-trigger]');
      if (!trigger) return;
      toggleAccordion(trigger);
    });

    acc.addEventListener('keydown', (event) => {
      if (event.key !== 'Enter' && event.key !== ' ') return;
      toggleAccordion(trigger);
      event.preventDefault();
    });
  });
}

function toggleAccordion(trigger, force) {
  const expanded = force !== undefined ? force : trigger.getAttribute('aria-expanded') !== 'true';
  const panel = trigger.parentElement?.querySelector('[data-rp-accordion-panel]');
  trigger.setAttribute('aria-expanded', expanded ? 'true' : 'false');
  if (panel) {
    panel.toggleAttribute('hidden', !expanded);
  }
}
```

**CSS**:
- `.rp-accordion`: 1px border, rounded
- `.rp-accordion__item + .rp-accordion__item`: top border
- `.rp-accordion__trigger`: Full-width button, 14px padding
- `.rp-accordion__panel`: 12px padding

---

### Data Display Components

#### 10. rp-table (TableTagHelpers.cs)

**Purpose**: Sortable, pageable table with server or client-side behavior.

**Parent: rp-table**
- `items` (IEnumerable<object>)
- `sortable` (bool)
- `pageable` (bool)
- `client` (bool): Client-side sorting/paging
- `page-size` (int): Default 25
- `page` (int): Default 1
- `total-items` (int)
- `sort-param` (string): Default "sort"
- `direction-param` (string): Default "dir"
- `page-param` (string): Default "page"
- `empty-text` (string)
- `key-selector` (string): Property for row keys

**Child: rp-column**
- `for` (string): Property name
- `header` (string)
- `width` (string): CSS width
- `align` (string): "left", "center", "right"
- `sortable` (bool)
- `sort-key` (string)
- `template` (Func<object, object?>): Custom renderer

**Column Definition Pattern**:
```csharp
public List<ColumnDefinition> Columns { get; } = new();

public override void Init(TagHelperContext context)
{
    context.Items[typeof(TableTagHelper)] = this;
}

// Child rp-column registers itself
public override void Init(TagHelperContext context)
{
    if (context.Items.TryGetValue(typeof(TableTagHelper), out var parent) && parent is TableTagHelper table)
    {
        var definition = new ColumnDefinition
        {
            For = For,
            Header = Header,
            Sortable = Sortable,
            SortKey = !string.IsNullOrWhiteSpace(SortKey) ? SortKey : For,
            Template = Template
        };
        table.Columns.Add(definition);
    }
}
```

**Server-Side Sorting**:
```csharp
// In TagHelper
var currentSort = GetQueryValue(currentQuery, SortParam);
var currentDir = GetQueryValue(currentQuery, DirectionParam) ?? "asc";

foreach (var c in Columns)
{
    if (Sortable && c.EnableSort && !string.IsNullOrWhiteSpace(c.SortKey))
    {
        var isActive = string.Equals(currentSort, c.SortKey, StringComparison.OrdinalIgnoreCase);
        var nextDir = isActive && currentDir == "asc" ? "desc" : "asc";
        var queryDict = new Dictionary<string, string?>();
        queryDict[SortParam] = c.SortKey;
        queryDict[DirectionParam] = nextDir;
        queryDict[PageParam] = "1";

        var sortUrl = QueryHelpers.AddQueryString(basePath, queryDict);
        var link = new TagBuilder("a");
        link.Attributes["href"] = sortUrl;
        link.AddCssClass("rp-table__sort");
        if (isActive) link.AddCssClass($"rp-table__sort--{currentDir}");
    }
}
```

**Client-Side Table (razorplus.table.js)**:
```javascript
export function enhanceClientTable(table) {
  const tbody = table.querySelector('tbody');
  const rows = Array.from(tbody.querySelectorAll('tr'));

  const state = {
    tbody,
    rows,
    originalOrder: rows.slice(),
    sortKey: null,
    sortDir: 'asc',
    page: parseInt(table.dataset.rpPage || '1', 10),
    pageSize: parseInt(table.dataset.rpPageSize || rows.length, 10),
    pageable: table.dataset.rpTablePageable === 'true'
  };

  // Add click handlers to sortable headers
  const headers = table.querySelectorAll('th[data-rp-sortable]');
  headers.forEach(header => {
    const button = header.querySelector('.rp-table__sort');
    button.addEventListener('click', () => {
      const key = header.dataset.rpSortKey;
      toggleSort(table, key);
    });
  });

  // Build client pagination controls
  if (state.pageable) {
    const nav = buildPaginationNav(table);
    table.after(nav);
  }
}

function toggleSort(table, key) {
  const state = tableState.get(table);
  if (state.sortKey === key) {
    state.sortDir = state.sortDir === 'asc' ? 'desc' : 'asc';
  } else {
    state.sortKey = key;
    state.sortDir = 'asc';
  }
  state.page = 1;
  render(table);
}

function render(table) {
  const state = tableState.get(table);
  let rows = state.originalOrder.slice();

  // Sort
  if (state.sortKey) {
    const columnIndex = getColumnIndex(table, state.sortKey);
    rows.sort((a, b) => compareCells(a, b, columnIndex, state.sortDir));
  }

  // Paginate
  const start = (state.page - 1) * state.pageSize;
  const end = start + state.pageSize;
  const slice = rows.slice(start, end);

  // Update DOM
  state.tbody.innerHTML = '';
  slice.forEach(row => state.tbody.appendChild(row));

  updateSortClasses(table, state);
}
```

**PageModel Pattern**:
```csharp
[BindProperty(SupportsGet = true)]
public string? Sort { get; set; }

[BindProperty(SupportsGet = true)]
public string? Dir { get; set; }

[BindProperty(SupportsGet = true)]
public int Page { get; set; } = 1;

public void OnGet()
{
    var query = _context.Customers.AsQueryable();

    // Apply sorting
    query = (Sort?.ToLower(), Dir?.ToLower()) switch
    {
        ("name", "desc") => query.OrderByDescending(c => c.Name),
        ("name", _) => query.OrderBy(c => c.Name),
        ("email", "desc") => query.OrderByDescending(c => c.Email),
        ("email", _) => query.OrderBy(c => c.Email),
        _ => query.OrderBy(c => c.CreatedAt)
    };

    TotalItems = query.Count();
    Customers = query
        .Skip((Page - 1) * PageSize)
        .Take(PageSize)
        .ToList();
}
```

---

#### 11. rp-pagination (PaginationTagHelper.cs)

**Purpose**: Pagination controls with query string preservation.

**Properties**:
- `page` (int): Current page
- `page-size` (int): Items per page
- `total-items` (int): Total count
- `page-param` (string): Query param name (default: "page")

**Implementation**:
```csharp
public override void Process(TagHelperContext context, TagHelperOutput output)
{
    var totalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
    if (totalPages <= 1)
    {
        output.SuppressOutput();
        return;
    }

    // Preserve existing query params
    var existing = new Dictionary<string, string?>();
    if (request?.Query != null)
    {
        foreach (var kv in request.Query)
        {
            existing[kv.Key] = kv.Value.ToString();
        }
    }

    string BuildUrl(int page)
    {
        var snapshot = new Dictionary<string, string?>(existing);
        snapshot[PageParam] = page.ToString();
        return QueryHelpers.AddQueryString(basePath, snapshot);
    }

    // Build pagination with window
    const int window = 2;
    var start = Math.Max(1, Page - window);
    var end = Math.Min(totalPages, Page + window);

    // Prev
    AppendItem("Prev", Math.Max(1, Page - 1), Page <= 1, false, "prev");

    // First + ellipsis
    if (start > 1)
    {
        AppendItem("1", 1, false, Page == 1, "nofollow");
        if (start > 2)
        {
            list.Append("<li class=\"rp-pagination__ellipsis\">&hellip;</li>");
        }
    }

    // Window
    for (var i = start; i <= end; i++)
    {
        AppendItem(i.ToString(), i, false, i == Page, "nofollow");
    }

    // Last + ellipsis
    if (end < totalPages)
    {
        if (end < totalPages - 1)
        {
            list.Append("<li class=\"rp-pagination__ellipsis\">&hellip;</li>");
        }
        AppendItem(totalPages.ToString(), totalPages, false, Page == totalPages, "nofollow");
    }

    // Next
    AppendItem("Next", Math.Min(totalPages, Page + 1), Page >= totalPages, false, "next");
}
```

**Features**:
- Window of ±2 pages
- First/Last links with ellipsis
- Preserves all query params (sort, filters, etc.)
- Auto-hides when only 1 page

---

#### 12. rp-chart (ChartTagHelper.cs)

**Purpose**: ECharts integration with theme support.

**Properties**:
- `id` (string)
- `type` (string): "line", "bar", "pie", etc. (default: "line")
- `data` (object): Chart data
- `options` (object): ECharts config
- `height` (int): Default 280
- `theme` (string): "auto", "light", "dark"
- `export` (string): "png" or "svg"
- `defer` (bool): Lazy load

**Implementation**:
```csharp
public override void Process(TagHelperContext context, TagHelperOutput output)
{
    output.TagName = "div";
    output.Attributes.SetAttribute("class", "rp-chart");
    var id = string.IsNullOrWhiteSpace(Id) ? $"rp-chart-{Guid.NewGuid():N}" : Id!;
    output.Attributes.SetAttribute("id", id);
    output.Attributes.SetAttribute("data-rp-chart", "");
    output.Attributes.SetAttribute("data-rp-chart-type", Type);
    output.Attributes.SetAttribute("data-rp-chart-theme", Theme);
    output.Attributes.SetAttribute("style", $"height:{Height}px");

    // Embed data as JSON script
    var payload = new { data = Data, options = Options };
    var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    output.Content.SetHtmlContent($"<script type=\"application/json\" class=\"rp-chart__data\">{json}</script>");
}
```

**JavaScript (razorplus.chart.js)**:
```javascript
let echartsPromise;
function ensureECharts() {
  if (window.echarts) return Promise.resolve(window.echarts);
  if (!echartsPromise) {
    echartsPromise = new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = 'https://cdn.jsdelivr.net/npm/echarts@5/dist/echarts.min.js';
      script.async = true;
      script.onload = () => resolve(window.echarts);
      script.onerror = reject;
      document.head.appendChild(script);
    });
  }
  return echartsPromise;
}

export function mountChart(element) {
  const payloadNode = element.querySelector('.rp-chart__data');
  const payload = JSON.parse(payloadNode.textContent || '{}');

  ensureECharts().then(echarts => {
    const themeChoice = resolveTheme(element.dataset.rpChartTheme || 'auto');
    const instance = echarts.init(element, themeChoice.theme);

    const option = Object.assign({}, payload.options || {});
    if (payload.data) {
      if (payload.data.series) option.series = payload.data.series;
      if (payload.data.xAxis) option.xAxis = payload.data.xAxis;
      if (payload.data.yAxis) option.yAxis = payload.data.yAxis;
    }

    instance.setOption(option);
    chartRegistry.set(element, { instance, payload, themeChoice });

    // Auto-resize
    const observer = new ResizeObserver(() => instance.resize());
    observer.observe(element);

    // Theme switching for "auto" mode
    if (themeChoice.auto) {
      const media = window.matchMedia('(prefers-color-scheme: dark)');
      media.addEventListener('change', () => {
        // Reinitialize with new theme
      });
    }
  });
}
```

**Usage in PageModel**:
```csharp
public object ChartData => new
{
    xAxis = new { type = "category", data = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" } },
    yAxis = new { type = "value" },
    series = new[]
    {
        new { name = "Revenue", type = "line", smooth = true, data = new[] { 120, 200, 180, 220, 260, 300 } }
    }
};

public object ChartOptions => new
{
    tooltip = new { trigger = "axis" },
    legend = new { data = new[] { "Revenue" } },
    grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true }
};
```

---

### Overlay Components

#### 13. rp-modal (ModalTagHelper.cs)

**Purpose**: Modal dialog with focus trap, backdrop, and sections.

**Parent: rp-modal**
- `id` (string)
- `title` (string)
- `open` (bool): Initially open
- `static-backdrop` (bool): Prevent backdrop close

**Children**:
- `rp-modal-header`: Custom header
- `rp-modal-body`: Main content
- `rp-modal-footer`: Action buttons

**Implementation**:
```csharp
public class ModalSections
{
    public TagHelperContent? Header { get; set; }
    public TagHelperContent? Body { get; set; }
    public TagHelperContent? Footer { get; set; }
}

// Parent modal
public override void Init(TagHelperContext context)
{
    var modalContext = new ModalSections();
    context.Items[typeof(ModalSections)] = modalContext;
}

public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
{
    await output.GetChildContentAsync(); // Let children register

    var sections = context.Items[typeof(ModalSections)] as ModalSections ?? new();

    // Header with close button
    var header = sections.Header != null
        ? $"<header class=\"rp-modal__header\">{sections.Header.GetContent()}<button type=\"button\" class=\"rp-modal__close\" data-rp-modal-close>×</button></header>"
        : $"<header class=\"rp-modal__header\"><h2 class=\"rp-modal__title\">{enc.Encode(titleText)}</h2><button type=\"button\" class=\"rp-modal__close\" data-rp-modal-close>×</button></header>";

    var body = $"<div class=\"rp-modal__body\">{(sections.Body ?? content).GetContent()}</div>";
    var footer = sections.Footer != null ? $"<footer class=\"rp-modal__footer\">{sections.Footer.GetContent()}</footer>" : "";

    var dialog = $"<div class=\"rp-modal__dialog\" role=\"dialog\" aria-modal=\"true\" aria-labelledby=\"{titleId}\" tabindex=\"-1\">{header}{body}{footer}</div>";
    var overlay = $"<div class=\"rp-modal__overlay\" data-rp-modal-overlay></div>";

    output.Content.SetHtmlContent(overlay + dialog);
}

// Child sections just store content
[HtmlTargetElement("rp-modal-header", ParentTag = "rp-modal")]
public class ModalHeaderTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items[typeof(ModalSections)] is ModalSections sections)
        {
            sections.Header = await output.GetChildContentAsync();
        }
        output.SuppressOutput();
    }
}
```

**JavaScript (core.js)**:
```javascript
const focusableSelector = 'a[href], button:not([disabled]), input:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])';
const modalState = new WeakMap();
let modalOpenCount = 0;

function enhanceModals(root) {
  const modals = root.querySelectorAll('[data-rp-modal]');
  modals.forEach(modal => {
    const dialog = modal.querySelector('[role="dialog"]');
    const overlay = modal.querySelector('[data-rp-modal-overlay]');
    const closeEls = modal.querySelectorAll('[data-rp-modal-close]');

    const state = { lastActive: null, trapHandler: null, escHandler: null };
    modalState.set(modal, state);

    const close = () => closeModalElement(modal);
    closeEls.forEach(btn => btn.addEventListener('click', close));

    if (overlay && modal.dataset.rpModalStatic !== 'true') {
      overlay.addEventListener('click', close);
    }

    // Escape key handler
    const handleEsc = (event) => {
      if (event.key === 'Escape' && modal.dataset.rpModalStatic !== 'true') {
        close();
      }
    };

    // Focus trap
    const trapFocus = (event) => {
      if (event.key !== 'Tab') return;
      const focusable = Array.from(dialog.querySelectorAll(focusableSelector))
        .filter(el => el.offsetParent !== null);
      if (focusable.length === 0) {
        event.preventDefault();
        return;
      }
      const first = focusable[0];
      const last = focusable[focusable.length - 1];
      if (!event.shiftKey && document.activeElement === last) {
        event.preventDefault();
        first.focus();
      } else if (event.shiftKey && document.activeElement === first) {
        event.preventDefault();
        last.focus();
      }
    };

    state.escHandler = handleEsc;
    state.trapHandler = trapFocus;
  });
}

function openModalElement(modal, focusDialog = true) {
  const state = modalState.get(modal);
  state.lastActive = document.activeElement;

  modal.removeAttribute('hidden');
  modal.classList.add('rp-modal--open');
  modal.dataset.rpOpen = 'true';

  lockBodyScroll();
  document.addEventListener('keydown', state.escHandler);
  modal.addEventListener('keydown', state.trapHandler);

  if (focusDialog) {
    requestAnimationFrame(() => {
      const focusable = dialog.querySelector(focusableSelector);
      (focusable || dialog).focus();
    });
  }
}

function closeModalElement(modal) {
  const state = modalState.get(modal);
  modal.classList.remove('rp-modal--open');
  modal.dataset.rpOpen = 'false';
  modal.setAttribute('hidden', 'hidden');

  document.removeEventListener('keydown', state.escHandler);
  modal.removeEventListener('keydown', state.trapHandler);

  unlockBodyScroll();
  state.lastActive?.focus();
}

// Global API
export function openModal(id) {
  const modal = document.getElementById(id);
  if (modal?.matches('[data-rp-modal]')) {
    openModalElement(modal);
  }
}

export function closeModal(id) {
  const modal = document.getElementById(id);
  if (modal?.matches('[data-rp-modal]')) {
    closeModalElement(modal);
  }
}
```

**Features**:
- Focus trap (Tab/Shift+Tab cycles within modal)
- Escape key closes (unless static-backdrop)
- Backdrop click closes (unless static-backdrop)
- Body scroll lock
- Returns focus to trigger element on close
- ARIA role="dialog" and aria-modal="true"

---

## Theming System

### CSS Custom Properties

Located in `src/RazorPlus/wwwroot/css/razorplus.css`:

```css
:root {
  /* Colors */
  --rp-bg: #ffffff;
  --rp-fg: #111827;
  --rp-primary: #2563eb;
  --rp-danger: #dc2626;
  --rp-muted: #6b7280;

  /* Geometry */
  --rp-radius: 8px;
  --rp-gap: 8px;
  --rp-shadow: 0 1px 2px rgba(0,0,0,.06), 0 1px 3px rgba(0,0,0,.1);
  --rp-focus: 2px solid #2563eb66;

  /* Typography */
  --rp-font: system-ui, -apple-system, Segoe UI, Roboto, Helvetica, Arial, sans-serif;

  /* Control Sizes */
  --rp-control-sm: 28px;
  --rp-control-md: 36px;
  --rp-control-lg: 44px;
}

[data-theme="dark"] {
  --rp-bg: #0b0f18;
  --rp-fg: #e5e7eb;
  --rp-primary: #2563eb;
  --rp-danger: #f87171;
  --rp-muted: #9ca3af;
  --rp-shadow: 0 1px 2px rgba(0,0,0,.5), 0 1px 3px rgba(0,0,0,.6);
}
```

### Dark Mode Toggle

```javascript
// Toggle between light/dark
document.documentElement.setAttribute('data-theme',
  document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark');
```

### Responsive Breakpoints

```css
@media (max-width: 640px) {
  .rp-btn { min-height: var(--rp-control-lg); padding: 0 18px; }
  .rp-input__control { min-height: var(--rp-control-lg); }
  .rp-switch__track { width: 56px; height: 30px; }
  .rp-modal__dialog { width: min(96%, 480px); padding: 16px; }
  .rp-radio-group--horizontal { grid-auto-flow: row; }
}
```

### Utility Classes

```css
.rp-sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border: 0;
}
```

---

## JavaScript Architecture

### Core Module (razorplus.core.js)

**Entry Point**: Auto-initializes on DOM ready

```javascript
export function init(root = document) {
  enhanceTabs(root);
  enhanceSwitches(root);
  enhanceAccordion(root);
  enhanceModals(root);
  enhanceSelects(root);
  enhanceTables(root);
  enhanceCharts(root);
}

// Auto-init
if (document.readyState !== 'loading') init();
else document.addEventListener('DOMContentLoaded', () => init());
```

**Global API**:
```javascript
if (typeof window !== 'undefined') {
  window.RazorPlus = window.RazorPlus || {};
  Object.assign(window.RazorPlus, {
    init,
    openModal,
    closeModal,
    refresh: (root = document) => init(root)
  });
}
```

**Lazy Module Loading**:
```javascript
let selectModulePromise;
function ensureSelectModule() {
  if (!selectModulePromise) {
    selectModulePromise = import('./razorplus.select.js');
  }
  return selectModulePromise;
}

function enhanceSelects(root) {
  const selects = root.querySelectorAll('select[data-rp-select][data-rp-select-enhance]');
  if (!selects.length) return;
  ensureSelectModule().then(mod => {
    selects.forEach(select => {
      if (select.dataset.enhanced) return;
      mod.enhanceSelect(select);
    });
  }).catch(err => console.error('RazorPlus select enhancement failed', err));
}
```

### Patterns

1. **Idempotent Enhancement**: Check `dataset.enhanced` to prevent double-initialization
2. **WeakMap State**: Store component state without memory leaks
3. **Event Delegation**: Listen on container, filter by selector
4. **Progressive Enhancement**: Components work without JS, enhanced when available
5. **Module Loading**: Dynamic imports only when features are used

---

## Development Workflow

### Setting Up a New Project

1. **Add Package Reference**:
```xml
<ItemGroup>
  <ProjectReference Include="..\RazorPlus\RazorPlus.csproj" />
</ItemGroup>
```

2. **Register TagHelpers** in `_ViewImports.cshtml`:
```razor
@addTagHelper *, RazorPlus
```

3. **Include Assets** in `_Layout.cshtml`:
```html
<head>
  <link rel="stylesheet" href="_content/RazorPlus/css/razorplus.css" />
</head>
<body>
  <!-- content -->
  <script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.select.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.table.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.chart.js" type="module"></script>
</body>
```

4. **Optional: Add Theme Toggle**:
```html
<button onclick="document.documentElement.setAttribute('data-theme', document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark')">
  Toggle Theme
</button>
```

### Common Patterns

#### Form with Validation

```razor
@page
@model MyFormModel

<form method="post">
  <rp-input asp-for="Name" label="Full Name" required="true" hint="Your legal name" />
  <rp-select asp-for="CountryId" items="Model.Countries" label="Country" clearable="true" />
  <rp-textarea asp-for="Bio" label="Bio" rows="5" />
  <rp-switch asp-for="AcceptTerms" label="Accept Terms" />

  <div asp-validation-summary="ModelOnly" class="rp-error"></div>

  <rp-button variant="primary" type="submit">Submit</rp-button>
</form>
```

```csharp
public class MyFormModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        // Process form
        TempData["success"] = "Form submitted successfully!";
        return RedirectToPage();
    }
}

public class InputModel
{
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    public int CountryId { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms.")]
    public bool AcceptTerms { get; set; }
}
```

#### Sortable/Pageable Table

```razor
<rp-table items="Model.Customers"
          sortable="true"
          pageable="true"
          page-size="25"
          page="@Model.Page"
          total-items="@Model.TotalCustomers"
          key-selector="Id">
  <rp-column for="Name" header="Customer" sortable="true" />
  <rp-column for="Email" header="Email" sortable="true" />
  <rp-column for="CreatedAt"
             header="Joined"
             sortable="true"
             template="@(c => ((Customer)c).CreatedAt.ToString("MM/dd/yyyy"))" />
  <rp-column for="TotalSpent"
             header="Total Spent"
             align="right"
             template="@(c => ((Customer)c).TotalSpent.ToString("C"))" />
</rp-table>

<rp-pagination page="@Model.Page"
               page-size="25"
               total-items="@Model.TotalCustomers" />
```

```csharp
public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Sort { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Dir { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Page { get; set; } = 1;

    public int TotalCustomers { get; set; }
    public List<Customer> Customers { get; set; } = new();

    public void OnGet()
    {
        var query = _context.Customers.AsQueryable();

        query = (Sort?.ToLower(), Dir?.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(c => c.Name),
            ("name", _) => query.OrderBy(c => c.Name),
            ("email", "desc") => query.OrderByDescending(c => c.Email),
            ("email", _) => query.OrderBy(c => c.Email),
            ("createdat", "desc") => query.OrderByDescending(c => c.CreatedAt),
            ("createdat", _) => query.OrderBy(c => c.CreatedAt),
            ("totalspent", "desc") => query.OrderByDescending(c => c.TotalSpent),
            ("totalspent", _) => query.OrderBy(c => c.TotalSpent),
            _ => query.OrderBy(c => c.Name)
        };

        TotalCustomers = query.Count();
        Customers = query
            .Skip((Page - 1) * 25)
            .Take(25)
            .ToList();
    }
}
```

#### Modal Dialog

```razor
<rp-button variant="primary" onclick="RazorPlus.openModal('confirm-delete')">
  Delete Account
</rp-button>

<rp-modal id="confirm-delete" title="Confirm Deletion" static-backdrop="true">
  <rp-modal-body>
    <p>Are you sure you want to delete your account? This action cannot be undone.</p>
  </rp-modal-body>
  <rp-modal-footer>
    <rp-button variant="ghost" onclick="RazorPlus.closeModal('confirm-delete')">
      Cancel
    </rp-button>
    <form method="post" asp-page-handler="DeleteAccount" style="display: inline;">
      <rp-button variant="danger" type="submit">
        Delete Forever
      </rp-button>
    </form>
  </rp-modal-footer>
</rp-modal>
```

#### Async Search Select

```razor
<rp-select asp-for="UserId"
           label="Assign User"
           items="Model.InitialUsers"
           clearable="true"
           filterable="true"
           fetch-url="@Url.Page(null, "SearchUsers")"
           search-param="q"
           search-min="2"
           debounce-ms="300"
           placeholder="Search users..." />
```

```csharp
public JsonResult OnGetSearchUsers(string q)
{
    var term = q?.Trim() ?? "";
    var users = _context.Users
        .Where(u => u.Name.Contains(term) || u.Email.Contains(term))
        .Take(10)
        .Select(u => new { value = u.Id, text = u.Name })
        .ToList();
    return new JsonResult(users);
}
```

---

## Testing

### Unit Test Structure

Located in `tests/RazorPlus.Tests/`

**Example Test** (ButtonTagHelperTests.cs):
```csharp
[Fact]
public async Task Button_WithPrimaryVariant_RendersCorrectClasses()
{
    var helper = new ButtonTagHelper { Variant = "primary", Size = "md" };
    var context = new TagHelperContext(
        new TagHelperAttributeList(),
        new Dictionary<object, object>(),
        Guid.NewGuid().ToString());
    var output = new TagHelperOutput("rp-button",
        new TagHelperAttributeList(),
        (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    output.Content.SetContent("Click me");

    await helper.ProcessAsync(context, output);

    Assert.Equal("button", output.TagName);
    Assert.Contains("rp-btn rp-btn--primary rp-btn--md", output.Attributes["class"]?.Value?.ToString());
}

[Fact]
public async Task Button_AsLink_RendersAnchorTag()
{
    var helper = new ButtonTagHelper { As = "a", Href = "/dashboard" };
    var context = new TagHelperContext(
        new TagHelperAttributeList(),
        new Dictionary<object, object>(),
        Guid.NewGuid().ToString());
    var output = new TagHelperOutput("rp-button",
        new TagHelperAttributeList(),
        (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    output.Content.SetContent("Go");

    await helper.ProcessAsync(context, output);

    Assert.Equal("a", output.TagName);
    Assert.Equal("/dashboard", output.Attributes["href"]?.Value);
    Assert.Equal("button", output.Attributes["role"]?.Value);
}
```

### Running Tests

```bash
cd tests/RazorPlus.Tests
dotnet test
```

---

## Sample Application

### Documentation Site (samples/RazorPlus.Docs)

**Program.cs**:
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
```

**Index.cshtml.cs**:
```csharp
public class IndexModel : PageModel
{
    [BindProperty]
    public DemoInput InputModel { get; set; } = new();

    public IEnumerable<SelectListItem> Roles { get; private set; }
    public IEnumerable<Customer> PagedCustomers { get; private set; }
    public PagerModel Pager { get; private set; }

    public object ChartData => new
    {
        xAxis = new { type = "category", data = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" } },
        yAxis = new { type = "value" },
        series = new[] { new { name = "Revenue", type = "line", smooth = true, data = new[] { 120, 200, 180, 220, 260, 300 } } }
    };

    public void OnGet(int? page, string? sort, string? dir)
    {
        Roles = GetRoles();
        var customers = GetCustomers();
        var sorted = SortCustomers(customers, sort ?? "name", dir ?? "asc");
        var totalItems = sorted.Count();
        var currentPage = Math.Max(1, page ?? 1);
        PagedCustomers = sorted.Skip((currentPage - 1) * 8).Take(8).ToList();
        Pager = new PagerModel(totalItems, currentPage, 8, sort ?? "name", dir ?? "asc");
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        TempData["ok"] = $"Saved {InputModel.Name} as {InputModel.Role}";
        return RedirectToPage();
    }

    public JsonResult OnGetRoleSearch(string q)
    {
        var roles = GetRoles()
            .Where(r => r.Text?.ToLowerInvariant().Contains(q?.ToLowerInvariant() ?? "") ?? false)
            .Select(r => new { value = r.Value, text = r.Text })
            .ToList();
        return new JsonResult(roles);
    }

    private static IEnumerable<SelectListItem> GetRoles() => new[]
    {
        new SelectListItem("Admin", "admin"),
        new SelectListItem("Editor", "editor"),
        new SelectListItem("Viewer", "viewer")
    };

    private static IEnumerable<Customer> GetCustomers() => new[]
    {
        new Customer("Ada Lovelace", "ada@example.com", "Analytical Engines", new DateTime(2019, 6, 1), 128_000m),
        new Customer("Alan Turing", "alan@example.com", "Enigma Labs", new DateTime(2020, 3, 14), 164_500m),
        // ... more customers
    };
}

public class DemoInput
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Role { get; set; }
    [Required]
    [StringLength(280, MinimumLength = 10)]
    public string? Bio { get; set; }
    [Required]
    public string? AccessLevel { get; set; }
    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the terms.")]
    public bool AcceptTerms { get; set; }
}

public record Customer(string Name, string Email, string Company, DateTime Joined, decimal LifetimeValue);
public record PagerModel(int TotalItems, int Page, int PageSize, string Sort, string Direction);
```

---

## Deployment

### NuGet Package (Future)

**RazorPlus.csproj** is configured for packaging:
```xml
<PropertyGroup>
  <TargetFrameworks>net6.0;net7.0</TargetFrameworks>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
  <GenerateRazorMetadata>true</GenerateRazorMetadata>
  <AddRazorSupportForMvc>true</AddRazorSupportForMvc>
  <IncludeRazorContentInPack>true</IncludeRazorContentInPack>
</PropertyGroup>
```

**Build Package**:
```bash
dotnet pack src/RazorPlus/RazorPlus.csproj -c Release
```

**Install from NuGet** (when published):
```bash
dotnet add package RazorPlus
```

---

## Architecture Decisions

### Why TagHelpers?

1. **Server-First**: HTML rendered on server = better SEO, faster initial load
2. **Type Safety**: Compile-time checking of properties and model binding
3. **IntelliSense**: Full IDE support for attributes
4. **ASP.NET Integration**: Native validation, model binding, IHtmlGenerator
5. **No Build Step**: No npm, webpack, or compilation required

### Why Minimal JavaScript?

1. **Performance**: Less to download and parse
2. **Reliability**: Components work without JS (progressive enhancement)
3. **Simplicity**: Easier to understand and customize
4. **Accessibility**: Server-rendered HTML is accessible by default

### Why CSS Variables?

1. **Runtime Theming**: Switch themes without rebuilding CSS
2. **Browser Support**: Supported in all modern browsers
3. **Simplicity**: No preprocessor required
4. **Customization**: Users can override tokens easily

### Why .NET 6/7 Multi-Targeting?

1. **Compatibility**: Support both LTS (.NET 6) and current (.NET 7)
2. **Migration Path**: Users can upgrade at their own pace
3. **No Breaking Changes**: Same API surface for both versions

---

## Roadmap

### Current State (Stage 1-3 Complete)

**Components**: 12 production-ready components
- Form: Input, Textarea, Select, Switch, RadioGroup, ValidationMessage
- Structural: Button, Tabs, Accordion
- Data: Table, Pagination, Chart
- Overlay: Modal

**Features**:
- Async select search
- Server/client table sorting
- Pagination with query preservation
- ECharts integration
- Focus trapping
- Dark mode
- Responsive design
- Full accessibility

**Status**: ~35% of full library vision

---

### Future Phases

#### Phase 3 (Next 2-3 months)
- File Upload (drag-and-drop, preview)
- Date Picker (calendar widget)
- Time Picker (hours/minutes/AM-PM)
- Toast Notifications (success/error/info)
- Alert/Banner (persistent messages)
- Breadcrumbs (navigation trail)
- Dropdown Menu (nested menus)
- Badge (status indicators)
- Tooltip (hover help)
- Progress Bar (linear indicator)

#### Phase 4 (3-4 months)
- Advanced Data Grid (column resize, filtering, Excel-like)
- Drawer (slide-in panel)
- Command Palette (⌘K style search)
- Stepper/Wizard (multi-step forms)
- Tree View (hierarchical data)
- Timeline (chronological events)
- Carousel (image slider)
- Popover (click-triggered content)
- Empty State (no-data placeholder)
- Card (content container)

#### Phase 5 (4-6 months)
- CLI Tool (scaffold components)
- VS Code Extension (IntelliSense for TagHelpers)
- Design Tokens (Figma integration)
- Virtual Scroll (large lists)
- Drag & Drop (sortable lists, kanban)
- Rich Text Editor (WYSIWYG)
- Autocomplete (suggestion dropdown)
- Tags Input (multi-value chips)
- Color Picker
- Range Slider

---

## Key Takeaways for AI Assistants

### When Working with RazorPlus:

1. **Server-First Mindset**: Components render HTML on the server, JavaScript only enhances
2. **TagHelper Patterns**: Use `IHtmlGenerator` for native ASP.NET integration
3. **Progressive Enhancement**: Ensure components work without JavaScript
4. **Accessibility**: ARIA attributes, keyboard navigation, semantic HTML are mandatory
5. **Idempotent JS**: Check `dataset.enhanced` before initializing
6. **WeakMap State**: Store component state without leaking memory
7. **CSS Variables**: Use tokens for theming, avoid hardcoded colors
8. **Query Preservation**: Pagination and sorting must preserve existing query params
9. **Model Binding**: Use `ModelExpression` for forms, leverage validation
10. **Lazy Loading**: Load heavy modules (charts, advanced features) only when needed

### Common User Requests:

- **"Add a form"**: Use rp-input, rp-select, rp-textarea with validation
- **"Make it sortable"**: Use rp-table with sortable="true" and server-side sorting
- **"Add pagination"**: Use rp-pagination with PageModel pattern
- **"Add a modal"**: Use rp-modal with focus trap and sections
- **"Make it searchable"**: Use rp-select with fetch-url for async search
- **"Add dark mode"**: Already built-in via `[data-theme="dark"]`

### File Locations:

- **TagHelpers**: `src/RazorPlus/TagHelpers/*.cs`
- **CSS**: `src/RazorPlus/wwwroot/css/razorplus.css`
- **JavaScript**: `src/RazorPlus/wwwroot/js/*.js`
- **Tests**: `tests/RazorPlus.Tests/*Tests.cs`
- **Docs Site**: `samples/RazorPlus.Docs/Pages/Index.cshtml`

---

## Summary

RazorPlus is a **production-ready, server-first UI library** for ASP.NET Core Razor Pages and MVC. With 12 accessible components, minimal JavaScript, token-based theming, and comprehensive documentation, it provides a solid foundation for modern web applications without the complexity of heavy client-side frameworks.

**Current Completion**: 35% of full vision (12 of ~50 planned components)
**Production Ready**: ✅ Yes
**Target Frameworks**: .NET 6.0, .NET 7.0
**Dependencies**: ASP.NET Core only (ECharts loaded dynamically for charts)
**License**: MIT (assumed, to be confirmed)

**Philosophy**:
- Server-rendered HTML for performance and SEO
- Progressive enhancement for reliability
- Accessibility built-in, not bolted on
- Minimal JavaScript for simplicity
- Token-based theming for customization
- Native ASP.NET integration for productivity

This file represents the complete knowledge base of RazorPlus as of Stage 3 completion.
