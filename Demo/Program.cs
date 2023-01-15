using System;
using System.IO;
using System.Linq;
using AlphaOmega.Debug;

namespace Demo
{
	class Program
	{
		static void Main(String[] args)
		{
			foreach(String so in Directory.GetFiles(@"C:\Visual Studio Projects\C#\Shared.Classes\AlphaOmega.Debug\FileReader\", "*.so", SearchOption.AllDirectories))
				ReadSo(so);
		}

		static void ReadSo(String binFile)
		{
			using(ElfFile file = new ElfFile(StreamLoader.FromFile(binFile)))
			{
				Utils.ConsoleWriteMembers(file.Header.Identification);
				if(file.Header.IsValid)
				{
					DebugStringSection debugSection = file.GetDebugStringSection();
					if(debugSection != null)
						foreach(var symbolSec in debugSection)
							Utils.ConsoleWriteMembers(symbolSec);

					foreach(var noteSec in file.GetNotesSections())
						foreach(var note in noteSec)
							Utils.ConsoleWriteMembers(note);

					Utils.ConsoleWriteMembers(file.Header.Header);

					foreach(var strSec in file.GetStringSections())
						foreach(var str in strSec)
							Utils.ConsoleWriteMembers(str);

					foreach(var symbolSec in file.GetSymbolSections())
						foreach(var symbol in symbolSec)
							Utils.ConsoleWriteMembers(symbol);

					foreach(var relSec in file.GetRelocationSections())
						foreach(var rel in relSec)
							Utils.ConsoleWriteMembers(rel);

					foreach(var relaSec in file.GetRelocationASections())
						foreach(var rela in relaSec)//TODO: Check it
							Utils.ConsoleWriteMembers(rela);

					SymbolSection symbols = file.GetSymbolSections().FirstOrDefault();
					if(symbols != null)
					{
						foreach(var item in symbols)
						{
							if(String.IsNullOrEmpty(item.Name))
								Console.WriteLine("EMPTY REF");
						}
						/*StringSection strings = file.GetStringSections().FirstOrDefault(p => p.Section.Name == ".dynstr");
						foreach(var item in symbols)
						{
							String str = strings[item.st_name];
							Console.WriteLine(str);
							if(String.IsNullOrEmpty(str))
								Console.WriteLine("EMPTY REF");
							if(item.st_shndx != 0)
							{
								Section someSec = file.Sections.First(p => p.Index == item.st_shndx);
								Utils.ConsoleWriteMembers(someSec);
							}
						}*/
					}

					String[] secTypes = file.Sections.Select(p => p.sh_type.ToString()).Distinct().ToArray();
				}
			}
		}
	}
}
