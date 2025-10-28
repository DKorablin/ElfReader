using System;
using System.Collections;
using System.Collections.Generic;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// Sometimes a vendor or system builder needs to mark an object file with special information that other programs will check for conformance, compatibility, and so forth.
	/// Sections of type SHT_NOTE and program header elements of type PT_NOTE can be used for this purpose.
	/// </summary>
	/// <remarks>The note information in sections and program header elements holds any number of entries, each of which is an array of 4-byte words in the format of the target processor.</remarks>
	public class NoteSection : SectionBase, IEnumerable<NoteSectionItem>
	{
		internal NoteSection(Section section)
			: base(section, Elf.SHT.NOTE)
		{ }

		/// <summary>Get all notes from current section</summary>
		/// <returns>Stream of notes from current section</returns>
		public IEnumerator<NoteSectionItem> GetEnumerator()
		{
			UInt64 offset = base.Section.sh_offset;
			UInt64 maxOffset = offset + base.Section.sh_size;
			ElfHeader header = base.Section.File.Header;

			while(offset < maxOffset)
			{
				UInt64 nameSz = header.ReadInt(ref offset);
				if(nameSz == 0)
					break;
				if(nameSz + offset > maxOffset)
					break;//TODO:mystem.so -> ...overflow error... Int32/Int64 (?)

				UInt64 descSz = header.ReadInt(ref offset);

				UInt64 type = header.ReadInt(ref offset);

				String name = header.PtrToStringAnsi(offset);
				offset += nameSz;

				String descriptor = header.PtrToStringAnsi(offset);
				offset += descSz;

				UInt64 p = header.SizeOfInt;
				UInt64 padding = (offset % p) != 0 ? (p - (offset % p)) : 0;
				offset += padding;//TODO: Check it

				yield return new NoteSectionItem(type, name, descriptor);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}