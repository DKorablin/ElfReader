using System;
using System.Diagnostics;

namespace AlphaOmega.Debug
{
	/// <summary>String tables string constant</summary>
	[DebuggerDisplay("Index={" + nameof(Index) + "} Name={" + nameof(Name) + "}")]
	public class StringSectionItem
	{
		/// <summary>String constant index</summary>
		public UInt32 Index { get; }

		/// <summary>String constant from string table section</summary>
		public String Name { get; }

		internal StringSectionItem(UInt32 index, String name)
		{
			this.Index = index;
			this.Name = name;
		}
	}
}