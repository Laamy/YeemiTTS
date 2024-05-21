using System.Collections.Generic;

class CommandHandler
{
    public static List<CommandBase> Commands = new List<CommandBase>();

    public static void Init()
    {
        Commands.Add(new ResetCommand());
        Commands.Add(new PlayCommand());
    }
}