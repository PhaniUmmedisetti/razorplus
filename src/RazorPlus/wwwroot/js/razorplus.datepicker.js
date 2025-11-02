/**
 * RazorPlus Date Picker Module
 * Lightweight date picker with calendar popup, range selection, and time support
 * No external dependencies - pure JavaScript
 */

const datePickers = new WeakMap();

// Date formatting utilities
const dateUtils = {
  formats: {
    'MM/dd/yyyy': { pattern: /^(\d{2})\/(\d{2})\/(\d{4})$/, order: [1, 0, 2] },
    'dd/MM/yyyy': { pattern: /^(\d{2})\/(\d{2})\/(\d{4})$/, order: [0, 1, 2] },
    'yyyy-MM-dd': { pattern: /^(\d{4})-(\d{2})-(\d{2})$/, order: [2, 1, 0] },
  },

  parse(dateString, format) {
    const formatDef = this.formats[format] || this.formats['MM/dd/yyyy'];
    const match = dateString.match(formatDef.pattern);
    if (!match) return null;

    const [_, ...parts] = match;
    const [day, month, year] = formatDef.order.map(i => parseInt(parts[i]));
    const date = new Date(year, month, day);

    // Validate
    if (date.getDate() !== day || date.getMonth() !== month || date.getFullYear() !== year) {
      return null;
    }
    return date;
  },

  format(date, format) {
    if (!date) return '';
    const d = String(date.getDate()).padStart(2, '0');
    const m = String(date.getMonth() + 1).padStart(2, '0');
    const y = date.getFullYear();

    return format
      .replace('dd', d)
      .replace('MM', m)
      .replace('yyyy', y);
  },

  isSameDay(date1, date2) {
    return date1 && date2 &&
      date1.getDate() === date2.getDate() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getFullYear() === date2.getFullYear();
  },

  isInRange(date, start, end) {
    return date >= start && date <= end;
  },

  addMonths(date, months) {
    const result = new Date(date);
    result.setMonth(result.getMonth() + months);
    return result;
  },

  startOfMonth(date) {
    return new Date(date.getFullYear(), date.getMonth(), 1);
  },

  endOfMonth(date) {
    return new Date(date.getFullYear(), date.getMonth() + 1, 0);
  },

  getDaysInMonth(date) {
    return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
  },

  getFirstDayOfMonth(date) {
    return new Date(date.getFullYear(), date.getMonth(), 1).getDay();
  }
};

// Calendar UI builder
function buildCalendar(date, selectedDate, rangeStart, rangeEnd, minDate, maxDate) {
  const today = new Date();
  const year = date.getFullYear();
  const month = date.getMonth();
  const daysInMonth = dateUtils.getDaysInMonth(date);
  const firstDay = dateUtils.getFirstDayOfMonth(date);

  let html = '<div class="rp-calendar">';

  // Header
  html += '<div class="rp-calendar__header">';
  html += '<button type="button" class="rp-calendar__nav" data-action="prev-month" aria-label="Previous month">‹</button>';
  html += `<div class="rp-calendar__title">${date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' })}</div>`;
  html += '<button type="button" class="rp-calendar__nav" data-action="next-month" aria-label="Next month">›</button>';
  html += '</div>';

  // Weekday headers
  html += '<div class="rp-calendar__weekdays">';
  ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'].forEach(day => {
    html += `<div class="rp-calendar__weekday">${day}</div>`;
  });
  html += '</div>';

  // Days grid
  html += '<div class="rp-calendar__days">';

  // Empty cells for days before month starts
  for (let i = 0; i < firstDay; i++) {
    html += '<div class="rp-calendar__day rp-calendar__day--empty"></div>';
  }

  // Days of the month
  for (let day = 1; day <= daysInMonth; day++) {
    const cellDate = new Date(year, month, day);
    const isSelected = dateUtils.isSameDay(cellDate, selectedDate);
    const isToday = dateUtils.isSameDay(cellDate, today);
    const isRangeStart = rangeStart && dateUtils.isSameDay(cellDate, rangeStart);
    const isRangeEnd = rangeEnd && dateUtils.isSameDay(cellDate, rangeEnd);
    const isInRange = rangeStart && rangeEnd && dateUtils.isInRange(cellDate, rangeStart, rangeEnd);
    const isDisabled = (minDate && cellDate < minDate) || (maxDate && cellDate > maxDate);

    const classes = ['rp-calendar__day'];
    if (isSelected) classes.push('rp-calendar__day--selected');
    if (isToday) classes.push('rp-calendar__day--today');
    if (isRangeStart) classes.push('rp-calendar__day--range-start');
    if (isRangeEnd) classes.push('rp-calendar__day--range-end');
    if (isInRange) classes.push('rp-calendar__day--in-range');
    if (isDisabled) classes.push('rp-calendar__day--disabled');

    html += `<button type="button" class="${classes.join(' ')}" data-date="${cellDate.toISOString()}" ${isDisabled ? 'disabled' : ''}>${day}</button>`;
  }

  html += '</div></div>';
  return html;
}

