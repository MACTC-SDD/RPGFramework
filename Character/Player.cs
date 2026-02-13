using System.Text.Json.Serialization;
using Spectre.Console;
using Spectre.Console.Rendering;

using RPGFramework.Enums;

namespace RPGFramework
{
    internal partial class Player : Character
    {
        #region --- Properties --- 
        // Properties to NOT save (don't serialize)
        [JsonIgnore]
        public bool IsAFK { get; set; } = false;

        [JsonIgnore]
        public bool IsOnline { get; set; }
        
        // Properties
        public DateTime LastLogin { get; set; }
        public int MapRadius { get; set; } = 2; // How far the player can see on the map
        public string Password { get; private set; } = "SomeGarbage";
        public TimeSpan PlayTime { get; set; } = new TimeSpan();
        public PlayerRole PlayerRole { get; set; }
        #endregion

        #region DisplayName Method
        public string DisplayName()
        {
            // We could add colors and other things later, for now, just afk
            return Name + (IsAFK ? " (AFK)" : "");

        }
        #endregion

        #region Exists Method (Static)
        /// <summary>
        /// Checks if a player with the specified name exists in the provided dictionary. This is case-insensitive!
        /// That is why we don't just use players.ContainsKey.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public static bool Exists(string playerName, Dictionary<string, Player> players)
        {
            // Check dictionary keys in a case-insensitive manner
            return players.Keys.Any(name => string.Equals(name, playerName, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Login/Logout Methods
        /// <summary>
        /// Things that should happen when a player logs in.
        /// </summary>
        public void Login()
        {
            IsOnline = true;
            LastLogin = DateTime.Now; 
            Console = CreateAnsiConsole();
        }

        /// <summary>
        /// Things that should happen when a player logs out. 
        /// </summary>
        public void Logout()
        {
            if (!IsOnline)
                return; // Player is already logged out, so do nothing

            TimeSpan duration = DateTime.Now - LastLogin;
            PlayTime += duration;
            IsOnline = false;            
            Save();


            WriteLine("Bye!");
            Network?.Client.Close();
        }
        #endregion

        #region Save Method
        /// <summary>
        /// Save the player to the database.
        /// </summary>
        public void Save()
        {
            GameState.Instance.SavePlayer(this);
        }
        #endregion

        #region SetPassword Method
        /// <summary>
        /// Sets the password to the specified value.
        /// </summary>
        /// <param name="newPassword">The new password to assign. Cannot be null.</param>
        /// <returns>true if the password was set successfully; otherwise, false.</returns>
        public bool SetPassword(string newPassword)
        {
            // TODO: Consider adding password complexity checking
            Password = newPassword;
            return true;
        }
        #endregion

        #region TryFindPlayer Method (Static)
        /// <summary>
        /// Attempts to find a player with the specified name in the provided collection.
        /// </summary>
        /// <param name="playerName">The name of the player to locate. Cannot be null.</param>
        /// <param name="players">A dictionary containing player names as keys and corresponding <see cref="Player"/> objects as values.
        /// Cannot be null.</param>
        /// <param name="player">When this method returns, contains the <see cref="Player"/> object associated with the specified name, if
        /// found;</param>
        /// <returns><see langword="true"/> if a player with the specified name is found; otherwise, <see langword="false"/>.</returns>
        public static bool TryFindPlayer(string playerName, Dictionary<string, Player> players, out Player? player)
        {
            player = players.Values.Where(o => string.Equals(o.Name, playerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            return player != null;
        }
        #endregion

        #region Write/WriteLine Methods
        public void Write(string message)
        {
            try
            {
                WriteNewLineIfNeeded();
                Console?.Write(message);
                var line = Network?.TelnetConnection?.CurrentLineText;
                Console?.Write(line ?? String.Empty); // Re-write current input line
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                GameState.Log(Enums.DebugLevel.Error, $"Error sending message to player {Name}: {ex.Message}");
                Logout(); // Log the player out if we can't send messages to them, as this likely means their connection is lost
            }
        }

        public void Write(IRenderable renderable)
        {
            try
            {
                WriteNewLineIfNeeded();
                Console?.Write(renderable);
                var line = Network?.TelnetConnection?.CurrentLineText;
                Console?.Write(line ?? String.Empty); // Re-write current input line
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                GameState.Log(Enums.DebugLevel.Error, $"Error sending message to player {Name}: {ex.Message}");
                Logout(); // Log the player out if we can't send messages to them, as this likely means their connection is lost
            }
        }

        
        /// <summary>
        /// Writes the specified message to the output, followed by a line terminator.
        /// </summary>
        /// <param name="message">The message to write. This value can include marku
        /// p formatting supported by the output system.</param>
        public void WriteLine(string message)
        {
            try
            {
                WriteNewLineIfNeeded();
                Console?.MarkupLine(message);
                var line = Network?.TelnetConnection?.CurrentLineText;
                Console?.Write(line ?? String.Empty); // Re-write current input line
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                GameState.Log(Enums.DebugLevel.Error, $"Error sending message to player {Name}: {ex.Message}");
                Logout(); // Log the player out if we can't send messages to them, as this likely means their connection is lost
            }
        }

        private void WriteNewLineIfNeeded()
        {
            try
            {
                if (Network == null)
                    return;
                if (Network.TelnetConnection == null)
                    return;
                if (Network.NeedsOutputNewline)
                {
                    Console?.Write("\r\n");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                GameState.Log(Enums.DebugLevel.Error, $"Error sending message to player {Name}: {ex.Message}");
                Logout(); // Log the player out if we can't send messages to them, as this likely means their connection is lost
            }
        }
        #endregion
    }

}
