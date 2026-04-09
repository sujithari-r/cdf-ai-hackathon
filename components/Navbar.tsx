// "use client";

// import Link from "next/link";
// import { usePathname } from "next/navigation";

// const links = [
//   { name: "Home", path: "/" },
//   { name: "Market", path: "/market" },
//   { name: "Calculator", path: "/calculator" },
//   { name: "Map", path: "/map" },
//   { name: "AI Assistant", path: "/assistant" },
// ];

// export default function Navbar() {
//   const pathname = usePathname();

//   return (
//     <nav className="flex gap-6 p-4 border-b bg-white">
//       {links.map((link) => (
//         <Link
//           key={link.path}
//           href={link.path}
//           className={`${
//             pathname === link.path
//               ? "text-blue-600 font-semibold"
//               : "text-gray-600"
//           } hover:text-blue-500`}
//         >
//           {link.name}
//         </Link>
//       ))}
//     </nav>
//   );
// }

"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const links = [
  { name: "Home", path: "/" },
  { name: "Market", path: "/market" },
  { name: "Calculator", path: "/calculator" },
  { name: "Map", path: "/map" },
  { name: "AI Assistant", path: "/assistant" },
];

export default function Navbar() {
  const pathname = usePathname();

  return (
    <header className="sticky top-0 z-50 border-b border-white/10 bg-slate-950/85 backdrop-blur-xl">
      <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4 md:px-10">
        <Link href="/" className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-2xl border border-cyan-400/20 bg-cyan-400/10 text-sm font-bold text-cyan-300 shadow-lg shadow-cyan-950/30">
            RE
          </div>
          <div className="hidden sm:block">
            <p className="text-sm font-semibold tracking-wide text-white">
              Renewable Dashboard
            </p>
            <p className="text-xs text-slate-400">
              Market • Map • Calculator • AI
            </p>
          </div>
        </Link>

        <nav className="flex items-center gap-2 rounded-2xl border border-white/10 bg-white/5 p-1.5">
          {links.map((link) => {
            const isActive = pathname === link.path;

            return (
              <Link
                key={link.path}
                href={link.path}
                className={`rounded-xl px-4 py-2 text-sm font-medium transition-all duration-200 ${
                  isActive
                    ? "bg-gradient-to-r from-cyan-500 to-blue-500 text-white shadow-md"
                    : "text-slate-300 hover:bg-white/10 hover:text-white"
                }`}
              >
                {link.name}
              </Link>
            );
          })}
        </nav>
      </div>
    </header>
  );
}