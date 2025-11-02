# RazorPlus Themes

This directory contains optional theme stylesheets that extend and customize the base RazorPlus design system.

## Available Themes

### 1. CaliberBRM Enterprise Theme (`razorplus-theme-caliber.css`)

**Target Use Case:** Enterprise workflow applications, CaliberBRM integration, pharmaceutical/manufacturing systems

**Color Palette:**
- Primary: `#36c6d3` (Cyan/Teal)
- Secondary: `#486CAE` (Blue)
- Success: `#32c5d2` (Teal)
- Warning: `#f1c40f` (Yellow)
- Danger: `#e74c3c` (Red)
- Info: `#3498db` (Blue)

**Key Features:**
- **Lightcyan input backgrounds** for better visual distinction
- **Sharp corners** (border-radius: 0) for formal enterprise look
- **Material Design shadows** (5 levels) for depth and hierarchy
- **Workflow stage colors** (8 predefined stages)
- **Three-tier button hierarchy**: Primary (cyan), Secondary (blue), Tertiary (outline)
- **Typography**: Open Sans font family
- **Enterprise-grade color system** with semantic naming

**Components Enhanced:**
- All form inputs (lightcyan backgrounds)
- Button variants (primary, secondary, tertiary, success)
- Cards with material shadows
- Badges for status indicators
- Alerts for messages
- Tables with hover effects
- Stage dividers for workflows

**Usage:**

```html
<!-- In your _Layout.cshtml -->
<head>
  <!-- Base RazorPlus styles (required) -->
  <link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />

  <!-- CaliberBRM theme (load after base) -->
  <link href="_content/RazorPlus/css/themes/razorplus-theme-caliber.css" rel="stylesheet" />
</head>
```

**Sample Page:**

```razor
@page
@model IndexModel

<form method="post">
  <!-- Stage 1: Preparation (lightcyan background) -->
  <rp-stage-divider stage="1" title="Preparation">
    <div class="row">
      <div class="col-md-6">
        <rp-input asp-for="ProductCode"
                 label="Product Code"
                 required="true"
                 hint="Enter product code" />
      </div>
    </div>
  </rp-stage-divider>

  <!-- Stage 2: Issuance (peachpuff background) -->
  <rp-stage-divider stage="2" title="Issuance">
    <div class="row">
      <div class="col-md-6">
        <rp-input asp-for="BatchNumber"
                 label="Batch Number"
                 required="true" />
      </div>
    </div>
  </rp-stage-divider>

  <!-- Action Buttons -->
  <div class="mt-4">
    <rp-button variant="primary" type="submit">Submit</rp-button>
    <rp-button variant="secondary" type="button">Save Draft</rp-button>
    <rp-button variant="tertiary" type="button">Cancel</rp-button>
  </div>
</form>
```

---

## Creating Custom Themes

### Method 1: CSS Variable Overrides

Create a custom theme by overriding CSS variables:

```css
/* my-theme.css */

:root {
  /* Brand Colors */
  --rp-primary: #7c3aed;        /* Purple */
  --rp-primary-dark: #6d28d9;
  --rp-secondary: #ec4899;      /* Pink */

  /* Functional Colors */
  --rp-input-bg: #faf5ff;       /* Light purple */
  --rp-readonly-bg: #f3f4f6;

  /* Geometry */
  --rp-radius: 12px;            /* Rounded corners */

  /* Shadows */
  --rp-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  --rp-shadow-md: 0 10px 15px rgba(0, 0, 0, 0.1);
}

/* Custom button variant */
.rp-btn--accent {
  background-color: var(--rp-secondary);
  color: white;
  border: none;
}

.rp-btn--accent:hover {
  background-color: #db2777;
}
```

**Usage:**
```html
<link href="_content/RazorPlus/css/razorplus.css" rel="stylesheet" />
<link href="~/css/my-theme.css" rel="stylesheet" />
```

### Method 2: Complete Theme File

Copy `razorplus-theme-caliber.css` and modify it:

```css
/* my-complete-theme.css */

:root {
  /* Define all variables */
  --rp-primary: #your-color;
  /* ... */
}

/* Override component styles */
.rp-btn--primary {
  /* Custom button styling */
}

/* Add new utilities */
.my-custom-class {
  /* Custom styles */
}
```

---

## Theme Variables Reference

### Colors

