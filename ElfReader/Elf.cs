using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AlphaOmega.Debug
{
	/// <summary>Basic Executable and Linkable Format structures</summary>
	/// <remarks>https://docs.oracle.com/cd/E19957-01/806-0641/chapter6-43405/index.html</remarks>
	public struct Elf
	{
		/// <summary>
		/// The file format is designed to be portable among machines of various sizes, without imposing the sizes of the largest machine on the smallest.
		/// The class of the file defines the basic types used by the data structures of the object file container itself.
		/// The data contained in object file sections may follow a different programming model.
		/// If so, the processor supplement describes the model used.
		/// </summary>
		/// <remarks>Other classes will be defined as necessary, with different basic types and sizes for object file data.</remarks>
		public enum ELFCLASS : byte
		{
			/// <summary>Invalid class</summary>
			NONE = 0,
			/// <summary>
			/// Supports machines with 32-bit architectures.
			/// It uses the basic types defined in the table labeled "32-Bit Data Types."
			/// </summary>
			CLASS32 = 1,
			/// <summary>
			/// Supports machines with 64-bit architectures.
			/// It uses the basic types defined in the table labeled "64-Bit Data Types."
			/// </summary>
			CLASS64 = 2,
		}

		/// <summary>Specifies the encoding of both the data structures used by object file container and data contained in object file sections.</summary>
		public enum ELFDATA : byte
		{
			/// <summary>Invalid data encoding</summary>
			NONE = 0,
			/// <summary>Encoding ELFDATA2LSB specifies 2's complement values, with the least significant byte occupying the lowest address.</summary>
			/// <remarks>LE</remarks>
			_2LSB = 1,
			/// <summary>Encoding ELFDATA2MSB specifies 2's complement values, with the most significant byte occupying the lowest address.</summary>
			/// <remarks>BE</remarks>
			_2MSB = 2,
		}

		/// <summary>This member identifies the object file version.</summary>
		public enum EV : byte
		{
			/// <summary>Invalid version</summary>
			NONE = 0,
			/// <summary>Current version</summary>
			CURRENT = 1,
		}

		/// <summary>Identifies the OS- or ABI-specific ELF extensions used by this file</summary>
		public enum ELFOSABI : byte
		{
			/// <summary>No extensions or unspecified</summary>
			NONE = 0,
			/// <summary>Hewlett-Packard HP-UX</summary>
			HPUX = 1,
			/// <summary>NetBSD</summary>
			NETBSD = 2,
			/// <summary>GNU (Linux)</summary>
			GNU = 3,
			/// <summary>Sun Solaris</summary>
			SOLARIS = 6,
			/// <summary>AIX</summary>
			AIX = 7,
			/// <summary>IRIX</summary>
			IRIX = 8,
			/// <summary>FreeBSD</summary>
			FREEBSD = 9,
			/// <summary>Compaq TRU64 UNIX</summary>
			TRU64 = 10,
			/// <summary>Novell Modesto</summary>
			MONDESTO = 11,
			/// <summary>Open BSD</summary>
			OPENBSD = 12,
			/// <summary>Open VMS</summary>
			OPENVMS = 13,
			/// <summary>Hewlett-Packard Non-Stop Kernel</summary>
			NSK = 14,
			/// <summary>Amiga Research OS</summary>
			AROS = 15,
			/// <summary>The FenixOS highly scalable multi-core OS</summary>
			FENIXOS = 16,
			/// <summary>Nuxi CloudABI</summary>
			CLOUDABI = 17,
			/// <summary>Stratus Technologies OpenVOS</summary>
			OPENVOS = 18,
		}

		/// <summary>ELF file identification information</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct EI
		{
			/// <summary>A file's first 4 bytes hold a ``magic number,'' identifying the file as an ELF object file.</summary>
			/// <remarks>0x7F, 'E', 'L', 'F'</remarks>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public Byte[] magic;

			/// <summary>
			/// The file format is designed to be portable among machines of various sizes, without imposing the sizes of the largest machine on the smallest.
			/// The class of the file defines the basic types used by the data structures of the object file container itself.
			/// The data contained in object file sections may follow a different programming model.
			/// If so, the processor supplement describes the model used.
			/// </summary>
			public Elf.ELFCLASS _class;

			/// <summary>Specifies the encoding of both the data structures used by object file container and data contained in object file sections.</summary>
			public Elf.ELFDATA data;

			/// <summary>This member identifies the object file version.</summary>
			/// <remarks>
			/// The value 1 signifies the original file format; extensions will create new versions with higher numbers.
			/// Although the value of EV_CURRENT is shown as 1 in the previous table, it will change as necessary to reflect the current version number.
			/// </remarks>
			public Elf.EV version;

			/// <summary>Identifies the OS- or ABI-specific ELF extensions used by this file</summary>
			public Elf.ELFOSABI osabi;

			/// <summary>Identifies the version of the ABI to which the object is targeted</summary>
			/// <remarks>
			/// This field is used to distinguish among incompatible versions of an ABI.
			/// The interpretation of this version number is dependent on the ABI identified by the EI_OSABI field.
			/// If no values are specified for the EI_OSABI field by the processor supplement or no version values are specified for the ABI determined by a particular value of the EI_OSABI byte, the value 0 shall be used for the EI_ABIVERSION byte;
			/// it indicates unspecified.
			/// </remarks>
			public Byte abiversion;

			/// <summary>This value marks the beginning of the unused bytes in e_ident</summary>
			/// <remarks>
			///  These bytes are reserved and set to zero; programs that read object files should ignore them.
			///  The value of EI_PAD will change in the future if currently unused bytes are given meanings.
			/// </remarks>
			public Byte pad;

			/// <summary>
			/// This value marks the beginning of the unused bytes in e_ident.
			/// These bytes are reserved and set to zero; programs that read object files should ignore them.
			/// The value of nident will change in the future if currently unused bytes are given meanings.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public Byte[] nident;

			/// <summary>String representation for signature field</summary>
			public String SignatureStr { get { return Encoding.ASCII.GetString(this.magic).Replace("\x7f", "\\x7f"); } }

			/// <summary>ELF header is valid</summary>
			public Boolean IsValid
			{
				get
				{
					return this.magic[0] == 0x7f && this.magic[1] == 0x45 && this.magic[2] == 0x4c && this.magic[3] == 0x46;
				}
			}
		}

		/// <summary>File Identifiers</summary>
		/// <remarks>
		/// Although the core file contents are unspecified, type ET_CORE is reserved to mark the file.
		/// Values from ET_LOPROC through ET_HIPROC (inclusive) are reserved for processor-specific semantics.
		/// Other values are reserved and will be assigned to new object file types as necessary.
		/// </remarks>
		public enum ET : ushort
		{
			/// <summary>No file type</summary>
			NONE = 0,
			/// <summary>Relocatable file</summary>
			REL = 1,
			/// <summary>Executable file</summary>
			EXEC = 2,
			/// <summary>Shared object file</summary>
			DYN = 3,
			/// <summary>Core file</summary>
			CORE = 4,
			/// <summary>Processor-specific</summary>
			LOPROC = 0xff00,
			/// <summary>Processor-specific</summary>
			HIPROC = 0xffff,
		}

		/// <summary>ELF Machines</summary>
		/// <remarks>
		/// Other values are reserved and will be assigned to new machines as necessary.
		/// Processor-specific ELF names use the machine name to distinguish them.
		/// For example, the flags mentioned below use the prefix EF_; a flag named WIDGET for the EM_XYZ machine would be called EF_XYZ_WIDGET.
		/// </remarks>
		public enum EM : ushort
		{
			/// <summary>No machine</summary>
			NONE = 0,
			/// <summary>AT&amp;T WE 32100</summary>
			M32 = 1,
			/// <summary>SPARC</summary>
			SPARC = 2,
			/// <summary>Intel 80386</summary>
			_386 = 3,
			/// <summary>Motorola 68000</summary>
			_68K = 4,
			/// <summary>Motorola 88000</summary>
			_88K = 5,
			/// <summary>Intel 80486</summary>
			_486 = 6,
			/// <summary>Intel 80860</summary>
			_860 = 7,
			/// <summary>MIPS RS3000 Big-Endian</summary>
			MIPS = 8,
			/// <summary>MIPS RS3000 Little-Endian</summary>
			MIPS_RS3_LE = 10,
			/// <summary>RS6000</summary>
			RS6000 = 11,
			/// <summary>Hewlett-Packard PA-RISC</summary>
			PA_RISC = 15,
			/// <summary>nCUBE</summary>
			nCUBE = 16,
			/// <summary>Fujitsu VPP500</summary>
			VPP500 = 17,
			/// <summary>Sun SPARC 32+</summary>
			SPARC32PLUS = 18,
			/// <summary>Intel 80960</summary>
			_960 = 19,
			/// <summary>PowerPC</summary>
			PPC = 20,
			/// <summary>64-bit PowerPC</summary>
			PPC64 = 21,
			/// <summary>NEC V800</summary>
			V800 = 36,
			/// <summary>Fujitsu FR20</summary>
			FR20 = 37,
			/// <summary>TRW RH-32</summary>
			RH32 = 38,
			/// <summary>Motorola RCE</summary>
			RCE = 39,
			/// <summary>Advanced RISC Machines ARM</summary>
			ARM = 40,
			/// <summary>Digital Alpha</summary>
			ALPHA = 41,
			/// <summary>Hitachi SH</summary>
			SH = 42,
			/// <summary>SPARC V9</summary>
			SPARCV9 = 43,
			/// <summary>Siemens Tricore embedded processor</summary>
			TRICORE = 44,
			/// <summary>Argonaut RISC Core, Argonaut Technologies Inc.</summary>
			ARC = 45,
			/// <summary>Hitachi H8/300</summary>
			H8_300 = 46,
			/// <summary>Hitachi H8/300H</summary>
			H8_300H = 47,
			/// <summary>Hitachi H8S</summary>
			H8S = 48,
			/// <summary>Hitachi H8/500</summary>
			H8_500 = 49,
			/// <summary>Intel IA-64 processor architecture</summary>
			IA_64 = 50,
			/// <summary>Stanford MIPS-X</summary>
			MIPS_X = 51,
			/// <summary>Motorola ColdFire</summary>
			COLDFIRE = 52,
			/// <summary>Motorola M68HC12</summary>
			_68HC12 = 53,
			/// <summary>Fujitsu MMA Multimedia Accelerator</summary>
			MMA = 54,
			/// <summary>Siemens PCP</summary>
			PCP = 55,
			/// <summary>Sony nCPU embedded RISC processor</summary>
			NCPU = 56,
			/// <summary>Denso NDR1 microprocessor</summary>
			NDR1 = 57,
			/// <summary>Motorola Star*Core processor</summary>
			STARCORE = 58,
			/// <summary>Toyota ME16 processor</summary>
			ME16 = 59,
			/// <summary>STMicroelectronics ST100 processor</summary>
			ST100 = 60,
			/// <summary>Advanced Logic Corp. TinyJ embedded processor family</summary>
			TINYJ = 61,
			/// <summary>Siemens FX66 microcontroller</summary>
			FX66 = 66,
			/// <summary>STMicroelectronics ST9+ 8/16 bit microcontroller</summary>
			ST9PLUS = 67,
			/// <summary>STMicroelectronics ST7 8-bit microcontroller</summary>
			ST7 = 68,
			/// <summary>Motorola MC68HC16 Microcontroller</summary>
			_68HC16 = 69,
			/// <summary>Motorola MC68HC11 Microcontroller</summary>
			_68HC11 = 70,
			/// <summary>Motorola MC68HC08 Microcontroller</summary>
			_68HC08 = 71,
			/// <summary>Motorola MC68HC05 Microcontroller</summary>
			_68HC05 = 72,
			/// <summary>Silicon Graphics SVx</summary>
			SVX = 73,
			/// <summary>STMicroelectronics ST19 8-bit microcontroller</summary>
			ST19 = 74,
			/// <summary>Digital VAX</summary>
			VAX = 75,
			/// <summary>Axis Communications 32-bit embedded processor</summary>
			CRIS = 76,
			/// <summary>Infineon Technologies 32-bit embedded processor</summary>
			JAVELIN = 77,
			/// <summary>Element 14 64-bit DSP Processor</summary>
			FIREPATH = 78,
			/// <summary>LSI Logic 16-bit DSP Processor</summary>
			ZSP = 79,
			/// <summary>Donald Knuth's educational 64-bit processor</summary>
			MMIX = 80,
			/// <summary>Harvard University machine-independent object files</summary>
			HUANY = 81,
			/// <summary>SiTera Prism</summary>
			PRISM = 82,
		}

		/// <summary>32b ELF image header</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf32_Ehdr
		{
			/// <summary>This member identifies the object file type.</summary>
			public Elf.ET e_type;
			/// <summary>This member's value specifies the required architecture for an individual file.</summary>
			public Elf.EM e_machine;
			/// <summary>This member identifies the object file version.</summary>
			/// <remarks>
			/// The value 1 signifies the original file format; extensions will create new versions with higher numbers.
			/// The value of EV_CURRENT changes as necessary to reflect the current version number.
			/// </remarks>
			public Elf.EV e_version;
			/// <summary>This member gives the virtual address to which the system first transfers control, thus starting the process. If the file has no associated entry point, this member holds zero.</summary>
			public UInt32 e_entry;
			/// <summary>This member holds the program header table's file offset in bytes.</summary>
			/// <remarks>If the file has no program header table, this member holds zero.</remarks>
			public UInt32 e_phoff;
			/// <summary>This member holds the section header table's file offset in bytes.</summary>
			/// <remarks>If the file has no section header table, this member holds zero.</remarks>
			public UInt32 e_shoff;
			/// <summary>
			/// This member holds processor-specific flags associated with the file.
			/// Flag names take the form EF_machine_flag.
			/// </summary>
			/// <remarks>This member is presently zero for SPARC and IA.</remarks>
			public UInt32 e_flags;
			/// <summary>This member holds the ELF header's size in bytes.</summary>
			public UInt16 e_ehsize;
			/// <summary>This member holds the size in bytes of one entry in the file's program header table; all entries are the same size.</summary>
			public UInt16 e_phentsize;
			/// <summary>
			/// This member holds the number of entries in the program header table.
			/// Thus the product of e_phentsize and e_phnum gives the table's size in bytes.
			/// </summary>
			/// <remarks>If a file has no program header table, e_phnum holds the value zero.</remarks>
			public UInt16 e_phnum;
			/// <summary>
			/// This member holds a section header's size in bytes.
			/// A section header is one entry in the section header table
			/// </summary>
			/// <remarks>all entries are the same size.</remarks>
			public UInt16 e_shentsize;
			/// <summary>
			/// This member holds the number of entries in the section header table.
			/// Thus the product of e_shentsize and e_shnum gives the section header table's size in bytes.
			/// </summary>
			/// <remarks>If a file has no section header table, e_shnum holds the value zero.</remarks>
			public UInt16 e_shnum;
			/// <summary>This member holds the section header table index of the entry associated with the section name string table.</summary>
			/// <remarks>If the file has no section name string table, this member holds the value SHN_UNDEF.</remarks>
			public UInt16 e_shstrndx;

			/// <summary>Special Section Indexes</summary>
			public SHN SpecShIndex
			{
				get
				{
					SHN result = (SHN)this.e_shnum;
					return result >= SHN.UNDEF
						? result
						: SHN.UNDEF;
				}
			}
		}

		/// <summary>64b ELF image header</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf64_Ehdr
		{
			/// <summary>This member identifies the object file type.</summary>
			public Elf.ET e_type;
			/// <summary>This member's value specifies the required architecture for an individual file.</summary>
			public Elf.EM e_machine;
			/// <summary>This member identifies the object file version.</summary>
			/// <remarks>
			/// The value 1 signifies the original file format; extensions will create new versions with higher numbers.
			/// The value of EV_CURRENT changes as necessary to reflect the current version number.
			/// </remarks>
			public Elf.EV e_version;
			/// <summary>This member gives the virtual address to which the system first transfers control, thus starting the process. If the file has no associated entry point, this member holds zero.</summary>
			public UInt64 e_entry;
			/// <summary>This member holds the program header table's file offset in bytes.</summary>
			/// <remarks>If the file has no program header table, this member holds zero.</remarks>
			public UInt64 e_phoff;
			/// <summary>This member holds the section header table's file offset in bytes.</summary>
			/// <remarks>If the file has no section header table, this member holds zero.</remarks>
			public UInt64 e_shoff;
			/// <summary>
			/// This member holds processor-specific flags associated with the file.
			/// Flag names take the form EF_machine_flag.
			/// </summary>
			/// <remarks>This member is presently zero for SPARC and IA.</remarks>
			public UInt32 e_flags;
			/// <summary>This member holds the ELF header's size in bytes.</summary>
			public UInt16 e_ehsize;
			/// <summary>This member holds the size in bytes of one entry in the file's program header table; all entries are the same size.</summary>
			public UInt16 e_phentsize;
			/// <summary>
			/// This member holds the number of entries in the program header table.
			/// Thus the product of e_phentsize and e_phnum gives the table's size in bytes.
			/// </summary>
			/// <remarks>If a file has no program header table, e_phnum holds the value zero.</remarks>
			public UInt16 e_phnum;
			/// <summary>
			/// This member holds a section header's size in bytes.
			/// A section header is one entry in the section header table
			/// </summary>
			/// <remarks>all entries are the same size.</remarks>
			public UInt16 e_shentsize;
			/// <summary>
			/// This member holds the number of entries in the section header table.
			/// Thus the product of e_shentsize and e_shnum gives the section header table's size in bytes.
			/// </summary>
			/// <remarks>If a file has no section header table, e_shnum holds the value zero.</remarks>
			public UInt16 e_shnum;
			/// <summary>This member holds the section header table index of the entry associated with the section name string table.</summary>
			/// <remarks>If the file has no section name string table, this member holds the value SHN_UNDEF.</remarks>
			public UInt16 e_shstrndx;
		}

		/// <summary>Special section indexes</summary>
		public enum SHN : ushort
		{
			/// <summary>
			/// This value marks an undefined, missing, irrelevant, or otherwise meaningless section reference.
			/// For example, a symbol "defined" relative to section number SHN_UNDEF is an undefined symbol.
			/// </summary>
			UNDEF = 0,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			LOPROC = 0xff00,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			HIPROC = 0xff1f,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			LOOS=0xff20,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			HIOS = 0xff3f,
			/// <summary>
			/// This value specifies absolute values for the corresponding reference.
			/// For example, symbols defined relative to section number SHN_ABS have absolute values and are not affected by relocation.
			/// </summary>
			ABS = 0xfff1,
			/// <summary>Symbols defined relative to this section are common symbols, such as FORTRAN COMMON or unallocated C external variables.</summary>
			COMMON = 0xfff2,
			/// <summary>
			/// This value specifies the upper bound of the range of reserved indexes.
			/// The system reserves indexes between SHN_LORESERVE and SHN_HIRESERVE, inclusive; the values do not reference the section header table.
			/// The section header table does not contain entries for the reserved indexes.
			/// </summary>
			XINDEX = 0xffff,
		}

		/// <summary>Section types</summary>
		/// <remarks>Other section type values are reserved. As mentioned before, the section header for index 0 (SHN_UNDEF) exists, even though the index marks undefined section references.</remarks>
		public enum SHT : uint
		{
			/// <summary>
			/// This value marks the section header as inactive; it does not have an associated section.
			/// Other members of the section header have undefined values.
			/// </summary>
			NULL = 0,
			/// <summary>This section holds information defined by the program, whose format and meaning are determined solely by the program.</summary>
			PROGBITS = 1,
			/// <summary>
			/// These sections hold a symbol table.
			/// Typically a SHT_SYMTAB section provides symbols for link-editing.
			/// As a complete symbol table, it can contain many symbols unnecessary for dynamic linking.
			/// Consequently, an object file can also contain a <see cref="SHT.DYNSYM"/> section, which holds a minimal set of dynamic linking symbols, to save space.
			/// </summary>
			SYMTAB = 2,
			/// <summary>These sections hold a string table. An object file can have multiple string table sections.</summary>
			STRTAB = 3,
			/// <summary>This section holds relocation entries with explicit addends, such as type Elf32_Rela for the 32-bit class of object files.</summary>
			/// <remarks>An object file can have multiple relocation sections.</remarks>
			RELA = 4,
			/// <summary>This section holds a symbol hash table. All dynamically linked object files must contain a symbol hash table.</summary>
			/// <remarks>Currently, an object file can have only one hash table, but this restriction might be relaxed in the future.</remarks>
			HASH = 5,
			/// <summary>This section holds information for dynamic linking.</summary>
			/// <remarks>Currently, an object file can have only one dynamic section, but this restriction might be relaxed in the future.</remarks>
			DYNAMIC = 6,
			/// <summary>This section holds information that marks the file in some way.</summary>
			NOTE = 7,
			/// <summary>A section of this type occupies no space in the file but otherwise resembles <see cref="SHT.PROGBITS"/>.</summary>
			/// <remarks>Although this section contains no bytes, the sh_offset member contains the conceptual file offset.</remarks>
			NOBITS = 8,
			/// <summary>
			/// This section holds relocation entries without explicit addends, such as type Elf32_Rel for the 32-bit class of object files.
			/// An object file can have multiple relocation sections.
			/// </summary>
			REL = 9,
			/// <summary>This section type is reserved but has unspecified semantics. Programs that contain a section of this type do not conform to the ABI.</summary>
			SHLIB = 10,
			/// <summary>
			/// These sections hold a symbol table.
			/// Typically a <see cref="SHT.SYMTAB"/> section provides symbols for link-editing.
			/// As a complete symbol table, it can contain many symbols unnecessary for dynamic linking.
			/// Consequently, an object file can also contain a SHT.DYNSYM section, which holds a minimal set of dynamic linking symbols, to save space.
			/// </summary>
			DYNSYM = 11,
			/// <summary>This section contains an array of pointers to initialization functions.</summary>
			INIT_ARRAY=14,
			/// <summary>This section contains an array of pointers to termination functions.</summary>
			FINI_ARRAY=15,
			/// <summary>This section contains an array of pointers to functions that are invoked before all other initialization functions.</summary>
			PREINIT_ARRAY=16,
			/// <summary>
			/// This section defines a section group.
			/// A section group is a set of sections that are related and that must be treated specially by the linker (see below for further details).
			/// Sections of type SHT.GROUP may appear only in relocatable objects (objects with the ELF header e_type member set to ET_REL).
			/// The section header table entry for a group section must appear in the section header table before the entries for any of the sections that are members of the group.
			/// </summary>
			GROUP=17,
			/// <summary>
			/// This section is associated with a section of type <see cref="SHT.SYMTAB"/> and is required if any of the section header indexes referenced by that symbol table contain the escape value <see cref="SHN.XINDEX"/>.
			/// The section is an array of Elf32_Word values.
			/// Each value corresponds one to one with a symbol table entry and appear in the same order as those entries.
			/// The values represent the section header indexes against which the symbol table entries are defined.
			/// All of the values in this section must be valid section header indexes whether or not the st_shndx field in the corresponding symbol table entry contains the special escape code <see cref="SHN.XINDEX"/>.
			/// </summary>
			SYMTAB_SHNDX=18,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			LOOS=0x60000000,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			HIOS = 0x6fffffff,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			LOPROC = 0x70000000,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			HIPROC = 0x7fffffff,
			/// <summary>This value specifies the lower boundary of the range of indexes reserved for application programs.</summary>
			LOUSER = 0x80000000,
			/// <summary>
			/// This value specifies the upper boundary of the range of indexes reserved for application programs.
			/// Section types between SHT_LOUSER and SHT_HIUSER can be used by the application, without conflicting with current or future system-defined section types.
			/// </summary>
			HIUSER = 0xffffffff,
		}

		/// <summary>Section Attribute Flags</summary>
		/// <remarks>
		/// If a flag bit is set in sh_flags, the attribute is on for the section.
		/// Otherwise, the attribute is off or does not apply.
		/// Undefined attributes are reserved and set to zero.
		/// </remarks>
		[Flags]
		public enum SHF : uint
		{
			/// <summary>This section contains data that should be writable during process execution.</summary>
			WRITE=0x1,
			/// <summary>
			/// This section occupies memory during process execution.
			/// Some control sections do not reside in the memory image of an object file; this attribute is off for those sections.
			/// </summary>
			ALLOC=0x2,
			/// <summary>This section contains executable machine instructions.</summary>
			EXECINSTR=0x4,
			/// <summary>
			/// The data in the section may be merged to eliminate duplication.
			/// Unless the SHF_STRINGS flag is also set, the data elements in the section are of a uniform size.
			/// The size of each element is specified in the section header's sh_entsize field.
			/// If the SHF_STRINGS flag is also set, the data elements consist of null-terminated character strings.
			/// The size of each character is specified in the section header's sh_entsize field.
			/// </summary>
			/// <remarks>
			/// Each element in the section is compared against other elements in sections with the same name, type and flags.
			/// Elements that would have identical values at program run-time may be merged.
			/// Relocations referencing elements of such sections must be resolved to the merged locations of the referenced values.
			/// Note that any relocatable values, including values that would result in run-time relocations, must be analyzed to determine whether the run-time values would actually be identical.
			/// An ABI-conforming object file may not depend on specific elements being merged, and an ABI-conforming link editor may choose not to merge specific elements.
			/// </remarks>
			MERGE=0x10,
			/// <summary>The data elements in the section consist of null-terminated character strings. The size of each character is specified in the section header's sh_entsize field.</summary>
			STRINGS=0x20,
			/// <summary>The sh_info field of this section header holds a section header table index.</summary>
			INFO_LINK=0x40,
			/// <summary>
			/// This flag adds special ordering requirements for link editors.
			/// The requirements apply if the sh_link field of this section's header references another section (the linked-to section).
			/// If this section is combined with other sections in the output file, it must appear in the same relative order with respect to those sections, as the linked-to section appears with respect to sections the linked-to section is combined with.
			/// </summary>
			LINK_ORDER=0x80,
			/// <summary>
			/// This section requires special OS-specific processing (beyond the standard linking rules) to avoid incorrect behavior.
			/// If this section has either an sh_type value or contains sh_flags bits in the OS-specific ranges for those fields, and a link editor processing this section does not recognize those values, then the link editor should reject the object file containing this section with an error.
			/// </summary>
			OS_NONCONFORMING=0x100,
			/// <summary>
			/// This section is a member (perhaps the only one) of a section group.
			/// The section must be referenced by a section of type SHT_GROUP.
			/// The SHF_GROUP flag may be set only for sections contained in relocatable objects (objects with the ELF header e_type member set to ET_REL).
			/// </summary>
			GROUP=0x200,
			/// <summary>All bits included in this mask are reserved for operating system-specific semantics.</summary>
			MASKOS = 0x0ff00000,
			/// <summary>All bits included in this mask are reserved for processor-specific semantics. If meanings are specified, the processor supplement explains them.</summary>
			MASKPROC = 0xf0000000,
		}

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
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf32_Shdr
		{
			/// <summary>This member specifies the name of the section.</summary>
			/// <remarks>Its value is an index into the section header string table section giving the location of a null-terminated string.</remarks>
			public UInt32 sh_name;
			/// <summary>This member categorizes the section's contents and semantics.</summary>
			public SHT sh_type;
			/// <summary>Sections support 1-bit flags that describe miscellaneous attributes.</summary>
			public UInt32 sh_flags;
			/// <summary>If the section is to appear in the memory image of a process, this member gives the address at which the section's first byte should reside. Otherwise, the member contains 0.</summary>
			public UInt32 sh_addr;
			/// <summary>
			/// This member gives the byte offset from the beginning of the file to the first byte in the section.
			/// Section type SHT_NOBITS, described below, occupies no space in the file, and its sh_offset member locates the conceptual placement in the file.
			/// </summary>
			public UInt32 sh_offset;
			/// <summary>
			/// This member gives the section's size in bytes. Unless the section type is SHT_NOBITS, the section occupies sh_size bytes in the file.
			/// A section of type SHT_NOBITS can have a nonzero size, but it occupies no space in the file.
			/// </summary>
			public UInt32 sh_size;
			/// <summary>This member holds a section header table index link, whose interpretation depends on the section type.</summary>
			public UInt32 sh_link;
			/// <summary>This member holds extra information, whose interpretation depends on the section type.</summary>
			public UInt32 sh_info;
			/// <summary>
			/// Some sections have address alignment constraints.
			/// For example, if a section holds a double-word, the system must ensure double-word alignment for the entire section.
			/// That is, the value of sh_addr must be congruent to 0, modulo the value of sh_addralign.
			/// Currently, only 0 and positive integral powers of two are allowed.
			/// Values 0 and 1 mean the section has no alignment constraints.
			/// </summary>
			public UInt32 sh_addralign;
			/// <summary>
			/// Some sections hold a table of fixed-size entries, such as a symbol table.
			/// For such a section, this member gives the size in bytes of each entry.
			/// The member contains 0 if the section does not hold a table of fixed-size entries.
			/// </summary>
			public UInt32 sh_entsize;

			/// <summary>Section Attribute Flags</summary>
			/// <remarks>
			/// If a flag bit is set in sh_flags, the attribute is on for the section.
			/// Otherwise, the attribute is off or does not apply.
			/// Undefined attributes are reserved and set to zero.
			/// </remarks>
			public Elf.SHF Flags { get { return (SHF)this.sh_flags; } }

			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			public Boolean IsOs { get { return this.sh_type >= SHT.LOOS && this.sh_type <= SHT.HIOS; } }

			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			public Boolean IsProc { get { return this.sh_type >= SHT.LOPROC && this.sh_type <= SHT.HIPROC; } }

			/// <summary>Section types between SHT_LOUSER and SHT_HIUSER can be used by the application, without conflicting with current or future system-defined section types.</summary>
			public Boolean IsUser { get { return this.sh_type >= SHT.LOUSER && this.sh_type <= SHT.HIUSER; } }
		}

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
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf64_Shdr
		{
			/// <summary>This member specifies the name of the section.</summary>
			/// <remarks>Its value is an index into the section header string table section giving the location of a null-terminated string.</remarks>
			public UInt32 sh_name;
			/// <summary>This member categorizes the section's contents and semantics.</summary>
			public SHT sh_type;
			/// <summary>Sections support 1-bit flags that describe miscellaneous attributes.</summary>
			public UInt64 sh_flags;
			/// <summary>If the section is to appear in the memory image of a process, this member gives the address at which the section's first byte should reside. Otherwise, the member contains 0.</summary>
			public UInt64 sh_addr;
			/// <summary>
			/// This member gives the byte offset from the beginning of the file to the first byte in the section.
			/// Section type SHT_NOBITS, described below, occupies no space in the file, and its sh_offset member locates the conceptual placement in the file.
			/// </summary>
			public UInt64 sh_offset;
			/// <summary>
			/// This member gives the section's size in bytes. Unless the section type is SHT_NOBITS, the section occupies sh_size bytes in the file.
			/// A section of type SHT_NOBITS can have a nonzero size, but it occupies no space in the file.
			/// </summary>
			public UInt64 sh_size;
			/// <summary>This member holds a section header table index link, whose interpretation depends on the section type.</summary>
			public UInt32 sh_link;
			/// <summary>This member holds extra information, whose interpretation depends on the section type.</summary>
			public UInt32 sh_info;
			/// <summary>
			/// Some sections have address alignment constraints.
			/// For example, if a section holds a double-word, the system must ensure double-word alignment for the entire section.
			/// That is, the value of sh_addr must be congruent to 0, modulo the value of sh_addralign.
			/// Currently, only 0 and positive integral powers of two are allowed.
			/// Values 0 and 1 mean the section has no alignment constraints.
			/// </summary>
			public UInt64 sh_addralign;
			/// <summary>
			/// Some sections hold a table of fixed-size entries, such as a symbol table.
			/// For such a section, this member gives the size in bytes of each entry.
			/// The member contains 0 if the section does not hold a table of fixed-size entries.
			/// </summary>
			public UInt64 sh_entsize;

			/// <summary>Section Attribute Flags</summary>
			/// <remarks>
			/// If a flag bit is set in sh_flags, the attribute is on for the section.
			/// Otherwise, the attribute is off or does not apply.
			/// Undefined attributes are reserved and set to zero.
			/// </remarks>
			public SHF Flags { get { return (SHF)this.sh_flags; } }

			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			public Boolean IsOs
			{
				get { return this.sh_type > SHT.LOOS && this.sh_type < SHT.HIOS; }
			}

			/// <summary>Values in this inclusive range are reserved for processor-specific semantics.</summary>
			public Boolean IsProc
			{
				get { return this.sh_type > SHT.LOPROC && this.sh_type < SHT.HIPROC; }
			}

			/// <summary>Section types between SHT_LOUSER and SHT_HIUSER can be used by the application, without conflicting with current or future system-defined section types.</summary>
			public Boolean IsUser
			{
				get { return this.sh_type > SHT.LOUSER && this.sh_type < SHT.HIUSER; }
			}
		}

		/// <summary>A symbol's visibility, although it may be specified in a relocatable object, defines how that symbol may be accessed once it has become part of an executable or shared object.</summary>
		public enum STV : byte
		{
			/// <summary>
			/// The visibility of symbols with the STV_DEFAULT attribute is as specified by the symbol's binding type.
			/// That is, global and weak symbols are visible outside of their defining component (executable file or shared object).
			/// Local symbols are hidden, as described below.
			/// Global and weak symbols are also preemptable, that is, they may by preempted by definitions of the same name in another component.
			/// </summary>
			DEFAULT = 0,
			/// <summary>
			/// The meaning of this visibility attribute may be defined by processor supplements to further constrain hidden symbols.
			/// A processor supplement's definition should be such that generic tools can safely treat internal symbols as hidden.
			/// </summary>
			/// <remarks>An internal symbol contained in a relocatable object must be either removed or converted to STB_LOCAL binding by the link-editor when the relocatable object is included in an executable file or shared object.</remarks>
			INTERNAL = 1,
			/// <summary>
			/// A symbol defined in the current component is hidden if its name is not visible to other components.
			/// Such a symbol is necessarily protected.
			/// This attribute may be used to control the external interface of a component.
			/// Note that an object named by such a symbol may still be referenced from another component if its address is passed outside.
			/// </summary>
			/// <remarks>A hidden symbol contained in a relocatable object must be either removed or converted to STB_LOCAL binding by the link-editor when the relocatable object is included in an executable file or shared object.</remarks>
			HIDDEN = 2,
			/// <summary>
			/// A symbol defined in the current component is protected if it is visible in other components but not preemptable, meaning that any reference to such a symbol from within the defining component must be resolved to the definition in that component, even if there is a definition in another component that would preempt by the default rules.
			/// A symbol with STB_LOCAL binding may not have STV_PROTECTED visibility.
			/// If a symbol definition with STV_PROTECTED visibility from a shared object is taken as resolving a reference from an executable or another shared object, the SHN_UNDEF symbol table entry created has STV_DEFAULT visibility.
			/// </summary>
			PROTECTED = 3,
		}

		/// <summary>A symbol's binding determines the linkage visibility and behavior</summary>
		public enum STB : byte
		{
			/// <summary>
			/// Local symbols are not visible outside the object file containing their definition.
			/// Local symbols of the same name can exist in multiple files without interfering with each other.
			/// </summary>
			LOCAL = 0,
			/// <summary>
			/// Global symbols are visible to all object files being combined.
			/// One file's definition of a global symbol will satisfy another file's undefined reference to the same global symbol.
			/// </summary>
			GLOBAL = 1,
			/// <summary>Weak symbols resemble global symbols, but their definitions have lower precedence.</summary>
			WEAK = 2,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			LOOS=10,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			HIOS = 12,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics. If meanings are specified, the processor supplement explains them.</summary>
			LOPROC = 13,
			/// <summary>Values in this inclusive range are reserved for processor-specific semantics. If meanings are specified, the processor supplement explains them.</summary>
			HIPROC = 15,
		}

		/// <summary>A symbol's type provides a general classification for the associated entity.</summary>
		public enum STT : byte
		{
			/// <summary>The symbol's type is not specified.</summary>
			NOTYPE = 0,
			/// <summary>The symbol is associated with a data object, such as a variable, an array, and so on.</summary>
			OBJECT = 1,
			/// <summary>The symbol is associated with a function or other executable code.</summary>
			/// <remarks>
			/// Function symbols (those with type STT_FUNC) in shared object files have special significance.
			/// When another object file references a function from a shared object, the link editor automatically creates a procedure linkage table entry for the referenced symbol.
			/// Shared object symbols with types other than STT_FUNC will not be referenced automatically through the procedure linkage table.
			/// </remarks>
			FUNC = 2,
			/// <summary>The symbol is associated with a section. Symbol table entries of this type exist primarily for relocation and normally have STB_LOCAL binding.</summary>
			SECTION = 3,
			/// <summary>
			/// Conventionally, the symbol's name gives the name of the source file associated with the object file.
			/// A file symbol has STB_LOCAL binding, its section index is SHN_ABS, and it precedes the other STB_LOCAL symbols for the file, if it is present.
			/// </summary>
			FILE = 4,
			/// <summary>The symbol labels an uninitialized common block.</summary>
			/// <remarks>
			/// Symbols with type STT_COMMON label uninitialized common blocks.
			/// In relocatable objects, these symbols are not allocated and must have the special section index SHN_COMMON (see below).
			/// In shared objects and executables these symbols must be allocated to some section in the defining object.
			/// 
			/// In relocatable objects, symbols with type STT_COMMON are treated just as other symbols with index SHN_COMMON.
			/// If the link-editor allocates space for the SHN_COMMON symbol in an output section of the object it is producing, it must preserve the type of the output symbol as STT_COMMON.
			/// 
			/// When the dynamic linker encounters a reference to a symbol that resolves to a definition of type STT_COMMON, it may (but is not required to) change its symbol resolution rules as follows: instead of binding the reference to the first symbol found with the given name, the dynamic linker searches for the first symbol with that name with type other than STT_COMMON.
			/// If no such symbol is found, it looks for the STT_COMMON definition of that name that has the largest size.
			/// </remarks>
			COMMON = 5,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			LOOS = 10,
			/// <summary>Values in this inclusive range are reserved for operating system-specific semantics.</summary>
			HIOS = 12,
			/// <summary>
			/// Values in this inclusive range are reserved for processor-specific semantics.
			/// If meanings are specified, the processor supplement explains them.
			/// </summary>
			LOPROC = 13,
			/// <summary>
			/// Values in this inclusive range are reserved for processor-specific semantics.
			/// If meanings are specified, the processor supplement explains them.
			/// </summary>
			HIPROC = 15,
		}

		/// <summary>
		/// An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references. A symbol table index is a subscript into this array.
		/// Index 0 both designates the first entry in the table and serves as the undefined symbol index.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf32_Sym
		{
			/// <summary>This member holds an index into the object file's symbol string table, which holds the character representations of the symbol names.</summary>
			/// <remarks>If the value is non-zero, it represents a string table index that gives the symbol name. Otherwise, the symbol table entry has no name.</remarks>
			public UInt32 st_name;
			/// <summary>
			/// This member gives the value of the associated symbol.
			/// Depending on the context, this may be an absolute value, an address, and so on.
			/// </summary>
			public UInt32 st_value;
			/// <summary>
			/// Many symbols have associated sizes.
			/// For example, a data object's size is the number of bytes contained in the object.
			/// </summary>
			/// <remarks>This member holds 0 if the symbol has no size or an unknown size.</remarks>
			public UInt32 st_size;
			/// <summary>This member specifies the symbol's type and binding attributes.</summary>
			public Byte st_info;
			/// <summary>This member currently specifies a symbol's visibility.</summary>
			public Byte st_other;
			/// <summary>
			/// Every symbol table entry is defined in relation to some section.
			/// This member holds the relevant section header table index.
			/// As the sh_link and sh_info interpretation table and the related text describe, some section indexes indicate special meanings.
			/// </summary>
			/// <remarks>
			/// If this member contains SHN_XINDEX, then the actual section header index is too large to fit in this field.
			/// The actual value is contained in the associated section of type SHT_SYMTAB_SHNDX.
			/// </remarks>
			public UInt16 st_shndx;

			/// <summary>A symbol's binding determines the linkage visibility and behavior</summary>
			public STB Bind { get { return (STB)(this.st_info >> 4); } }

			/// <summary>A symbol's type provides a general classification for the associated entity.</summary>
			public STT Type { get { return (STT)(this.st_info & 0xf); } }

			/// <summary>Symbol visibility</summary>
			public STV Visibility { get { return (STV)(this.st_other & 0x3); } }
		}

		/// <summary>
		/// An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references. A symbol table index is a subscript into this array.
		/// Index 0 both designates the first entry in the table and serves as the undefined symbol index.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf64_Sym
		{
			/// <summary>This member holds an index into the object file's symbol string table, which holds the character representations of the symbol names.</summary>
			/// <remarks>If the value is non-zero, it represents a string table index that gives the symbol name. Otherwise, the symbol table entry has no name.</remarks>
			public UInt32 st_name;

			/// <summary>This member specifies the symbol's type and binding attributes.</summary>
			public Byte st_info;
			/// <summary>This member currently specifies a symbol's visibility.</summary>
			public Byte st_other;
			/// <summary>
			/// Every symbol table entry is defined in relation to some section.
			/// This member holds the relevant section header table index.
			/// As the sh_link and sh_info interpretation table and the related text describe, some section indexes indicate special meanings.
			/// </summary>
			/// <remarks>
			/// If this member contains SHN_XINDEX, then the actual section header index is too large to fit in this field.
			/// The actual value is contained in the associated section of type SHT_SYMTAB_SHNDX.
			/// </remarks>
			public UInt16 st_shndx;
			/// <summary>
			/// This member gives the value of the associated symbol.
			/// Depending on the context, this may be an absolute value, an address, and so on.
			/// </summary>
			public UInt64 st_value;
			/// <summary>
			/// Many symbols have associated sizes.
			/// For example, a data object's size is the number of bytes contained in the object.
			/// </summary>
			/// <remarks>This member holds 0 if the symbol has no size or an unknown size.</remarks>
			public UInt64 st_size;

			/// <summary>A symbol's binding determines the linkage visibility and behavior</summary>
			public STB Bind { get { return (STB)(this.st_info >> 4); } }

			/// <summary>A symbol's type provides a general classification for the associated entity.</summary>
			public STT Type { get { return (STT)(this.st_info & 0xf); } }

			/// <summary>Symbol visibility</summary>
			public STV Visibility { get { return (STV)(this.st_other & 0x3); } }
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf32_Rel
		{
			/// <summary>
			/// This member gives the location at which to apply the relocation action.
			/// For a relocatable file, the value is the byte offset from the beginning of the section to the storage unit affected by the relocation.
			/// For an executable file or a shared object, the value is the virtual address of the storage unit affected by the relocation.
			/// </summary>
			public UInt32 r_offset;

			/// <summary>
			/// This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.
			/// For example, a call instruction's relocation entry will hold the symbol table index of the function being called.
			/// If the index is STN_UNDEF, the undefined symbol index, the relocation uses 0 as the symbol value.
			/// </summary>
			public UInt32 r_info;
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf64_Rel
		{
			/// <summary>
			/// This member gives the location at which to apply the relocation action.
			/// For a relocatable file, the value is the byte offset from the beginning of the section to the storage unit affected by the relocation.
			/// For an executable file or a shared object, the value is the virtual address of the storage unit affected by the relocation.
			/// </summary>
			public UInt64 r_offset;

			/// <summary>
			/// This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.
			/// For example, a call instruction's relocation entry will hold the symbol table index of the function being called.
			/// If the index is STN_UNDEF, the undefined symbol index, the relocation uses 0 as the symbol value.
			/// </summary>
			public UInt64 r_info;
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf32_Rela
		{
			/// <summary>
			/// This member gives the location at which to apply the relocation action.
			/// For a relocatable file, the value is the byte offset from the beginning of the section to the storage unit affected by the relocation.
			/// For an executable file or a shared object, the value is the virtual address of the storage unit affected by the relocation.
			/// </summary>
			public UInt32 r_offset;

			/// <summary>
			/// This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.
			/// For example, a call instruction's relocation entry will hold the symbol table index of the function being called.
			/// If the index is STN_UNDEF, the undefined symbol index, the relocation uses 0 as the symbol value.
			/// </summary>
			public UInt32 r_info;

			/// <summary>This member specifies a constant addend used to compute the value to be stored into the relocatable field.</summary>
			public Int32 r_addend;
		}

		/// <summary>
		/// Relocation is the process of connecting symbolic references with symbolic definitions.
		/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
		/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Elf64_Rela
		{
			/// <summary>
			/// This member gives the location at which to apply the relocation action.
			/// For a relocatable file, the value is the byte offset from the beginning of the section to the storage unit affected by the relocation.
			/// For an executable file or a shared object, the value is the virtual address of the storage unit affected by the relocation.
			/// </summary>
			public UInt64 r_offset;

			/// <summary>
			/// This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.
			/// For example, a call instruction's relocation entry will hold the symbol table index of the function being called.
			/// If the index is STN_UNDEF, the undefined symbol index, the relocation uses 0 as the symbol value.
			/// </summary>
			public UInt64 r_info;

			/// <summary>This member specifies a constant addend used to compute the value to be stored into the relocatable field.</summary>
			public Int64 r_addend;
		}
	}
}