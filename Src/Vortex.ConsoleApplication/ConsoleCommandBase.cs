using System.Collections.Generic;
using LoyaltyProgram.Console;
using NDesk.Options;

namespace Vortex.ConsoleApplication
{
    public abstract class ConsoleCommandBase : IConsoleCommand
    {
        public abstract string CommandDescription { get; }

        public abstract string CommandName { get; }

        public abstract int Execute(IEnumerable<string> args);

        public void ShowHelp()
        {
            Options.WriteOptionDescriptions(System.Console.Out);
        }

        protected OptionSet Options
        {
            get
            {
                return options;
            }
        }

        protected void AddOptions(OptionSet additionalOptions)
        {
            foreach (Option option in additionalOptions)
            {
                options.Add(option);
            }
        }

        private readonly OptionSet options = new OptionSet();
    }
}
