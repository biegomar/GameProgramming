using System.Diagnostics;
using System.Text;

namespace Supporter;

public class StatisticGenerator : IStatisticGenerator
{
    public string Generate(AutomataStatistics statistics)
    {
        var totalSum = statistics.RuleCalculationTimes.Sum() + statistics.RenderingTimes.Sum();

        var totalStats = new StringBuilder();
        totalStats.AppendLine($"{statistics.CurrentGeneration} Generationen auf {statistics.MaxDegreeOfParallelism} Kernen:");
        totalStats.AppendLine($"Gesamtzeit: {FormatTimeFromTicks(statistics.TotalTicks)} s");
        totalStats.AppendLine($"Gesamtzeit der Einzelmessungen: {FormatTimeFromTicks(totalSum)} s");
        totalStats.AppendLine($"Differenz zur Gesamtzeit: {FormatTimeFromTicks(Math.Abs(statistics.TotalTicks - totalSum))} s");
        totalStats.AppendLine("");

        var ruleCalculationStats = CalculateStatistics(statistics.RuleCalculationTimes, "Regelberechnung", statistics.RuleCounter);
        var renderingStats = CalculateStatistics(statistics.RenderingTimes, "Rendering");

        totalStats.AppendLine(ruleCalculationStats);
        totalStats.AppendLine(renderingStats);

        return totalStats.ToString();
    }
    
    private string CalculateStatistics(IList<long> times, string type, IDictionary<string, uint>? ruleCounter = null)
    {
        var statistics = new StringBuilder();
        
        var total = times.Sum();                
        var min = times.Min();                  
        var max = times.Max();                  
        var average = times.Average();        

        var totalFormatted = FormatTimeFromTicks(total);
        var minFormatted = FormatTimeInMicroseconds(min);
        var maxFormatted = FormatTimeInMicroseconds(max);
        var averageFormatted = FormatTimeInMicroseconds((long)average);
        
        statistics.AppendLine($"{type}-Statistik:");
        statistics.AppendLine($"- Gesamtzeit: {totalFormatted} s");
        statistics.AppendLine($"- Langsamste: {maxFormatted} µs");
        statistics.AppendLine($"- Schnellste: {minFormatted} µs");
        statistics.AppendLine($"- Durchschnitt: {averageFormatted} µs");

        AppendRuleCountsToStatistics(ruleCounter, statistics);
        

        times.Clear();
        
        return statistics.ToString();
    }

    private static void AppendRuleCountsToStatistics(IDictionary<string, uint>? ruleCounter, StringBuilder statistics)
    {
        if (ruleCounter != null && ruleCounter.Any())
        {
            statistics.AppendLine("");
            foreach (var ruleCount in ruleCounter)
            {
                if (ruleCount.Value != 0)
                {
                    statistics.AppendLine($"- {ruleCount.Key}: {ruleCount.Value}");   
                }
            }   
        }
    }

    private string FormatTimeFromTicks(long ticks)
    {
        try
        {
            var timespan = TimeSpan.FromTicks(ticks);
            var totalMicroseconds = ticks * (1000000.0 / TimeSpan.TicksPerSecond);
            var microseconds = (int)(totalMicroseconds % 1000); 
        
            return $"{timespan.Seconds}.{timespan.Milliseconds:D3}{microseconds:D3}";
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return string.Empty;
        }
    }
    
    private string FormatTimeInMicroseconds(long ticks)
    {
        try
        {
            var totalMicroseconds = ticks * (1000000.0 / TimeSpan.TicksPerSecond);
            return $"{(int)totalMicroseconds:D3}";
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return string.Empty;
        }
    }
}