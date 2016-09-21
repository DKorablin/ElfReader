using System;
using System.Diagnostics;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references. A symbol table index is a subscript into this array.
	/// Index 0 both designates the first entry in the table and serves as the undefined symbol index.
	/// </summary>
	[DebuggerDisplay("SecRef={st_shndx} NameRef={Name}")]
	public class SymbolSectionItem
	{
		/// <summary>This member holds an index into the object file's symbol string table, which holds the character representations of the symbol names.</summary>
		/// <remarks>If the value is non-zero, it represents a string table index that gives the symbol name. Otherwise, the symbol table entry has no name.</remarks>
		public readonly UInt32 st_name;

		/// <summary>This member specifies the symbol's type and binding attributes.</summary>
		public readonly Byte st_info;

		/// <summary>This member currently specifies a symbol's visibility.</summary>
		public readonly Byte st_other;

		/// <summary>
		/// Every symbol table entry is defined in relation to some section.
		/// This member holds the relevant section header table index.
		/// As the sh_link and sh_info interpretation table and the related text describe, some section indexes indicate special meanings.
		/// </summary>
		/// <remarks>
		/// If this member contains SHN_XINDEX, then the actual section header index is too large to fit in this field.
		/// The actual value is contained in the associated section of type SHT_SYMTAB_SHNDX.
		/// </remarks>
		public readonly UInt16 st_shndx;

		/// <summary>
		/// This member gives the value of the associated symbol.
		/// Depending on the context, this may be an absolute value, an address, and so on.
		/// </summary>
		public readonly UInt64 st_value;

		/// <summary>
		/// Many symbols have associated sizes.
		/// For example, a data object's size is the number of bytes contained in the object.
		/// </summary>
		/// <remarks>This member holds 0 if the symbol has no size or an unknown size.</remarks>
		public readonly UInt64 st_size;

		/// <summary>A symbol's binding determines the linkage visibility and behavior</summary>
		public Elf.STB Bind { get { return (Elf.STB)(this.st_info >> 4); } }

		/// <summary>A symbol's type provides a general classification for the associated entity.</summary>
		public Elf.STT Type { get { return (Elf.STT)(this.st_info & 0xf); } }

		/// <summary>Symbol visibility</summary>
		public Elf.STV Visibility { get { return (Elf.STV)(this.st_other & 0x3); } }

		/// <summary>Character representation of the symbol name</summary>
		public String Name { get; internal set; }

		internal SymbolSectionItem(Elf.Elf32_Sym item)
		{
			this.st_name = item.st_name;
			this.st_info = item.st_info;
			this.st_other = item.st_other;
			this.st_shndx = item.st_shndx;
			this.st_value = item.st_value;
			this.st_size = item.st_size;
		}

		internal SymbolSectionItem(Elf.Elf64_Sym item)
		{
			this.st_name = item.st_name;
			this.st_info = item.st_info;
			this.st_other = item.st_other;
			this.st_shndx = item.st_shndx;
			this.st_value = item.st_value;
			this.st_size = item.st_size;
		}
	}
}