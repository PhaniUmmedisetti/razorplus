# RazorPlus Customization Guide

## 🎨 Quick Customization Examples

### 1. Change Button Colors

**Option A - Global Color Change (Easiest):**
```css
/* In your site.css or _Layout.cshtml */
:root {
  --rp-primary: #ff6b6b;  /* Red */
  --rp-danger: #8b5cf6;   /* Purple */
}
```

**Option B - Custom Button Variant:**
```css
.rp-btn--purple { background: #8b5cf6; color: #fff; }
.rp-btn--green { background: #10b981; color: #fff; }
```
```razor
<rp-button variant="purple">Purple Button</rp-button>
<rp-button variant="green">Green Button</rp-button>
```

**Option C - Inline Style:**
```razor
<rp-button variant="primary" style="background: #ff6b6b; border-color: #ff6b6b;">
  Custom Color
</rp-button>
```

---

### 2. Chart Customization

**Add Legend Below Chart:**
```csharp
public object ChartOptions => new
{
    legend = new
    {
        data = new[] { "Revenue", "Expenses", "Profit" },
        bottom = 0  // Position at bottom
    },
    tooltip = new { trigger = "axis" },
    grid = new { bottom = 60 } // Make room for legend
};
```

**Full Custom Chart:**
```csharp
public object ChartData => new
{
    xAxis = new { type = "category", data = new[] { "Jan", "Feb", "Mar" } },
    yAxis = new { type = "value" },
    series = new[]
    {
        new {
            name = "Revenue",
            type = "line",
            data = new[] { 120, 200, 180 },
            itemStyle = new { color = "#ff6b6b" } // Custom color
        }
    }
};

public object ChartOptions => new
{
    title = new { text = "Monthly Revenue", left = "center" },
    legend = new
    {
        data = new[] { "Revenue" },
        bottom = 0,
        textStyle = new { fontSize = 12 }
    },
    tooltip = new
    {
        trigger = "axis",
        formatter = "{b}: ${c}" // Custom format
    }
};
```

---

### 3. Table Customization

**Custom Row Colors:**
```css
.rp-table tbody tr[data-rp-row-key="5"] {
  background: #fef3c7 !important; /* Highlight specific row */
}
```

**Alternate Row Colors (Striped):**
```css
.rp-table tbody tr:nth-child(even) {
  background: #f9fafb;
}
[data-theme="dark"] .rp-table tbody tr:nth-child(even) {
  background: #1f2937;
}
```

---

### 4. Form Input Styling

**Rounded Inputs:**
```css
:root {
  --rp-radius: 20px; /* Makes everything rounded */
}
```

**Change Input Background:**
```css
.rp-input__control {
  background: #f0f9ff !important;
  border-color: #3b82f6 !important;
}
```

---

### 5. Dark Mode Colors

```css
[data-theme="dark"] {
  --rp-bg: #0f172a;      /* Darker background */
  --rp-fg: #f1f5f9;      /* Lighter text */
  --rp-primary: #60a5fa; /* Lighter blue for dark mode */
}
```

---

### 6. Progress Bar Colors

```css
.rp-progress__bar--custom {
  background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
}
```
```razor
<rp-progress value="75" variant="custom" />
```

---

### 7. Modal Size

```css
.rp-modal__dialog--large {
  max-width: 800px;
  width: 90%;
}
```
```razor
<rp-modal id="large-modal" title="Large Modal" css-class="rp-modal__dialog--large">
  <!-- content -->
</rp-modal>
```

---

### 8. Custom Dropdown Style

```css
.rp-dropdown-menu {
  border-radius: 12px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.2);
}
.rp-dropdown-item:hover {
  background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
  color: white;
}
```

---

## 🎯 All CSS Custom Properties

```css
:root {
  /* Colors */
  --rp-bg: #ffffff;
  --rp-fg: #111827;
  --rp-primary: #2563eb;
  --rp-danger: #dc2626;
  --rp-muted: #6b7280;

  /* Borders & Radius */
  --rp-radius: 8px;
  --rp-border: #cbd5e1;

  /* Spacing */
  --rp-gap: 8px;

  /* Shadows */
  --rp-shadow: 0 1px 2px rgba(0,0,0,.06), 0 1px 3px rgba(0,0,0,.1);
  --rp-shadow-lg: 0 10px 15px rgba(0,0,0,0.1);

  /* Focus */
  --rp-focus: 2px solid #2563eb66;

  /* Typography */
  --rp-font: system-ui, sans-serif;

  /* Control Sizes */
  --rp-control-sm: 28px;
  --rp-control-md: 36px;
  --rp-control-lg: 44px;
}
```

---

## 🚀 Quick Copy-Paste Themes

### Theme 1: Purple Theme
```css
:root {
  --rp-primary: #8b5cf6;
  --rp-danger: #ef4444;
  --rp-radius: 12px;
}
```

### Theme 2: Green Theme
```css
:root {
  --rp-primary: #10b981;
  --rp-danger: #f59e0b;
  --rp-radius: 6px;
}
```

### Theme 3: Sharp/Enterprise
```css
:root {
  --rp-primary: #36c6d3;
  --rp-radius: 0px;
  --rp-shadow: none;
}
```
