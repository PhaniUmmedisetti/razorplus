const originalOptions = new WeakMap();

const debounce = (fn, wait = 200) => {
  let timeout;
  return (...args) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => fn(...args), wait);
  };
};

export function enhanceSelect(select) {
  const container = select.closest('[data-rp-select-container]');
  if (!container) return;
  if (select.dataset.enhanced) return;
  select.dataset.enhanced = '1';

  const filterable = select.dataset.rpSelectFilterable === 'true';
  const clearable = select.dataset.rpSelectClearable === 'true';
  const fetchUrl = select.dataset.rpSelectFetch;
  const placeholder = select.dataset.rpSelectPlaceholder || '';
  const searchParam = select.dataset.rpSelectSearchParam || 'q';
  const searchMin = parseInt(select.dataset.rpSelectSearchMin || '2', 10);
  const debounceMs = parseInt(select.dataset.rpSelectDebounce || '250', 10);
  const status = document.createElement('div');
  status.className = 'rp-select__status';
  status.hidden = true;

  if (filterable || fetchUrl) {
    const filterRow = document.createElement('div');
    filterRow.className = 'rp-select__filter';
    const input = document.createElement('input');
    input.type = 'search';
    input.placeholder = placeholder ? `Search ${placeholder.toLowerCase()}` : 'Search options';
    filterRow.appendChild(input);
    if (clearable) {
      const clear = document.createElement('button');
      clear.type = 'button';
      clear.className = 'rp-select__clear';
      clear.textContent = 'Clear';
      clear.addEventListener('click', () => {
        if (input) input.value = '';
        restoreOptions(select);
        Array.from(select.options).forEach(option => { option.selected = false; option.hidden = false; });
        if (!select.multiple) {
          select.selectedIndex = 0;
          select.value = select.options.length > 0 ? select.options[0].value : '';
        }
        status.textContent = '';
        status.hidden = true;
        select.dispatchEvent(new Event('change', { bubbles: true }));
      });
      filterRow.appendChild(clear);
    }
    filterRow.appendChild(status);
    container.insertBefore(filterRow, select);

    snapshotOptions(select);

    if (fetchUrl) {
      bindAsyncSearch(select, input, { fetchUrl, searchParam, searchMin, debounceMs, status, placeholder, clearable });
    } else {
      input.addEventListener('input', () => applyFilter(select, input.value, placeholder));
    }
  } else if (clearable) {
    snapshotOptions(select);
    const clear = document.createElement('button');
    clear.type = 'button';
    clear.className = 'rp-select__clear';
    clear.textContent = 'Clear';
    clear.addEventListener('click', () => {
      restoreOptions(select);
      Array.from(select.options).forEach(option => { option.selected = false; option.hidden = false; });
      if (!select.multiple) {
        select.selectedIndex = 0;
        select.value = select.options.length > 0 ? select.options[0].value : '';
      }
      select.dispatchEvent(new Event('change', { bubbles: true }));
    });
    container.appendChild(clear);
  }
}

function snapshotOptions(select) {
  if (originalOptions.has(select)) return;
  const snapshot = Array.from(select.options).map(option => ({
    value: option.value,
    text: option.text,
    disabled: option.disabled,
    selected: option.selected
  }));
  originalOptions.set(select, snapshot);
}

function restoreOptions(select) {
  const snapshot = originalOptions.get(select);
  if (!snapshot) return;
  select.innerHTML = '';
  snapshot.forEach(opt => {
    const option = new Option(opt.text, opt.value, opt.selected, opt.selected);
    option.disabled = opt.disabled;
    select.appendChild(option);
  });
}

function applyFilter(select, query, placeholder) {
  const text = query.trim().toLowerCase();
  Array.from(select.options).forEach((option, index) => {
    if (index === 0 && placeholder) {
      option.hidden = false;
      return;
    }
    if (!text) {
      option.hidden = false;
      return;
    }
    option.hidden = !option.text.toLowerCase().includes(text);
  });
}

function bindAsyncSearch(select, input, { fetchUrl, searchParam, searchMin, debounceMs, status, placeholder, clearable }) {
  let abortController;
  const runSearch = async (value) => {
    const term = value.trim();
    if (term.length < searchMin) {
      restoreOptions(select);
      if (!term) {
        status.hidden = true;
      }
      return;
    }
    if (abortController) abortController.abort();
    abortController = new AbortController();
    status.textContent = 'Searching...';
    status.hidden = false;
    try {
      const url = new URL(fetchUrl, window.location.origin);
      url.searchParams.set(searchParam, term);
      const response = await fetch(url.toString(), { signal: abortController.signal, headers: { 'Accept': 'application/json' } });
      if (!response.ok) throw new Error(`HTTP ${response.status}`);
      const payload = await response.json();
      updateOptions(select, payload, { placeholder, clearable });
      status.textContent = `${select.options.length} results`;
    } catch (error) {
      if (error.name === 'AbortError') return;
      console.error('rp-select fetch failed', error);
      status.textContent = 'Unable to load results';
    }
  };

  input.addEventListener('input', debounce((event) => runSearch(event.target.value), debounceMs));
}

function updateOptions(select, payload, { placeholder, clearable }) {
  select.innerHTML = '';
  if (placeholder) {
    const option = new Option(placeholder, '', false, false);
    option.disabled = !clearable;
    select.appendChild(option);
  }
  const items = Array.isArray(payload) ? payload : (payload?.items ?? []);
  items.forEach(item => {
    if (item == null) return;
    const text = item.text ?? item.label ?? item.value ?? '';
    const value = item.value ?? text;
    const option = new Option(text, value, item.selected ?? false, item.selected ?? false);
    if (item.disabled) option.disabled = true;
    select.appendChild(option);
  });
  select.dispatchEvent(new Event('change', { bubbles: true }));
}
