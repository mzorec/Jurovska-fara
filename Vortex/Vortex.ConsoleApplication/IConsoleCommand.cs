using System.Collections.Generic;

namespace Vortex.ConsoleApplication
{
    public interface IConsoleCommand
    {
        string CommandDescription { get; }

        string CommandName { get; }

        int Execute(IEnumerable<string> args);

        void ShowHelp();
    }
}
