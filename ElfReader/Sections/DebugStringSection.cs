using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlphaOmega.Debug
{
	/// <summary>This section contains information about user readable objects for debugging</summary>
	public class DebugStringSection : SectionBase,IEnumerable<StringSectionItem>
	{
		private SortedList<UInt32, String> _data;

		internal DebugStringSection(Section section)
			: base(section, Elf.SHT.PROGBITS)
		{ }

		private SortedList<UInt32, String> GetData()
		{
			if(this._data == null)
			{
				Byte[] bytes = base.Section.GetData();
				UInt32 ptr = 0;

				SortedList<UInt32, String> strings = new SortedList<UInt32, String>();
				for(UInt32 loop = 0; loop < bytes.Length; loop++)
				{
					if(bytes[loop] == 0)
					{
						String str = Encoding.ASCII.GetString(bytes, (Int32)ptr, (Int32)(loop - ptr));

						strings.Add(ptr, str);
						ptr = loop + 1;
					}
				}

				this._data = strings;
			}

			return this._data;
		}

		/// <summary>Get all debug symbols names from current section</summary>
		/// <remarks>TODO: Undocumented section</remarks>
		/// <returns>Stream of debug symbol names from current section</returns>
		public IEnumerator<StringSectionItem> GetEnumerator()
		{
			foreach(KeyValuePair<UInt32, String> item in this.GetData())
				yield return new StringSectionItem(item.Key, item.Value);
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}