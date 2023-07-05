using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("32beaa22-a8fe-4671-8294-4bb1b9cbb1ca")]

[assembly: System.CLSCompliant(false)]

#if !NETSTANDARD
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016")]
[assembly: AssemblyProduct("Executable and Linkable Format Reader")]
[assembly: AssemblyTitle("ELF Reader")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
#endif