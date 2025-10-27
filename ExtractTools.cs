using MidiSharp;
using MidiSharp.Events.Meta;
using MidiSharp.Events.Voice;
using MidiSharp.Events.Voice.Note;

namespace psxbios2mid_app;

public abstract class ExtractTools
{
    public static void ConvertSeqMid(string inFile, string outFile)
    {
        var seq = new PsxSeq(File.OpenRead(inFile));
        foreach (var (i, t) in seq.Tracks.Index())
        {
            var outStream =  new FileStream($"{outFile[..^3] }{i}.mid", FileMode.Create);
            var midiFile = new MidiSequence(Format.Zero, 960);
            midiFile.Tracks.AddNewTrack();
            foreach (var e in t)
            {
                var channelNum = e.ProgramNumber;
                midiFile.Tracks.Last().Events
                    .Add(new ProgramChangeVoiceMidiEvent(e.TimeOffset, channelNum, e.ProgramNumber));
                midiFile.Tracks.Last().Events
                    .Add(new OnNoteVoiceMidiEvent(e.TimeOffset, channelNum, e.NoteNumber, e.Velocity));
            }
            midiFile.Tracks.Last().Events.Add(new EndOfTrackMetaMidiEvent(9999));
            midiFile.Save(outStream);
            outStream.Close();
        }
    }
    
    public static void FindExtractVab(string inFile, string outFile)
    {
        Console.WriteLine("Searching for VAB header...");
        using var sr = File.OpenRead(inFile);
        var offset = FindVab(sr);
        if (offset == -1)
        {
            Console.WriteLine("No VAB header found.");
            return;
        }
        Console.WriteLine("Found a header! Extracting...");
        sr.Position = offset + 0xC;
        var lengthBuffer = new byte[4];
        sr.ReadExactly(lengthBuffer, 0, 4);
        var vabLength = BitConverter.ToInt32(lengthBuffer);
        var vabBuffer =  new byte[vabLength];
        sr.Position = offset;
        sr.ReadExactly(vabBuffer, 0, vabBuffer.Length);
        using (var sw = File.OpenWrite(outFile))
        {
            sw.Write(vabBuffer, 0, vabBuffer.Length);
            sw.Close();
        }

        sr.Close();
        Console.WriteLine("Done!");
    }

    public static void FindExtractSeq(string inFile, string outFile)
    {
        Console.WriteLine("Searching for VAB header...");
        using var sr = File.OpenRead(inFile);
        var offset = FindVab(sr);
        if (offset == -1)
        {
            Console.WriteLine("No VAB header found.");
            return;
        }
        Console.WriteLine("Found a header! Skipping it...");
        sr.Position = offset + 0xC;
        var lengthBuffer = new byte[4];
        sr.ReadExactly(lengthBuffer, 0, 4);
        var vabLength = BitConverter.ToInt32(lengthBuffer);
        sr.Position = offset + vabLength;
        
        Console.WriteLine("Saving sequence data...");

        using var sw = File.OpenWrite(outFile);
        while (sr.Position < offset + vabLength + 0x1F0)
        {
            sw.WriteByte((byte)sr.ReadByte());
        }

        sw.Close();
        sr.Close();
        
        Console.WriteLine("Done!");
    }

    private static int FindVab(Stream inStream)
    {
        var offset = -1;
        var i = 0;
        inStream.Position = 0;
        while (inStream.Position < inStream.Length - 4)
        {
            var buffer = new byte[4];
            inStream.ReadExactly(buffer, 0, 4);

            if (BitConverter.ToUInt32(buffer) != 0x56414270)
            {
                i += 4;
                continue;
            }
            offset = i;
            break;
        }

        return offset;
    }
}