using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// Relocation is the process of connecting symbolic references with symbolic definitions.
	/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
	/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
	/// </summary>
	public class RelocationASection : SectionBase, IEnumerable<RelocationSectionItem>
	{
		private UInt64 SizeOfStruct
			=> base.Section.File.Header.Is64Bit
				? (UInt64)Marshal.SizeOf(typeof(Elf.Elf64_Rela))
				: (UInt64)Marshal.SizeOf(typeof(Elf.Elf32_Rela));

		internal RelocationASection(Section section)
			: base(section, Elf.SHT.RELA)
		{ }

		/// <summary>Get all relocations with addendum constant in the section</summary>
		/// <returns>Stream of all relocations with addendum constant from current section</returns>
		public IEnumerator<RelocationSectionItem> GetEnumerator()
		{
			UInt64 offset = base.Section.sh_offset;
			UInt64 maxOffset = base.Section.sh_offset + base.Section.sh_size;
			UInt64 sizeOfStruct = this.SizeOfStruct;
			UInt64 itemsCount = (maxOffset - offset) / sizeOfStruct;

			ElfHeader header = base.Section.File.Header;
			Boolean is64Bit = header.Is64Bit;

			for(UInt64 loop = 0; loop < itemsCount; loop++)
			{
				if(is64Bit)
				{
					Elf.Elf64_Rela rel = header.PtrToStructure<Elf.Elf64_Rela>(offset);
					yield return new RelocationASectionItem(rel);
				} else
				{
					Elf.Elf32_Rela rel = header.PtrToStructure<Elf.Elf32_Rela>(offset);
					yield return new RelocationASectionItem(rel);
				}
				offset += sizeOfStruct;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}