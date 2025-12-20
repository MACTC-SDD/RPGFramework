using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace RPGFramework.Display
{
    public class RPGPanel
    {
        public static BoxBorder Border { get; private set; } = BoxBorder.Rounded;
        public static string HeaderColor { get; private set; } = "[bold yellow]";

        /// <summary>
        /// Get a Spectre Panel object using our default settings
        /// </summary>
        /// <param name="content"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static Panel GetPanel(string content, string header)
        {
            return new Panel(content)
            {
                Header = new PanelHeader($"{HeaderColor}Welcome[/]"),
                Border = Border
            };
        }
    }
}
