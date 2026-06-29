using RenewableDashboard.Models;

namespace RenewableDashboard.Services;

public sealed class CalculatorService
{
    private const int ProjectLifeYears = 20;
    private const double DiscountRate = 0.08;

    public CalculatorMetrics Calculate(CalculatorInputs inputs, decimal rate, decimal operationsMaintenanceCost, double capacityFactor)
    {
        var totalProjectCost = (decimal)inputs.SystemSizeKw * 1000m * inputs.InstallCostPerW;
        var annualEnergyProduction = inputs.SystemSizeKw * 8760d * (capacityFactor / 100d);
        var annualRevenue = (decimal)annualEnergyProduction * rate;
        var netOperatingIncome = annualRevenue - operationsMaintenanceCost;
        var paybackPeriod = netOperatingIncome > 0
            ? (double)(totalProjectCost / netOperatingIncome)
            : (double?)null;

        var netPresentValue = -totalProjectCost;

        for (var year = 1; year <= ProjectLifeYears; year++)
        {
            netPresentValue += netOperatingIncome / (decimal)Math.Pow(1 + DiscountRate, year);
        }

        return new CalculatorMetrics(
            totalProjectCost,
            annualEnergyProduction,
            annualRevenue,
            netOperatingIncome,
            paybackPeriod,
            netPresentValue);
    }

    public CalculatorSnapshot BuildSnapshot(CalculatorInputs inputs, Location? selectedLocation)
    {
        var capacityFactor = inputs.Scenario == "optimistic"
            ? inputs.CapacityFactor + 3
            : inputs.CapacityFactor;

        var operationsMaintenanceCost = inputs.Scenario == "optimistic"
            ? inputs.AnnualOperationsMaintenanceCost * 0.95m
            : inputs.AnnualOperationsMaintenanceCost;

        var manualRate = inputs.Scenario == "optimistic"
            ? inputs.ElectricityRate + 0.02m
            : inputs.ElectricityRate;

        var mapRateBase = selectedLocation?.ElectricityRate ?? inputs.ElectricityRate;
        var mapRate = inputs.Scenario == "optimistic"
            ? mapRateBase + 0.02m
            : mapRateBase;

        var activeRate = inputs.RateMode == "map" ? mapRate : manualRate;
        var metrics = Calculate(inputs, activeRate, operationsMaintenanceCost, capacityFactor);

        return new CalculatorSnapshot(
            inputs.Scenario,
            inputs.RateMode,
            inputs.ElectricityRate,
            activeRate,
            metrics.TotalProjectCost,
            metrics.AnnualRevenue,
            metrics.NetOperatingIncome,
            metrics.PaybackPeriod,
            metrics.NetPresentValue);
    }
}
