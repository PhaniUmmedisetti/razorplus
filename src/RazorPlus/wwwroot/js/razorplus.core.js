// RazorPlus core (Stage 1): tiny auto-init hook and progressive enhancement stubs

export function init(root = document) {
  enhanceTabs(root);
  enhanceSwitches(root);
  enhanceAccordion(root);
  enhanceModals(root);
  enhanceSelects(root);
  enhanceTables(root);
  enhanceCharts(root);
}

function enhanceTabs(root) {
  const containers = root.querySelectorAll('[data-rp-tabs]');
  containers.forEach(c => {
    if (c.dataset.enhanced) return; c.dataset.enhanced = '1';
    const panels = Array.from(c.querySelectorAll('.rp-tab'));
    if (panels.length === 0) return;

    // Build tablist from <template data-rp-tab-header>
    const list = document.createElement('div');
    list.setAttribute('role', 'tablist');
    list.className = 'rp-tablist';

    panels.forEach((p, i) => {
      const tid = p.getAttribute('id') || `rp-tab-${i}`;
      p.id = tid;
      const tpl = c.querySelector(`#tab-${CSS.escape(tid)}`) || p.querySelector('template[data-rp-tab-header]');
      const label = (tpl?.textContent || `Tab ${i+1}`).trim();
      const btn = document.createElement('button');
      btn.className = 'rp-btn rp-btn--ghost rp-btn--sm';
      btn.setAttribute('role', 'tab');
      btn.id = `tabbtn-${tid}`;
      btn.setAttribute('aria-controls', tid);
      btn.setAttribute('tabindex', i === 0 ? '0' : '-1');
      btn.setAttribute('aria-selected', p.classList.contains('rp-tab--active') || i === 0 ? 'true' : 'false');
      btn.textContent = label;
      list.appendChild(btn);
      if (i === 0) p.classList.add('rp-tab--active');
    });

    c.prepend(list);

    // Roving tabindex + activation
    list.addEventListener('keydown', (e) => {
      const tabs = Array.from(list.querySelectorAll('[role=tab]'));
      const current = document.activeElement;
      let idx = tabs.indexOf(current);
      if (e.key === 'ArrowRight') { idx = (idx + 1 + tabs.length) % tabs.length; tabs[idx].focus(); e.preventDefault(); }
      if (e.key === 'ArrowLeft') { idx = (idx - 1 + tabs.length) % tabs.length; tabs[idx].focus(); e.preventDefault(); }
      if (e.key === 'Home') { tabs[0].focus(); e.preventDefault(); }
      if (e.key === 'End') { tabs[tabs.length-1].focus(); e.preventDefault(); }
      if (e.key === 'Enter' || e.key === ' ') { current?.click(); e.preventDefault(); }
    });

    list.addEventListener('click', (e) => {
      const btn = e.target.closest('[role=tab]');
      if (!btn) return;
      const tabs = Array.from(list.querySelectorAll('[role=tab]'));
      tabs.forEach(t => { t.setAttribute('aria-selected', 'false'); t.setAttribute('tabindex', '-1'); });
      btn.setAttribute('aria-selected', 'true'); btn.setAttribute('tabindex', '0');
      panels.forEach(p => p.classList.remove('rp-tab--active'));
      const panel = c.querySelector(`#${btn.getAttribute('aria-controls')}`);
      if (panel) panel.classList.add('rp-tab--active');
    });
  });
}

function enhanceSwitches(root) {
  const tracks = root.querySelectorAll('[data-rp-switch]');
  tracks.forEach(track => {
    if (track.dataset.enhanced) return;
    track.dataset.enhanced = '1';
    const input = track.querySelector('.rp-switch__control');
    if (!input) return;
    const state = track.querySelector('.rp-switch__state');
    const update = () => {
      input.setAttribute('aria-checked', input.checked ? 'true' : 'false');
      if (state) {
        const onText = state.dataset.on || 'On';
        const offText = state.dataset.off || 'Off';
        state.setAttribute('aria-label', input.checked ? onText : offText);
      }
      track.classList.toggle('rp-switch__track--checked', input.checked);
    };
    input.addEventListener('change', update);
    update();
  });
}

