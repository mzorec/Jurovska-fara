using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;
using Vortex.ConsoleApplication;

namespace LoyaltyProgram.Console
{
    public class ConsoleApp
    {
          public ConsoleApp(string[] args, IEnumerable<IConsoleCommand> commands)
        {
            this.args = args;

            foreach (IConsoleCommand command in commands)
            {
                AddCommand(command);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public int Process()
        {
            ShowBanner();

            try
            {
                if (args.Length == 0)
                {
                    ShowHelp();
                    return 0;
                }

                string commandName = args[0];

                if (commands.ContainsKey(commandName))
                {
                    IConsoleCommand command = commands[commandName];

                    List<string> remainingArgs = new List<string>(args);
                    remainingArgs.RemoveAt(0);

                    return command.Execute(remainingArgs);
                }

                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "Unknown command: '{0}'", commandName));
            }
            catch (OptionException ex)
            {
                System.Console.Error.WriteLine("ERROR: {0}", ex);
            }
            catch (ArgumentException ex)
            {
                System.Console.Error.WriteLine("ERROR: {0}", ex);
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("ERROR: {0}", ex);
            }

            return 1;
        }

        private void AddCommand(IConsoleCommand command)
        {
            commands.Add(command.CommandName, command);
        }

        private static void ShowBanner()
        {
            System.Console.Out.WriteLine();

            FileVersionInfo version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            System.Console.Out.Write("Loyalty.Console v{0}", version.FileVersion);
            System.Console.Out.WriteLine();
            System.Console.Out.WriteLine();
        }

        private void ShowHelp()
        {
            System.Console.Out.WriteLine("USAGE: Loyalty.Console <command> <options>");
            System.Console.Out.WriteLine("----------------------------------");
            System.Console.Out.WriteLine("LIST OF COMMANDS:");
            System.Console.Out.WriteLine();

            foreach (IConsoleCommand command in commands.Values)
            {
                System.Console.Out.Write("COMMAND '{0}': {1}", command.CommandName, command.CommandDescription);
                System.Console.Out.WriteLine();
                System.Console.Out.WriteLine();
                System.Console.Out.Write("OPTIONS for '{0}':", command.CommandName);
                System.Console.Out.WriteLine();
                System.Console.Out.WriteLine();
                command.ShowHelp();
                System.Console.Out.WriteLine();
                System.Console.Out.WriteLine("----------------------------------");
            }
        }

        private readonly string[] args;

        private readonly SortedDictionary<string, IConsoleCommand> commands =
            new SortedDictionary<string, IConsoleCommand>();
    }
}
