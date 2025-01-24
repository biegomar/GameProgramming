namespace CellularAutomata;

public class Automata<T>
{
    public PlayGround<T> PlayGround { get; init; }
    public IRuleSet<T> RuleSet { get; init; }

    public Automata(PlayGround<T> playGround, IRuleSet<T> ruleSet)
    {
        this.PlayGround = playGround;
        this.RuleSet = ruleSet;
    }

    public void NextGeneration()
    {
        for (var x = 0; x < this.PlayGround.Dimension.X; x++)
        {
            for (var y = 0; y < this.PlayGround.Dimension.Y; y++)
            {
                var position = new Vector(x,y,0);
                this.PlayGround[position] = this.RuleSet.ApplyRules(position);
            }
        }
    }
}