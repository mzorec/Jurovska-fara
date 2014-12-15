using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vortex.ConsoleApplication
{
   public class Program
    {
        public static int Main(string[] args)
        {
            List<IConsoleCommand> commands = new List<IConsoleCommand>();
            commands.Add(new CreateDbCommand());
            
            ConsoleApp consoleApp = new ConsoleApp(args, commands);

            Environment.ExitCode = consoleApp.Process();
            return Environment.ExitCode;
        }
    }
}
