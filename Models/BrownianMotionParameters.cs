namespace BrownianMotionGraphic.Models;

public class BrownianMotionParameters
{
    public double InitialPrice { get; set; } = 100.0;
    public double Volatility { get; set; } = 20.0;
    public double Mean { get; set; } = 1.0;
    public int NumberOfDays { get; set; } = 252;
    public int NumberOfSimulations { get; set; } = 1;
}

public class BrownianMotionResult
{
    public List<double[]> Simulations { get; set; } = new();
    public double[] TimeSteps { get; set; } = Array.Empty<double>();
    public BrownianMotionParameters Parameters { get; set; } = new();
}

