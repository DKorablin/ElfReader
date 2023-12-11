using System;
using System.Runtime.InteropServices;
using AlphaOmega.Debug.Properties;

namespace AlphaOmega.Debug
{
	/// <summary>ELF header data information</summary>
	public class ElfHeader : IDisposable
	{
		private static readonly UInt32 SizeOfIdentity = (UInt32)Marshal.SizeOf(typeof(Elf.EI));
		private Elf.EI? _identity;
		private Header _header;

		/// <summary>Image loader interface</summary>
		public IImageLoader Loader { get; private set; }

		/// <summary>ELF file identification information</summary>
		public Elf.EI Identification
		{
			get
			{
				if(this._identity == null)
				{
					this._identity = this.Loader.PtrToStructure<Elf.EI>(0);
					if(this._identity.Value.IsValid)
						switch(this._identity.Value.data)
						{
						case Elf.ELFDATA._2LSB:
							this.Loader.Endianness = EndianHelper.Endian.Little;
							break;
						case Elf.ELFDATA._2MSB:
							this.Loader.Endianness = EndianHelper.Endian.Big;
							break;
						default:
							throw new NotImplementedException();
						}
				}
				return this._identity.Value;
			}
		}

		/// <summary>This file contains valid ELF header</summary>
		public Boolean IsValid => this.Identification.IsValid;

		/// <summary>This file for 64-byte address space</summary>
		public Boolean Is64Bit
			=> this.Identification._class == Elf.ELFCLASS.CLASS64;

		internal UInt64 SizeOfInt
			=> (UInt64)(this.Is64Bit ? sizeof(UInt64) : sizeof(UInt32));

		/// <summary>ELF file header</summary>
		public Header Header
		{
			get
			{
				if(this._header == null)
				{
					this._header = this.Is64Bit
						? new Header(this.GetHeader<Elf.Elf64_Ehdr>())
						: new Header(this.GetHeader<Elf.Elf32_Ehdr>());
				}
				return this._header;
			}
		}

		/// <summary>Create instance of the ELF basic data information</summary>
		/// <param name="loader">Stream of data</param>
		public ElfHeader(IImageLoader loader)
		{
			this.Loader = loader ?? throw new ArgumentNullException(nameof(loader));
			this.Loader.Endianness = EndianHelper.Endianness;
		}

		private T GetHeader<T>() where T : struct
			=> this.Identification.IsValid
				? this.Loader.PtrToStructure<T>(ElfHeader.SizeOfIdentity)
				: throw new InvalidOperationException(Resources.errHeaderInvalid);

		/// <summary>Read bytes from image</summary>
		/// <param name="offset">offset from beggining of the file</param>
		/// <param name="length">How much to read</param>
		/// <returns>Readed bytes</returns>
		public Byte[] ReadBytes(UInt64 offset, UInt64 length)
			=> this.Loader.ReadBytes(checked((UInt32)offset), checked((UInt32)length));

		/// <summary>Reads integer</summary>
		/// <param name="offset"></param>
		/// <returns></returns>
		public UInt64 ReadInt(ref UInt64 offset)
		{
			UInt64 result;
			if(this.Is64Bit)
			{
				result = this.PtrToStructure<UInt64>(offset);
				offset += this.SizeOfInt;
			} else
			{
				result = this.PtrToStructure<UInt32>(offset);
				offset += this.SizeOfInt;
			}

			return result;
		}

		/// <summary>Get structure from specific RVA</summary>
		/// <typeparam name="T">Structure to map</typeparam>
		/// <param name="offset">RVA to the beggining of structure</param>
		/// <returns>Mapped structure</returns>
		public T PtrToStructure<T>(UInt64 offset) where T : struct
			=> this.Loader.PtrToStructure<T>(checked((UInt32)offset));

		/// <summary>Get string from specific RVA</summary>
		/// <param name="offset">RVA to the beggining of string</param>
		/// <returns>Mapped string</returns>
		public String PtrToStringAnsi(UInt64 offset)
			=> this.Loader.PtrToStringAnsi(checked((UInt32)offset));

		/// <summary>dispose data reader and all managed resources</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Dispose managed objects</summary>
		/// <param name="disposing">Dispose managed objects</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if(disposing && this.Loader != null)
			{
				this.Loader.Dispose();
				this.Loader = null;
			}
		}
	}
}