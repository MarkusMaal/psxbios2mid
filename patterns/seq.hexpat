struct Event {
    u8 ProgramNumber [[color("ffff00")]];
    u8 NoteNumber [[color("ff00ff")]];
    u8 Velocity [[color("00ff00")]];
    u8 Pan [[color("00ffff")]];
    u16 TimeOffset [[color("ff0000")]];
    padding[2];
};

Event Track1[0x28] @ 0x4;
Event Track2[0x14] @ 0x14C;