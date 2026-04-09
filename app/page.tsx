// "use client";

// import Link from "next/link";
// import { useEffect, useState } from "react";
// import { useLocationContext } from "@/context/LocationContext";
// import { useCalculatorContext } from "@/context/CalculatorContext";

// type MarketData = {
//   electricityPrice: number;
//   capacityGrowth: number;
//   renewableShare: number;
// };

// export default function Home() {
//   const { selectedLocation } = useLocationContext();
//   const { calculatorSnapshot } = useCalculatorContext();

//   const [marketData, setMarketData] = useState<MarketData | null>(null);
//   const [loading, setLoading] = useState(true);

//   useEffect(() => {
//     async function fetchMarketData() {
//       try {
//         const res = await fetch("/api/market");
//         const json = await res.json();

//         setMarketData({
//           electricityPrice: json.electricityPrice ?? 0,
//           capacityGrowth: json.capacityGrowth ?? 0,
//           renewableShare: json.renewableShare ?? 0,
//         });
//       } catch {
//         setMarketData({
//           electricityPrice: 0,
//           capacityGrowth: 0,
//           renewableShare: 0,
//         });
//       } finally {
//         setLoading(false);
//       }
//     }

//     fetchMarketData();
//   }, []);

//   return (
//     <main className="min-h-screen bg-gradient-to-b from-white to-gray-50 px-6 py-10 md:px-10 space-y-10">
//       <section className="rounded-3xl border bg-white shadow-sm p-8 md:p-12">
//         <div className="max-w-4xl space-y-5">
//           <div className="inline-flex items-center rounded-full border px-3 py-1 text-sm text-gray-600 bg-gray-50">
//             U.S. Renewable Energy Investment Analysis
//           </div>

//           <h1 className="text-4xl md:text-6xl font-bold tracking-tight leading-tight">
//             Make smarter renewable energy investment decisions
//           </h1>

//           <p className="text-lg text-gray-600 max-w-3xl">
//             Explore market signals, evaluate project economics, compare
//             location-based pricing, and use AI to interpret real dashboard
//             context.
//           </p>

//           <div className="flex flex-wrap gap-3 pt-2">
//             <Link
//               href="/calculator"
//               className="rounded-xl bg-black px-5 py-3 text-white font-medium hover:opacity-90 transition"
//             >
//               Start with Calculator
//             </Link>

//             <Link
//               href="/map"
//               className="rounded-xl border px-5 py-3 font-medium bg-white hover:bg-gray-50 transition"
//             >
//               View Map Insights
//             </Link>
//           </div>
//         </div>
//       </section>

//       <section className="space-y-4">
//         <div className="flex items-center justify-between">
//           <h2 className="text-2xl font-semibold">Live Highlights</h2>
//           <p className="text-sm text-gray-500">Current dashboard snapshot</p>
//         </div>

//         {loading ? (
//           <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-6">
//             {Array.from({ length: 4 }).map((_, index) => (
//               <div
//                 key={index}
//                 className="rounded-2xl border bg-white p-6 shadow-sm animate-pulse"
//               >
//                 <div className="h-4 w-28 bg-gray-200 rounded mb-4" />
//                 <div className="h-8 w-24 bg-gray-200 rounded mb-3" />
//                 <div className="h-3 w-40 bg-gray-100 rounded" />
//               </div>
//             ))}
//           </div>
//         ) : (
//           <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-6">
//             <div className="rounded-2xl border bg-white p-6 shadow-sm hover:shadow-md transition">
//               <p className="text-sm text-gray-500">U.S. Electricity Price</p>
//               <p className="text-3xl font-bold mt-2">
//                 ${marketData?.electricityPrice.toFixed(3) ?? "0.000"}/kWh
//               </p>
//               <p className="text-sm text-gray-500 mt-3">Source: EIA API</p>
//             </div>

//             <div className="rounded-2xl border bg-white p-6 shadow-sm hover:shadow-md transition">
//               <p className="text-sm text-gray-500">Capacity Growth</p>
//               <p className="text-3xl font-bold mt-2">
//                 {marketData?.capacityGrowth ?? 0}%
//               </p>
//               <p className="text-sm text-gray-500 mt-3">
//                 Market growth indicator
//               </p>
//             </div>

//             <div className="rounded-2xl border bg-white p-6 shadow-sm hover:shadow-md transition">
//               <p className="text-sm text-gray-500">Selected Location</p>
//               <p className="text-3xl font-bold mt-2">
//                 {selectedLocation?.name ?? "None"}
//               </p>
//               <p className="text-sm text-gray-500 mt-3">
//                 {selectedLocation
//                   ? `Rate: $${selectedLocation.electricityRate}/kWh`
//                   : "Choose a state from the Map tab"}
//               </p>
//             </div>

