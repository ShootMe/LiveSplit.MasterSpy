using LiveSplit.MasterSpy;
using LiveSplit.UI.Components;
using System.Reflection;
using System.Runtime.InteropServices;
[assembly: AssemblyTitle("LiveSplit.MasterSpy")]
[assembly: AssemblyDescription("Autosplitter for Master Spy")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("LiveSplit.MasterSpy")]
[assembly: AssemblyCopyright("Copyright © 2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("b3294e28-2bd4-4e39-92fa-e04a620c7e11")]
[assembly: AssemblyVersion("1.0.1.0")]
[assembly: AssemblyFileVersion("1.0.1.0")]
#if !DEBUG
[assembly: ComponentFactory(typeof(SplitterFactory))]
#endif