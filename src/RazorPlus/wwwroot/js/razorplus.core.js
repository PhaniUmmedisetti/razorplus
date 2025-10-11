// RazorPlus core (Stage 1): tiny auto-init hook and progressive enhancement stubs

export function init(root = document) {
  enhanceTabs(root);
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

// Auto-init on DOM ready
if (document.readyState !== 'loading') init();
else document.addEventListener('DOMContentLoaded', () => init());

