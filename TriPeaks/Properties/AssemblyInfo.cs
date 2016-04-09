using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: AssemblyTitle("TriPeaks")]
[assembly: AssemblyDescription("A clone of the 1991 card game.")]
[assembly: AssemblyCompany("dfragment.net")]
[assembly: AssemblyProduct("TriPeaks")]
[assembly: AssemblyCopyright("© dfragment.net 2015-2016")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("TriPeaks.Test")]

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
