let echartsPromise;
const chartRegistry = new WeakMap();

function ensureECharts() {
  if (typeof window === 'undefined') return Promise.reject('No window');
  if (window.echarts) return Promise.resolve(window.echarts);
  if (!echartsPromise) {
    echartsPromise = new Promise((resolve, reject) => {
      const script = document.createElement('script');
      script.src = 'https://cdn.jsdelivr.net/npm/echarts@5/dist/echarts.min.js';
      script.async = true;
      script.onload = () => resolve(window.echarts);
      script.onerror = reject;
      document.head.appendChild(script);
    });
  }
  return echartsPromise;
}

export function mountChart(element) {
  if (chartRegistry.has(element)) return;
  const payloadNode = element.querySelector('.rp-chart__data');
  let payload = {};
  if (payloadNode) {
    try {
      payload = JSON.parse(payloadNode.textContent || '{}');
    } catch (error) {
      console.error('Unable to parse chart payload', error);
    }
  }

  ensureECharts().then(echarts => {
    if (!echarts) return;
    const themeChoice = resolveTheme(element.dataset.rpChartTheme || 'auto');
    const instance = echarts.init(element, themeChoice.theme);
    const option = Object.assign({}, payload.options || {});
    if (payload.data) {
      if (Array.isArray(payload.data)) {
        option.series = payload.data;
      } else if (payload.data.series) {
        option.series = payload.data.series;
        if (payload.data.xAxis) option.xAxis = payload.data.xAxis;
        if (payload.data.yAxis) option.yAxis = payload.data.yAxis;
        if (payload.data.legend) option.legend = payload.data.legend;
      } else {
        option.dataset = payload.data;
      }
    }
    instance.setOption(option);
    chartRegistry.set(element, { instance, payload, themeChoice });
    setupResizeObserver(element, instance);
    if (themeChoice.auto) {
      bindThemeListener(element, option);
    }
  }).catch(error => console.error('Failed to load ECharts', error));
}

function resolveTheme(theme) {
  if (theme === 'dark') return { theme: 'dark', auto: false };
  if (theme === 'light') return { theme: null, auto: false };
  const media = window.matchMedia('(prefers-color-scheme: dark)');
  return { theme: media.matches ? 'dark' : null, auto: true };
}

function bindThemeListener(element, option) {
  const record = chartRegistry.get(element);
  if (!record || !record.themeChoice.auto) return;
  const media = window.matchMedia('(prefers-color-scheme: dark)');
  const handler = () => {
    const echarts = record.instance;
    if (!echarts) return;
    const currentOption = echarts.getOption();
    const newTheme = media.matches ? 'dark' : null;
    echarts.dispose();
    ensureECharts().then(lib => {
      const instance = lib.init(element, newTheme);
      instance.setOption(option);
      chartRegistry.set(element, { instance, payload: record.payload, themeChoice: { theme: newTheme, auto: true }, media });
      setupResizeObserver(element, instance);
    });
  };
  media.addEventListener('change', handler);
  record.media = media;
  record.themeHandler = handler;
}

function setupResizeObserver(element, instance) {
  const observer = new ResizeObserver(() => instance.resize());
  observer.observe(element);
  const record = chartRegistry.get(element);
  if (record) record.observer = observer;
}

export function exportChart(id, type = 'png') {
  const element = document.getElementById(id);
  if (!element) throw new Error(`Chart with id ${id} not found`);
  const record = chartRegistry.get(element);
  if (!record) throw new Error('Chart not initialised');
  const instance = record.instance;
  if (type === 'svg') {
    return instance.renderToSVGString();
  }
  return instance.getDataURL({ type: 'png', pixelRatio: window.devicePixelRatio || 1 });
}

export function disposeChart(id) {
  const element = document.getElementById(id);
  if (!element) return;
  const record = chartRegistry.get(element);
  if (!record) return;
  record.media?.removeEventListener('change', record.themeHandler);
  record.instance.dispose();
  record.observer?.disconnect();
  chartRegistry.delete(element);
}
