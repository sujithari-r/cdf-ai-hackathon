"use client";

import { createContext, useContext, useState } from "react";

type CalculatorSnapshot = {
  scenario: "base" | "optimistic";
  rateMode: "manual" | "map" | "compare";
  manualElectricityRate: number;
  activeElectricityRate: number;
  totalProjectCost: number;
  annualRevenue: number;
  netOperatingIncome: number;
  paybackPeriod: number | null;
  npv: number;
} | null;

type CalculatorContextType = {
  calculatorSnapshot: CalculatorSnapshot;
  setCalculatorSnapshot: React.Dispatch<React.SetStateAction<CalculatorSnapshot>>;
};

const CalculatorContext = createContext<CalculatorContextType | undefined>(undefined);

export function CalculatorProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [calculatorSnapshot, setCalculatorSnapshot] =
    useState<CalculatorSnapshot>(null);

  return (
    <CalculatorContext.Provider
      value={{ calculatorSnapshot, setCalculatorSnapshot }}
    >
      {children}
    </CalculatorContext.Provider>
  );
}

export function useCalculatorContext() {
  const context = useContext(CalculatorContext);

  if (!context) {
    throw new Error("useCalculatorContext must be used inside CalculatorProvider");
  }

  return context;
}