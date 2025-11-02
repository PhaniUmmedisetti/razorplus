# RazorPlus - Quick Reference Card

## 📦 Installation
```bash
dotnet add package RazorPlus --version 1.1.0
```

```razor
@* _ViewImports.cshtml *@
@addTagHelper *, RazorPlus
```

```html
<!-- _Layout.cshtml -->
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
```

---

## 🎨 Themes

### Default Theme (Modern)
```html
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
```

### Enterprise Theme
```html
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<link href="_content/RazorPlus/css/themes/razorplus-theme-enterprise.css" rel="stylesheet" />
```

---

## 📝 Form Components

### Button
```razor
<rp-button variant="primary|secondary|tertiary|ghost|danger|success"
           size="sm|md|lg"
           icon="icon-name"
           loading="true|false"
           disabled="true|false"
           block="true|false"
           as="button|a"
           href="/url"
           type="submit|button|reset">
    Click Me
</rp-button>
```

### Input
```razor
<rp-input asp-for="PropertyName"
          label="Label Text"
          required="true"
          prefix="$"
          suffix="USD"
          hint="Helper text"
          placeholder="Enter value" />
```

### Textarea
```razor
<rp-textarea asp-for="PropertyName"
             label="Label"
             rows="4"
             hint="Helper text" />
```

### Select
```razor
<rp-select asp-for="PropertyName"
           items="Model.SelectList"
           label="Label"
           placeholder="Select..."
           clearable="true"
           filterable="true"
           fetch-url="@Url.Page(null, 'Search')"
           search-param="q"
           multiple="true" />
```

### Switch
```razor
<rp-switch asp-for="BoolProperty"
           label="Label"
           on-text="On"
           off-text="Off" />
```

### Radio Group
```razor
<rp-radio-group asp-for="PropertyName"
                items="Model.SelectList"
                label="Label"
                layout="vertical|horizontal" />
```

---

## 🏗️ Structural Components

### Tabs
```razor
<rp-tabs id="myTabs">
    <rp-tab id="tab1" header="Tab 1" active="true">
        Content 1
    </rp-tab>
    <rp-tab id="tab2" header="Tab 2">
        Content 2
    </rp-tab>
</rp-tabs>
```

### Accordion
```razor
<rp-accordion id="myAccordion">
    <rp-accordion-item id="item1" header="Item 1" expanded="true">
        Content 1
    </rp-accordion-item>
    <rp-accordion-item id="item2" header="Item 2">
        Content 2
    </rp-accordion-item>
</rp-accordion>
```

### Modal
```razor
<!-- Trigger -->
<rp-button onclick="RazorPlus.openModal('myModal')">Open</rp-button>

<!-- Modal -->
<rp-modal id="myModal" title="Title" static-backdrop="false">
    <rp-modal-body>
        Content
    </rp-modal-body>
    <rp-modal-footer>
        <rp-button onclick="RazorPlus.closeModal('myModal')">Close</rp-button>
    </rp-modal-footer>
</rp-modal>
```

---

## 📊 Data Components

### Table
```razor
<rp-table items="Model.Items"
          sortable="true"
          pageable="true"
          client="false"
          page="@Model.Page"
          page-size="25"
          total-items="@Model.Total">

    <rp-column for="Name"
               header="Name"
               sortable="true" />

    <rp-column for="Price"
               header="Price"
               align="right"
               template="@(item => ((Product)item).Price.ToString("C"))" />
</rp-table>
```

### Pagination
```razor
<rp-pagination page="@Model.Page"
               page-size="25"
               total-items="@Model.Total"
               page-param="page" />
```

### Chart
```razor
<rp-chart id="myChart"
          type="line|bar|pie"
          height="350"
          theme="auto|light|dark"
          data="@Model.ChartData"
          options="@Model.ChartOptions" />
```

---

## 🔄 Workflow Components

### Stage Divider
```razor
<rp-stage-divider stage="1-8" title="Stage Title">
    Content
</rp-stage-divider>
```

**Stage Colors**: 1=Lightcyan, 2=Peachpuff, 3=Lightblue, 4=Lemonchiffon, 5=LightBlue, 6=PowderBlue, 7=Yellow, 8=Thistle

### Popup Selector
```razor
<rp-popup-selector id="selector"
                   label="Label"
                   required="true"
                   modal-id="modalId"
                   display-text="@Model.Selected"
                   value="@Model.Id"
                   name="FieldName"
                   hint="Helper text" />
```

---

## 🎨 UI Utilities

### Badges
```html
<span class="rp-badge rp-badge--primary|success|warning|danger|info">
    Text
</span>
```

### Alerts
```html
<div class="rp-alert rp-alert--success|info|warning|danger">
    <div class="rp-alert__icon">✓</div>
    <div class="rp-alert__content">Message</div>
    <button class="rp-alert__close">&times;</button>
</div>
```

### Cards
```html
<div class="rp-card md-shadow-z-2">
    <div class="rp-card-header">
        <h3 class="rp-card-title">Title</h3>
    </div>
    <div class="rp-card-body">Content</div>
    <div class="rp-card-footer">Footer</div>
</div>
```

