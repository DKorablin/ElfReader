using System;
using AlphaOmega.Debug.Properties;

namespace AlphaOmega.Debug
{
	/// <summary>Basic section information for the strongly typed sections</summary>
	public class SectionBase
	{
		/// <summary>ELF section information</summary>
		public Section Section { get; }

		/// <summary>Create instance of the basic section information with section type check</summary>
		/// <param name="section">ELF section information</param>
		/// <param name="type">Required section type</param>
		/// <exception cref="ArgumentException">section st_type is not equals with <paramref name="type"/></exception>
		internal SectionBase(Section section, Elf.SHT type)
			: this(section)
		{
			if(section.sh_type != type)
				throw new ArgumentException(String.Format(Resources.errUnsupportedSectionArg1, type));
		}

		/// <summary>Create instance of the basic section information</summary>
		/// <param name="section">ELF section information</param>
		/// <exception cref="ArgumentNullException">Section is null</exception>
		internal SectionBase(Section section)
			=> this.Section = section ?? throw new ArgumentNullException(nameof(section));
	}
}