"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import { useLocationContext } from "@/context/LocationContext";
import { useCalculatorContext } from "@/context/CalculatorContext";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  ResponsiveContainer,
  Cell,
} from "recharts";

export default function CalculatorPage() {
  const router = useRouter();
  const compareSectionRef = useRef<HTMLElement | null>(null);
  const { selectedLocation } = useLocationContext();
  const { setCalculatorSnapshot } = useCalculatorContext();

  const [scenario, setScenario] = useState<"base" | "optimistic">("base");
  const [rateMode, setRateMode] = useState<"manual" | "map" | "compare">("manual");

  const [systemSizeKw, setSystemSizeKw] = useState(5000);
  const [capacityFactor, setCapacityFactor] = useState(25);
  const [installCostPerW, setInstallCostPerW] = useState(1.2);
  const [electricityRate, setElectricityRate] = useState(0.12);
  const [annualOMCost, setAnnualOMCost] = useState(50000);

  const projectLifeYears = 20;
  const discountRate = 0.08;

  function calculateMetrics(rate: number, omCost: number, capFactor: number) {
    const totalProjectCost = systemSizeKw * 1000 * installCostPerW;

    const annualEnergyProduction = systemSizeKw * 8760 * (capFactor / 100);

    const annualRevenue = annualEnergyProduction * rate;

    const netOperatingIncome = annualRevenue - omCost;

    const paybackPeriod =
      netOperatingIncome > 0 ? totalProjectCost / netOperatingIncome : null;

    let npv = -totalProjectCost;

    for (let year = 1; year <= projectLifeYears; year++) {
      npv += netOperatingIncome / Math.pow(1 + discountRate, year);
    }

    return {
      totalProjectCost,
      annualEnergyProduction,
      annualRevenue,
      netOperatingIncome,
      paybackPeriod,
      npv,
    };
  }

  const capFactorForScenario =
    scenario === "optimistic" ? capacityFactor + 3 : capacityFactor;

  const omCostForScenario =
    scenario === "optimistic" ? annualOMCost * 0.95 : annualOMCost;

  const manualRateForScenario =
    scenario === "optimistic" ? electricityRate + 0.02 : electricityRate;

  const mapRateBase = selectedLocation?.electricityRate ?? electricityRate;
  const mapRateForScenario =
    scenario === "optimistic" ? mapRateBase + 0.02 : mapRateBase;

  const manualMetrics = calculateMetrics(
    manualRateForScenario,
    omCostForScenario,
    capFactorForScenario
  );

  const mapMetrics = calculateMetrics(
    mapRateForScenario,
    omCostForScenario,
    capFactorForScenario
  );

  const activeMetrics = rateMode === "map" ? mapMetrics : manualMetrics;

  const totalProjectCost = activeMetrics.totalProjectCost;
  const annualEnergyProduction = activeMetrics.annualEnergyProduction;
  const annualRevenue = activeMetrics.annualRevenue;
  const netOperatingIncome = activeMetrics.netOperatingIncome;
  const paybackPeriod = activeMetrics.paybackPeriod;
  const npv = activeMetrics.npv;

  const activeElectricityRate =
    rateMode === "map" ? mapRateForScenario : manualRateForScenario;

  const cashFlowData = [
    { year: "Year 0", cashFlow: -totalProjectCost },
    ...Array.from({ length: projectLifeYears }, (_, i) => ({
      year: `Year ${i + 1}`,
      cashFlow: netOperatingIncome,
    })),
  ];

  useEffect(() => {
    if (rateMode === "map" && !selectedLocation) {
      router.push("/map?message=select-location");
    }
  }, [rateMode, selectedLocation, router]);

  useEffect(() => {
    if (rateMode === "compare" && compareSectionRef.current) {
      compareSectionRef.current.scrollIntoView({
        behavior: "smooth",
        block: "start",
      });
    }
  }, [rateMode]);

  useEffect(() => {
    setCalculatorSnapshot({
      scenario,
      rateMode,
      manualElectricityRate: electricityRate,
      activeElectricityRate,
      totalProjectCost,
      annualRevenue,
      netOperatingIncome,
      paybackPeriod,
      npv,
    });
  }, [
    scenario,
    rateMode,
    electricityRate,
    activeElectricityRate,
    totalProjectCost,
    annualRevenue,
    netOperatingIncome,
    paybackPeriod,
    npv,
    setCalculatorSnapshot,
  ]);

  const scenarioLabel = scenario === "base" ? "Base Case" : "Optimistic Case";

  const activeRateDescription =
    rateMode === "manual"
      ? "Using manually entered electricity rate"
      : rateMode === "map"
      ? `Using ${selectedLocation?.name ?? "selected map"} electricity rate`
      : "Compare mode enabled — primary view currently shows manual rate";

  const compareDelta = useMemo(() => {
    return {
      revenue: mapMetrics.annualRevenue - manualMetrics.annualRevenue,
      noi: mapMetrics.netOperatingIncome - manualMetrics.netOperatingIncome,
      npv: mapMetrics.npv - manualMetrics.npv,
    };
  }, [manualMetrics, mapMetrics]);

  const kpiCards = [
    {
      title: "Total Project Cost",
      value: `$${totalProjectCost.toLocaleString("en-US", {
        maximumFractionDigits: 0,
      })}`,
      subtitle: "Capital requirement",
      accent: "emerald",
    },
    {
      title: "Annual Revenue",
      value: `$${annualRevenue.toLocaleString("en-US", {
        maximumFractionDigits: 0,
      })}`,
      subtitle: "Top-line project revenue",
      accent: "blue",
    },
    {
      title: "Net Operating Income",
      value: `$${netOperatingIncome.toLocaleString("en-US", {
        maximumFractionDigits: 0,
      })}`,
      subtitle: "Revenue minus O&M cost",
      accent: "violet",
    },
    {
      title: "NPV",
      value: `$${npv.toLocaleString("en-US", {
        maximumFractionDigits: 0,
      })}`,
      subtitle: "20 years at 8% discount",
      accent: "amber",
    },
  ];

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-10 md:px-10">
      <div className="mx-auto max-w-7xl space-y-8">
        <section className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-slate-950 via-slate-900 to-indigo-950/55 p-8 shadow-2xl md:p-10">
          <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,rgba(59,130,246,0.16),transparent_30%),radial-gradient(circle_at_bottom_right,rgba(16,185,129,0.14),transparent_30%)]" />

          <div className="relative grid gap-8 lg:grid-cols-[1.15fr_0.85fr]">
            <div>
              <div className="inline-flex rounded-full border border-blue-400/20 bg-blue-400/10 px-3 py-1 text-sm font-medium text-blue-300">
                Project Economics
              </div>

              <h1 className="mt-5 text-4xl font-bold tracking-tight text-white md:text-5xl">
                Renewable project economics calculator
              </h1>

              <p className="mt-4 max-w-3xl text-lg leading-8 text-slate-300">
                Evaluate renewable energy project returns using editable assumptions,
                selected market inputs, and location-based electricity pricing.
              </p>

              <div className="mt-8 flex flex-wrap gap-3">
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Scenario testing
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Manual vs map pricing
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Cash flow analysis
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white/5 p-6 backdrop-blur-sm">
              <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
                Active Run
              </p>

              <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border border-white/10 bg-black/20 p-4 sm:col-span-2">
                  <p className="text-sm text-slate-400">Scenario</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {scenarioLabel}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Rate Source</p>
                  <p className="mt-2 text-2xl font-bold text-emerald-300">
                    {rateMode === "manual"
                      ? "Manual"
                      : rateMode === "map"
                      ? "Map"
                      : "Compare"}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Active Rate</p>
                  <p className="mt-2 text-2xl font-bold text-blue-300">
                    ${activeElectricityRate.toFixed(3)}/kWh
                  </p>
                </div>
              </div>

              <div className="mt-5 rounded-2xl border border-emerald-400/15 bg-emerald-400/10 p-4">
                <p className="text-sm text-emerald-200">Current Context</p>
                <p className="mt-2 text-sm leading-7 text-slate-200">
                  {selectedLocation
                    ? `${selectedLocation.name} is available as a map-based pricing source at $${selectedLocation.electricityRate}/kWh.`
                    : "No map location selected yet. You can still run manual scenarios or choose a state from the Map tab."}
                </p>
              </div>
            </div>
          </div>
        </section>

        {selectedLocation && (
          <section className="rounded-2xl border border-emerald-400/20 bg-emerald-400/10 p-4 shadow-sm">
            <h2 className="text-lg font-semibold text-emerald-200">
              Map selection connected
            </h2>
            <p className="mt-1 text-sm text-emerald-100">
              {selectedLocation.name} selected — electricity rate available at $
              {selectedLocation.electricityRate}/kWh
            </p>
          </section>
        )}

        <section className="grid grid-cols-1 gap-6 lg:grid-cols-[0.95fr_1.05fr]">
          <section className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
            <div className="mb-5">
              <p className="text-sm text-slate-500">Inputs</p>
              <h2 className="mt-2 text-2xl font-semibold text-slate-900">
                Configure project assumptions
              </h2>
            </div>

            <div className="grid grid-cols-1 gap-5">
              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Scenario
                </label>
                <select
                  value={scenario}
                  onChange={(e) =>
                    setScenario(e.target.value as "base" | "optimistic")
                  }
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                >
                  <option value="base">Base Case</option>
                  <option value="optimistic">Optimistic Case</option>
                </select>
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Rate Source
                </label>
                <select
                  value={rateMode}
                  onChange={(e) =>
                    setRateMode(e.target.value as "manual" | "map" | "compare")
                  }
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                >
                  <option value="manual">Manual Only</option>
                  <option value="map">Map Only</option>
                  <option value="compare">Compare Both</option>
                </select>
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  System Size (kW)
                </label>
                <input
                  type="number"
                  value={systemSizeKw}
                  onChange={(e) => setSystemSizeKw(Number(e.target.value))}
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Capacity Factor (%)
                </label>
                <input
                  type="number"
                  value={capacityFactor}
                  onChange={(e) => setCapacityFactor(Number(e.target.value))}
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Installation Cost ($/W)
                </label>
                <input
                  type="number"
                  step="0.01"
                  value={installCostPerW}
                  onChange={(e) => setInstallCostPerW(Number(e.target.value))}
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Manual Electricity Rate ($/kWh)
                </label>
                <input
                  type="number"
                  step="0.01"
                  value={electricityRate}
                  onChange={(e) => setElectricityRate(Number(e.target.value))}
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                />
              </div>

              <div>
                <label className="mb-2 block text-sm font-medium text-slate-700">
                  Annual O&amp;M Cost ($)
                </label>
                <input
                  type="number"
                  value={annualOMCost}
                  onChange={(e) => setAnnualOMCost(Number(e.target.value))}
                  className="w-full rounded-xl border border-slate-200 bg-white p-3 text-slate-900 outline-none transition focus:border-slate-400"
                />
              </div>
            </div>
          </section>

          <section className="space-y-6">
            <section className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
              <div className="flex flex-col gap-3 md:flex-row md:items-start md:justify-between">
                <div>
                  <p className="text-sm text-slate-500">Outputs</p>
                  <h2 className="mt-2 text-2xl font-semibold text-slate-900">
                    Active project results
                  </h2>
                </div>

                <div className="inline-flex rounded-full border border-slate-200 bg-slate-50 px-3 py-1 text-xs text-slate-600">
                  {scenarioLabel}
                </div>
              </div>

              <div className="mt-5 rounded-2xl border border-emerald-300 bg-emerald-50 p-4">
                <p className="text-sm font-medium text-emerald-700">
                  Active Electricity Rate
                </p>
                <p className="mt-2 text-2xl font-bold text-emerald-900">
                  ${activeElectricityRate.toFixed(3)} / kWh
                </p>
                <p className="mt-1 text-xs text-emerald-700">
                  {activeRateDescription}
                </p>
              </div>

              <div className="mt-6 grid grid-cols-1 gap-4 md:grid-cols-2">
                {kpiCards.map((card) => (
                  <div
                    key={card.title}
                    className="rounded-2xl border border-slate-200 bg-slate-50 p-4"
                  >
                    <p className="text-sm text-slate-500">{card.title}</p>
                    <p className="mt-2 text-2xl font-bold text-slate-900">
                      {card.value}
                    </p>
                    <p className="mt-1 text-sm text-slate-500">{card.subtitle}</p>
                  </div>
                ))}

                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Annual Energy Production</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    {annualEnergyProduction.toLocaleString("en-US")} kWh
                  </p>
                  <p className="mt-1 text-sm text-slate-500">
                    Output from system size and capacity factor
                  </p>
                </div>

                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Annual Operating Cost</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    ${omCostForScenario.toLocaleString("en-US")}
                  </p>
                  <p className="mt-1 text-sm text-slate-500">
                    Scenario-adjusted O&amp;M cost
                  </p>
                </div>

                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4 md:col-span-2">
                  <p className="text-sm text-slate-500">Simple Payback Period</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    {paybackPeriod
                      ? `${paybackPeriod.toFixed(1)} years`
                      : "Not achievable"}
                  </p>
                  <p className="mt-1 text-sm text-slate-500">
                    Based on current net operating income
                  </p>
                </div>
              </div>
            </section>

            <section className="rounded-3xl border border-blue-200 bg-gradient-to-br from-blue-50 to-white p-6 shadow-xl">
              <p className="text-sm text-blue-700">How to use this tab</p>
              <h2 className="mt-2 text-2xl font-semibold text-slate-900">
                Turn assumptions into decision-ready outputs
              </h2>
              <div className="mt-5 space-y-3">
                <div className="rounded-2xl border border-blue-100 bg-white p-4">
                  <p className="font-medium text-slate-900">1. Set assumptions</p>
                  <p className="mt-1 text-sm text-slate-500">
                    Adjust system size, cost, capacity factor, and electricity rate.
                  </p>
                </div>

                <div className="rounded-2xl border border-blue-100 bg-white p-4">
                  <p className="font-medium text-slate-900">2. Switch pricing source</p>
                  <p className="mt-1 text-sm text-slate-500">
                    Compare manual assumptions with map-based electricity pricing.
                  </p>
                </div>

                <div className="rounded-2xl border border-blue-100 bg-white p-4">
                  <p className="font-medium text-slate-900">3. Read the outputs</p>
                  <p className="mt-1 text-sm text-slate-500">
                    Use revenue, NOI, payback, and NPV to judge project viability.
                  </p>
                </div>
              </div>
            </section>
          </section>
        </section>

        {rateMode === "compare" && (
          <section
            ref={compareSectionRef}
            className="grid grid-cols-1 gap-6 lg:grid-cols-[1fr_1fr_0.9fr]"
          >
            <div className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
              <h2 className="text-2xl font-semibold text-slate-900">
                Manual Rate Results
              </h2>
              <div className="mt-5 space-y-4">
                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Rate Used</p>
                  <p className="mt-2 text-xl font-bold text-slate-900">
                    ${manualRateForScenario.toFixed(3)}/kWh
                  </p>
                </div>

                <div className="space-y-3 text-sm">
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">Annual Revenue</span>
                    <span className="font-semibold text-slate-900">
                      ${manualMetrics.annualRevenue.toLocaleString("en-US")}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">NOI</span>
                    <span className="font-semibold text-slate-900">
                      ${manualMetrics.netOperatingIncome.toLocaleString("en-US")}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">Payback</span>
                    <span className="font-semibold text-slate-900">
                      {manualMetrics.paybackPeriod
                        ? `${manualMetrics.paybackPeriod.toFixed(1)} years`
                        : "Not achievable"}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">NPV</span>
                    <span className="font-semibold text-slate-900">
                      $
                      {manualMetrics.npv.toLocaleString("en-US", {
                        maximumFractionDigits: 0,
                      })}
                    </span>
                  </p>
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
              <h2 className="text-2xl font-semibold text-slate-900">
                Map Rate Results
              </h2>
              <div className="mt-5 space-y-4">
                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Rate Used</p>
                  <p className="mt-2 text-xl font-bold text-slate-900">
                    ${mapRateForScenario.toFixed(3)}/kWh
                  </p>
                  <p className="mt-1 text-xs text-slate-500">
                    {selectedLocation
                      ? `From ${selectedLocation.name}`
                      : "Fallback/manual equivalent"}
                  </p>
                </div>

                <div className="space-y-3 text-sm">
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">Annual Revenue</span>
                    <span className="font-semibold text-slate-900">
                      ${mapMetrics.annualRevenue.toLocaleString("en-US")}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">NOI</span>
                    <span className="font-semibold text-slate-900">
                      ${mapMetrics.netOperatingIncome.toLocaleString("en-US")}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">Payback</span>
                    <span className="font-semibold text-slate-900">
                      {mapMetrics.paybackPeriod
                        ? `${mapMetrics.paybackPeriod.toFixed(1)} years`
                        : "Not achievable"}
                    </span>
                  </p>
                  <p className="flex justify-between gap-4">
                    <span className="text-slate-500">NPV</span>
                    <span className="font-semibold text-slate-900">
                      $
                      {mapMetrics.npv.toLocaleString("en-US", {
                        maximumFractionDigits: 0,
                      })}
                    </span>
                  </p>
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-emerald-200 bg-gradient-to-br from-emerald-50 to-white p-6 shadow-xl">
              <p className="text-sm text-emerald-700">Comparison Readout</p>
              <h2 className="mt-2 text-2xl font-semibold text-slate-900">
                Difference from map pricing
              </h2>

              <div className="mt-5 space-y-3">
                <div className="rounded-2xl border border-emerald-100 bg-white p-4">
                  <p className="text-sm text-slate-500">Revenue Delta</p>
                  <p
                    className={`mt-2 text-2xl font-bold ${
                      compareDelta.revenue >= 0 ? "text-emerald-600" : "text-rose-600"
                    }`}
                  >
                    {compareDelta.revenue >= 0 ? "+" : ""}$
                    {compareDelta.revenue.toLocaleString("en-US", {
                      maximumFractionDigits: 0,
                    })}
                  </p>
                </div>

                <div className="rounded-2xl border border-emerald-100 bg-white p-4">
                  <p className="text-sm text-slate-500">NOI Delta</p>
                  <p
                    className={`mt-2 text-2xl font-bold ${
                      compareDelta.noi >= 0 ? "text-emerald-600" : "text-rose-600"
                    }`}
                  >
                    {compareDelta.noi >= 0 ? "+" : ""}$
                    {compareDelta.noi.toLocaleString("en-US", {
                      maximumFractionDigits: 0,
                    })}
                  </p>
                </div>

                <div className="rounded-2xl border border-emerald-100 bg-white p-4">
                  <p className="text-sm text-slate-500">NPV Delta</p>
                  <p
                    className={`mt-2 text-2xl font-bold ${
                      compareDelta.npv >= 0 ? "text-emerald-600" : "text-rose-600"
                    }`}
                  >
                    {compareDelta.npv >= 0 ? "+" : ""}$
                    {compareDelta.npv.toLocaleString("en-US", {
                      maximumFractionDigits: 0,
                    })}
                  </p>
                </div>
              </div>
            </div>
          </section>
        )}

        <section className="rounded-[28px] border border-white/10 bg-white p-6 shadow-xl md:p-8">
          <div className="mb-6 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
            <div>
              <p className="text-sm text-slate-500">Cash Flow</p>
              <h2 className="mt-2 text-3xl font-semibold text-slate-900">
                Project cash flow chart
              </h2>
              <p className="mt-2 text-sm text-slate-500">
                Year 0 reflects project investment and later years reflect annual NOI.
              </p>
            </div>

            <div className="inline-flex rounded-full border border-slate-200 bg-slate-50 px-3 py-1 text-xs text-slate-600">
              {projectLifeYears} year project life
            </div>
          </div>

          <div className="rounded-3xl border border-slate-200 bg-gradient-to-b from-slate-50 to-white p-4">
            <div className="h-96 min-w-0">
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={cashFlowData}>
                  <CartesianGrid strokeDasharray="3 3" stroke="#dbe4ee" />
                  <XAxis
                    dataKey="year"
                    interval={2}
                    angle={-30}
                    textAnchor="end"
                    height={60}
                    tick={{ fontSize: 12, fill: "#64748b" }}
                  />
                  <YAxis
                    tickFormatter={(value) => `$${(value / 1000000).toFixed(1)}M`}
                    tick={{ fontSize: 12, fill: "#64748b" }}
                  />
                  <Tooltip
  contentStyle={{
    borderRadius: "16px",
    border: "1px solid #e2e8f0",
    boxShadow: "0 12px 30px rgba(15, 23, 42, 0.12)",
  }}
  formatter={(value) => {
    const numericValue =
      typeof value === "number"
        ? value
        : typeof value === "string"
        ? Number(value)
        : 0;

    return [
      `$${numericValue.toLocaleString("en-US", {
        maximumFractionDigits: 0,
      })}`,
      "Cash Flow",
    ];
  }}
/>
                  <Bar dataKey="cashFlow" radius={[8, 8, 0, 0]}>
                    {cashFlowData.map((entry, index) => (
                      <Cell
                        key={`cell-${index}`}
                        fill={entry.cashFlow < 0 ? "#0f172a" : "#10b981"}
                      />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        </section>
      </div>
    </main>
  );
}