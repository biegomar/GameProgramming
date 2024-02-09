namespace MonsterAttack
{
    /// <summary>
    /// The impossible fight exception.
    /// Is thrown, if the fight between two monsters is not possible or allowed.
    /// </summary>
    internal class ImpossibleFightException : Exception
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="message"></param>
        internal ImpossibleFightException(string message) : base(message)
        {
        }
    }
}
