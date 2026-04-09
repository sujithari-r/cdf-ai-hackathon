// "use client";

// import { useEffect, useState } from "react";
// import {
//   LineChart,
//   Line,
//   XAxis,
//   YAxis,
//   Tooltip,
//   CartesianGrid,
//   ResponsiveContainer,
// } from "recharts";

// type TrendPoint = {
//   month: string;
//   price: number;
// };

// type MarketData = {
//   electricityPrice: number;
//   capacityGrowth: number;
//   renewableShare: number;
//   trend: TrendPoint[];
// };

// export default function MarketPage() {
//   const [data, setData] = useState<MarketData | null>(null);

//   useEffect(() => {
//     async function fetchData() {
//       const res = await fetch("/api/market");
//       const json = await res.json();
//       setData(json);
//     }

//     fetchData();
//   }, []);

//   return (
//     <main className="p-10 space-y-8">
//       <div className="space-y-2">
//         <h1 className="text-3xl font-bold">Market Overview</h1>
//         <p className="text-gray-600">
//           View key renewable energy market metrics and trends.
//         </p>
//       </div>

//       {!data ? (
//         <p>Loading...</p>
//       ) : (
//         <>
//           <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
//             <div className="p-6 border rounded-xl shadow-sm">
//               <h2 className="text-sm text-gray-500">Electricity Price</h2>
//               <p className="text-2xl font-bold">
//                 ${data.electricityPrice.toFixed(3)}/kWh
//               </p>
//             </div>

//             <div className="p-6 border rounded-xl shadow-sm">
//               <h2 className="text-sm text-gray-500">Capacity Growth</h2>
//               <p className="text-2xl font-bold">{data.capacityGrowth}%</p>
//             </div>

//             <div className="p-6 border rounded-xl shadow-sm">
//               <h2 className="text-sm text-gray-500">Renewable Share</h2>
//               <p className="text-2xl font-bold">{data.renewableShare}%</p>
//             </div>
//           </div>

//           <section className="p-6 border rounded-xl shadow-sm">
//             <h2 className="text-xl font-semibold mb-4">
//               Electricity Price Trend
//             </h2>

//             <div className="h-80">
//               <ResponsiveContainer width="100%" height="100%">
//                 <LineChart data={data.trend}>
//                   <CartesianGrid strokeDasharray="3 3" />
//                   <XAxis dataKey="month" />
//                   <YAxis />
//                   <Tooltip />
//                   <Line type="monotone" dataKey="price" strokeWidth={2} />
//                 </LineChart>
//               </ResponsiveContainer>
//             </div>
//           </section>
//         </>
//       )}
//     </main>
//   );
// }


// "use client";

// import { useEffect, useMemo, useState } from "react";
// import {
//   LineChart,
//   Line,
//   XAxis,
//   YAxis,
//   Tooltip,
//   CartesianGrid,
//   ResponsiveContainer,
// } from "recharts";

// type TrendPoint = {
//   month: string;
//   price: number;
// };

// type MarketData = {
//   electricityPrice: number;
//   capacityGrowth: number;
//   renewableShare: number;
//   trend: TrendPoint[];
// };

// export default function MarketPage() {
//   const [data, setData] = useState<MarketData | null>(null);
//   const [range, setRange] = useState<"6M" | "ALL">("ALL");

//   useEffect(() => {
//     async function fetchData() {
//       const res = await fetch("/api/market");
//       const json = await res.json();
//       setData(json);
//     }

//     fetchData();
//   }, []);

//   const filteredTrend = useMemo(() => {
//     if (!data?.trend) return [];
//     if (range === "6M") return data.trend.slice(-6);
//     return data.trend;
//   }, [data, range]);

//   const trendSummary = useMemo(() => {
//     if (!filteredTrend || filteredTrend.length < 2) return null;

//     const first = filteredTrend[0].price;
//     const last = filteredTrend[filteredTrend.length - 1].price;
//     const change = last - first;
//     const percentage = first !== 0 ? (change / first) * 100 : 0;

