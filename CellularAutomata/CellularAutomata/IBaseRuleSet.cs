namespace CellularAutomata;

public interface IBaseRuleSet
{
    IDictionary<string, uint> RuleCounter { get; init; }
}