let selectModulePromise;
function ensureSelectModule() {
  if (!selectModulePromise) {
    selectModulePromise = import('./razorplus.select.js');
  }
  return selectModulePromise;
}

function enhanceSelects(root) {
  const selects = root.querySelectorAll('select[data-rp-select][data-rp-select-enhance]');
  if (!selects.length) return;
  ensureSelectModule().then(mod => {
    selects.forEach(select => {
      if (select.dataset.enhanced) return;
      mod.enhanceSelect(select);
    });
  }).catch(err => console.error('RazorPlus select enhancement failed', err));
}

let tableModulePromise;
function ensureTableModule() {
  if (!tableModulePromise) {
    tableModulePromise = import('./razorplus.table.js');
  }
  return tableModulePromise;
}

function enhanceTables(root) {
  const tables = root.querySelectorAll('[data-rp-table][data-rp-table-client]');
  if (!tables.length) return;
  ensureTableModule().then(mod => {
    tables.forEach(table => {
      if (table.dataset.enhancedClient) return;
      mod.enhanceClientTable(table);
    });
  }).catch(err => console.error('RazorPlus table enhancement failed', err));
}

let chartModulePromise;
function ensureChartModule() {
  if (!chartModulePromise) {
    chartModulePromise = import('./razorplus.chart.js');
  }
  return chartModulePromise;
}

function enhanceCharts(root) {
  const charts = root.querySelectorAll('[data-rp-chart]');
  if (!charts.length) return;
  ensureChartModule().then(mod => {
    if (typeof window !== 'undefined') {
      window.RazorPlus = window.RazorPlus || {};
      if (mod.exportChart) window.RazorPlus.exportChart = mod.exportChart;
      if (mod.disposeChart) window.RazorPlus.disposeChart = mod.disposeChart;
    }
    charts.forEach(chart => {
      if (chart.dataset.enhanced) return;
      mod.mountChart(chart);
    });
  }).catch(err => console.error('RazorPlus chart enhancement failed', err));
}

function enhanceAccordion(root) {
  const accordions = root.querySelectorAll('[data-rp-accordion]');
  accordions.forEach(acc => {
    if (acc.dataset.enhanced) return;
    acc.dataset.enhanced = '1';
    acc.addEventListener('click', (event) => {
      const trigger = event.target.closest('[data-rp-accordion-trigger]');
      if (!trigger || !acc.contains(trigger)) return;
      toggleAccordion(trigger);
    });
    acc.addEventListener('keydown', (event) => {
      if (event.key !== 'Enter' && event.key !== ' ') return;
      const trigger = event.target.closest('[data-rp-accordion-trigger]');
      if (!trigger || !acc.contains(trigger)) return;
      event.preventDefault();
      toggleAccordion(trigger);
    });
  });
}

function toggleAccordion(trigger, force) {
  const expanded = force !== undefined ? force : trigger.getAttribute('aria-expanded') !== 'true';
  const panel = trigger.parentElement?.querySelector('[data-rp-accordion-panel]');
  trigger.setAttribute('aria-expanded', expanded ? 'true' : 'false');
  if (panel) {
    if (expanded) {
      panel.removeAttribute('hidden');
    } else {
      panel.setAttribute('hidden', '');
    }
  }
}

const focusableSelector = 'a[href], area[href], button:not([disabled]), input:not([disabled]):not([type="hidden"]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])';
const modalState = new WeakMap();
let modalOpenCount = 0;

function lockBodyScroll() {
  if (typeof document === 'undefined') return;
  if (modalOpenCount === 0) {
    const body = document.body;
    if (body) {
      body.dataset.rpModalScroll = body.style.overflow || '';
      body.style.overflow = 'hidden';
    }
  }
  modalOpenCount++;
}

function unlockBodyScroll() {
  if (typeof document === 'undefined') return;
  modalOpenCount = Math.max(0, modalOpenCount - 1);
  if (modalOpenCount === 0) {
    const body = document.body;
    if (body) {
      const prev = body.dataset.rpModalScroll ?? '';
      body.style.overflow = prev;
      delete body.dataset.rpModalScroll;
    }
  }
}

