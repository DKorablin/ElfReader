ELF Reader
========

Executable and Linkable file reader

Usage:
<pre>
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
</pre>

<ul>
	<li><i>Header</i> &mdash; ELF file header
		<ul>
			<li><i>Identification</i> &mdash; ELF file identification information</li>
		</ul>
	</li>
	<li><i>Sections</i> &mdash; An object file's section header table lets one locate all the file's sections.</li>
	<li><i>SectionNames</i> &mdash; This member holds the section header table of the entry associated with the section name string table</li>
	<li><i>SHT_STRTAB</i> &mdash; String table sections hold null-terminated character sequences, commonly called strings.</li>
	<li><i>SHT_NOTE</i> &mdash; Sometimes a vendor or system builder needs to mark an object file with special information that other programs will check for conformance, compatibility, and so forth.</li>
	<li><i>SHT_DYNSYM SHT_SYMTAB</i> &mdash; An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references.</li>
	<li><i>SHT_REL</i> &mdash; Relocation is the process of connecting symbolic references with symbolic definitions.</li>
	<li><i>SHT_RELA</i> &mdash; Relocation is the process of connecting symbolic references with symbolic definitions.</li>
</ul>