//     return {
//       first,
//       last,
//       change,
//       percentage,
//       isPositive: change >= 0,
//       latestMonth: filteredTrend[filteredTrend.length - 1].month,
//       average:
//         filteredTrend.reduce((sum, item) => sum + item.price, 0) /
//         filteredTrend.length,
//       min: Math.min(...filteredTrend.map((item) => item.price)),
//       max: Math.max(...filteredTrend.map((item) => item.price)),
//     };
//   }, [filteredTrend]);

//   return (
//     <main className="min-h-screen bg-gradient-to-b from-slate-50 via-white to-slate-100 px-6 py-10 md:px-10">
//       <div className="mx-auto max-w-7xl space-y-8">
//         <section className="rounded-[32px] border border-slate-200 bg-white p-8 shadow-xl md:p-10">
//           <div className="grid gap-8 lg:grid-cols-[1.1fr_0.9fr]">
//             <div>
//               <div className="inline-flex rounded-full border border-blue-200 bg-blue-50 px-3 py-1 text-sm font-medium text-blue-700">
//                 Market Analysis
//               </div>

//               <h1 className="mt-4 text-4xl font-bold tracking-tight text-slate-900 md:text-5xl">
//                 Renewable market trends
//               </h1>

//               <p className="mt-4 max-w-3xl text-lg leading-8 text-slate-600">
//                 Explore electricity price movement, capacity growth, and
//                 renewable penetration through a cleaner analytics-focused market
//                 view.
//               </p>

//               <div className="mt-8 grid gap-4 sm:grid-cols-3">
//                 <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                   <p className="text-sm text-slate-500">Trend Range</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {range}
//                   </p>
//                 </div>

//                 <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                   <p className="text-sm text-slate-500">Latest Month</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {trendSummary?.latestMonth ?? "--"}
//                   </p>
//                 </div>

//                 <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                   <p className="text-sm text-slate-500">Direction</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {trendSummary
//                       ? trendSummary.isPositive
//                         ? "Uptrend"
//                         : "Downtrend"
//                       : "--"}
//                   </p>
//                 </div>
//               </div>
//             </div>

//             <div className="rounded-3xl border border-slate-200 bg-slate-50 p-6">
//               <div className="flex items-center justify-between">
//                 <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
//                   Market Snapshot
//                 </p>

//                 <div className="inline-flex rounded-full border border-slate-200 bg-white p-1">
//                   <button
//                     onClick={() => setRange("6M")}
//                     className={`rounded-full px-3 py-1 text-xs font-medium transition ${
//                       range === "6M"
//                         ? "bg-slate-900 text-white"
//                         : "text-slate-600"
//                     }`}
//                   >
//                     6M
//                   </button>
//                   <button
//                     onClick={() => setRange("ALL")}
//                     className={`rounded-full px-3 py-1 text-xs font-medium transition ${
//                       range === "ALL"
//                         ? "bg-slate-900 text-white"
//                         : "text-slate-600"
//                     }`}
//                   >
//                     All
//                   </button>
//                 </div>
//               </div>

//               <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
//                 <div className="rounded-2xl border border-slate-200 bg-white p-4">
//                   <p className="text-sm text-slate-500">Current Price</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {data ? `$${data.electricityPrice.toFixed(3)}/kWh` : "--"}
//                   </p>
//                 </div>

//                 <div className="rounded-2xl border border-slate-200 bg-white p-4">
//                   <p className="text-sm text-slate-500">Capacity Growth</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {data ? `${data.capacityGrowth.toFixed(1)}%` : "--"}
//                   </p>
//                 </div>

//                 <div className="rounded-2xl border border-slate-200 bg-white p-4">
//                   <p className="text-sm text-slate-500">Renewable Share</p>
//                   <p className="mt-2 text-2xl font-bold text-slate-900">
//                     {data ? `${data.renewableShare.toFixed(0)}%` : "--"}
//                   </p>
//                 </div>

