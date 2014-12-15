using System.Collections.Generic;
using NDesk.Options;

namespace Vortex.ConsoleApplication
{
    public abstract class ConsoleCommandBase : IConsoleCommand
    {
        private readonly OptionSet options = new OptionSet();

        public abstract string CommandDescription { get; }

        public abstract string CommandName { get; }

        protected OptionSet Options
        {
            get
            {
                return options;
            }
        }

        public abstract int Execute(IEnumerable<string> args);

        public void ShowHelp()
        {
            Options.WriteOptionDescriptions(System.Console.Out);
        }

        protected void AddOptions(OptionSet additionalOptions)
        {
            foreach (Option option in additionalOptions)
            {
                options.Add(option);
            }
        }
    }
}
