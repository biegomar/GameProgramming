namespace EscapeRoom
{
    /// <summary>
    /// This exception will be fired if the player wins the game.
    /// </summary>
    internal class WinException : Exception
    {
        internal WinException(string message) : base(message)
        {
        }
    }
}