//                 <div className="rounded-2xl border border-slate-200 bg-white p-4">
//                   <p className="text-sm text-slate-500">Trend Change</p>
//                   <p
//                     className={`mt-2 text-2xl font-bold ${
//                       trendSummary?.isPositive
//                         ? "text-emerald-600"
//                         : "text-rose-600"
//                     }`}
//                   >
//                     {trendSummary
//                       ? `${trendSummary.isPositive ? "+" : ""}${trendSummary.percentage.toFixed(1)}%`
//                       : "--"}
//                   </p>
//                 </div>
//               </div>
//             </div>
//           </div>
//         </section>

//         {!data ? (
//           <section className="grid grid-cols-1 gap-6 md:grid-cols-3">
//             {Array.from({ length: 3 }).map((_, index) => (
//               <div
//                 key={index}
//                 className="rounded-3xl border border-slate-200 bg-white p-6 animate-pulse"
//               >
//                 <div className="mb-4 h-4 w-28 rounded bg-slate-200" />
//                 <div className="mb-3 h-8 w-24 rounded bg-slate-200" />
//                 <div className="h-3 w-40 rounded bg-slate-100" />
//               </div>
//             ))}
//           </section>
//         ) : (
//           <>
//             <section className="grid grid-cols-1 gap-6 md:grid-cols-3">
//               <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                 <p className="text-sm text-slate-500">Electricity Price</p>
//                 <p className="mt-3 text-4xl font-bold text-slate-900">
//                   ${data.electricityPrice.toFixed(3)}
//                 </p>
//                 <p className="mt-1 text-sm text-slate-500">per kWh</p>
//               </div>

//               <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                 <p className="text-sm text-slate-500">Capacity Growth</p>
//                 <p className="mt-3 text-4xl font-bold text-slate-900">
//                   {data.capacityGrowth.toFixed(1)}%
//                 </p>
//                 <p className="mt-1 text-sm text-slate-500">
//                   Growth indicator
//                 </p>
//               </div>

//               <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                 <p className="text-sm text-slate-500">Renewable Share</p>
//                 <p className="mt-3 text-4xl font-bold text-slate-900">
//                   {data.renewableShare.toFixed(0)}%
//                 </p>
//                 <p className="mt-1 text-sm text-slate-500">
//                   Renewable penetration
//                 </p>
//               </div>
//             </section>

//             {trendSummary && (
//               <section className="grid grid-cols-1 gap-6 lg:grid-cols-[1.15fr_0.85fr]">
//                 <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                   <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
//                     <div>
//                       <h2 className="text-2xl font-semibold text-slate-900">
//                         Price Trend
//                       </h2>
//                       <p className="mt-1 text-sm text-slate-500">
//                         National monthly electricity price trend
//                       </p>
//                     </div>

//                     <div className="inline-flex rounded-full border border-slate-200 bg-slate-50 px-3 py-1 text-xs text-slate-600">
//                       Source: EIA Open Data API
//                     </div>
//                   </div>

//                   <div className="mt-6 rounded-3xl border border-slate-200 bg-slate-50 p-4">
//                     <div className="h-96 min-w-0">
//                       <ResponsiveContainer width="100%" height="100%">
//                         <LineChart
//                           data={filteredTrend}
//                           margin={{ top: 10, right: 20, left: 0, bottom: 20 }}
//                         >
//                           <CartesianGrid
//                             strokeDasharray="3 3"
//                             stroke="#dbe4ee"
//                           />
//                           <XAxis
//                             dataKey="month"
//                             angle={-25}
//                             textAnchor="end"
//                             height={60}
//                             interval={0}
//                             tick={{ fontSize: 12, fill: "#64748b" }}
//                           />
//                           <YAxis
//                             tickFormatter={(value) => `$${value.toFixed(2)}`}
//                             tick={{ fontSize: 12, fill: "#64748b" }}
//                           />
//                           <Tooltip
//                             contentStyle={{
//                               borderRadius: "16px",
//                               border: "1px solid #e2e8f0",
//                               boxShadow:
//                                 "0 12px 30px rgba(15, 23, 42, 0.12)",
//                             }}
//                             formatter={(value: number) => [
//                               `$${value.toFixed(3)}/kWh`,
//                               "Electricity Price",
//                             ]}
//                           />
//                           <Line
//                             type="monotone"
//                             dataKey="price"
//                             stroke="#0f172a"
//                             strokeWidth={3}
//                             dot={{
//                               r: 4,
//                               fill: "#10b981",
//                               stroke: "#0f172a",
//                               strokeWidth: 1.5,
//                             }}
//                             activeDot={{ r: 7, fill: "#059669" }}
//                           />
//                         </LineChart>
//                       </ResponsiveContainer>
//                     </div>
//                   </div>
//                 </div>

