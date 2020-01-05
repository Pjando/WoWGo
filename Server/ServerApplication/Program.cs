using System;
using System.Threading;

/// <summary>
/// The main thread of the server, intitalises other server components.
/// </summary>
class Program {

    /// <summary>
    /// Thread for accepting user input, server side.
    /// </summary>
    private static Thread threadConsole;
    /// <summary>
    /// Whether the console/Server is running.
    /// </summary>
    private static bool consoleRunning;

    /// <summary>
    /// Main program starts the console thread and starts other server components.
    /// </summary>
    static void Main(string[] args) {

        threadConsole = new Thread(new ThreadStart(ConsoleThread));
        threadConsole.Start();
        ServerHandleData.instance.InitMessages();
        Network.instance.ServerStart();
        GameLogic.instance.ServerLoop();
    }

    /// <summary>
    /// Console thread, closes server upon detecting empty read line.
    /// </summary>
    private static void ConsoleThread() {
        string line;
        consoleRunning = true;

        while (consoleRunning) {
            line = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(line)) {
                consoleRunning = false;
                return;
            } else {

            }
        }
    }

}
