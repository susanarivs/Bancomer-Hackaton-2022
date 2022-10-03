using System.Reflection;
using System.Runtime.CompilerServices;
using WhiteLabel.Core;
using Xamarin.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Xamarin.Forms.Dependency(typeof(MirrorService))]
[assembly: AssemblyDescription("Core support library.")]
[assembly: AssemblyCopyright("Altergo - 2020")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.Droid")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.iOS")]
[assembly: InternalsVisibleTo("WhiteLabel.Core.UWP")]
[assembly: XmlnsDefinition("http://binariatechnologies.com/grial", "WhiteLabel.Core")]
