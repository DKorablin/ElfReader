using System;
using System.Diagnostics;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// An object file's section header table lets one locate all the file's sections.
	/// The section header table is an array of Elf32_Shdr or Elf64_Shdr structures as described below.
	/// A section header table index is a subscript into this array.
	/// The ELF header's e_shoff member gives the byte offset from the beginning of the file to the section header table.
	/// e_shnum normally tells how many entries the section header table contains.
	/// e_shentsize gives the size in bytes of each entry.
	/// </summary>
	/// <remarks>
	/// Some section header table indexes are reserved in contexts where index size is restricted, for example, the st_shndx member of a symbol table entry and the e_shnum and e_shstrndx members of the ELF header.
	/// In such contexts, the reserved values do not represent actual sections in the object file.
	/// Also in such contexts, an escape value indicates that the actual section index is to be found elsewhere, in a larger field.
	/// </remarks>
	[DebuggerDisplay("Index={Index} Name={Name}")]
	public class Section : ISectionData
	{
		private readonly ElfFile _file;
		private readonly UInt32 _index;

		/// <summary>This member specifies the name of the section.</summary>
		/// <remarks>Its value is an index into the section header string table section (see "String Table") giving the location of a null-terminated string.</remarks>
		public readonly UInt32 sh_name;

		/// <summary>This member categorizes the section's contents and semantics.</summary>
		public readonly Elf.SHT sh_type;

		/// <summary>Sections support 1-bit flags that describe miscellaneous attributes.</summary>
		public readonly Elf.SHF sh_flags;

		/// <summary>If the section is to appear in the memory image of a process, this member gives the address at which the section's first byte should reside. Otherwise, the member contains 0.</summary>
		public readonly UInt64 sh_addr;

		/// <summary>
		/// This member gives the byte offset from the beginning of the file to the first byte in the section.
		/// Section type SHT_NOBITS, described below, occupies no space in the file, and its sh_offset member locates the conceptual placement in the file.
		/// </summary>
		public readonly UInt64 sh_offset;

		/// <summary>
		/// This member gives the section's size in bytes. Unless the section type is SHT_NOBITS, the section occupies sh_size bytes in the file.
		/// A section of type SHT_NOBITS can have a nonzero size, but it occupies no space in the file.
		/// </summary>
		public readonly UInt64 sh_size;

		/// <summary>This member holds a section header table index link, whose interpretation depends on the section type.</summary>
		public readonly UInt32 sh_link;

		/// <summary>This member holds extra information, whose interpretation depends on the section type.</summary>
		public readonly UInt32 sh_info;

		/// <summary>
		/// Some sections have address alignment constraints.
		/// For example, if a section holds a double-word, the system must ensure double-word alignment for the entire section.
		/// That is, the value of sh_addr must be congruent to 0, modulo the value of sh_addralign.
		/// Currently, only 0 and positive integral powers of two are allowed.
		/// Values 0 and 1 mean the section has no alignment constraints.
		/// </summary>
		public readonly UInt64 sh_addralign;

		/// <summary>
		/// Some sections hold a table of fixed-size entries, such as a symbol table.
		/// For such a section, this member gives the size in bytes of each entry.
		/// The member contains 0 if the section does not hold a table of fixed-size entries.
		/// </summary>
		public readonly UInt64 sh_entsize;

		internal ElfFile File { get { return this._file; } }

		/// <summary>This member specifies the name of the section.</summary>
		public String Name { get { return this.File.SectionNames[this.sh_name]; } }

		/// <summary>Section index</summary>
		public UInt32 Index { get { return this._index; } }

		/// <summary>Section Attribute Flags</summary>
		/// <remarks>
		/// If a flag bit is set in sh_flags, the attribute is on for the section.
		/// Otherwise, the attribute is off or does not apply.
		/// Undefined attributes are reserved and set to zero.
		/// </remarks>
		public Elf.SHF Flags { get { return (Elf.SHF)this.sh_flags; } }

		/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
		public Boolean IsOs { get { return this.sh_type >= Elf.SHT.LOOS && this.sh_type <= Elf.SHT.HIOS; } }

		/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
		public Boolean IsProc { get { return this.sh_type >= Elf.SHT.LOPROC && this.sh_type <= Elf.SHT.HIPROC; } }

		/// <summary>Section types between SHT_LOUSER and SHT_HIUSER can be used by the application, without conflicting with current or future system-defined section types.</summary>
		public Boolean IsUser { get { return this.sh_type >= Elf.SHT.LOUSER && this.sh_type <= Elf.SHT.HIUSER; } }


		internal Section(ElfFile file, UInt32 index, Elf.Elf32_Shdr section)
			: this(file, index)
		{
			this.sh_name = section.sh_name;
			this.sh_type = section.sh_type;
			this.sh_flags = (Elf.SHF)section.sh_flags;
			this.sh_addr = section.sh_addr;
			this.sh_offset = section.sh_offset;
			this.sh_size = section.sh_size;
			this.sh_link = section.sh_link;
			this.sh_info = section.sh_info;
			this.sh_addralign = section.sh_addralign;
			this.sh_entsize = section.sh_entsize;
		}

		internal Section(ElfFile file, UInt32 index, Elf.Elf64_Shdr section)
			: this(file, index)
		{
			this.sh_name = section.sh_name;
			this.sh_type = section.sh_type;
			this.sh_flags = (Elf.SHF)section.sh_flags;
			this.sh_addr = section.sh_addr;
			this.sh_offset = section.sh_offset;
			this.sh_size = section.sh_size;
			this.sh_link = section.sh_link;
			this.sh_info = section.sh_info;
			this.sh_addralign = section.sh_addralign;
			this.sh_entsize = section.sh_entsize;
		}

		private Section(ElfFile file, UInt32 index)
		{
			if(file == null)
				throw new ArgumentNullException("file");

			this._file = file;
			this._index = index;
		}

		/// <summary>Get raw section data</summary>
		/// <returns>Raw section data</returns>
		public Byte[] GetData()
		{
			if(this.sh_type == Elf.SHT.NOBITS)
				return new Byte[] { };
			else
				return this.File.Header.ReadBytes(this.sh_offset, this.sh_size);
		}
	}
}