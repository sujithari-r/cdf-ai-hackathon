// "use client";

// import { useState } from "react";
// import { useCalculatorContext } from "@/context/CalculatorContext";
// import { useLocationContext } from "@/context/LocationContext";

// type ChatMessage = {
//   role: "user" | "assistant";
//   content: string;
// };

// export default function AssistantPage() {
//   const [input, setInput] = useState("");
//   const [loading, setLoading] = useState(false);
//   const { selectedLocation } = useLocationContext();
//   const { calculatorSnapshot } = useCalculatorContext();
//   const [messages, setMessages] = useState<ChatMessage[]>([
//     {
//       role: "assistant",
//       content:
//         "Hi — I can help analyze renewable energy markets, project economics, and location-based tradeoffs.",
//     },
//   ]);

//   async function handleSend() {
//     if (!input.trim() || loading) return;

//     const userMessage = input.trim();

//     setMessages((prev) => [
//       ...prev,
//       { role: "user", content: userMessage },
//     ]);
//     setInput("");
//     setLoading(true);

//     try {
//       const res = await fetch("/api/assistant", {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify({
//   question: userMessage,
//   context: {
//     selectedLocation,
//     calculatorSnapshot,
//   },
// }),
//       });

//       const data = await res.json();

//       setMessages((prev) => [
//         ...prev,
//         {
//           role: "assistant",
//           content: data.answer ?? "No response received.",
//         },
//       ]);
//     } catch (error) {
//       setMessages((prev) => [
//         ...prev,
//         {
//           role: "assistant",
//           content: "Something went wrong while contacting the assistant.",
//         },
//       ]);
//     } finally {
//       setLoading(false);
//     }
//   }

//   return (
//     <main className="p-10 space-y-6">
//       <div className="space-y-2">
//         <h1 className="text-3xl font-bold">AI Research Assistant</h1>
//         <p className="text-gray-600">
//           Ask questions about renewable energy markets, project economics, and regional tradeoffs.
//         </p>
//       </div>

//       <section className="border rounded-xl shadow-sm p-6 bg-white">
//         <div className="h-[400px] border rounded-lg p-4 bg-gray-50 mb-4 overflow-y-auto space-y-3">
//           {messages.map((message, index) => (
//             <div
//               key={index}
//               className={`max-w-[80%] rounded-lg p-3 ${
//                 message.role === "user"
//                   ? "ml-auto bg-black text-white"
//                   : "bg-white border text-gray-800"
//               }`}
//             >
//               <p className="text-sm font-medium mb-1">
//                 {message.role === "user" ? "You" : "Assistant"}
//               </p>
//               <p>{message.content}</p>
//             </div>
//           ))}

//           {loading && (
//             <div className="max-w-[80%] rounded-lg p-3 bg-white border text-gray-800">
//               <p className="text-sm font-medium mb-1">Assistant</p>
//               <p>Thinking...</p>
//             </div>
//           )}
//         </div>

//         <div className="flex gap-3">
//           <input
//             type="text"
//             value={input}
//             onChange={(e) => setInput(e.target.value)}
//             onKeyDown={(e) => {
//               if (e.key === "Enter") handleSend();
//             }}
//             placeholder="Ask about market trends, project returns, or selected locations..."
//             className="flex-1 border rounded-lg p-3"
//           />
//           <button
//             onClick={handleSend}
//             disabled={loading}
//             className="px-5 py-3 rounded-lg bg-black text-white disabled:opacity-50"
//           >
//             {loading ? "Sending..." : "Send"}
//           </button>
//         </div>
//       </section>
//     </main>
//   );
// }



"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { useCalculatorContext } from "@/context/CalculatorContext";
import { useLocationContext } from "@/context/LocationContext";

type ChatMessage = {
  role: "user" | "assistant";
  content: string;
  suggestions?: string[];
};

