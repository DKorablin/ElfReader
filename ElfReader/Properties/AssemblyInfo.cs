using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("32beaa22-a8fe-4671-8294-4bb1b9cbb1ca")]

[assembly: System.CLSCompliant(false)]

#if NETSTANDARD
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/DKorablin/ElfReader")]
#else
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2024")]
[assembly: AssemblyProduct("Executable and Linkable Format Reader")]
[assembly: AssemblyTitle("ELF Reader")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
#endif