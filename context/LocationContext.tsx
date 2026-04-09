"use client";

import { createContext, useContext, useState } from "react";

type SelectedLocation = {
  name: string;
  electricityRate: number;
  solarScore: number;
  note: string;
} | null;

type LocationContextType = {
  selectedLocation: SelectedLocation;
  setSelectedLocation: React.Dispatch<React.SetStateAction<SelectedLocation>>;
};

const LocationContext = createContext<LocationContextType | undefined>(undefined);

export function LocationProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [selectedLocation, setSelectedLocation] = useState<SelectedLocation>(null);

  return (
    <LocationContext.Provider value={{ selectedLocation, setSelectedLocation }}>
      {children}
    </LocationContext.Provider>
  );
}

export function useLocationContext() {
  const context = useContext(LocationContext);

  if (!context) {
    throw new Error("useLocationContext must be used inside LocationProvider");
  }

  return context;
}