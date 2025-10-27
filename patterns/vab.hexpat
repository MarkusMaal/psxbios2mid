u32 VagPos = 0;

struct ToneAttribChunk { 
    u8 TonePriority [[color("ff00ff")]];
    u8 Mode [[color("ffff00")]];
    u8 ToneVolume [[color("ff00ff")]];
    s8 TonePan [[color("ff00ff")]];
    u8 CentreNote [[color("ff0000")]];
    u8 CentreNoteFineTune [[color("ff0000")]];
    u8 NoteLimMin [[color("ff0000")]];
    u8 NoteLimMax [[color("ff0000")]];
    u8 VibW [[color("7fff00")]];
    u8 VibT [[color("7fff00")]];
    u8 PorW [[color("7fff00")]];
    u8 PorT [[color("7fff00")]];
    u8 PitchBendMin [[color("7fff00")]];
    u8 PitchBendMax [[color("7fff00")]];
    u8 Reserved1 [[color("202020")]];
    u8 Reserved2 [[color("202020")]];
    u16 AttackDecay [[color("00007f")]];
    u16 ReleaseSustain [[color("00007f")]];
    u16 ProgNum [[color("007fff")]];
    u16 VagNum [[color("007fff")]];
    u64 Reserved [[color("202020")]];
};

struct ProgAttribChunk {
    u8 Tones [[color("ff00ff")]];
    u8 MasterVolume [[color("ff0000")]];
    u8 Prior [[color("00ffff")]];
    u8 Mode [[color("00ff00")]];
    u8 MPan [[color("ff0000")]];
    u8 Reserved0 [[color("202020")]];
    u16 Attrib [[color("ff7f00")]];
    u32 Reserved1 [[color("202020")]];
    u32 Reserved2 [[color("202020")]];
};

struct VagSize {
    u16 VagSize [[color("00ff00")]];
    u8 VagSample[VagSize*8] @ parent.Position + 0x200 + VagPos;
    VagPos += VagSize*8;
};

struct Header {
    char FileId[4];
    u32 Version;
    u32 VabId;
    u32 TotalSize;
    u16 Reserved;
    u16 NumPrograms;
    u16 NumTones;
    u16 NumVags;
    u8 MasterVolume;
    u8 MasterPan;
    u8 BankAttrib1;
    u8 BankAttrib2;
    u32 Reserved2;
    ProgAttribChunk ProgramAttributes[0x80];
    ToneAttribChunk ToneAttribs[NumPrograms*2*NumTones];
    u32 Position = $;
    VagSize VagSizes[NumVags+1];
    u8 Padding[0x200-((NumVags+1)*2)];
};

Header VabHeader @ 0x00;