function enhanceModals(root) {
  const modals = root.querySelectorAll('[data-rp-modal]');
  modals.forEach(modal => {
    if (modal.dataset.enhanced) return;
    modal.dataset.enhanced = '1';
    const dialog = modal.querySelector('[role="dialog"]');
    const overlay = modal.querySelector('[data-rp-modal-overlay]');
    const closeEls = modal.querySelectorAll('[data-rp-modal-close]');
    if (!dialog) return;

    const state = { lastActive: null, trapHandler: null, escHandler: null };
    modalState.set(modal, state);

    const close = () => closeModalElement(modal);
    closeEls.forEach(btn => btn.addEventListener('click', close));
    if (overlay && modal.dataset.rpModalStatic !== 'true') {
      overlay.addEventListener('click', close);
    }

    const handleEsc = (event) => {
      if (event.key === 'Escape') {
        if (modal.dataset.rpModalStatic === 'true') return;
        close();
      }
    };

    const trapFocus = (event) => {
      if (event.key !== 'Tab') return;
      const focusable = Array.from(dialog.querySelectorAll(focusableSelector)).filter(el => el.offsetParent !== null);
      if (focusable.length === 0) {
        event.preventDefault();
        return;
      }
      const first = focusable[0];
      const last = focusable[focusable.length - 1];
      if (!event.shiftKey && document.activeElement === last) {
        event.preventDefault();
        first.focus();
      } else if (event.shiftKey && document.activeElement === first) {
        event.preventDefault();
        last.focus();
      }
    };

    state.escHandler = handleEsc;
    state.trapHandler = trapFocus;

    if (modal.dataset.rpOpen === 'true') {
      openModalElement(modal, false);
    }
  });
}

function openModalElement(modal, focusDialog = true) {
  const dialog = modal.querySelector('[role="dialog"]');
  if (!dialog) return;
  const state = modalState.get(modal) ?? {};
  state.lastActive = document.activeElement instanceof HTMLElement ? document.activeElement : null;
  modal.removeAttribute('hidden');
  modal.classList.add('rp-modal--open');
  modal.dataset.rpOpen = 'true';
  lockBodyScroll();
  if (state.escHandler) {
    document.addEventListener('keydown', state.escHandler);
  }
  if (state.trapHandler) {
    modal.addEventListener('keydown', state.trapHandler);
  }
  if (focusDialog) {
    requestAnimationFrame(() => {
      const focusable = dialog.querySelector(focusableSelector);
      if (focusable instanceof HTMLElement) {
        focusable.focus();
      } else {
        dialog.focus();
      }
    });
  }
}

function closeModalElement(modal) {
  const dialog = modal.querySelector('[role="dialog"]');
  if (!dialog) return;
  const state = modalState.get(modal) ?? {};
  modal.classList.remove('rp-modal--open');
  modal.dataset.rpOpen = 'false';
  modal.setAttribute('hidden', 'hidden');
  if (state.escHandler) {
    document.removeEventListener('keydown', state.escHandler);
  }
  if (state.trapHandler) {
    modal.removeEventListener('keydown', state.trapHandler);
  }
  unlockBodyScroll();
  if (state.lastActive instanceof HTMLElement) {
    state.lastActive.focus({ preventScroll: false });
  }
}

export function openModal(id) {
  const modal = document.getElementById(id);
  if (modal && modal.matches('[data-rp-modal]')) {
    openModalElement(modal);
  }
}

export function closeModal(id) {
  const modal = document.getElementById(id);
  if (modal && modal.matches('[data-rp-modal]')) {
    closeModalElement(modal);
  }
}

if (typeof window !== 'undefined') {
  window.RazorPlus = window.RazorPlus || {};
  Object.assign(window.RazorPlus, {
    init,
    openModal,
    closeModal,
    refresh: (root = document) => {
      init(root);
    }
  });
}

// Auto-init on DOM ready
if (document.readyState !== 'loading') init();
else document.addEventListener('DOMContentLoaded', () => init());

