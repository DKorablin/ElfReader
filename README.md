## ELF Reader

[![Auto build](https://github.com/DKorablin/ElfReader/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/ElfReader/releases/latest)
[![Nuget](https://img.shields.io/nuget/v/AlphaOmega.ElfReader)](https://www.nuget.org/packages/AlphaOmega.ElfReader)

Executable and Linkable file reader

Usage:

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

- _Header_ &mdash; ELF file header
  - _Identification_ &mdash; ELF file identification information
- _Sections_ &mdash; An object file's section header table lets one locate all the file's sections.
- _SectionNames_ &mdash; This member holds the section header table of the entry associated with the section name string table
- _SHT_STRTAB_ &mdash; String table sections hold null-terminated character sequences, commonly called strings.
- _SHT_NOTE_ &mdash; Sometimes a vendor or system builder needs to mark an object file with special information that other programs will check for conformance, compatibility, and so forth.
- _SHT_DYNSYM SHT_SYMTAB_ &mdash; An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references.
- _SHT_REL_ &mdash; Relocation is the process of connecting symbolic references with symbolic definitions.
- _SHT_RELA_ &mdash; Relocation is the process of connecting symbolic references with symbolic definitions.