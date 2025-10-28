using System;
using System.Collections.Generic;

namespace AlphaOmega.Debug
{
	/// <summary>ELF file description. With header and all sections</summary>
	public class ElfFile : IDisposable
	{
		private Section[] _sections;
		private StringSection _sectionNames;

		/// <summary>ELF32/64 header</summary>
		public ElfHeader Header { get; private set; }

		/// <summary>
		/// An object file's section header table lets one locate all the file's sections.
		/// The section header table is an array of <see cref="Elf.Elf32_Shdr"/> or <see cref="Elf.Elf64_Shdr"/> structures as described below.
		/// A section header table index is a subscript into this array.
		/// The ELF header's <see cref="Header.e_shoff"/> member gives the byte offset from the beginning of the file to the section header table.
		/// <see cref="Header.e_shnum"/> normally tells how many entries the section header table contains.
		/// <see cref="Header.e_shentsize"/> gives the size in bytes of each entry.
		/// </summary>
		/// <remarks>
		/// Some section header table indexes are reserved in contexts where index size is restricted, for example, the <see cref="Elf.Elf64_Sym.st_shndx"/> member of a symbol table entry and the e_shnum and e_shstrndx members of the ELF header.
		/// In such contexts, the reserved values do not represent actual sections in the object file.
		/// Also in such contexts, an escape value indicates that the actual section index is to be found elsewhere, in a larger field.
		/// </remarks>
		public Section[] Sections => this._sections ?? (this._sections = this.ReadSections());

		/// <summary>This member holds the section header table of the entry associated with the section name string table</summary>
		/// <remarks>If the file has no section name string table, this member returns null</remarks>
		public StringSection SectionNames
		{
			get
			{
				if(this._sectionNames == null)
				{
					UInt16 index = this.Header.Header.e_shstrndx;
					if(index != (UInt16)Elf.SHN.UNDEF)
						this._sectionNames = this.GetStringSection(index);
				}
				return this._sectionNames;
			}
		}

		/// <summary>Create instance of the ELF file reader</summary>
		/// <param name="loader">Stream of data</param>
		/// <exception cref="ArgumentNullException">Loader is null</exception>
		/// <exception cref="InvalidOperationException">Invalid ELF header</exception>
		public ElfFile(IImageLoader loader)
		{
			_ = loader ?? throw new ArgumentNullException(nameof(loader));
			this.Header = new ElfHeader(loader);

			if(!this.Header.IsValid)
				throw new InvalidOperationException("Invalid ELF header");
		}

		/// <summary>Gets a STRTAB section from specified index</summary>
		/// <param name="index">section index with type STRTAB</param>
		/// <returns>STRTAB section from specified index or null</returns>
		public StringSection GetStringSection(UInt32 index)
		{
			foreach(Section section in this.GetSectionByType(Elf.SHT.STRTAB))
				if(section.Index == index)
					return new StringSection(section);

			return null;
		}

		/// <summary>
		/// String table sections hold null-terminated character sequences, commonly called strings.
		/// The object file uses these strings to represent symbol and section names.
		/// One references a string as an index into the string table section.
		/// </summary>
		/// <returns>All STRTAB sections from current ELF image</returns>
		public IEnumerable<StringSection> GetStringSections()
		{
			foreach(Section section in this.GetSectionByType(Elf.SHT.STRTAB))
				yield return new StringSection(section);
		}

		/// <summary>
		/// Sometimes a vendor or system builder needs to mark an object file with special information that other programs will check for conformance, compatibility, and so forth.
		/// Sections of type <see cref="Elf.SHT.NOTE"/> and program header elements of type PT_NOTE can be used for this purpose.
		/// </summary>
		/// <remarks>The note information in sections and program header elements holds any number of entries, each of which is an array of 4-byte words in the format of the target processor</remarks>
		public IEnumerable<NoteSection> GetNotesSections()
		{
			foreach(Section section in this.GetSectionByType(Elf.SHT.NOTE))
				yield return new NoteSection(section);
		}

		/// <summary>
		/// An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references. A symbol table index is a subscript into this array.
		/// Index 0 both designates the first entry in the table and serves as the undefined symbol index.
		/// </summary>
		public IEnumerable<SymbolSection> GetSymbolSections()
		{
			foreach(Section section in this.Sections)
				if(section.sh_type == Elf.SHT.SYMTAB || section.sh_type == Elf.SHT.DYNSYM)
					yield return new SymbolSection(section);
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		public IEnumerable<RelocationSection> GetRelocationSections()
		{
			foreach(Section section in this.Sections)
				if(section.sh_type == Elf.SHT.REL)
					yield return new RelocationSection(section);
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		public IEnumerable<RelocationASection> GetRelocationASections()
		{
			foreach(Section section in this.Sections)
				if(section.sh_type == Elf.SHT.RELA)
					yield return new RelocationASection(section);
		}

		/// <summary>This section contains information about user readable objects for debugging</summary>
		/// <remarks>This contents are unspecified</remarks>
		public DebugStringSection GetDebugStringSection()
		{
			foreach(Section section in this.Sections)
				if(section.Name == Constant.SectionNames.DebugString)
					return new DebugStringSection(section);

			return null;
		}

		/// <summary>Get all sections by section type</summary>
		/// <param name="type">Type of the required sections</param>
		/// <returns>All sections with required type</returns>
		public IEnumerable<Section> GetSectionByType(Elf.SHT type)
		{
			foreach(Section section in this.Sections)
				if(section.sh_type == type)
					yield return section;
		}

		private Section[] ReadSections()
		{
			UInt64 offset = this.Header.Header.e_shoff;
			UInt16 count = this.Header.Header.e_shnum;
			UInt32 size = this.Header.Header.e_shentsize;

			Section[] result = new Section[count];
			for(UInt16 loop = 0; loop < count; loop++)
			{
				if(this.Header.Is64Bit)
				{
					Elf.Elf64_Shdr section = this.Header.PtrToStructure<Elf.Elf64_Shdr>(offset);
					result[loop] = new Section(this, loop, section);
				} else
				{
					Elf.Elf32_Shdr section = this.Header.PtrToStructure<Elf.Elf32_Shdr>(offset);
					result[loop] = new Section(this, loop, section);
				}
				offset += size;
			}
			return result;
		}

		/// <summary>Close loader</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Dispose managed objects</summary>
		/// <param name="disposing">Dispose managed objects</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if(disposing && this.Header != null)
			{
				this.Header.Dispose();
				this.Header = null;
			}
		}
	}
}