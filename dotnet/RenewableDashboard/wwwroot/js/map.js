window.renewableMap = (() => {
    let map = null;

    return {
        init(elementId, locations, dotnetRef) {
            if (typeof L === "undefined") return;

            // Tear down any prior instance (e.g. after re-render).
            if (map) {
                map.remove();
                map = null;
            }

            const container = document.getElementById(elementId);
            if (!container) return;

            map = L.map(elementId).setView([39.5, -98.35], 4);

            L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
                attribution: "&copy; OpenStreetMap contributors",
            }).addTo(map);

            const icon = new L.Icon({
                iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
                shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
                iconSize: [25, 41],
                iconAnchor: [12, 41],
            });

            locations.forEach((loc) => {
                const marker = L.marker([loc.latitude, loc.longitude], { icon }).addTo(map);
                marker.bindPopup(
                    `<div style="line-height:1.4">
                        <strong>${loc.name}</strong><br/>
                        Electricity Rate: $${loc.electricityRate}/kWh<br/>
                        Solar Score: ${loc.solarScore}/10<br/>
                        ${loc.note}
                     </div>`
                );
                marker.on("click", () => {
                    dotnetRef.invokeMethodAsync("SelectLocationFromMap", loc.name);
                });
            });

            // Leaflet needs a size recalculation once it is visible in the DOM.
            setTimeout(() => map && map.invalidateSize(), 200);
        },
    };
})();
