namespace EscapeRoom
{
    /// <summary>
    /// This exception will be fired if the player quits the game.
    /// </summary>
    internal class QuitException : Exception
    {
        /// <summary>
        /// ctor. 
        /// </summary>
        /// <param name="message"></param>
        internal QuitException(string message) : base(message)
        {
        }
    }
}
