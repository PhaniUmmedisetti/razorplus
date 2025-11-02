const tableState = new WeakMap();

export function enhanceClientTable(table) {
  const tbody = table.querySelector('tbody');
  if (!tbody) return;
  const rows = Array.from(tbody.querySelectorAll('tr'));
  if (!rows.length) return;

  const state = {
    tbody,
    rows,
    originalOrder: rows.slice(),
    sortKey: null,
    sortDir: 'asc',
    page: parseInt(table.dataset.rpPage || '1', 10) || 1,
    pageSize: parseInt(table.dataset.rpPageSize || rows.length, 10),
    pageable: table.dataset.rpTablePageable === 'true',
    paginationNav: null
  };
  tableState.set(table, state);
  table.dataset.enhancedClient = 'true';

  const headers = table.querySelectorAll('th[data-rp-sortable]');
  headers.forEach(header => {
    const button = header.querySelector('.rp-table__sort');
    if (!button) return;
    button.type = 'button';
    button.addEventListener('click', () => {
      const key = header.dataset.rpSortKey;
      toggleSort(table, key);
    });
  });

  if (state.pageable) {
    const nav = buildPaginationNav(table);
    table.after(nav);
    state.paginationNav = nav;
  }

  render(table);
}

function toggleSort(table, key) {
  const state = tableState.get(table);
  if (!state || !key) return;
  if (state.sortKey === key) {
    state.sortDir = state.sortDir === 'asc' ? 'desc' : 'asc';
  } else {
    state.sortKey = key;
    state.sortDir = 'asc';
  }
  state.page = 1;
  render(table);
}

function render(table) {
  const state = tableState.get(table);
  if (!state) return;

  let rows = state.originalOrder.slice();
  if (state.sortKey) {
    const columnIndex = getColumnIndex(table, state.sortKey);
    if (columnIndex !== -1) {
      rows.sort((a, b) => compareCells(a, b, columnIndex, state.sortDir));
    }
  }

  const totalRows = rows.length;
  const pageSize = Math.max(1, state.pageSize);
  const totalPages = Math.max(1, Math.ceil(totalRows / pageSize));
  if (state.page > totalPages) state.page = totalPages;
  const start = state.pageable ? (state.page - 1) * pageSize : 0;
  const end = state.pageable ? start + pageSize : rows.length;
  const slice = rows.slice(start, end);

  state.tbody.innerHTML = '';
  slice.forEach(row => state.tbody.appendChild(row));

  updateSortClasses(table, state);
  if (state.pageable && state.paginationNav) {
    renderPagination(table, state.paginationNav, totalPages);
  }
}

function getColumnIndex(table, sortKey) {
  const header = table.querySelector(`th[data-rp-sort-key="${CSS.escape(sortKey)}"]`);
  if (!header) return -1;
  const index = parseInt(header.dataset.rpColumnIndex || '-1', 10);
  return Number.isInteger(index) ? index : -1;
}

function compareCells(a, b, index, direction) {
  const aText = extractText(a.cells[index]);
  const bText = extractText(b.cells[index]);

  const aNumber = parseFloat(aText.replace(/[^\d.-]/g, ''));
  const bNumber = parseFloat(bText.replace(/[^\d.-]/g, ''));
  const bothNumbers = !Number.isNaN(aNumber) && !Number.isNaN(bNumber);

  let result;
  if (bothNumbers) {
    result = aNumber - bNumber;
  } else {
    result = aText.localeCompare(bText, undefined, { numeric: true, sensitivity: 'base' });
  }
  return direction === 'asc' ? result : -result;
}

function extractText(cell) {
  if (!cell) return '';
  return cell.textContent.trim();
}

function updateSortClasses(table, state) {
  const headers = table.querySelectorAll('th[data-rp-sortable]');
  headers.forEach(header => {
    header.removeAttribute('aria-sort');
    header.classList.remove('rp-table__sort--asc', 'rp-table__sort--desc');
    const button = header.querySelector('.rp-table__sort');
    if (!button) return;
    button.classList.remove('rp-table__sort--asc', 'rp-table__sort--desc');
    const isActive = header.dataset.rpSortKey === state.sortKey;
    if (isActive) {
      const indicatorClass = state.sortDir === 'asc' ? 'rp-table__sort--asc' : 'rp-table__sort--desc';
      button.classList.add(indicatorClass);
      header.setAttribute('aria-sort', state.sortDir === 'asc' ? 'ascending' : 'descending');
    }
  });
}

function buildPaginationNav(table) {
  const nav = document.createElement('nav');
  nav.className = 'rp-pagination';
  nav.setAttribute('aria-label', 'Pagination');
  const list = document.createElement('ul');
  list.className = 'rp-pagination__list';
  nav.appendChild(list);
  nav.addEventListener('click', (event) => {
    const target = event.target.closest('[data-page]');
    if (!target) return;
    event.preventDefault();
    const page = parseInt(target.dataset.page, 10);
    if (Number.isNaN(page)) return;
    const state = tableState.get(table);
    if (!state) return;
    state.page = page;
    render(table);
  });
  return nav;
}

