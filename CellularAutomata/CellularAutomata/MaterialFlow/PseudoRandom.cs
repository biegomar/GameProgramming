using System.Runtime.CompilerServices;

namespace CellularAutomata.MaterialFlow;

public class PseudoRandom
{
    private uint x = 123456789;
    private uint y = 362436069;
    private uint z = 521288629;
    private uint w = 88675123;

    public PseudoRandom(uint seed)
    {
        x ^= seed;
        y ^= seed << 13;
        z ^= seed >> 9;
        w ^= seed << 7;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next(uint max)
    {
        var t = x ^ (x << 11);
        x = y; y = z; z = w;
        w = w ^ (w >> 19) ^ t ^ (t >> 8);
        return w % max;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Chance(int percent) => Next(100) < percent;
}