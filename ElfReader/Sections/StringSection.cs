using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlphaOmega.Debug
{
	/// <summary>
	/// String table sections hold null-terminated character sequences, commonly called strings.
	/// The object file uses these strings to represent symbol and section names.
	/// One references a string as an index into the string table section.
	/// </summary>
	public class StringSection : SectionBase, IEnumerable<StringSectionItem>
	{
		private SortedList<UInt32, String> _data;

		/// <summary>Gets string constant by index</summary>
		/// <param name="index">Index of the string constant</param>
		/// <returns>String constant or null</returns>
		public String this[UInt32 index]
		{
			get
			{
				SortedList<UInt32, String> strings = this.GetData();
				return strings.TryGetValue(index, out String result)
					? result
					: this.GetDataByPointer(index);
			}
		}

		/// <summary>Create instance of the string table section</summary>
		/// <param name="section">String table section. section.st_type must be equal to Elf.SHT.STRTAB</param>
		internal StringSection(Section section)
			: base(section, Elf.SHT.STRTAB)
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

		private String GetDataByPointer(UInt32 pointer)
		{
			SortedList<UInt32, String> data = this.GetData();
			UInt32 key = pointer;
			String nearestString = null;
			while(key >= 0 && !data.TryGetValue(key, out nearestString))
				key--;

			UInt32 diff = pointer - key;

			//TODO: Here i can add found string to the SortedList<,>
			String result = nearestString?.Substring((Int32)diff, (Int32)(nearestString.Length - diff));
			return result;
		}

		/// <summary>Gets all string with indexes</summary>
		/// <returns>Index and Values</returns>
		public IEnumerator<StringSectionItem> GetEnumerator()
		{
			foreach(KeyValuePair<UInt32, String> item in this.GetData())
				yield return new StringSectionItem(item.Key, item.Value);
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}