```css
:root {
  /* Brand */
  --rp-primary: #color;
  --rp-primary-dark: #color;
  --rp-secondary: #color;

  /* Status */
  --rp-success: #color;
  --rp-warning: #color;
  --rp-danger: #color;
  --rp-info: #color;

  /* Neutral */
  --rp-fg: #color;              /* Foreground/text */
  --rp-bg: #color;              /* Background */
  --rp-bg-alt: #color;          /* Alternate background */
  --rp-border: #color;          /* Border color */
  --rp-text-secondary: #color;
  --rp-text-muted: #color;

  /* Functional */
  --rp-input-bg: #color;        /* Input background */
  --rp-readonly-bg: #color;     /* Read-only fields */
  --rp-disabled-bg: #color;     /* Disabled fields */
}
```

### Typography

```css
:root {
  --rp-font: font-family;
  --rp-font-mono: font-family;
}
```

### Geometry

```css
:root {
  --rp-radius: 8px;             /* Border radius */
  --rp-gap: 8px;                /* Default spacing */

  /* Control sizes */
  --rp-control-sm: 32px;
  --rp-control-md: 38px;
  --rp-control-lg: 44px;
}
```

### Shadows

```css
:root {
  --rp-shadow: box-shadow;
  --rp-shadow-sm: box-shadow;
  --rp-shadow-md: box-shadow;
  --rp-shadow-lg: box-shadow;
  --rp-shadow-xl: box-shadow;
  --rp-shadow-2xl: box-shadow;
}
```

### Focus State

```css
:root {
  --rp-focus: 0 0 0 2px rgba(color, 0.25);
}
```

### Workflow Stages (CaliberBRM)

```css
:root {
  --rp-stage-preparation: lightcyan;
  --rp-stage-issuance: peachpuff;
  --rp-stage-execution: lightblue;
  --rp-stage-closure: lemonchiffon;
  --rp-stage-equipment: LightBlue;
  --rp-stage-location: PowderBlue;
  --rp-stage-workflow: #ffffcc;
  --rp-stage-dispensing: Thistle;
}
```

---

## Dark Mode Support

All themes support dark mode via `[data-theme="dark"]`:

```css
[data-theme="dark"] {
  --rp-bg: #111827;
  --rp-fg: #e5e7eb;
  --rp-input-bg: #0f172a;
  /* Override other variables */
}
```

**Toggle Dark Mode:**

```html
<button onclick="toggleDarkMode()">Toggle Dark Mode</button>

<script>
function toggleDarkMode() {
  const html = document.documentElement;
  const current = html.getAttribute('data-theme');
  html.setAttribute('data-theme', current === 'dark' ? 'light' : 'dark');

  // Save preference
  localStorage.setItem('theme', current === 'dark' ? 'light' : 'dark');
}

// Load saved preference
document.addEventListener('DOMContentLoaded', () => {
  const saved = localStorage.getItem('theme');
  if (saved) {
    document.documentElement.setAttribute('data-theme', saved);
  }
});
</script>
```

---

## Theme Comparison

| Feature                  | Default | CaliberBRM |
|-------------------------|---------|------------|
| Primary Color           | Blue    | Cyan/Teal  |
| Input Background        | White   | Lightcyan  |
| Border Radius           | 8px     | 0px        |
| Shadow Style            | Subtle  | Material   |
| Button Hierarchy        | 2-tier  | 3-tier     |
| Workflow Stages         | ❌      | ✅ (8)     |
| Font Family             | System  | Open Sans  |
| Best For                | Modern apps | Enterprise |

---

## Best Practices

1. **Load Order:** Always load the base `razorplus.css` first, then your theme
2. **Don't Modify Base:** Keep the base file intact; override in theme files
3. **Use Variables:** Leverage CSS custom properties for consistency
4. **Test Dark Mode:** Ensure your theme works in both light and dark modes
5. **Mobile First:** Test responsive behavior on small screens
6. **Accessibility:** Maintain WCAG 2.1 AA contrast ratios

---

## Contributing Themes

Want to contribute a theme? Follow these steps:

1. Create a new theme file: `razorplus-theme-yourname.css`
2. Follow the CaliberBRM theme structure as a template
3. Document your theme's purpose and features
4. Test thoroughly in both light and dark modes
5. Submit a pull request with:
   - Theme CSS file
   - Documentation updates
   - Sample usage examples
   - Screenshots

---

## Support

- **Documentation:** [Main README](../../../../README.md)
- **Issues:** [GitHub Issues](https://github.com/razorplus/razorplus/issues)
- **Examples:** [Sample Application](../../../../samples/RazorPlus.Docs/)