//                 <div className="space-y-6">
//                   <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                     <h3 className="text-xl font-semibold text-slate-900">
//                       Trend Summary
//                     </h3>

//                     <div className="mt-5 grid grid-cols-2 gap-4">
//                       <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                         <p className="text-sm text-slate-500">Average</p>
//                         <p className="mt-2 text-2xl font-bold text-slate-900">
//                           ${trendSummary.average.toFixed(3)}
//                         </p>
//                       </div>

//                       <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                         <p className="text-sm text-slate-500">Absolute Change</p>
//                         <p
//                           className={`mt-2 text-2xl font-bold ${
//                             trendSummary.isPositive
//                               ? "text-emerald-600"
//                               : "text-rose-600"
//                           }`}
//                         >
//                           {trendSummary.isPositive ? "+" : ""}
//                           {trendSummary.change.toFixed(3)}
//                         </p>
//                       </div>

//                       <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                         <p className="text-sm text-slate-500">Minimum</p>
//                         <p className="mt-2 text-2xl font-bold text-slate-900">
//                           ${trendSummary.min.toFixed(3)}
//                         </p>
//                       </div>

//                       <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                         <p className="text-sm text-slate-500">Maximum</p>
//                         <p className="mt-2 text-2xl font-bold text-slate-900">
//                           ${trendSummary.max.toFixed(3)}
//                         </p>
//                       </div>
//                     </div>
//                   </div>

//                   <div className="rounded-3xl border border-blue-200 bg-gradient-to-br from-blue-50 to-white p-6 shadow-sm">
//                     <p className="text-sm text-blue-700">Key Takeaways</p>
//                     <h3 className="mt-2 text-2xl font-semibold text-slate-900">
//                       What stands out in this market view
//                     </h3>

//                     <div className="mt-5 space-y-3">
//                       <div className="rounded-2xl border border-blue-100 bg-white p-4">
//                         <p className="font-medium text-slate-900">
//                           Price direction
//                         </p>
//                         <p className="mt-1 text-sm text-slate-500">
//                           The selected range is showing a{" "}
//                           {trendSummary.isPositive ? "positive" : "negative"}{" "}
//                           trend in electricity pricing.
//                         </p>
//                       </div>

//                       <div className="rounded-2xl border border-blue-100 bg-white p-4">
//                         <p className="font-medium text-slate-900">
//                           Revenue implication
//                         </p>
//                         <p className="mt-1 text-sm text-slate-500">
//                           Higher electricity prices can improve renewable
//                           project revenue assumptions.
//                         </p>
//                       </div>

//                       <div className="rounded-2xl border border-blue-100 bg-white p-4">
//                         <p className="font-medium text-slate-900">
//                           Planning context
//                         </p>
//                         <p className="mt-1 text-sm text-slate-500">
//                           Capacity growth and renewable share help frame broader
//                           market expansion and adoption.
//                         </p>
//                       </div>
//                     </div>
//                   </div>
//                 </div>
//               </section>
//             )}

//             <section className="grid grid-cols-1 gap-6 lg:grid-cols-2">
//               <div className="rounded-3xl border border-slate-200 bg-white p-6 shadow-sm">
//                 <p className="text-sm text-slate-500">Investor Read</p>
//                 <h3 className="mt-2 text-2xl font-semibold text-slate-900">
//                   What this means
//                 </h3>
//                 <p className="mt-4 leading-8 text-slate-600">
//                   Higher electricity prices can support stronger renewable
//                   project revenue assumptions, while capacity growth and
//                   renewable share provide useful direction on overall market
//                   momentum.
//                 </p>

