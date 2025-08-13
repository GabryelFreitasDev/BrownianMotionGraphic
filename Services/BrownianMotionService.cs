using BrownianMotionGraphic.Models;

namespace BrownianMotionGraphic.Services;

public class BrownianMotionService : IBrownianMotionService
{
    private readonly Random _random = new();

    public BrownianMotionResult GenerateBrownianMotion(BrownianMotionParameters parameters)
    {
        var result = new BrownianMotionResult
        {
            Parameters = parameters
        };

        result.TimeSteps = new double[parameters.NumberOfDays + 1];
        for (int i = 0; i <= parameters.NumberOfDays; i++)
        {
            result.TimeSteps[i] = i; 
        }

        for (int sim = 0; sim < parameters.NumberOfSimulations; sim++)
        {
            var prices = GenerateSingleSimulation(parameters);
            result.Simulations.Add(prices);
        }

        return result;
    }

    private double[] GenerateSingleSimulation(BrownianMotionParameters parameters)
    {
        // Implementação baseada no código fornecido
        double sigma = parameters.Volatility;
        double mean = parameters.Mean;
        double initialPrice = parameters.InitialPrice;
        int numDays = parameters.NumberOfDays;

        double[] prices = new double[numDays + 1];
        prices[0] = initialPrice;

        for (int i = 1; i <= numDays; i++)
        {
            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            double returnDiario = mean + sigma * z;

            prices[i] = prices[i - 1] * Math.Exp(returnDiario);
        }

        return prices;
    }
}

