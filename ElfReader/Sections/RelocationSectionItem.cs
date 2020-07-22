using System;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// Relocation is the process of connecting symbolic references with symbolic definitions.
	/// For example, when a program calls a function, the associated call instruction must transfer control to the proper destination address at execution.
	/// In other words, relocatable files must have information that describes how to modify their section contents, thus allowing executable and shared object files to hold the right information for a process's program image.
	/// </summary>
	public class RelocationSectionItem
	{
		/// <summary>
		/// This member gives the location at which to apply the relocation action.
		/// For a relocatable file, the value is the byte offset from the beginning of the section to the storage unit affected by the relocation.
		/// For an executable file or a shared object, the value is the virtual address of the storage unit affected by the relocation.
		/// </summary>
		public readonly UInt64 r_offset;

		/// <summary>
		/// This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.
		/// For example, a call instruction's relocation entry will hold the symbol table index of the function being called.
		/// If the index is STN_UNDEF, the undefined symbol index, the relocation uses 0 as the symbol value.
		/// </summary>
		public readonly UInt64 r_info;

		internal RelocationSectionItem(Elf.Elf32_Rel relocation)
			: this(relocation.r_offset, relocation.r_info)
		{ }

		internal RelocationSectionItem(Elf.Elf64_Rel relocation)
			: this(relocation.r_offset, relocation.r_info)
		{ }

		/// <summary>Create instance of the relocation section item information (Used for .rela section)</summary>
		/// <param name="r_offset">This member gives the location at which to apply the relocation action.</param>
		/// <param name="r_info">This member gives both the symbol table index, with respect to which the relocation must be made, and the type of relocation to apply.</param>
		protected internal RelocationSectionItem(UInt64 r_offset, UInt64 r_info)
		{
			this.r_offset = r_offset;
			this.r_info = r_info;
		}
	}
}