window.renewableCharts = (() => {
    const charts = {};

    function destroy(id) {
        if (charts[id]) {
            charts[id].destroy();
            delete charts[id];
        }
    }

    return {
        renderLine(id, labels, values) {
            const canvas = document.getElementById(id);
            if (!canvas || typeof Chart === "undefined") return;
            destroy(id);

            charts[id] = new Chart(canvas, {
                type: "line",
                data: {
                    labels,
                    datasets: [{
                        label: "Electricity Price",
                        data: values,
                        borderColor: "#0f172a",
                        backgroundColor: "rgba(16,185,129,0.15)",
                        borderWidth: 3,
                        pointRadius: 4,
                        pointBackgroundColor: "#10b981",
                        pointBorderColor: "#0f172a",
                        tension: 0.35,
                        fill: true,
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        tooltip: {
                            callbacks: {
                                label: (ctx) => `$${Number(ctx.parsed.y).toFixed(3)}/kWh`,
                            },
                        },
                    },
                    scales: {
                        x: { ticks: { color: "#64748b", maxRotation: 45, minRotation: 25 }, grid: { color: "#e2e8f0" } },
                        y: {
                            ticks: { color: "#64748b", callback: (v) => `$${Number(v).toFixed(2)}` },
                            grid: { color: "#e2e8f0" },
                        },
                    },
                },
            });
        },

        renderBar(id, labels, values) {
            const canvas = document.getElementById(id);
            if (!canvas || typeof Chart === "undefined") return;
            destroy(id);

            const colors = values.map((v) => (v < 0 ? "#0f172a" : "#10b981"));

            charts[id] = new Chart(canvas, {
                type: "bar",
                data: {
                    labels,
                    datasets: [{
                        label: "Cash Flow",
                        data: values,
                        backgroundColor: colors,
                        borderRadius: 8,
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { display: false },
                        tooltip: {
                            callbacks: {
                                label: (ctx) => `$${Number(ctx.parsed.y).toLocaleString("en-US", { maximumFractionDigits: 0 })}`,
                            },
                        },
                    },
                    scales: {
                        x: { ticks: { color: "#64748b", maxRotation: 45, minRotation: 30 }, grid: { display: false } },
                        y: {
                            ticks: { color: "#64748b", callback: (v) => `$${(Number(v) / 1000000).toFixed(1)}M` },
                            grid: { color: "#e2e8f0" },
                        },
                    },
                },
            });
        },
    };
})();
