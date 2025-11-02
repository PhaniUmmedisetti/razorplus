/**
 * RazorPlus Dropdown Module
 * Accessible dropdown menus with keyboard navigation and nested submenus
 * No external dependencies - pure JavaScript
 */

const dropdowns = new WeakMap();

// Initialize dropdown
export function enhanceDropdown(dropdown) {
  if (dropdown.dataset.rpEnhanced) return;
  dropdown.dataset.rpEnhanced = 'true';

  const trigger = dropdown.querySelector('.rp-dropdown__trigger');
  const menu = dropdown.querySelector('.rp-dropdown-menu');

  if (!trigger || !menu) return;

  const state = {
    isOpen: false,
    activeSubmenu: null
  };

  dropdowns.set(dropdown, state);

  // Toggle dropdown
  trigger.addEventListener('click', (e) => {
    e.stopPropagation();
    if (state.isOpen) {
      closeDropdown(dropdown);
    } else {
      openDropdown(dropdown);
    }
  });

  // Click outside to close
  document.addEventListener('click', (e) => {
    if (state.isOpen && !dropdown.contains(e.target)) {
      closeDropdown(dropdown);
    }
  });

  // Keyboard navigation
  trigger.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && state.isOpen) {
      closeDropdown(dropdown);
      e.preventDefault();
    } else if (e.key === 'ArrowDown' && state.isOpen) {
      focusFirstItem(menu);
      e.preventDefault();
    } else if (e.key === 'Enter' || e.key === ' ') {
      if (!state.isOpen) {
        openDropdown(dropdown);
        e.preventDefault();
      }
    }
  });

  // Menu keyboard navigation
  menu.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
      closeDropdown(dropdown);
      trigger.focus();
      e.preventDefault();
    } else if (e.key === 'ArrowDown') {
      focusNextItem(menu, e.target);
      e.preventDefault();
    } else if (e.key === 'ArrowUp') {
      focusPrevItem(menu, e.target);
      e.preventDefault();
    } else if (e.key === 'Home') {
      focusFirstItem(menu);
      e.preventDefault();
    } else if (e.key === 'End') {
      focusLastItem(menu);
      e.preventDefault();
    }
  });

  // Submenu triggers
  const submenuTriggers = menu.querySelectorAll('.rp-dropdown-submenu__trigger');
  submenuTriggers.forEach(submenuTrigger => {
    const submenu = submenuTrigger.closest('.rp-dropdown-submenu');
    const submenuMenu = submenu?.querySelector('.rp-dropdown-menu--submenu');

    if (!submenuMenu) return;

    // Hover to open submenu
    submenu.addEventListener('mouseenter', () => {
      openSubmenu(dropdown, submenu, submenuMenu);
    });

    submenu.addEventListener('mouseleave', () => {
      closeSubmenu(dropdown, submenu, submenuMenu);
    });

    // Keyboard navigation for submenu
    submenuTrigger.addEventListener('keydown', (e) => {
      if (e.key === 'ArrowRight') {
        openSubmenu(dropdown, submenu, submenuMenu);
        focusFirstItem(submenuMenu);
        e.preventDefault();
      } else if (e.key === 'Enter' || e.key === ' ') {
        openSubmenu(dropdown, submenu, submenuMenu);
        focusFirstItem(submenuMenu);
        e.preventDefault();
      }
    });

    submenuMenu.addEventListener('keydown', (e) => {
      if (e.key === 'ArrowLeft') {
        closeSubmenu(dropdown, submenu, submenuMenu);
        submenuTrigger.focus();
        e.preventDefault();
      }
    });
  });

  // Item clicks
  const items = menu.querySelectorAll('.rp-dropdown-item:not(.rp-dropdown-submenu__trigger)');
  items.forEach(item => {
    if (!item.hasAttribute('disabled')) {
      item.addEventListener('click', () => {
        // Close dropdown after action (unless it's a link)
        if (item.tagName !== 'A') {
          closeDropdown(dropdown);
        }
      });
    }
  });
}

// Open dropdown
function openDropdown(dropdown) {
  const state = dropdowns.get(dropdown);
  if (!state || state.isOpen) return;

  const trigger = dropdown.querySelector('.rp-dropdown__trigger');
  const menu = dropdown.querySelector('.rp-dropdown-menu');

  if (!trigger || !menu) return;

  state.isOpen = true;
  trigger.setAttribute('aria-expanded', 'true');
  menu.removeAttribute('hidden');

  // Position menu
  positionMenu(dropdown, menu);

  // Focus first item
  requestAnimationFrame(() => {
    focusFirstItem(menu);
  });
}

