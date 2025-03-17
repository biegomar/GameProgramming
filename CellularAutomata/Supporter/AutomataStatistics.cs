namespace Supporter;

public record AutomataStatistics(
    long TotalTicks, 
    int CurrentGeneration, 
    int MaxDegreeOfParallelism, 
    IList<long> RenderingTimes, 
    IList<long> RuleCalculationTimes, 
    IDictionary<string, uint>? RuleCounter = null);