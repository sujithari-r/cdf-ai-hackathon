let mapInstance = null;
let markers = [];

export function initializeMap(elementId, locations, dotNetRef) {
  destroyMap();

  const container = document.getElementById(elementId);
  if (!container) return;

  mapInstance = L.map(elementId).setView([39.5, -98.35], 4);

  L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap contributors",
  }).addTo(mapInstance);

  const icon = L.icon({
    iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
    shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
  });

  locations.forEach((location) => {
    const marker = L.marker([location.latitude, location.longitude], { icon })
      .addTo(mapInstance)
      .bindPopup(
        `<strong>${location.name}</strong><br/>` +
          `Electricity Rate: $${location.electricityRate}/kWh<br/>` +
          `Solar Score: ${location.solarScore}/10<br/>` +
          `${location.note}`
      );

    marker.on("click", () => {
      dotNetRef.invokeMethodAsync("OnLocationSelected", {
        name: location.name,
        electricityRate: location.electricityRate,
        solarScore: location.solarScore,
        note: location.note,
      });
    });

    markers.push(marker);
  });

  setTimeout(() => mapInstance.invalidateSize(), 100);
}

export function destroyMap() {
  markers = [];
  if (mapInstance) {
    mapInstance.remove();
    mapInstance = null;
  }
}