// Close dropdown
function closeDropdown(dropdown) {
  const state = dropdowns.get(dropdown);
  if (!state || !state.isOpen) return;

  const trigger = dropdown.querySelector('.rp-dropdown__trigger');
  const menu = dropdown.querySelector('.rp-dropdown-menu');

  if (!trigger || !menu) return;

  state.isOpen = false;
  trigger.setAttribute('aria-expanded', 'false');
  menu.setAttribute('hidden', 'hidden');

  // Close any open submenus
  if (state.activeSubmenu) {
    const submenuMenu = state.activeSubmenu.querySelector('.rp-dropdown-menu--submenu');
    if (submenuMenu) {
      submenuMenu.setAttribute('hidden', 'hidden');
    }
    state.activeSubmenu = null;
  }
}

// Open submenu
function openSubmenu(dropdown, submenu, submenuMenu) {
  const state = dropdowns.get(dropdown);
  if (!state) return;

  // Close previous submenu
  if (state.activeSubmenu && state.activeSubmenu !== submenu) {
    const prevMenu = state.activeSubmenu.querySelector('.rp-dropdown-menu--submenu');
    if (prevMenu) {
      prevMenu.setAttribute('hidden', 'hidden');
    }
  }

  state.activeSubmenu = submenu;
  submenuMenu.removeAttribute('hidden');

  // Position submenu
  positionSubmenu(submenu, submenuMenu);
}

// Close submenu
function closeSubmenu(dropdown, submenu, submenuMenu) {
  const state = dropdowns.get(dropdown);
  if (!state || state.activeSubmenu !== submenu) return;

  submenuMenu.setAttribute('hidden', 'hidden');
  state.activeSubmenu = null;
}

// Position menu
function positionMenu(dropdown, menu) {
  const rect = dropdown.getBoundingClientRect();
  const menuRect = menu.getBoundingClientRect();

  // Check if menu would go off-screen
  if (rect.bottom + menuRect.height > window.innerHeight) {
    menu.style.bottom = '100%';
    menu.style.top = 'auto';
  } else {
    menu.style.top = '100%';
    menu.style.bottom = 'auto';
  }

  if (rect.right > window.innerWidth - menuRect.width) {
    menu.style.right = '0';
    menu.style.left = 'auto';
  }
}

// Position submenu
function positionSubmenu(submenu, submenuMenu) {
  const rect = submenu.getBoundingClientRect();
  const menuRect = submenuMenu.getBoundingClientRect();

  // Check if submenu would go off-screen
  if (rect.right + menuRect.width > window.innerWidth) {
    submenuMenu.style.left = 'auto';
    submenuMenu.style.right = '100%';
  } else {
    submenuMenu.style.left = '100%';
    submenuMenu.style.right = 'auto';
  }

  if (rect.bottom + menuRect.height > window.innerHeight) {
    submenuMenu.style.bottom = '0';
    submenuMenu.style.top = 'auto';
  } else {
    submenuMenu.style.top = '0';
    submenuMenu.style.bottom = 'auto';
  }
}

// Focus management
function getFocusableItems(menu) {
  return Array.from(
    menu.querySelectorAll('.rp-dropdown-item:not([disabled]):not(.rp-dropdown-item--disabled)')
  ).filter(item => {
    // Only include direct children, not submenu items
    return item.parentElement === menu || item.closest('.rp-dropdown-menu') === menu;
  });
}

function focusFirstItem(menu) {
  const items = getFocusableItems(menu);
  if (items.length > 0) {
    items[0].focus();
  }
}

function focusLastItem(menu) {
  const items = getFocusableItems(menu);
  if (items.length > 0) {
    items[items.length - 1].focus();
  }
}

function focusNextItem(menu, currentItem) {
  const items = getFocusableItems(menu);
  const currentIndex = items.indexOf(currentItem);
  if (currentIndex < items.length - 1) {
    items[currentIndex + 1].focus();
  } else {
    items[0].focus(); // Wrap to first
  }
}

function focusPrevItem(menu, currentItem) {
  const items = getFocusableItems(menu);
  const currentIndex = items.indexOf(currentItem);
  if (currentIndex > 0) {
    items[currentIndex - 1].focus();
  } else {
    items[items.length - 1].focus(); // Wrap to last
  }
}

// Auto-enhance on load
export function enhanceDropdowns(root = document) {
  const allDropdowns = root.querySelectorAll('[data-rp-dropdown]');
  allDropdowns.forEach(enhanceDropdown);
}

// Auto-initialize
if (typeof window !== 'undefined') {
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => enhanceDropdowns());
  } else {
    enhanceDropdowns();
  }
}
