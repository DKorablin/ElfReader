
using System;

namespace AlphaOmega.Debug
{
	internal struct Constant
	{
		/// <summary>Special sections</summary>
		/// <remarks>
		/// Various sections hold program and control information.
		/// Sections in the list below are used by the system and have the indicated types and attributes.
		/// </remarks>
		public struct SectionNames
		{
			/// <summary>
			/// This section holds uninitialized data that contribute to the program’s memory image.
			/// By definition, the system initializes the data with zeros when the program begins to run.
			/// The section occupies no file space, as indicated by the section type, SHT_NOBITS.
			/// </summary>
			public const String Bss = ".bss";
			/// <summary>This section holds version control information.</summary>
			public const String Comment = ".comment";
			/// <summary>These sections hold initialized data that contribute to the program’s memory image.</summary>
			public const String Data = ".data";
			/// <summary>These sections hold initialized data that contribute to the program’s memory image.</summary>
			public const String Data1 = ".data1";
			/// <summary>
			/// This section holds information for symbolic debugging.
			/// The contents are unspecified.
			/// </summary>
			public const String Debug = ".debug";

			/// <summary>
			/// This section contains information about user readable objects for debugging.
			/// This contents are unspecified.
			/// </summary>
			public const String DebugString = ".debug_str";
			/// <summary>
			/// This section holds dynamic linking information.
			/// The section’s attributes will include the SHF_ALLOC bit.
			/// Whether the SHF_WRITE bit is set is processor specific.
			/// </summary>
			public const String Dynamic = ".dynamic";
			/// <summary>This section holds strings needed for dynamic linking, most commonly the strings that represent the names associated with symbol table entries.</summary>
			public const String DynStr = ".dynstr";
			/// <summary>This section holds the dynamic linking symbol table.</summary>
			public const String DynSym = ".dynsym";
			/// <summary>
			/// This section holds executable instructions that contribute to the process termination code.
			/// That is, when a program exits normally, the system arranges to execute the code in this section.
			/// </summary>
			public const String Fini = ".fini";
			/// <summary>This section holds the global offset table.</summary>
			public const String Got = ".got";
			/// <summary>This section holds a symbol hash table.</summary>
			public const String Hash = ".hash";
			/// <summary>
			/// This section holds executable instructions that contribute to the process initialization code.
			/// That is, when a program starts to run, the system arranges to execute the code in this section before calling the main program entry point (called main for C programs)
			/// </summary>
			public const String Init = ".init";
			/// <summary>
			/// This section holds the path name of a program interpreter.
			/// If the file has a loadable segment that includes the section, the section’s attributes will include the SHF_ALLOC bit; otherwise, that bit will be off.
			/// </summary>
			public const String Interp = ".interp";
			/// <summary>
			/// This section holds line number information for symbolic debugging, which describes the correspondence between the source program and the machine code.
			/// The contents are unspecified.
			/// </summary>
			public const String Line = ".line";
			public const String Note = ".note";
			/// <summary>This section holds the procedure linkage table.</summary>
			public const String Plt = ".plt";
			/// <summary>
			/// These sections hold relocation information.
			/// If the file has a loadable segment that includes relocation, the sections’ attributes will include the SHF_ALLOC bit; otherwise, that bit will be off. Conventionally, name is supplied by the section to which the relocations apply. Thus a relocation section for .text normally would have the name .rel.text or .rela.text.
			/// </summary>
			public const String RelArg1 = ".rel.{0}";
			/// <summary>
			/// These sections hold relocation information.
			/// If the file has a loadable segment that includes relocation, the sections’ attributes will include the SHF_ALLOC bit; otherwise, that bit will be off. Conventionally, name is supplied by the section to which the relocations apply. Thus a relocation section for .text normally would have the name .rel.text or .rela.text.
			/// </summary>
			public const String RelaArg1 = ".rela.{0}";
			/// <summary>These sections hold read-only data that typically contribute to a non-writable segment in the process image.</summary>
			public const String Rodata = ".rodata";
			/// <summary>These sections hold read-only data that typically contribute to a non-writable segment in the process image.</summary>
			public const String Rodata1 = ".rodata1";
			/// <summary>This section holds section names.</summary>
			public const String Shstrtab = ".shstrtab";
			/// <summary>
			/// This section holds strings, most commonly the strings that represent the names associated with symbol table entries.
			/// If the file has a loadable segment that includes the symbol string table, the section’s attributes will include the SHF_ALLOC bit; otherwise, that bit will be off.
			/// </summary>
			public const String StrTab = ".strtab";
			/// <summary>
			/// This section holds a symbol table, as ‘‘Symbol Table’’ in this section describes.
			/// If the file has a loadable segment that includes the symbol table, the section’s attributes will include the SHF_ALLOC bit; otherwise, that bit will be off.
			/// </summary>
			public const String SymTab = ".symtab";
			/// <summary>This section holds the ‘‘text,’’ or executable instructions, of a program.</summary>
			public const String Text = ".text";
		}
	}
}