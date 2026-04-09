export async function GET() {
  const apiKey = process.env.EIA_API_KEY;

  try {
    const res = await fetch(
`https://api.eia.gov/v2/electricity/retail-sales/data/?api_key=${apiKey}&data[0]=price&frequency=monthly&facets[stateid][]=US&facets[sectorid][]=ALL&length=12&sort[0][column]=period&sort[0][direction]=desc`    );

    const json = await res.json();

    const rows = json?.response?.data ?? [];

    const latest = rows[0];

    const trend = [...rows]
  .reverse()
  .map((item: any) => ({
    month: String(item.period),
    price: Number(item.price ?? 0) / 100,
  }));

    return Response.json({
      electricityPrice: Number(latest?.price ?? 0) / 100,
      capacityGrowth: 8.5,
      renewableShare: 32,
      trend,
    });
  } catch (error) {
    console.error("MARKET API ERROR:", error);

    return Response.json({
      electricityPrice: 0,
      capacityGrowth: 0,
      renewableShare: 0,
      trend: [],
    });
  }
}