using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AlphaOmega.Debug.Properties;

namespace AlphaOmega.Debug
{
	/// <summary>An object file's symbol table holds information needed to locate and relocate a program's symbolic definitions and references. A symbol table index is a subscript into this array</summary>
	/// <remarks>Index 0 both designates the first entry in the table and serves as the undefined symbol index</remarks>
	public class SymbolSection : SectionBase, IEnumerable<SymbolSectionItem>
	{//https://blogs.oracle.com/ali/entry/inside_elf_symbol_tables
		//https://blogs.oracle.com/ali/entry/gnu_hash_elf_sections
		private StringSection _sectionSymbolNames;

		private UInt64 SizeOfStruct
		{
			get
			{
				return base.Section.File.Header.Is64Bit
					? (UInt64)Marshal.SizeOf(typeof(Elf.Elf64_Sym))
					: (UInt64)Marshal.SizeOf(typeof(Elf.Elf32_Sym));
			}
		}

		private StringSection SectionSymbolNames
		{
			get
			{
				if(this._sectionSymbolNames == null)
				{
					String sectionName;
					switch(base.Section.sh_type)
					{
					case Elf.SHT.SYMTAB:
						sectionName = Constant.SectionNames.StrTab;
						break;
					case Elf.SHT.DYNSYM:
						sectionName = Constant.SectionNames.DynStr;
						break;
					default:
						throw new NotImplementedException(String.Format(Resources.errNoStringConstantArg1, base.Section.sh_type));
					}

					foreach(StringSection section in this.Section.File.GetStringSections())
						if(section.Section.Name == sectionName)
						{
							this._sectionSymbolNames = section;
							break;
						}

					if(this._sectionSymbolNames == null)
						throw new InvalidOperationException(String.Format(Resources.errSectionNotFoundArg1, sectionName));
				}

				return this._sectionSymbolNames;
			}
		}

		internal SymbolSection(Section section)
			: base(section)
		{
			if(section.sh_type != Elf.SHT.SYMTAB && section.sh_type != Elf.SHT.DYNSYM)
				throw new ArgumentException(String.Format(Resources.errUnsupportedSectionArg1, String.Join("|", new String[] { Elf.SHT.SYMTAB.ToString(), Elf.SHT.DYNSYM.ToString(), })));
		}

		/// <summary>Get all symbols in the section</summary>
		/// <returns>Stream of all symbols in the current section</returns>
		public IEnumerator<SymbolSectionItem> GetEnumerator()
		{
			//Byte[] payload = this._section.GetData();

			UInt64 offset = base.Section.sh_offset;
			UInt64 maxOffset = base.Section.sh_offset + base.Section.sh_size;
			UInt64 sizeOfStruct = this.SizeOfStruct;
			UInt64 itemsCount = (maxOffset - offset) / sizeOfStruct;

			ElfHeader header = base.Section.File.Header;
			StringSection stringTable = this.SectionSymbolNames;
			Boolean is64Bit = header.Is64Bit;

			for(UInt64 loop = 0; loop < itemsCount; loop++)
			{
				SymbolSectionItem result;
				if(is64Bit)
				{
					Elf.Elf64_Sym symbol = header.PtrToStructure<Elf.Elf64_Sym>(offset);
					result = new SymbolSectionItem(symbol);
				} else
				{
					Elf.Elf32_Sym symbol = header.PtrToStructure<Elf.Elf32_Sym>(offset);
					result = new SymbolSectionItem(symbol);
				}

				result.Name = stringTable[result.st_name];

				yield return result;
				offset += sizeOfStruct;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}