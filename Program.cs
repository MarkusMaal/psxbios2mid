using System.Reflection;

namespace psxbios2mid_app;

public static class Program
{
    public static int Main(string[] args)
    {
        var processor = new ArgProcessor(args);
        if (!processor.Validate())
        {
            Console.WriteLine("Invalid arguments. Please pass --help to see correct usage.");
            return 1;
        }

        switch (processor.Mode)
        {
            case Modes.Help:
                ShowHelp();
                break;
            case Modes.Version:
                Console.WriteLine($"Version: {System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}\n.NET runtime version: {Environment.Version}");
                break;
            case Modes.ExtractVab:
                ExtractTools.FindExtractVab(processor.InputFile!, processor.OutFile!);
                break;
            case Modes.ExtractSeq:
                ExtractTools.FindExtractSeq(processor.InputFile!, processor.OutFile!);
                break;
            case Modes.SeqToMidi:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return 0;
    }

    private static void ShowHelp()
    {
        Console.WriteLine("""
                          psxbios2mid
                          Usage: psxbios2mid [args] -i [input] -o [output]
                          
                          --help [-h]           - Show command line usage
                          --version [-v]        - Show version information
                          
                          --extract-vab [-ev]   - Extract VAB file from the memory dump
                          --extract-seq [-es]   - Extract sequence from memory dump (proprietary format)
                          --seq-to-midi [-sm]   - Convert extracted sequence to MIDI
                          
                          --input [-i]          - Input file (not required for -h and -v)
                          --output [-o]         - Output file (not required for -h and -v)
                          """);
    }
}