//                 <div className="mt-6 space-y-3">
//                   <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                     <p className="font-medium text-slate-900">Revenue Signal</p>
//                     <p className="mt-1 text-sm text-slate-500">
//                       Higher power prices can strengthen expected project value.
//                     </p>
//                   </div>

//                   <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
//                     <p className="font-medium text-slate-900">Expansion Signal</p>
//                     <p className="mt-1 text-sm text-slate-500">
//                       Capacity growth helps support broader sector momentum.
//                     </p>
//                   </div>
//                 </div>
//               </div>

//               <div className="rounded-3xl border border-emerald-200 bg-gradient-to-br from-emerald-50 to-white p-6 shadow-sm">
//                 <p className="text-sm text-emerald-700">How to use this tab</p>
//                 <h3 className="mt-2 text-2xl font-semibold text-slate-900">
//                   Read the market before modeling
//                 </h3>
//                 <p className="mt-4 leading-8 text-slate-600">
//                   Use this tab to understand macro pricing direction first, then
//                   move to the Map tab for state-level context and the Calculator
//                   tab for project-specific returns.
//                 </p>

//                 <div className="mt-6 grid gap-3">
//                   <div className="rounded-2xl border border-emerald-100 bg-white p-4">
//                     <p className="font-medium text-slate-900">
//                       1. Review price direction
//                     </p>
//                     <p className="mt-1 text-sm text-slate-500">
//                       Check recent momentum using 6M or All.
//                     </p>
//                   </div>

//                   <div className="rounded-2xl border border-emerald-100 bg-white p-4">
//                     <p className="font-medium text-slate-900">
//                       2. Compare market strength
//                     </p>
//                     <p className="mt-1 text-sm text-slate-500">
//                       Use growth and renewable share as context signals.
//                     </p>
//                   </div>

//                   <div className="rounded-2xl border border-emerald-100 bg-white p-4">
//                     <p className="font-medium text-slate-900">
//                       3. Move to project economics
//                     </p>
//                     <p className="mt-1 text-sm text-slate-500">
//                       Use the Calculator once market conditions look favorable.
//                     </p>
//                   </div>
//                 </div>
//               </div>
//             </section>
//           </>
//         )}
//       </div>
//     </main>
//   );
// }

"use client";

import { useEffect, useMemo, useState } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  ResponsiveContainer,
} from "recharts";

type TrendPoint = {
  month: string;
  price: number;
};

type MarketData = {
  electricityPrice: number;
  capacityGrowth: number;
  renewableShare: number;
  trend: TrendPoint[];
};

