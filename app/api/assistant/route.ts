import OpenAI from "openai";

const client = new OpenAI({
  apiKey: process.env.OPENAI_API_KEY,
});

export async function POST(req: Request) {
  try {
    let body = {};

try {
  body = await req.json();
} catch (e) {
  body = {};
}

    const question = body.question || "";
    const context = body.context || {};

    const location = context.selectedLocation;
    const calculator = context.calculatorSnapshot;

    let contextSummary = "";

    if (location) {
      contextSummary += `
Selected Location:
- Name: ${location.name}
- Electricity Rate: $${location.electricityRate}/kWh
- Solar Score: ${location.solarScore}/10
- Note: ${location.note}
`;
    } else {
      contextSummary += `
Selected Location:
- None
`;
    }

    if (calculator) {
      contextSummary += `
Calculator Snapshot:
- Scenario: ${calculator.scenario}
- Rate Mode: ${calculator.rateMode}
- Manual Electricity Rate: $${calculator.manualElectricityRate}/kWh
- Active Electricity Rate: $${calculator.activeElectricityRate}/kWh
- Total Project Cost: $${calculator.totalProjectCost}
- Annual Revenue: $${calculator.annualRevenue}
- Net Operating Income: $${calculator.netOperatingIncome}
- Payback Period: ${
        calculator.paybackPeriod
          ? `${calculator.paybackPeriod.toFixed(1)} years`
          : "Not achievable"
      }
- NPV: $${calculator.npv}
`;
    } else {
      contextSummary += `
Calculator Snapshot:
- None
`;
    }

    const prompt = `
You are a renewable energy investment analyst AI embedded inside a dashboard.

STRICT RULES:
- You MUST prioritize the provided dashboard context over general knowledge.
- If location or calculator data exists, you MUST use it in your answer.
- DO NOT give generic answers when context is available.
- If no context is available, then use general knowledge.
- Always reference numbers from the dashboard when present.

Response format:
1. Key Insight
2. Financial Impact
3. Recommendation
4. Sources used

Dashboard Context:
${contextSummary}

User Question:
${question}
`;

    const response = await client.responses.create({
      model: "gpt-4o-mini",
      input: prompt,
    });

    const answer =
      response.output_text ||
      "I could not generate a response at the moment.";

    return Response.json({ answer });
  } catch (error) {
    console.error("ASSISTANT API ERROR:", error);

    return Response.json(
      {
        error: "Failed to process assistant request",
        answer:
          "Something went wrong while generating the AI response. Please try again.",
      },
      { status: 500 }
    );
  }
}