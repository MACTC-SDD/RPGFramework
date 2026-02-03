
using RPGFramework.Interfaces;

namespace RPGFramework.Commands
{
    /// <summary>
    /// Provides access to the set of built-in core command implementations.
    /// </summary>
    /// <remarks>The <c>CoreCommands</c> class exposes static methods for retrieving all available core
    /// commands. These commands represent fundamental operations supported by the system </remarks>
    internal class CoreCommands
    {
        public static List<ICommand> GetAllCommands()
        {
            return
            [
                new AFKCommand(),
                new IpCommand(),
                new LookCommand(),
                new QuitCommand(),
                new SayCommand(),
                new TimeCommand(),
                // Add other core commands here as they are implemented
            ];
        }
    }

    #region AFKCommand Class
    internal class AFKCommand : ICommand
    {
        public string Name => "afk";
        public IEnumerable<string> Aliases => [];
        public string Help => "Usage: afk\n" +
            "Toggles your Away From Keyboard (AFK) status.";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.IsAFK = !player.IsAFK;
                player.WriteLine($"You are now {(player.IsAFK ? "AFK" : "no longer AFK")}.");
                return true;
            }
            return false;
        }
    }
    #endregion

    #region IpCommand Class
    internal class IpCommand : ICommand
    {
        public string Name => "ip";
        public IEnumerable<string> Aliases => [];
        public string Help => "Usage: ip\n" +
            "Displays your current IP address.";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"Your IP address is {player.GetIPAddress()}");
                return true;
            }
            return false;
        }
    }
    #endregion

    #region LookCommand Class
    internal class LookCommand : ICommand
    {
        public string Name => "look";
        public IEnumerable<string> Aliases => [ "l" ];
        public string Help => "Usage: look\n" +
            "Displays the description of your current location and its exits.";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                // For now, we'll ignore the command and just show the room description
                player.WriteLine($"{player.GetRoom().Description}");
                player.WriteLine("Exits:");
                foreach (var exit in player.GetRoom().GetExits())
                {
                    player.WriteLine($"{exit.Description} to the {exit.ExitDirection}");
                }
                return true;
            }
            return false;
        }
    }
    #endregion

    #region QuitCommand Class
    internal class QuitCommand : ICommand
    {
        public string Name => "quit";
        public IEnumerable<string> Aliases => [ "exit" ];
        public string Help => "Usage: quit\n" +
            "Logs you out of the game.";

        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.Logout();
                return true;
            }
            return false;
        }
    }
    #endregion

    #region SayCommand Class
    internal class SayCommand : ICommand
    {
        public string Name => "say";
        public IEnumerable<string> Aliases => ["\"".Normalize(), "'".Normalize()];
        public string Help => "Usage: say <message>\n" +
            "Sends a message to all characters in the same room.";
        public bool Execute(Character character, List<string> parameters)
        {
            // If no message and it's a player, tell them to say something
            if (parameters.Count < 2 && character is Player player)
            {
                player.WriteLine("Say what?");
                return true;
            }
            Comm.RoomSay(character.GetRoom(), parameters[1], character);
            return true;
        }
    }
    #endregion

    #region TimeCommand Class
    internal class TimeCommand : ICommand
    {
        public string Name => "time";
        public IEnumerable<string> Aliases => [];
        public string Help => "Usage: time\n" +
            "Displays the current in-game time.";
        public bool Execute(Character character, List<string> parameters)
        {
            if (character is Player player)
            {
                player.WriteLine($"The time is {GameState.Instance.GameDate:t}");
                return true;
            }
            return false;
        }
    }
    #endregion

}
