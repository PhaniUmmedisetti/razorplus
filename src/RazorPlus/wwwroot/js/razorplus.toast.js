/**
 * RazorPlus Toast Notification System
 * Lightweight toast notifications with auto-dismiss, stacking, and variants
 * No external dependencies - pure JavaScript
 */

let toastContainer = null;
let toastIdCounter = 0;
const activeToasts = new Map();

// Toast configuration defaults
const defaults = {
  variant: 'info',      // 'success', 'info', 'warning', 'danger'
  duration: 4000,       // Auto-dismiss duration in ms (0 = no auto-dismiss)
  position: 'top-right', // 'top-right', 'top-left', 'top-center', 'bottom-right', 'bottom-left', 'bottom-center'
  dismissible: true,    // Show close button
  icon: null,          // Custom icon (defaults to variant icon)
  action: null         // { text: 'Action', onClick: function }
};

// Variant icons
const variantIcons = {
  success: '✓',
  info: 'ℹ',
  warning: '⚠',
  danger: '✕'
};

// Ensure toast container exists
function ensureContainer(position = 'top-right') {
  if (!toastContainer || toastContainer.dataset.position !== position) {
    // Remove old container if position changed
    if (toastContainer) {
      toastContainer.remove();
    }

    toastContainer = document.createElement('div');
    toastContainer.className = `rp-toast-container rp-toast-container--${position}`;
    toastContainer.dataset.position = position;
    toastContainer.setAttribute('aria-live', 'polite');
    toastContainer.setAttribute('aria-atomic', 'false');
    document.body.appendChild(toastContainer);
  }
  return toastContainer;
}

// Build toast element
function buildToast(id, message, options) {
  const toast = document.createElement('div');
  toast.className = `rp-toast rp-toast--${options.variant}`;
  toast.dataset.toastId = id;
  toast.setAttribute('role', 'alert');
  toast.setAttribute('aria-live', 'assertive');
  toast.setAttribute('aria-atomic', 'true');

  let html = '<div class="rp-toast__content">';

  // Icon
  const icon = options.icon || variantIcons[options.variant] || '';
  if (icon) {
    html += `<div class="rp-toast__icon" aria-hidden="true">${icon}</div>`;
  }

  // Message
  html += `<div class="rp-toast__message">${message}</div>`;

  html += '</div>';

  // Action button
  if (options.action) {
    html += `<button type="button" class="rp-toast__action">${options.action.text}</button>`;
  }

  // Close button
  if (options.dismissible) {
    html += '<button type="button" class="rp-toast__close" aria-label="Close">&times;</button>';
  }

  toast.innerHTML = html;

  // Add event listeners
  if (options.dismissible) {
    const closeBtn = toast.querySelector('.rp-toast__close');
    closeBtn?.addEventListener('click', () => dismissToast(id));
  }

  if (options.action?.onClick) {
    const actionBtn = toast.querySelector('.rp-toast__action');
    actionBtn?.addEventListener('click', () => {
      options.action.onClick();
      dismissToast(id);
    });
  }

  return toast;
}

// Show toast
function showToast(message, options = {}) {
  const opts = { ...defaults, ...options };
  const id = ++toastIdCounter;

  const container = ensureContainer(opts.position);
  const toast = buildToast(id, message, opts);

  // Animation: slide in
  toast.style.opacity = '0';
  toast.style.transform = opts.position.startsWith('top') ? 'translateY(-20px)' : 'translateY(20px)';

  container.appendChild(toast);

  // Trigger animation
  requestAnimationFrame(() => {
    toast.style.transition = 'all 0.3s ease';
    toast.style.opacity = '1';
    toast.style.transform = 'translateY(0)';
  });

  // Auto-dismiss
  let timeoutId = null;
  if (opts.duration > 0) {
    timeoutId = setTimeout(() => {
      dismissToast(id);
    }, opts.duration);
  }

  activeToasts.set(id, { toast, timeoutId, options: opts });

  return id;
}

// Dismiss toast
function dismissToast(id) {
  const toastData = activeToasts.get(id);
  if (!toastData) return;

  const { toast, timeoutId, options } = toastData;

  // Clear auto-dismiss timer
  if (timeoutId) {
    clearTimeout(timeoutId);
  }

  // Animation: slide out
  toast.style.transition = 'all 0.3s ease';
  toast.style.opacity = '0';
  toast.style.transform = options.position.startsWith('top') ? 'translateY(-20px)' : 'translateY(20px)';

  setTimeout(() => {
    toast.remove();
    activeToasts.delete(id);

    // Remove container if no toasts left
    if (activeToasts.size === 0 && toastContainer) {
      toastContainer.remove();
      toastContainer = null;
    }
  }, 300);
}

// Dismiss all toasts
function dismissAll() {
  const ids = Array.from(activeToasts.keys());
  ids.forEach(id => dismissToast(id));
}

// Convenience methods
export function success(message, options = {}) {
  return showToast(message, { ...options, variant: 'success' });
}

export function info(message, options = {}) {
  return showToast(message, { ...options, variant: 'info' });
}

export function warning(message, options = {}) {
  return showToast(message, { ...options, variant: 'warning' });
}

export function danger(message, options = {}) {
  return showToast(message, { ...options, variant: 'danger' });
}

export function error(message, options = {}) {
  return showToast(message, { ...options, variant: 'danger' });
}

// Export main API
export const toast = {
  show: showToast,
  dismiss: dismissToast,
  dismissAll,
  success,
  info,
  warning,
  danger,
  error
};

// Global API
if (typeof window !== 'undefined') {
  window.RazorPlus = window.RazorPlus || {};
  window.RazorPlus.toast = toast;
}

export default toast;
