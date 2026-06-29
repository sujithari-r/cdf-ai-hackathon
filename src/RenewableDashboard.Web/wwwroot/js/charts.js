const charts = {};

export function createLineChart(canvasId, labels, values, label) {
  destroyChart(canvasId);

  const canvas = document.getElementById(canvasId);
  if (!canvas) return;

  charts[canvasId] = new Chart(canvas, {
    type: "line",
    data: {
      labels,
      datasets: [
        {
          label,
          data: values,
          borderColor: "#0f172a",
          backgroundColor: "rgba(16, 185, 129, 0.15)",
          borderWidth: 3,
          pointBackgroundColor: "#10b981",
          pointBorderColor: "#0f172a",
          pointRadius: 4,
          tension: 0.3,
          fill: true,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { display: false },
        tooltip: {
          callbacks: {
            label: (ctx) => `$${Number(ctx.raw).toFixed(3)}/kWh`,
          },
        },
      },
      scales: {
        x: {
          ticks: { maxRotation: 25, minRotation: 25, color: "#64748b" },
          grid: { color: "#dbe4ee" },
        },
        y: {
          ticks: {
            color: "#64748b",
            callback: (value) => `$${Number(value).toFixed(2)}`,
          },
          grid: { color: "#dbe4ee" },
        },
      },
    },
  });
}

export function createBarChart(canvasId, labels, values) {
  destroyChart(canvasId);

  const canvas = document.getElementById(canvasId);
  if (!canvas) return;

  charts[canvasId] = new Chart(canvas, {
    type: "bar",
    data: {
      labels,
      datasets: [
        {
          label: "Cash Flow",
          data: values,
          backgroundColor: values.map((v) => (v < 0 ? "#0f172a" : "#10b981")),
          borderRadius: 8,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { display: false },
        tooltip: {
          callbacks: {
            label: (ctx) =>
              `$${Number(ctx.raw).toLocaleString("en-US", { maximumFractionDigits: 0 })}`,
          },
        },
      },
      scales: {
        x: {
          ticks: { maxRotation: 30, minRotation: 30, color: "#64748b" },
          grid: { display: false },
        },
        y: {
          ticks: {
            color: "#64748b",
            callback: (value) => `$${(value / 1000000).toFixed(1)}M`,
          },
          grid: { color: "#dbe4ee" },
        },
      },
    },
  });
}

export function destroyChart(canvasId) {
  if (charts[canvasId]) {
    charts[canvasId].destroy();
    delete charts[canvasId];
  }
}
