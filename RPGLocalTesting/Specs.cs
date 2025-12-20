using Spectre.Console;
using Spectre.Console.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLocalTesting
{
    internal partial class Program
    {
        public static void DemoBreakdownChart()
        {
            AnsiConsole.Write(
                new BreakdownChart()
                    .Width(60)
                    .AddItem("SCSS", 80, Color.Red)
                    .AddItem("HTML", 28.3, Color.Blue)
                    .AddItem("C#", 22.6, Color.Green)
                    .AddItem("JavaScript", 6, Color.Yellow));
        }

        public static void DemoCalendar()
        {
            DateTime today = DateTime.Today;

            Calendar calendar = new Calendar(today.Year, today.Month);
            calendar.AddCalendarEvent(today);  // highlight today

            AnsiConsole.Write(calendar);
        }

        public static void DemoCanvas()
        {
            Canvas canvas = new Canvas(16, 16);

            for (int x = 0; x < canvas.Width; x++)
            {
                // Cross
                canvas.SetPixel(x, x, Color.White);
                canvas.SetPixel(canvas.Width - x - 1, x, Color.White);

                // Border
                canvas.SetPixel(x, 0, Color.Red);
                canvas.SetPixel(0, x, Color.Green);
                canvas.SetPixel(x, canvas.Height - 1, Color.Blue);
                canvas.SetPixel(canvas.Width - 1, x, Color.Yellow);
            }

            AnsiConsole.Write(canvas);
        }

        public static async Task DemoLiveDisplayAsync()
        {
            Table table = new Table().Centered();
            table.AddColumn("Tick");
            table.AddColumn("Message");

            await AnsiConsole.Live(table)
                .StartAsync(async context =>
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        table.Rows.Clear();
                        table.AddRow(i.ToString(), $"Update at {DateTime.Now:T}");
                        context.Refresh();
                        await Task.Delay(500);
                    }
                });
        }


        public static async Task DemoLiveUiAsync()
        {
            Table table = new Table().AddColumn("Tick").AddColumn("Status");

            await AnsiConsole.Live(table)
                .StartAsync(async context =>
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        table.Rows.Clear();
                        table.AddRow(i.ToString(), i % 2 == 0 ? "Even" : "Odd");
                        context.Refresh();
                        await Task.Delay(500);
                    }
                });
        }

        public static void DemoPanel()
        {
            // Panel
            var panel = new Panel("This is the content\nThat goes in the panel.")
            {
                Header = new PanelHeader($"[bold yellow]Yellow Header[/]"),
                Border = BoxBorder.Rounded
            };
            AnsiConsole.Write(panel);
        }

        public static async Task DemoProgress()
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask("Loading areas");
                    while (!task.IsFinished)
                    {
                        task.Increment(5);
                        Thread.Sleep(80);
                    }
                });
        }

        public static async Task DemoSpinnerAsync()
        {
            // Simulated work wrapped with a spinner
            await Task.Delay(1500)
                .Spinner(
                    Spinner.Known.Dots,
                    new Style(foreground: Color.Green));

            AnsiConsole.MarkupLine("[green]Work complete![/]");
        }

        public static async Task DemoStatusAsync()
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green"))
                .StartAsync("Initializing...", async ctx =>
                {
                    AnsiConsole.MarkupLine("Connecting to server...");
                    await Task.Delay(1000);

                    ctx.Status("Downloading data...");
                    await Task.Delay(1000);

                    ctx.Status("Finishing up...");
                    await Task.Delay(1000);

                    AnsiConsole.MarkupLine("[green]All done![/]");
                });
        }


        public static void DemoTable()
        {
            var table = new Table();
            table.AddColumn("Direction");
            table.AddColumn("Description");
            table.AddRow("North", "A wooden door");
            table.AddRow("West", "A narrow hallway");

            AnsiConsole.Write(table);
        }

        public static void DemoTree()
        {
            var tree = new Tree("A world map");
            var area = tree.AddNode("[yellow]Starting Area[/]");
            area.AddNode("Room 0: The void");
            area.AddNode("Room 1: Entrance hall");
            AnsiConsole.Write(tree);
        }

        public static void DemoBarChart()
        {
            AnsiConsole.Write(
                new BarChart()
                    .Width(60)
                    .Label("[green bold underline]Damage by Skill[/]")
                    .CenterLabel()
                    .AddItem("Slash", 12, Color.Yellow)
                    .AddItem("Fireball", 25, Color.Red)
                    .AddItem("Heal", 8, Color.Green));
        }

    }
}
