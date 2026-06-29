using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

/// <summary>
/// Pure financial calculations for renewable project economics.
/// Ported from the original calculator page logic.
/// </summary>
public class CalculatorService
{
    public const int ProjectLifeYears = 20;
    public const double DiscountRate = 0.08;

    public CalculatorMetrics CalculateMetrics(
        CalculatorInputs inputs,
        double rate,
        double omCost,
        double capacityFactor)
    {
        var totalProjectCost = inputs.SystemSizeKw * 1000 * inputs.InstallCostPerW;
        var annualEnergyProduction = inputs.SystemSizeKw * 8760 * (capacityFactor / 100.0);
        var annualRevenue = annualEnergyProduction * rate;
        var netOperatingIncome = annualRevenue - omCost;

        double? paybackPeriod = netOperatingIncome > 0
            ? totalProjectCost / netOperatingIncome
            : null;

        var npv = -totalProjectCost;
        for (var year = 1; year <= ProjectLifeYears; year++)
        {
            npv += netOperatingIncome / Math.Pow(1 + DiscountRate, year);
        }

        return new CalculatorMetrics
        {
            TotalProjectCost = totalProjectCost,
            AnnualEnergyProduction = annualEnergyProduction,
            AnnualRevenue = annualRevenue,
            NetOperatingIncome = netOperatingIncome,
            PaybackPeriod = paybackPeriod,
            Npv = npv,
        };
    }

    public double CapacityFactorForScenario(CalculatorInputs inputs) =>
        inputs.Scenario == Scenario.Optimistic ? inputs.CapacityFactor + 3 : inputs.CapacityFactor;

    public double OmCostForScenario(CalculatorInputs inputs) =>
        inputs.Scenario == Scenario.Optimistic ? inputs.AnnualOMCost * 0.95 : inputs.AnnualOMCost;

    public double ManualRateForScenario(CalculatorInputs inputs) =>
        inputs.Scenario == Scenario.Optimistic ? inputs.ElectricityRate + 0.02 : inputs.ElectricityRate;

    public double MapRateForScenario(CalculatorInputs inputs, double? mapRate)
    {
        var mapRateBase = mapRate ?? inputs.ElectricityRate;
        return inputs.Scenario == Scenario.Optimistic ? mapRateBase + 0.02 : mapRateBase;
    }

    public CalculatorMetrics ManualMetrics(CalculatorInputs inputs) =>
        CalculateMetrics(
            inputs,
            ManualRateForScenario(inputs),
            OmCostForScenario(inputs),
            CapacityFactorForScenario(inputs));

    public CalculatorMetrics MapMetrics(CalculatorInputs inputs, double? mapRate) =>
        CalculateMetrics(
            inputs,
            MapRateForScenario(inputs, mapRate),
            OmCostForScenario(inputs),
            CapacityFactorForScenario(inputs));

    public List<CashFlowPoint> BuildCashFlow(double totalProjectCost, double netOperatingIncome)
    {
        var data = new List<CashFlowPoint> { new("Year 0", -totalProjectCost) };
        for (var i = 1; i <= ProjectLifeYears; i++)
        {
            data.Add(new CashFlowPoint($"Year {i}", netOperatingIncome));
        }

        return data;
    }
}
