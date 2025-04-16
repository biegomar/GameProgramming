using System.Runtime.CompilerServices;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.GameOfLive;

public sealed class GameOfLifeRuleSet(Vector dimension) 
{
    private readonly Random random = new ();
    
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
    
    public bool ApplyRules(SimplePlayGround playGround, Vector position)
    {
        var isAlive = playGround.GetState(position);
        
        var liveNeighbors = CountLivingNeighbors(playGround, position);
        
        return liveNeighbors == 3 || (isAlive && liveNeighbors == 2);
    }

    public SimplePlayGround ApplySpawnRules(SimplePlayGround playGround, Vector spawnPosition, Vector brushSize, double probability)
    {
        var startX = spawnPosition.X;
        var endX = spawnPosition.X + brushSize.X - 1;
        
        var startY = spawnPosition.Y;
        var endY = spawnPosition.Y + brushSize.Y - 1;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var newPos = new Vector(x, y);
                if (IsWithinBounds(newPos) && !playGround.GetState(newPos))
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround.SetState(newPos, true);
                    }
                }
            }    
        }
        
        return playGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountLivingNeighbors(SimplePlayGround playGround, Vector position)
    {
        int liveNeighbors = 0;
    
        foreach (var (dx, dy) in NeighborOffsets)
        {
            var neighbor = new Vector(position.X + dx, position.Y + dy);
    
            if (IsWithinBounds(neighbor) && playGround.GetState(neighbor))
            {
                liveNeighbors++;
                if (liveNeighbors == 4)
                    break;
            }
        }
    
        return liveNeighbors;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector neighbor)
    {
        return (uint)neighbor.X < (uint)dimension.X && (uint)neighbor.Y < (uint)dimension.Y;
    }
}