namespace EscapeRoom
{
    /// <summary>
    /// This exception will be fired if the player quits the game.
    /// </summary>
    internal class QuitException : Exception
    {
        internal QuitException(string message) : base(message)
        {
        }
    }
}
