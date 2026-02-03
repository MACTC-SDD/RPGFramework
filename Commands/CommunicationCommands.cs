
using RPGFramework.Interfaces;

namespace RPGFramework.Commands
{
    internal class CommunicationCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                // Add other communication commands here as they are implemented
            ];
        }


    }

    internal class SocialCommand : ICommand
    {
        public string Name => "soc";
        public IEnumerable<string> Aliases => [];
        public string Help => "Usage: <social> [[<target>]]\n" +
            "Do something silly..";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"Not Yet Implemented!");
                return true;
            }
            return false;
        }
    }
}
