using System;
using System.Diagnostics;

namespace AlphaOmega.Debug
{
	/// <summary>String tables string constant</summary>
	[DebuggerDisplay("Index={Index} Name={Name}")]
	public class StringSectionItem
	{
		private readonly UInt32 _index;
		private readonly String _name;

		/// <summary>String constant index</summary>
		public UInt32 Index { get { return this._index; } }

		/// <summary>String constant from string table section</summary>
		public String Name { get { return this._name; } }

		internal StringSectionItem(UInt32 index, String name)
		{
			this._index = index;
			this._name = name;
		}
	}
}