### Material Shadows
```html
<div class="md-shadow-z-1|z-2|z-3|z-4|z-5">Content</div>
```

---

## 🎯 Common Patterns

### Complete Form
```razor
<form method="post">
    <rp-input asp-for="Name" label="Name" required="true" />
    <rp-select asp-for="CategoryId" items="Model.Categories" label="Category" />
    <rp-textarea asp-for="Description" label="Description" rows="4" />
    <rp-switch asp-for="IsActive" label="Active" />

    <rp-button variant="primary" type="submit">Save</rp-button>
    <rp-button variant="ghost" as="a" href="/list">Cancel</rp-button>
</form>
```

### Workflow Transaction
```razor
<form method="post">
    <rp-stage-divider stage="1" title="Preparation">
        <rp-input asp-for="Code" label="Code" />
    </rp-stage-divider>

    <rp-stage-divider stage="2" title="Execution">
        <rp-textarea asp-for="Notes" label="Notes" />
    </rp-stage-divider>

    <rp-button variant="primary" type="submit">Submit</rp-button>
</form>
```

### Data Table with Pagination
```razor
<rp-table items="Model.Items"
          sortable="true"
          page="@Model.Page"
          page-size="25"
          total-items="@Model.Total">
    <rp-column for="Name" header="Name" sortable="true" />
    <rp-column for="Status" header="Status" />
</rp-table>

<rp-pagination page="@Model.Page"
               page-size="25"
               total-items="@Model.Total" />
```

---

## ⌨️ JavaScript API

```javascript
// Modal
RazorPlus.openModal('modalId');
RazorPlus.closeModal('modalId');

// Refresh components after AJAX
RazorPlus.refresh(document.getElementById('container'));
```

---

## 🎨 CSS Variables (Theming)

```css
:root {
  --rp-primary: #36c6d3;
  --rp-secondary: #486CAE;
  --rp-success: #32c5d2;
  --rp-warning: #f1c40f;
  --rp-danger: #e74c3c;
  --rp-info: #3498db;

  --rp-input-bg: lightcyan;
  --rp-bg: #ffffff;
  --rp-fg: #333333;
  --rp-border: #c2cad8;

  --rp-radius: 0px;
  --rp-gap: 8px;
  --rp-control-md: 38px;

  --rp-font: "Open Sans", sans-serif;
}
```

---

## 📱 Responsive Classes

```css
/* Breakpoints */
@media (max-width: 640px) { /* Mobile */ }
@media (min-width: 641px) and (max-width: 992px) { /* Tablet */ }
@media (min-width: 993px) { /* Desktop */ }
```

---

## ✅ PageModel Pattern (Server-Side Table)

```csharp
public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Sort { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Dir { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Page { get; set; } = 1;

    public int Total { get; set; }
    public List<Item> Items { get; set; } = new();

    public void OnGet()
    {
        var query = _db.Items.AsQueryable();

        // Sort
        query = (Sort?.ToLower(), Dir?.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(x => x.Name),
            ("name", _) => query.OrderBy(x => x.Name),
            _ => query.OrderBy(x => x.Id)
        };

        // Paginate
        Total = query.Count();
        Items = query.Skip((Page - 1) * 25).Take(25).ToList();
    }
}
```

---

## 🔍 Common Issues

### Issue: TagHelpers not working
**Solution**: Add `@addTagHelper *, RazorPlus` to `_ViewImports.cshtml`

### Issue: Styles not loading
**Solution**: Ensure `_content/RazorPlus/css/razorplus.css` path is correct

### Issue: JavaScript not working
**Solution**: Add `<script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>` before `</body>`

### Issue: Modal won't open
**Solution**: Use `RazorPlus.openModal('id')` or check that modal `id` matches

### Issue: Table not sorting
**Solution**: Ensure PageModel has `[BindProperty(SupportsGet = true)]` on Sort/Dir properties

---

## 📚 Documentation

- **README.md** - Getting started guide
- **CLAUDE.md** - Complete architecture documentation
- **VERSIONING.md** - Version management and migration
- **EXECUTIVE_PRESENTATION.md** - Comprehensive overview
- **QUICK_REFERENCE.md** - This document

---

## 📊 Component Count Summary

- **Form Components**: 6 (Input, Textarea, Select, Switch, RadioGroup, Button)
- **Structural**: 3 (Tabs, Accordion, Modal)
- **Data Display**: 3 (Table, Pagination, Chart)
- **Workflow**: 2 (Stage Divider, Popup Selector)
- **UI Utilities**: 3 (Badges, Alerts, Cards)
- **Total**: **17 Components**

---

## 🎯 Quick Start (Copy & Paste)

```bash
# 1. Install
dotnet add package RazorPlus --version 1.1.0
```

```razor
@* 2. _ViewImports.cshtml *@
@addTagHelper *, RazorPlus
```

```html
<!-- 3. _Layout.cshtml -->
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
```

```razor
@* 4. Use in any .cshtml *@
<rp-input asp-for="Name" label="Name" required="true" />
<rp-button variant="primary" type="submit">Submit</rp-button>
```

**Done!** ✅

---

**Version**: 1.1.0 | **License**: MIT | **Framework**: ASP.NET Core 6.0, 7.0
