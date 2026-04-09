"use client";

import "leaflet/dist/leaflet.css";
import { useMemo } from "react";
import { useSearchParams } from "next/navigation";
import { locationData } from "@/lib/locationData";
import { useLocationContext } from "@/context/LocationContext";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";

export default function MapPage() {
  const searchParams = useSearchParams();
  const message = searchParams.get("message");
  const { selectedLocation, setSelectedLocation } = useLocationContext();

  let customIcon: any = undefined;

  if (typeof window !== "undefined") {
    const L = require("leaflet");

    customIcon = new L.Icon({
      iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
      shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
      iconSize: [25, 41],
      iconAnchor: [12, 41],
    });
  }

  const selectedLocationSummary = useMemo(() => {
    if (!selectedLocation) {
      return {
        title: "No location selected",
        rate: "--",
        score: "--",
        note: "Choose a state from the map or the tracked locations panel.",
      };
    }

    return {
      title: selectedLocation.name,
      rate: `$${selectedLocation.electricityRate}/kWh`,
      score: `${selectedLocation.solarScore}/10`,
      note: selectedLocation.note,
    };
  }, [selectedLocation]);

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-10 md:px-10">
      <div className="mx-auto max-w-7xl space-y-8">
        <section className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-slate-950 via-slate-900 to-cyan-950/50 p-8 shadow-2xl md:p-10">
          <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,rgba(59,130,246,0.16),transparent_30%),radial-gradient(circle_at_bottom_right,rgba(16,185,129,0.14),transparent_30%)]" />

          <div className="relative grid gap-8 lg:grid-cols-[1.15fr_0.85fr]">
            <div>
              <div className="inline-flex rounded-full border border-cyan-400/20 bg-cyan-400/10 px-3 py-1 text-sm font-medium text-cyan-300">
                Geographic Insights
              </div>

              <h1 className="mt-5 text-4xl font-bold tracking-tight text-white md:text-5xl">
                Renewable energy location intelligence
              </h1>

              <p className="mt-4 max-w-3xl text-lg leading-8 text-slate-300">
                Compare state-level renewable opportunity signals, select a
                location, and push map-based electricity pricing into your
                project workflow.
              </p>

              <div className="mt-8 flex flex-wrap gap-3">
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  State-level pricing
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Solar score comparison
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Calculator-connected selection
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white/5 p-6 backdrop-blur-sm">
              <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
                Selected Location Snapshot
              </p>

              <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border border-white/10 bg-black/20 p-4 sm:col-span-2">
                  <p className="text-sm text-slate-400">Location</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {selectedLocationSummary.title}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Electricity Rate</p>
                  <p className="mt-2 text-2xl font-bold text-emerald-300">
                    {selectedLocationSummary.rate}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Solar Score</p>
                  <p className="mt-2 text-2xl font-bold text-blue-300">
                    {selectedLocationSummary.score}
                  </p>
                </div>
              </div>

              <div className="mt-5 rounded-2xl border border-cyan-400/15 bg-cyan-400/10 p-4">
                <p className="text-sm text-cyan-200">Location Note</p>
                <p className="mt-2 text-sm leading-7 text-slate-200">
                  {selectedLocationSummary.note}
                </p>
              </div>
            </div>
          </div>
        </section>

        {message === "select-location" && (
          <section className="rounded-2xl border border-amber-400/20 bg-amber-400/10 p-4 shadow-sm">
            <h2 className="text-lg font-semibold text-amber-200">
              Please select a location
            </h2>
            <p className="mt-1 text-sm text-amber-100">
              Choose a state on the map to use map-based electricity pricing in
              the calculator.
            </p>
          </section>
        )}

        <section className="grid grid-cols-1 gap-6 xl:grid-cols-[1.15fr_0.85fr]">
          <div className="rounded-[28px] border border-white/10 bg-white p-4 shadow-xl md:p-5">
            <div className="mb-4 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
              <div>
                <h2 className="text-2xl font-semibold text-slate-900">
                  U.S. opportunity map
                </h2>
                <p className="mt-1 text-sm text-slate-500">
                  Click a marker to update the shared dashboard location context.
                </p>
              </div>

              <div className="inline-flex rounded-full border border-slate-200 bg-slate-50 px-3 py-1 text-xs text-slate-600">
                Live selection → Calculator context
              </div>
            </div>

            <div className="h-[560px] w-full overflow-hidden rounded-3xl border border-slate-200">
              <MapContainer
                center={[39.5, -98.35]}
                zoom={4}
                scrollWheelZoom={true}
                className="h-full w-full"
              >
                <TileLayer
                  attribution="&copy; OpenStreetMap contributors"
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />

                {locationData.map((location) => (
                  <Marker
                    key={location.name}
                    position={location.position}
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
                    <Popup>
                      <div className="space-y-1">
                        <h3 className="font-semibold">{location.name}</h3>
                        <p>Electricity Rate: ${location.electricityRate}/kWh</p>
                        <p>Solar Score: {location.solarScore}/10</p>
                        <p>{location.note}</p>
                      </div>
                    </Popup>
                  </Marker>
                ))}
              </MapContainer>
            </div>
          </div>

          <div className="space-y-6">
            <section className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-slate-900 to-emerald-950 p-6 shadow-lg text-white">
              <p className="text-sm text-emerald-200">How to use this tab</p>
              <h2 className="mt-2 text-2xl font-semibold">
                Select a state to drive pricing assumptions
              </h2>
              <p className="mt-4 leading-7 text-slate-200">
                Clicking a marker updates the shared dashboard context. That
                selected electricity rate can then be used in the calculator or
                compared against manual project assumptions.
              </p>

              <div className="mt-6 space-y-3">
                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">1. Review locations</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Explore opportunity signals across tracked states.
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">2. Select one state</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Choose a marker or use the tracked locations list.
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">3. Continue to modeling</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Use the selected rate inside the calculator workflow.
                  </p>
                </div>
              </div>
            </section>

            <section className="rounded-3xl border border-white/10 bg-white p-6 shadow-lg">
              <p className="text-sm text-slate-500">Tracked Locations</p>
              <div className="mt-4 space-y-4">
                {locationData.map((location) => {
                  const isSelected = selectedLocation?.name === location.name;

                  return (
                    <button
                      key={location.name}
                      onClick={() =>
                        setSelectedLocation({
                          name: location.name,
                          electricityRate: location.electricityRate,
                          solarScore: location.solarScore,
                          note: location.note,
                        })
                      }
                      className={`w-full rounded-2xl border p-4 text-left transition ${
                        isSelected
                          ? "border-slate-900 bg-slate-50 shadow-sm"
                          : "border-slate-200 bg-white hover:bg-slate-50"
                      }`}
                    >
                      <div className="flex items-start justify-between gap-3">
                        <div>
                          <h3 className="font-semibold text-slate-900">
                            {location.name}
                          </h3>
                          <p className="mt-2 text-sm text-slate-500">
                            Rate: ${location.electricityRate}/kWh
                          </p>
                        </div>

                        <span
                          className={`rounded-full px-2.5 py-1 text-xs font-medium ${
                            isSelected
                              ? "bg-slate-900 text-white"
                              : "bg-slate-100 text-slate-600"
                          }`}
                        >
                          {location.solarScore}/10
                        </span>
                      </div>

                      <p className="mt-3 text-sm leading-6 text-slate-600">
                        {location.note}
                      </p>
                    </button>
                  );
                })}
              </div>
            </section>
          </div>
        </section>
      </div>
    </main>
  );
}