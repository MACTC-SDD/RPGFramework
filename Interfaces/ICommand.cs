
namespace RPGFramework.Interfaces
{
    internal interface ICommand
    {
        /// <summary>
        /// Primary name for this command (used as an identifier).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// All the words that should trigger this command (e.g. "look", "l").
        /// </summary>
        IEnumerable<string> Aliases { get; }

        /// <summary>
        /// Gets the help text or usage information associated with this instance.
        /// This is used to generate help entries as well as display help when using
        /// the command.
        /// </summary>
        public string Help { get; }

        /// <summary>
        /// Execute the command.
        /// Returns true if the command was successfully handled.
        /// </summary>
        /// <param name="character">The character issuing the command.</param>
        /// <param name="parameters">
        /// The parsed parameters, including the command word at index 0.
        /// </param>
        bool Execute(Character character, List<string> parameters);
    }
}
