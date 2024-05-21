class PlayCommand : CommandBase
{
    public PlayCommand()
    {
        Name = "play";
    }

    public override void Execute(string[] args)
    {
        if (args[1] == "sound")
        {
            Program.PlaySound($"Soundboard\\{args[2]}.mp3");
        }
    }
}