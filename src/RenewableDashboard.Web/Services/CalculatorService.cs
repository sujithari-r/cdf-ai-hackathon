using RenewableDashboard.Web.Models;

namespace RenewableDashboard.Web.Services;

public class CalculatorService
{
    private const int ProjectLifeYears = 20;
    private const decimal DiscountRate = 0.08m;

    public CalculatorResult Calculate(CalculatorInput input, decimal? mapRate)
    {
        var scenario = input.Scenario.ToLowerInvariant();
        var rateMode = input.RateMode.ToLowerInvariant();

        var capacityFactor = scenario == "optimistic"
            ? input.CapacityFactorPercent + 3
            : input.CapacityFactorPercent;

        var annualOmCost = scenario == "optimistic"
            ? input.AnnualOmCost * 0.95m
            : input.AnnualOmCost;

        var manualRate = scenario == "optimistic"
            ? input.ElectricityRate + 0.02m
            : input.ElectricityRate;

        var mapRateBase = mapRate ?? input.ElectricityRate;
        var mapRateScenario = scenario == "optimistic"
            ? mapRateBase + 0.02m
            : mapRateBase;

        var activeRate = rateMode == "map" ? mapRateScenario : manualRate;

        var totalProjectCost = input.SystemSizeKw * 1000m * input.InstallCostPerWatt;
        var annualEnergy = input.SystemSizeKw * 8760m * (capacityFactor / 100m);
        var annualRevenue = annualEnergy * activeRate;
        var noi = annualRevenue - annualOmCost;

        decimal? paybackPeriod = noi > 0 ? totalProjectCost / noi : null;

        var npv = -totalProjectCost;
        for (var year = 1; year <= ProjectLifeYears; year++)
        {
            var discountFactor = (decimal)Math.Pow((double)(1 + DiscountRate), year);
            npv += noi / discountFactor;
        }

        var cashFlow = new List<CashFlowPoint>
        {
            new() { Year = 0, Value = -totalProjectCost }
        };
        for (var year = 1; year <= ProjectLifeYears; year++)
        {
            cashFlow.Add(new CashFlowPoint { Year = year, Value = noi });
        }

        return new CalculatorResult
        {
            ActiveElectricityRate = activeRate,
            TotalProjectCost = totalProjectCost,
            AnnualEnergyProduction = annualEnergy,
            AnnualRevenue = annualRevenue,
            NetOperatingIncome = noi,
            PaybackPeriodYears = paybackPeriod,
            Npv = npv,
            CashFlow = cashFlow
        };
    }
}
