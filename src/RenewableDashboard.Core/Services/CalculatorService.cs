using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Services;

public class CalculatorService
{
    private const int ProjectLifeYears = 20;
    private const decimal DiscountRate = 0.08m;

    public CalculatorMetrics CalculateMetrics(
        decimal systemSizeKw,
        decimal capacityFactor,
        decimal installCostPerW,
        decimal rate,
        decimal omCost)
    {
        var totalProjectCost = systemSizeKw * 1000 * installCostPerW;
        var annualEnergyProduction = systemSizeKw * 8760 * (capacityFactor / 100);
        var annualRevenue = annualEnergyProduction * rate;
        var netOperatingIncome = annualRevenue - omCost;
        decimal? paybackPeriod = netOperatingIncome > 0
            ? totalProjectCost / netOperatingIncome
            : null;

        var npv = -totalProjectCost;
        for (var year = 1; year <= ProjectLifeYears; year++)
        {
            npv += netOperatingIncome / (decimal)Math.Pow((double)(1 + DiscountRate), year);
        }

        return new CalculatorMetrics
        {
            TotalProjectCost = totalProjectCost,
            AnnualEnergyProduction = annualEnergyProduction,
            AnnualRevenue = annualRevenue,
            NetOperatingIncome = netOperatingIncome,
            PaybackPeriod = paybackPeriod,
            Npv = npv,
            ActiveElectricityRate = rate,
            OmCostForScenario = omCost
        };
    }

    public (decimal CapacityFactor, decimal OmCost, decimal Rate) ApplyScenario(
        string scenario,
        decimal capacityFactor,
        decimal annualOMCost,
        decimal electricityRate)
    {
        if (scenario == "optimistic")
        {
            return (capacityFactor + 3, annualOMCost * 0.95m, electricityRate + 0.02m);
        }

        return (capacityFactor, annualOMCost, electricityRate);
    }

    public List<CashFlowPoint> BuildCashFlowData(decimal totalProjectCost, decimal netOperatingIncome)
    {
        var data = new List<CashFlowPoint>
        {
            new() { Year = "Year 0", CashFlow = -totalProjectCost }
        };

        for (var i = 1; i <= ProjectLifeYears; i++)
        {
            data.Add(new CashFlowPoint { Year = $"Year {i}", CashFlow = netOperatingIncome });
        }

        return data;
    }
}