//             <div className="rounded-2xl border bg-white p-6 shadow-sm hover:shadow-md transition">
//               <p className="text-sm text-gray-500">Current Project NPV</p>
//               <p className="text-3xl font-bold mt-2">
//                 {calculatorSnapshot
//                   ? `$${calculatorSnapshot.npv.toLocaleString("en-US", {
//                       maximumFractionDigits: 0,
//                     })}`
//                   : "Not ready"}
//               </p>
//               <p className="text-sm text-gray-500 mt-3">
//                 {calculatorSnapshot
//                   ? `Scenario: ${calculatorSnapshot.scenario}`
//                   : "Open Calculator to generate metrics"}
//               </p>
//             </div>
//           </div>
//         )}
//       </section>

//       <section className="grid grid-cols-1 lg:grid-cols-2 gap-6">
//         <div className="rounded-2xl border bg-white p-8 shadow-sm">
//           <p className="text-sm text-gray-500">Why this dashboard matters</p>
//           <h2 className="text-2xl font-semibold mt-2">
//             One place for market, location, financial, and AI analysis
//           </h2>
//           <p className="text-gray-600 mt-4 leading-7">
//             Instead of jumping between spreadsheets, policy pages, and market
//             reports, this dashboard combines market signals, project economics,
//             map-based pricing, and AI-backed interpretation into one workflow.
//           </p>
//         </div>

//         <div className="rounded-2xl border bg-black text-white p-8 shadow-sm">
//           <p className="text-sm text-gray-300">AI Assistant</p>
//           <h2 className="text-2xl font-semibold mt-2">
//             Ask grounded investment questions
//           </h2>
//           <p className="text-gray-300 mt-4 leading-7">
//             Use the assistant to analyze selected locations, review financial
//             outputs, and interpret tradeoffs using the current dashboard
//             context.
//           </p>

//           <div className="pt-5">
//             <Link
//               href="/assistant"
//               className="inline-flex rounded-xl bg-white px-5 py-3 font-medium text-black hover:bg-gray-100 transition"
//             >
//               Open AI Assistant
//             </Link>
//           </div>
//         </div>
//       </section>
//     </main>
//   );
// }


"use client";

import Link from "next/link";
import { useEffect, useMemo, useState } from "react";
import { useLocationContext } from "@/context/LocationContext";
import { useCalculatorContext } from "@/context/CalculatorContext";

type MarketData = {
  electricityPrice: number;
  capacityGrowth: number;
  renewableShare: number;
};

