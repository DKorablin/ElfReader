using System;

namespace AlphaOmega.Debug
{
	/// <summary>Relocation is the process of connecting symbolic references with symbolic definitions.</summary>
	/// <remarks>
	/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
	/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
	/// </remarks>
	public class RelocationASectionItem : RelocationSectionItem
	{
		/// <summary>This member specifies a constant addend used to compute the value to be stored into the relocatable field</summary>
		public readonly Int64 r_addend;

		internal RelocationASectionItem(Elf.Elf32_Rela relocation)
			: base(relocation.r_offset, relocation.r_info)
			=> this.r_addend = relocation.r_addend;

		internal RelocationASectionItem(Elf.Elf64_Rela relocation)
			: base(relocation.r_offset, relocation.r_info)
			=> this.r_addend = relocation.r_addend;
	}
}