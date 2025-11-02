# RazorPlus Versioning & Migration Guide

## Semantic Versioning

RazorPlus follows [Semantic Versioning 2.0.0](https://semver.org/) (MAJOR.MINOR.PATCH):

```
1.0.0 → 1.1.0 → 1.2.0 → 2.0.0
```

- **MAJOR** (e.g., 2.0.0): Breaking changes that require code updates
- **MINOR** (e.g., 1.1.0): New features, backward compatible
- **PATCH** (e.g., 1.0.1): Bug fixes, backward compatible

## Version History

### Version 1.1.0 (Current)

**Release Date**: January 2025

**What's New:**
- ✨ **CaliberBRM Enterprise Theme** - Opt-in theme with cyan/teal primary colors
- ✨ **New Components**:
  - `<rp-stage-divider>` - Workflow stage containers with colored backgrounds
  - `<rp-popup-selector>` - Entity picker with modal selection
- ✨ **Theme System** - Multi-theme architecture with CSS variable overrides
- ✨ **Material Design Shadows** - `.md-shadow-z-1` through `.md-shadow-z-5` utility classes
- 🎨 **Enhanced Button Variants** - Added tertiary and success button styles
- 🎨 **Badge Components** - Status badges with color variants
- 🎨 **Alert Components** - Message alerts with dismissible option
- 📚 **Improved Documentation** - Theme switching guide and CaliberBRM integration

**Breaking Changes:** None (fully backward compatible)

**Migration:** No changes required. New features are opt-in.

---

### Version 1.0.0 (Initial Release)

**Release Date**: December 2024

**Features:**
- ✅ Core Components: Button, Input, Textarea, Select, Switch, RadioGroup
- ✅ Structural Components: Tabs, Accordion, Modal
- ✅ Data Components: Table, Pagination, Chart
- ✅ Default Theme with dark mode support
- ✅ Accessibility: ARIA attributes, keyboard navigation
- ✅ Server-first rendering with progressive enhancement
- ✅ .NET 6.0 and .NET 7.0 support

---

## Choosing a Version

### For New Projects

**Use the latest version (1.1.0+)** to get all features and improvements:

```xml
<PackageReference Include="RazorPlus" Version="1.1.0" />
```

### For Existing Projects

**Stay on your current version** if it's working well, or upgrade when ready:

```xml
<!-- Pin to specific version -->
<PackageReference Include="RazorPlus" Version="1.0.0" />

<!-- Allow minor updates -->
<PackageReference Include="RazorPlus" Version="1.*" />

<!-- Allow patch updates only -->
<PackageReference Include="RazorPlus" Version="1.0.*" />
```

---

## Theme Selection Guide

### Default Theme (Clean & Modern)

**Best for:** New applications, modern web apps, SaaS products

**Features:**
- Blue primary color (#2563eb)
- White input backgrounds
- Rounded corners (8px)
- Subtle shadows
- Modern, clean aesthetic

**Setup:**

```html
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
```

---

### CaliberBRM Enterprise Theme

**Best for:** Enterprise applications, workflow systems, CaliberBRM integration

**Features:**
- Cyan/teal primary color (#36c6d3)
- Lightcyan input backgrounds
- Sharp corners (0px)
- Material Design shadows
- Workflow stage colors
- Three-tier button hierarchy

**Setup:**

```html
<!-- Base styles -->
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />

<!-- CaliberBRM theme (load after base) -->
<link href="_content/RazorPlus/css/themes/razorplus-theme-caliber.css" rel="stylesheet" />

<script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>
```

---

## Migration Guides

### Migrating from 1.0.0 to 1.1.0

**Summary:** No breaking changes. All 1.0.0 code works in 1.1.0.

#### Step 1: Update Package Reference

```xml
<PackageReference Include="RazorPlus" Version="1.1.0" />
```

#### Step 2: (Optional) Add CaliberBRM Theme

If you want the CaliberBRM enterprise styling, add the theme CSS **after** the base CSS:

```html
<!-- Your _Layout.cshtml -->
<head>
  <link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />

  <!-- NEW: Add CaliberBRM theme -->
  <link href="_content/RazorPlus/css/themes/razorplus-theme-caliber.css" rel="stylesheet" />
</head>
```

#### Step 3: (Optional) Use New Components

Start using the new components in your pages:

**Stage Divider Example:**
```razor
<rp-stage-divider stage="1" title="Preparation">
  <div class="row">
    <rp-input asp-for="ProductCode" label="Product Code" required="true" />
  </div>
</rp-stage-divider>

<rp-stage-divider stage="2" title="Issuance">
  <div class="row">
    <rp-input asp-for="BatchNumber" label="Batch Number" />
  </div>
</rp-stage-divider>
```

**Popup Selector Example:**
```razor
<rp-popup-selector id="ProductPopup"
                   label="Product Name"
                   required="true"
                   modal-id="ProductSelectionModal"
                   display-text="@Model.SelectedProductName"
                   value="@Model.ProductId"
                   name="ProductId"
                   hint="Select a product from the list">
</rp-popup-selector>

<!-- Define your modal elsewhere -->
<rp-modal id="ProductSelectionModal" title="Select Product">
  <rp-modal-body>
    <!-- Your selection table/grid here -->
  </rp-modal-body>
</rp-modal>
```

**Material Design Shadows:**
```razor
<div class="rp-card md-shadow-z-2">
  <div class="rp-card-header">
    <h3 class="rp-card-title">Card with Shadow</h3>
  </div>
  <div class="rp-card-body">
    Content goes here
  </div>
</div>
```

#### Step 4: Test Your Application

Run your application and verify everything works as expected. The default theme should remain unchanged unless you explicitly added the CaliberBRM theme.

---

## CaliberBRM Integration Guide

### Complete Setup for CaliberBRM Applications

#### 1. Install RazorPlus

```bash
dotnet add package RazorPlus --version 1.1.0
```

#### 2. Add TagHelper Registration

In `_ViewImports.cshtml`:

```razor
@addTagHelper *, RazorPlus
```

#### 3. Add CaliberBRM Theme to Layout

In `_Layout.cshtml`:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - My App</title>

    <!-- RazorPlus Base Styles -->
    <link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />

    <!-- CaliberBRM Enterprise Theme -->
    <link href="_content/RazorPlus/css/themes/razorplus-theme-caliber.css" rel="stylesheet" />

    <!-- Your custom styles -->
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    @RenderBody()

    <!-- RazorPlus JavaScript (at end of body) -->
    <script src="_content/RazorPlus/js/razorplus.core.js" type="module"></script>

    <!-- Optional: Enhanced components -->
    <script src="_content/RazorPlus/js/razorplus.select.js" type="module"></script>
    <script src="_content/RazorPlus/js/razorplus.table.js" type="module"></script>
    <script src="_content/RazorPlus/js/razorplus.chart.js" type="module"></script>

    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

#### 4. Use CaliberBRM Components

**Transaction Form with Workflow Stages:**

```razor
@page
@model TransactionModel

<form method="post">
    <!-- Stage 1: Preparation -->
    <rp-stage-divider stage="1" title="Preparation">
        <div class="row">
            <div class="col-md-4">
                <rp-input asp-for="TransactionCode"
                         label="Transaction Code"
                         required="true"
                         hint="Auto-generated if left blank" />
            </div>

            <div class="col-md-4">
                <rp-popup-selector id="ProductPopup"
                                  label="Product"
                                  required="true"
                                  modal-id="ProductModal"
                                  display-text="@Model.SelectedProductName"
                                  value="@Model.ProductId"
                                  name="ProductId" />
            </div>

            <div class="col-md-4">
                <rp-input asp-for="Quantity"
                         label="Quantity"
                         required="true"
                         type="number" />
            </div>
        </div>
    </rp-stage-divider>

    <!-- Stage 2: Issuance -->
    <rp-stage-divider stage="2" title="Issuance">
        <div class="row">
            <div class="col-md-6">
                <rp-input asp-for="BatchNumber"
                         label="Batch Number"
                         required="true" />
            </div>

            <div class="col-md-6">
                <rp-popup-selector id="AreaPopup"
                                  label="Storage Area"
                                  required="true"
                                  modal-id="AreaModal"
                                  display-text="@Model.SelectedAreaName"
                                  value="@Model.AreaId"
                                  name="AreaId" />
            </div>
        </div>
    </rp-stage-divider>

    <!-- Stage 3: Execution -->
    <rp-stage-divider stage="3" title="Execution">
        <div class="row">
            <div class="col-md-12">
                <rp-textarea asp-for="ExecutionNotes"
                            label="Execution Notes"
                            rows="4"
                            hint="Enter detailed execution notes" />
            </div>
        </div>
    </rp-stage-divider>

    <!-- Action Buttons -->
    <div class="form-actions mt-4">
        <rp-button variant="primary" type="submit">
            Save Transaction
        </rp-button>

        <rp-button variant="secondary" type="button" onclick="window.history.back()">
            Cancel
        </rp-button>

        <rp-button variant="tertiary" type="button" onclick="resetForm()">
            Reset
        </rp-button>
    </div>
</form>

<!-- Product Selection Modal -->
<rp-modal id="ProductModal" title="Select Product">
    <rp-modal-body>
        <rp-input id="productSearch" label="Search" placeholder="Search products..." />

        <div class="table-responsive mt-3">
            <table class="rp-table">
                <thead>
                    <tr>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Status</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var product in Model.Products)
                    {
                        <tr>
                            <td>@product.Code</td>
                            <td>@product.Name</td>
                            <td>
                                <span class="rp-badge rp-badge--success">Active</span>
                            </td>
                            <td>
                                <rp-button variant="ghost"
                                          size="sm"
                                          onclick="selectProduct('@product.Id', '@product.Name')">
                                    Select
                                </rp-button>
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    </rp-modal-body>
</rp-modal>

@section Scripts {
    <script>
        function selectProduct(id, name) {
            // Update hidden input
            document.getElementById('ProductId').value = id;

            // Update display
            document.getElementById('ProductPopupDiv').innerHTML = `
                <div class="rp-popup-value-item">
                    <span class="rp-popup-value-item-text">${name}</span>
                    <span class="rp-popup-value-item-close" onclick="clearProduct()">&times;</span>
                </div>
            `;

            // Close modal
            if (typeof RazorPlus !== 'undefined' && RazorPlus.closeModal) {
                RazorPlus.closeModal('ProductModal');
            }
        }

        function clearProduct() {
            document.getElementById('ProductId').value = '';
            document.getElementById('ProductPopupDiv').innerHTML = '';
        }
    </script>
}
```

---

## Custom Theme Creation

### Creating Your Own Theme

You can create a custom theme by overriding CSS variables:

```css
/* my-custom-theme.css */

:root {
  /* Override primary color */
  --rp-primary: #7c3aed;
  --rp-primary-dark: #6d28d9;

  /* Override input background */
  --rp-input-bg: #faf5ff;

  /* Override radius */
  --rp-radius: 12px;

  /* Add custom colors */
  --rp-accent: #f59e0b;
}

/* Custom button variant */
.rp-btn--accent {
  background-color: var(--rp-accent);
  color: white;
}
```

**Load Order:**
```html
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<link href="~/css/my-custom-theme.css" rel="stylesheet" />
```

---

## Compatibility Matrix

| RazorPlus Version | .NET 6.0 | .NET 7.0 | .NET 8.0 |
|-------------------|----------|----------|----------|
| 1.0.0             | ✅       | ✅       | ⚠️*      |
| 1.1.0             | ✅       | ✅       | ⚠️*      |

*⚠️ Not explicitly targeted, but works via .NET 7.0 compatibility

---

## Support Policy

- **Current Version (1.1.x)**: Full support, active development
- **Previous Minor (1.0.x)**: Security fixes only
- **Older Versions**: No support, upgrade recommended

---

## Release Schedule

- **Patch Releases**: As needed for bug fixes
- **Minor Releases**: Every 2-3 months
- **Major Releases**: Yearly (with deprecation warnings 6 months prior)

---

## Getting Help

- 📚 **Documentation**: [README.md](./README.md), [CLAUDE.md](./CLAUDE.md)
- 🐛 **Issues**: [GitHub Issues](https://github.com/razorplus/razorplus/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/razorplus/razorplus/discussions)

---

## Changelog

See [CHANGELOG.md](./CHANGELOG.md) for detailed version history.