export default function AssistantPage() {
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const { selectedLocation } = useLocationContext();
  const { calculatorSnapshot } = useCalculatorContext();

  const [messages, setMessages] = useState<ChatMessage[]>([
    {
      role: "assistant",
      content:
        "Hello. I’m your renewable energy AI analyst. I can help you evaluate market signals, project economics, selected locations, and investment tradeoffs using the current dashboard context.",
      suggestions: [
        "Analyze the current project economics.",
        "Is the selected location a good investment opportunity?",
        "Explain the current NPV and payback in simple terms.",
      ],
    },
  ]);

  const [typingMessageIndex, setTypingMessageIndex] = useState<number | null>(null);
  const [displayedAssistantContent, setDisplayedAssistantContent] = useState("");
  const messagesEndRef = useRef<HTMLDivElement | null>(null);
  const textareaRef = useRef<HTMLTextAreaElement | null>(null);

  const contextSummary = useMemo(() => {
    return {
      location: selectedLocation?.name ?? "No location selected",
      rate: selectedLocation ? `$${selectedLocation.electricityRate}/kWh` : "--",
      solarScore: selectedLocation ? `${selectedLocation.solarScore}/10` : "--",
      scenario: calculatorSnapshot?.scenario ?? "Not available",
      npv: calculatorSnapshot
        ? `$${calculatorSnapshot.npv.toLocaleString("en-US", {
            maximumFractionDigits: 0,
          })}`
        : "--",
      revenue: calculatorSnapshot
        ? `$${calculatorSnapshot.annualRevenue.toLocaleString("en-US", {
            maximumFractionDigits: 0,
          })}`
        : "--",
      noi: calculatorSnapshot
        ? `$${calculatorSnapshot.netOperatingIncome.toLocaleString("en-US", {
            maximumFractionDigits: 0,
          })}`
        : "--",
      payback: calculatorSnapshot?.paybackPeriod
        ? `${calculatorSnapshot.paybackPeriod.toFixed(1)} years`
        : "Not available",
    };
  }, [selectedLocation, calculatorSnapshot]);

  const quickPrompts = [
    "Analyze the current project economics.",
    "Is the selected location a good investment opportunity?",
    "Explain the current NPV and payback in simple terms.",
    "Compare market signals with this calculator scenario.",
  ];

  function buildFollowUpSuggestions(answer: string): string[] {
    const answerLower = answer.toLowerCase();

    if (
      answerLower.includes("npv") ||
      answerLower.includes("payback") ||
      answerLower.includes("revenue") ||
      answerLower.includes("noi")
    ) {
      return [
        "Explain which input has the biggest impact on returns.",
        "How can I improve this project's NPV?",
        "Compare this with a more optimistic scenario.",
      ];
    }

    if (
      answerLower.includes("location") ||
      answerLower.includes("state") ||
      answerLower.includes("solar") ||
      answerLower.includes("electricity rate")
    ) {
      return [
        "Compare this location with another state.",
        "Is this location better for revenue or long-term growth?",
        "How does the electricity rate affect project returns here?",
      ];
    }

    if (
      answerLower.includes("market") ||
      answerLower.includes("trend") ||
      answerLower.includes("renewable share") ||
      answerLower.includes("capacity growth")
    ) {
      return [
        "How should I use these market signals in the calculator?",
        "What is the main investment risk from this market view?",
        "Summarize this market outlook in simple terms.",
      ];
    }

    return [
      "Summarize that in simple terms.",
      "What should I look at next in the dashboard?",
      "Give me the key investment takeaway.",
    ];
  }

  function scrollToBottom() {
    requestAnimationFrame(() => {
      messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    });
  }

  function resizeTextarea() {
    if (!textareaRef.current) return;
    textareaRef.current.style.height = "auto";
    textareaRef.current.style.height = `${Math.min(
      textareaRef.current.scrollHeight,
      180
    )}px`;
  }

  useEffect(() => {
    scrollToBottom();
  }, [messages, loading, displayedAssistantContent]);

  useEffect(() => {
    resizeTextarea();
  }, [input]);

  useEffect(() => {
    if (typingMessageIndex === null) return;

    const targetMessage = messages[typingMessageIndex];
    if (!targetMessage || targetMessage.role !== "assistant") {
      setTypingMessageIndex(null);
      return;
    }

    const fullText = targetMessage.content;
    let currentIndex = 0;

    setDisplayedAssistantContent("");

    const interval = window.setInterval(() => {
      currentIndex += 2;
      const nextText = fullText.slice(0, currentIndex);
      setDisplayedAssistantContent(nextText);

      if (currentIndex >= fullText.length) {
        window.clearInterval(interval);
        setDisplayedAssistantContent(fullText);
        setTypingMessageIndex(null);
      }
    }, 14);

    return () => window.clearInterval(interval);
  }, [typingMessageIndex, messages]);

  async function sendMessage(messageText?: string) {
    if (loading) return;

    const finalMessage = (messageText ?? input).trim();
    if (!finalMessage) return;

    setMessages((prev) => [...prev, { role: "user", content: finalMessage }]);
    setInput("");
    setLoading(true);

    try {
      const res = await fetch("/api/assistant", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          question: finalMessage,
          context: {
            selectedLocation,
            calculatorSnapshot,
          },
        }),
      });

      const data = await res.json();
      const answer = data.answer ?? "No response received.";

      const assistantMessage: ChatMessage = {
        role: "assistant",
        content: answer,
        suggestions: buildFollowUpSuggestions(answer),
      };

      setMessages((prev) => {
        const updated = [...prev, assistantMessage];
        return updated;
      });

      const nextIndex = messages.length + 1;
      setTypingMessageIndex(nextIndex);
    } catch (error) {
      const fallback =
        "Something went wrong while contacting the assistant.";

      setMessages((prev) => [
        ...prev,
        {
          role: "assistant",
          content: fallback,
          suggestions: [
            "Try that again.",
            "Summarize the current dashboard context.",
            "Analyze the selected location instead.",
          ],
        },
      ]);

      const nextIndex = messages.length + 1;
      setTypingMessageIndex(nextIndex);
    } finally {
      setLoading(false);
      requestAnimationFrame(() => {
        if (textareaRef.current) {
          textareaRef.current.style.height = "auto";
        }
      });
    }
  }

  function handleSend() {
    sendMessage();
  }

  function handleSuggestionClick(suggestion: string) {
    sendMessage(suggestion);
  }

  return (
    <main className="min-h-screen bg-slate-950 px-6 py-10 md:px-10">
      <div className="mx-auto max-w-7xl space-y-8">
        <section className="relative overflow-hidden rounded-[32px] border border-white/10 bg-gradient-to-br from-slate-950 via-slate-900 to-cyan-950/50 p-8 shadow-2xl md:p-10">
          <div className="absolute inset-0 bg-[radial-gradient(circle_at_top_left,rgba(59,130,246,0.16),transparent_30%),radial-gradient(circle_at_bottom_right,rgba(16,185,129,0.14),transparent_30%)]" />

          <div className="relative grid gap-8 lg:grid-cols-[1.15fr_0.85fr]">
            <div>
              <div className="inline-flex rounded-full border border-cyan-400/20 bg-cyan-400/10 px-3 py-1 text-sm font-medium text-cyan-300">
                AI Research Assistant
              </div>

              <h1 className="mt-5 text-4xl font-bold tracking-tight text-white md:text-5xl">
                Speak with your renewable energy AI analyst
              </h1>

              <p className="mt-4 max-w-3xl text-lg leading-8 text-slate-300">
                Ask about renewable energy markets, project economics,
                location-based pricing, and investment tradeoffs using the live
                dashboard context.
              </p>

              <div className="mt-8 flex flex-wrap gap-3">
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Market-aware
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Calculator-connected
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  Location-informed
                </div>
                <div className="rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-slate-200">
                  AI-generated insights
                </div>
              </div>
            </div>

            <div className="rounded-3xl border border-white/10 bg-white/5 p-6 backdrop-blur-sm">
              <p className="text-sm uppercase tracking-[0.2em] text-slate-400">
                Live Context
              </p>

              <div className="mt-6 grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border border-white/10 bg-black/20 p-4 sm:col-span-2">
                  <p className="text-sm text-slate-400">Selected Location</p>
                  <p className="mt-2 text-2xl font-bold text-white">
                    {contextSummary.location}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Electricity Rate</p>
                  <p className="mt-2 text-2xl font-bold text-emerald-300">
                    {contextSummary.rate}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Solar Score</p>
                  <p className="mt-2 text-2xl font-bold text-blue-300">
                    {contextSummary.solarScore}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">Scenario</p>
                  <p className="mt-2 text-xl font-bold text-white capitalize">
                    {contextSummary.scenario}
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-black/20 p-4">
                  <p className="text-sm text-slate-400">NPV</p>
                  <p className="mt-2 text-xl font-bold text-amber-300">
                    {contextSummary.npv}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="grid grid-cols-1 gap-6 lg:grid-cols-[0.75fr_1.25fr]">
          <div className="space-y-6">
            <section className="rounded-3xl border border-emerald-400/20 bg-gradient-to-br from-slate-900 to-emerald-950 p-6 text-white shadow-xl">
              <p className="text-sm text-emerald-200">AI Capabilities</p>
              <h2 className="mt-2 text-2xl font-semibold">
                What this assistant can help with
              </h2>

              <div className="mt-5 space-y-3">
                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">Market interpretation</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Understand price trends, renewable share, and growth signals.
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">Project economics</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Explain NPV, NOI, revenue, and payback in plain English.
                  </p>
                </div>

                <div className="rounded-2xl border border-white/10 bg-white/10 p-4">
                  <p className="font-medium">Location tradeoffs</p>
                  <p className="mt-1 text-sm text-slate-200">
                    Compare selected locations with financial assumptions.
                  </p>
                </div>
              </div>
            </section>

            <section className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
              <p className="text-sm text-slate-500">Quick Prompts</p>
              <div className="mt-4 space-y-3">
                {quickPrompts.map((prompt) => (
                  <button
                    key={prompt}
                    onClick={() => handleSuggestionClick(prompt)}
                    className="w-full rounded-2xl border border-slate-200 bg-slate-50 p-4 text-left transition hover:bg-slate-100"
                  >
                    <p className="text-sm font-medium text-slate-800">
                      {prompt}
                    </p>
                  </button>
                ))}
              </div>
            </section>

            <section className="rounded-3xl border border-white/10 bg-white p-6 shadow-xl">
              <p className="text-sm text-slate-500">Current Financial Context</p>
              <div className="mt-4 grid grid-cols-1 gap-4">
                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Annual Revenue</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    {contextSummary.revenue}
                  </p>
                </div>

                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Net Operating Income</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    {contextSummary.noi}
                  </p>
                </div>

                <div className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                  <p className="text-sm text-slate-500">Payback Period</p>
                  <p className="mt-2 text-2xl font-bold text-slate-900">
                    {contextSummary.payback}
                  </p>
                </div>
              </div>
            </section>
          </div>

          <section className="rounded-[28px] border border-white/10 bg-white p-6 shadow-xl md:p-8">
            <div className="mb-6 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
              <div>
                <p className="text-sm text-slate-500">Conversation</p>
                <h2 className="mt-2 text-3xl font-semibold text-slate-900">
                  AI analysis console
                </h2>
                <p className="mt-2 text-sm text-slate-500">
                  Your assistant uses the current dashboard context to answer
                  with more relevant insights.
                </p>
              </div>

              <div className="inline-flex rounded-full border border-cyan-200 bg-cyan-50 px-3 py-1 text-xs text-cyan-700">
                Live AI session
              </div>
            </div>

            <div className="mb-5 h-[560px] overflow-y-auto rounded-3xl border border-slate-200 bg-gradient-to-b from-slate-50 to-white p-4 md:p-5">
              <div className="space-y-4">
                {messages.map((message, index) => {
                  const isTypingAssistant =
                    message.role === "assistant" && typingMessageIndex === index;

                  const shownContent = isTypingAssistant
                    ? displayedAssistantContent
                    : message.content;

                  return (
                    <div key={index}>
                      <div
                        className={`flex ${
                          message.role === "user"
                            ? "justify-end"
                            : "justify-start"
                        }`}
                      >
                        <div
                          className={`max-w-[85%] rounded-2xl p-4 shadow-sm ${
                            message.role === "user"
                              ? "bg-slate-900 text-white"
                              : "border border-slate-200 bg-white text-slate-800"
                          }`}
                        >
                          <p
                            className={`mb-2 text-xs font-semibold uppercase tracking-[0.15em] ${
                              message.role === "user"
                                ? "text-slate-300"
                                : "text-cyan-700"
                            }`}
                          >
                            {message.role === "user" ? "You" : "AI Assistant"}
                          </p>

                          <p className="whitespace-pre-wrap leading-7">
                            {shownContent}
                            {isTypingAssistant && (
                              <span className="ml-1 inline-block h-5 w-2 animate-pulse rounded bg-cyan-500 align-middle" />
                            )}
                          </p>
                        </div>
                      </div>

                      {message.role === "assistant" &&
                        !isTypingAssistant &&
                        message.suggestions &&
                        message.suggestions.length > 0 && (
                          <div className="mt-3 flex justify-start">
                            <div className="max-w-[85%]">
                              <div className="flex flex-wrap gap-2">
                                {message.suggestions.map((suggestion) => (
                                  <button
                                    key={`${index}-${suggestion}`}
                                    onClick={() =>
                                      handleSuggestionClick(suggestion)
                                    }
                                    disabled={loading}
                                    className="rounded-full border border-cyan-200 bg-cyan-50 px-3 py-2 text-sm text-cyan-700 transition hover:bg-cyan-100 disabled:opacity-50"
                                  >
                                    {suggestion}
                                  </button>
                                ))}
                              </div>
                            </div>
                          </div>
                        )}
                    </div>
                  );
                })}

                {loading && typingMessageIndex === null && (
                  <div className="flex justify-start">
                    <div className="max-w-[85%] rounded-2xl border border-slate-200 bg-white p-4 text-slate-800 shadow-sm">
                      <p className="mb-2 text-xs font-semibold uppercase tracking-[0.15em] text-cyan-700">
                        AI Assistant
                      </p>
                      <div className="flex items-center gap-2">
                        <span className="h-2.5 w-2.5 animate-pulse rounded-full bg-cyan-500" />
                        <span className="h-2.5 w-2.5 animate-pulse rounded-full bg-cyan-500 [animation-delay:150ms]" />
                        <span className="h-2.5 w-2.5 animate-pulse rounded-full bg-cyan-500 [animation-delay:300ms]" />
                        <p className="ml-2 text-sm text-slate-500">
                          Analyzing dashboard context...
                        </p>
                      </div>
                    </div>
                  </div>
                )}

                <div ref={messagesEndRef} />
              </div>
            </div>

            <div className="rounded-3xl border border-slate-200 bg-slate-50 p-4">
              <div className="flex flex-col gap-3">
                <textarea
                  ref={textareaRef}
                  rows={1}
                  value={input}
                  onChange={(e) => setInput(e.target.value)}
                  onKeyDown={(e) => {
                    if (e.key === "Enter" && !e.shiftKey) {
                      e.preventDefault();
                      handleSend();
                    }
                  }}
                  placeholder="Ask about market trends, NPV, payback, location tradeoffs, or project returns..."
                  className="max-h-[180px] min-h-[56px] w-full resize-none rounded-2xl border border-slate-200 bg-white p-4 text-slate-900 outline-none transition focus:border-slate-400"
                />

                <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                  <p className="text-xs text-slate-500">
                    Press Enter to send • Shift + Enter for a new line
                  </p>

                  <button
                    onClick={handleSend}
                    disabled={loading || !input.trim()}
                    className="rounded-2xl bg-slate-900 px-6 py-3 font-medium text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-50"
                  >
                    {loading ? "Analyzing..." : "Send to AI"}
                  </button>
                </div>
              </div>
            </div>
          </section>
        </section>
      </div>
    </main>
  );
}