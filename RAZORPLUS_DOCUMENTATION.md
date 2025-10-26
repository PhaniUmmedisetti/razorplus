# RazorPlus Documentation

## Overview

**RazorPlus** is a production-ready Razor Class Library (RCL) that provides a comprehensive suite of accessible, server-first UI components for ASP.NET Core Razor Pages and MVC applications. Built with TagHelper ergonomics, minimal JavaScript, and a tokenized theming system, RazorPlus enables rapid development of modern web applications without heavy client-side frameworks.

### Key Features

- **Server-First Architecture**: Components render on the server with progressive enhancement
- **Accessible by Default**: ARIA attributes, keyboard navigation, and semantic HTML
- **Minimal JavaScript**: Core interactions handled with vanilla JS modules
- **Tokenized Theming**: CSS custom properties for light/dark mode and easy customization
- **ASP.NET Integration**: Seamless integration with model binding and validation
- **Multi-Framework Support**: Targets both .NET 6.0 and .NET 7.0

---

## Table of Contents

1. [Installation](#installation)
2. [Components](#components)
   - [Form Controls](#form-controls)
   - [Structural Components](#structural-components)
   - [Data Display](#data-display)
   - [Overlays](#overlays)
3. [Theming](#theming)
4. [JavaScript API](#javascript-api)
5. [Testing](#testing)
6. [Architecture](#architecture)
7. [Roadmap](#roadmap)

---

## Installation

### 1. Add Package Reference

```xml
<ItemGroup>
  <ProjectReference Include="..\RazorPlus\RazorPlus.csproj" />
</ItemGroup>
```

### 2. Register in _ViewImports.cshtml

```razor
@addTagHelper *, RazorPlus
```

### 3. Include Static Assets

Add to your `_Layout.cshtml`:

```html
<head>
  <link rel="stylesheet" href="_content/RazorPlus/css/razorplus.css" />
</head>
<body>
  <!-- Your content -->
  <script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.select.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.table.js" type="module"></script>
  <script src="_content/RazorPlus/js/razorplus.chart.js" type="module"></script>
</body>
```

---

## Components

### Form Controls

#### rp-input

Text input with integrated label, hint, and validation.

**Properties:**
- `asp-for` (ModelExpression): Binds to model property
- `label` (string): Label text (defaults to display name)
- `hint` (string): Help text below input
- `required` (bool): Adds required attribute
- `prefix` (string): Text/icon before input
- `suffix` (string): Text/icon after input

**Example:**
```razor
<rp-input asp-for="Email"
          label="Email Address"
          hint="We'll never share your email"
          required="true"
          suffix="@example.com" />
```

**Generated HTML:**
```html
<div class="rp-field">
  <label class="rp-label" for="Email">Email Address</label>
  <div class="rp-input">
    <input class="rp-input__control" type="text" id="Email" name="Email" required />
    <span class="rp-input__suffix">@example.com</span>
  </div>
  <div class="rp-hint">We'll never share your email</div>
  <span class="rp-error" data-valmsg-for="Email"></span>
</div>
```

---

#### rp-textarea

Multi-line text input with field layout.

**Properties:**
- `asp-for` (ModelExpression)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `rows` (int): Default 4
- `cols` (int?): Optional column count
- `placeholder` (string)

**Example:**
```razor
<rp-textarea asp-for="Bio"
             label="About You"
             rows="6"
             placeholder="Tell us about yourself..." />
```

---

#### rp-select

Select dropdown with filtering, async search, and clearable options.

**Properties:**
- `asp-for` (ModelExpression)
- `items` (IEnumerable<SelectListItem>)
- `label` (string)
- `multiple` (bool): Enable multi-select
- `clearable` (bool): Show clear button
- `filterable` (bool): Client-side filtering
- `placeholder` (string)
- `fetch-url` (string): URL for async search
- `search-param` (string): Query param for search (default: "q")
- `search-min` (int): Min characters to trigger search (default: 2)
- `debounce-ms` (int): Search debounce delay (default: 250)

**Example:**
```razor
<rp-select asp-for="CountryId"
           label="Country"
           items="Model.Countries"
           clearable="true"
           filterable="true"
           fetch-url="@Url.Page(null, "SearchCountries")"
           placeholder="Select a country..." />
```

**Async Search Endpoint:**
```csharp
public IActionResult OnGetSearchCountries(string q)
{
    var results = _countries
        .Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
        .Take(10)
        .Select(c => new { value = c.Id, text = c.Name });
    return new JsonResult(results);
}
```

---

#### rp-switch

Toggle switch for boolean values.

**Properties:**
- `asp-for` (ModelExpression)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `on-text` (string): Text shown when enabled
- `off-text` (string): Text shown when disabled

**Example:**
```razor
<rp-switch asp-for="EmailNotifications"
           label="Email Notifications"
           on-text="Enabled"
           off-text="Disabled"
           hint="Receive updates via email" />
```

---

#### rp-radio-group

Accessible radio button group with fieldset/legend.

**Properties:**
- `asp-for` (ModelExpression)
- `items` (IEnumerable<SelectListItem>)
- `label` (string)
- `hint` (string)
- `required` (bool)
- `layout` (string): "vertical" (default) or "horizontal"

**Example:**
```razor
<rp-radio-group asp-for="ShippingSpeed"
                items="Model.ShippingOptions"
                label="Shipping Method"
                layout="horizontal" />
```

---

#### rp-validation-message

Standalone validation message display.

**Properties:**
- `asp-for` (ModelExpression)

**Example:**
```razor
<rp-validation-message asp-for="Password" />
```

---

### Structural Components

#### rp-button

Accessible button/link with consistent styling.

**Properties:**
- `variant` (string): "primary", "secondary", "ghost", "danger" (default: "primary")
- `size` (string): "sm", "md", "lg" (default: "md")
- `icon` (string): Icon identifier
- `block` (bool): Full-width button
- `loading` (bool): Show loading state
- `as` (string): "button" (default) or "a"
- `href` (string): Link URL when as="a"
- `disabled` (bool)
- `type` (string): "button" (default), "submit", "reset"

**Example:**
```razor
<rp-button variant="primary" type="submit" icon="save">
  Save Changes
</rp-button>

<rp-button as="a" href="/dashboard" variant="ghost">
  Cancel
</rp-button>
```

---

#### rp-tabs

Tab navigation with panels.

**Properties:**
- `id` (string): Container ID

**Child: rp-tab**
- `id` (string): Tab panel ID
- `header` (string): Tab button text
- `active` (bool): Initially active tab

**Example:**
```razor
<rp-tabs id="settings-tabs">
  <rp-tab id="general" header="General" active>
    <p>General settings content...</p>
  </rp-tab>
  <rp-tab id="security" header="Security">
    <p>Security settings content...</p>
  </rp-tab>
  <rp-tab id="billing" header="Billing">
    <p>Billing settings content...</p>
  </rp-tab>
</rp-tabs>
```

**Features:**
- Server-rendered structure
- Keyboard navigation (arrow keys, Home/End)
- Roving tabindex
- ARIA roles and states

---

#### rp-accordion

Collapsible sections with accessible controls.

**Properties:**
- `id` (string): Container ID

**Child: rp-accordion-item**
- `id` (string): Item ID
- `header` (string): Trigger button text
- `expanded` (bool): Initially expanded

**Example:**
```razor
<rp-accordion>
  <rp-accordion-item header="What is RazorPlus?" expanded>
    <p>RazorPlus is a server-first UI library for ASP.NET Core...</p>
  </rp-accordion-item>
  <rp-accordion-item header="How do I install it?">
    <p>Add the package reference and register tag helpers...</p>
  </rp-accordion-item>
</rp-accordion>
```

**Features:**
- Space/Enter to toggle
- ARIA expanded/controls/labelledby
- Keyboard accessible

---

### Data Display

#### rp-table

Sortable, pageable table with server or client-side behavior.

**Properties:**
- `items` (IEnumerable<object>): Data collection
- `sortable` (bool): Enable sorting
- `pageable` (bool): Enable pagination
- `client` (bool): Client-side sorting/paging
- `page-size` (int): Items per page (default: 25)
- `page` (int): Current page (default: 1)
- `total-items` (int): Total record count
- `sort-param` (string): Query param for sort field (default: "sort")
- `direction-param` (string): Query param for direction (default: "dir")
- `page-param` (string): Query param for page (default: "page")
- `empty-text` (string): Message when no data
- `key-selector` (string): Property name for row keys

**Child: rp-column**
- `for` (string): Property name
- `header` (string): Column header text
- `width` (string): Column width (CSS)
- `align` (string): "left" (default), "center", "right"
- `sortable` (bool): Enable sorting for column
- `sort-key` (string): Sort field name (defaults to `for`)
- `template` (Func<object, object?>): Custom cell renderer

**Example:**
```razor
<rp-table items="Model.Customers"
          sortable="true"
          pageable="true"
          page-size="25"
          page="@Model.Page"
          total-items="@Model.TotalCustomers"
          key-selector="Id">
  <rp-column for="Name" header="Customer Name" sortable="true" />
  <rp-column for="Email" header="Email" sortable="true" />
  <rp-column for="Status" header="Status" sortable="true" />
  <rp-column for="CreatedAt"
             header="Joined"
             sortable="true"
             template="@(c => ((Customer)c).CreatedAt.ToString("MM/dd/yyyy"))" />
  <rp-column for="Revenue"
             header="Revenue"
             align="right"
             template="@(c => ((Customer)c).Revenue.ToString("C"))" />
</rp-table>
```

**Server-Side Sorting Example:**
```csharp
public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Sort { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Dir { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 25;
    public int TotalCustomers { get; set; }
    public List<Customer> Customers { get; set; } = new();

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

        TotalCustomers = query.Count();
        Customers = query
            .Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}
```

---

#### rp-pagination

Pagination controls with query string preservation.

**Properties:**
- `page` (int): Current page
- `page-size` (int): Items per page
- `total-items` (int): Total record count
- `page-param` (string): Query param name (default: "page")

**Example:**
```razor
<rp-pagination page="@Model.Page"
               page-size="25"
               total-items="@Model.TotalItems" />
```

**Features:**
- Preserves existing query parameters
- Window of ±2 pages from current
- First/Last page links
- Ellipsis for gaps
- Prev/Next navigation

---

#### rp-chart

Chart visualization using Apache ECharts.

**Properties:**
- `id` (string): Chart container ID
- `type` (string): "line", "bar", "pie", etc. (default: "line")
- `data` (object): Chart data object
- `options` (object): ECharts configuration
- `height` (int): Chart height in pixels (default: 280)
- `theme` (string): "auto", "light", "dark" (default: "auto")
- `export` (string): Enable export ("png", "svg")
- `defer` (bool): Lazy load chart

**Example:**
```razor
<rp-chart id="revenue-chart"
          type="line"
          height="320"
          data="@Model.ChartData"
          options="@Model.ChartOptions"
          theme="auto" />
```

**C# Data Example:**
```csharp
public object ChartData => new
{
    labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" },
    datasets = new[]
    {
        new
        {
            name = "Revenue",
            data = new[] { 12500, 18300, 15200, 21400, 19800, 24600 }
        }
    }
};

public object ChartOptions => new
{
    legend = new { show = true },
    tooltip = new { trigger = "axis" },
    grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true }
};
```

---

### Overlays

#### rp-modal

Modal dialog with focus trapping and backdrop.

**Properties:**
- `id` (string): Modal ID
- `title` (string): Modal title
- `open` (bool): Initially open
- `static-backdrop` (bool): Prevent closing on backdrop click

**Children:**
- `rp-modal-header`: Custom header content
- `rp-modal-body`: Main content
- `rp-modal-footer`: Action buttons

**Example:**
```razor
<rp-button onclick="RazorPlus.openModal('confirm-modal')">
  Delete Account
</rp-button>

<rp-modal id="confirm-modal" title="Confirm Deletion">
  <rp-modal-header>
    <h2 class="rp-modal__title">Confirm Deletion</h2>
  </rp-modal-header>
  <rp-modal-body>
    <p>Are you sure you want to delete your account? This action cannot be undone.</p>
  </rp-modal-body>
  <rp-modal-footer>
    <rp-button variant="ghost" onclick="RazorPlus.closeModal('confirm-modal')">
      Cancel
    </rp-button>
    <rp-button variant="danger" type="submit">
      Delete Forever
    </rp-button>
  </rp-modal-footer>
</rp-modal>
```

**Features:**
- Focus trap within modal
- Escape key to close
- Backdrop click to close (unless static)
- ARIA role="dialog" and aria-modal

---

## Theming

### CSS Custom Properties

RazorPlus uses CSS variables for comprehensive theming:

```css
:root {
  --rp-bg: #ffffff;
  --rp-fg: #111827;
  --rp-primary: #2563eb;
  --rp-danger: #dc2626;
  --rp-muted: #6b7280;
  --rp-radius: 8px;
  --rp-gap: 8px;
  --rp-shadow: 0 1px 2px rgba(0,0,0,.06), 0 1px 3px rgba(0,0,0,.1);
  --rp-focus: 2px solid #2563eb66;
  --rp-font: system-ui, -apple-system, Segoe UI, Roboto, sans-serif;
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

### Custom Theme Example

```css
/* Override in your site.css */
:root {
  --rp-primary: #7c3aed; /* Purple brand color */
  --rp-radius: 4px; /* Sharper corners */
  --rp-font: 'Inter', system-ui, sans-serif;
}
```

### Theme Toggle

```html
<button onclick="document.documentElement.setAttribute('data-theme',
  document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark')">
  Toggle Theme
</button>
```

---

## JavaScript API

### Core Module (razorplus.core.js)

Automatically initializes:
- Tabs (keyboard navigation)
- Accordion (expand/collapse)
- Modals (focus trap, backdrop)

**Global Functions:**

```javascript
// Modal control
RazorPlus.openModal(id)
RazorPlus.closeModal(id)

// Tab control
RazorPlus.activateTab(tabsId, tabId)
```

### Select Module (razorplus.select.js)

Enhances `rp-select` with:
- Client-side filtering
- Clear button
- Async search with debouncing
- Keyboard navigation

### Table Module (razorplus.table.js)

Enables client-side sorting and pagination for tables with `client="true"`.

### Chart Module (razorplus.chart.js)

Loads ECharts on demand and initializes chart instances.

---

## Testing

### Unit Tests

Located in `tests/RazorPlus.Tests/`:

**ButtonTagHelperTests.cs**
- Variant rendering
- Size classes
- Icon support
- As="a" link rendering
- Disabled state

**PaginationTagHelperTests.cs**
- Page calculation
- URL generation with query params
- Window rendering
- Ellipsis placement

**StructureTagHelperTests.cs**
- Tabs structure
- Accordion ARIA attributes
- Modal sections

### Running Tests

```bash
cd tests/RazorPlus.Tests
dotnet test
```

---

## Architecture

### Project Structure

```
RazorPlus/
├── src/RazorPlus/
│   ├── TagHelpers/              # All TagHelper implementations
│   │   ├── ButtonTagHelper.cs
│   │   ├── InputTagHelper.cs
│   │   ├── SelectTagHelper.cs
│   │   ├── TableTagHelpers.cs
│   │   ├── ModalTagHelper.cs
│   │   ├── TabsTagHelpers.cs
│   │   ├── AccordionTagHelper.cs
│   │   ├── ChartTagHelper.cs
│   │   ├── PaginationTagHelper.cs
│   │   └── ...
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── razorplus.css    # Complete stylesheet
│   │   └── js/
│   │       ├── razorplus.core.js
│   │       ├── razorplus.select.js
│   │       ├── razorplus.table.js
│   │       └── razorplus.chart.js
│   └── RazorPlus.csproj
├── samples/RazorPlus.Docs/      # Documentation site
└── tests/RazorPlus.Tests/       # Unit tests
```

### Design Principles

1. **Server-First**: HTML generated on server, enhanced progressively
2. **Accessibility**: ARIA attributes, semantic HTML, keyboard support
3. **Minimal JS**: Only essential interactions require JavaScript
4. **Composable**: TagHelpers can be combined and nested
5. **Framework Integration**: Leverages ASP.NET Core's IHtmlGenerator
6. **Performance**: Lazy loading, debouncing, minimal dependencies

---

## Roadmap

### ✅ Completed (Current State)

#### Form Controls
- ✅ Input (text, email, number, etc.)
- ✅ Textarea
- ✅ Select (with async search, filtering, clearable)
- ✅ Switch (toggle)
- ✅ Radio Group
- ✅ Validation Message

#### Structural Components
- ✅ Button (multiple variants, sizes, icons)
- ✅ Tabs (keyboard navigation)
- ✅ Accordion (accessible expand/collapse)

#### Data Display
- ✅ Table (server-side sorting/paging, client-side option)
- ✅ Pagination (query string preservation)
- ✅ Chart (ECharts integration)

#### Overlays
- ✅ Modal (focus trap, sections)

#### Theming & Styling
- ✅ CSS custom properties
- ✅ Light/dark mode support
- ✅ Responsive design
- ✅ Screen reader utilities

#### Testing
- ✅ Unit tests for core components
- ✅ .NET 6.0 and 7.0 support

---

### 🚀 Future Enhancements (Stage 3+)

#### Additional Form Controls
- ⬜ **Date Picker**: Calendar-based date selection
- ⬜ **Time Picker**: Time input with AM/PM
- ⬜ **Color Picker**: Visual color selection
- ⬜ **Checkbox Group**: Multi-select with fieldset
- ⬜ **File Upload**: Drag-and-drop file input with preview
- ⬜ **Range Slider**: Numeric range with visual slider
- ⬜ **Rich Text Editor**: WYSIWYG content editing
- ⬜ **Autocomplete**: Input with suggestions dropdown
- ⬜ **Tags Input**: Multi-value input with chips

#### Navigation Components
- ⬜ **Breadcrumbs**: Hierarchical navigation trail
- ⬜ **Menu/Dropdown**: Flyout menus with nesting
- ⬜ **Sidebar**: Collapsible navigation panel
- ⬜ **Navbar**: Top-level navigation bar
- ⬜ **Pagination (Advanced)**: Jump to page, custom ranges

#### Feedback & Messaging
- ⬜ **Toast/Notification**: Temporary messages (success, error, info)
- ⬜ **Alert**: Persistent message banners
- ⬜ **Progress Bar**: Linear progress indicator
- ⬜ **Spinner**: Loading state indicator
- ⬜ **Skeleton**: Content placeholder during load
- ⬜ **Badge**: Status indicators and counters
- ⬜ **Tooltip**: Hover/focus contextual help
- ⬜ **Popover**: Click-triggered floating content

#### Data Display (Extended)
- ⬜ **Card**: Content container with header/footer
- ⬜ **List**: Styled ordered/unordered lists
- ⬜ **Description List**: Key-value pairs display
- ⬜ **Tree View**: Hierarchical data display
- ⬜ **Timeline**: Chronological event display
- ⬜ **Stats Dashboard**: Metric cards with trends
- ⬜ **Empty State**: Placeholder for no data

#### Layout Components
- ⬜ **Grid System**: Responsive grid layout
- ⬜ **Stack**: Vertical/horizontal flex layout
- ⬜ **Divider**: Visual separator
- ⬜ **Container**: Max-width content wrapper
- ⬜ **Aspect Ratio**: Fixed ratio container

#### Advanced Features
- ⬜ **Data Grid**: Advanced table with column resizing, reordering, filtering
- ⬜ **Virtual Scroll**: Efficient rendering of large lists
- ⬜ **Drag & Drop**: Sortable lists and kanban boards
- ⬜ **Command Palette**: Keyboard-driven command menu (Cmd+K)
- ⬜ **Drawer**: Slide-in panel (sidebar modal)
- ⬜ **Stepper**: Multi-step form wizard
- ⬜ **Carousel**: Image/content slider
- ⬜ **Lightbox**: Full-screen image viewer

#### Theming & Customization
- ⬜ **Theme Builder**: Visual theme customization tool
- ⬜ **Icon System**: Built-in icon library integration
- ⬜ **CSS-in-C#**: Inline style generation from C# objects
- ⬜ **Component Variants**: Pre-built style variations
- ⬜ **Animation Utilities**: Transition and animation helpers

#### Developer Experience
- ⬜ **CLI Tool**: Scaffold new components and pages
- ⬜ **VS Code Extension**: IntelliSense for TagHelpers
- ⬜ **Storybook Integration**: Component documentation
- ⬜ **Design Tokens**: Figma/design system sync
- ⬜ **Localization**: Multi-language support

#### Performance & Optimization
- ⬜ **Bundle Optimization**: Tree-shaking for unused components
- ⬜ **CDN Support**: Hosted static assets
- ⬜ **Service Worker**: Offline support
- ⬜ **Lazy Loading**: Progressive component loading

#### Integrations
- ⬜ **SignalR**: Real-time component updates
- ⬜ **HTMX**: Enhanced partial rendering
- ⬜ **Blazor Interop**: Shared components between Razor and Blazor
- ⬜ **OpenAPI**: Auto-generated forms from schema

---

## Completion Assessment

### Current State: **Phase 2 Complete** (~35% of Full Vision)

**What We've Built:**
- ✅ Complete form control suite (input, textarea, select, switch, radio)
- ✅ Core structural components (button, tabs, accordion)
- ✅ Data display with sorting/paging (table, pagination, chart)
- ✅ Modal system with accessibility
- ✅ Theming foundation (light/dark, CSS tokens)
- ✅ JavaScript interactivity layer
- ✅ Test coverage for core components
- ✅ Multi-framework support (.NET 6/7)
- ✅ Production-ready documentation site

**Production Ready:** ✅ YES
The current library is fully functional and can be used in production applications. All core components are:
- Accessible (ARIA compliant)
- Keyboard navigable
- Server-side rendered
- Progressively enhanced
- Well-tested
- Documented

### What's Next: **Phase 3-5** (~65% Remaining)

**Immediate Priorities (Phase 3):**
1. **File Upload Component** - Common requirement for forms
2. **Toast Notifications** - Essential feedback mechanism
3. **Date/Time Pickers** - Frequently requested form controls
4. **Breadcrumbs & Menus** - Navigation essentials
5. **Card Component** - Layout building block

**Medium-Term (Phase 4):**
1. **Advanced Data Grid** - Excel-like table features
2. **Drawer Component** - Slide-in panels
3. **Command Palette** - Modern app UX pattern
4. **Stepper/Wizard** - Multi-step workflows
5. **Rich Notifications** - Alert system

**Long-Term (Phase 5):**
1. **Developer Tooling** - CLI, VS Code extension
2. **Design System Integration** - Figma tokens
3. **Advanced Charts** - More visualization types
4. **Real-time Features** - SignalR integration
5. **Enterprise Features** - Advanced grid, virtualization

### Estimated Effort to Full Completion

**Remaining Components:** ~40-50 additional components
**Estimated Timeline:**
- Phase 3: 2-3 months (10-12 components)
- Phase 4: 3-4 months (15-20 components)
- Phase 5: 4-6 months (15-20 components + tooling)

**Total to "Complete" Vision:** 9-13 months of development

### Market Comparison

**RazorPlus vs. Alternatives:**
- **Radzen Blazor**: RazorPlus is lighter, server-first vs. Blazor WebAssembly
- **Telerik UI**: RazorPlus is open-source, no licensing costs
- **MudBlazor**: RazorPlus works with Razor Pages/MVC, not just Blazor
- **Bootstrap TagHelpers**: RazorPlus has integrated validation, theming, accessibility

**Unique Value:**
- Server-first architecture (better SEO, faster initial load)
- Minimal JavaScript footprint
- Native ASP.NET Core integration
- Accessibility-first design
- Token-based theming system

---

## Contributing

### Development Setup

```bash
git clone https://github.com/yourusername/razorplus.git
cd razorplus
dotnet restore
dotnet build
```

### Running the Docs Site

```bash
cd samples/RazorPlus.Docs
dotnet run
```

Navigate to `https://localhost:5001`

### Adding a New Component

1. Create TagHelper in `src/RazorPlus/TagHelpers/`
2. Add styles to `wwwroot/css/razorplus.css`
3. Add JavaScript if needed in `wwwroot/js/`
4. Write tests in `tests/RazorPlus.Tests/`
5. Document in `samples/RazorPlus.Docs/Pages/Index.cshtml`

---

## License

MIT License - see LICENSE file for details

---

## Support

- **GitHub Issues**: Report bugs and request features
- **Documentation**: See samples/RazorPlus.Docs
- **Email**: support@razorplus.dev (if applicable)

---

**Built with ❤️ for the ASP.NET Core community**
