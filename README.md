## ELF Reader

[![Auto build](https://github.com/DKorablin/ElfReader/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/ElfReader/releases/latest)
[![NuGet](https://img.shields.io/nuget/v/AlphaOmega.ElfReader)](https://www.nuget.org/packages/AlphaOmega.ElfReader)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AlphaOmega.ElfReader)](https://www.nuget.org/packages/AlphaOmega.ElfReader)

Executable and Linkable Format (ELF) reader / parser for .NET.

Targets: net35, netstandard2.0 (usable on modern .NET via netstandard2.0).

---
### Features
- Parse ELF32 / ELF64 headers
- Endianness aware (LSB / MSB)
- Enumerate sections, symbols, notes, string tables
- Read relocations (REL & RELA)
- Access raw section bytes (except NOBITS)
- Strongly typed enums / structs mirroring spec
- Pure managed code (no native dependency)

### Installation
```
dotnet add package AlphaOmega.ElfReader
```
Or
```
Install-Package AlphaOmega.ElfReader
```

### Quick Start
```csharp
using AlphaOmega.Debug;
using(var elf = new ElfFile(StreamLoader.FromFile(@"/usr/lib/libc.so.6")))
{
    if(!elf.Header.IsValid) return;

    // Sections
    foreach(var s in elf.Sections)
        Console.WriteLine($"[{s.Index}] {s.Name} Type={s.sh_type} Size=0x{s.sh_size:X}");

    // Symbols (SYMTAB + DYNSYM)
    foreach(var symSec in elf.GetSymbolSections())
        foreach(var sym in symSec)
            Console.WriteLine($"SYM {sym.Name} Bind={sym.Bind} Type={sym.Type} Vis={sym.Visibility}");

    // Relocations (no addend)
    foreach(var relSec in elf.GetRelocationSections())
        foreach(var rel in relSec) { /* process */ }

    // Relocations with addend
    foreach(var relaSec in elf.GetRelocationASections())
        foreach(var rela in relaSec) { /* process */ }

    // Notes
    foreach(var noteSec in elf.GetNotesSections())
        foreach(var note in noteSec)
            Console.WriteLine($"NOTE {note.Name} Type={note.Type} Size={note.Desc?.Length}");
}
```

### API Overview
- Header – ELF header wrapper (IsValid, Is64Bit)
- Sections – Parsed section headers
- SectionNames – Section name string table (if present)
- GetStringSections() – STRTAB enumerators
- GetNotesSections() – NOTE sections
- GetSymbolSections() – SYMTAB / DYNSYM combined helper
- GetRelocationSections() – REL
- GetRelocationASections() – RELA
- Section.GetData() – Raw bytes (except NOBITS)

### Typical Workflow
1. Open file via StreamLoader / custom IImageLoader
2. Validate header
3. Enumerate sections or filter by type
4. Enumerate symbols / relocations / notes
5. Read raw data for custom parsing

### Safety
Only structural parsing performed. Validate offsets / sizes for untrusted input.

### Limitations / TODO
- Program headers (segments) helper not implemented
- Architecture-specific relocation interpretation left to caller
- DWARF / debug info not parsed

---
Original minimal sample:
```
using(ElfFile file = new ElfFile(StreamLoader.FromFile(@"C:\Test\libmono.so")))
{
    if(file.Header.IsValid)
    {
        foreach(var strSec in file.GetStringSections())
            foreach(var str in strSec)
                //...

        foreach(var noteSec in file.GetNotesSections())
            foreach(var note in noteSec)
                //...

        foreach(var symbolSec in file.GetSymbolSections())
            foreach(var symbol in symbolSec)
                //...

        foreach(var relSec in file.GetRelocationSections())
            foreach(var rel in relSec)
                //...

        foreach(var relaSec in file.GetRelocationASections())
            foreach(var rela in relaSec)//TODO: Check it
                //...

        SymbolSection symbols = file.GetSymbolSections().FirstOrDefault();
        if(symbols != null)
            //...
    }
}
```