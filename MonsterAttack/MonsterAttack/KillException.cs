namespace MonsterAttack
{
    /// <summary>
    /// The kill exception.
    /// Is thrown, if a monster dies.
    /// </summary>
    internal class KillException : Exception
    {
        internal KillException(string message) : base(message)
        {
        }
    }
}
