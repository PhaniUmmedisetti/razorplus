# Changelog

All notable changes to RazorPlus will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2025-01-XX

### Added

#### **CaliberBRM Enterprise Theme**
- New opt-in theme for enterprise applications (`razorplus-theme-caliber.css`)
- Cyan/teal primary colors (#36c6d3)
- Lightcyan input backgrounds for better visual distinction
- Three-tier button hierarchy (primary, secondary, tertiary)
- Workflow stage colors for transaction processing
- Material Design shadow system (`md-shadow-z-1` through `md-shadow-z-5`)

#### **New Components**
- **Stage Divider** (`<rp-stage-divider>`) - Colored containers for workflow sections
  - 8 predefined stage colors (preparation, issuance, execution, etc.)
  - Title display via data attribute
  - Custom background color support
- **Popup Selector** (`<rp-popup-selector>`) - Entity picker with modal integration
  - Selected value display with chip design
  - Clear selection functionality
  - Modal integration for selection UI
  - Hidden field for form submission

#### **UI Enhancements**
- Badge component classes (`.rp-badge` with variants: primary, success, warning, danger, info)
- Alert component classes (`.rp-alert` with variants matching badges)
- Card component classes (`.rp-card`, `.rp-card-header`, `.rp-card-body`, `.rp-card-footer`)
- Material Design shadow utility classes
- Button success variant (`.rp-btn--success`)
- Button tertiary variant (`.rp-btn--tertiary`) for outline buttons

#### **Documentation**
- `VERSIONING.md` - Comprehensive versioning and migration guide
- CaliberBRM integration examples
- Theme switching documentation
- Custom theme creation guide

### Changed

#### **CSS Architecture**
- Refactored CSS variable system for better theme support
- All colors now use CSS custom properties with fallbacks
- Border radius now respects theme (CaliberBRM uses sharp corners)
- Shadow system expanded to support Material Design levels

#### **Component Improvements**
- Buttons now support `success` and `tertiary` variants
- Input backgrounds now themeable via `--rp-input-bg` variable
- Read-only inputs now use dashed border style
- Enhanced focus states with theme-aware colors

### Fixed

#### **Accessibility**
- Improved ARIA labels for popup selectors
- Better keyboard navigation for new components
- Focus indicators now respect theme colors

#### **Responsive Design**
- Stage dividers now have reduced padding on mobile
- Popup selectors stack better on small screens

### Internal

#### **Build & Package**
- Updated `.csproj` with NuGet package metadata
- Added version number (1.1.0)
- Added package tags for better discoverability
- Configured release notes in package

---

## [1.0.0] - 2024-12-XX

### Added

#### **Core Components**
- **Button** (`<rp-button>`) - Variants: primary, secondary, ghost, danger
  - Size options: sm, md, lg
  - Block layout support
  - Icon support
  - Loading state
  - Link variant (`as="a"`)

- **Input** (`<rp-input>`) - Single-line text input
  - Model binding via `asp-for`
  - Label, hint, validation support
  - Prefix/suffix support
  - Required field indicator

- **Textarea** (`<rp-textarea>`) - Multi-line text input
  - Configurable rows
  - Auto-resize option
  - Model binding

- **Select** (`<rp-select>`) - Dropdown selection
  - Single/multiple selection
  - Client-side filtering
  - Server-side async search
  - Clearable option
  - SelectListItem binding

- **Switch** (`<rp-switch>`) - Toggle switch
  - Boolean model binding
  - On/off text labels
  - Animated transitions

- **Radio Group** (`<rp-radio-group>`) - Radio button group
  - Vertical/horizontal layout
  - Fieldset/legend structure
  - Keyboard navigation

- **Validation Message** (`<rp-validation-message>`) - Standalone validation display

#### **Structural Components**
- **Tabs** (`<rp-tabs>`, `<rp-tab>`) - Tab navigation
  - ARIA roles
  - Keyboard navigation (arrow keys, home, end)
  - Auto-generated tablist

- **Accordion** (`<rp-accordion>`, `<rp-accordion-item>`) - Collapsible sections
  - ARIA roles
  - Keyboard support
  - Multiple items can be open

- **Modal** (`<rp-modal>`) - Dialog overlay
  - Focus trap
  - Backdrop close (optional)
  - ESC key close
  - Header/body/footer sections
  - Scroll lock

#### **Data Display Components**
- **Table** (`<rp-table>`, `<rp-column>`) - Data table
  - Server-side sorting/paging
  - Client-side sorting/paging
  - Column alignment
  - Empty state
  - Custom cell templates

- **Pagination** (`<rp-pagination>`) - Page navigation
  - Window-based page links
  - First/last with ellipsis
  - Query string preservation

- **Chart** (`<rp-chart>`) - ECharts integration
  - Lazy loading via CDN
  - Theme support (light/dark/auto)
  - Auto-resize
  - Export support

#### **Design System**
- CSS custom properties for theming
- Dark mode support via `[data-theme="dark"]`
- Responsive breakpoints
- Spacing scale (4px grid)
- Typography scale
- Shadow system
- Color palette (primary, danger, muted, etc.)

#### **JavaScript Features**
- Progressive enhancement pattern
- Idempotent component initialization
- WeakMap-based state management
- Lazy module loading
- Auto-initialization on DOM ready
- Global `RazorPlus` API for modal control

#### **Accessibility**
- ARIA attributes throughout
- Keyboard navigation support
- Screen reader friendly
- Focus indicators
- Semantic HTML
- Skip links ready

#### **Documentation**
- README.md with quick start guide
- CLAUDE.md with complete architecture documentation
- Component usage examples
- TypeScript definitions
- Sample application

### Technical Details

#### **Framework Support**
- .NET 6.0
- .NET 7.0
- ASP.NET Core Razor Pages
- ASP.NET Core MVC

#### **Browser Support**
- Modern browsers (Chrome, Firefox, Edge, Safari)
- Progressive enhancement for older browsers
- Touch device support
- Responsive design

#### **File Structure**
```
src/RazorPlus/
├── TagHelpers/          # All TagHelper implementations
├── wwwroot/
│   ├── css/
│   │   └── razorplus.css    # Complete stylesheet
│   └── js/
│       ├── razorplus.core.js   # Core initialization
│       ├── razorplus.select.js # Select enhancements
│       ├── razorplus.table.js  # Table features
│       └── razorplus.chart.js  # ECharts integration
└── RazorPlus.csproj
```

---

## Future Releases

### [1.2.0] - Planned

**Potential Features:**
- Material Design floating label inputs
- Audit trail component for version comparison
- Search control table component
- Page layout components (header/body/footer structure)
- Toast notification component
- Dropdown menu component
- Breadcrumb component
- Progress bar component
- Loading spinner component

### [2.0.0] - Future

**Potential Breaking Changes:**
- .NET 8.0 minimum requirement
- Refactored JavaScript architecture (ES modules)
- CSS modernization (CSS Grid, Container Queries)
- Component API standardization

---

## Version Comparison

| Feature                  | 1.0.0 | 1.1.0 |
|-------------------------|-------|-------|
| Form Components         | 6     | 6     |
| Structural Components   | 3     | 3     |
| Data Components         | 3     | 3     |
| Enterprise Components   | 0     | 2     |
| Themes                  | 1     | 2     |
| Shadow Levels           | 1     | 5     |
| Button Variants         | 4     | 6     |
| Badge Components        | 0     | 5     |
| Alert Components        | 0     | 4     |
| Card Components         | 0     | 1     |

---

## Upgrade Path

- **1.0.x → 1.1.x**: No breaking changes, optional theme adoption
- **1.x.x → 2.0.0**: Breaking changes expected (with 6-month deprecation warnings)

---

## Links

- [GitHub Repository](https://github.com/razorplus/razorplus)
- [Documentation](./README.md)
- [Versioning Guide](./VERSIONING.md)
- [Architecture Guide](./CLAUDE.md)
- [Specification](./RAZORPLUS_SPECIFICATION.md)
