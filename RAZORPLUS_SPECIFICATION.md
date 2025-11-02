# RazorPlus UI Library Specification
## CaliberBRM UI Component Library - Complete Reference Guide

> **Purpose**: This document provides a comprehensive specification for building RazorPlus, an Element Plus-inspired UI component library tailored for ASP.NET Razor Pages applications, based on the CaliberBRM enterprise system's proven patterns and requirements.

---

## Table of Contents
1. [Technology Stack](#technology-stack)
2. [Design System](#design-system)
3. [Component Catalog](#component-catalog)
4. [Layout System](#layout-system)
5. [Form Components](#form-components)
6. [Data Display Components](#data-display-components)
7. [Navigation Components](#navigation-components)
8. [Feedback Components](#feedback-components)
9. [Utilities & Helpers](#utilities--helpers)
10. [Integration Patterns](#integration-patterns)

---

## Technology Stack

### Core Dependencies
```json
{
  "css-frameworks": {
    "bootstrap": "5.x (bootstrap.min.css, bootstrap-extended.css)",
    "metro-ui": "4.x (metro.css, metro-icons.css, metro-responsive.css)",
    "font-awesome": "6.x (all.min.css)",
    "bootstrap-icons": "1.x",
    "simple-line-icons": "custom",
    "feather-icons": "4.x"
  },
  "javascript-libraries": {
    "jquery": "3.7.x",
    "bootstrap": "5.3.x (bootstrap.bundle.min.js)",
    "jquery-ui": "1.13.x",
    "popper.js": "2.x"
  },
  "plugins": {
    "datatables": "1.13.x (jquery.dataTables.min.js)",
    "select2": "4.1.x",
    "daterangepicker": "3.x (bootstrap-daterangepicker)",
    "summernote": "0.8.x (WYSIWYG editor)",
    "sweetalert2": "11.x (modal/alerts)",
    "toastr": "2.x (toast notifications)",
    "jquery-validate": "1.19.x",
    "axios": "1.x",
    "moment.js": "2.x",
    "dayjs": "1.11.x",
    "apexcharts": "3.x (charting)",
    "plotly.js": "2.x (advanced charts)",
    "crypto-js": "4.x",
    "gridstack.js": "drag-drop layouts"
  }
}
```

### Browser Compatibility
- Modern browsers (Chrome, Firefox, Edge, Safari)
- IE11 legacy support (with polyfills)
- Touch-enabled devices support
- Responsive breakpoints: 320px, 768px, 992px, 1200px, 1920px

---

## Design System

### Color Palette

#### Primary Colors
```css
:root {
  /* Brand Colors */
  --caliber-primary: #36c6d3;        /* Cyan - primary actions */
  --caliber-primary-dark: #2a9ba5;  /* Dark cyan - hover states */
  --caliber-secondary: #486CAE;      /* Blue - secondary actions */

  /* Status Colors */
  --caliber-success: #32c5d2;        /* Teal - success states */
  --caliber-warning: #f1c40f;        /* Yellow - warnings */
  --caliber-danger: #e74c3c;         /* Red - errors/destructive actions */
  --caliber-info: #3498db;           /* Blue - informational */

  /* Neutral Colors */
  --caliber-text-primary: #333333;   /* Main text */
  --caliber-text-secondary: #888888; /* Secondary text */
  --caliber-text-muted: #999999;     /* Muted text */
  --caliber-border: #c2cad8;         /* Standard borders */
  --caliber-background: #ffffff;     /* Main background */
  --caliber-background-alt: #f5f5f5; /* Alternate background */

  /* Functional Colors */
  --caliber-input-bg: lightcyan;     /* Input fields */
  --caliber-readonly-bg: #f0f0f0;    /* Read-only fields */
  --caliber-disabled-bg: #e9ecef;    /* Disabled fields */

  /* Workflow Stage Colors */
  --caliber-preparation: lightcyan;  /* #e0ffff */
  --caliber-issuance: peachpuff;     /* #ffdab9 */
  --caliber-execution: lightblue;    /* #add8e6 */
  --caliber-closure: lemonchiffon;   /* #fffacd */
  --caliber-equipment: LightBlue;
  --caliber-location: PowderBlue;
  --caliber-workflow: #ffffcc;
  --caliber-dispensing: Thistle;
}
```

#### Semantic Color Classes
```css
/* Background Colors */
.bg-primary, .bg-info, .bg-success, .bg-warning, .bg-danger
.bg-light, .bg-dark, .bg-white

/* Text Colors */
.text-primary, .text-success, .text-warning, .text-danger
.text-muted, .text-white

/* Border Colors */
.border-primary, .border-success, .border-danger
```

### Typography

#### Font Families
```css
:root {
  --caliber-font-family: "Open Sans", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
  --caliber-font-family-mono: "Courier New", monospace;
  --caliber-font-family-fallback: "Microsoft Sans Serif", Arial;
}

body {
  font-family: var(--caliber-font-family);
  font-size: 14px;
  line-height: 1.5;
  color: #333333;
}
```

#### Font Sizes
```css
.text-xs   { font-size: 11px; }
.text-sm   { font-size: 12px; }
.text-base { font-size: 14px; } /* Default */
.text-lg   { font-size: 16px; }
.text-xl   { font-size: 18px; }
.text-2xl  { font-size: 20px; }
.text-3xl  { font-size: 24px; }
```

#### Font Weights
```css
.font-light  { font-weight: 300; }
.font-normal { font-weight: 400; }
.font-medium { font-weight: 500; }
.font-bold   { font-weight: 700; }
```

#### Headings
```css
h1, .h1 { font-size: 24px; font-weight: 700; margin-bottom: 16px; }
h2, .h2 { font-size: 20px; font-weight: 700; margin-bottom: 14px; }
h3, .h3 { font-size: 18px; font-weight: 600; margin-bottom: 12px; }
h4, .h4 { font-size: 16px; font-weight: 600; margin-bottom: 10px; }
h5, .h5 { font-size: 14px; font-weight: 600; margin-bottom: 8px; }
h6, .h6 { font-size: 12px; font-weight: 600; margin-bottom: 8px; }
```

### Spacing System

```css
/* Margin & Padding Scale (based on 4px grid) */
.m-0  { margin: 0; }
.m-1  { margin: 4px; }
.m-2  { margin: 8px; }
.m-3  { margin: 12px; }
.m-4  { margin: 16px; }
.m-5  { margin: 20px; }
.m-6  { margin: 24px; }
.m-8  { margin: 32px; }
.m-10 { margin: 40px; }

/* Directional variants: mt-, mb-, ml-, mr-, mx-, my- */
/* Padding variants: p-0 through p-10, pt-, pb-, pl-, pr-, px-, py- */
```

### Shadows (Material Design Inspired)

```css
.md-shadow-z-1 {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.18);
}

.md-shadow-z-2 {
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.22);
}

.md-shadow-z-3 {
  box-shadow: 0 8px 18px rgba(0, 0, 0, 0.18), 0 6px 6px rgba(0, 0, 0, 0.23);
}

.md-shadow-z-4 {
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.26), 0 10px 10px rgba(0, 0, 0, 0.22);
}

.md-shadow-z-5 {
  box-shadow: 0 19px 38px rgba(0, 0, 0, 0.28), 0 15px 12px rgba(0, 0, 0, 0.22);
}

.md-shadow-none {
  box-shadow: none !important;
}
```

### Border Radius
```css
:root {
  --caliber-radius-none: 0;
  --caliber-radius-sm: 2px;
  --caliber-radius-base: 4px;
  --caliber-radius-lg: 6px;
  --caliber-radius-xl: 8px;
  --caliber-radius-full: 50%;
}

/* Note: CaliberBRM uses sharp corners by default (border-radius: 0) */
/* Override with .rounded, .rounded-sm, .rounded-lg classes */
```

---

## Component Catalog

### 1. Layout Components

#### 1.1 Container
```html
<!-- Standard page container -->
<div class="container">
  <!-- Fixed width, responsive -->
</div>

<!-- Full-width container -->
<div class="container-fluid">
  <!-- 100% width -->
</div>
```

#### 1.2 Grid System
```html
<!-- Bootstrap 5 Grid (12-column) -->
<div class="row">
  <div class="col-sm-12 col-md-6 col-lg-3">
    <!-- Responsive columns -->
  </div>
</div>

<!-- Gutters -->
<div class="row no-gutters">   <!-- No spacing -->
<div class="row g-3">          <!-- 1rem spacing -->
<div class="row gx-4 gy-2">    <!-- Custom horizontal/vertical -->
```

#### 1.3 Page Layout Structure
```html
<!-- Standard Transaction Page Layout -->
<section class="sub-page-layout sub-page-layout-transaction">
  <!-- Header Section (fixed at top) -->
  <header class="sub-page-layout-header navbar-fixed-top">
    <div class="sub-page-layout-header-main">
      <!-- Left: Pre-actions (breadcrumbs, back button) -->
      <div class="sub-page-layout-header-preactions">
        <!-- Navigation elements -->
      </div>

      <!-- Center: Title -->
      <div class="sub-page-layout-header-title">
        <div>
          <span class="caliber-sub-title-1" id="MainTitle">
            Main Heading
          </span>
        </div>
        <div class="trans_subtitle">
          <span class="caliber-sub-title-2" id="SubTitle">
            Subtitle / Breadcrumb
          </span>
        </div>
      </div>

      <!-- Right: Action Buttons -->
      <div class="sub-page-layout-header-actions">
        <div id="screenactionbuttons">
          <button type="submit" class="caliber-button-primary">
            Submit
          </button>
          <button type="button" class="caliber-button-tertiary">
            Cancel
          </button>
        </div>
      </div>
    </div>
  </header>

  <!-- Body Section (scrollable content) -->
  <section class="sub-page-layout-body">
    <div class="row no-gutters transaction-panel-container">
      <div class="auditPanels col">
        <div class="auditPanelContainer">
          <!-- Page content goes here -->
        </div>
      </div>
    </div>
  </section>
</section>
```

#### 1.4 Card Component
```html
<!-- Standard Card -->
<div class="card md-shadow-z-2">
  <div class="card-header">
    <h3 class="card-title">Card Title</h3>
  </div>
  <div class="card-body">
    <!-- Content -->
  </div>
  <div class="card-footer">
    <!-- Footer actions -->
  </div>
</div>

<!-- Collapsible Card -->
<div class="card">
  <div class="card-header" data-bs-toggle="collapse" data-bs-target="#collapse1">
    <h3 class="card-title">Expandable Section</h3>
  </div>
  <div id="collapse1" class="collapse show">
    <div class="card-body">
      <!-- Content -->
    </div>
  </div>
</div>
```

#### 1.5 Stage Dividers (Workflow Sections)
```html
<!-- Workflow stage container with colored background -->
<div class="stagediv stagediv-1" data-title="Preparation">
  <div class="row">
    <!-- Form fields specific to this stage -->
  </div>
</div>

<div class="stagediv stagediv-2" data-title="Issuance">
  <!-- Next stage -->
</div>

<!-- CSS Classes for stages -->
.stagediv-1 { background-color: var(--caliber-preparation); }
.stagediv-2 { background-color: var(--caliber-issuance); }
.stagediv-3 { background-color: var(--caliber-execution); }
.stagediv-4 { background-color: var(--caliber-closure); }
```

---

### 2. Form Components

#### 2.1 Text Input
```html
<!-- Standard Input -->
<div class="control-column col-sm-3">
  <div class="caliber-control-group grid-stack-item draggable">
    <div class="grid-stack-item-content">
      <div class="form-group">
        <label class="title-heading" for="inputId">
          Field Label
        </label>
        <span class="required">*</span>
        <input type="text"
               id="inputId"
               class="form-control caliber-textbox"
               placeholder="Enter value" />
        <span class="validation-message">Error message</span>
      </div>
    </div>
  </div>
</div>

<!-- Read-Only Input -->
<input type="text" class="form-control caliber-textbox" readonly />

<!-- Disabled Input -->
<input type="text" class="form-control caliber-textbox" disabled />

<!-- CSS Classes -->
.form-control {
  height: 38px;
  padding: 8px 12px;
  border: 1px solid var(--caliber-border);
  background: var(--caliber-input-bg);
  font-size: 14px;
}

.caliber-textbox:focus {
  border-color: var(--caliber-primary);
  outline: none;
  box-shadow: 0 0 0 2px rgba(54, 198, 211, 0.25);
}

.caliber-textbox[readonly] {
  background-color: var(--caliber-readonly-bg);
  cursor: not-allowed;
  border-style: dashed;
}
```

#### 2.2 Material Design Line Input (Floating Label)
```html
<div class="form-group form-md-line-input form-md-floating-label">
  <input type="text" class="form-control" id="mdInput" />
  <label for="mdInput">Floating Label</label>
  <span class="form-control-focus"></span>
  <span class="help-block">Helper text</span>
</div>

<!-- Features:
- Animated floating label on focus
- Bottom border line that animates on focus
- Focus color: #36c6d3
- Label transitions from placeholder to top label
-->
```

#### 2.3 Textarea
```html
<div class="form-group">
  <label for="description">Description</label>
  <textarea id="description"
            class="form-control caliber-textarea"
            rows="4"
            placeholder="Enter description"></textarea>
</div>

<!-- Auto-resizing textarea -->
<textarea class="form-control caliber-textarea auto-resize"></textarea>
```

#### 2.4 Select Dropdown
```html
<!-- Standard Select -->
<div class="form-group">
  <label for="country">Country</label>
  <select id="country" class="form-control caliber-select">
    <option value="">-- Select --</option>
    <option value="1">United States</option>
    <option value="2">Canada</option>
  </select>
</div>

<!-- Select2 Enhanced Dropdown (searchable, multi-select) -->
<select id="products"
        class="form-control select2"
        multiple
        data-placeholder="Select products">
  <option value="1">Product A</option>
  <option value="2">Product B</option>
</select>

<script>
$('.select2').select2({
  theme: 'bootstrap-5',
  width: '100%',
  allowClear: true
});
</script>
```

#### 2.5 Checkbox
```html
<!-- Standard Checkbox -->
<div class="form-check">
  <input type="checkbox" id="check1" class="form-check-input" />
  <label for="check1" class="form-check-label">
    Accept terms and conditions
  </label>
</div>

<!-- Checkbox Group -->
<div class="form-group">
  <label>Select Options</label>
  <div class="form-check">
    <input type="checkbox" id="opt1" name="options" value="1" />
    <label for="opt1">Option 1</label>
  </div>
  <div class="form-check">
    <input type="checkbox" id="opt2" name="options" value="2" />
    <label for="opt2">Option 2</label>
  </div>
</div>
```

#### 2.6 Radio Buttons
```html
<div class="form-group">
  <label>Select Status</label>
  <div class="form-check">
    <input type="radio" id="active" name="status" value="1" />
    <label for="active">Active</label>
  </div>
  <div class="form-check">
    <input type="radio" id="inactive" name="status" value="0" />
    <label for="inactive">Inactive</label>
  </div>
</div>
```

#### 2.7 Date/Time Picker
```html
<!-- Single Date Picker -->
<div class="form-group">
  <label for="startDate">Start Date</label>
  <input type="text"
         id="startDate"
         class="form-control caliber-datepicker"
         placeholder="Select date" />
</div>

<!-- Date Range Picker (Bootstrap DateRangePicker) -->
<div class="form-group">
  <label for="dateRange">Date Range</label>
  <input type="text"
         id="dateRange"
         class="form-control caliber-daterangepicker"
         placeholder="Select date range" />
</div>

<script>
// Single Date
$('.caliber-datepicker').daterangepicker({
  singleDatePicker: true,
  showDropdowns: true,
  locale: {
    format: 'DD/MM/YYYY'
  }
});

// Date Range
$('.caliber-daterangepicker').daterangepicker({
  startDate: moment().startOf('month'),
  endDate: moment().endOf('month'),
  locale: {
    format: 'DD/MM/YYYY'
  },
  ranges: {
    'Today': [moment(), moment()],
    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
    'This Month': [moment().startOf('month'), moment().endOf('month')]
  }
});
</script>
```

#### 2.8 File Upload
```html
<div class="form-group">
  <label for="fileUpload">Attach File</label>
  <input type="file"
         id="fileUpload"
         class="form-control caliber-file-input"
         accept=".pdf,.doc,.docx" />
  <small class="form-text text-muted">
    Accepted formats: PDF, DOC, DOCX (Max 10MB)
  </small>
</div>

<!-- Multiple Files -->
<input type="file" class="form-control" multiple />

<!-- Custom File Upload Button -->
<div class="file-upload-wrapper">
  <button type="button" class="caliber-button-secondary">
    <i class="ft-upload"></i> Choose File
  </button>
  <input type="file" class="file-upload-input" hidden />
  <span class="file-upload-name">No file chosen</span>
</div>
```

#### 2.9 Popup Selector (Entity Picker)
```html
<!-- Used for selecting related entities (Product, Area, Equipment, etc.) -->
<div class="form-group">
  <label class="title-heading">Product Name</label>
  <span class="required">*</span>

  <!-- Display selected value -->
  <div id="ProductPopupCodeDiv" class="popuplabel">
    <div class="popup-value-item">
      <span class="popup-value-item-text" title="Product ID">
        Product Code - Product Name
      </span>
      <span class="popup-value-item-close" onclick="clearSelection()">&times;</span>
    </div>
  </div>

  <!-- Button to open popup -->
  <button type="button"
          id="ProductPopupAddBtn"
          class="btn btn-popup"
          data-bs-toggle="modal"
          data-bs-target="#ProductPopupModal">
    <i class="ft-plus"></i><span class="add-item">Add Item</span>
  </button>

  <!-- Hidden field to store selected ID -->
  <input type="hidden" id="ProductId" name="ProductId" />
</div>

<!-- CSS for Popup Selector -->
.popuplabel {
  min-height: 38px;
  border: 1px solid var(--caliber-border);
  padding: 4px 8px;
  background: white;
  margin-bottom: 8px;
}

.popup-value-item {
  display: inline-flex;
  align-items: center;
  background: var(--caliber-primary);
  color: white;
  padding: 4px 8px;
  border-radius: 3px;
  margin: 2px;
}

.popup-value-item-close {
  margin-left: 8px;
  cursor: pointer;
  font-weight: bold;
  font-size: 18px;
}

.btn-popup {
  background: var(--caliber-primary);
  color: white;
  border: none;
  padding: 8px 16px;
  cursor: pointer;
}

.btn-popup:hover {
  background: var(--caliber-primary-dark);
}
```

#### 2.10 WYSIWYG Editor (Summernote)
```html
<div class="form-group">
  <label for="richText">Content</label>
  <textarea id="richText" class="summernote"></textarea>
</div>

<script>
$('.summernote').summernote({
  height: 300,
  minHeight: 200,
  maxHeight: 500,
  focus: true,
  toolbar: [
    ['style', ['style', 'bold', 'italic', 'underline', 'clear']],
    ['font', ['strikethrough', 'superscript', 'subscript']],
    ['fontsize', ['fontsize']],
    ['color', ['color']],
    ['para', ['ul', 'ol', 'paragraph']],
    ['height', ['height']],
    ['table', ['table']],
    ['insert', ['link', 'picture', 'video']],
    ['view', ['fullscreen', 'codeview', 'help']]
  ]
});
</script>
```

#### 2.11 Switch/Toggle
```html
<!-- Bootstrap Switch -->
<div class="form-check form-switch">
  <input type="checkbox"
         class="form-check-input"
         id="toggleSwitch"
         role="switch" />
  <label class="form-check-label" for="toggleSwitch">
    Enable Notifications
  </label>
</div>

<!-- Styled Switch (Bootstrap-Switch plugin) -->
<input type="checkbox"
       class="bootstrap-switch"
       data-on-text="ON"
       data-off-text="OFF"
       data-on-color="success"
       data-off-color="danger" />

<script>
$('.bootstrap-switch').bootstrapSwitch({
  size: 'normal',
  onColor: 'success',
  offColor: 'danger',
  onText: 'Yes',
  offText: 'No'
});
</script>
```

#### 2.12 Form Validation
```html
<!-- Client-Side Validation (jQuery Validate + Unobtrusive) -->
<form id="transactionForm" method="post">
  <div class="form-group">
    <label for="email">Email</label>
    <input type="email"
           id="email"
           name="email"
           class="form-control"
           data-val="true"
           data-val-required="Email is required"
           data-val-email="Invalid email format" />
    <span class="field-validation-valid text-danger"
          data-valmsg-for="email"></span>
  </div>

  <button type="submit" class="caliber-button-primary">Submit</button>
</form>

<script>
// Custom validation rules
$.validator.addMethod("positiveInteger", function(value, element) {
  return this.optional(element) || /^[1-9]\d*$/.test(value);
}, "Please enter a positive integer");

// Form validation setup
$('#transactionForm').validate({
  errorClass: 'text-danger',
  validClass: 'text-success',
  errorElement: 'span',
  highlight: function(element) {
    $(element).addClass('is-invalid').removeClass('is-valid');
  },
  unhighlight: function(element) {
    $(element).addClass('is-valid').removeClass('is-invalid');
  }
});
</script>
```

---

### 3. Data Display Components

#### 3.1 DataTable (jQuery DataTables)
```html
<!-- Standard DataTable -->
<div class="table-responsive">
  <table id="dataTable" class="table table-bordered table-striped">
    <thead class="table-dark">
      <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Status</th>
        <th>Date</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <!-- Rows populated via AJAX or server-side -->
    </tbody>
  </table>
</div>

<script>
$('#dataTable').DataTable({
  pageLength: 25,
  lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
  order: [[0, 'desc']],
  responsive: true,
  processing: true,
  serverSide: true, // For large datasets
  ajax: {
    url: '/api/data',
    type: 'POST',
    data: function(d) {
      d.additionalFilter = $('#filter').val();
    }
  },
  columns: [
    { data: 'id' },
    { data: 'name' },
    {
      data: 'status',
      render: function(data, type, row) {
        if (data === 1) {
          return '<span class="badge bg-success">Active</span>';
        } else {
          return '<span class="badge bg-danger">Inactive</span>';
        }
      }
    },
    {
      data: 'date',
      render: function(data) {
        return moment(data).format('DD/MM/YYYY');
      }
    },
    {
      data: null,
      orderable: false,
      render: function(data, type, row) {
        return `
          <button class="btn btn-sm btn-primary" onclick="edit(${row.id})">
            <i class="ft-edit"></i>
          </button>
          <button class="btn btn-sm btn-danger" onclick="deleteRow(${row.id})">
            <i class="ft-trash"></i>
          </button>
        `;
      }
    }
  ],
  dom: 'Bfrtip',
  buttons: [
    'copy', 'csv', 'excel', 'pdf', 'print'
  ],
  language: {
    search: "_INPUT_",
    searchPlaceholder: "Search records...",
    lengthMenu: "Show _MENU_ entries",
    info: "Showing _START_ to _END_ of _TOTAL_ entries",
    paginate: {
      first: "First",
      last: "Last",
      next: "Next",
      previous: "Previous"
    }
  }
});
</script>

<!-- CSS Customization -->
.table-bordered {
  border: 1px solid var(--caliber-border);
}

.table-striped tbody tr:nth-of-type(odd) {
  background-color: rgba(0, 0, 0, 0.02);
}

.table-hover tbody tr:hover {
  background-color: rgba(54, 198, 211, 0.1);
}
```

#### 3.2 Search Control Table
```html
<!-- Filter/Search Panel above DataTable -->
<table class="table-bordered search-control">
  <tr>
    <td>
      <label>Code</label>
      <input type="text" id="searchCode" class="form-control form-control-sm" />
    </td>
    <td>
      <label>Status</label>
      <select id="searchStatus" class="form-control form-control-sm">
        <option value="">All</option>
        <option value="1">Active</option>
        <option value="0">Inactive</option>
      </select>
    </td>
    <td>
      <label>Date Range</label>
      <input type="text" id="searchDateRange" class="form-control form-control-sm" />
    </td>
    <td class="align-middle">
      <button type="button" class="caliber-button-primary" onclick="searchTable()">
        <i class="ft-search"></i> Search
      </button>
      <button type="button" class="caliber-button-secondary" onclick="resetSearch()">
        <i class="ft-refresh-cw"></i> Reset
      </button>
    </td>
  </tr>
</table>

<div class="table-responsive mt-3">
  <table id="resultsTable" class="table">
    <!-- Table content -->
  </table>
</div>

<script>
function searchTable() {
  $('#resultsTable').DataTable().ajax.reload();
}

function resetSearch() {
  $('#searchCode').val('');
  $('#searchStatus').val('');
  $('#searchDateRange').val('');
  $('#resultsTable').DataTable().ajax.reload();
}
</script>
```

#### 3.3 Badge/Label
```html
<!-- Status Badges -->
<span class="badge bg-success">Active</span>
<span class="badge bg-warning">Pending</span>
<span class="badge bg-danger">Rejected</span>
<span class="badge bg-info">Draft</span>
<span class="badge bg-primary">Approved</span>

<!-- Pill Badges (rounded) -->
<span class="badge rounded-pill bg-success">New</span>

<!-- Count Badges -->
<button type="button" class="btn btn-primary">
  Notifications <span class="badge bg-light text-dark">4</span>
</button>

<!-- Custom Status Labels -->
<span class="label label-sm label-success">Completed</span>
<span class="label label-sm label-warning">In Progress</span>
<span class="label label-sm label-danger">Failed</span>
```

#### 3.4 Progress Bar
```html
<!-- Standard Progress Bar -->
<div class="progress">
  <div class="progress-bar"
       role="progressbar"
       style="width: 75%"
       aria-valuenow="75"
       aria-valuemin="0"
       aria-valuemax="100">
    75%
  </div>
</div>

<!-- Colored Progress Bars -->
<div class="progress">
  <div class="progress-bar bg-success" style="width: 25%">25%</div>
</div>

<!-- Striped & Animated -->
<div class="progress">
  <div class="progress-bar progress-bar-striped progress-bar-animated"
       style="width: 50%">
    Loading...
  </div>
</div>

<!-- Stacked Progress -->
<div class="progress">
  <div class="progress-bar bg-success" style="width: 30%">30%</div>
  <div class="progress-bar bg-warning" style="width: 20%">20%</div>
  <div class="progress-bar bg-danger" style="width: 15%">15%</div>
</div>

<!-- Timer Progress Bar (auto-countdown) -->
<div class="timer-progress-bar-container">
  <div class="timer-progress-bar" id="timerBar"></div>
</div>

<script>
// Auto-countdown progress bar
function startTimer(duration) {
  var bar = $('#timerBar');
  bar.css('width', '100%');

  var start = Date.now();
  var timer = setInterval(function() {
    var elapsed = Date.now() - start;
    var remaining = Math.max(0, duration - elapsed);
    var percent = (remaining / duration) * 100;

    bar.css('width', percent + '%');

    if (remaining === 0) {
      clearInterval(timer);
    }
  }, 10);
}
</script>
```

#### 3.5 Charts (ApexCharts)
```html
<div id="chart" style="height: 350px;"></div>

<script>
// Line Chart
var options = {
  series: [{
    name: "Sales",
    data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
  }],
  chart: {
    height: 350,
    type: 'line',
    zoom: {
      enabled: false
    },
    toolbar: {
      show: true
    }
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    curve: 'smooth',
    width: 2
  },
  title: {
    text: 'Monthly Sales',
    align: 'left'
  },
  grid: {
    row: {
      colors: ['#f3f3f3', 'transparent'],
      opacity: 0.5
    },
  },
  xaxis: {
    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
  },
  colors: ['#36c6d3']
};

var chart = new ApexCharts(document.querySelector("#chart"), options);
chart.render();
</script>

<!-- Bar Chart -->
<div id="barChart"></div>
<script>
var barOptions = {
  series: [{
    name: 'Count',
    data: [44, 55, 57, 56, 61, 58]
  }],
  chart: {
    type: 'bar',
    height: 350
  },
  plotOptions: {
    bar: {
      horizontal: false,
      columnWidth: '55%',
      endingShape: 'rounded'
    },
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    show: true,
    width: 2,
    colors: ['transparent']
  },
  xaxis: {
    categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
  },
  fill: {
    opacity: 1
  },
  colors: ['#36c6d3']
};

var barChart = new ApexCharts(document.querySelector("#barChart"), barOptions);
barChart.render();
</script>

<!-- Pie Chart -->
<div id="pieChart"></div>
<script>
var pieOptions = {
  series: [44, 55, 13, 43, 22],
  chart: {
    type: 'pie',
    height: 350
  },
  labels: ['Approved', 'Pending', 'Rejected', 'Draft', 'Completed'],
  colors: ['#32c5d2', '#f1c40f', '#e74c3c', '#95a5a6', '#2ecc71'],
  responsive: [{
    breakpoint: 480,
    options: {
      chart: {
        width: 200
      },
      legend: {
        position: 'bottom'
      }
    }
  }]
};

var pieChart = new ApexCharts(document.querySelector("#pieChart"), pieOptions);
pieChart.render();
</script>

<!-- Donut Chart -->
<div id="donutChart"></div>
<script>
var donutOptions = {
  ...pieOptions,
  chart: {
    type: 'donut',
    height: 350
  }
};
var donutChart = new ApexCharts(document.querySelector("#donutChart"), donutOptions);
donutChart.render();
</script>
```

#### 3.6 Plotly Charts (Advanced/Scientific)
```html
<div id="plotlyChart" style="width:100%; height:400px;"></div>

<script>
// Scatter Plot
var trace1 = {
  x: [1, 2, 3, 4],
  y: [10, 15, 13, 17],
  mode: 'markers',
  type: 'scatter',
  name: 'Series A',
  marker: { size: 12, color: '#36c6d3' }
};

var trace2 = {
  x: [2, 3, 4, 5],
  y: [16, 5, 11, 9],
  mode: 'lines',
  type: 'scatter',
  name: 'Series B'
};

var data = [trace1, trace2];

var layout = {
  title: 'Process Data Visualization',
  xaxis: { title: 'X Axis' },
  yaxis: { title: 'Y Axis' }
};

Plotly.newPlot('plotlyChart', data, layout, {responsive: true});
</script>

<!-- Flowchart (for IPQC/Process Flow) -->
<div id="flowchart"></div>
<script>
// Can use Plotly or specialized flowchart library
// For simple flowcharts, use SVG or HTML with arrows
</script>
```

#### 3.7 Audit Trail / Version Comparison
```html
<!-- Side-by-side audit view -->
<div class="audit-comparison-container">
  <div class="row">
    <!-- Left Panel: Old Version -->
    <div class="col-md-6 audit-panel">
      <div class="audit-panel-header">
        <h4>Version 1.0 (Old)</h4>
        <span class="audit-date">Modified: 15/01/2025 10:30 AM</span>
      </div>
      <div class="audit-panel-body">
        <div class="audit-field">
          <label>Product Name:</label>
          <span class="audit-value OldValCls">Product A</span>
        </div>
        <div class="audit-field">
          <label>Quantity:</label>
          <span class="audit-value ChangedOldValCls">100</span>
        </div>
      </div>
    </div>

    <!-- Right Panel: New Version -->
    <div class="col-md-6 audit-panel">
      <div class="audit-panel-header">
        <h4>Version 2.0 (New)</h4>
        <span class="audit-date">Modified: 20/01/2025 2:45 PM</span>
      </div>
      <div class="audit-panel-body">
        <div class="audit-field">
          <label>Product Name:</label>
          <span class="audit-value NOChangeOldValCls">Product A</span>
        </div>
        <div class="audit-field">
          <label>Quantity:</label>
          <span class="audit-value ChangedNewValCls">150</span>
        </div>
      </div>
    </div>
  </div>
</div>

<!-- CSS for Audit Highlighting -->
.ChangedOldValCls {
  background-color: #ffcccc; /* Light red - old value */
  text-decoration: line-through;
}

.ChangedNewValCls {
  background-color: #ccffcc; /* Light green - new value */
  font-weight: bold;
}

.NOChangeOldValCls {
  /* No change - normal display */
}

.OutofRangeValCls {
  color: red !important;
  font-weight: bold;
}
```

---

### 4. Button Components

#### 4.1 Primary Buttons
```html
<!-- Hierarchy of Buttons -->
<button type="button" class="caliber-button-primary">
  Primary Action
</button>

<button type="button" class="caliber-button-secondary">
  Secondary Action
</button>

<button type="button" class="caliber-button-tertiary">
  Tertiary Action
</button>

<!-- With Icons -->
<button type="button" class="caliber-button-primary">
  <i class="ft-save"></i> Save
</button>

<button type="button" class="caliber-button-secondary">
  <i class="ft-x"></i> Cancel
</button>

<!-- Sizes -->
<button class="caliber-button-primary btn-sm">Small</button>
<button class="caliber-button-primary">Normal</button>
<button class="caliber-button-primary btn-lg">Large</button>

<!-- States -->
<button class="caliber-button-primary" disabled>Disabled</button>
<button class="caliber-button-primary is-loading">
  <span class="spinner-border spinner-border-sm"></span> Loading...
</button>
```

#### 4.2 CSS Styling for Buttons
```css
.caliber-button-primary {
  background-color: var(--caliber-primary);
  color: white;
  border: none;
  padding: 10px 20px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.caliber-button-primary:hover {
  background-color: var(--caliber-primary-dark);
  box-shadow: 0 2px 8px rgba(54, 198, 211, 0.3);
}

.caliber-button-primary:active {
  transform: translateY(1px);
}

.caliber-button-primary:disabled {
  background-color: #ccc;
  cursor: not-allowed;
  opacity: 0.6;
}

.caliber-button-secondary {
  background-color: #486CAE;
  color: white;
  /* Similar styling */
}

.caliber-button-tertiary {
  background-color: transparent;
  color: var(--caliber-primary);
  border: 1px solid var(--caliber-primary);
}

.caliber-button-tertiary:hover {
  background-color: rgba(54, 198, 211, 0.1);
}
```

#### 4.3 Icon Buttons
```html
<!-- Round Icon Buttons -->
<button type="button" class="btn-icon btn-icon-primary">
  <i class="ft-edit"></i>
</button>

<button type="button" class="btn-icon btn-icon-danger">
  <i class="ft-trash"></i>
</button>

<button type="button" class="btn-icon btn-icon-success">
  <i class="ft-check"></i>
</button>

<!-- CSS -->
.btn-icon {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.btn-icon-primary {
  background: var(--caliber-primary);
  color: white;
}
```

#### 4.4 Button Group
```html
<div class="btn-group" role="group">
  <button type="button" class="caliber-button-secondary">Left</button>
  <button type="button" class="caliber-button-secondary">Middle</button>
  <button type="button" class="caliber-button-secondary">Right</button>
</div>

<!-- Dropdown Button -->
<div class="btn-group">
  <button type="button" class="caliber-button-primary dropdown-toggle"
          data-bs-toggle="dropdown">
    Actions
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="#">Edit</a></li>
    <li><a class="dropdown-item" href="#">Delete</a></li>
    <li><hr class="dropdown-divider"></li>
    <li><a class="dropdown-item" href="#">Archive</a></li>
  </ul>
</div>
```

---

### 5. Modal/Dialog Components

#### 5.1 Standard Modal (Bootstrap 5)
```html
<!-- Trigger Button -->
<button type="button" class="caliber-button-primary"
        data-bs-toggle="modal"
        data-bs-target="#standardModal">
  Open Modal
</button>

<!-- Modal Structure -->
<div class="modal fade" id="standardModal" tabindex="-1">
  <div class="modal-dialog modal-lg modal-dialog-centered">
    <div class="modal-content">
      <!-- Header -->
      <div class="modal-header">
        <h5 class="modal-title">Modal Title</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>

      <!-- Body -->
      <div class="modal-body">
        <p>Modal content goes here...</p>
      </div>

      <!-- Footer -->
      <div class="modal-footer">
        <button type="button" class="caliber-button-secondary" data-bs-dismiss="modal">
          Close
        </button>
        <button type="button" class="caliber-button-primary">
          Save Changes
        </button>
      </div>
    </div>
  </div>
</div>

<!-- Modal Sizes -->
<div class="modal-dialog modal-sm">    <!-- Small -->
<div class="modal-dialog">             <!-- Default -->
<div class="modal-dialog modal-lg">    <!-- Large -->
<div class="modal-dialog modal-xl">    <!-- Extra Large -->
<div class="modal-dialog modal-fullscreen">  <!-- Fullscreen -->

<!-- Scrollable Modal -->
<div class="modal-dialog modal-dialog-scrollable">
```

#### 5.2 Entity Selection Popup Modal
```html
<!-- Product/Area/Equipment Selection Popup -->
<div class="modal fade" id="ProductPopupModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Select Product</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>

      <div class="modal-body">
        <!-- Search Filter -->
        <div class="mb-3">
          <input type="text"
                 id="productSearch"
                 class="form-control"
                 placeholder="Search products..." />
        </div>

        <!-- Selection Table -->
        <div class="table-responsive">
          <table id="productSelectionTable" class="table table-hover">
            <thead>
              <tr>
                <th>Select</th>
                <th>Code</th>
                <th>Name</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr onclick="selectProduct(1, 'PROD001', 'Product A')">
                <td><input type="radio" name="productSelect" value="1" /></td>
                <td>PROD001</td>
                <td>Product A</td>
                <td><span class="badge bg-success">Active</span></td>
              </tr>
              <!-- More rows -->
            </tbody>
          </table>
        </div>
      </div>

      <div class="modal-footer">
        <button type="button" class="caliber-button-secondary" data-bs-dismiss="modal">
          Cancel
        </button>
        <button type="button" class="caliber-button-primary" onclick="confirmSelection()">
          Confirm
        </button>
      </div>
    </div>
  </div>
</div>

<script>
function selectProduct(id, code, name) {
  // Store selection
  $('#ProductId').val(id);
  $('#ProductPopupCodeDiv').html(`
    <div class="popup-value-item">
      <span class="popup-value-item-text">${code} - ${name}</span>
      <span class="popup-value-item-close" onclick="clearSelection()">&times;</span>
    </div>
  `);

  // Close modal
  $('#ProductPopupModal').modal('hide');
}

function clearSelection() {
  $('#ProductId').val('');
  $('#ProductPopupCodeDiv').html('');
}
</script>
```

#### 5.3 SweetAlert2 Dialogs
```html
<script>
// Success Message
Swal.fire({
  icon: 'success',
  title: 'Success!',
  text: 'Record saved successfully',
  confirmButtonText: 'OK',
  confirmButtonColor: '#36c6d3'
});

// Error Message
Swal.fire({
  icon: 'error',
  title: 'Error!',
  text: 'Failed to save record',
  confirmButtonText: 'OK'
});

// Confirmation Dialog
Swal.fire({
  title: 'Are you sure?',
  text: "You won't be able to revert this!",
  icon: 'warning',
  showCancelButton: true,
  confirmButtonColor: '#36c6d3',
  cancelButtonColor: '#e74c3c',
  confirmButtonText: 'Yes, delete it!',
  cancelButtonText: 'Cancel'
}).then((result) => {
  if (result.isConfirmed) {
    // Perform delete action
    deleteRecord();
  }
});

// E-Signature Dialog
Swal.fire({
  title: 'E-Signature Required',
  html: `
    <div class="form-group">
      <label>User ID</label>
      <input type="text" id="esignUserId" class="swal2-input" />
    </div>
    <div class="form-group">
      <label>Password</label>
      <input type="password" id="esignPassword" class="swal2-input" />
    </div>
    <div class="form-group">
      <label>Remarks</label>
      <textarea id="esignRemarks" class="swal2-textarea"></textarea>
    </div>
  `,
  showCancelButton: true,
  confirmButtonText: 'Sign',
  preConfirm: () => {
    return {
      userId: document.getElementById('esignUserId').value,
      password: document.getElementById('esignPassword').value,
      remarks: document.getElementById('esignRemarks').value
    };
  }
}).then((result) => {
  if (result.isConfirmed) {
    validateEsignature(result.value);
  }
});

// Loading Dialog
Swal.fire({
  title: 'Processing...',
  text: 'Please wait',
  allowOutsideClick: false,
  didOpen: () => {
    Swal.showLoading();
  }
});

// Close loading
Swal.close();
</script>
```

---

### 6. Toast Notifications

#### 6.1 Toastr Notifications
```html
<script>
// Configure Toastr
toastr.options = {
  closeButton: true,
  debug: false,
  newestOnTop: true,
  progressBar: true,
  positionClass: "toast-top-right",
  preventDuplicates: false,
  onclick: null,
  showDuration: "300",
  hideDuration: "1000",
  timeOut: "5000",
  extendedTimeOut: "1000",
  showEasing: "swing",
  hideEasing: "linear",
  showMethod: "fadeIn",
  hideMethod: "fadeOut"
};

// Success Toast
toastr.success('Record saved successfully', 'Success');

// Error Toast
toastr.error('Failed to save record', 'Error');

// Warning Toast
toastr.warning('Please fill all required fields', 'Warning');

// Info Toast
toastr.info('This is an informational message', 'Info');

// Custom Toast
toastr.success('Custom message', 'Title', {
  timeOut: 10000,
  closeButton: true,
  progressBar: true,
  onclick: function() {
    alert('Toast clicked!');
  }
});
</script>

<!-- Positions -->
toast-top-right (default)
toast-top-left
toast-top-center
toast-top-full-width
toast-bottom-right
toast-bottom-left
toast-bottom-center
toast-bottom-full-width
```

---

### 7. Navigation Components

#### 7.1 Tabs
```html
<!-- Bootstrap Tabs -->
<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active"
            data-bs-toggle="tab"
            data-bs-target="#tab1">
      Tab 1
    </button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link"
            data-bs-toggle="tab"
            data-bs-target="#tab2">
      Tab 2
    </button>
  </li>
</ul>

<div class="tab-content mt-3">
  <div class="tab-pane fade show active" id="tab1">
    <p>Content for Tab 1</p>
  </div>
  <div class="tab-pane fade" id="tab2">
    <p>Content for Tab 2</p>
  </div>
</div>

<!-- Pills Variant -->
<ul class="nav nav-pills">
  <!-- Same structure -->
</ul>
```

#### 7.2 Breadcrumb
```html
<nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><a href="/">Home</a></li>
    <li class="breadcrumb-item"><a href="/module">Module</a></li>
    <li class="breadcrumb-item active">Current Page</li>
  </ol>
</nav>
```

#### 7.3 Pagination
```html
<nav>
  <ul class="pagination">
    <li class="page-item disabled">
      <a class="page-link" href="#" tabindex="-1">Previous</a>
    </li>
    <li class="page-item active">
      <a class="page-link" href="#">1</a>
    </li>
    <li class="page-item">
      <a class="page-link" href="#">2</a>
    </li>
    <li class="page-item">
      <a class="page-link" href="#">3</a>
    </li>
    <li class="page-item">
      <a class="page-link" href="#">Next</a>
    </li>
  </ul>
</nav>

<!-- Sizes -->
<ul class="pagination pagination-sm">  <!-- Small -->
<ul class="pagination">                <!-- Default -->
<ul class="pagination pagination-lg">  <!-- Large -->
```

#### 7.4 Sidebar Menu
```html
<div class="page-sidebar-wrapper">
  <div class="page-sidebar">
    <ul class="page-sidebar-menu">
      <!-- Menu Item with Submenu -->
      <li class="nav-item">
        <a href="#" class="nav-link nav-toggle">
          <i class="ft-layout"></i>
          <span class="title">Dashboard</span>
          <span class="arrow"></span>
        </a>
        <ul class="sub-menu">
          <li class="nav-item">
            <a href="/dashboard/analytics" class="nav-link">
              <span class="title">Analytics</span>
            </a>
          </li>
        </ul>
      </li>

      <!-- Single Menu Item -->
      <li class="nav-item active">
        <a href="/transactions" class="nav-link">
          <i class="ft-file-text"></i>
          <span class="title">Transactions</span>
        </a>
      </li>
    </ul>
  </div>
</div>
```

---

### 8. Alert/Banner Components

```html
<!-- Bootstrap Alerts -->
<div class="alert alert-success alert-dismissible" role="alert">
  <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  <strong>Success!</strong> Your changes have been saved.
</div>

<div class="alert alert-info alert-dismissible" role="alert">
  <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  <i class="ft-info"></i> <strong>Info:</strong> This is an informational message.
</div>

<div class="alert alert-warning alert-dismissible" role="alert">
  <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  <i class="ft-alert-triangle"></i> <strong>Warning:</strong> Please review your input.
</div>

<div class="alert alert-danger alert-dismissible" role="alert">
  <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  <i class="ft-x-circle"></i> <strong>Error:</strong> An error occurred.
</div>

<!-- Custom CaliberBRM Message Display -->
<div class="alert alert-info alert-dismissable whiteSpace-preLine" role="alert">
  <div>@Model.Message</div>
  <button type="button" class="close" data-bs-dismiss="alert">&times;</button>
</div>
```

---

### 9. Loader/Spinner Components

```html
<!-- Bootstrap Spinners -->
<div class="spinner-border text-primary" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<div class="spinner-grow text-success" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Centered Loading Overlay -->
<div class="loading-overlay">
  <div class="loading-spinner">
    <div class="spinner-border text-primary" role="status"></div>
    <p class="mt-2">Loading...</p>
  </div>
</div>

<!-- CSS -->
.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.loading-spinner {
  text-align: center;
  color: white;
}

<!-- Button with Loader -->
<button class="caliber-button-primary" disabled>
  <span class="spinner-border spinner-border-sm me-2"></span>
  Processing...
</button>
```

---

### 10. Icon System

#### Available Icon Libraries
```html
<!-- Font Awesome -->
<i class="fa fa-user"></i>
<i class="fas fa-home"></i>
<i class="far fa-envelope"></i>

<!-- Bootstrap Icons -->
<i class="bi bi-house"></i>
<i class="bi bi-person-fill"></i>

<!-- Feather Icons -->
<i class="ft-user"></i>
<i class="ft-home"></i>
<i class="ft-settings"></i>

<!-- Simple Line Icons -->
<i class="icon-user"></i>
<i class="icon-home"></i>

<!-- Metro UI Icons -->
<i class="mif-user"></i>
<i class="mif-home"></i>
```

#### Common Icons by Category
```
Actions:
ft-save, ft-edit, ft-trash, ft-x, ft-check, ft-plus, ft-minus

Navigation:
ft-arrow-left, ft-arrow-right, ft-chevron-up, ft-chevron-down, ft-home

Files:
ft-file, ft-file-text, ft-folder, ft-download, ft-upload

Communication:
ft-mail, ft-phone, ft-message-circle, ft-bell

Status:
ft-check-circle, ft-x-circle, ft-alert-triangle, ft-info

Media:
ft-image, ft-video, ft-music, ft-camera

UI:
ft-settings, ft-search, ft-menu, ft-grid, ft-list
```

---

### 11. Utility Classes

#### Display Utilities
```css
.d-none           { display: none; }
.d-block          { display: block; }
.d-inline         { display: inline; }
.d-inline-block   { display: inline-block; }
.d-flex           { display: flex; }
.d-table          { display: table; }
.d-table-cell     { display: table-cell; }

/* Responsive variants: .d-sm-block, .d-md-flex, etc. */
```

#### Flexbox Utilities
```css
.flex-row         { flex-direction: row; }
.flex-column      { flex-direction: column; }
.justify-content-start
.justify-content-center
.justify-content-end
.justify-content-between
.align-items-start
.align-items-center
.align-items-end
.flex-wrap
.flex-nowrap
```

#### Visibility Utilities
```css
.visible          { visibility: visible; }
.invisible        { visibility: hidden; }
.hidden           { display: none; }
```

#### Text Utilities
```css
.text-left        { text-align: left; }
.text-center      { text-align: center; }
.text-right       { text-align: right; }
.text-uppercase   { text-transform: uppercase; }
.text-lowercase   { text-transform: lowercase; }
.text-capitalize  { text-transform: capitalize; }
.text-truncate    { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.whiteSpace-preLine { white-space: pre-line; }
```

#### Width & Height Utilities
```css
.w-25   { width: 25%; }
.w-50   { width: 50%; }
.w-75   { width: 75%; }
.w-100  { width: 100%; }
.w-auto { width: auto; }

/* Height variants: h-25, h-50, h-75, h-100, h-auto */
```

#### Position Utilities
```css
.position-static
.position-relative
.position-absolute
.position-fixed
.position-sticky

.top-0, .bottom-0, .start-0, .end-0
```

#### Border Utilities
```css
.border           { border: 1px solid #dee2e6; }
.border-top
.border-bottom
.border-start
.border-end
.border-0         { border: 0; }
.rounded          { border-radius: 4px; }
.rounded-circle   { border-radius: 50%; }
```

---

## Integration Patterns

### 1. Razor Pages Integration
```csharp
// Page Model (Trn.cshtml.cs)
public class TrnModel : PageModel
{
    [BindProperty]
    public YourEntityModel EntityModel { get; set; }

    public UICaptions UiCaptions { get; set; }
    public bool ShowMessage { get; set; }
    public string Message { get; set; }

    public IActionResult OnGet(int? id)
    {
        // Load data
        EntityModel = service.GetById(id);
        UiCaptions = captionService.GetCaptions();

        return Page();
    }

    public IActionResult OnPostForSubmit()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = service.Save(EntityModel);

        if (result.Success)
        {
            ShowMessage = true;
            Message = "Record saved successfully";
        }

        return Page();
    }
}
```

### 2. ViewComponents Pattern
```csharp
// ViewComponent Class
public class ProductListPopupVC : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string popupId, string targetField)
    {
        var model = new ProductListPopupViewModel
        {
            PopupId = popupId,
            TargetField = targetField,
            Products = await service.GetProductsAsync()
        };

        return View(model);
    }
}

// Usage in Razor Page
@await Component.InvokeAsync("ProductListPopupVC", new {
    popupId = "ProductPopup",
    targetField = "ProductId"
})
```

### 3. AJAX Data Loading Pattern
```javascript
// Common AJAX pattern for data retrieval
function loadData(url, data, successCallback) {
  $.ajax({
    url: url,
    type: 'POST',
    data: JSON.stringify(data),
    contentType: 'application/json',
    headers: {
      'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
    },
    success: function(response) {
      if (response.success) {
        successCallback(response.data);
      } else {
        toastr.error(response.message, 'Error');
      }
    },
    error: function(xhr, status, error) {
      toastr.error('An error occurred: ' + error, 'Error');
    }
  });
}

// Usage
loadData('/api/getProducts', { categoryId: 1 }, function(products) {
  // Populate dropdown or table
  populateProductDropdown(products);
});
```

### 4. Form Validation Pattern
```javascript
// Client-side validation
function validateForm() {
  var isValid = true;

  // Check required fields
  $('.required-field').each(function() {
    if ($(this).val() === '') {
      $(this).addClass('is-invalid');
      isValid = false;
    } else {
      $(this).removeClass('is-invalid');
    }
  });

  if (!isValid) {
    toastr.warning('Please fill all required fields', 'Validation');
  }

  return isValid;
}

// E-signature validation
function validateEsignature(userId, password, callback) {
  $.ajax({
    url: '/api/validateEsign',
    type: 'POST',
    data: { userId: userId, password: password },
    success: function(response) {
      if (response.isValid) {
        callback(true);
      } else {
        Swal.fire('Error', 'Invalid credentials', 'error');
        callback(false);
      }
    }
  });
}
```

### 5. DataTable Initialization Pattern
```javascript
// Standard DataTable initialization
function initializeDataTable(tableId, ajaxUrl, columns) {
  return $(tableId).DataTable({
    processing: true,
    serverSide: true,
    ajax: {
      url: ajaxUrl,
      type: 'POST',
      data: function(d) {
        // Add custom filters
        d.customFilter = $('#filterValue').val();
      }
    },
    columns: columns,
    pageLength: 25,
    lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
    order: [[0, 'desc']],
    language: {
      search: "_INPUT_",
      searchPlaceholder: "Search..."
    },
    dom: 'Bfrtip',
    buttons: ['copy', 'excel', 'pdf', 'print']
  });
}

// Usage
var table = initializeDataTable('#listTable', '/api/getList', [
  { data: 'id' },
  { data: 'name' },
  { data: 'status', render: renderStatus },
  { data: null, render: renderActions, orderable: false }
]);
```

---

## Responsive Design Guidelines

### Breakpoints
```css
/* Extra small devices (phones, less than 576px) */
@media (max-width: 575.98px) { }

/* Small devices (tablets, 576px and up) */
@media (min-width: 576px) { }

/* Medium devices (desktops, 768px and up) */
@media (min-width: 768px) { }

/* Large devices (large desktops, 992px and up) */
@media (min-width: 992px) { }

/* Extra large devices (larger desktops, 1200px and up) */
@media (min-width: 1200px) { }
```

### Responsive Column Classes
```html
<!-- Stack on mobile, 2 columns on tablet, 4 columns on desktop -->
<div class="row">
  <div class="col-12 col-sm-6 col-lg-3">Column</div>
  <div class="col-12 col-sm-6 col-lg-3">Column</div>
  <div class="col-12 col-sm-6 col-lg-3">Column</div>
  <div class="col-12 col-sm-6 col-lg-3">Column</div>
</div>
```

---

## Accessibility Guidelines

### ARIA Attributes
```html
<!-- Buttons -->
<button type="button" aria-label="Close">&times;</button>

<!-- Form Controls -->
<input type="text"
       id="username"
       aria-describedby="usernameHelp"
       aria-required="true" />
<small id="usernameHelp">Enter your username</small>

<!-- Modals -->
<div class="modal"
     role="dialog"
     aria-labelledby="modalTitle"
     aria-modal="true">
  <h5 id="modalTitle">Modal Title</h5>
</div>

<!-- Alerts -->
<div role="alert" aria-live="assertive" aria-atomic="true">
  Important message
</div>
```

### Keyboard Navigation
- All interactive elements must be keyboard accessible (Tab navigation)
- Focus indicators must be visible
- Modal traps focus within dialog
- Escape key closes modals/dropdowns

---

## Performance Best Practices

### 1. Lazy Loading
```javascript
// Lazy load images
<img data-src="image.jpg" class="lazy" />

// Initialize lazy loading
document.addEventListener("DOMContentLoaded", function() {
  var lazyImages = [].slice.call(document.querySelectorAll("img.lazy"));

  if ("IntersectionObserver" in window) {
    let lazyImageObserver = new IntersectionObserver(function(entries) {
      entries.forEach(function(entry) {
        if (entry.isIntersecting) {
          let lazyImage = entry.target;
          lazyImage.src = lazyImage.dataset.src;
          lazyImage.classList.remove("lazy");
          lazyImageObserver.unobserve(lazyImage);
        }
      });
    });

    lazyImages.forEach(function(lazyImage) {
      lazyImageObserver.observe(lazyImage);
    });
  }
});
```

### 2. Debouncing Search Inputs
```javascript
function debounce(func, wait) {
  let timeout;
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}

// Usage
$('#searchInput').on('input', debounce(function() {
  performSearch($(this).val());
}, 300));
```

### 3. Minification & Bundling
- Use minified CSS/JS in production
- Combine multiple CSS files into one bundle
- Combine multiple JS files into one bundle
- Use CDN for common libraries

---

## Security Considerations

### 1. CSRF Protection
```html
<!-- Include anti-forgery token in all forms -->
@Html.AntiForgeryToken()

<!-- Include in AJAX requests -->
<script>
$.ajax({
  url: '/api/endpoint',
  type: 'POST',
  headers: {
    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
  },
  data: data
});
</script>
```

### 2. Input Sanitization
```javascript
// Escape HTML to prevent XSS
function escapeHtml(text) {
  return $('<div>').text(text).html();
}

// Usage
$('#display').text(userInput); // Safe
$('#display').html(escapeHtml(userInput)); // Safe
```

### 3. E-Signature Validation
```javascript
// Always validate e-signatures server-side
// Never trust client-side validation alone
function validateEsignature(data) {
  return $.ajax({
    url: '/api/validateEsign',
    type: 'POST',
    data: {
      userId: data.userId,
      password: data.password, // Sent over HTTPS
      transactionId: data.transactionId
    }
  });
}
```

---

## Testing Guidelines

### 1. Browser Testing
- Chrome (latest)
- Firefox (latest)
- Edge (latest)
- Safari (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

### 2. Responsive Testing
- Test all breakpoints: 320px, 768px, 992px, 1200px, 1920px
- Test orientation changes (portrait/landscape)
- Test touch interactions on tablets

### 3. Accessibility Testing
- Keyboard navigation
- Screen reader compatibility (NVDA, JAWS)
- Color contrast ratios (WCAG AA minimum)

---

## Sample Component Usage Examples

### Complete Form Example
```html
<form id="productForm" method="post">
  @Html.AntiForgeryToken()

  <div class="row">
    <!-- Text Input -->
    <div class="col-sm-6">
      <div class="form-group">
        <label for="productCode">Product Code <span class="required">*</span></label>
        <input type="text"
               id="productCode"
               name="ProductCode"
               class="form-control caliber-textbox"
               required />
      </div>
    </div>

    <!-- Dropdown -->
    <div class="col-sm-6">
      <div class="form-group">
        <label for="category">Category <span class="required">*</span></label>
        <select id="category"
                name="CategoryId"
                class="form-control select2"
                required>
          <option value="">-- Select Category --</option>
        </select>
      </div>
    </div>

    <!-- Date Picker -->
    <div class="col-sm-6">
      <div class="form-group">
        <label for="expiryDate">Expiry Date</label>
        <input type="text"
               id="expiryDate"
               name="ExpiryDate"
               class="form-control caliber-datepicker" />
      </div>
    </div>

    <!-- Checkbox -->
    <div class="col-sm-6">
      <div class="form-check mt-4">
        <input type="checkbox"
               id="isActive"
               name="IsActive"
               class="form-check-input" />
        <label for="isActive" class="form-check-label">Active</label>
      </div>
    </div>

    <!-- Textarea -->
    <div class="col-sm-12">
      <div class="form-group">
        <label for="description">Description</label>
        <textarea id="description"
                  name="Description"
                  class="form-control caliber-textarea"
                  rows="4"></textarea>
      </div>
    </div>
  </div>

  <!-- Action Buttons -->
  <div class="form-actions mt-4">
    <button type="submit" class="caliber-button-primary">
      <i class="ft-save"></i> Save
    </button>
    <button type="button" class="caliber-button-secondary" onclick="resetForm()">
      <i class="ft-refresh-cw"></i> Reset
    </button>
    <button type="button" class="caliber-button-tertiary" onclick="goBack()">
      <i class="ft-x"></i> Cancel
    </button>
  </div>
</form>

<script>
$(document).ready(function() {
  // Initialize Select2
  $('.select2').select2({ theme: 'bootstrap-5' });

  // Initialize Date Picker
  $('.caliber-datepicker').daterangepicker({
    singleDatePicker: true,
    locale: { format: 'DD/MM/YYYY' }
  });

  // Form Validation
  $('#productForm').validate({
    rules: {
      ProductCode: { required: true, minlength: 3 },
      CategoryId: { required: true }
    },
    messages: {
      ProductCode: {
        required: "Product code is required",
        minlength: "Product code must be at least 3 characters"
      },
      CategoryId: "Please select a category"
    },
    submitHandler: function(form) {
      submitForm();
      return false;
    }
  });
});

function submitForm() {
  var formData = $('#productForm').serialize();

  $.ajax({
    url: '/api/saveProduct',
    type: 'POST',
    data: formData,
    success: function(response) {
      if (response.success) {
        toastr.success('Product saved successfully', 'Success');
        setTimeout(function() {
          window.location.href = '/products/list';
        }, 1500);
      } else {
        toastr.error(response.message, 'Error');
      }
    },
    error: function() {
      toastr.error('An error occurred', 'Error');
    }
  });
}

function resetForm() {
  $('#productForm')[0].reset();
  $('.select2').val('').trigger('change');
}

function goBack() {
  window.history.back();
}
</script>
```

---

## Conclusion

This specification provides a comprehensive foundation for building the **RazorPlus** UI library based on CaliberBRM's proven patterns. The library should:

1. **Maintain Consistency**: All components follow CaliberBRM's established design language
2. **Be Modular**: Each component can be used independently or composed together
3. **Support Accessibility**: WCAG 2.1 AA compliant
4. **Be Responsive**: Mobile-first approach with touch support
5. **Ensure Performance**: Optimized for large-scale enterprise applications
6. **Provide Security**: Built-in CSRF protection, input sanitization, e-signature support
7. **Enable Extensibility**: Easy to customize and extend for specific needs

### Next Steps for RazorPlus Development:

1. Create component library structure (CSS + JS + Razor Tag Helpers)
2. Implement base components (buttons, inputs, modals)
3. Build advanced components (DataTables, charts, form builders)
4. Create comprehensive documentation with live examples
5. Develop sample templates for common page types
6. Build CLI tool for component scaffolding
7. Create NuGet package for easy distribution

---

**Document Version**: 1.0
**Last Updated**: January 2025
**Based on**: CaliberBRM 5.2 (NET 9.0)
