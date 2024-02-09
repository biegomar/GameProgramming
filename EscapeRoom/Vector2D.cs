namespace EscapeRoom
{
    /// <summary>
    /// 2D coordinate for playground dimension, player position, ...
    /// records are .net new way of modelling shiny little data classes.
    /// </summary>
    internal sealed record Vector2D(int X, int Y);
}
