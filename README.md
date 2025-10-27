# psxbios2mid

This tool lets you extract soundfont and sequence data from the PSX memory dump.

## Taking the memory dump

This is easy enough to do with DuckStation. First enable the debug menu by navigating to `Settings > Advanced > System Settings`. Accept the warning to actually enable it.

Then, boot to BIOS with no game disc inserted. Take a memory dump by going to `Debug > Dump RAM` and save it somewhere. This file is now a memory dump of PlayStation in the BIOS menu.

## Extracting VAB file

This is the soundfont. You can extract it by running the program like so:

`psxbios2mid -ev -i <path to your memory dump> -o <path to output .VAB file>`

## Extracting the sequence

This is basically like MIDI, where there is a track with a bunch of note on events, but it is non-standard. You can extract it like so:

`psxbios2mid -es -i <path to your memory dump> -o <path to output .SEQ file>`

## Converting sequence to MIDI

The extracted .SEQ file will be unusable. To fix this, we need to convert it to some other format first, such as MIDI, which this program can do. You can run the conversion like this:

`psxbios2mid -sm -i <path to extracted SEQ file> -o <path to output .MID file>`

The PlayStation BIOS has 2 sequences, one for the initial boot and the second one that plays when you insert a game disc, which is why you'll end up with 2 MIDI files.

## Data analysis

If you want to analyse these formats (including the non-standard SEQ file), I have provided ImHex patterns on this repository located at the **patterns/** directory.