export default function MarketPage() {
  const [data, setData] = useState<MarketData | null>(null);
  const [range, setRange] = useState<"6M" | "ALL">("ALL");

  useEffect(() => {
    async function fetchData() {
      const res = await fetch("/api/market");
      const json = await res.json();
      setData(json);
    }

    fetchData();
  }, []);

  const filteredTrend = useMemo(() => {
    if (!data?.trend) return [];
    if (range === "6M") return data.trend.slice(-6);
    return data.trend;
  }, [data, range]);

  const trendSummary = useMemo(() => {
    if (!filteredTrend || filteredTrend.length < 2) return null;

    const first = filteredTrend[0].price;
    const last = filteredTrend[filteredTrend.length - 1].price;
    const change = last - first;
    const percentage = first !== 0 ? (change / first) * 100 : 0;

    return {
      first,
      last,
      change,
      percentage,
      isPositive: change >= 0,
      latestMonth: filteredTrend[filteredTrend.length - 1].month,
      average:
        filteredTrend.reduce((sum, item) => sum + item.price, 0) /
        filteredTrend.length,
      min: Math.min(...filteredTrend.map((item) => item.price)),
      max: Math.max(...filteredTrend.map((item) => item.price)),
    };
  }, [filteredTrend]);

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-10 md:px-10">
      <div className="mx-auto max-w-7xl space-y-8">
        <section className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-slate-950 via-slate-900 to-blue-950/50 p-8 shadow-2xl md:p-10">
          <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,rgba(59,130,246,0.14),transparent_30%),radial-gradient(circle_at_bottom_right,rgba(16,185,129,0.10),transparent_30%)]" />

          <div className="relative grid gap-8 lg:grid-cols-[1.1fr_0.9fr]">
            <div>
              <div className="inline-flex rounded-full border border-blue-400/20 bg-blue-400/10 px-3 py-1 text-sm font-medium text-blue-300">
                Market Analysis
              </div>

              <h1 className="mt-4 text-4xl font-bold tracking-tight text-white md:text-5xl">
                Renewable market trends
              </h1>

              <p className="mt-4 max-w-3xl text-lg leading-8 text-slate-300">
                Explore electricity price movement, capacity growth, and
                renewable penetration through a cleaner analytics-focused market
                view.
              </p>

              <div className="mt-8 grid gap-4 sm:grid-cols-3">
                <div className="rounded-2xl border border-white/10 bg-white/5 p-4">
                  <p className="text-sm text-slate-400">Trend Range</p>
                  <p className="mt-2 text-2xl font-bold text-white">{range}</p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/5 p-4">
                  <p className="text-sm text-slate-400">Latest Month</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {trendSummary?.latestMonth ?? "--"}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/5 p-4">
                  <p className="text-sm text-slate-400">Direction</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {trendSummary
                      ? trendSummary.isPositive
                        ? "Uptrend"
                        : "Downtrend"
                      : "--"}
                  </p>
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white/5 p-6 backdrop-blur-sm">
              <div className="flex items-center justify-between">
                <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
                  Market Snapshot
                </p>

                <div className="inline-flex rounded-full border border-white/10 bg-black/20 p-1">
                  <button
                    onClick={() => setRange("6M")}
                    className={`rounded-full px-3 py-1 text-xs font-medium transition ${
                      range === "6M"
                        ? "bg-white text-slate-900"
                        : "text-slate-300"
                    }`}
                  >
                    6M
                  </button>
                  <button
                    onClick={() => setRange("ALL")}
                    className={`rounded-full px-3 py-1 text-xs font-medium transition ${
                      range === "ALL"
                        ? "bg-white text-slate-900"
                        : "text-slate-300"
                    }`}
                  >
                    All
                  </button>
                </div>
              </div>

              <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Current Price</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {data ? `$${data.electricityPrice.toFixed(3)}/kWh` : "--"}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Capacity Growth</p>
                  <p className="mt-2 text-2xl font-bold text-emerald-300">
                    {data ? `${data.capacityGrowth.toFixed(1)}%` : "--"}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Renewable Share</p>
                  <p className="mt-2 text-2xl font-bold text-blue-300">
                    {data ? `${data.renewableShare.toFixed(0)}%` : "--"}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Trend Change</p>
                  <p
                    className={`mt-2 text-2xl font-bold ${
                      trendSummary?.isPositive
                        ? "text-emerald-300"
                        : "text-rose-300"
                    }`}
                  >
                    {trendSummary
                      ? `${trendSummary.isPositive ? "+" : ""}${trendSummary.percentage.toFixed(1)}%`
                      : "--"}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>

        {!data ? (
          <section className="grid grid-cols-1 gap-6 md:grid-cols-3">
            {Array.from({ length: 3 }).map((_, index) => (
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
          <>
            <section className="grid grid-cols-1 gap-6 md:grid-cols-3">
              <div className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-white to-emerald-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
                <p className="text-sm text-slate-500">Electricity Price</p>
                <p className="mt-3 text-4xl font-bold text-slate-900">
                  ${data.electricityPrice.toFixed(3)}
                </p>
                <p className="mt-1 text-sm text-slate-500">per kWh</p>
                <div className="mt-5 h-1.5 rounded-full bg-emerald-100">
                  <div className="h-1.5 w-[72%] rounded-full bg-emerald-500" />
                </div>
              </div>

              <div className="rounded-3xl border border-blue-400/20 bg-gradient-to-br from-white to-blue-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
                <p className="text-sm text-slate-500">Capacity Growth</p>
                <p className="mt-3 text-4xl font-bold text-slate-900">
                  {data.capacityGrowth.toFixed(1)}%
                </p>
                <p className="mt-1 text-sm text-slate-500">
                  Growth indicator
                </p>
                <div className="mt-5 h-1.5 rounded-full bg-blue-100">
                  <div className="h-1.5 w-[58%] rounded-full bg-blue-500" />
                </div>
              </div>

              <div className="rounded-3xl border border-violet-400/20 bg-gradient-to-br from-white to-violet-50 p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl">
                <p className="text-sm text-slate-500">Renewable Share</p>
                <p className="mt-3 text-4xl font-bold text-slate-900">
                  {data.renewableShare.toFixed(0)}%
                </p>
                <p className="mt-1 text-sm text-slate-500">
                  Renewable penetration
                </p>
                <div className="mt-5 h-1.5 rounded-full bg-violet-100">
                  <div className="h-1.5 w-[32%] rounded-full bg-violet-500" />
                </div>
              </div>
            </section>

            {trendSummary && (
              <section className="grid grid-cols-1 gap-6 lg:grid-cols-[1.15fr_0.85fr]">
                <div className="rounded-[28px] border border-white/10 bg-white p-6 shadow-xl md:p-8">
                  <div className="mb-6 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                    <div>
                      <h2 className="text-3xl font-semibold text-slate-900">
                        Price Trend
                      </h2>
                      <p className="mt-2 text-sm text-slate-500">
                        National monthly electricity price trend
                      </p>
                    </div>

                    <div className="inline-flex rounded-full border border-slate-200 bg-slate-50 px-3 py-1 text-xs text-slate-600">
                      Source: EIA Open Data API
                    </div>
                  </div>

                  <div className="rounded-3xl border border-slate-200 bg-gradient-to-b from-slate-50 to-white p-4">
                    <div className="h-96 min-w-0">
                      <ResponsiveContainer width="100%" height="100%">
                        <LineChart
                          data={filteredTrend}
                          margin={{ top: 10, right: 20, left: 0, bottom: 20 }}
                        >
                          <CartesianGrid
                            strokeDasharray="3 3"
                            stroke="#dbe4ee"
                          />
                          <XAxis
                            dataKey="month"
                            angle={-25}
                            textAnchor="end"
                            height={60}
                            interval={0}
                            tick={{ fontSize: 12, fill: "#64748b" }}
                          />
                          <YAxis
                            tickFormatter={(value) => `$${value.toFixed(2)}`}
                            tick={{ fontSize: 12, fill: "#64748b" }}
                          />
                          <Tooltip
                            contentStyle={{
                              borderRadius: "16px",
                              border: "1px solid #e2e8f0",
                              boxShadow:
                                "0 12px 30px rgba(15, 23, 42, 0.12)",
                            }}
                            formatter={(value: number) => [
                              `$${value.toFixed(3)}/kWh`,
                              "Electricity Price",
                            ]}
                          />
                          <Line
                            type="monotone"
                            dataKey="price"
                            stroke="#0f172a"
                            strokeWidth={3}
                            dot={{
                              r: 4,
                              fill: "#10b981",
                              stroke: "#0f172a",
                              strokeWidth: 1.5,
                            }}
                            activeDot={{ r: 7, fill: "#059669" }}
                          />
                        </LineChart>
                      </ResponsiveContainer>
                    </div>
                  </div>
                </div>

                <div className="space-y-6">
                  <div className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
                    <h2 className="text-2xl font-semibold text-slate-900">
                      Trend Summary
                    </h2>

                    <div className="mt-5 grid grid-cols-2 gap-4">
                      <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                        <p className="text-sm text-slate-500">Average</p>
                        <p className="mt-2 text-2xl font-bold text-slate-900">
                          ${trendSummary.average.toFixed(3)}
                        </p>
                      </div>

                      <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                        <p className="text-sm text-slate-500">Absolute Change</p>
                        <p
                          className={`mt-2 text-2xl font-bold ${
                            trendSummary.isPositive
                              ? "text-emerald-600"
                              : "text-rose-600"
                          }`}
                        >
                          {trendSummary.isPositive ? "+" : ""}
                          {trendSummary.change.toFixed(3)}
                        </p>
                      </div>

                      <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                        <p className="text-sm text-slate-500">Minimum</p>
                        <p className="mt-2 text-2xl font-bold text-slate-900">
                          ${trendSummary.min.toFixed(3)}
                        </p>
                      </div>

                      <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                        <p className="text-sm text-slate-500">Maximum</p>
                        <p className="mt-2 text-2xl font-bold text-slate-900">
                          ${trendSummary.max.toFixed(3)}
                        </p>
                      </div>
                    </div>
                  </div>

                  <div className="rounded-3xl border border-blue-400/20 bg-gradient-to-br from-slate-900 to-blue-950 p-6 shadow-xl text-white">
                    <p className="text-sm text-blue-200">Key Takeaways</p>
                    <h2 className="mt-2 text-2xl font-semibold">
                      What stands out in this market view
                    </h2>

                    <div className="mt-5 space-y-3">
                      <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                        <p className="font-medium">Price direction</p>
                        <p className="mt-1 text-sm text-slate-200">
                          The selected range is showing a{" "}
                          {trendSummary.isPositive ? "positive" : "negative"}{" "}
                          trend in electricity pricing.
                        </p>
                      </div>

                      <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                        <p className="font-medium">Revenue implication</p>
                        <p className="mt-1 text-sm text-slate-200">
                          Higher electricity prices can improve renewable
                          project revenue assumptions.
                        </p>
                      </div>

                      <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                        <p className="font-medium">Planning context</p>
                        <p className="mt-1 text-sm text-slate-200">
                          Capacity growth and renewable share help frame broader
                          market expansion and adoption.
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </section>
            )}

            <section className="grid grid-cols-1 gap-6 lg:grid-cols-2">
              <div className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
                <p className="text-sm text-slate-500">Investor Read</p>
                <h2 className="mt-2 text-2xl font-semibold text-slate-900">
                  What this means
                </h2>
                <p className="mt-4 leading-8 text-slate-600">
                  Higher electricity prices can support stronger renewable
                  project revenue assumptions, while capacity growth and
                  renewable share provide useful direction on overall market
                  momentum.
                </p>

                <div className="mt-6 space-y-3">
                  <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                    <p className="font-medium text-slate-900">Revenue Signal</p>
                    <p className="mt-1 text-sm text-slate-500">
                      Higher power prices can strengthen expected project value.
                    </p>
                  </div>

                  <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                    <p className="font-medium text-slate-900">Expansion Signal</p>
                    <p className="mt-1 text-sm text-slate-500">
                      Capacity growth helps support broader sector momentum.
                    </p>
                  </div>
                </div>
              </div>

              <div className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-slate-900 to-emerald-950 p-6 shadow-xl text-white">
                <p className="text-sm text-emerald-200">How to use this tab</p>
                <h2 className="mt-2 text-2xl font-semibold">
                  Read the market before modeling
                </h2>
                <p className="mt-4 leading-8 text-slate-200">
                  Use this tab to understand macro pricing direction first, then
                  move to the Map tab for state-level context and the Calculator
                  tab for project-specific returns.
                </p>

                <div className="mt-6 grid gap-3">
                  <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                    <p className="font-medium">1. Review price direction</p>
                    <p className="mt-1 text-sm text-slate-200">
                      Check recent momentum using 6M or All.
                    </p>
                  </div>

                  <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                    <p className="font-medium">2. Compare market strength</p>
                    <p className="mt-1 text-sm text-slate-200">
                      Use growth and renewable share as context signals.
                    </p>
                  </div>

                  <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                    <p className="font-medium">3. Move to project economics</p>
                    <p className="mt-1 text-sm text-slate-200">
                      Use the Calculator once market conditions look favorable.
                    </p>
                  </div>
                </div>
              </div>
            </section>
          </>
        )}
      </div>
    </main>
  );
}