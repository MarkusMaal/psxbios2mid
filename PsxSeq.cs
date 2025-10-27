namespace psxbios2mid_app;

public class PsxSeq
{
    
    public List<List<Event>> Tracks { get; set; } = [];
    
    public PsxSeq(Stream stream)
    {
        stream.Position = 4;
        Tracks.Add([]);
        while (stream.Position < stream.Length - 8)
        {
            var buffer = new byte[8];
            stream.ReadExactly(buffer, 0, buffer.Length);
            if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 0)
            {
                Tracks.Add([]);
                continue;
            }
            Tracks.Last().Add(new Event
            {
                ProgramNumber = buffer[0],
                NoteNumber = buffer[1],
                Velocity = buffer[2],
                Pan = buffer[3],
                TimeOffset = BitConverter.ToUInt16(buffer.Skip(4).Take(2).ToArray(), 0),
            });
        }
    }

    public struct Event
    {
        public byte ProgramNumber { get; set; }
        public byte NoteNumber { get; set; }
        public byte Velocity { get; set; }
        public byte Pan { get; set; }
        public ushort TimeOffset { get; set; }
    }
}