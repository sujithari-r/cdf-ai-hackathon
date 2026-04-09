"use client";

import "leaflet/dist/leaflet.css";
import { useMemo } from "react";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";

type LocationItem = {
  name: string;
  position: [number, number] | number[];
  electricityRate: number;
  solarScore: number;
  note: string;
};

type Props = {
  locationData: LocationItem[];
  setSelectedLocation: (location: {
    name: string;
    electricityRate: number;
    solarScore: number;
    note: string;
  }) => void;
};

const TypedMapContainer = MapContainer as any;
const TypedTileLayer = TileLayer as any;
const TypedMarker = Marker as any;
const TypedPopup = Popup as any;

const mapCenter: [number, number] = [39.5, -98.35];

export default function MapClient({
  locationData,
  setSelectedLocation,
}: Props) {
  const customIcon = useMemo(() => {
    if (typeof window === "undefined") return undefined;

    // eslint-disable-next-line @typescript-eslint/no-require-imports
    const L = require("leaflet");

    return new L.Icon({
      iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
      shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
      iconSize: [25, 41],
      iconAnchor: [12, 41],
    });
  }, []);

  return (
    <div className="h-[560px] w-full overflow-hidden rounded-3xl border border-slate-200">
      <TypedMapContainer
        center={mapCenter}
        zoom={4}
        scrollWheelZoom={true}
        className="h-full w-full"
      >
        <TypedTileLayer
          attribution="&copy; OpenStreetMap contributors"
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        {locationData.map((location) => (
          <TypedMarker
            key={location.name}
            position={location.position as [number, number]}
            icon={customIcon}
            eventHandlers={{
              click: () =>
                setSelectedLocation({
                  name: location.name,
                  electricityRate: location.electricityRate,
                  solarScore: location.solarScore,
                  note: location.note,
                }),
            }}
          >
            <TypedPopup>
              <div className="space-y-1">
                <h3 className="font-semibold">{location.name}</h3>
                <p>Electricity Rate: ${location.electricityRate}/kWh</p>
                <p>Solar Score: {location.solarScore}/10</p>
                <p>{location.note}</p>
              </div>
            </TypedPopup>
          </TypedMarker>
        ))}
      </TypedMapContainer>
    </div>
  );
}