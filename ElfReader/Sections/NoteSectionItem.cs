using System;
using System.Diagnostics;

namespace AlphaOmega.Debug
{
	/// <summary>Sometimes a vendor or system builder needs to mark an object file with special information that other programs will check for conformance, compatibility, and so forth</summary>
	[DebuggerDisplay("Name={"+nameof(name)+"} Descriptor={"+nameof(descriptor)+"}")]
	public readonly struct NoteSectionItem
	{
		/// <summary>Flags</summary>
		public readonly UInt64 type;

		/// <summary>Key</summary>
		public readonly String name;

		/// <summary>Value</summary>
		public readonly String descriptor;

		internal NoteSectionItem(UInt64 type, String name, String descriptor)
		{
			this.type = type;
			this.name = name;
			this.descriptor = descriptor;
		}
	}
}