export default function Home() {
  const { selectedLocation } = useLocationContext();
  const { calculatorSnapshot } = useCalculatorContext();

  const [marketData, setMarketData] = useState<MarketData | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchMarketData() {
      try {
        const res = await fetch("/api/market");
        const json = await res.json();

        setMarketData({
          electricityPrice: json.electricityPrice ?? 0,
          capacityGrowth: json.capacityGrowth ?? 0,
          renewableShare: json.renewableShare ?? 0,
        });
      } catch {
        setMarketData({
          electricityPrice: 0,
          capacityGrowth: 0,
          renewableShare: 0,
        });
      } finally {
        setLoading(false);
      }
    }

    fetchMarketData();
  }, []);

  const locationValue = useMemo(() => {
    if (!selectedLocation) return "No location selected";
    return selectedLocation.name;
  }, [selectedLocation]);

  const npvValue = useMemo(() => {
    if (!calculatorSnapshot) return "Not ready";
    return `$${calculatorSnapshot.npv.toLocaleString("en-US", {
      maximumFractionDigits: 0,
    })}`;
  }, [calculatorSnapshot]);

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-10 md:px-10">
      <div className="mx-auto max-w-7xl space-y-8">
        <section className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-slate-950 via-slate-900 to-emerald-950/50 p-8 shadow-2xl md:p-10">
          <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,rgba(59,130,246,0.16),transparent_30%),radial-gradient(circle_at_bottom_right,rgba(16,185,129,0.18),transparent_30%)]" />

          <div className="relative grid gap-8 lg:grid-cols-[1.2fr_0.8fr]">
            <div>
              <div className="inline-flex rounded-full border border-emerald-400/20 bg-emerald-400/10 px-3 py-1 text-sm font-medium text-emerald-300">
                U.S. Renewable Energy Investment Analysis
              </div>

              <h1 className="mt-5 max-w-4xl text-4xl font-bold tracking-tight text-white md:text-6xl md:leading-[1.05]">
                Make smarter renewable energy investment decisions
              </h1>

              <p className="mt-5 max-w-3xl text-lg leading-8 text-slate-300">
                Explore market signals, evaluate project economics, compare
                location-based pricing, and use AI to interpret real dashboard
                context in one connected workflow.
              </p>

              <div className="mt-8 flex flex-wrap gap-3">
                <Link
                  href="/calculator"
                  className="inline-flex items-center rounded-xl bg-white px-5 py-3 font-medium text-slate-950 transition hover:bg-slate-100"
                >
                  Start with Calculator
                </Link>

                <Link
                  href="/map"
                  className="inline-flex items-center rounded-xl border border-white/15 bg-white/5 px-5 py-3 font-medium text-white transition hover:bg-white/10"
                >
                  View Map Insights
                </Link>

                <Link
                  href="/assistant"
                  className="inline-flex items-center rounded-xl border border-emerald-400/20 bg-emerald-400/10 px-5 py-3 font-medium text-emerald-300 transition hover:bg-emerald-400/15"
                >
                  Open AI Assistant
                </Link>
              </div>

              <div className="mt-8 flex flex-wrap gap-3">
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Market signals
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Location comparison
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Financial modeling
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  AI-backed interpretation
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white/5 p-6 backdrop-blur-sm">
              <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
                Dashboard Snapshot
              </p>

              <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Electricity Price</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {loading || !marketData
                      ? "--"
                      : `$${marketData.electricityPrice.toFixed(3)}/kWh`}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Capacity Growth</p>
                  <p className="mt-2 text-2xl font-bold text-emerald-300">
                    {loading || !marketData
                      ? "--"
                      : `${marketData.capacityGrowth.toFixed(1)}%`}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Renewable Share</p>
                  <p className="mt-2 text-2xl font-bold text-blue-300">
                    {loading || !marketData
                      ? "--"
                      : `${marketData.renewableShare.toFixed(0)}%`}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Project NPV</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {npvValue}
                  </p>
                </div>
              </div>

              <div className="mt-5 rounded-2xl border border-emerald-400/15 bg-emerald-400/10 p-4">
                <p className="text-sm text-emerald-200">Selected Location</p>
                <p className="mt-1 text-lg font-semibold text-white">
                  {locationValue}
                </p>
                <p className="mt-2 text-sm text-slate-300">
                  {selectedLocation
                    ? `Rate: $${selectedLocation.electricityRate}/kWh`
                    : "Choose a state from the Map tab to connect market and project context."}
                </p>
              </div>
            </div>
          </div>
        </section>

        {loading ? (
          <section className="grid grid-cols-1 gap-6 md:grid-cols-2 xl:grid-cols-4">
            {Array.from({ length: 4 }).map((_, index) => (
              <div
                key={index}
                className="rounded-3xl border border-white/10 bg-slate-900 p-6 animate-pulse"
              >
                <div className="mb-4 h-4 w-28 rounded bg-slate-700" />
                <div className="mb-3 h-8 w-24 rounded bg-slate-700" />
                <div className="h-3 w-40 rounded bg-slate-800" />
              </div>
            ))}
          </section>
        ) : (
          <section className="grid grid-cols-1 gap-6 md:grid-cols-2 xl:grid-cols-4">
            <div className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-white to-emerald-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
              <div className="flex items-center justify-between">
                <p className="text-sm font-medium text-slate-600">
                  U.S. Electricity Price
                </p>
                <span className="rounded-full border border-emerald-200 bg-emerald-100 px-2.5 py-1 text-xs font-medium text-emerald-700">
                  EIA
                </span>
              </div>
              <p className="mt-4 text-4xl font-bold text-slate-900">
                ${marketData?.electricityPrice.toFixed(3) ?? "0.000"}
              </p>
              <p className="mt-1 text-sm text-slate-500">per kWh</p>
              <div className="mt-5 h-1.5 rounded-full bg-emerald-100">
                <div className="h-1.5 w-[72%] rounded-full bg-emerald-500" />
              </div>
            </div>

            <div className="rounded-3xl border border-blue-400/20 bg-gradient-to-br from-white to-blue-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
              <div className="flex items-center justify-between">
                <p className="text-sm font-medium text-slate-600">
                  Capacity Growth
                </p>
                <span className="rounded-full border border-blue-200 bg-blue-100 px-2.5 py-1 text-xs font-medium text-blue-700">
                  Market
                </span>
              </div>
              <p className="mt-4 text-4xl font-bold text-slate-900">
                {marketData?.capacityGrowth ?? 0}%
              </p>
              <p className="mt-1 text-sm text-slate-500">
                Growth indicator
              </p>
              <div className="mt-5 h-1.5 rounded-full bg-blue-100">
                <div className="h-1.5 w-[58%] rounded-full bg-blue-500" />
              </div>
            </div>

            <div className="rounded-3xl border border-violet-400/20 bg-gradient-to-br from-white to-violet-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
              <div className="flex items-center justify-between">
                <p className="text-sm font-medium text-slate-600">
                  Selected Location
                </p>
                <span className="rounded-full border border-violet-200 bg-violet-100 px-2.5 py-1 text-xs font-medium text-violet-700">
                  Map
                </span>
              </div>
              <p className="mt-4 text-3xl font-bold text-slate-900">
                {selectedLocation?.name ?? "None"}
              </p>
              <p className="mt-1 text-sm text-slate-500">
                {selectedLocation
                  ? `Rate: $${selectedLocation.electricityRate}/kWh`
                  : "Choose a state from Map"}
              </p>
              <div className="mt-5 h-1.5 rounded-full bg-violet-100">
                <div className="h-1.5 w-[46%] rounded-full bg-violet-500" />
              </div>
            </div>

            <div className="rounded-3xl border border-amber-400/20 bg-gradient-to-br from-white to-amber-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
              <div className="flex items-center justify-between">
                <p className="text-sm font-medium text-slate-600">
                  Current Project NPV
                </p>
                <span className="rounded-full border border-amber-200 bg-amber-100 px-2.5 py-1 text-xs font-medium text-amber-700">
                  Finance
                </span>
              </div>
              <p className="mt-4 text-3xl font-bold text-slate-900">
                {calculatorSnapshot
                  ? `$${calculatorSnapshot.npv.toLocaleString("en-US", {
                      maximumFractionDigits: 0,
                    })}`
                  : "Not ready"}
              </p>
              <p className="mt-1 text-sm text-slate-500">
                {calculatorSnapshot
                  ? `Scenario: ${calculatorSnapshot.scenario}`
                  : "Open Calculator to generate metrics"}
              </p>
              <div className="mt-5 h-1.5 rounded-full bg-amber-100">
                <div className="h-1.5 w-[64%] rounded-full bg-amber-500" />
              </div>
            </div>
          </section>
        )}

        <section className="grid grid-cols-1 gap-6 lg:grid-cols-[1.1fr_0.9fr]">
          <div className="rounded-3xl border border-white/10 bg-white p-7 shadow-xl">
            <p className="text-sm text-slate-500">Platform Overview</p>
            <h2 className="mt-2 text-3xl font-semibold text-slate-900">
              One place for market, map, calculator, and AI workflows
            </h2>
            <p className="mt-4 leading-8 text-slate-600">
              This dashboard helps you move from national market signals to
              location-specific pricing, then into project economics and AI
              interpretation without jumping across disconnected tools.
            </p>

            <div className="mt-6 grid gap-4 md:grid-cols-3">
              <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                <p className="text-sm font-semibold text-slate-800">Market</p>
                <p className="mt-2 text-sm leading-6 text-slate-500">
                  Review pricing, growth, and renewable share.
                </p>
              </div>

              <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                <p className="text-sm font-semibold text-slate-800">Map</p>
                <p className="mt-2 text-sm leading-6 text-slate-500">
                  Compare state-level electricity rates and opportunity.
                </p>
              </div>

              <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                <p className="text-sm font-semibold text-slate-800">
                  Calculator
                </p>
                <p className="mt-2 text-sm leading-6 text-slate-500">
                  Evaluate NPV, NOI, and payback with project inputs.
                </p>
              </div>
            </div>
          </div>

          <div className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-slate-900 to-emerald-950 p-7 text-white shadow-xl">
            <p className="text-sm text-emerald-200">AI Assistant</p>
            <h2 className="mt-2 text-3xl font-semibold">
              Ask grounded investment questions
            </h2>
            <p className="mt-4 leading-8 text-slate-200">
              Use the assistant to interpret dashboard context, selected
              locations, and project outputs so you can make faster, more
              confident investment decisions.
            </p>

            <div className="mt-6 space-y-3">
              <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                <p className="font-medium">1. Review market context</p>
                <p className="mt-1 text-sm text-slate-200">
                  Start with price, growth, and renewable share.
                </p>
              </div>

              <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                <p className="font-medium">2. Connect location insights</p>
                <p className="mt-1 text-sm text-slate-200">
                  Use the selected state to bring in local pricing context.
                </p>
              </div>

              <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                <p className="font-medium">3. Interpret project returns</p>
                <p className="mt-1 text-sm text-slate-200">
                  Ask AI to explain tradeoffs and economics clearly.
                </p>
              </div>
            </div>

            <div className="mt-6">
              <Link
                href="/assistant"
                className="inline-flex rounded-xl bg-white px-5 py-3 font-medium text-slate-950 transition hover:bg-slate-100"
              >
                Open AI Assistant
              </Link>
            </div>
          </div>
        </section>
      </div>
    </main>
  );
}