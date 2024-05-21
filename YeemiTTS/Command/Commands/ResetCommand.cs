class ResetCommand : CommandBase
{
    public ResetCommand()
    {
        Name = "reset";
    }

    public override void Execute(string[] args)
    {
        Program.etts = new EdgeTTS.EdgeTTSClient();
    }
}