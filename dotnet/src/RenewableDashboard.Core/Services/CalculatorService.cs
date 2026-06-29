using RenewableDashboard.Core.DTOs;

namespace RenewableDashboard.Core.Services;

public class CalculatorService
{
    private const int ProjectLifeYears = 20;
    private const double DiscountRate = 0.08;

    public CalculatorResultDto Calculate(CalculatorInputDto input)
    {
        double capacityFactor = input.CapacityFactor;
        double omCost = input.OmCost;
        double electricityRate = input.ElectricityRate;

        if (input.Scenario == "optimistic")
        {
            capacityFactor += 3;
            omCost *= 0.95;
            electricityRate += 0.02;
        }

        double activeRate = input.RateMode switch
        {
            "map" => input.MapElectricityRate ?? electricityRate,
            _ => electricityRate
        };

        var result = ComputeMetrics(input.SystemSizeKw, capacityFactor, input.InstallCostPerW, activeRate, omCost);
        result.ActiveElectricityRate = activeRate;
        result.ActiveCapacityFactor = capacityFactor;
        result.ActiveOmCost = omCost;

        if (input.RateMode == "compare" && input.MapElectricityRate.HasValue)
        {
            var compareResult = ComputeMetrics(input.SystemSizeKw, capacityFactor, input.InstallCostPerW, input.MapElectricityRate.Value, omCost);
            compareResult.ActiveElectricityRate = input.MapElectricityRate.Value;
            compareResult.ActiveCapacityFactor = capacityFactor;
            compareResult.ActiveOmCost = omCost;
            result.CompareResult = compareResult;
        }

        return result;
    }

    private static CalculatorResultDto ComputeMetrics(double systemSizeKw, double capacityFactor, double installCostPerW, double electricityRate, double omCost)
    {
        double totalProjectCost = systemSizeKw * 1000 * installCostPerW;
        double annualEnergyProduction = systemSizeKw * 8760 * (capacityFactor / 100.0);
        double annualRevenue = annualEnergyProduction * electricityRate;
        double netOperatingIncome = annualRevenue - omCost;
        double? paybackPeriod = netOperatingIncome > 0 ? totalProjectCost / netOperatingIncome : null;

        double npv = -totalProjectCost;
        var cashFlows = new List<CashFlowPoint>
        {
            new() { Year = 0, Amount = -totalProjectCost }
        };

        for (int year = 1; year <= ProjectLifeYears; year++)
        {
            double pv = netOperatingIncome / Math.Pow(1 + DiscountRate, year);
            npv += pv;
            cashFlows.Add(new CashFlowPoint { Year = year, Amount = netOperatingIncome });
        }

        return new CalculatorResultDto
        {
            TotalProjectCost = totalProjectCost,
            AnnualEnergyProduction = annualEnergyProduction,
            AnnualRevenue = annualRevenue,
            NetOperatingIncome = netOperatingIncome,
            PaybackPeriod = paybackPeriod,
            Npv = npv,
            CashFlows = cashFlows
        };
    }
}