// Date picker popup
function createDatePickerPopup(input, state) {
  const popup = document.createElement('div');
  popup.className = 'rp-date-picker-popup';
  popup.style.display = 'none';

  // Position near input
  const rect = input.getBoundingClientRect();
  popup.style.position = 'absolute';
  popup.style.top = `${rect.bottom + window.scrollY}px`;
  popup.style.left = `${rect.left + window.scrollX}px`;
  popup.style.zIndex = '1000';

  document.body.appendChild(popup);
  return popup;
}

// Initialize date picker
export function enhanceDatePicker(input) {
  if (input.dataset.rpEnhanced) return;
  input.dataset.rpEnhanced = 'true';

  const format = input.dataset.format || 'MM/dd/yyyy';
  const isRange = input.dataset.range === 'true';
  const hasTime = input.dataset.time === 'true';
  const minDate = input.dataset.minDate ? new Date(input.dataset.minDate) : null;
  const maxDate = input.dataset.maxDate ? new Date(input.dataset.maxDate) : null;
  const clearable = input.dataset.clearable === 'true';

  const state = {
    selectedDate: null,
    rangeStart: null,
    rangeEnd: null,
    currentMonth: new Date(),
    isOpen: false,
    popup: null
  };

  datePickers.set(input, state);

  // Parse initial value
  if (input.value) {
    if (isRange) {
      const parts = input.value.split(' - ');
      if (parts.length === 2) {
        state.rangeStart = dateUtils.parse(parts[0], format);
        state.rangeEnd = dateUtils.parse(parts[1], format);
      }
    } else {
      state.selectedDate = dateUtils.parse(input.value, format);
    }
  }

  // Create popup
  state.popup = createDatePickerPopup(input, state);

  // Update calendar view
  function updateCalendar() {
    state.popup.innerHTML = buildCalendar(
      state.currentMonth,
      state.selectedDate,
      state.rangeStart,
      state.rangeEnd,
      minDate,
      maxDate
    );

    // Add event listeners
    state.popup.querySelectorAll('[data-action="prev-month"]').forEach(btn => {
      btn.addEventListener('click', () => {
        state.currentMonth = dateUtils.addMonths(state.currentMonth, -1);
        updateCalendar();
      });
    });

    state.popup.querySelectorAll('[data-action="next-month"]').forEach(btn => {
      btn.addEventListener('click', () => {
        state.currentMonth = dateUtils.addMonths(state.currentMonth, 1);
        updateCalendar();
      });
    });

    state.popup.querySelectorAll('.rp-calendar__day[data-date]').forEach(dayBtn => {
      dayBtn.addEventListener('click', () => {
        const clickedDate = new Date(dayBtn.dataset.date);

        if (isRange) {
          if (!state.rangeStart || (state.rangeStart && state.rangeEnd)) {
            // Start new range
            state.rangeStart = clickedDate;
            state.rangeEnd = null;
          } else {
            // Complete range
            if (clickedDate < state.rangeStart) {
              state.rangeEnd = state.rangeStart;
              state.rangeStart = clickedDate;
            } else {
              state.rangeEnd = clickedDate;
            }
            input.value = `${dateUtils.format(state.rangeStart, format)} - ${dateUtils.format(state.rangeEnd, format)}`;
            input.dispatchEvent(new Event('change', { bubbles: true }));
            closePopup();
          }
          updateCalendar();
        } else {
          state.selectedDate = clickedDate;
          input.value = dateUtils.format(clickedDate, format);
          input.dispatchEvent(new Event('change', { bubbles: true }));
          closePopup();
        }
      });
    });
  }

  // Open/close popup
  function openPopup() {
    if (state.isOpen) return;
    state.isOpen = true;
    state.currentMonth = state.selectedDate || state.rangeStart || new Date();
    updateCalendar();
    state.popup.style.display = 'block';

    // Position popup
    const rect = input.getBoundingClientRect();
    state.popup.style.top = `${rect.bottom + window.scrollY + 4}px`;
    state.popup.style.left = `${rect.left + window.scrollX}px`;

    // Adjust if off-screen
    requestAnimationFrame(() => {
      const popupRect = state.popup.getBoundingClientRect();
      if (popupRect.right > window.innerWidth) {
        state.popup.style.left = `${window.innerWidth - popupRect.width - 10}px`;
      }
      if (popupRect.bottom > window.innerHeight) {
        state.popup.style.top = `${rect.top + window.scrollY - popupRect.height - 4}px`;
      }
    });
  }

  function closePopup() {
    if (!state.isOpen) return;
    state.isOpen = false;
    state.popup.style.display = 'none';
  }

  // Input click - open popup
  input.addEventListener('click', (e) => {
    e.stopPropagation();
    openPopup();
  });

  // Icon click - open popup
  const icon = input.parentElement?.querySelector('.rp-date-picker__icon');
  if (icon) {
    icon.addEventListener('click', (e) => {
      e.stopPropagation();
      input.focus();
      openPopup();
    });
  }

  // Click outside - close popup
  document.addEventListener('click', (e) => {
    if (state.isOpen && !state.popup.contains(e.target) && e.target !== input) {
      closePopup();
    }
  });

  // Keyboard support
  input.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && state.isOpen) {
      closePopup();
      e.preventDefault();
    } else if (e.key === 'Enter') {
      if (!state.isOpen) {
        openPopup();
      }
      e.preventDefault();
    }
  });

  // Manual input - parse and validate
  input.addEventListener('blur', () => {
    if (!input.value) {
      state.selectedDate = null;
      state.rangeStart = null;
      state.rangeEnd = null;
      return;
    }

    if (isRange) {
      const parts = input.value.split(' - ');
      if (parts.length === 2) {
        const start = dateUtils.parse(parts[0], format);
        const end = dateUtils.parse(parts[1], format);
        if (start && end) {
          state.rangeStart = start;
          state.rangeEnd = end;
        } else {
          input.value = state.rangeStart && state.rangeEnd
            ? `${dateUtils.format(state.rangeStart, format)} - ${dateUtils.format(state.rangeEnd, format)}`
            : '';
        }
      }
    } else {
      const parsed = dateUtils.parse(input.value, format);
      if (parsed) {
        state.selectedDate = parsed;
      } else {
        input.value = state.selectedDate ? dateUtils.format(state.selectedDate, format) : '';
      }
    }
  });
}

// Auto-enhance on load
export function enhanceDatePickers(root = document) {
  const pickers = root.querySelectorAll('[data-rp-date-picker]');
  pickers.forEach(enhanceDatePicker);
}

// Auto-initialize
if (typeof window !== 'undefined') {
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => enhanceDatePickers());
  } else {
    enhanceDatePickers();
  }
}
