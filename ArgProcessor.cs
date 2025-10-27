namespace psxbios2mid_app;

public class ArgProcessor
{
    public Modes Mode { get; set; } = Modes.Help;
    public string? InputFile { get; set; }
    public string? OutFile { get; set; }
    
    public ArgProcessor(string[] args)
    {
        var lastArg = "";
        foreach (var arg in args)
        {
            Mode = arg switch
            {
                "--help" or "-h" => Modes.Help,
                "--version" or "-v" => Modes.Version,
                "--extract-vab" or "-ev" => Modes.ExtractVab,
                "--extract-seq" or "-es" => Modes.ExtractSeq,
                "--seq-to-midi" or "-sm" => Modes.SeqToMidi,
                _ => Mode
            };

            switch (lastArg)
            {
                case "--input":
                case "-i":
                    InputFile = arg;
                    break;
                case "--output":
                case "-o":
                    OutFile = arg;
                    break;
            }
            lastArg = arg;
        }
    }

    public bool Validate()
    {
        return Mode switch
        {
            Modes.ExtractVab or Modes.ExtractSeq or Modes.SeqToMidi =>
                InputFile != null && File.Exists(InputFile) && OutFile != null,
            _ => true
        };
    }
}

public enum Modes
{
    Help,
    Version,
    ExtractVab,
    ExtractSeq,
    SeqToMidi,
}