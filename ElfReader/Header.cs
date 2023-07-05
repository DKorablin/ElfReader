using System;

namespace AlphaOmega.Debug
{
	/// <summary>ELF image header</summary>
	public class Header
	{
		private readonly ElfHeader _header;

		/// <summary>This member identifies the object file type</summary>
		public readonly Elf.ET e_type;
		/// <summary>This member's value specifies the required architecture for an individual file</summary>
		public readonly Elf.EM e_machine;
		/// <summary>This member identifies the object file version</summary>
		/// <remarks>
		/// The value 1 signifies the original file format; extensions will create new versions with higher numbers.
		/// The value of <see cref="Elf.EV.CURRENT"/> changes as necessary to reflect the current version number.
		/// </remarks>
		public readonly Elf.EV e_version;
		/// <summary>This member gives the virtual address to which the system first transfers control, thus starting the process</summary>
		/// <remarks>If the file has no associated entry point, this member holds zero</remarks>
		public readonly UInt64 e_entry;
		/// <summary>This member holds the program header table's file offset in bytes</summary>
		/// <remarks>If the file has no program header table, this member holds zero</remarks>
		public readonly UInt64 e_phoff;
		/// <summary>This member holds the section header table's file offset in bytes</summary>
		/// <remarks>If the file has no section header table, this member holds zero</remarks>
		public readonly UInt64 e_shoff;
		/// <summary>
		/// This member holds processor-specific flags associated with the file.
		/// Flag names take the form EF_machine_flag.
		/// </summary>
		/// <remarks>This member is presently zero for SPARC and IA</remarks>
		public readonly UInt32 e_flags;
		/// <summary>This member holds the ELF header's size in bytes</summary>
		public readonly UInt16 e_ehsize;
		/// <summary>This member holds the size in bytes of one entry in the file's program header table; all entries are the same size</summary>
		public readonly UInt16 e_phentsize;
		/// <summary>
		/// This member holds the number of entries in the program header table.
		/// Thus the product of e_phentsize and e_phnum gives the table's size in bytes.
		/// </summary>
		/// <remarks>If a file has no program header table, e_phnum holds the value zero</remarks>
		public readonly UInt16 e_phnum;
		/// <summary>
		/// This member holds a section header's size in bytes.
		/// A section header is one entry in the section header table
		/// </summary>
		/// <remarks>all entries are the same size</remarks>
		public readonly UInt16 e_shentsize;
		/// <summary>
		/// This member holds the number of entries in the section header table.
		/// Thus the product of e_shentsize and e_shnum gives the section header table's size in bytes.
		/// </summary>
		/// <remarks>If a file has no section header table, e_shnum holds the value zero</remarks>
		public readonly UInt16 e_shnum;

		/// <summary>This member holds the section header table index of the entry associated with the section name string table</summary>
		/// <remarks>If the file has no section name string table, this member holds the value SHN_UNDEF</remarks>
		public readonly UInt16 e_shstrndx;

		/// <summary>Special Section Indexes</summary>
		public Elf.SHN SpecShIndex
		{
			get
			{
				Elf.SHN result = (Elf.SHN)this.e_shnum;
				return result >= Elf.SHN.UNDEF
					? result
					: Elf.SHN.UNDEF;
			}
		}

		internal Header(ElfHeader header, Elf.Elf32_Ehdr header32)
			: this(header)
		{
			this.e_type = header32.e_type;
			this.e_machine = header32.e_machine;
			this.e_version = header32.e_version;
			this.e_entry = header32.e_entry;
			this.e_phoff = header32.e_phoff;
			this.e_shoff = header32.e_shoff;
			this.e_flags = header32.e_flags;
			this.e_ehsize = header32.e_ehsize;
			this.e_phentsize = header32.e_phentsize;
			this.e_phnum = header32.e_phnum;
			this.e_shentsize = header32.e_shentsize;
			this.e_shnum = header32.e_shnum;
			this.e_shstrndx = header32.e_shstrndx;
		}

		internal Header(ElfHeader header, Elf.Elf64_Ehdr header64)
			: this(header)
		{
			this.e_type = header64.e_type;
			this.e_machine = header64.e_machine;
			this.e_version = header64.e_version;
			this.e_entry = header64.e_entry;
			this.e_phoff = header64.e_phoff;
			this.e_shoff = header64.e_shoff;
			this.e_flags = header64.e_flags;
			this.e_ehsize = header64.e_ehsize;
			this.e_phentsize = header64.e_phentsize;
			this.e_phnum = header64.e_phnum;
			this.e_shentsize = header64.e_shentsize;
			this.e_shnum = header64.e_shnum;
			this.e_shstrndx = header64.e_shstrndx;
		}

		private Header(ElfHeader header)
		{
			this._header = header?? throw new ArgumentNullException(nameof(header));
		}
	}
}