function renderPagination(table, nav, totalPages) {
  const state = tableState.get(table);
  if (!state) return;
  const list = nav.querySelector('.rp-pagination__list');
  if (!list) return;
  list.innerHTML = '';
  if (totalPages <= 1) {
    nav.hidden = true;
    return;
  }
  nav.hidden = false;

  const append = (label, page, disabled = false, active = false, rel = '') => {
    const item = document.createElement('li');
    item.className = 'rp-pagination__item';
    if (disabled) item.classList.add('is-disabled');
    if (active) item.classList.add('is-active');
    if (disabled) {
      const span = document.createElement('span');
      span.textContent = label;
      item.appendChild(span);
    } else {
      const button = document.createElement('a');
      button.href = '#';
      button.dataset.page = String(page);
      if (rel) button.rel = rel;
      button.textContent = label;
      item.appendChild(button);
    }
    list.appendChild(item);
  };

  const windowSize = 2;
  const prevDisabled = state.page <= 1;
  append('Prev', Math.max(1, state.page - 1), prevDisabled, false, 'prev');

  const start = Math.max(1, state.page - windowSize);
  const end = Math.min(totalPages, state.page + windowSize);

  if (start > 1) {
    append('1', 1, false, state.page === 1);
    if (start > 2) {
      const ellipsis = document.createElement('li');
      ellipsis.className = 'rp-pagination__ellipsis';
      ellipsis.innerHTML = '&hellip;';
      list.appendChild(ellipsis);
    }
  }

  for (let i = start; i <= end; i++) {
    append(String(i), i, false, state.page === i);
  }

  if (end < totalPages) {
    if (end < totalPages - 1) {
      const ellipsis = document.createElement('li');
      ellipsis.className = 'rp-pagination__ellipsis';
      ellipsis.innerHTML = '&hellip;';
      list.appendChild(ellipsis);
    }
    append(String(totalPages), totalPages, false, state.page === totalPages);
  }

  const nextDisabled = state.page >= totalPages;
  append('Next', Math.min(totalPages, state.page + 1), nextDisabled, false, 'next');
}

// Row selection handling
export function enhanceTableSelection(table) {
  const selectAll = table.querySelector('.rp-table__select-all');
  if (!selectAll) return;

  const tbody = table.querySelector('tbody');
  if (!tbody) return;

  // Select all checkbox
  selectAll.addEventListener('change', () => {
    const isChecked = selectAll.checked;
    const rowCheckboxes = tbody.querySelectorAll('.rp-table__select-row');
    rowCheckboxes.forEach(checkbox => {
      checkbox.checked = isChecked;
      updateRowSelection(checkbox.closest('tr'), isChecked);
    });

    dispatchSelectionChange(table);
  });

  // Individual row checkboxes
  tbody.addEventListener('change', (e) => {
    if (e.target.classList.contains('rp-table__select-row')) {
      const row = e.target.closest('tr');
      updateRowSelection(row, e.target.checked);

      // Update select-all state
      const rowCheckboxes = Array.from(tbody.querySelectorAll('.rp-table__select-row'));
      const allChecked = rowCheckboxes.every(cb => cb.checked);
      const someChecked = rowCheckboxes.some(cb => cb.checked);
      selectAll.checked = allChecked;
      selectAll.indeterminate = someChecked && !allChecked;

      dispatchSelectionChange(table);
    }
  });

  // Stop propagation on checkbox clicks to prevent row click handler
  tbody.addEventListener('click', (e) => {
    if (e.target.classList.contains('rp-table__select-row') ||
        e.target.closest('.rp-table__select-cell')) {
      e.stopPropagation();
    }
  });
}

function updateRowSelection(row, isSelected) {
  if (!row) return;
  if (isSelected) {
    row.classList.add('rp-table__row--selected');
    row.setAttribute('aria-selected', 'true');
  } else {
    row.classList.remove('rp-table__row--selected');
    row.removeAttribute('aria-selected');
  }
}

function dispatchSelectionChange(table) {
  const tbody = table.querySelector('tbody');
  if (!tbody) return;

  const selectedRows = Array.from(tbody.querySelectorAll('.rp-table__select-row:checked'))
    .map(checkbox => {
      const row = checkbox.closest('tr');
      return {
        element: row,
        key: row?.dataset.rpRowKey || null
      };
    });

  table.dispatchEvent(new CustomEvent('rp-selection-change', {
    detail: { selectedRows, selectedKeys: selectedRows.map(r => r.key).filter(k => k !== null) }
  }));
}

// Row click handling
export function enhanceRowClick(table) {
  const onRowClickHandler = table.dataset.rpRowClick;
  if (!onRowClickHandler) return;

  const tbody = table.querySelector('tbody');
  if (!tbody) return;

  tbody.addEventListener('click', (e) => {
    const row = e.target.closest('tr');
    if (!row || !row.classList.contains('rp-table__row--clickable')) return;

    // Don't trigger row click if clicking on interactive elements
    if (e.target.closest('a, button, input, select, textarea')) return;

    const rowKey = row.dataset.rpRowKey || null;

    // Call the handler function if it exists
    if (typeof window[onRowClickHandler] === 'function') {
      window[onRowClickHandler](rowKey, row, e);
    }

    // Dispatch event
    table.dispatchEvent(new CustomEvent('rp-row-click', {
      detail: { row, key: rowKey, event: e }
    }));
  });
}

// Get selected rows API
export function getSelectedRows(table) {
  const tbody = table.querySelector('tbody');
  if (!tbody) return [];

  return Array.from(tbody.querySelectorAll('.rp-table__select-row:checked'))
    .map(checkbox => {
      const row = checkbox.closest('tr');
      return {
        element: row,
        key: row?.dataset.rpRowKey || null
      };
    });
}

// Clear selection API
export function clearSelection(table) {
  const tbody = table.querySelector('tbody');
  if (!tbody) return;

  const rowCheckboxes = tbody.querySelectorAll('.rp-table__select-row');
  rowCheckboxes.forEach(checkbox => {
    checkbox.checked = false;
    updateRowSelection(checkbox.closest('tr'), false);
  });

  const selectAll = table.querySelector('.rp-table__select-all');
  if (selectAll) {
    selectAll.checked = false;
    selectAll.indeterminate = false;
  }

  dispatchSelectionChange(table);
}

